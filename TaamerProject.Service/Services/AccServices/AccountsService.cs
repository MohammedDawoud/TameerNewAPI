using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class AccountsService : IAccountsService
    {

        private readonly IAccountsRepository _accountsRepository;
        private readonly ICustomerRepository _CustomersRepository;
        private readonly IFiscalyearsRepository _fiscalyearsRepository;
        private readonly IEmployeesRepository _EmployeesRepository;
        private readonly IVoucherSettingsRepository _VoucherSettingsRepository;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly ITransactionsRepository _TransactionsRepository;
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;
        private readonly IJournalsRepository _JournalsRepository;
        private readonly IInvoicesRepository _InvoicesRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;


        public AccountsService(IAccountsRepository accountsRepository, ICustomerRepository customerRepository, IFiscalyearsRepository fiscalyearsRepository
            ,IEmployeesRepository employeesRepository, IVoucherSettingsRepository voucherSettingsRepository, IBranchesRepository branchesRepository,
            ITransactionsRepository transactionsRepository, ISys_SystemActionsRepository sys_SystemActionsRepository, IJournalsRepository journalsRepository,
            IInvoicesRepository invoicesRepository, TaamerProjectContext dataContext
            , ISystemAction systemAction)
        {
            _accountsRepository = accountsRepository;
            _CustomersRepository = customerRepository;
            _fiscalyearsRepository = fiscalyearsRepository;
            _EmployeesRepository = employeesRepository;
            _VoucherSettingsRepository = voucherSettingsRepository;
            _BranchesRepository = branchesRepository;
            _TransactionsRepository = transactionsRepository;
            _Sys_SystemActionsRepository = sys_SystemActionsRepository;
            _JournalsRepository = journalsRepository;
            _InvoicesRepository = invoicesRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;

        }

        public async Task<List<AccountVM>> GetAllAccounts(string SearchText, string lang, int BranchId)
        {
            var accs = await _accountsRepository.GetAllAccounts(SearchText, lang, BranchId);

            return accs;
        }
        public async Task<List<AccountVM>> GetAllAccountsCustomerBranch(string SearchText, string lang, int BranchId)
        {
            var MainCustomerAccount = _TaamerProContext.Branch.Where(x=>x.BranchId==BranchId).FirstOrDefault().CustomersAccId;
            if (MainCustomerAccount != null)
            {
                var accs = await _accountsRepository.GetAllAccountsCustomerBranch(SearchText, lang, BranchId, MainCustomerAccount ?? 0);
                return accs;
            }
            else
            {
                List<AccountVM> lmd = new List<AccountVM>();
                return lmd;
            }

        }
        public async Task<IEnumerable<AccountVM>> GetAllAccountsOpening(string SearchText, string lang, int BranchId)
        {
            var accs = await _accountsRepository.GetAllAccountsOpening(SearchText, lang, BranchId);
            return accs;
        }
        public async Task<IEnumerable<AccountVM>> GetAllAccounts2(string SearchText, string lang, int BranchId)
        {
            var accs =await _accountsRepository.GetAllAccounts2(SearchText, lang, BranchId);
            return accs;
        }


        public async Task<IEnumerable<AccountVM>> GetAllHirearchialAccounts(int BranchId, string lang)
        {
            var accs = await _accountsRepository.GetAllHirearchialAccounts(BranchId, lang);
            return accs;
        }
        public async Task<IEnumerable<AccountVM>> GetAllSubAccounts(string SearchText, string lang, int BranchId)
        {
            var subAccs = await _accountsRepository.GetAllSubAccounts(SearchText, lang, BranchId);
            return subAccs;
        }

        public string GetAccountCodeNewValue(int parentid, int accountid)
        {
            try
            {
                var AccountCodeNew = "";
                var CodeNewList = new List<decimal>();
                var Accounts = _TaamerProContext.Accounts.Where(s => s.ParentId == parentid && s.IsDeleted == false).Where(a => a.AccountId != accountid);
                if (Accounts.Count() > 0)
                {
                    foreach (var v in Accounts)
                    {
                        if (v.AccountCodeNew != "")
                        {
                            decimal ValueDec = Convert.ToDecimal(v.AccountCodeNew);
                            CodeNewList.Add(ValueDec);
                        }

                    }
                    var getmax = CodeNewList.Max();
                    AccountCodeNew = (getmax + 1).ToString();

                }
                else
                {
                    var Accounts2 = _TaamerProContext.Accounts.Where(x=>x.AccountId==parentid).FirstOrDefault();
                    if (Accounts2 != null)
                    {
                        if (((Accounts2.Level ?? 0) + 1 == 1) || ((Accounts2.Level ?? 0) + 1 == 2))
                        {
                            AccountCodeNew = Accounts2.AccountCodeNew + "01";
                        }
                        else
                        {
                            AccountCodeNew = Accounts2.AccountCodeNew + "0001";

                        }
                    }
                    else
                    {
                        AccountCodeNew = "";
                    }

                }
                return AccountCodeNew;
            }
            catch (Exception ex)
            {

                return "";
            }


        }

        public GeneralMessage SaveAccount(Accounts account, int UserId, int BranchId, int yearid)
        {
            try
            {





                var codeExist = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.AccountId != account.AccountId && s.Code.Trim() == account.Code.Trim() && s.BranchId == BranchId).FirstOrDefault();
                if (codeExist != null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ الحساب";
                   _SystemAction.SaveAction("SaveAccount", "AccountsService", 1, Resources.TheCodeAlreadyExists, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.TheCodeAlreadyExists };

                }
                if (account.AccountId == 0)
                {



                    var Branch = _TaamerProContext.Branch.Where(x => x.BranchId == BranchId).FirstOrDefault();
                    if (Branch != null && Branch.CustomersAccId != null)
                    {
                        if (account.ParentId == Branch.CustomersAccId)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote3 = "فشل في حفظ الحساب";
                            _SystemAction.SaveAction("SaveAccount", "AccountsService", 1, "لا يمكنك الحفظ في حساب العملاء .. يجب عليك حفظ عميل", "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكنك الحفظ في حساب العملاء .. يجب عليك حفظ عميل" };

                        }

                    }
                    if (Branch != null && Branch.SuppliersAccId != null)
                    {
                        if (account.ParentId == Branch.SuppliersAccId)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote3 = "فشل في حفظ الحساب";
                            _SystemAction.SaveAction("SaveAccount", "AccountsService", 1, "لا يمكنك الحفظ في حساب الموردين .. يجب عليك حفظ مورد", "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكنك الحفظ في حساب الموردين .. يجب عليك حفظ مورد" };
                        }
                    }

                    if (Branch != null && Branch.EmployeesAccId != null)
                    {
                        if (account.ParentId == Branch.EmployeesAccId)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote3 = "فشل في حفظ الحساب";
                            _SystemAction.SaveAction("SaveAccount", "AccountsService", 1, "لا يمكنك الحفظ في حساب الموظفين .. يجب عليك حفظ موظف", "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكنك الحفظ في حساب الموظفين .. يجب عليك حفظ موظف" };

                        }

                    }


                    if (Branch != null && Branch.LoanAccId != null)
                    {
                        if (account.ParentId == Branch.LoanAccId)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote3 = "فشل في حفظ الحساب";
                            _SystemAction.SaveAction("SaveAccount", "AccountsService", 1, "لا يمكنك الحفظ في حساب سلف العاملين .. يجب عليك حفظ موظف", "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكنك الحفظ في حساب سلف العاملين .. يجب عليك حفظ موظف" };

                        }

                    }


                    if (Branch != null && Branch.PurchaseDelayAccId != null)
                    {
                        if (account.ParentId == Branch.PurchaseDelayAccId)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote3 = "فشل في حفظ الحساب";
                            _SystemAction.SaveAction("SaveAccount", "AccountsService", 1, "لا يمكنك الحفظ في حساب عهد العاملين .. يجب عليك حفظ موظف", "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكنك الحفظ في حساب عهد العاملين .. يجب عليك حفظ موظف" };

                        }

                    }


                    account.IsMain = false;
                    account.AddUser = UserId;
                    account.AddDate = DateTime.Now;
                    account.IsDeleted = false;
                    account.BranchId = BranchId;

                    var ValNewAccount = GetAccountCodeNewValue(account.ParentId ?? 0, 0);
                    account.AccountCodeNew = ValNewAccount;

                    _TaamerProContext.Accounts.Add(account);
                    _TaamerProContext.SaveChanges();

                    //update main acc
                    if (account.ParentId != null)
                    {
                        var ParentAccount = _TaamerProContext.Accounts.Where(x=>x.AccountId==account.ParentId).FirstOrDefault();
                        ParentAccount.IsMain = true;
                    }
                    List<Transactions> trans = new List<Transactions>();
                    if ((account.OpenAccCredit != 0 && account.OpenAccCredit != null) || account.OpenAccDepit != 0 && account.OpenAccDepit != null)
                    {
                        var AccCreditDate_M = "";var AccDepitDate_M = ""; var AccCreditDate_H = ""; var AccDepitDate_H = "";
                        if (account.OpenAccCreditDate!=null)
                        {
                            DateTime myDateCredit = DateTime.Parse(account.OpenAccCreditDate);
                            AccCreditDate_M = account.OpenAccCreditDate;
                            AccCreditDate_H = myDateCredit.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                        }
                        else
                        {
                            AccCreditDate_M = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            AccCreditDate_H = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                        }
                        if (account.OpenAccDepitDate != null)
                        {
                            DateTime myDateDepit = DateTime.Parse(account.OpenAccDepitDate);
                            AccDepitDate_M = account.OpenAccDepitDate;
                            AccDepitDate_H = myDateDepit.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                        }
                        else
                        {
                            AccDepitDate_M = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            AccDepitDate_H = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                        }

                        var CostId = 0;
                        try
                        {
                            CostId = _TaamerProContext.CostCenters.Where(w => w.BranchId == BranchId && w.IsDeleted == false && w.ProjId == 0).Select(s => s.CostCenterId).FirstOrDefault();
                        }
                        catch (Exception)
                        {
                            CostId = 1;
                        }

                        Invoices newinvoice = new Invoices();
                        newinvoice.InvoiceNumber = _InvoicesRepository.GenerateNextInvoiceNumber(31, yearid, BranchId).Result.ToString();
                        newinvoice.Type = 31;
                        newinvoice.Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        newinvoice.HijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                        newinvoice.IsPost = true;
                        newinvoice.PostDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        newinvoice.PostHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                        if (account.OpenAccDepit != 0 && account.OpenAccDepit != null)
                        {
                            newinvoice.InvoiceValue = account.OpenAccDepit;
                            newinvoice.TotalValue = account.OpenAccDepit;

                            newinvoice.InvoiceValueText = ConvertNumToString(account.OpenAccDepit.ToString());
                        }
                        if (account.OpenAccCredit != 0 && account.OpenAccCredit != null)
                        {
                            newinvoice.InvoiceValue = account.OpenAccCredit;
                            newinvoice.TotalValue = account.OpenAccCredit;

                            newinvoice.InvoiceValueText = ConvertNumToString(account.OpenAccCredit.ToString());
                        }
                        newinvoice.BranchId = 1;
                        newinvoice.YearId = yearid;
                        newinvoice.AddDate = DateTime.Now;
                        newinvoice.AddUser = UserId;
                        newinvoice.printBankAccount = false;
                        newinvoice.DunCalc = false;
                        newinvoice.ToAccountId = account.AccountId;
                        _TaamerProContext.Invoices.Add(newinvoice);
                        _TaamerProContext.SaveChanges();


                        if (account.OpenAccDepit != 0 && account.OpenAccDepit != null)
                        {
                            //depit 
                            trans.Add(new Transactions
                            {
                                AccTransactionDate = AccDepitDate_M,
                                AccTransactionHijriDate = AccDepitDate_H,
                                TransactionDate = AccDepitDate_M,
                                TransactionHijriDate = AccDepitDate_H,
                                AccountId = account.AccountId,
                                CostCenterId = CostId,
                                AccountType = account.Type,
                                Type = 31,
                                LineNumber = 1,
                                Depit = account.OpenAccDepit,
                                Credit = 0,
                                YearId = yearid,
                                Notes = "رصيد افتتاحي",
                                Details = "رصيد افتتاحي",
                                //InvoiceReference = voucher.InvoiceNumber.ToString(),
                                InvoiceReference = "",

                                IsPost = true,
                                BranchId = BranchId,
                                AddDate = DateTime.Now,
                                AddUser = UserId,
                                IsDeleted = false,
                            });

                        }
                        if (account.OpenAccCredit != 0 && account.OpenAccCredit != null)
                        {
                            //Credit
                            trans.Add(new Transactions
                            {
                                AccTransactionDate = AccCreditDate_M,
                                AccTransactionHijriDate = AccCreditDate_H,
                                TransactionDate = AccCreditDate_M,
                                TransactionHijriDate = AccCreditDate_H,
                                AccountId = account.AccountId,
                                CostCenterId = CostId,
                                AccountType = account.Type,
                                Type = 31,
                                LineNumber = 2,
                                Depit = 0,
                                Credit = account.OpenAccCredit,
                                YearId = yearid,
                                Notes = "رصيد حساب افتتاحي",
                                Details = "رصيد حساب افتتاحي",
                                //InvoiceReference = voucher.InvoiceNumber.ToString(),
                                InvoiceReference = "",
                                IsPost = true,
                                BranchId = BranchId,
                                AddDate = DateTime.Now,
                                AddUser = UserId,
                                IsDeleted = false,
                            });
                        }
                        if ((account.OpenAccCredit != 0 && account.OpenAccCredit != null) || account.OpenAccDepit != 0 && account.OpenAccDepit != null)
                        {

                            var newJournal = new Journals();
                            var JNo = _JournalsRepository.GenerateNextJournalNumber(yearid, BranchId).Result;

                            JNo = (newJournal.VoucherType == 10 && JNo == 1) ? JNo : (newJournal.VoucherType != 10 && JNo == 1) ? JNo + 1 : JNo;



                            newJournal.JournalNo = JNo;
                            //newJournal.JournalNo = _JournalsRepository.GenerateNextJournalNumber();
                            newJournal.YearMalia = yearid;
                            newJournal.VoucherType = 31;
                            newJournal.VoucherId = newinvoice.InvoiceId;
                            newJournal.BranchId = BranchId;
                            newJournal.AddDate = DateTime.Now;
                            newJournal.AddUser = newJournal.UserId = UserId;
                            _TaamerProContext.Journals.Add(newJournal);

                            foreach (var tr in trans.ToList())
                            {
                                tr.IsPost = true;
                                tr.JournalNo = newJournal.JournalNo;
                                tr.InvoiceId = newinvoice.InvoiceId;
                            }


                        }



                        _TaamerProContext.Transactions.AddRange(trans);
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة حساب جديد";
                   _SystemAction.SaveAction("SaveAccount", "AccountsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };


                }
                else
                {
                    var accountUpdated = _TaamerProContext.Accounts.Where(x=>x.AccountId==account.AccountId).FirstOrDefault();
                    if (accountUpdated != null)
                    {
                        if (accountUpdated.Nature != account.Nature)
                        {
                            var ParentAccount_P = _TaamerProContext.Accounts.Where(x => x.AccountId == account.ParentId).FirstOrDefault();
                            if (ParentAccount_P != null)
                            {
                                if (ParentAccount_P.Nature != account.Nature)
                                {
                                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =Resources.Verify_the_nature_of_the_parent_account };

                                }

                            }


                            var ParentAccount_C = _TaamerProContext.Accounts.Where(s => s.ParentId == account.AccountId);
                            if (ParentAccount_C.Count() != 0)
                            {
                                foreach (var child in ParentAccount_C)
                                {
                                    var accountUpdated_C = _TaamerProContext.Accounts.Where(x => x.AccountId == child.AccountId).FirstOrDefault();
                                    accountUpdated_C.Nature = account.Nature;
                                    //_TaamerProContext.SaveChanges();



                                    var ParentAccount_C2 = _TaamerProContext.Accounts.Where(s => s.ParentId == child.AccountId);
                                    if (ParentAccount_C2.Count() != 0)
                                    {
                                        foreach (var child2 in ParentAccount_C2)
                                        {
                                            var accountUpdated_C2 = _TaamerProContext.Accounts.Where(x => x.AccountId == child2.AccountId).FirstOrDefault();
                                            accountUpdated_C2.Nature = account.Nature;
                                            //_TaamerProContext.SaveChanges();



                                            var ParentAccount_C3 = _TaamerProContext.Accounts.Where(s => s.ParentId == child2.AccountId);
                                            if (ParentAccount_C3.Count() != 0)
                                            {
                                                foreach (var child3 in ParentAccount_C3)
                                                {
                                                    var accountUpdated_C3 = _TaamerProContext.Accounts.Where(x=>x.AccountId==child3.AccountId).FirstOrDefault();
                                                    accountUpdated_C3.Nature = account.Nature;
                                                    //_TaamerProContext.SaveChanges();



                                                    var ParentAccount_C4 = _TaamerProContext.Accounts.Where(s => s.ParentId == child3.AccountId);
                                                    if (ParentAccount_C4.Count() != 0)
                                                    {
                                                        foreach (var child4 in ParentAccount_C4)
                                                        {
                                                            var accountUpdated_C4 = _TaamerProContext.Accounts.Where(x=>x.AccountId==child4.AccountId).FirstOrDefault();
                                                            accountUpdated_C4.Nature = account.Nature;
                                                            //_TaamerProContext.SaveChanges();
                                                        }
                                                    }

                                                }
                                            }

                                        }
                                    }


                                }
                            }
                        }


                        if (accountUpdated.Classification != account.Classification)
                        {
                            var ParentAccount_PClass = _TaamerProContext.Accounts.Where(x=>x.AccountId==account.ParentId).FirstOrDefault();
                            if (ParentAccount_PClass != null)
                            {
                                if (ParentAccount_PClass.Classification != account.Classification)
                                {
                                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.classifyTheFathersAccountForThisAccount };

                                    }

                            }


                            var ParentAccount_CClass = _TaamerProContext.Accounts.Where(s => s.ParentId == account.AccountId);
                            if (ParentAccount_CClass.Count() != 0)
                            {
                                foreach (var childClass in ParentAccount_CClass)
                                {
                                    var accountUpdated_CClass = _TaamerProContext.Accounts.Where(x=>x.AccountId==childClass.AccountId).FirstOrDefault();
                                    accountUpdated_CClass.Classification = account.Classification;
                                    //_TaamerProContext.SaveChanges();



                                    var ParentAccount_CClass2 = _TaamerProContext.Accounts.Where(s => s.ParentId == childClass.AccountId);
                                    if (ParentAccount_CClass2.Count() != 0)
                                    {
                                        foreach (var childClass2 in ParentAccount_CClass2)
                                        {
                                            var accountUpdated_CClass2 = _TaamerProContext.Accounts.Where(x=>x.AccountId==childClass2.AccountId).FirstOrDefault();
                                            accountUpdated_CClass2.Classification = account.Classification;
                                            //_TaamerProContext.SaveChanges();



                                            var ParentAccount_CClass3 = _TaamerProContext.Accounts.Where(s => s.ParentId == childClass2.AccountId);
                                            if (ParentAccount_CClass3.Count() != 0)
                                            {
                                                foreach (var childClass3 in ParentAccount_CClass3)
                                                {
                                                    var accountUpdated_CClass3 = _TaamerProContext.Accounts.Where(x=>x.AccountId==childClass3.AccountId).FirstOrDefault();
                                                    accountUpdated_CClass3.Classification = account.Classification;
                                                    //_TaamerProContext.SaveChanges();



                                                    var ParentAccount_CClass4 = _TaamerProContext.Accounts.Where(s => s.ParentId == childClass3.AccountId);
                                                    if (ParentAccount_CClass4.Count() != 0)
                                                    {
                                                        foreach (var childClass4 in ParentAccount_CClass4)
                                                        {
                                                            var accountUpdated_CClass4 = _TaamerProContext.Accounts.Where(x=>x.AccountId==childClass4.AccountId).FirstOrDefault();
                                                            accountUpdated_CClass4.Classification = account.Classification;
                                                            //_TaamerProContext.SaveChanges();
                                                        }
                                                    }

                                                }
                                            }

                                        }
                                    }


                                }
                            }
                        }





                        if (account.AccountId == 41 || account.AccountId == 106)
                        {

                            accountUpdated.NameAr = account.NameAr;
                            accountUpdated.NameEn = account.NameEn;
                            accountUpdated.Code = account.Code;
                            accountUpdated.Classification = account.Classification;
                            accountUpdated.CurrencyId = account.CurrencyId;
                            accountUpdated.Halala = account.Halala;
                            accountUpdated.Notes = account.Notes;
                            accountUpdated.Type = account.Type;
                            accountUpdated.AccountIdAhlak = account.AccountIdAhlak;
                            accountUpdated.Nature = account.Nature;
                        }
                        else
                        {
                            accountUpdated.NameAr = account.NameAr;
                            accountUpdated.NameEn = account.NameEn;
                            accountUpdated.Code = account.Code;
                            accountUpdated.Classification = account.Classification;
                            accountUpdated.CurrencyId = account.CurrencyId;
                            accountUpdated.Halala = account.Halala;
                            accountUpdated.Notes = account.Notes;
                            accountUpdated.Type = account.Type;

                            accountUpdated.AccountIdAhlak = account.AccountIdAhlak;
                            accountUpdated.Nature = account.Nature;
                            accountUpdated.OpenAccCreditDate = account.OpenAccCreditDate;
                            accountUpdated.OpenAccDepitDate = account.OpenAccDepitDate;


                            var ParentAccount_VV =  _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.ParentId == account.AccountId);
                            if (ParentAccount_VV.Count() == 0)
                            {
                                accountUpdated.IsMain = false;
                            }
                            else
                            {
                                accountUpdated.IsMain = true;
                            }

                        }


                        var ValNewAccount = GetAccountCodeNewValue(account.ParentId ?? 0, account.AccountId);
                        accountUpdated.AccountCodeNew = ValNewAccount;


                        accountUpdated.Active = account.Active;
                        if (accountUpdated.ParentId != account.ParentId && account.ParentId != null) // تعديل حساب فرعي ليكون فرعي لحساب اخر
                        {
                            accountUpdated.Level = account.Level;
                            accountUpdated.ParentId = account.ParentId;
                            //update main acc
                            var ParentAccount = _TaamerProContext.Accounts.Where(x=>x.AccountId==account.ParentId).FirstOrDefault();
                            ParentAccount.IsMain = true;
                        }
                        if (account.Level == 1 && account.ParentId == null) // تعديل حساب فرعي ليكون رئيسي
                        {
                            accountUpdated.ParentId = null;
                            accountUpdated.IsMain = true;

                        }
                        else
                        {
                            accountUpdated.IsMain = false;
                        }
                        accountUpdated.UpdateUser = UserId;
                        accountUpdated.UpdateDate = DateTime.Now;
                        accountUpdated.OpenAccCredit = account.OpenAccCredit;
                        accountUpdated.OpenAccDepit = account.OpenAccDepit;


                        var transactiondelet = _TaamerProContext.Transactions.Where(x => x.IsDeleted == false && x.Type == 31 && x.AccountId == account.AccountId).FirstOrDefault();
                        if (transactiondelet != null)
                        {
                            var JournalExist = _TaamerProContext.Journals.Where(s => s.IsDeleted == false && s.JournalNo == transactiondelet.JournalNo && s.YearMalia == yearid && s.VoucherType == 31 && s.BranchId == BranchId).FirstOrDefault();
                            if (JournalExist != null)
                            {
                                _TaamerProContext.Journals.Remove(JournalExist);
                            }
                            if (transactiondelet != null)
                            {
                                _TaamerProContext.Transactions.Remove(transactiondelet);
                            }
                        }

                        /////////////////////////////////////////////////////////////////////////تعديل الرصيد الافتتاحي//////////////////////////////////////////////////
                        if ((account.OpenAccCredit != 0 && account.OpenAccCredit != null) || account.OpenAccDepit != 0 && account.OpenAccDepit != null)
                        {

                            var AccCreditDate_M = ""; var AccDepitDate_M = ""; var AccCreditDate_H = ""; var AccDepitDate_H = "";
                            if (account.OpenAccCreditDate != null)
                            {
                                DateTime myDateCredit = DateTime.Parse(account.OpenAccCreditDate);
                                AccCreditDate_M = account.OpenAccCreditDate;
                                AccCreditDate_H = myDateCredit.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                            }
                            else
                            {
                                AccCreditDate_M = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                AccCreditDate_H = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                            }
                            if (account.OpenAccDepitDate != null)
                            {
                                DateTime myDateDepit = DateTime.Parse(account.OpenAccDepitDate);
                                AccDepitDate_M = account.OpenAccDepitDate;
                                AccDepitDate_H = myDateDepit.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                            }
                            else
                            {
                                AccDepitDate_M = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                AccDepitDate_H = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                            }
                            var CostId = 0;
                            try
                            {
                                CostId = _TaamerProContext.CostCenters.Where(w => w.BranchId == BranchId && w.IsDeleted == false && w.ProjId == 0).Select(s => s.CostCenterId).FirstOrDefault();
                            }
                            catch (Exception)
                            {
                                CostId = 1;
                            }

                            //var transactiondelet = _TaamerProContext.Transactions.Where(x => x.IsDeleted == false && x.Type == 31 && x.AccountId == account.AccountId).FirstOrDefault();
                            if (transactiondelet != null)
                            {


                                var invoiceupdate = _TaamerProContext.Invoices.Where(x=>x.InvoiceId==transactiondelet.InvoiceId).FirstOrDefault();

                                List<Transactions> transupdate = new List<Transactions>();

                                if (account.OpenAccDepit != 0 && account.OpenAccDepit != null)
                                {
                                    transupdate.Add(new Transactions
                                    {
                                        AccTransactionDate = AccDepitDate_M,
                                        AccTransactionHijriDate = AccDepitDate_H,
                                        TransactionDate = AccDepitDate_M,
                                        TransactionHijriDate = AccDepitDate_H,
                                        AccountId = account.AccountId,
                                        CostCenterId = CostId,
                                        AccountType = account.Type,
                                        Type = 31,
                                        LineNumber = 1,
                                        Depit = account.OpenAccDepit,
                                        Credit = 0,
                                        YearId = yearid,
                                        Notes = "رصيد افتتاحي",
                                        Details = "رصيد افتتاحي",

                                        //InvoiceReference = voucher.InvoiceNumber.ToString(),
                                        InvoiceReference = "",

                                        IsPost = true,
                                        BranchId = BranchId,
                                        AddDate = DateTime.Now,
                                        AddUser = UserId,
                                        IsDeleted = false,
                                    });
                                }

                                if (account.OpenAccCredit != 0 && account.OpenAccCredit != null)
                                {
                                    //Credit
                                    transupdate.Add(new Transactions
                                    {
                                        AccTransactionDate = AccCreditDate_M,
                                        AccTransactionHijriDate = AccCreditDate_H,
                                        TransactionDate = AccCreditDate_M,
                                        TransactionHijriDate = AccCreditDate_H,
                                        AccountId = account.AccountId,
                                        CostCenterId = CostId,
                                        AccountType = account.Type,
                                        Type = 31,
                                        LineNumber = 2,
                                        Depit = 0,
                                        Credit = account.OpenAccCredit,
                                        YearId = yearid,
                                        Notes = "رصيد حساب افتتاحي",
                                        Details = "رصيد حساب افتتاحي",
                                        //InvoiceReference = voucher.InvoiceNumber.ToString(),
                                        InvoiceReference = "",

                                        IsPost = true,
                                        BranchId = BranchId,
                                        AddDate = DateTime.Now,
                                        AddUser = UserId,
                                        IsDeleted = false,
                                    });
                                }

                                if (account.OpenAccDepit != 0 && account.OpenAccDepit != null)
                                {
                                    invoiceupdate.InvoiceValue = account.OpenAccDepit;
                                    invoiceupdate.TotalValue = account.OpenAccDepit;

                                    invoiceupdate.InvoiceValueText = ConvertNumToString(account.OpenAccDepit.ToString());
                                }
                                else
                                {
                                    if (invoiceupdate != null)
                                    {
                                        invoiceupdate.IsDeleted = true;
                                    }
                                }
                                if (account.OpenAccCredit != 0 && account.OpenAccCredit != null)
                                {
                                    invoiceupdate.InvoiceValue = account.OpenAccCredit;
                                    invoiceupdate.TotalValue = account.OpenAccCredit;

                                    invoiceupdate.InvoiceValueText = ConvertNumToString(account.OpenAccCredit.ToString());
                                }
                                else
                                {
                                    if (invoiceupdate != null)
                                    {
                                        invoiceupdate.IsDeleted = true;
                                    }
                                }
                                if ((account.OpenAccCredit != 0 && account.OpenAccCredit != null) || account.OpenAccDepit != 0 && account.OpenAccDepit != null)
                                {

                                    var newJournal = new Journals();
                                    var JNo = _JournalsRepository.GenerateNextJournalNumber(yearid, BranchId).Result;

                                    JNo = (newJournal.VoucherType == 10 && JNo == 1) ? JNo : (newJournal.VoucherType != 10 && JNo == 1) ? JNo + 1 : JNo;



                                    newJournal.JournalNo = JNo;
                                    //newJournal.JournalNo = _JournalsRepository.GenerateNextJournalNumber();
                                    newJournal.VoucherId = invoiceupdate.InvoiceId;
                                    newJournal.YearMalia = yearid;
                                    newJournal.VoucherType = 31;
                                    newJournal.BranchId = BranchId;
                                    newJournal.AddDate = DateTime.Now;
                                    newJournal.AddUser = newJournal.UserId = UserId;
                                    _TaamerProContext.Journals.Add(newJournal);


                                    foreach (var tr in transupdate.ToList())
                                    {
                                        tr.IsPost = true;
                                        tr.JournalNo = newJournal.JournalNo;
                                        tr.InvoiceId = invoiceupdate.InvoiceId;
                                    }

                                    invoiceupdate.JournalNumber = newJournal.JournalNo;

                                }
                                _TaamerProContext.Transactions.AddRange(transupdate);
                            }
                            ///////////////////////////////////////////////////////havent transactions/////////////////////////////////////////////////
                            else
                            {

                                if ((account.OpenAccCredit != 0 && account.OpenAccCredit != null) || account.OpenAccDepit != 0 && account.OpenAccDepit != null)
                                {
                                    Invoices newinvoice = new Invoices();
                                    newinvoice.InvoiceNumber = _InvoicesRepository.GenerateNextInvoiceNumber(31, yearid, BranchId).Result.ToString();
                                    newinvoice.Type = 31;
                                    newinvoice.Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    newinvoice.HijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                                    newinvoice.IsPost = true;
                                    newinvoice.PostDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    newinvoice.PostHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                                    if (account.OpenAccDepit != 0 && account.OpenAccDepit != null)
                                    {
                                        newinvoice.InvoiceValue = account.OpenAccDepit;
                                        newinvoice.TotalValue = account.OpenAccDepit;

                                        newinvoice.InvoiceValueText = ConvertNumToString(account.OpenAccDepit.ToString());
                                    }
                                    if (account.OpenAccCredit != 0 && account.OpenAccCredit != null)
                                    {
                                        newinvoice.InvoiceValue = account.OpenAccCredit;
                                        newinvoice.TotalValue = account.OpenAccCredit;

                                        newinvoice.InvoiceValueText = ConvertNumToString(account.OpenAccCredit.ToString());
                                    }
                                    newinvoice.BranchId = 1;
                                    newinvoice.YearId = yearid;
                                    newinvoice.AddDate = DateTime.Now;
                                    newinvoice.AddUser = UserId;
                                    newinvoice.printBankAccount = false;
                                    newinvoice.DunCalc = false;
                                    newinvoice.ToAccountId = account.AccountId;
                                    _TaamerProContext.Invoices.Add(newinvoice);
                                    _TaamerProContext.SaveChanges();


                                    List<Transactions> newtrans = new List<Transactions>();

                                    if (account.OpenAccDepit != 0 && account.OpenAccDepit != null)
                                    {
                                        //depit 
                                        newtrans.Add(new Transactions
                                        {
                                            AccTransactionDate = AccDepitDate_M,
                                            AccTransactionHijriDate = AccDepitDate_H,
                                            TransactionDate = AccDepitDate_M,
                                            TransactionHijriDate = AccDepitDate_H,
                                            AccountId = account.AccountId,
                                            CostCenterId = CostId,
                                            AccountType = account.Type,
                                            Type = 31,
                                            LineNumber = 1,
                                            Depit = account.OpenAccDepit,
                                            Credit = 0,
                                            YearId = yearid,
                                            Notes = "رصيد افتتاحي",
                                            Details = "رصيد افتتاحي",

                                            //InvoiceReference = voucher.InvoiceNumber.ToString(),
                                            InvoiceReference = "",

                                            IsPost = true,
                                            BranchId = BranchId,
                                            AddDate = DateTime.Now,
                                            AddUser = UserId,
                                            IsDeleted = false,
                                        });

                                    }
                                    if (account.OpenAccCredit != 0 && account.OpenAccCredit != null)
                                    {
                                        //Credit
                                        newtrans.Add(new Transactions
                                        {
                                            AccTransactionDate = AccCreditDate_M,
                                            AccTransactionHijriDate = AccCreditDate_H,
                                            TransactionDate = AccCreditDate_M,
                                            TransactionHijriDate = AccCreditDate_H,
                                            AccountId = account.AccountId,
                                            CostCenterId = CostId,
                                            AccountType = account.Type,
                                            Type = 31,
                                            LineNumber = 2,
                                            Depit = 0,
                                            Credit = account.OpenAccCredit,
                                            YearId = yearid,
                                            Notes = "رصيد حساب افتتاحي",
                                            Details = "رصيد حساب افتتاحي",
                                            //InvoiceReference = voucher.InvoiceNumber.ToString(),
                                            InvoiceReference = "",

                                            IsPost = true,
                                            BranchId = BranchId,
                                            AddDate = DateTime.Now,
                                            AddUser = UserId,
                                            IsDeleted = false,
                                        });
                                    }
                                    if ((account.OpenAccCredit != 0 && account.OpenAccCredit != null) || account.OpenAccDepit != 0 && account.OpenAccDepit != null)
                                    {

                                        var newJournal = new Journals();
                                        var JNo = _JournalsRepository.GenerateNextJournalNumber(yearid, BranchId).Result;

                                        JNo = (newJournal.VoucherType == 10 && JNo == 1) ? JNo : (newJournal.VoucherType != 10 && JNo == 1) ? JNo + 1 : JNo;



                                        newJournal.JournalNo = JNo;
                                        //newJournal.JournalNo = _JournalsRepository.GenerateNextJournalNumber();
                                        newJournal.YearMalia = yearid;
                                        newJournal.VoucherId = newinvoice.InvoiceId;
                                        newJournal.VoucherType = 31;
                                        newJournal.BranchId = BranchId;
                                        newJournal.AddDate = DateTime.Now;
                                        newJournal.AddUser = newJournal.UserId = UserId;
                                        _TaamerProContext.Journals.Add(newJournal);
                                        

                                        foreach (var tr in newtrans.ToList())
                                        {
                                            tr.IsPost = true;
                                            tr.JournalNo = newJournal.JournalNo;
                                            tr.InvoiceId = newinvoice.InvoiceId;
                                        }


                                    }



                                    _TaamerProContext.Transactions.AddRange(newtrans);
                                }

                            }
                        }




                    }

                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل حساب رقم " + account.AccountId;
                   _SystemAction.SaveAction("SaveAccount", "AccountsService", 2,Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }

            }
            catch (Exception ex)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الحساب";
                _SystemAction.SaveAction("SaveAccount", "AccountsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public static string ConvertNumToString(string Num)
        {
            Utilities util = new Utilities(Num);
            if (util.GetNumberAr() == " ")
            {
                NumberToText numberToText = new NumberToText();
                return (numberToText.EnglishNumToText(Num) + " ريال فقط ");
            }
            return (util.GetNumberAr());
        }
        public GeneralMessage DeleteAccount(int AccountId, int UserId, int BranchId)
        {
            try
            {
                if (AccountId == 41)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حذف الحساب";
                    _SystemAction.SaveAction("DeleteAccount", "AccountsService", 1, Resources.Sales_account_cannot_be_deleted, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Sales_account_cannot_be_deleted };
                }
                if (AccountId == 106)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote1 = "فشل في حذف الحساب";
                    _SystemAction.SaveAction("DeleteAccount", "AccountsService", 1, Resources.The_general_fund_account_cannot_be_deleted, "", "", ActionDate1, UserId, BranchId, ActionNote1, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.The_general_fund_account_cannot_be_deleted };

                }
                if (AccountId == 27)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote1 = "فشل في حذف الحساب";
                    _SystemAction.SaveAction("DeleteAccount", "AccountsService", 1, "لا يمكن حذف حساب مجمع الأهلاك", "", "", ActionDate1, UserId, BranchId, ActionNote1, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.accrualAccountCannotBeDeleted };

                }
                var AccTrans = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.AccountId == AccountId);
                if (AccTrans != null && AccTrans.Count() > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "فشل في حذف الحساب";
                    _SystemAction.SaveAction("DeleteAccount", "AccountsService", 1, Resources.Financial_Transactions_Message_Error, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Financial_Transactions_Message_Error };

                }

                int Count =  _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.ParentId == AccountId).Count();
                if (Count > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote3 = "فشل في حذف الحساب";
                     _SystemAction.SaveAction("DeleteAccount", "AccountsService", 1, Resources.AccountDeletedError, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.AccountDeletedError };

                }

                var AccountCheck = _TaamerProContext.Accounts.Where(x => x.AccountId == AccountId).FirstOrDefault();
                var Branch = _TaamerProContext.Branch.Where(x=>x.BranchId==BranchId).FirstOrDefault();
                if (Branch != null && Branch.CustomersAccId != null)
                {
                    if (AccountCheck.ParentId == Branch.CustomersAccId)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote3 = "فشل في حذف الحساب";
                         _SystemAction.SaveAction("DeleteAccount", "AccountsService", 1, Resources.deleteTheCustomerFirstItPossibleToDelete, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.deleteTheCustomerFirstItPossibleToDelete };

                    }

                }

                if (Branch != null && Branch.SuppliersAccId != null)
                {
                    //var AccountCheck = _TaamerProContext.Accounts.Where(x => x.AccountId == AccountId).FirstOrDefault();
                    if (AccountCheck.ParentId == Branch.SuppliersAccId)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote3 = "فشل في حذف الحساب";
                        _SystemAction.SaveAction("DeleteAccount", "AccountsService", 1, "لا يمكن حذف حساب من حسابات الموردين , من فضلك قم بمسح المورد أولا", "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حذف حساب من حسابات الموردين , من فضلك قم بمسح المورد أولا" };

                    }

                }

                if (Branch != null && Branch.EmployeesAccId != null)
                {
                    //var AccountCheck = _TaamerProContext.Accounts.Where(x=>x.AccountId==AccountId).FirstOrDefault();
                    if (AccountCheck.ParentId == Branch.EmployeesAccId)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote3 = "فشل في حذف الحساب";
                         _SystemAction.SaveAction("DeleteAccount", "AccountsService", 1, Resources.cannot_delete_employees_accounts, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.cannot_delete_employees_accounts };

                    }

                }


                if (Branch != null && Branch.LoanAccId != null)
                {
                    //var AccountCheck = _TaamerProContext.Accounts.Where(x=>x.AccountId==AccountId).FirstOrDefault();
                    if (AccountCheck.ParentId == Branch.LoanAccId)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote3 = "فشل في حذف الحساب";
                         _SystemAction.SaveAction("DeleteAccount", "AccountsService", 1, Resources.Delete_Emp_First, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Delete_Emp_First};

                    }

                }


                if (Branch != null && Branch.PurchaseDelayAccId != null)
                {
                    //var AccountCheck = _TaamerProContext.Accounts.Where(x=>x.AccountId==AccountId).FirstOrDefault();
                    if (AccountCheck.ParentId == Branch.PurchaseDelayAccId)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote3 = "فشل في حذف الحساب";
                         _SystemAction.SaveAction("DeleteAccount", "AccountsService", 1, Resources.Delete_Emp_First, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Delete_Emp_First };

                    }

                }




                Accounts account = _TaamerProContext.Accounts.Where(x=>x.AccountId==AccountId).FirstOrDefault();
                account.IsDeleted = true;
                account.DeleteDate = DateTime.Now;
                account.DeleteUser = UserId;


                if (account.ParentId != null)
                {

                    int Count2 =  _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.ParentId == account.ParentId).Count();
                    if (Count2 == 0)
                    {
                        var ParentAccount = _TaamerProContext.Accounts.Where(x=>x.AccountId==account.ParentId).FirstOrDefault();
                        ParentAccount.IsMain = false;
                    }
                }

                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote4 = " حذف حساب رقم " + AccountId;
                 _SystemAction.SaveAction("DeleteClause", "Acc_ClausesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate4, UserId, BranchId, ActionNote4, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حذف الحساب";
                 _SystemAction.SaveAction("DeleteAccount", "AccountsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };

            }
        }

        public List<AccountTreeVM> GetAccountTree(string Lang, int BranchId)
        {

            //var accounts2 = _accountsRepository.GetAllAccounts("", Lang, BranchId).Result.OrderBy(s => s.AccountId);
            var accounts = _accountsRepository.GetAllAccounts("", Lang, BranchId).Result.OrderBy(s => s.Code);
            if (accounts != null && accounts.Count() > 0)
            {
                List<AccountTreeVM> treeItems = new List<AccountTreeVM>();
                foreach (var item in accounts)
                {
                    treeItems.Add(new AccountTreeVM(item.AccountId.ToString(), ((item.ParentId == 0 || item.ParentId == null) ? "#" : item.ParentId.ToString()), item.AccountName = item.Code + " - " + item.AccountName));
                }
                return treeItems;
            }
            else
            {
                return new List<AccountTreeVM>();
            }
        }
        public List<AccountTreeVM> GetAccountTreeIncome(string Lang, int BranchId)
        {

            //var accounts2 = _accountsRepository.GetAllAccounts("", Lang, BranchId).Result.OrderBy(s => s.AccountId);
            var accounts = _accountsRepository.GetAccountTreeIncome("", Lang, BranchId).Result.OrderBy(s => s.Code);
            if (accounts != null && accounts.Count() > 0)
            {
                List<AccountTreeVM> treeItems = new List<AccountTreeVM>();
                foreach (var item in accounts)
                {
                    treeItems.Add(new AccountTreeVM(item.AccountId.ToString(), ((item.ParentId == 0 || item.ParentId == null) ? "#" : item.ParentId.ToString()), item.AccountName = item.Code + " - " + item.AccountName));
                }
                return treeItems;
            }
            else
            {
                return new List<AccountTreeVM>();
            }
        }


        public GeneralMessage SaveAccountTree(List<int> Privs, int UserId, int BranchId, string Con)
        {
            try
            {
                var accountUpdatedAll =  _TaamerProContext.Accounts.Where(s => s.IsDeleted == false);
                foreach (var acc in accountUpdatedAll)
                {
                    acc.TransferedAccId = null;
                }
                _TaamerProContext.SaveChanges();

                if (Privs != null)
                {
                    foreach (var item in Privs)
                    {
                        var accountUpdated = _TaamerProContext.Accounts.Where(x=>x.AccountId==item).FirstOrDefault();
                        accountUpdated.TransferedAccId = 1;
                    }
                    _TaamerProContext.SaveChanges();

                }

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "حفظ حسابات تكلفة الايرادات / المصروفات التشغيلية";
                 _SystemAction.SaveAction("SaveAccountTree", "AccountsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ حسابات تكلفة الايرادات / المصروفات التشغيلية";
                 _SystemAction.SaveAction("SaveAccountTree", "AccountsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage SaveAccountTreeEA(List<int> Privs, int UserId, int BranchId, string Con)
        {
            try
            {
                var accountUpdatedAll =  _TaamerProContext.Accounts.Where(s => s.IsDeleted == false);
                foreach (var acc in accountUpdatedAll)
                {
                    acc.ExpensesAccId = null;
                }
                _TaamerProContext.SaveChanges();

                if (Privs != null)
                {
                    foreach (var item in Privs)
                    {
                        var accountUpdated = _TaamerProContext.Accounts.Where(x=>x.AccountId==item).FirstOrDefault();
                        accountUpdated.ExpensesAccId = 1;
                    }
                    _TaamerProContext.SaveChanges();

                }

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "حفظ حسابات المصاريف العمومية والادارية";
                 _SystemAction.SaveAction("SaveAccountTreeEA", "AccountsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ المصاريف العمومية والادارية";
                 _SystemAction.SaveAction("SaveAccountTreeEA", "AccountsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public GeneralMessage SaveAccountTreeotherrev(List<int> Privs, int UserId, int BranchId)
        {
            try
            {
                var accountUpdatedAll =  _TaamerProContext.Accounts.Where(s => s.IsDeleted == false);
                foreach (var acc in accountUpdatedAll)
                {
                    acc.OtherRev = null;
                }
                _TaamerProContext.SaveChanges();

                if (Privs != null)
                {
                    foreach (var item in Privs)
                    {
                        var accountUpdated = _TaamerProContext.Accounts.Where(x=>x.AccountId==item).FirstOrDefault();
                        accountUpdated.OtherRev = 1;
                    }
                    _TaamerProContext.SaveChanges();

                }

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "حفظ حسابات الايرادات الاخري";
                 _SystemAction.SaveAction("SaveAccountTreeEA", "AccountsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الايرادات الاخري";
                 _SystemAction.SaveAction("SaveAccountTreeEA", "AccountsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }




        public GeneralMessage SaveAccountTreePublicRev(List<int> Privs, int UserId, int BranchId)
        {
            try
            {
                var accountUpdatedAll =  _TaamerProContext.Accounts.Where(s => s.IsDeleted == false);
                foreach (var acc in accountUpdatedAll)
                {
                    acc.PublicRev = null;
                }
                _TaamerProContext.SaveChanges();

                if (Privs != null)
                {
                    foreach (var item in Privs)
                    {
                        var accountUpdated = _TaamerProContext.Accounts.Where(x=>x.AccountId==item).FirstOrDefault();
                        accountUpdated.PublicRev = 1;
                    }
                    _TaamerProContext.SaveChanges();

                }

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "حفظ حسابات الايرادات العامة";
                 _SystemAction.SaveAction("SaveAccountTreeEA", "AccountsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الايرادات العامة";
                 _SystemAction.SaveAction("SaveAccountTreeEA", "AccountsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public List<int> GetAccountTreeKD()
        {
            var PrivIds = new List<int>();
            var Privs =  _TaamerProContext.Accounts.Where(s => s.TransferedAccId == 1 && s.IsDeleted == false).OrderBy(o => o.AccountId).ToList();
            if (Privs != null && Privs.Count > 0)
            {
                PrivIds = Privs.Select(s => s.AccountId).ToList();
            }
            return PrivIds;
        }
        public List<int> GetAccountTreeEA()
        {
            var PrivIds = new List<int>();
            var Privs =  _TaamerProContext.Accounts.Where(s => s.ExpensesAccId == 1 && s.IsDeleted == false).OrderBy(o => o.AccountId).ToList();
            if (Privs != null && Privs.Count > 0)
            {
                PrivIds = Privs.Select(s => s.AccountId).ToList();
            }
            return PrivIds;
        }



        public List<int> GetAccountTreepublicrev()
        {
            var PrivIds = new List<int>();
            var Privs =  _TaamerProContext.Accounts.Where(s => s.PublicRev == 1 && s.IsDeleted == false).OrderBy(o => o.AccountId).ToList();
            if (Privs != null && Privs.Count > 0)
            {
                PrivIds = Privs.Select(s => s.AccountId).ToList();
            }
            return PrivIds;
        }

        public List<int> GetAccountTreeotherrev()
        {
            var PrivIds = new List<int>();
            var Privs =  _TaamerProContext.Accounts.Where(s => s.OtherRev == 1 && s.IsDeleted == false).OrderBy(o => o.AccountId).ToList();
            if (Privs != null && Privs.Count > 0)
            {
                PrivIds = Privs.Select(s => s.AccountId).ToList();
            }
            return PrivIds;
        }
        public async Task<AccountVM> GetAccountById(int accountId)
        {
            return await _accountsRepository.GetAccountById(accountId);
        }
        public async Task<AccountVM>GetAccountByClassificationParent(int classification)
        {
            return await _accountsRepository.GetAccountByClassificationParent(classification);
        }
        public async Task<AccountVM>GetCustMainAccByBranchId(int BranchId)
        {
            var BranchAcc = _TaamerProContext.Branch.Where(x=>x.BranchId==BranchId).FirstOrDefault();
            if (BranchAcc.CustomersAccId != null)
            {
                return await _accountsRepository.GetAccountById(BranchAcc.CustomersAccId ?? 0);
            }
            return new AccountVM();
        }
        public async Task<AccountVM>GetAccountByCode(string Code, string Lang, int BranchId)
        {
            return await _accountsRepository.GetAccountByCode(Code, Lang, BranchId);
        }
        public async Task<IEnumerable<object>> FillCustAccountsSelect(string lang, int BranchId, int param)
        {
            var Accounts = _accountsRepository.GetAllAccounts("", lang, BranchId).Result.Where(t => t.IsMain == false && (t.Classification == 1 || t.Classification == 6 || t.Classification == 2)).Select(s => new
            {
                Id = s.AccountId,
                Name = s.Code + " - " + s.AccountName
            });

            if (param == 1)
            {
                // var NakdiAccIdsList = _TaamerProContext.Customer.ToList().Select(s => s.AccountId);;
                Accounts = _accountsRepository.GetAllAccounts("", lang, BranchId).Result.Where(t => t.IsMain == false).Select(s => new
                {
                    Id = s.AccountId,
                    Name = s.Code + " - " + s.AccountName
                });
                return Accounts;
            }
            else if (param == 8)
            {
                var CusAccIdsList =  _TaamerProContext.Customer.ToList().Select(s => s.AccountId);
                Accounts = _accountsRepository.GetAllAccounts("", lang, BranchId).Result.Where(t => t.IsMain == false && CusAccIdsList.Contains(t.AccountId)).Select(s => new
                {
                    Id = s.AccountId,
                    Name = s.Code + " - " + s.AccountName
                });
                return Accounts;
            }

            return Accounts;


        }

        public async Task<IEnumerable<object>> FillCustAccountsSelect2(string lang, int BranchId, int param)
        {

            var Branch = _BranchesRepository.GetById(BranchId);

            var Accounts = _accountsRepository.GetAllAccounts("", lang, BranchId).Result.Where(t => t.IsMain == false).Select(s => new
            {
                Id = s.AccountId,
                Name = s.Code + " - " + s.AccountName
            });

            if (param == 1)
            {
                // var NakdiAccIdsList = _TaamerProContext.Customer.ToList().Select(s => s.AccountId);;
                Accounts = _accountsRepository.GetAllAccounts("", lang, BranchId).Result.Where(t => (t.AccountId == Branch.BoxAccId || t.ParentId == Branch.BoxAccId) && t.IsMain == false).Select(s => new
                {
                    Id = s.AccountId,
                    Name = s.Code + " - " + s.AccountName
                });

                return Accounts;
            }
            else if (param == 10)
            {
                // var NakdiAccIdsList = _TaamerProContext.Customer.ToList().Select(s => s.AccountId);;
                Accounts = _accountsRepository.GetAllAccounts("", lang, BranchId).Result.Where(t => t.AccountId == Branch.PurchaseCashAccId).Select(s => new
                {
                    Id = s.AccountId,
                    Name = s.Code + " - " + s.AccountName
                });
                return Accounts;
            }
            else if (param == 11)
            {
                // var NakdiAccIdsList = _TaamerProContext.Customer.ToList().Select(s => s.AccountId);;
                Accounts = _accountsRepository.GetAllAccounts("", lang, BranchId).Result.Where(t => t.ParentId == Branch.PurchaseOutCashAccId).Select(s => new
                {
                    Id = s.AccountId,
                    Name = s.Code + " - " + s.AccountName
                });
                return Accounts;
            }
            else if (param == 12)
            {
                // var NakdiAccIdsList = _TaamerProContext.Customer.ToList().Select(s => s.AccountId);;
                Accounts = _accountsRepository.GetAllAccounts("", lang, BranchId).Result.Where(t => t.AccountId == Branch.PurchaseOutDelayAccId).Select(s => new
                {
                    Id = s.AccountId,
                    Name = s.Code + " - " + s.AccountName
                });
                return Accounts;
            }
            else if (param == 8)
            {
                var CusAccIdsList = _TaamerProContext.Customer.ToList().Select(s => s.AccountId);
                Accounts = _accountsRepository.GetAllAccounts("", lang, BranchId).Result.Where(t => t.IsMain == false && CusAccIdsList.Contains(t.AccountId)).Select(s => new
                {
                    Id = s.AccountId,
                    Name = s.Code + " - " + s.AccountName
                });
                return Accounts;
            }
            else if (param == 2)
            {
                var CusAccIdsList = _TaamerProContext.Customer.ToList().Select(s => s.AccountId);;
                Accounts = _accountsRepository.GetAllAccounts("", lang, BranchId).Result.Where(t => t.AccountId == Branch.ContractsAccId).Select(s => new
                {
                    Id = s.AccountId,
                    Name = s.Code + " - " + s.AccountName
                });
                return Accounts;
            }
            else if (param == 15)
            {
                Accounts = _accountsRepository.GetAllAccounts("", lang, BranchId).Result.Where(t => t.ParentId == Branch.PurchaseDelayAccId).Select(s => new
                {
                    Id = s.AccountId,
                    Name = s.Code + " - " + s.AccountName
                });
                return Accounts;
            }
            else if (param == 16)
            {
                Accounts = _accountsRepository.GetAllAccounts("", lang, BranchId).Result.Where(t => t.IsMain == false && (t.AccountId == Branch.SaleCostAccId || t.ParentId == Branch.SaleCostAccId)).Select(s => new
                {
                    Id = s.AccountId,
                    Name = s.Code + " - " + s.AccountName
                });
                return Accounts;
            }

            return Accounts;


        }


        public async Task<IEnumerable<object>> FillSubAccountLoad(string lang, int BranchId)
        {
            var Branch = _BranchesRepository.GetById(BranchId);

            var Accounts = _accountsRepository.GetAllAccounts("", lang, BranchId).Result.Where(t => t.IsMain == false).Select(s => new
            {
                Id = s.AccountId,
                Name = s.Code + " - " + s.AccountName,
                AccountId = s.AccountId,
                Code = s.Code,
                AccountName = s.AccountName
            });
            return Accounts;
        }

        public string GetAccCodeFormID(int AccID, string lang, int BranchId)
        {
            var Accounts = _TaamerProContext.Accounts.Where(x=>x.AccountId==AccID).FirstOrDefault().Code;
            return Accounts;
        }
        public async Task<IEnumerable<object>> FillAccountSelect(string Con, string SelectStetment)
        {
            //var Statuses = _ProjectRepository.GetAllArchiveProjectsByDateSearch(DateFrom, DateTo, BranchId);

            SqlConnection con = new SqlConnection(Con);
            SqlDataAdapter da = new SqlDataAdapter(SelectStetment, Con);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);

            DataTable myDataTable = ds.Tables[0];
            con.Close();

            return myDataTable.AsEnumerable().Select(row => new
            {
                Id = int.Parse(row[0].ToString()),
                Name = row[1].ToString()
            });
        }
        public async Task<IEnumerable<object>> FillAccountNewSelect(string Con, string SelectStetment)
        {
            //var Statuses = _ProjectRepository.GetAllArchiveProjectsByDateSearch(DateFrom, DateTo, BranchId);

            SqlConnection con = new SqlConnection(Con);
            SqlDataAdapter da = new SqlDataAdapter(SelectStetment, Con);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);

            DataTable myDataTable = ds.Tables[0];
            con.Close();

            return myDataTable.AsEnumerable().Select(row => new
            {
                Id = int.Parse(row[0].ToString()),
                Name = row[1].ToString(),
                Classification = row[2].ToString(),
            });
        }
        public async Task<IEnumerable<object>> FillYearsSelect(string Con, string SelectStetment)
        {
            //var Statuses = _ProjectRepository.GetAllArchiveProjectsByDateSearch(DateFrom, DateTo, BranchId);

            SqlConnection con = new SqlConnection(Con);
            SqlDataAdapter da = new SqlDataAdapter(SelectStetment, Con);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);

            DataTable myDataTable = ds.Tables[0];
            con.Close();

            return myDataTable.AsEnumerable().Select(row => new
            {
                Id = int.Parse(row[0].ToString()),
                Name = row[1].ToString(),
                YearId = row[2].ToString()
            });
        }
        public async Task<IEnumerable<object>> GetNetValue(string Con, string SelectStetment)
        {
            //var Statuses = _ProjectRepository.GetAllArchiveProjectsByDateSearch(DateFrom, DateTo, BranchId);

            SqlConnection con = new SqlConnection(Con);
            SqlDataAdapter da = new SqlDataAdapter(SelectStetment, Con);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);

            DataTable myDataTable = ds.Tables[0];
            con.Close();

            return myDataTable.AsEnumerable().Select(row => new
            {
                NetValue = row[0].ToString(),
            });
        }

        public async Task<IEnumerable<object>> FillAccountSelectPurchase(string Con, string SelectStetment)
        {
            //var Statuses = _ProjectRepository.GetAllArchiveProjectsByDateSearch(DateFrom, DateTo, BranchId);

            SqlConnection con = new SqlConnection(Con);
            SqlDataAdapter da = new SqlDataAdapter(SelectStetment, Con);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);

            DataTable myDataTable = ds.Tables[0];
            con.Close();

            return myDataTable.AsEnumerable().Select(row => new
            {
                Id = int.Parse(row[0].ToString()),
                Name = row[1].ToString(),
                IsMainV = row[2].ToString(),
            });
        }

        public IEnumerable<AccountVM> FillAccountSelect2(int AccountID)
        {
            var Accounts = _accountsRepository.GetAllAccountsWithChild(AccountID).Result.ToList();
            //var Accounts =  _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && (s.AccountId == AccountID || s.ParentId == AccountID));
            return Accounts;
        }
        public async Task<IEnumerable<object>> FillEmpAccountsSelect(string lang, int BranchId)
        {
            var EmpAccIdsList = _TaamerProContext.Employees.ToList().Select(s => s.AccountId);
            var Accounts = _accountsRepository.GetAllAccounts("", lang, BranchId).Result.Where(t => t.IsMain == false && t.Classification == 7 && !EmpAccIdsList.Contains(t.AccountId)).Select(s => new
            {
                Id = s.AccountId,
                Name = s.Code + " - " + s.AccountName
            });
            return Accounts;
        }
        public GeneralMessage TransFerAccounts(int FromBranchId, int ToBranchId, int UserId)
        {
            try
            {
                var TransferBranchAccsExist =  _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == ToBranchId);
                if (TransferBranchAccsExist != null && TransferBranchAccsExist.Count() > 0)
                {
                    _TaamerProContext.Accounts.RemoveRange(TransferBranchAccsExist);
                }
                var TransferBranchAccs =  _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == FromBranchId);
                foreach (var acc in TransferBranchAccs)
                {
                    acc.TransferedAccId = acc.AccountId;
                    acc.BranchId = ToBranchId;
                    acc.IsDeleted = false;
                    acc.AddUser = UserId;
                    acc.AddDate = DateTime.Now;
                }
                _TaamerProContext.Accounts.AddRange(TransferBranchAccs);
                _TaamerProContext.SaveChanges();
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public bool CheckAccTreeExist(int ToBranchId)
        {
            var TransferBranchAccs =  _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == ToBranchId);
            if (TransferBranchAccs != null && TransferBranchAccs.Count() > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<IEnumerable<AccountVM>> GetAllAccountsTransactions(string FromDate, string ToDate, string lang, int BranchId, int? yearid)
        {
            return await _accountsRepository.GetAllAccountsTransactions(FromDate, ToDate, yearid ?? default(int), lang, BranchId);
        }

        public async Task<IEnumerable<AccountVM>> GetAccountsByType(string accountName, string lang)
        {
            return await _accountsRepository.GetAccountsByType(accountName, lang);
        }
        public async Task<IEnumerable<AccountVM>> GetAllAccountsTransactionsByAccType(int AccType, string FromDate, string ToDate, string lang, int BranchId, int? yearid)
        {
            return await _accountsRepository.GetAllAccountsTransactionsByAccType(AccType, FromDate, ToDate, yearid ?? default(int), lang, BranchId);
        }
        public async Task<IEnumerable<AccountVM>> GetAllReceiptExchangeAccounts(string SearchText, string lang, int BranchId, int Type)
        {
            List<int> ReceiptExchangeAccId = _TaamerProContext.VoucherSettings.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type).Select(s => s.AccountId).ToList();
            var accs = await _accountsRepository.GetAllReceiptExchangeAccounts(SearchText, lang, BranchId, ReceiptExchangeAccId);
            return accs;
        }
        public async Task<IEnumerable<AccountVM>> GetAccountSatement(int BranchId, string lang, int AccountId, int CostCenterId, string FromDate, string ToDate, int? yearid)
        {
            return await _accountsRepository.GetAccountSatement(BranchId, lang, yearid ?? default(int), AccountId, CostCenterId, FromDate, ToDate);
        }
        public async Task<IEnumerable<AccountVM>> GetGeneralBudget(int BranchId, string lang, string FromDate, string ToDate, int? yearid)
        {
            return await _accountsRepository.GetGeneralBudget(BranchId, lang, yearid ?? default(int), FromDate, ToDate);
        }
        public async Task<IEnumerable<AccountVM>> GetGeneralLedger(int BranchId, string lang, string FromDate, string ToDate, int? yearid)
        {
            return await _accountsRepository.GetGeneralLedger(BranchId, lang, yearid ?? default(int), FromDate, ToDate);
        }
        public async Task<IEnumerable<AccountVM>> GetGeneralLedgerDGV(int BranchId, string lang, string FromDate, string ToDate, string Con, int? yearid)
        {
            return await _accountsRepository.GetGeneralLedgerDGV(BranchId, lang, yearid ?? default(int), FromDate, ToDate, Con);
        }
        //heba
        public async Task<DataTable> TreeView(string Con)
        {
            return await _accountsRepository.TreeView(Con);
        }
        public async Task<IEnumerable<AccountVM>> GetGeneralBudgetDGV(int BranchId, string lang, string FromDate, string ToDate, string Con, int? yearid)
        {
            return await _accountsRepository.GetGeneralBudgetDGV(BranchId, lang, yearid ?? default(int), FromDate, ToDate, Con);
        }
        public async Task<IEnumerable<AccountVM>> GetCustomerFinancialDetails(int BranchId, string lang, int? yearid)
        {
            var MainCustomerAccount = _BranchesRepository.GetById(BranchId).CustomersAccId;
            if (MainCustomerAccount != null)
            {
                return await _accountsRepository.GetCustomerFinancialDetails(MainCustomerAccount, yearid ?? default(int), BranchId, lang);
            }
            return null;
        }
        public async Task<IEnumerable<AccountVM>> GetCustomerFinancialDetailsByFilter(int? CustomerId, string FromDate, string ToDate, int BranchId, string lang, int? yearid, int ZeroCheck)
        {
            if (CustomerId == null)
            {
                var MainCustomerAccount = _BranchesRepository.GetById(BranchId).CustomersAccId;
                if (MainCustomerAccount != null)
                {
                    return await _accountsRepository.GetCustomerFinancialDetailsByFilter(MainCustomerAccount, FromDate, ToDate, yearid ?? default(int), BranchId, lang, ZeroCheck);
                }
            }
            else
            {
                int? accountId = _TaamerProContext.Customer.Where(x => x.CustomerId == CustomerId).FirstOrDefault().AccountId;
                return await _accountsRepository.GetCustomerFinancialDetailsByFilter(accountId, FromDate, ToDate, yearid ?? default(int), BranchId, lang, ZeroCheck);
            }

            return null;
        }
        public async Task<IEnumerable<AccountVM>> GetCustomerFinancialDetailsNew(int? CustomerId, string FromDate, string ToDate, int Zerocheck, int BranchId, string lang, int? yearid, string Con)
        {
            if (CustomerId == null)
            {
                var MainCustomerAccount = _BranchesRepository.GetById(BranchId).CustomersAccId;
                if (MainCustomerAccount != null)
                {
                    return await _accountsRepository.GetCustomerFinancialDetailsNew(CustomerId, FromDate, ToDate, Zerocheck, yearid ?? default(int), BranchId, lang, Con);
                }
            }
            else
            {
                int? accountId = _TaamerProContext.Customer.Where(x=>x.CustomerId==CustomerId).FirstOrDefault().AccountId;
                return await _accountsRepository.GetCustomerFinancialDetailsNew(accountId, FromDate, ToDate, Zerocheck, yearid ?? default(int), BranchId, lang, Con);
            }
            return null;
        }

        public async Task<IEnumerable<AccountVM>> GetAccsTransByType(int Type, int BranchId, string lang, string FromDate, string ToDate, int? yearid)
        {
            return await _accountsRepository.GetAccsTransByType(Type, BranchId, lang, yearid ?? default(int), FromDate, ToDate);
        }
        public ProfitAndLossesVM GetAccsProfitAndLosses(int BranchId, string lang, string FromDate, string ToDate, int? yearid)
        {
            var ProfitAndLosses = new ProfitAndLossesVM();
            ProfitAndLosses.Trading = _accountsRepository.GetAccsTransByType(4, BranchId, lang, yearid ?? default(int), FromDate, ToDate).Result.ToList();
            ProfitAndLosses.InComeState = _accountsRepository.GetAccsTransByType(3, BranchId, lang, yearid ?? default(int), FromDate, ToDate).Result.ToList();
            ProfitAndLosses.Expenses = new List<AccountVM>();
            return ProfitAndLosses;
        }
        public async Task<IEnumerable<AccountVM>> GetAccountSearchValue(string SearchName, int BranchId)
        {
            return await _accountsRepository.GetAccountSearchValue(SearchName, BranchId);
        }
        public async Task<IEnumerable<AccountVM>> GetAllAccount(int BranchId)
        {
            return await _accountsRepository.GetAllAccount(BranchId);
        }
        public async Task<IEnumerable<AccountVM>> GetAllAccountBySearch(AccountVM Account, int BranchId)
        {
            return await _accountsRepository.GetAllAccountBySearch(Account, BranchId);
        }

        public async Task<IEnumerable<TrainBalanceVM>> GetTrailBalanceDGV(string FromDate, string ToDate, string CCID, string Con)
        {
            return await _accountsRepository.GetTrailBalanceDGV(FromDate, ToDate, CCID, Con);
        }
        public async Task<IEnumerable<TrainBalanceVM>> GetTrailBalanceDGVNew(string FromDate, string ToDate, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck, string AccountCode, string LVL, int FilteringType, string FilteringTypeStr, string AccountIds)
        {
            if (yearid != null)
            {
                return await _accountsRepository.GetTrailBalanceDGVNew(FromDate, ToDate, CCID, yearid ?? default(int), BranchId, lang, Con, ZeroCheck, AccountCode, LVL, FilteringType, FilteringTypeStr, AccountIds);
            }
            return new List<TrainBalanceVM>();

        }
        public async Task<IEnumerable<TrainBalanceVM>> GetGeneralBudgetAMRDGVNew(string FromDate, string ToDate, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck, string AccountCode, string LVL, int FilteringType, string FilteringTypeStr, string AccountIds)
        {
            if (yearid != null)
            {
                return await _accountsRepository.GetGeneralBudgetAMRDGVNew(FromDate, ToDate, CCID, yearid ?? default(int), BranchId, lang, Con, ZeroCheck, AccountCode, LVL, FilteringType, FilteringTypeStr, AccountIds);
            }
            return new List<TrainBalanceVM>();

        }

        public async Task<IEnumerable<TrainBalanceVM>> GetTrailBalanceDGVNew_old(string FromDate, string ToDate, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck, string AccountCode, string LVL)
        {
            if (yearid != null)
            {
                return await _accountsRepository.GetTrailBalanceDGVNew_old(FromDate, ToDate, CCID, yearid ?? default(int), BranchId, lang, Con, ZeroCheck, AccountCode, LVL);
            }
            return new List<TrainBalanceVM>();


        }

        public async Task<IEnumerable<TrainBalanceVM>> GetTrailBalanceDGVNew2_old(string FromDate, string ToDate, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck, string AccountCode, string LVL)
        {
            if (yearid != null)
            {
                return await _accountsRepository.GetTrailBalanceDGVNew2_old(FromDate, ToDate, CCID, yearid ?? default(int), BranchId, lang, Con, ZeroCheck, AccountCode, LVL);
            }
            return new List<TrainBalanceVM>();


        }

        public async Task<IEnumerable<TrainBalanceVM>> GetTrailBalanceDGVNew2(string FromDate, string ToDate, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck, string AccountCode, string LVL, int FilteringType, string FilteringTypeStr, string AccountIds)
        {
            if (yearid != null)
            {
                return await _accountsRepository.GetTrailBalanceDGVNew2(FromDate, ToDate, CCID, yearid ?? default(int), BranchId, lang, Con, ZeroCheck, AccountCode, LVL, FilteringType, FilteringTypeStr, AccountIds);
            }
            return new List<TrainBalanceVM>();
        }
        public async Task<IEnumerable<DetailsMonitorVM>> GetDetailsMonitor(string FromDate, string ToDate, int CCID, int BranchId, string lang, string Con, int? yearid, int FilteringType, string FilteringTypeStr, int AccountId, int Type, int Type2)
        {
            if (yearid != null)
            {
                return await _accountsRepository.GetDetailsMonitor(FromDate, ToDate, CCID, yearid ?? default(int), BranchId, lang, Con, FilteringType, FilteringTypeStr, AccountId, Type, Type2);
            }
            return new List<DetailsMonitorVM>();
        }
        public async Task<IEnumerable<CostCenterExpRevVM>> GetProjectExpRev(string CCID, string Con)
        {
            return await _accountsRepository.GetProjectExpRev(CCID, Con);
        }

        public async Task<IEnumerable<IncomeStatmentVM>> GetIncomeStatmentDGV(string FromDate, string ToDate, string CCID, string Con)
        {
            return await _accountsRepository.GetIncomeStatmentDGV(FromDate, ToDate, CCID, Con);
        }
        public async Task<IEnumerable<IncomeStatmentVM>> GetIncomeStatmentDGVNew(string FromDate, string ToDate, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck, string LVL)
        {
            //var year = _fiscalyearsRepository.GetCurrentYear();

            var Branch = _BranchesRepository.GetById(BranchId);
            if (Branch == null || Branch.TaxsAccId == null)
            {
                return new List<IncomeStatmentVM>();
            }
            else if (Branch == null || Branch.SuspendedFundAccId == null)
            {
                return new List<IncomeStatmentVM>();
            }
            //else if (Branch == null || Branch.PurchaseReturnApprovAccId == null)
            //{
            //    return new List<IncomeStatmentVM>();
            //}
            else if (Branch == null || Branch.PurchaseReturnDiscAccId == null)
            {
                return new List<IncomeStatmentVM>();
            }

            if (yearid != null)
            {
                return await _accountsRepository.GetIncomeStatmentDGVNew(FromDate, ToDate, CCID, yearid ?? default(int), BranchId, lang, Con, ZeroCheck, LVL, Branch.TaxsAccId, Branch.SuspendedFundAccId, Branch.PurchaseReturnApprovAccId, Branch.PurchaseReturnDiscAccId, Branch.ContractsAccId, Branch.PurchaseReturnCashAccId);
            }
            return new List<IncomeStatmentVM>();


        }

        public async Task<IEnumerable<IncomeStatmentVMWithLevels>> GetIncomeStatmentDGVLevels(string FromDate, string ToDate, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck, string LVL, int FilteringType, int FilteringTypeAll, string FilteringTypeStr, string FilteringTypeAllStr, string AccountIds, int PeriodFillterType, int PeriodCounter, int TypeF)
        {

            if (yearid != null)
            {
                return await _accountsRepository.GetIncomeStatmentDGVLevels(FromDate, ToDate, CCID, yearid ?? default(int), BranchId, lang, Con, ZeroCheck, LVL, FilteringType, FilteringTypeAll, FilteringTypeStr, FilteringTypeAllStr, AccountIds, PeriodFillterType, PeriodCounter, TypeF);
            }
            return new List<IncomeStatmentVMWithLevels>();


        }


        public async Task<IEnumerable<DetailsMonitorVM>> GetIncomeStatmentDGVLevelsdetails(int AccountId, int type, int type2, string FromDate, string ToDate, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck, string LVL, int FilteringType, int FilteringTypeAll, string FilteringTypeStr, string FilteringTypeAllStr, string AccountIds, int PeriodFillterType, int PeriodCounter, int TypeF)
        {

            if (yearid != null)
            {
                return await _accountsRepository.GetIncomeStatmentDGVLevelsdetails(AccountId, type, type2, FromDate, ToDate, CCID, yearid ?? default(int), BranchId, lang, Con, ZeroCheck, LVL, FilteringType, FilteringTypeAll, FilteringTypeStr, FilteringTypeAllStr, AccountIds, PeriodFillterType, PeriodCounter, TypeF);
            }
            return new List<DetailsMonitorVM>();


        }

        public async Task<IEnumerable<IncomeStatmentVM>> GetAllIncomeStatmentDGVNew(string FromDate, string ToDate, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck, string LVL)
        {
            if (yearid != null)
            {
                return await _accountsRepository.GetALLIncomeStatmentDGVNew(FromDate, ToDate, CCID, yearid ?? default(int), lang, Con, ZeroCheck, LVL);
            }
            return new List<IncomeStatmentVM>();


        }

        public async Task<IEnumerable<GeneralBudgetVM>> GetGeneralBudgetAMRDGV(string FromDate, string ToDate, string LVL, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck)
        {

            if (yearid != null)
            {
                return await _accountsRepository.GetGeneralBudgetAMRDGV(FromDate, ToDate, LVL, CCID, yearid ?? default(int), BranchId, lang, Con, ZeroCheck);
            }
            return new List<GeneralBudgetVM>();


            //return null;
        }
        //heba
        public async Task<DataTable> GetGeneralBudgetFRENCHDGV(string FromDate, string ToDate, string LVL, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck)
        {
            if (yearid != null)
            {
                return await _accountsRepository.GetGeneralBudgetFRENCHDGV(FromDate, ToDate, LVL, CCID, yearid ?? default(int), BranchId, lang, Con, ZeroCheck);
            }
            return new DataTable();
        }
        public async Task<IEnumerable<GeneralmanagerRevVM>> GetGeneralManagerRevenueAMRDGV(int? ManagerId, string FromDate, string ToDate, int BranchId, string Con, int? yearid)
        {

            var Branch = _BranchesRepository.GetById(BranchId);
            if (Branch == null || Branch.TaxsAccId == null)
            {
                return new List<GeneralmanagerRevVM>();
            }
            else if (Branch == null || Branch.SuspendedFundAccId == null)
            {
                return new List<GeneralmanagerRevVM>();
            }
            //dawoud
            if (yearid != null)
            {
                return await _accountsRepository.GetGeneralManagerRevenueAMRDGV(ManagerId, FromDate, ToDate, BranchId, Con, yearid ?? default(int), Branch.TaxsAccId, Branch.SuspendedFundAccId);
            }
            return new List<GeneralmanagerRevVM>();


            //return null;
        }
        public async Task<IEnumerable<ClosingVouchers>> GetClosingVouchers(int BranchId, string Con, int? yearid)
        {

            if (yearid != null)
            {
                return await _accountsRepository.GetClosingVouchers(BranchId, Con, yearid ?? default(int));
            }
            return new List<ClosingVouchers>();
        }
        public async Task<IEnumerable<CostCenterEX_REVM>> GetCostCenterEX_RE(int? CostCenterId, string FromDate, string ToDate, int BranchId, string Con, int? yearid, string FlagTotal)
        {

            var Branch = _BranchesRepository.GetById(BranchId);
            if (Branch == null || Branch.TaxsAccId == null)
            {
                return new List<CostCenterEX_REVM>();
            }
            else if (Branch == null || Branch.SuspendedFundAccId == null)
            {
                return new List<CostCenterEX_REVM>();
            }
            else if (Branch == null || Branch.PurchaseReturnCashAccId == null)
            {
                return new List<CostCenterEX_REVM>();
            }
            if (yearid != null)
            {
                return await _accountsRepository.GetCostCenterEX_RE(CostCenterId, FromDate, ToDate, BranchId, Con, yearid ?? default(int), Branch.TaxsAccId, Branch.SuspendedFundAccId, Branch.PurchaseReturnCashAccId, FlagTotal);
            }
            return new List<CostCenterEX_REVM>();
        }

        public async Task<IEnumerable<DetailedRevenuVM>> GetDetailedRevenu(int? CustomerId, string FromDate, string ToDate, int BranchId, string Con, int? yearid)
        {

            var Branch = _BranchesRepository.GetById(BranchId);
            if (Branch == null || Branch.TaxsAccId == null)
            {
                return new List<DetailedRevenuVM>();
            }
            else if (Branch == null || Branch.SuspendedFundAccId == null)
            {
                return new List<DetailedRevenuVM>();
            }
            else if (Branch == null || Branch.ContractsAccId == null)
            {
                return new List<DetailedRevenuVM>();
            }
            else if (Branch == null || Branch.PurchaseReturnCashAccId == null)
            {
                return new List<DetailedRevenuVM>();
            }
            if (yearid != null)
            {
                return await _accountsRepository.GetDetailedRevenu(CustomerId, FromDate, ToDate, BranchId, Con, yearid ?? default(int), Branch.TaxsAccId, Branch.SuspendedFundAccId, Branch.ContractsAccId, Branch.PurchaseReturnCashAccId);
            }
            return new List<DetailedRevenuVM>();
        }

        public async Task<IEnumerable<DetailedRevenuVM>> GetDetailedRevenuExtra(int? CustomerId, int? ProjectId, string FromDate, string ToDate, int BranchId, string Con, int? yearid)
        {

            var Branch = _BranchesRepository.GetById(BranchId);
            if (Branch == null || Branch.TaxsAccId == null)
            {
                return new List<DetailedRevenuVM>();
            }
            else if (Branch == null || Branch.SuspendedFundAccId == null)
            {
                return new List<DetailedRevenuVM>();
            }

            if (yearid != null)
            {
                return await _accountsRepository.GetDetailedRevenuExtra(CustomerId, ProjectId, FromDate, ToDate, BranchId, Con, yearid ?? default(int), Branch.TaxsAccId, Branch.SuspendedFundAccId);
            }
            return new List<DetailedRevenuVM>();
        }

        public async Task<IEnumerable<InvoicedueC>> GetInvoicedue(int? CustomerId, string FromDate, string ToDate, int BranchId, string Con, int? yearid)
        {
            if (yearid != null)
            {
                return await _accountsRepository.GetInvoicedue(CustomerId, FromDate, ToDate, BranchId, Con, yearid ?? default(int));
            }
            return new List<InvoicedueC>();
        }


        //salah
        public async Task<IEnumerable<DetailedExpenseVM>> GetDetailedExpensesd(int? AccountId, string FromDate, string ToDate, string ExpenseType, int BranchId, string Con, int? yearid)
        {

            var Branch = _BranchesRepository.GetById(BranchId);
            if (Branch == null || Branch.TaxsAccId == null)
            {
                return new List<DetailedExpenseVM>();
            }
            else if (Branch == null || Branch.SuspendedFundAccId == null)
            {
                return new List<DetailedExpenseVM>();
            }

            if (yearid != null)
            {
                return await _accountsRepository.GetDetailedExpensesd(AccountId, FromDate, ToDate, ExpenseType, BranchId, Con, yearid ?? default(int), Branch.TaxsAccId, Branch.SuspendedFundAccId);
            }
            return new List<DetailedExpenseVM>();
        }

        public async Task<IEnumerable<AccountStatmentVM>> GetFullAccountStatmentDGV(string FromDate, string ToDate, string AccountCode, string CCID, string Con, int BranchId, int? yearid)
        {
            //var year = _fiscalyearsRepository.GetCurrentYear();
            if (yearid != null)
            {
                return await _accountsRepository.GetFullAccountStatmentDGV(FromDate, ToDate, AccountCode, CCID, Con, BranchId, yearid ?? default(int));
            }
            return new List<AccountStatmentVM>();
            //return null;
        }
        public async Task<string> GetNewCodeByParentId(int ParentId, int Type)
        {
            return await _accountsRepository.GetNewCodeByParentId(ParentId, Type);
        }
    
    }
}

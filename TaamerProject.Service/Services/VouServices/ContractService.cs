using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.Generic;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;
using Twilio.TwiML.Voice;

namespace TaamerProject.Service.Services
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _ContractRepository;
        private readonly ICustomerPaymentsRepository _customerPaymentsRepository;
        private readonly IProjectRepository _ProjectRepository;
        private readonly ITransactionsRepository _TransactionsRepository;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountsRepository _AccountsRepository;
        private readonly IFiscalyearsRepository _fiscalyearsRepository;
        private readonly ISystemSettingsRepository _SystemSettingsRepository;
        private readonly IOrganizationsRepository _OrganizationsRepository;
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;
        private readonly IContractDetailsRepository _contractDetailsRepository;
        private readonly IDraftDetailsRepository _draftDetailsRepository;
        private readonly IDraftRepository _draftRepository;
        private readonly IInvoicesRepository _invoicesRepository;
        private readonly IContractStageRepository _contractStageRepository;
        private readonly IContractServicesRepository _contractServicesRepository;
        private readonly ISystemAction _SystemAction;
        private readonly TaamerProjectContext _TaamerProContext;


        public ContractService(IContractRepository contractRepository, ICustomerPaymentsRepository customerPaymentsRepository, IProjectRepository projectRepository,
            ITransactionsRepository transactions, IBranchesRepository branches, ICustomerRepository customerRepository,
           IAccountsRepository accountsRepository, IFiscalyearsRepository fiscalyearsRepository, ISystemSettingsRepository systemSettings,
           IOrganizationsRepository organizations, ISys_SystemActionsRepository sys_SystemActionsRepository, IContractDetailsRepository contractDetailsRepository,
           IDraftDetailsRepository draftDetailsRepository, IDraftRepository draft, IInvoicesRepository invoicesRepository, IContractStageRepository contractStageRepository,
           IContractServicesRepository contractServicesRepository, ISystemAction systemAction, TaamerProjectContext taamerProjectContext)
        {
            _ContractRepository = contractRepository;
            _customerPaymentsRepository = customerPaymentsRepository;
            _ProjectRepository = projectRepository;
            _TransactionsRepository = transactions;
            _BranchesRepository = branches;
            _customerRepository = customerRepository;
            _AccountsRepository = accountsRepository;
            _SystemSettingsRepository = systemSettings;
            _fiscalyearsRepository = fiscalyearsRepository;
            _OrganizationsRepository = organizations;
            _Sys_SystemActionsRepository = sys_SystemActionsRepository;
            _contractDetailsRepository = contractDetailsRepository;
            _draftDetailsRepository = draftDetailsRepository;
            _draftRepository = draft;
            _invoicesRepository = invoicesRepository;
            _contractStageRepository = contractStageRepository;
            _contractServicesRepository = contractServicesRepository;
            _SystemAction = systemAction;
            _TaamerProContext = taamerProjectContext;
        }

        public async Task< IEnumerable<ContractsVM>> GetAllContracts()
        {
            var contracts = await _ContractRepository.GetAllContracts();
            return contracts;
        }
        public async Task<IEnumerable<ContractsVM>> GetAllContracts_B(int BranchId, int? yearid)
        {
            var contracts = await _ContractRepository.GetAllContracts_B(BranchId, yearid ?? 0);
            return contracts;
        }


        public async Task<IEnumerable<ContractsVM>> GetContractbyid(int contractid)
        {
            var contracts =await _ContractRepository.GetContractById(contractid);
            return contracts;
        }
        public async Task<IEnumerable<ContractsVM>> GetAllContractsBySearch(ContractsVM contractsVM, int BranchId, int? yearid)
        {
            if (yearid != null)
            {
                if (contractsVM.IsSearch)
                {

                    return await _ContractRepository.GetAllContractsBySearch(contractsVM, BranchId, yearid ?? default(int));
                }
                //IEnumerable<InvoicesVM> orderbynewinvoice = _InvoicesRepository.GetAllVouchers(voucherFilterVM, yearid ?? default(int), BranchId);
                //orderbynewinvoice = orderbynewinvoice.OrderBy(a => a.InvoiceNumber);
                //return orderbynewinvoice;
                //return _ContractRepository.GetAllContracts_B(BranchId, yearid ?? 0);

                return await _ContractRepository.GetAllContracts_BSearch(contractsVM, BranchId, yearid ?? 0);

            }

            return new List<ContractsVM>();
        }
        public async Task<IEnumerable<ContractsVM>> GetAllContractsBySearchCustomer(ContractsVM contractsVM, int BranchId, int? yearid)
        {
            if (yearid != null)
            {
                if (contractsVM.IsSearch)
                {

                    return await _ContractRepository.GetAllContractsBySearchCustomer(contractsVM, BranchId, yearid ?? default(int));
                }
                return await _ContractRepository.GetAllContracts_BSearchCustomer(contractsVM, BranchId, yearid ?? 0);

            }

            return new List<ContractsVM>();
        }
        public GeneralMessage SaveContract(Contracts contract, int UserId, int BranchId, int? yearid)
        {
            try
            {
                var codeExist = _TaamerProContext.Contracts.Where(s => s.IsDeleted == false && s.ContractId != contract.ContractId && s.ContractNo == contract.ContractNo).FirstOrDefault();
                if (codeExist != null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ العقد";
                    _SystemAction.SaveAction("SaveContract", "ContractService", 1, Resources.TheCodeAlreadyExists, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.TheCodeAlreadyExists };
                }
                //var year = _fiscalyearsRepository.GetCurrentYear();
                if (yearid == null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ العقد";
                    _SystemAction.SaveAction("SaveContract", "ContractService", 1, "فشل في الحفظ ,تأكد من اختيار السنة الماليه أولا", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }
                if (contract.ContractId == 0)
                {
                    contract.BranchId = BranchId;
                    contract.AddUser = UserId;
                    contract.AddDate = DateTime.Now;
                    _TaamerProContext.Contracts.Add(contract);
                    //_uow.SaveChanges();

                    if (contract.CustomerPayments != null && contract.CustomerPayments.Count > 0)
                    {
                        foreach (var item in contract.CustomerPayments.ToList())
                        {
                            item.PaymentDate = item.PaymentDate;
                            item.PaymentDateHijri = item.PaymentDateHijri;
                            item.AddUser = UserId;
                            item.AddDate = DateTime.Now;
                            Utilities util = new Utilities(item.Amount.ToString());
                            try
                            {
                                item.AmountValueText = util.GetNumberAr();
                            }
                            catch (Exception)
                            {
                                item.AmountValueText = item.Amount.ToString();
                            }
                            _TaamerProContext.CustomerPayments.Add(item);
                        }
                    }
                    //ContractDetails
                    if (contract.ContractDetails != null && contract.ContractDetails.Count > 0)
                    {
                        foreach (var item in contract.ContractDetails)
                        {
                            item.ContractId = contract.ContractId;
                            _TaamerProContext.ContractDetails.Add(item);
                        }
                    }
                    //End ContractDetails

                    //Contractphases
                    if (contract.ContractStage != null && contract.ContractStage.Count > 0)
                    {
                        foreach (var item in contract.ContractStage)
                        {
                            item.ContractId = contract.ContractId;
                            _TaamerProContext.ContractStage.Add(item);
                        }
                    }
                    //End Contractphases

                    
                    // add transactions
                    //depit 
                    var depitTrans = new Transactions();
                    depitTrans.TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    depitTrans.TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                    depitTrans.ContractId = _ContractRepository.GetMaxId().Result + 1;
                    /////////////////////////////////////////////
                    var CustAccId = _customerRepository.GetById(contract.CustomerId??0).AccountId;
                    if (CustAccId != null)
                    {
                        depitTrans.AccountId = _customerRepository.GetById(contract.CustomerId??0).AccountId;//contract.Customer.AccountId;
                    }
                    else
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حفظ العقد";
                       _SystemAction.SaveAction("SaveContract", "ContractService", 1, "فشل في الحفظ ,تأكد من وجود حساب للعميل", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                    }
                    depitTrans.AccountType = _AccountsRepository.GetById(depitTrans.AccountId??0).Type;
                    depitTrans.CustomerId = contract.CustomerId;
                    depitTrans.Type = 12;
                    depitTrans.LineNumber = 1;
                    depitTrans.Depit = contract.Value; depitTrans.Credit = 0;
                    depitTrans.YearId = yearid;
                    depitTrans.Notes = depitTrans.Details = _customerRepository.GetById(contract.CustomerId??0).CustomerNameAr + " للعميل " + contract.ContractNo + "عقد رقم";
                    depitTrans.InvoiceReference = contract.ContractNo;
                    depitTrans.IsPost = true;
                    depitTrans.AddDate = DateTime.Now;
                    depitTrans.AddUser = UserId;
                    depitTrans.IsDeleted = false;
                    _TaamerProContext.Transactions.Add(depitTrans);

                    //credit 
                    var creditTrans = new Transactions();
                    creditTrans.TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    creditTrans.TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                    creditTrans.ContractId = _ContractRepository.GetMaxId().Result + 1;
                    try
                    {
                        var contarctBranchAcc = _BranchesRepository.GetById(BranchId).ContractsAccId;
                        if (contarctBranchAcc == null)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote3 = "فشل في حفظ العقد";
                           _SystemAction.SaveAction("SaveContract", "ContractService", 1, "فشل في الحفظ ,تأكد من ربط حساب العقود بالفرع", "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                            //-----------------------------------------------------------------------------------------------------------------

                            return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                        }
                        creditTrans.AccountId = contarctBranchAcc;
                    }
                    catch (Exception)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote4 = "فشل في حفظ العقد";
                        _SystemAction.SaveAction("SaveContract", "ContractService", 1, "فشل في الحفظ ,تأكد من ربط حساب العقود بالفرع", "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                    }
                    creditTrans.AccountType = _AccountsRepository.GetById((int)creditTrans.AccountId).Type;
                    creditTrans.CustomerId = contract.CustomerId;
                    creditTrans.Type = 12;
                    creditTrans.LineNumber = 2;
                    creditTrans.Credit = contract.Value; creditTrans.Depit = 0;
                    creditTrans.YearId = yearid;
                    creditTrans.Notes = creditTrans.Details = _customerRepository.GetById((int)contract.CustomerId).CustomerNameAr + " للعميل " + contract.ContractNo + "عقد رقم ";
                    creditTrans.InvoiceReference = contract.ContractNo;
                    creditTrans.IsPost = true;
                    creditTrans.AddDate = DateTime.Now;
                    creditTrans.AddUser = UserId;
                    creditTrans.IsDeleted = false;
                    _TaamerProContext.Transactions.Add(creditTrans);
                    _TaamerProContext.SaveChanges();
                    if (contract.ServicesPriceOffer != null && contract.ServicesPriceOffer.Count > 0)
                    {
                        foreach (var item in contract.ServicesPriceOffer)
                        {
                            item.AddUser = UserId;
                            item.AddDate = DateTime.Now;
                            item.ContractId = contract.ContractId;

                            _TaamerProContext.Acc_Services_PriceOffer.Add(item);

                        }

                    }
                    var project = _TaamerProContext.Project.Where(x => x.ProjectId == contract.ProjectId).FirstOrDefault();
                    if (project != null)
                    {
                        project.ContractId = contract.ContractId;
                        project.MotionProject = 1;
                        project.MotionProjectDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        project.MotionProjectNote = "أضافة عقد علي مشروع";
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة عقد جديد" +contract.ContractNo;
                    _SystemAction.SaveAction("SaveContract", "ContractService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedStr = contract.ContractId.ToString() };
                }
                else
                {
                    var ContractUpdated = _ContractRepository.GetById(contract.ContractId);
                    if (ContractUpdated != null)
                    {
                        ContractUpdated.ContractNo = contract.ContractNo;
                        ContractUpdated.Date = contract.Date;
                        ContractUpdated.Value = contract.Value;
                        ContractUpdated.ValueText = contract.ValueText;
                        ContractUpdated.TaxesValue = contract.TaxesValue;
                        ContractUpdated.ProjectId = contract.ProjectId;
                        ContractUpdated.CustomerId = contract.CustomerId;
                        ContractUpdated.Type = contract.Type;
                        ContractUpdated.OrgId = contract.OrgId;
                        ContractUpdated.OrgEmpId = contract.OrgEmpId;
                        ContractUpdated.OrgEmpJobId = contract.OrgEmpJobId;
                        ContractUpdated.ServiceId = contract.ServiceId;
                        ContractUpdated.ProjBriefDesc_Des = contract.ProjBriefDesc_Des;
                    }

                    //remove prev ServicesPriceOffer
                    var OldDataDetails = _TaamerProContext.Acc_Services_PriceOffer.Where(s => s.ContractId == contract.ContractId).ToList();
                    if (OldDataDetails.Count() > 0)
                    {
                        _TaamerProContext.Acc_Services_PriceOffer.RemoveRange(OldDataDetails);
                    }
                    if (contract.ServicesPriceOffer != null && contract.ServicesPriceOffer.Count > 0)
                    {
                        foreach (var item in contract.ServicesPriceOffer)
                        {
                            item.AddUser = UserId;
                            item.AddDate = DateTime.Now;
                            item.ContractId = contract.ContractId;
                            _TaamerProContext.Acc_Services_PriceOffer.Add(item);
                        }
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل عقد رقم " + contract.ContractNo;
                   _SystemAction.SaveAction("SaveContract", "ContractService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase =  Resources.General_EditedSuccessfully , ReturnedStr = contract.ContractId.ToString() };

                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ العقد"+contract.ContractNo;
                _SystemAction.SaveAction("SaveContract", "ContractService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed,ReturnedStrNeeded=ex.Message };
            }
        }
        public GeneralMessage EditContract(Contracts contract, int UserId, int BranchId, int? yearid)
        {
            try
            {

                if (yearid == null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote4 = "فشل في حفظ العقد";
                    _SystemAction.SaveAction("EditContract", "ContractService", 1, "فشل في الحفظ ,تأكد من اختيار السنة الماليه أولا", "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }
                var ContractUpdated = _ContractRepository.GetById(contract.ContractId);
                if (ContractUpdated != null)
                {
                    ContractUpdated.Value = contract.Value;
                    ContractUpdated.ValueText = contract.ValueText;
                    ContractUpdated.TaxesValue = contract.TaxesValue;
                    ContractUpdated.TotalValue = contract.TotalValue;
                    ContractUpdated.UpdateUser = UserId;
                    ContractUpdated.UpdateDate = DateTime.Now;

                    decimal TotalValuePayment = 0;
                    if (ContractUpdated.CustomerPayments != null && ContractUpdated.CustomerPayments.Count > 0)
                    {
                        foreach (var item in ContractUpdated.CustomerPayments.ToList())
                        {
                            TotalValuePayment += item.Amount;
                        }
                    }

                    if (contract.Value < TotalValuePayment)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate22 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote22 = "فشل في حفظ العقد";
                       _SystemAction.SaveAction("EditContract", "ContractService", 1, "فشل في الحفظ ,قيمة العقد اقل من قيمة الدفعات", "", "", ActionDate22, UserId, BranchId, ActionNote22, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                    }

                    //remove transaction 
                    var ConTra = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.ContractId == contract.ContractId);
                    if (ConTra.Count() > 0)
                    {
                        _TaamerProContext.Transactions.RemoveRange(ConTra);

                    }

                    // add transactions
                    //depit 
                    var depitTrans = new Transactions();
                    depitTrans.TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    depitTrans.TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                    depitTrans.ContractId = contract.ContractId;
                    /////////////////////////////////////////////
                    var CustAccId = _customerRepository.GetById((int)ContractUpdated.CustomerId).AccountId;
                    if (CustAccId != null)
                    {
                        depitTrans.AccountId = CustAccId;//contract.Customer.AccountId;
                    }
                    else
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate23 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote23 = "فشل في حفظ العقد";
                        _SystemAction.SaveAction("EditContract", "ContractService", 1, "فشل في الحفظ ,تأكد من وجود حساب للعميل", "", "", ActionDate23, UserId, BranchId, ActionNote23, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                    }
                    depitTrans.AccountType = _AccountsRepository.GetById((int)depitTrans.AccountId).Type;
                    depitTrans.CustomerId = ContractUpdated.CustomerId;
                    depitTrans.Type = 12;
                    depitTrans.LineNumber = 1;
                    depitTrans.Depit = contract.Value; depitTrans.Credit = 0;
                    depitTrans.YearId = yearid;
                    depitTrans.Notes = depitTrans.Details = _customerRepository.GetById((int)ContractUpdated.CustomerId).CustomerNameAr + " للعميل " + ContractUpdated.ContractNo + "عقد رقم";
                    depitTrans.InvoiceReference = ContractUpdated.ContractNo;
                    depitTrans.IsPost = true;
                    depitTrans.AddDate = DateTime.Now;
                    depitTrans.AddUser = UserId;
                    depitTrans.IsDeleted = false;
                    _TaamerProContext.Transactions.Add(depitTrans);

                    //credit 
                    var creditTrans = new Transactions();
                    creditTrans.TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    creditTrans.TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                    creditTrans.ContractId = contract.ContractId;
                    try
                    {
                        var contarctBranchAcc = _BranchesRepository.GetById(BranchId).ContractsAccId;
                        if (contarctBranchAcc == null)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote3 = "فشل في حفظ العقد";
                           _SystemAction.SaveAction("EditContract", "ContractService", 1, "فشل في الحفظ ,تأكد من ربط حساب العقود بالفرع", "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                            //-----------------------------------------------------------------------------------------------------------------

                            return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                        }
                        creditTrans.AccountId = contarctBranchAcc;
                    }
                    catch (Exception)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote4 = "فشل في حفظ العقد";
                       _SystemAction.SaveAction("EditContract", "ContractService", 1, "فشل في الحفظ ,تأكد من ربط حساب العقود بالفرع", "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                    }
                    creditTrans.AccountType = _AccountsRepository.GetById((int)creditTrans.AccountId).Type;
                    creditTrans.CustomerId = ContractUpdated.CustomerId;
                    creditTrans.Type = 12;
                    creditTrans.LineNumber = 2;
                    creditTrans.Credit = contract.Value; creditTrans.Depit = 0;
                    creditTrans.YearId = yearid;
                    creditTrans.Notes = creditTrans.Details = _customerRepository.GetById((int)ContractUpdated.CustomerId).CustomerNameAr + " للعميل " + ContractUpdated.ContractNo + "عقد رقم ";
                    creditTrans.InvoiceReference = ContractUpdated.ContractNo;
                    creditTrans.IsPost = true;
                    creditTrans.AddDate = DateTime.Now;
                    creditTrans.AddUser = UserId;
                    creditTrans.IsDeleted = false;
                    _TaamerProContext.Transactions.Add(creditTrans);

                }
                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تعديل عقد رقم " + contract.ContractId;
               _SystemAction.SaveAction("EditContract", "ContractService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase =  Resources.General_EditedSuccessfully , ReturnedStr = contract.ContractId.ToString() };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ العقد";
                _SystemAction.SaveAction("SaveContract", "ContractService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public GeneralMessage SaveContract2(Contracts contract, int UserId, int BranchId, int? yearid)
        {
            try
            {
                var codeExist = _TaamerProContext.Contracts.Where(s => s.IsDeleted == false && s.ContractId != contract.ContractId && s.ContractNo == contract.ContractNo).FirstOrDefault();
                if (codeExist != null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ العقد";
                    _SystemAction.SaveAction("SaveContract2", "ContractService", 1, Resources.TheCodeAlreadyExists, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.TheCodeAlreadyExists };
                }
                //var year = _fiscalyearsRepository.GetCurrentYear();
                if (yearid == null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ العقد";
                    _SystemAction.SaveAction("SaveContract2", "ContractService", 1, "فشل في الحفظ ,تأكد من اختيار السنة الماليه أولا", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }
                if (contract.ContractId == 0)
                {
                    contract.BranchId = BranchId;
                    contract.AddUser = UserId;
                    contract.AddDate = DateTime.Now;
                    _TaamerProContext.Contracts.Add(contract);

                    if (contract.CustomerPayments != null && contract.CustomerPayments.Count > 0)
                    {
                        foreach (var item in contract.CustomerPayments.ToList())
                        {
                            item.PaymentDate = item.PaymentDate;
                            item.PaymentDateHijri = item.PaymentDateHijri;
                            item.AddUser = UserId;
                            item.AddDate = DateTime.Now;
                            Utilities util = new Utilities(item.Amount.ToString());
                            try
                            {
                                item.AmountValueText = util.GetNumberAr();
                            }
                            catch (Exception)
                            {
                                item.AmountValueText = item.Amount.ToString();
                            }
                            _TaamerProContext.CustomerPayments.Add(item);
                        }
                    }
                    var project = _TaamerProContext.Project.Where(x=>x.ProjectId==contract.ProjectId).FirstOrDefault();
                    if (project != null)
                    {
                        project.ContractId = _ContractRepository.GetMaxId().Result + 1;
                    }
                    // add transactions
                    //depit 
                    var depitTrans = new Transactions();
                    depitTrans.TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    depitTrans.TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                    depitTrans.ContractId = _ContractRepository.GetMaxId().Result + 1;
                    /////////////////////////////////////////////
                    var CustAccId = _customerRepository.GetById((int)contract.CustomerId).AccountId;
                    if (CustAccId != null)
                    {
                        depitTrans.AccountId = _customerRepository.GetById((int)contract.CustomerId).AccountId;//contract.Customer.AccountId;
                    }
                    else
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حفظ العقد";
                        _SystemAction.SaveAction("SaveContract2", "ContractService", 1, "فشل في الحفظ ,تأكد من وجود حساب للعميل", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                    }
                    depitTrans.AccountType = _AccountsRepository.GetById((int)depitTrans.AccountId).Type;
                    depitTrans.CustomerId = contract.CustomerId;
                    depitTrans.Type = 12;
                    depitTrans.LineNumber = 1;
                    depitTrans.Depit = contract.Value; depitTrans.Credit = 0;
                    depitTrans.YearId = yearid;
                    depitTrans.Notes = depitTrans.Details = _customerRepository.GetById((int)contract.CustomerId).CustomerNameAr + " للعميل " + contract.ContractNo + "عقد رقم";
                    depitTrans.InvoiceReference = contract.ContractNo;
                    depitTrans.IsPost = true;
                    depitTrans.AddDate = DateTime.Now;
                    depitTrans.AddUser = UserId;
                    depitTrans.IsDeleted = false;
                    _TaamerProContext.Transactions.Add(depitTrans);

                    //credit 
                    var creditTrans = new Transactions();
                    creditTrans.TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    creditTrans.TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                    creditTrans.ContractId = _ContractRepository.GetMaxId().Result + 1;
                    try
                    {
                        var contarctBranchAcc = _BranchesRepository.GetById(BranchId).ContractsAccId;
                        if (contarctBranchAcc == null)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote3 = "فشل في حفظ العقد";
                            _SystemAction.SaveAction("SaveContract2", "ContractService", 1, "فشل في الحفظ ,تأكد من ربط حساب العقود بالفرع", "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                            //-----------------------------------------------------------------------------------------------------------------

                            return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                        }
                        creditTrans.AccountId = contarctBranchAcc;
                    }
                    catch (Exception)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote4 = "فشل في حفظ العقد";
                        _SystemAction.SaveAction("SaveContract2", "ContractService", 1, "فشل في الحفظ ,تأكد من ربط حساب العقود بالفرع", "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                    }
                    creditTrans.AccountType = _AccountsRepository.GetById((int)creditTrans.AccountId).Type;
                    creditTrans.CustomerId = contract.CustomerId;
                    creditTrans.Type = 12;
                    creditTrans.LineNumber = 2;
                    creditTrans.Credit = contract.Value; creditTrans.Depit = 0;
                    creditTrans.YearId = yearid;
                    creditTrans.Notes = creditTrans.Details = _customerRepository.GetById((int)contract.CustomerId).CustomerNameAr + " للعميل " + contract.ContractNo + "عقد رقم ";
                    creditTrans.InvoiceReference = contract.ContractNo;
                    creditTrans.IsPost = true;
                    creditTrans.AddDate = DateTime.Now;
                    creditTrans.AddUser = UserId;
                    creditTrans.IsDeleted = false;

                    _TaamerProContext.Transactions.Add(creditTrans);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة عقد جديد";
                    _SystemAction.SaveAction("SaveContract2", "ContractService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var ContractUpdated = _ContractRepository.GetById(contract.ContractId);
                    if (ContractUpdated != null)
                    {
                        ContractUpdated.ContractNo = contract.ContractNo;
                        ContractUpdated.Date = contract.Date;
                        ContractUpdated.Value = contract.Value;
                        ContractUpdated.ValueText = contract.ValueText;
                        ContractUpdated.TaxesValue = contract.TaxesValue;
                        ContractUpdated.ProjectId = contract.ProjectId;
                        ContractUpdated.CustomerId = contract.CustomerId;
                        ContractUpdated.Type = contract.Type;
                        ContractUpdated.OrgId = contract.OrgId;
                        ContractUpdated.OrgEmpId = contract.OrgEmpId;
                        ContractUpdated.OrgEmpJobId = contract.OrgEmpJobId;
                        ContractUpdated.ServiceId = contract.ServiceId;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل عقد رقم " + contract.ContractId;
                    _SystemAction.SaveAction("SaveContract2", "ContractService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };
                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ العقد";
                _SystemAction.SaveAction("SaveContract2", "ContractService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage SaveContractFile(int contractid, int UserId, int BranchId, string Url)
        {
            try
            {
                // var contract = _ContractRepository.GetMatching(s => s.ContractId == contractid);
                var ContractUpdated = _ContractRepository.GetById(contractid);
                string atturl = ContractUpdated.AttachmentUrl;

                //if (!String.IsNullOrEmpty( atturl) )
                //{


                //    return new GeneralMessage { Result = false, Message = Resources.MAcc_ContractChanged };
                //}

                if (ContractUpdated != null)
                {
                    ContractUpdated.ContractNo = ContractUpdated.ContractNo;
                    ContractUpdated.Date = ContractUpdated.Date;
                    ContractUpdated.Value = ContractUpdated.Value;
                    ContractUpdated.ValueText = ContractUpdated.ValueText;
                    ContractUpdated.TaxesValue = ContractUpdated.TaxesValue;
                    ContractUpdated.ProjectId = ContractUpdated.ProjectId;
                    ContractUpdated.CustomerId = ContractUpdated.CustomerId;
                    ContractUpdated.Type = ContractUpdated.Type;
                    ContractUpdated.AttachmentUrl = Url;

                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تعديل ملف عقد رقم " + contractid;
               _SystemAction.SaveAction("SaveContractFile", "ContractService", 2, "Resources.MAcc_ContractSaved", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.MAcc_ContractSaved };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ ملف العقد";
                _SystemAction.SaveAction("SaveContractFile", "ContractService", 1, "Resources.MAcc_ContractSavedFailed", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.MAcc_ContractSavedFailed };
            }
        }
        public GeneralMessage SaveContractFileExtra(int contractid, int UserId, int BranchId, string Url)
        {
            try
            {
                var ContractUpdated = _ContractRepository.GetById(contractid);
                string atturl = ContractUpdated.AttachmentUrlExtra;
                if (ContractUpdated != null)
                {
                    ContractUpdated.AttachmentUrlExtra = Url;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تعديل ملف عقد رقم " + contractid;
                _SystemAction.SaveAction("SaveContractFileExtra", "ContractService", 2, "Resources.MAcc_ContractSaved", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الملف ";
                _SystemAction.SaveAction("SaveContractFileExtra", "ContractService", 1, "فشل في حفظ الملف ", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage CancelContract(int ContractId, int UserId, int BranchId)
        {
            try
            {
                //var contractPayments = _customerPaymentsRepository.GetMatching(s => s.ContractId == ContractId);
                var contractPayments = _TaamerProContext.CustomerPayments.Where(s => s.ContractId == ContractId && s.IsPaid == false).ToList();
                var contractPaymentsPaid = _TaamerProContext.CustomerPayments.Where(s => s.ContractId == ContractId && s.IsPaid == true).ToList();

                if (contractPaymentsPaid.Count() > 0)
                    foreach (var loop in contractPaymentsPaid)
                    {
                        var InvoiceCust = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.InvoiceId == loop.InvoiceId && s.Rad == false).ToList();
                        if (InvoiceCust.Count() > 0)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = " فشل في الغاء عقد  ";
                            _SystemAction.SaveAction("CancelContract", "ContractService", 3, "لايمكن الغاء العقد هناك فواتير تم اصدارها من دفعات يجب الغاؤها اولا", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لايمكن الغاء العقد هناك فواتير تم اصدارها من دفعات يجب الغاؤها اولا" };
                        }
                    }
                if (contractPayments.Count() > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " فشل في الغاء عقد  ";
                    _SystemAction.SaveAction("CancelContract", "ContractService", 3, "لايمكن الغاء العقد هناك دفعات لم يتم تسديدها", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لايمكن الغاء العقد هناك دفعات لم يتم تسديدها" };
                }
                Contracts contract = _ContractRepository.GetById(ContractId);
                if(contract!=null)
                {
                    contract.IsDeleted = true;
                    contract.DeleteDate = DateTime.Now;
                    contract.DeleteUser = 1;
                    var pro=_TaamerProContext.Project.Where(s => s.ProjectId == contract.ProjectId).FirstOrDefault();
                    if(pro!=null) pro.ContractId = null;
                    //contract.Project.ContractId = null;
                    var tranCon = _TaamerProContext.Transactions.Where(s => s.ContractId == contract.ContractId).ToList();
                    var contractDetailsCon = _TaamerProContext.ContractDetails.Where(s => s.ContractId == contract.ContractId).ToList();
                    var draftDetailsCon = _TaamerProContext.DraftDetails.Where(s => s.ProjectId == contract.ProjectId).ToList();
                    if (tranCon.Count() > 0)
                    {
                        _TaamerProContext.Transactions.RemoveRange(tranCon);
                    }
                    if (contractDetailsCon.Count() > 0)
                    {
                        _TaamerProContext.ContractDetails.RemoveRange(contractDetailsCon);
                    }
                    if (draftDetailsCon.Count() > 0)
                    {
                        _TaamerProContext.DraftDetails.RemoveRange(draftDetailsCon);
                    }
                }



                //_TaamerProContext.Transactions.RemoveRange(contract.TransactionDetails.ToList());
                //_TaamerProContext.ContractDetails.RemoveRange(contract.ContractDetails.ToList());
                //_TaamerProContext.DraftDetails.RemoveRange(contract.Project.DraftDetails.ToList());
                var draft = _TaamerProContext.Draft.Where(x => x.Name == "Contract_" + contract.Project.ProjectNo + ".docx").FirstOrDefault();
                if (draft != null)
                    draft.IsDeleted = true;

                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " الغاء عقد رقم " + ContractId;
                _SystemAction.SaveAction("CancelContract", "ContractService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في الغاء عقد  ";
                _SystemAction.SaveAction("CancelContract", "ContractService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                var exc = ex;
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public List<CustomerPaymentsVM> GenerateCustomerPayments(ContractsVM contractsVM, int OrgId)
        {



            var Organizations = _OrganizationsRepository.GetBranchOrganizationData(OrgId).Result;

            decimal ValueAdded_V = 0;
            if (Convert.ToInt32(Organizations.VAT) == 0)
            {
                ValueAdded_V = 15;

            }
            else
            {
                ValueAdded_V = Convert.ToDecimal(Organizations.VAT);

            }

            //decimal TaxSha = (Convert.ToDecimal(ValueAdded_V) / 100);
            //decimal Tax8erSha = (Convert.ToDecimal(ValueAdded_V) / 100)+1;


            var PaymentList = new List<CustomerPaymentsVM>();
            var Date_T = DateTime.ParseExact(contractsVM.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddMonths(-1);
            var advancePayment = new CustomerPayments();
            for (int i = 1; i <= contractsVM.PaymentsCount + 1; i++)
            {
                var obj = new CustomerPaymentsVM();
                obj.PaymentNo = i;
                switch (contractsVM.GregorianHijriPay)
                {
                    case 1:
                        obj.PaymentDate = Date_T.AddMonths(i).ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        obj.PaymentDateHijri = ConvertDateCalendar(DateTime.ParseExact(obj.PaymentDate, "yyyy-MM-dd", CultureInfo.InvariantCulture), "Hijri", "en-US");
                        break;
                    case 2:
                        var PayHijriDate = DateTime.ParseExact(contractsVM.PaymentHijriDate, "yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar")).AddMonths(i);
                        obj.PaymentDate = HijriToGreg(PayHijriDate.ToString("yyyy-MM-dd")).ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        obj.PaymentDateHijri = ConvertDateCalendar(HijriToGreg(PayHijriDate.ToString("yyyy-MM-dd")), "Hijri", "en-US");
                        break;
                    default:
                        break;
                }
                if (contractsVM.AdvancePayValue != 0 && i == 1)  ///case advancevalue
                {
                    contractsVM.PaymentsCount += 1;
                    obj.PaymentDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    obj.PaymentDateHijri = ConvertDateCalendar(DateTime.ParseExact(obj.PaymentDate, "yyyy-MM-dd", CultureInfo.InvariantCulture), "Hijri", "en-US");
                    switch (contractsVM.TaxType)
                    {
                        case 1: // no tax
                            obj.Amount = contractsVM.AdvancePayValue;
                            obj.TaxAmount = 0;
                            obj.TotalAmount = contractsVM.AdvancePayValue;
                            break;
                        case 3: //with tax
                            //obj.Amount = Convert.ToInt32(contractsVM.AdvancePayValue)- Convert.ToInt32(contractsVM.AdvancePayValue - (contractsVM.AdvancePayValue / Convert.ToDecimal(1.15)));
                            //obj.TaxAmount =Convert.ToInt32(contractsVM.AdvancePayValue - (contractsVM.AdvancePayValue / Convert.ToDecimal(1.15)));
                            //obj.TotalAmount = Convert.ToInt32(contractsVM.AdvancePayValue);


                            obj.Amount = Convert.ToInt32(contractsVM.AdvancePayValue) - Convert.ToInt32(contractsVM.AdvancePayValue - (contractsVM.AdvancePayValue / Convert.ToDecimal(Convert.ToDecimal(ValueAdded_V / 100) + 1)));
                            obj.TaxAmount = Convert.ToInt32(contractsVM.AdvancePayValue - (contractsVM.AdvancePayValue / Convert.ToDecimal(Convert.ToDecimal(ValueAdded_V / 100) + 1)));
                            obj.TotalAmount = Convert.ToInt32(contractsVM.AdvancePayValue);


                            break;
                        case 2: //without tax 


                            //obj.Amount = Convert.ToInt32(contractsVM.AdvancePayValue);
                            //obj.TaxAmount = Convert.ToInt32((contractsVM.AdvancePayValue * Convert.ToDecimal(0.15)));
                            //obj.TotalAmount = Convert.ToInt32((Convert.ToInt32(contractsVM.AdvancePayValue) + (contractsVM.AdvancePayValue * Convert.ToDecimal(0.15))));//obj.Amount + obj.TaxAmount;


                            obj.Amount = Convert.ToInt32(contractsVM.AdvancePayValue);
                            obj.TaxAmount = Convert.ToInt32((contractsVM.AdvancePayValue * Convert.ToDecimal(Convert.ToDecimal(ValueAdded_V / 100))));
                            obj.TotalAmount = Convert.ToInt32((Convert.ToInt32(contractsVM.AdvancePayValue) + (contractsVM.AdvancePayValue * Convert.ToDecimal(Convert.ToDecimal(ValueAdded_V / 100)))));//obj.Amount + obj.TaxAmount;


                            break;
                        default:
                            break;
                    }
                    PaymentList.Add(obj);

                }
                else if (contractsVM.MonthlyPayValue != contractsVM.LastPayValue && i == contractsVM.PaymentsCount + 1)
                {
                    if (Convert.ToInt32(contractsVM.LastPayValue) != 0)
                    {
                        switch (contractsVM.TaxType)
                        {
                            case 1: // no tax
                                obj.Amount = contractsVM.LastPayValue;
                                obj.TaxAmount = 0;
                                obj.TotalAmount = contractsVM.LastPayValue;
                                break;
                            case 3: //with tax
                                    //obj.Amount = Convert.ToInt32(contractsVM.LastPayValue)- Convert.ToInt32((contractsVM.LastPayValue - (contractsVM.LastPayValue / Convert.ToDecimal(1.15))));
                                    //obj.TaxAmount = Convert.ToInt32((contractsVM.LastPayValue - (contractsVM.LastPayValue / Convert.ToDecimal(1.15))));
                                    //obj.TotalAmount = Convert.ToInt32(contractsVM.LastPayValue);


                                obj.Amount = Convert.ToInt32(contractsVM.LastPayValue) - Convert.ToInt32((contractsVM.LastPayValue - (contractsVM.LastPayValue / Convert.ToDecimal(Convert.ToDecimal(ValueAdded_V / 100) + 1))));
                                obj.TaxAmount = Convert.ToInt32((contractsVM.LastPayValue - (contractsVM.LastPayValue / Convert.ToDecimal(Convert.ToDecimal(ValueAdded_V / 100) + 1))));
                                obj.TotalAmount = Convert.ToInt32(contractsVM.LastPayValue);

                                break;
                            case 2: //without tax 

                                //obj.Amount = Convert.ToInt32(contractsVM.LastPayValue);
                                //obj.TaxAmount = Convert.ToInt32((contractsVM.LastPayValue * Convert.ToDecimal(0.15)));
                                //obj.TotalAmount = Convert.ToInt32((Convert.ToInt32(contractsVM.LastPayValue) + (contractsVM.LastPayValue * Convert.ToDecimal(0.15))));//obj.Amount + obj.TaxAmount;


                                obj.Amount = Convert.ToInt32(contractsVM.LastPayValue);
                                obj.TaxAmount = Convert.ToInt32((contractsVM.LastPayValue * Convert.ToDecimal(Convert.ToDecimal(ValueAdded_V / 100))));
                                obj.TotalAmount = Convert.ToInt32((Convert.ToInt32(contractsVM.LastPayValue) + (contractsVM.LastPayValue * Convert.ToDecimal(Convert.ToDecimal(ValueAdded_V / 100)))));//obj.Amount + obj.TaxAmount;

                                break;
                            default:
                                break;
                        }

                        PaymentList.Add(obj);

                    }
                }
                else
                {
                    switch (contractsVM.TaxType)
                    {
                        case 1: // no tax
                            obj.Amount = contractsVM.MonthlyPayValue;
                            obj.TaxAmount = 0;
                            obj.TotalAmount = contractsVM.MonthlyPayValue;
                            break;
                        case 3: //with tax
                            //obj.Amount = Convert.ToInt32(contractsVM.MonthlyPayValue) - Convert.ToInt32(contractsVM.MonthlyPayValue - (contractsVM.MonthlyPayValue / Convert.ToDecimal(1.15)));
                            //obj.TaxAmount = Convert.ToInt32(contractsVM.MonthlyPayValue - (contractsVM.MonthlyPayValue / Convert.ToDecimal(1.15)));
                            //obj.TotalAmount = Convert.ToInt32(contractsVM.MonthlyPayValue);


                            obj.Amount = Convert.ToInt32(contractsVM.MonthlyPayValue) - Convert.ToInt32(contractsVM.MonthlyPayValue - (contractsVM.MonthlyPayValue / Convert.ToDecimal(Convert.ToDecimal(ValueAdded_V / 100) + 1)));
                            obj.TaxAmount = Convert.ToInt32(contractsVM.MonthlyPayValue - (contractsVM.MonthlyPayValue / Convert.ToDecimal(Convert.ToDecimal(ValueAdded_V / 100) + 1)));
                            obj.TotalAmount = Convert.ToInt32(contractsVM.MonthlyPayValue);

                            break;
                        case 2: //without tax 


                            //obj.Amount = Convert.ToInt32(contractsVM.MonthlyPayValue);
                            //obj.TaxAmount = Convert.ToInt32((contractsVM.MonthlyPayValue * Convert.ToDecimal(0.15)));
                            //obj.TotalAmount = Convert.ToInt32((Convert.ToInt32(contractsVM.MonthlyPayValue) + (contractsVM.MonthlyPayValue * Convert.ToDecimal(0.15))));//obj.Amount + obj.TaxAmount;
                            obj.Amount = Convert.ToInt32(contractsVM.MonthlyPayValue);
                            obj.TaxAmount = Convert.ToInt32((contractsVM.MonthlyPayValue * Convert.ToDecimal(Convert.ToDecimal(ValueAdded_V / 100))));
                            obj.TotalAmount = Convert.ToInt32((Convert.ToInt32(contractsVM.MonthlyPayValue) + (contractsVM.MonthlyPayValue * Convert.ToDecimal(Convert.ToDecimal(ValueAdded_V / 100)))));//obj.Amount + obj.TaxAmount;


                            break;
                        default:
                            break;
                    }
                    PaymentList.Add(obj);

                }

            }
            return PaymentList;
        }
        public string ConvertDateCalendar(DateTime DateConv, string Calendar, string DateLangCulture)
        {
            //DateTimeFormatInfo DTFormat;
            //DateTimeFormatInfo DTFormat2;
            DateLangCulture = DateLangCulture.ToLower();
            /// We can't have the hijri date writen in English. We will get a runtime error


            //string Da = DateConv.ToString("yyyy-MM-dd");
            //if (Calendar == "Hijri" && DateLangCulture.StartsWith("en-"))
            //{

            //}
            //DateLangCulture = "ar-sa";
            ///// Set the date time format to the given culture
            //DTFormat = new System.Globalization.CultureInfo(DateLangCulture, false).DateTimeFormat;
            //DTFormat2 = new System.Globalization.CultureInfo(DateLangCulture, false).DateTimeFormat;
            ///// Set the calendar property of the date time format to the given calendar
            //switch (Calendar)
            //{
            //    case "Hijri":
            //        DTFormat.Calendar = new System.Globalization.HijriCalendar();
            //        DTFormat2.Calendar = new System.Globalization.HijriCalendar();
            //        break;
            //    case "Gregorian":
            //        DTFormat.Calendar = new System.Globalization.GregorianCalendar();
            //        DTFormat2.Calendar = new System.Globalization.GregorianCalendar();
            //        break;
            //    default:
            //        return "";
            //}


            string formattedDate = DateConv.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            string DAtee = GregToHijri2(formattedDate);


            //DTFormat2.ShortDatePattern = "dd/MM/yyyy";
            //var x=DateConv.Date.ToString("f", DTFormat2);

            ///// We format the date structure to whatever we want
            //DTFormat.ShortDatePattern = @"yyyy/MM/dd";
            //DTFormat.DateSeparator = "-";
            //var ss = DateConv.Date.ToString("d", DTFormat);
            return (DAtee);

            //return (DateConv.Date.ToString("d", DTFormat));
        }

        public HijriDateFormat ConvertDateCalendar3(DateTime DateConv, string Calendar, string DateLangCulture)
        {
            //DateTimeFormatInfo DTFormat;
            //DateTimeFormatInfo DTFormat2;
            DateLangCulture = DateLangCulture.ToLower();
            /// We can't have the hijri date writen in English. We will get a runtime error


            //string Da = DateConv.ToString("yyyy-MM-dd");
            //if (Calendar == "Hijri" && DateLangCulture.StartsWith("en-"))
            //{

            //}
            //DateLangCulture = "ar-sa";
            ///// Set the date time format to the given culture
            //DTFormat = new System.Globalization.CultureInfo(DateLangCulture, false).DateTimeFormat;
            //DTFormat2 = new System.Globalization.CultureInfo(DateLangCulture, false).DateTimeFormat;
            ///// Set the calendar property of the date time format to the given calendar
            //switch (Calendar)
            //{
            //    case "Hijri":
            //        DTFormat.Calendar = new System.Globalization.HijriCalendar();
            //        DTFormat2.Calendar = new System.Globalization.HijriCalendar();
            //        break;
            //    case "Gregorian":
            //        DTFormat.Calendar = new System.Globalization.GregorianCalendar();
            //        DTFormat2.Calendar = new System.Globalization.GregorianCalendar();
            //        break;
            //    default:
            //        return "";
            //}


            string formattedDate = DateConv.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);

            //string DAtee = GregToHijri2(formattedDate);
            CultureInfo arCul = new CultureInfo("ar-SA");
            CultureInfo enCul = new CultureInfo("en");
            DateTime tempDate = DateTime.ParseExact(formattedDate, allFormats, enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);

            HijriDateFormat hijriDateFormat = new HijriDateFormat();
            hijriDateFormat.Year =Convert.ToInt32( tempDate.ToString("yyyy", arCul.DateTimeFormat));
            hijriDateFormat.Month = Convert.ToInt32(tempDate.ToString("MM", arCul.DateTimeFormat));
            hijriDateFormat.Day = Convert.ToInt32(tempDate.ToString("dd", arCul.DateTimeFormat));
            //DTFormat2.ShortDatePattern = "dd/MM/yyyy";
            //var x=DateConv.Date.ToString("f", DTFormat2);

            ///// We format the date structure to whatever we want
            //DTFormat.ShortDatePattern = @"yyyy/MM/dd";
            //DTFormat.DateSeparator = "-";
            //var ss = DateConv.Date.ToString("d", DTFormat);
            return (hijriDateFormat);

            //return (DateConv.Date.ToString("d", DTFormat));
        }

        private string[] allFormats ={"yyyy/MM/dd","yyyy/M/d",
           "dd/MM/yyyy","d/M/yyyy",
            "dd/M/yyyy","d/MM/yyyy","yyyy-MM-dd",
            "yyyy-M-d","dd-MM-yyyy","d-M-yyyy",
            "dd-M-yyyy","d-MM-yyyy","yyyy MM dd",
            "yyyy M d","dd MM yyyy","d M yyyy",
            "dd M yyyy","d MM yyyy"
        };

        public string GregToHijri2(string Greg)
        {
            CultureInfo arCul = new CultureInfo("ar-SA");
            CultureInfo enCul = new CultureInfo("en");
            DateTime tempDate = DateTime.ParseExact(Greg, allFormats, enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
            
            return tempDate.ToString("yyyy-MM-dd", arCul.DateTimeFormat);
        }
        public string GregToHijri3(string Greg)
        {
            CultureInfo arCul = new CultureInfo("ar-SA");
            CultureInfo enCul = new CultureInfo("en");
            DateTime tempDate = DateTime.ParseExact(Greg, allFormats, enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
            return tempDate.ToString("dd/M/yyyy", arCul.DateTimeFormat);
        }

        public string ConvertDateCalendar2(DateTime DateConv, string Calendar, string DateLangCulture)
        {
            System.Globalization.DateTimeFormatInfo DTFormat;
            DateLangCulture = DateLangCulture.ToLower();
            /// We can't have the hijri date writen in English. We will get a runtime error - LAITH - 11/13/2005 1:01:45 PM -

            if (Calendar == "Hijri" && DateLangCulture.StartsWith("en-"))
            {
                DateLangCulture = "ar-sa";
            }

            /// Set the date time format to the given culture - LAITH - 11/13/2005 1:04:22 PM -
            DTFormat = new System.Globalization.CultureInfo(DateLangCulture, false).DateTimeFormat;

            /// Set the calendar property of the date time format to the given calendar - LAITH - 11/13/2005 1:04:52 PM -
            switch (Calendar)
            {
                case "Hijri":
                    DTFormat.Calendar = new System.Globalization.HijriCalendar();
                    break;

                case "Gregorian":
                    DTFormat.Calendar = new System.Globalization.GregorianCalendar();
                    break;

                default:
                    return "";
            }

            /// We format the date structure to whatever we want - LAITH - 11/13/2005 1:05:39 PM -
            DTFormat.ShortDatePattern = "dd/MM/yyyy";
            return (DateConv.Date.ToString("f", DTFormat));
        }
        public DateTime HijriToGreg(string hijri)
        {
            CultureInfo arCul = new CultureInfo("ar-SA");
            HijriCalendar h = new HijriCalendar();
            arCul.DateTimeFormat.Calendar = h;
            DateTime tempDate = DateTime.ParseExact(hijri, "yyyy-MM-dd", arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
            return tempDate;
        }

        public string GenerateContractNumber(int BranchId)
        {

            // return _ContractRepository.GenerateNextContractNumber( yearid, BranchId);

            var OrganzationId = _BranchesRepository.GetById(BranchId).OrganizationId;
            var sysSetting = _SystemSettingsRepository.GetSystemSettingsByBranchId(OrganzationId).Result;
            var codePrefix = "";
            if (sysSetting != null && sysSetting.ContractGenerateCode != null)
            {
                codePrefix = sysSetting.ContractGenerateCode;
            }
            return (codePrefix + _ContractRepository.GenerateNextContractNumber(BranchId, codePrefix).Result.ToString());
        }
        public string GenerateContractNumber2(int BranchId)
        {

            // return _ContractRepository.GenerateNextContractNumber( yearid, BranchId);

            var OrganzationId = _BranchesRepository.GetById(BranchId).OrganizationId;
            var sysSetting = _SystemSettingsRepository.GetSystemSettingsByBranchId(OrganzationId).Result;
            var codePrefix = "";
            if (sysSetting != null && sysSetting.ContractGenerateCode2 != null)
            {
                codePrefix = sysSetting.ContractGenerateCode2;
            }
            return (codePrefix + _ContractRepository.GenerateNextContractNumber2(BranchId, codePrefix).Result.ToString());
        }

        public string GenerateContractNumber3(int BranchId)
        {

            // return _ContractRepository.GenerateNextContractNumber( yearid, BranchId);

            var OrganzationId = _BranchesRepository.GetById(BranchId).OrganizationId;
            var sysSetting = _SystemSettingsRepository.GetSystemSettingsByBranchId(OrganzationId).Result;
            var codePrefix = "";
            if (sysSetting != null && sysSetting.Contract_Con_Code != null)
            {
                codePrefix = sysSetting.Contract_Con_Code;
            }
            return (codePrefix + _ContractRepository.GenerateNextContractNumber3(BranchId, codePrefix).Result.ToString());
        }

        public string GenerateContractNumber4(int BranchId)
        {

            // return _ContractRepository.GenerateNextContractNumber( yearid, BranchId);

            var OrganzationId = _BranchesRepository.GetById(BranchId).OrganizationId;
            var sysSetting = _SystemSettingsRepository.GetSystemSettingsByBranchId(OrganzationId).Result;
            var codePrefix = "";
            if (sysSetting != null && sysSetting.Contract_Sup_Code != null)
            {
                codePrefix = sysSetting.Contract_Sup_Code;
            }
            return (codePrefix + _ContractRepository.GenerateNextContractNumber4(BranchId, codePrefix).Result.ToString());
        }

        public string GenerateContractNumber5(int BranchId)
        {

            // return _ContractRepository.GenerateNextContractNumber( yearid, BranchId);

            var OrganzationId = _BranchesRepository.GetById(BranchId).OrganizationId;
            var sysSetting = _SystemSettingsRepository.GetSystemSettingsByBranchId(OrganzationId).Result;
            var codePrefix = "";
            if (sysSetting != null && sysSetting.Contract_Des_Code != null)
            {
                codePrefix = sysSetting.Contract_Des_Code;
            }
            return (codePrefix + _ContractRepository.GenerateNextContractNumber5(BranchId, codePrefix).Result.ToString());
        }


        public GeneralMessage EditContractService(Contracts contract, int UserId, int BranchId, int? yearid)
        {
            try
            {
                if (yearid == null) 
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote4 = "فشل في حفظ العقد";
                    _SystemAction.SaveAction("EditContractService", "ContractService", 1, "فشل في الحفظ ,تأكد من اختيار السنة الماليه أولا", "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }



                //var contractPayments = _customerPaymentsRepository.GetMatching(s => s.ContractId == contract.ContractId && s.IsPaid == false);
                //var contractPaymentsPaid = _customerPaymentsRepository.GetMatching(s => s.ContractId == contract.ContractId && s.IsPaid == true);

                //if (contractPaymentsPaid.Count() > 0)
                //    foreach (var loop in contractPaymentsPaid)
                //    {
                //        var InvoiceCust = _invoicesRepository.GetMatching(s => s.IsDeleted == false && s.InvoiceId == loop.InvoiceId && s.Rad == false);
                //        if (InvoiceCust.Count() > 0)
                //        {
                //            //-----------------------------------------------------------------------------------------------------------------
                //            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //            string ActionNote2 = " فشل في تعديل العقد  ";
                //            SaveAction("EditContractService", "ContractService", 3, "لايمكن تعديل العقد هناك فواتير تم اصدارها من دفعات يجب الغاؤها اولا", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //            //-----------------------------------------------------------------------------------------------------------------
                //            return new GeneralMessage { Result = false, Message = "لايمكن تعديل العقد هناك فواتير تم اصدارها من دفعات يجب الغاؤها اولا" };
                //        }
                //    }
                //if (contractPayments.Count() > 0)
                //{
                //    //-----------------------------------------------------------------------------------------------------------------
                //    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //    string ActionNote2 = " فشل في تعديل العقد  ";
                //    SaveAction("EditContractService", "ContractService", 3, "لايمكن تعديل العقد هناك دفعات لم يتم تسديدها", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //    //-----------------------------------------------------------------------------------------------------------------
                //    return new GeneralMessage { Result = false, Message = "لايمكن تعديل العقد هناك دفعات لم يتم تسديدها" };
                //}






                var ContractUpdated = _ContractRepository.GetById(contract.ContractId);
                if (ContractUpdated != null)
                {
                    ContractUpdated.Value = contract.Value;
                    ContractUpdated.TaxType = contract.TaxType;

                    ContractUpdated.ValueText = contract.ValueText;
                    ContractUpdated.TaxesValue = contract.TaxesValue;
                    ContractUpdated.TotalValue = contract.TotalValue;
                    ContractUpdated.UpdateUser = UserId;
                    ContractUpdated.UpdateDate = DateTime.Now;
                    int serv = 0;
                    try
                    {
                        serv = contract.ContractServices.FirstOrDefault().ServiceId ?? 0;
                    }
                    catch (Exception)
                    {

                        serv = 0;
                    }
                    ContractUpdated.ServiceId = serv;
                    decimal TotalValuePayment = 0;
                    if (ContractUpdated.CustomerPayments != null && ContractUpdated.CustomerPayments.Count > 0)
                    {
                        foreach (var item in ContractUpdated.CustomerPayments.ToList())
                        {
                            TotalValuePayment += item.Amount;
                        }
                    }

                    if (contract.Value < TotalValuePayment)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate22 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote22 = "فشل في حفظ العقد";
                        _SystemAction.SaveAction("EditContractService", "ContractService", 1, "فشل في الحفظ ,قيمة العقد اقل من قيمة الدفعات", "", "", ActionDate22, UserId, BranchId, ActionNote22, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = "فشل في الحفظ ,قيمة العقد اقل من قيمة الدفعات" };
                    }

                    //remove transaction 
                    var ConTra = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.ContractId == contract.ContractId).ToList();
                    if (ConTra.Count() > 0)
                    {
                        _TaamerProContext.Transactions.RemoveRange(ConTra);

                    }

                    // add transactions
                    //depit 
                    var depitTrans = new Transactions();
                    depitTrans.TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    depitTrans.TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                    depitTrans.ContractId = contract.ContractId;
                    /////////////////////////////////////////////
                    var CustAccId = _customerRepository.GetById(ContractUpdated.CustomerId??0).AccountId;
                    if (CustAccId != null)
                    {
                        depitTrans.AccountId = CustAccId;//contract.Customer.AccountId;
                    }
                    else
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate23 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote23 = "فشل في حفظ العقد";
                       _SystemAction.SaveAction("EditContractService", "ContractService", 1, "فشل في الحفظ ,تأكد من وجود حساب للعميل", "", "", ActionDate23, UserId, BranchId, ActionNote23, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                    }
                    depitTrans.AccountType = _AccountsRepository.GetById((int)depitTrans.AccountId).Type;
                    depitTrans.CustomerId = ContractUpdated.CustomerId;
                    depitTrans.Type = 12;
                    depitTrans.LineNumber = 1;
                    depitTrans.Depit = contract.Value; depitTrans.Credit = 0;
                    depitTrans.YearId = yearid;
                    depitTrans.Notes = depitTrans.Details = _customerRepository.GetById(ContractUpdated.CustomerId??0).CustomerNameAr + " للعميل " + ContractUpdated.ContractNo + "عقد رقم";
                    depitTrans.InvoiceReference = ContractUpdated.ContractNo;
                    depitTrans.IsPost = true;
                    depitTrans.AddDate = DateTime.Now;
                    depitTrans.AddUser = UserId;
                    depitTrans.IsDeleted = false;
                    _TaamerProContext.Transactions.Add(depitTrans);

                    //credit 
                    var creditTrans = new Transactions();
                    creditTrans.TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    creditTrans.TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                    creditTrans.ContractId = contract.ContractId;
                    try
                    {
                        var contarctBranchAcc = _BranchesRepository.GetById(BranchId).ContractsAccId;
                        if (contarctBranchAcc == null)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote3 = "فشل في حفظ العقد";
                           _SystemAction.SaveAction("EditContractService", "ContractService", 1, "فشل في الحفظ ,تأكد من ربط حساب العقود بالفرع", "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                            //-----------------------------------------------------------------------------------------------------------------

                            return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                        }
                        creditTrans.AccountId = contarctBranchAcc;
                    }
                    catch (Exception)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote4 = "فشل في حفظ العقد";
                       _SystemAction.SaveAction("EditContractService", "ContractService", 1, "فشل في الحفظ ,تأكد من ربط حساب العقود بالفرع", "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedFailed };
                    }
                    creditTrans.AccountType = _AccountsRepository.GetById((int)creditTrans.AccountId).Type;
                    creditTrans.CustomerId = ContractUpdated.CustomerId;
                    creditTrans.Type = 12;
                    creditTrans.LineNumber = 2;
                    creditTrans.Credit = contract.Value; creditTrans.Depit = 0;
                    creditTrans.YearId = yearid;
                    creditTrans.Notes = creditTrans.Details = _customerRepository.GetById(ContractUpdated.CustomerId??0).CustomerNameAr + " للعميل " + ContractUpdated.ContractNo + "عقد رقم ";
                    creditTrans.InvoiceReference = ContractUpdated.ContractNo;
                    creditTrans.IsPost = true;
                    creditTrans.AddDate = DateTime.Now;
                    creditTrans.AddUser = UserId;
                    creditTrans.IsDeleted = false;
                    _TaamerProContext.Transactions.Add(creditTrans);

                }



                var conservice = _TaamerProContext.ContractServices.Where(s => s.IsDeleted == false && s.ContractId == contract.ContractId).ToList();
                if (conservice.Count() > 0)
                {
                    _TaamerProContext.ContractServices.RemoveRange(conservice);

                }

                if(contract.ContractServices.Count() > 0)
                {
                    foreach (var item in contract.ContractServices)
                    {
                        item.AddDate = DateTime.Now;
                        item.AddUser = UserId;
                        item.BranchId = BranchId;
                        item.ContractId = contract.ContractId;
                        _TaamerProContext.ContractServices.Add(item);
                    }
                }


                //remove prev ServicesPriceOffer
                var OldDataDetails = _TaamerProContext.Acc_Services_PriceOffer.Where(s => s.ContractId == contract.ContractId).ToList();
                if (OldDataDetails.Count() > 0)
                {
                    _TaamerProContext.Acc_Services_PriceOffer.RemoveRange(OldDataDetails);
                }
                if (contract.ServicesPriceOffer != null && contract.ServicesPriceOffer.Count > 0)
                {
                    foreach (var item in contract.ServicesPriceOffer)
                    {
                        item.AddUser = UserId;
                        item.AddDate = DateTime.Now;
                        item.ContractId = contract.ContractId;
                        _TaamerProContext.Acc_Services_PriceOffer.Add(item);
                        _TaamerProContext.Acc_Services_PriceOffer.Add(item);
                    }
                }
                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تعديل عقد رقم " + contract.ContractId;
               _SystemAction.SaveAction("EditContractService", "ContractService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase =  Resources.General_EditedSuccessfully , ReturnedStr = contract.ContractId.ToString() };


            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ العقد";
               _SystemAction.SaveAction("EditContractService", "ContractService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }


    }
    public  class HijriDateFormat
    {
        public int? Year { set; get; }
        public int? Month { get; set; }
        public int? Day { get; set; }
    }

}

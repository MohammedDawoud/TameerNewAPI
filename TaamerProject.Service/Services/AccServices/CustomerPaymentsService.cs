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
    public class CustomerPaymentsService : ICustomerPaymentsService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly ICustomerPaymentsRepository _CustomerPaymentsRepository;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly IInvoicesRepository _InvoicesRepositoryRepository;
        private readonly IAccountsRepository _AccountsRepository;

        public CustomerPaymentsService(TaamerProjectContext dataContext, ISystemAction systemAction, ICustomerPaymentsRepository customerPaymentsRepository, 
            IBranchesRepository branchesRepository, IInvoicesRepository invoicesRepositoryRepository, IAccountsRepository accountsRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _CustomerPaymentsRepository = customerPaymentsRepository;
            _BranchesRepository = branchesRepository;
            _InvoicesRepositoryRepository = invoicesRepositoryRepository;
            _AccountsRepository = accountsRepository;
        }
        public async Task<IEnumerable<CustomerPaymentsVM>> GetAllCustomerPayments(int ContractId)
        {
            var customerPayments =await _CustomerPaymentsRepository.GetAllCustomerPayments(ContractId);
            return customerPayments;
        }
        public async Task<IEnumerable<CustomerPaymentsVM>> GetAllCustomerPaymentsIsNotCanceled(int ContractId)
        {
            var customerPayments = await _CustomerPaymentsRepository.GetAllCustomerPaymentsIsNotCanceled(ContractId);
            return customerPayments;
        }
        public IEnumerable<CustomerPaymentsVM> GetAllCustomerPaymentsPaid(int ContractId)
        {
            var customerPayments = _CustomerPaymentsRepository.GetAllCustomerPayments(ContractId).Result.Where(s => s.IsPaid == true);
            return customerPayments;
        }
        public async Task<IEnumerable<CustomerPaymentsVM>> GetAllCustomerPaymentsboffer(int offerid)
        {
            var customerPayments =await _CustomerPaymentsRepository.GetAllCustomerPaymentsbyofferis(offerid);
            return customerPayments;
        }

        public async Task<IEnumerable<CustomerPaymentsVM>> GetAllCustomerPaymentsbyamonttxt(decimal amount, string txt)
        {
            var customerPayments = await _CustomerPaymentsRepository.GetAllCustomerPaymentsbyamounttxt(amount, txt);
            return customerPayments;
        }
        public async Task<IEnumerable<CustomerPaymentsVM>> GetAllCustomerPaymentsconst(int BranchId)
        {
            var customerPayments = await _CustomerPaymentsRepository.GetAllCustomerPaymentsconst(BranchId);
            return customerPayments;
        }

        public GeneralMessage SaveCustomerPayment(CustomerPayments customerPayments, int UserId, int BranchId)
        {
            try
            {
                Utilities util = new Utilities(customerPayments.TotalAmount.ToString());
                try
                {
                    customerPayments.AmountValueText = util.GetNumberAr();
                }
                catch (Exception)
                {
                    customerPayments.AmountValueText = customerPayments.Amount.ToString();
                }
                if (customerPayments.PaymentId == 0)
                {
                    customerPayments.BranchId = BranchId;
                    customerPayments.AddUser = UserId;
                    customerPayments.AddDate = DateTime.Now;
                    _TaamerProContext.CustomerPayments.Add(customerPayments);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "حفظ دفعة عميل";
                   _SystemAction.SaveAction("SaveCustomerPayment", "CustomerPaymentsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var CustomerPaymentsUpdated = _CustomerPaymentsRepository.GetById(customerPayments.PaymentId);
                    if (CustomerPaymentsUpdated != null && CustomerPaymentsUpdated.IsPaid == false)
                    {
                        CustomerPaymentsUpdated.PaymentNo = customerPayments.PaymentNo;
                        CustomerPaymentsUpdated.ContractId = customerPayments.ContractId;
                        CustomerPaymentsUpdated.PaymentDate = customerPayments.PaymentDate;
                        CustomerPaymentsUpdated.PaymentDateHijri = customerPayments.PaymentDateHijri;
                        CustomerPaymentsUpdated.Amount = customerPayments.Amount;
                        CustomerPaymentsUpdated.AmountValueText = customerPayments.AmountValueText;
                        CustomerPaymentsUpdated.TaxAmount = customerPayments.TaxAmount;
                        CustomerPaymentsUpdated.TotalAmount = customerPayments.TotalAmount;
                        CustomerPaymentsUpdated.ServiceId = customerPayments.ServiceId;

                        CustomerPaymentsUpdated.UpdateUser = UserId;
                        CustomerPaymentsUpdated.UpdateDate = DateTime.Now;

                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " تعديل دفعة عميل رقم " + customerPayments.PaymentId;
                        _SystemAction.SaveAction("SaveCustomerPayment", "CustomerPaymentsService", 2, Resources.General_EditedSuccessfully , "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };
                    }
                    else
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "فشل في حفظ دفعة العميل ";
                        _SystemAction.SaveAction("SaveCustomerPayment", "CustomerPaymentsService", 1, Resources.Failed_to_save_paid, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Failed_to_save_paid };
                    }
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ دفعة العميل";
                _SystemAction.SaveAction("SaveCustomerPayment", "CustomerPaymentsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage PayPayment(CustomerPayments customerPayments, int UserId, int BranchId, int? yearid)
        {
            try
            {
                //get accid
                //get accid
                //return msg  if null
                var BoxAccId = _BranchesRepository.GetCashBoxViaBranchID(BranchId).Result;
                if (BoxAccId == null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote3 = "فشل في دفع دفعة عميل";
                   _SystemAction.SaveAction("PayPayment", "CustomerPaymentsService", 1, Resources.Failed_to_save_account_to_the_branch, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.Failed_to_save_account_to_the_branch };
                }


                //var year = _fiscalyearsRepository.GetCurrentYear();
                if (yearid == null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "فشل في دفع دفعة عميل";
                    _SystemAction.SaveAction("PayPayment", "CustomerPaymentsService", 1, Resources.Failed_to_pay, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Failed_to_pay };
                }
                var CustomerPaymentsUpdated = _CustomerPaymentsRepository.GetById(customerPayments.PaymentId);
                if (CustomerPaymentsUpdated != null)
                {
                    CustomerPaymentsUpdated.ToAccountId = BoxAccId.BoxAccId;// accid
                    CustomerPaymentsUpdated.IsPaid = true;
                    CustomerPaymentsUpdated.InvoiceId = _InvoicesRepositoryRepository.GetMaxId().Result + 1;
                    CustomerPaymentsUpdated.UpdateUser = UserId;
                    CustomerPaymentsUpdated.UpdateDate = DateTime.Now;

                    // Add receipt voucher 
                    var receiptVoucher = new Invoices();
                    var Value = _InvoicesRepositoryRepository.GenerateNextInvoiceNumber(6, yearid, BranchId).ToString();
                    var NewValue = string.Format("{0:000000}", Value);
                    receiptVoucher.InvoiceNumber = NewValue;
                    receiptVoucher.Type = 6;
                    receiptVoucher.Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    receiptVoucher.HijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                    receiptVoucher.Notes = CustomerPaymentsUpdated.Contracts.Customer.CustomerNameAr + " دفعة عقد رقم " + CustomerPaymentsUpdated.Contracts.ContractNo + "  للعميل";
                    receiptVoucher.InvoiceValue = CustomerPaymentsUpdated.Amount;
                    receiptVoucher.TotalValue = CustomerPaymentsUpdated.Amount;
                    receiptVoucher.IsPost = true;
                    receiptVoucher.PostDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    receiptVoucher.PostHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                    receiptVoucher.YearId = yearid;
                    receiptVoucher.ProjectId = CustomerPaymentsUpdated.Contracts.ProjectId;
                    receiptVoucher.CustomerId = CustomerPaymentsUpdated.Contracts.CustomerId;
                    receiptVoucher.IsTax = true;
                    receiptVoucher.TaxAmount = CustomerPaymentsUpdated.TaxAmount;
                    receiptVoucher.VoucherType = 1; //نقدي
                    receiptVoucher.BranchId = BranchId;
                    receiptVoucher.AddUser = UserId;
                    receiptVoucher.AddDate = DateTime.Now;
                    receiptVoucher.IsDeleted = false;
                    Utilities util = new Utilities(CustomerPaymentsUpdated.Amount.ToString());
                    try
                    {
                        receiptVoucher.InvoiceValueText = util.GetNumberAr();
                    }
                    catch (Exception)
                    {
                        receiptVoucher.InvoiceValueText = CustomerPaymentsUpdated.Amount.ToString();
                    }
                    _TaamerProContext.Invoices.Add(receiptVoucher);
                    // add transactions
                    //depit 
                    var depitTrans = new Transactions();
                    depitTrans.TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    depitTrans.TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                    //depitTrans.InvoiceId = CustomerPaymentsUpdated.InvoiceId;
                    depitTrans.ContractId = CustomerPaymentsUpdated.ContractId;
                    depitTrans.PaymentId = CustomerPaymentsUpdated.PaymentId;
                    depitTrans.AccountId = BoxAccId.BoxAccId;
                    depitTrans.AccountType = _AccountsRepository.GetById((int)depitTrans.AccountId).Type;
                    depitTrans.CustomerId = CustomerPaymentsUpdated.Contracts.CustomerId;
                    depitTrans.Type = 18;
                    depitTrans.LineNumber = 1;
                    depitTrans.Depit = CustomerPaymentsUpdated.Amount; depitTrans.Credit = 0;
                    depitTrans.YearId = yearid;
                    depitTrans.Notes = depitTrans.Details = CustomerPaymentsUpdated.Contracts.Customer.CustomerNameAr + " دفعة عقد رقم " + CustomerPaymentsUpdated.Contracts.ContractNo + "  للعميل";
                    depitTrans.InvoiceReference = CustomerPaymentsUpdated.Contracts.ContractNo;
                    depitTrans.IsPost = true;
                    depitTrans.BranchId = BranchId;
                    depitTrans.AddDate = DateTime.Now;
                    depitTrans.AddUser = UserId;
                    depitTrans.IsDeleted = false;
                    _TaamerProContext.Transactions.Add(depitTrans);

                    //credit 
                    var creditTrans = new Transactions();
                    creditTrans.TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    creditTrans.TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                    //creditTrans.InvoiceId = CustomerPaymentsUpdated.InvoiceId;
                    creditTrans.ContractId = CustomerPaymentsUpdated.ContractId;
                    creditTrans.PaymentId = CustomerPaymentsUpdated.PaymentId;
                    creditTrans.AccountId = CustomerPaymentsUpdated.Contracts.Customer.AccountId;
                    creditTrans.AccountType = _AccountsRepository.GetById((int)creditTrans.AccountId).Type;
                    creditTrans.CustomerId = CustomerPaymentsUpdated.Contracts.CustomerId;
                    creditTrans.Type = 18;
                    creditTrans.LineNumber = 2;
                    creditTrans.Credit = CustomerPaymentsUpdated.Amount; creditTrans.Depit = 0;
                    creditTrans.YearId = yearid;
                    creditTrans.Notes = creditTrans.Details = CustomerPaymentsUpdated.Contracts.Customer.CustomerNameAr + " دفعة عقد رقم " + CustomerPaymentsUpdated.Contracts.ContractNo + "  للعميل";
                    creditTrans.InvoiceReference = CustomerPaymentsUpdated.Contracts.ContractNo;
                    creditTrans.IsPost = true;
                    creditTrans.BranchId = BranchId;
                    creditTrans.AddDate = DateTime.Now;
                    creditTrans.AddUser = UserId;
                    creditTrans.IsDeleted = false;
                    _TaamerProContext.Transactions.Add(creditTrans);

                    //taxs
                    if (CustomerPaymentsUpdated.TaxAmount > 0)
                    {
                        var creditTaxTrans = new Transactions();
                        creditTaxTrans.TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        creditTaxTrans.TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                        creditTaxTrans.ContractId = CustomerPaymentsUpdated.ContractId;
                        creditTaxTrans.PaymentId = CustomerPaymentsUpdated.PaymentId;
                        var BranchTaxAcc = _BranchesRepository.GetById(BranchId).TaxsAccId;
                        if (BranchTaxAcc != null)
                        {
                            creditTaxTrans.AccountId = BranchTaxAcc;
                        }
                        else
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote4 = "فشل في دفع دفعة لعميل";
                            _SystemAction.SaveAction("PayPayment", "CustomerPaymentsService", 1, Resources.Failed_to_save_branch, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                            //-----------------------------------------------------------------------------------------------------------------

                            return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Failed_to_save_branch };
                        }


                        creditTaxTrans.AccountType = _TaamerProContext.Accounts.Where(x=>x.AccountId==creditTaxTrans.AccountId).FirstOrDefault().Type;

                        creditTaxTrans.CustomerId = CustomerPaymentsUpdated.Contracts.CustomerId;
                        creditTaxTrans.Type = 17;
                        creditTaxTrans.LineNumber = 3;
                        creditTaxTrans.Credit = CustomerPaymentsUpdated.TaxAmount; creditTrans.Depit = 0;
                        creditTaxTrans.YearId = yearid;
                        creditTaxTrans.Notes = creditTaxTrans.Details = CustomerPaymentsUpdated.Contracts.Customer.CustomerNameAr + " دفعة عقد رقم " + CustomerPaymentsUpdated.Contracts.ContractNo + "  للعميل";
                        creditTaxTrans.InvoiceReference = CustomerPaymentsUpdated.Contracts.ContractNo;
                        creditTaxTrans.IsPost = true;
                        creditTaxTrans.BranchId = BranchId;
                        creditTaxTrans.AddDate = DateTime.Now;
                        creditTaxTrans.AddUser = UserId;
                        creditTaxTrans.IsDeleted = false;
                        _TaamerProContext.Transactions.Add(creditTaxTrans);
                    }

                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "دفع دفعة لعميل";
                _SystemAction.SaveAction("PayPayment", "CustomerPaymentsService", 1, Resources.Payment_completed, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.Payment_completed };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في دفع دفعة عميل";
                _SystemAction.SaveAction("PayPayment", "CustomerPaymentsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ex.Message }; //"فشل في عملية الدفع"
            }
        }
        public GeneralMessage DeleteCustomerPayment(int PaymentId, int UserId, int BranchId)
        {
            try
            {
                CustomerPayments customerPayments = _CustomerPaymentsRepository.GetById(PaymentId);
                if (customerPayments != null)
                {
                    _TaamerProContext.CustomerPayments.Remove(customerPayments);
                }
                var PaymentTransaction = _TaamerProContext.Transactions.Where(s => s.PaymentId == PaymentId);
                if (PaymentTransaction != null && PaymentTransaction.Count() > 0)
                {
                    _TaamerProContext.Transactions.RemoveRange(PaymentTransaction);

                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف دفعة عميل رقم " + PaymentId;
                _SystemAction.SaveAction("DeleteCustomerPayment", "CustomerPaymentsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف دفعة عميل رقم " + PaymentId; ;
                _SystemAction.SaveAction("DeleteCustomerPayment", "CustomerPaymentsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public GeneralMessage CancelPayment(int PaymentId, int UserId, int BranchId)
        {
            try
            {
                var CustomerPaymentsCancelled = _CustomerPaymentsRepository.GetById(PaymentId);
                if (CustomerPaymentsCancelled != null)
                {
                    //get voucer invoice 
                    var invoice = _TaamerProContext.Invoices.Where(x=>x.InvoiceId==CustomerPaymentsCancelled.InvoiceId).FirstOrDefault();
                    invoice.IsDeleted = true;
                    invoice.DeleteDate = DateTime.Now;
                    invoice.DeleteUser = UserId;

                    CustomerPaymentsCancelled.ToAccountId = null;
                    CustomerPaymentsCancelled.IsPaid = false;
                    CustomerPaymentsCancelled.InvoiceId = null;
                    CustomerPaymentsCancelled.UpdateUser = UserId;
                    CustomerPaymentsCancelled.UpdateDate = DateTime.Now;

                    var PaymentTransaction = _TaamerProContext.Transactions.Where(s => s.PaymentId == PaymentId);
                    if (PaymentTransaction != null && PaymentTransaction.Count() > 0)
                    {
                        _TaamerProContext.Transactions.RemoveRange(PaymentTransaction);
                    }
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "الغاء دفعة عميل";
                _SystemAction.SaveAction("CancelPayment", "CustomerPaymentsService", 1, Resources.Payment_has_been_cancelled, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.Payment_has_been_cancelled };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في الغاء دفعة عميل";
                _SystemAction.SaveAction("CancelPayment", "CustomerPaymentsService", 1, Resources.Payment_has_been_faild, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =Resources.Payment_has_been_faild };
            }
        }
        public int? GenerateCustPaymentNumber(int ContractId)
        {
            var payments = _TaamerProContext.CustomerPayments.Where(s => s.ContractId == ContractId).ToList();

            if (payments.Count() > 0)
            {
                return payments.Max(s => s.PaymentNo) + 1;
            }
            else
            {
                return 1;
            }
        }
        public PaymentReceiptVoucherVM GetPaymentReceipVoucher(int PaymentId, int BranchId)
        {
            var PaymentObj = _CustomerPaymentsRepository.GetById(PaymentId);
            var PaymentVoucher = _TaamerProContext.Invoices.Where(x=>x.InvoiceId==PaymentObj.InvoiceId).FirstOrDefault();
            var organizatin = _TaamerProContext.Organizations.Where(s => s.IsDeleted == false && s.BranchId == BranchId).FirstOrDefault();
            var customerPaidPayments = _CustomerPaymentsRepository.GetMatching(s => s.IsDeleted == false && s.ContractId == PaymentObj.ContractId && s.BranchId == BranchId && s.IsPaid == true);
            var remaining = _CustomerPaymentsRepository.GetMatching(s => s.IsDeleted == false && s.ContractId == PaymentObj.ContractId && s.BranchId == BranchId && s.IsPaid == false).Sum(s => s.Amount);
            try
            {
                return new PaymentReceiptVoucherVM
                {
                    VoucherNumber = PaymentVoucher.InvoiceNumber.ToString(),
                    ContractNumber = PaymentObj.Contracts.ContractNo,
                    CustomerName = PaymentVoucher.Customer.CustomerNameAr,
                    AccountName = PaymentObj.Accounts.NameAr,
                    ProjectNumber = PaymentVoucher.Project.ProjectNo,
                    VoucherAmount = PaymentVoucher.TotalValue,
                    VoucherDate = PaymentObj.PaymentDate,
                    VoucherDateHijri = PaymentObj.PaymentDateHijri,
                    VoucherDescription = PaymentVoucher.Notes,
                    TotalVoucherAmount = PaymentVoucher.TotalValue,
                    VoucherTaxAmount = PaymentVoucher.TaxAmount,
                    VoucherAmountValText = PaymentVoucher.InvoiceValueText,
                    RequiredAmount = PaymentVoucher.TotalValue - PaymentVoucher.TaxAmount,
                    TotalPiad = customerPaidPayments.Sum(s => s.Amount),
                    TotalRemaining = remaining,
                    PaidCustomerPayments = customerPaidPayments.Where(s => s.PaymentId != PaymentId).Select(x => new CustomerPaymentsVM
                    {
                        PaymentNo = x.PaymentNo,
                        PaymentDate = x.PaymentDate,
                        PaymentDateHijri = x.PaymentDateHijri,
                        Amount = x.Amount,
                        TaxAmount = x.TaxAmount,
                        TotalAmount = x.TotalAmount,
                        AccountName = x.Accounts.NameAr,
                    }).ToList(),
                    TaxCode = organizatin != null ? organizatin.TaxCode : "",
                    OrganizationLogoUrl = organizatin != null ? organizatin.LogoUrl : "",
                    OrganizationPhone = organizatin != null ? organizatin.Mobile : "",
                    OrganizationName = organizatin != null ? organizatin.NameAr : "",
                    OrganizationMail = organizatin != null ? organizatin.Email : "",
                    OrganizationWebSite = organizatin != null ? organizatin.WebSite : "",
                    OrganizationAddress = organizatin != null ? organizatin.Address : "",
                    OrganizationCityName = organizatin != null ? organizatin.City.NameAr : "",
                };
            }
            catch (Exception)
            {
                return new PaymentReceiptVoucherVM();
            }
        }
    }
}

using System.Globalization;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Service.IGeneric;
using System.Net;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class Acc_SuppliersService : IAcc_SuppliersService
    {

        private readonly IAcc_SuppliersRepository _Acc_SuppliersRepository;
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;
        private readonly IUserNotificationPrivilegesRepository _userNotificationPrivilegesRepository;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly IAccountsRepository _accountsRepository;
        private readonly IUserNotificationPrivilegesService _userNotificationPrivilegesService;
        private readonly ITransactionsRepository _TransactionsRepository;
        private readonly IInvoicesRepository _InvoicesRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;


        public Acc_SuppliersService(IAcc_SuppliersRepository acc_SuppliersRepository, ISys_SystemActionsRepository sys_SystemActionsRepository, IUserNotificationPrivilegesRepository userNotificationPrivilegesRepository,
            IBranchesRepository branchesRepository, IAccountsRepository accountsRepository, IUserNotificationPrivilegesService userNotificationPrivilegesService,
            ITransactionsRepository transactionsRepository, IInvoicesRepository invoicesRepository, TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _Acc_SuppliersRepository = acc_SuppliersRepository;
            _Sys_SystemActionsRepository = sys_SystemActionsRepository;
            _userNotificationPrivilegesRepository = userNotificationPrivilegesRepository;
            _BranchesRepository = branchesRepository;
            _accountsRepository = accountsRepository;
            _userNotificationPrivilegesService = userNotificationPrivilegesService;
            _TransactionsRepository = transactionsRepository;
            _InvoicesRepository = invoicesRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;

        }


        public async Task<IEnumerable<Acc_SuppliersVM>> GetAllSuppliers(string SearchText, int BranchId, int? yearid)
        {
            var Suppliers =await _Acc_SuppliersRepository.GetAllSuppliers(SearchText, BranchId, yearid);
            return Suppliers;
        }
        public IEnumerable<Acc_SuppliersVM> GetAllSuppliersAllNoti(string SearchText, string lang, int BranchId, int? yearid)
        {
            var AllSuppliersInvoices = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.YearId == yearid && (s.Type == 32 || s.Type == 33)).Where(w => !(w.SupplierId == null || w.SupplierId.Equals(""))).Select(s => s.SupplierId);
            var Supp = _Acc_SuppliersRepository.GetAllSuppliers(SearchText, BranchId, yearid).Result.Where(w => AllSuppliersInvoices.Contains(w.SupplierId));
            return Supp;
        }
        public async Task<Acc_SuppliersVM> GetSupplierByID(int SupplierId)
        {
            var Suppliers =await _Acc_SuppliersRepository.GetSupplierByID(SupplierId);
            return Suppliers;
        }
        public GeneralMessage SaveSupplier(Acc_Suppliers Supplier, int UserId, int BranchId)
        {
            try
            {

                if (Supplier.SupplierId == 0)
                {
                    Supplier.AddUser = UserId;
                    Supplier.AddDate = DateTime.Now;

                    var Branch = _BranchesRepository.GetById(BranchId);
                    if (Branch != null && Branch.SuppliersAccId != null)
                    {
                        var parentSuppAcc = _accountsRepository.GetById((int)Branch.SuppliersAccId);
                        var newSuppAcc = new Accounts();
                        var AccCode = _accountsRepository.GetNewCodeByParentId(Branch.SuppliersAccId ?? 0,3).Result;
                        newSuppAcc.Code = AccCode;

                        newSuppAcc.Classification = 2;
                        newSuppAcc.ParentId = parentSuppAcc.AccountId;
                        newSuppAcc.IsMain = false;
                        newSuppAcc.Level = parentSuppAcc.Level + 1;
                        newSuppAcc.Nature = 1; //depit
                        newSuppAcc.Halala = true;
                        newSuppAcc.NameAr = "حساب المورد" + "  " + Supplier.NameAr;
                        newSuppAcc.NameEn = Supplier.NameEn + " " + "Supplier Account";
                        newSuppAcc.Type = 2; //bugget
                        newSuppAcc.IsMainCustAcc = false;
                        newSuppAcc.Active = true;
                        newSuppAcc.AddUser = UserId;
                        newSuppAcc.BranchId = BranchId;
                        newSuppAcc.AddDate = DateTime.Now;

                        _TaamerProContext.Accounts.Add(newSuppAcc);
                        parentSuppAcc.IsMain = true; // update main acc
                        _TaamerProContext.SaveChanges();
                        Supplier.AccountId = newSuppAcc.AccountId;

                    }
                    else
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حفظ المورد";
                       _SystemAction.SaveAction("SaveSupplier", "Acc_SuppliersService", 1, Resources.Acc_SupplierMessageError, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.Acc_SupplierMessageError };
                    }






                    _TaamerProContext.Acc_Suppliers.Add(Supplier);

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة مورد جديد";
                    _SystemAction.SaveAction("SaveSupplier", "Acc_SuppliersService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    var SupplierUpdated = _Acc_SuppliersRepository.GetById(Supplier.SupplierId);
                    if (SupplierUpdated != null)
                    {
                        SupplierUpdated.NameAr = Supplier.NameAr;
                        SupplierUpdated.NameEn = Supplier.NameEn;
                        SupplierUpdated.TaxNo = Supplier.TaxNo;
                        SupplierUpdated.PhoneNo = Supplier.PhoneNo;
                        SupplierUpdated.UpdateUser = UserId;
                        SupplierUpdated.UpdateDate = DateTime.Now;


                        SupplierUpdated.CompAddress = Supplier.CompAddress;
                        SupplierUpdated.PostalCodeFinal = Supplier.PostalCodeFinal;
                        SupplierUpdated.ExternalPhone = Supplier.ExternalPhone;
                        SupplierUpdated.Country = Supplier.Country;
                        SupplierUpdated.Neighborhood = Supplier.Neighborhood;
                        SupplierUpdated.StreetName = Supplier.StreetName;
                        SupplierUpdated.BuildingNumber = Supplier.BuildingNumber;
                        SupplierUpdated.CityId = Supplier.CityId;


                        if (SupplierUpdated.AccountId == null)
                        {
                            var Branch = _BranchesRepository.GetById(BranchId);
                            if (Branch != null && Branch.SuppliersAccId != null)
                            {
                                var parentSuppAcc = _accountsRepository.GetById((int)Branch.SuppliersAccId);
                                var newSuppAcc = new Accounts();
                                var AccCode = _accountsRepository.GetNewCodeByParentId(Branch.SuppliersAccId ?? 0,3).Result;
                                newSuppAcc.Code = AccCode;

                                newSuppAcc.Classification = parentSuppAcc.Classification;
                                newSuppAcc.ParentId = parentSuppAcc.AccountId;
                                newSuppAcc.IsMain = false;
                                newSuppAcc.Level = parentSuppAcc.Level + 1;
                                newSuppAcc.Nature = 1; //depit
                                newSuppAcc.Halala = true;
                                newSuppAcc.NameAr = "حساب المورد" + "  " + Supplier.NameAr;
                                newSuppAcc.NameEn = Supplier.NameEn + " " + "Supplier Account";
                                newSuppAcc.Type = 2; //bugget
                                newSuppAcc.IsMainCustAcc = false;
                                newSuppAcc.Active = true;
                                newSuppAcc.AddUser = UserId;
                                newSuppAcc.BranchId = BranchId;
                                newSuppAcc.AddDate = DateTime.Now;

                                _TaamerProContext.Accounts.Add(newSuppAcc);
                                parentSuppAcc.IsMain = true; // update main acc
                                _TaamerProContext.SaveChanges();
                                SupplierUpdated.AccountId = newSuppAcc.AccountId;


                            }
                            else
                            {
                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote2 = "فشل في حفظ المورد";
                                _SystemAction.SaveAction("SaveSupplier", "Acc_SuppliersService", 1, Resources.Acc_SupplierMessageError, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                //-----------------------------------------------------------------------------------------------------------------
                                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Acc_SupplierMessageError };
                            }
                        }
                    }

                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل مورد رقم " + Supplier.SupplierId;
                    _SystemAction.SaveAction("SaveSupplier", "Acc_SuppliersService", 2,Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully};
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ البند";
                _SystemAction.SaveAction("SaveSupplier", "Acc_SuppliersService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteSupplier(int SupplierId, int UserId, int BranchId)
        {
            try
            {
                Acc_Suppliers Supplier = _Acc_SuppliersRepository.GetById(SupplierId);
                var AccTrans = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.AccountId == Supplier.AccountId);
                if (AccTrans != null && AccTrans.Count() > 0)
                {

                    foreach (var item in AccTrans)
                    {
                        var invv = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.InvoiceId == item.InvoiceId && s.Rad == false);
                        if (invv != null && invv.Count() > 0)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = " فشل في حذف عميل رقم " + SupplierId; ;
                            _SystemAction.SaveAction("DeleteCustomer", "CustomerService", 3, Resources.Financial_Transactions_Message_Error, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------

                            return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Financial_Transactions_Message_Error };
                        }
                    }

                }

                int Count = _accountsRepository.GetMatching(s => s.IsDeleted == false && s.ParentId == Supplier.AccountId).Count();
                if (Count > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote3 = " فشل في حذف عميل رقم " + SupplierId; ;
                    _SystemAction.SaveAction("DeleteCustomer", "Acc_ClausesService", 3, Resources.AccountDeletedError, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.AccountDeletedError };
                }

                Accounts account = _accountsRepository.GetById((int)Supplier.AccountId);
                account.IsDeleted = true;
                account.DeleteDate = DateTime.Now;
                account.DeleteUser = UserId;

                Supplier.IsDeleted = true;
                Supplier.DeleteDate = DateTime.Now;
                Supplier.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف مورد رقم " + SupplierId;
                _SystemAction.SaveAction("DeleteSupplier", "Acc_SuppliersService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف مورد رقم " + SupplierId; ;
                _SystemAction.SaveAction("DeleteSupplier", "Acc_SuppliersService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------


                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }

        public string GetTaxNoBySuppId(int SupplierId, string lang, int BranchId)
        {

            var Supplier = _Acc_SuppliersRepository.GetById(SupplierId).TaxNo;
            return Supplier;
        }
        public int GetAccIdBySuppId(int SupplierId, string lang, int BranchId)
        {

            var Supplier = _Acc_SuppliersRepository.GetById(SupplierId).AccountId ?? 0;
            return Supplier;
        }
        public int GetSuppIdByAccId(int AccountId, string lang, int BranchId)
        {

            var Supplier = _Acc_SuppliersRepository.GetMatching(s => s.AccountId == AccountId).FirstOrDefault().SupplierId;
            return Supplier;
        }
        public string GetSuppNameBySuppId(int SupplierId, string lang, int BranchId)
        {

            var Supplier = _Acc_SuppliersRepository.GetById(SupplierId).NameAr;
            return Supplier;
        }
    }
}

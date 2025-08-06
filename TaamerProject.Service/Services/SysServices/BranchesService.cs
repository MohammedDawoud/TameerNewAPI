using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using System.Net;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;
using TaamerProject.Repository.Repositories;

namespace TaamerProject.Service.Services
{
    public class BranchesService : IBranchesService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly IOrganizationsRepository _organizationsRepository;
        private readonly IUsersRepository _UsersRepository;
        private readonly IUserBranchesRepository _UserBranchesRepository;
        private readonly IProjectRepository _projectRepository;


        public BranchesService(TaamerProjectContext dataContext, ISystemAction systemAction, IBranchesRepository branchesRepository,
            IUsersRepository usersRepository, IUserBranchesRepository userBranchesRepository, IOrganizationsRepository organizationsRepository,
            IProjectRepository projectRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _organizationsRepository = organizationsRepository;
            _BranchesRepository = branchesRepository;
            this._UsersRepository = usersRepository;
            _UserBranchesRepository = userBranchesRepository;
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<BranchesVM>> GetAllBranches(string lang)
        {
            var Branches = await _BranchesRepository.GetAllBranches(lang);
            return Branches;
        }
        public Task<IEnumerable<BranchesVM>> FillBranchSelectNew(string lang)
        {
            var Branches =  _BranchesRepository.FillBranchSelectNew(lang);
            return Branches;
        }
        public async Task<IEnumerable<BranchesVM>> GetBranchByBranchId(string lang, int BranchId)
        {
            var Branches =await _BranchesRepository.GetBranchByBranchId(lang, BranchId);
            return Branches;
        }
        public async Task<BranchesVM> GetBranchByBranchIdCheck(string lang, int BranchId)
        {
            var Branches = await _BranchesRepository.GetBranchByBranchIdCheck(lang, BranchId);
            return Branches;
        }
        public async Task<IEnumerable<BranchesVM>> GetAllBranchesByUserId(string Lang, int UserId)
        {
            var Branches =await _UserBranchesRepository.GetAllBranchesByUserId(Lang, UserId);
            var UserMainBranch =await _UsersRepository.GetUserById(UserId, Lang);
            var mainbarnchObj = await _BranchesRepository.GetBranchById(0, Lang);
            var check = 0;
            if (Branches.Count() != 0)
            {
                foreach (var item in Branches)
                {
                    if (item.BranchId == UserMainBranch.BranchId)
                    {
                        check = 1;
                    }
                }
            }
            if (check == 0)
            {
                if (UserMainBranch != null)
                {
                    mainbarnchObj = _BranchesRepository.GetBranchById(UserMainBranch.BranchId ?? 0, Lang).Result;
                }
                else
                {
                    mainbarnchObj = _BranchesRepository.GetBranchById(0, Lang).Result;
                }
                //mainbarnchObj.BranchId = UserMainBranch.BranchId ?? 0;
                //mainbarnchObj.BranchName = UserMainBranch.BranchName;
                //Branches.Concat(mainbarnchObj);
                var allBranch = Branches.Union(mainbarnchObj);
                return allBranch;
            }
            else
            {
                return Branches;
            }

        }
        public async Task< IEnumerable<BranchesVM>> GetAllBranchesAndMainByUserId(string Lang, int UserId)
        {
            var Branches = await _UserBranchesRepository.GetAllBranchesAndMainByUserId(Lang, UserId);
            return Branches;
        }
        public GeneralMessage SaveBranches(Branch branches, int UserId, string Lang, int BranchId)
        {
            try
            {
                var Org = _organizationsRepository.GetBranchOrganization().Result;
                var codeExist = _TaamerProContext.Branch.Where(s => s.IsDeleted == false && s.BranchId != branches.BranchId && s.Code == branches.Code).FirstOrDefault();
                if (codeExist != null)
                {

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ الفرع";
                   _SystemAction.SaveAction("SaveBranches", "BranchesService", 1, Resources.TheCodeAlreadyExists, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.TheCodeAlreadyExists };
                }
                if (branches.BranchId == 0)
                {
                    branches.IsActive = true; //Edit
                    branches.AddUser = UserId;
                    if ((branches.TaxCode! ?? "").Trim() == (Org.TaxCode! ?? "").Trim())
                    {
                        branches.InvoiceBranchSeparated = false;
                    }
                    else
                    {
                        branches.InvoiceBranchSeparated = true;
                    }
                    branches.AddDate = DateTime.Now;
                    _TaamerProContext.Branch.Add(branches);
                    _TaamerProContext.SaveChanges();


                    //// add cost center
                    var newcostCenter = new CostCenters();
                    newcostCenter.Code = branches.Code;
                    newcostCenter.NameAr = branches.NameAr;
                    newcostCenter.NameEn = branches.NameEn;
                    newcostCenter.AddDate = DateTime.Now;
                    newcostCenter.ParentId = null;
                    newcostCenter.AddUser = UserId;
                    newcostCenter.BranchId = branches.BranchId;
                    newcostCenter.ProjId = 0;
                    _TaamerProContext.CostCenters.Add(newcostCenter);
                    //////
                    _TaamerProContext.SaveChanges();
                    var message = "";
                    if (Lang == "rtl")
                    {
                        message = Resources.General_SavedSuccessfully;
                    }
                    else if (Lang == "ltr")
                    {
                        message = "Saved Successfully";
                    }
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة فرع جديد";
                    _SystemAction.SaveAction("SaveBranches", "BranchesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = message };
                }
                else
                {
                    var BranchesUpdated = _BranchesRepository.GetById(branches.BranchId);
                    if (BranchesUpdated != null)
                    {
                        BranchesUpdated.Code = branches.Code;
                        BranchesUpdated.NameAr = branches.NameAr;
                        BranchesUpdated.NameEn = branches.NameEn;
                        BranchesUpdated.BranchManager = branches.BranchManager;
                        BranchesUpdated.Phone = branches.Phone;
                        BranchesUpdated.Mobile = branches.Mobile;
                        BranchesUpdated.CurrencyId = branches.CurrencyId;
                        BranchesUpdated.EngineeringLicense = branches.EngineeringLicense;
                        BranchesUpdated.LabLicense = branches.LabLicense;
                        BranchesUpdated.Mailbox = branches.Mailbox;
                        BranchesUpdated.CityId = branches.CityId;
                        BranchesUpdated.UpdateUser = UserId;
                        BranchesUpdated.UpdateDate = DateTime.Now;
                        BranchesUpdated.OrganizationId = branches.OrganizationId;
                        BranchesUpdated.PostalCodeFinal = branches.PostalCodeFinal;
                        BranchesUpdated.ExternalPhone = branches.ExternalPhone;
                        BranchesUpdated.Country = branches.Country;
                        BranchesUpdated.Neighborhood = branches.Neighborhood;
                        BranchesUpdated.StreetName = branches.StreetName;
                        BranchesUpdated.BuildingNumber = branches.BuildingNumber;
                        BranchesUpdated.AccountBank = branches.AccountBank;
                        BranchesUpdated.AccountBank2 = branches.AccountBank2;

                        BranchesUpdated.Engineering_License = branches.Engineering_License;
                        BranchesUpdated.Engineering_LicenseDate = branches.Engineering_LicenseDate;

                        BranchesUpdated.Address = branches.Address;
                        BranchesUpdated.TaxCode = branches.TaxCode;
                        BranchesUpdated.PostalCode = branches.PostalCode;
                        BranchesUpdated.ProjectStartCode = branches.ProjectStartCode;
                        BranchesUpdated.OfferStartCode = branches.OfferStartCode;
                        BranchesUpdated.TaskStartCode = branches.TaskStartCode;
                        BranchesUpdated.OrderStartCode = branches.OrderStartCode;
                        //BranchesUpdated.InvoiceStartCode = branches.InvoiceStartCode;
                        if ((branches.TaxCode!??"").Trim() == (Org.TaxCode! ?? "").Trim())
                        {
                            BranchesUpdated.InvoiceBranchSeparated = false;
                        }
                        else
                        {
                            BranchesUpdated.InvoiceBranchSeparated = true;
                        }
                        BranchesUpdated.BankId = branches.BankId;
                        BranchesUpdated.BankId2 = branches.BankId2;
                        BranchesUpdated.IsPrintInvoice = branches.IsPrintInvoice;

                        BranchesUpdated.headerPrintInvoice = branches.headerPrintInvoice;
                        BranchesUpdated.headerPrintrevoucher = branches.headerPrintrevoucher;
                        BranchesUpdated.headerprintdarvoucher = branches.headerprintdarvoucher;
                        BranchesUpdated.headerPrintpayvoucher = branches.headerPrintpayvoucher;
                        BranchesUpdated.headerPrintcontract = branches.headerPrintcontract;


                        //if (branches.BranchLogoUrl !=null && branches.BranchLogoUrl != "")
                        //{
                        BranchesUpdated.BranchLogoUrl = branches.BranchLogoUrl;

                        //}
                        //if (branches.HeaderLogoUrl != null && branches.HeaderLogoUrl != "")
                        //{
                        BranchesUpdated.HeaderLogoUrl = branches.HeaderLogoUrl;

                        //}
                        //if (branches.FooterLogoUrl != null && branches.FooterLogoUrl != "")
                        //{
                        BranchesUpdated.FooterLogoUrl = branches.FooterLogoUrl;

                        //}

                    }

                    var CostCenterUpdated = _TaamerProContext.CostCenters.Where(s => s.BranchId == branches.BranchId && s.IsDeleted == false && s.ProjId == 0).FirstOrDefault();
                    if (CostCenterUpdated != null)
                    {
                        CostCenterUpdated.NameAr = branches.NameAr;
                        CostCenterUpdated.NameEn = branches.NameEn;

                    }
                    else
                    {
                        var newcostCenter = new CostCenters();
                        newcostCenter.Code = branches.Code;
                        newcostCenter.NameAr = branches.NameAr;
                        newcostCenter.NameEn = branches.NameEn;
                        newcostCenter.AddDate = DateTime.Now;
                        newcostCenter.ParentId = null;
                        newcostCenter.AddUser = UserId;
                        newcostCenter.BranchId = branches.BranchId;
                        newcostCenter.ProjId = 0;
                        _TaamerProContext.CostCenters.Add(newcostCenter);
                    }

                    _TaamerProContext.SaveChanges();
                    var message = "";
                    if (Lang == "rtl")
                    {
                        message = Resources.General_SavedSuccessfully;
                    }
                    else if (Lang == "ltr")
                    {
                        message = "Saved Successfully";
                    }
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل فرع رقم " + branches.BranchId;
                    _SystemAction.SaveAction("SaveBranches", "BranchesService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = message };
                }

            }
            catch (Exception ex)
            {
                var message = "";
                if (Lang == "rtl")
                {
                    message = Resources.General_SavedFailed;
                }
                else if (Lang == "ltr")
                {
                    message = "Saved Falied";
                }
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الفرع";
                _SystemAction.SaveAction("SaveBranches", "BranchesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = message };
            }
        }
        public GeneralMessage SaveBranchesInvoiceCode(Branch branches, int UserId, int BranchId)
        {
            try
            {
                var BranchesUpdated = _BranchesRepository.GetById(branches.BranchId);
                if (BranchesUpdated != null)
                {
                    BranchesUpdated.InvoiceStartCode = branches.InvoiceStartCode;
                    BranchesUpdated.UpdateUser = UserId;
                    BranchesUpdated.UpdateDate = DateTime.Now;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تعدلات بادئة الفاتورة الفرع رقم " + branches.BranchId;
                _SystemAction.SaveAction("SaveBranchesInvoiceCode", "BranchesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حفظ بادئة الفاتورة الفرع رقم " + branches.BranchId;
                _SystemAction.SaveAction("SaveBranchesInvoiceCode", "BranchesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage SaveBrancheAccs(Branch branches, int UserId, int BranchId)
        {
            try
            {
                var BranchesUpdated = _BranchesRepository.GetById(branches.BranchId);
                if (BranchesUpdated != null)
                {
                    BranchesUpdated.BoxAccId = branches.BoxAccId;
                    BranchesUpdated.SuspendedFundAccId = branches.SuspendedFundAccId;
                    BranchesUpdated.CustomersAccId = branches.CustomersAccId;
                    BranchesUpdated.SuppliersAccId = branches.SuppliersAccId;
                    BranchesUpdated.EmployeesAccId = branches.EmployeesAccId;
                    BranchesUpdated.GuaranteeAccId = branches.GuaranteeAccId;
                    BranchesUpdated.ContractsAccId = branches.ContractsAccId;
                    BranchesUpdated.TaxsAccId = branches.TaxsAccId;
                    BranchesUpdated.LoanAccId = branches.LoanAccId;
                    BranchesUpdated.PurchaseReturnCashAccId = branches.PurchaseReturnCashAccId;
                    BranchesUpdated.CashReturnInvoicesAccId = branches.CashReturnInvoicesAccId;

                    BranchesUpdated.SaleDiscountAccId = branches.SaleDiscountAccId;

                    BranchesUpdated.PurchaseDiscAccId = branches.PurchaseDiscAccId;
                    BranchesUpdated.PurchaseApprovalAccId = branches.PurchaseApprovalAccId;
                    BranchesUpdated.RevenuesAccountId = branches.RevenuesAccountId;
                    BranchesUpdated.PurchaseDelayAccId = branches.PurchaseDelayAccId;
                    BranchesUpdated.CheckInvoicesAccId = branches.CheckInvoicesAccId;
                    BranchesUpdated.BoxAccId2 = branches.BoxAccId2;

                    BranchesUpdated.CashInvoicesAccId = branches.CashInvoicesAccId;
                    BranchesUpdated.SaleCashAccId = branches.SaleCashAccId;
                    BranchesUpdated.SaleCostAccId = branches.SaleCostAccId;
                    BranchesUpdated.UpdateUser = UserId;
                    BranchesUpdated.UpdateDate = DateTime.Now;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تعدلات حسابات الفرع رقم " + branches.BranchId;
                _SystemAction.SaveAction("SaveBrancheAccs", "BranchesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حفظ حسابات الفرع رقم " + branches.BranchId;
                _SystemAction.SaveAction("SaveBrancheAccs", "BranchesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public GeneralMessage SaveBranchesAccsBS(Branch branches, int UserId, int BranchId)
        {
            try
            {
                var BranchesUpdated = _BranchesRepository.GetById(branches.BranchId);
                if (BranchesUpdated != null)
                {
                    BranchesUpdated.PurchaseCashAccId = branches.PurchaseCashAccId;
                    BranchesUpdated.PurchaseOutCashAccId = branches.PurchaseOutCashAccId;
                    BranchesUpdated.PurchaseOutDelayAccId = branches.PurchaseOutDelayAccId;

                    BranchesUpdated.UpdateUser = UserId;
                    BranchesUpdated.UpdateDate = DateTime.Now;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "  حفظ حسابات بنود الصرف للفرع رقم " + branches.BranchId;
                _SystemAction.SaveAction("SaveBranchesAccsBS", "BranchesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حفظ حسابات بنود الصرف للفرع رقم " + branches.BranchId;
                _SystemAction.SaveAction("SaveBranchesAccsBS", "BranchesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage SaveBranchesAccsKD(Branch branches, int UserId, int BranchId)
        {
            try
            {
                var BranchesUpdated = _BranchesRepository.GetById(branches.BranchId);
                if (BranchesUpdated != null)
                {
                    BranchesUpdated.PurchaseReturnApprovAccId = branches.PurchaseReturnApprovAccId;
                    BranchesUpdated.PurchaseReturnDiscAccId = branches.PurchaseReturnDiscAccId;

                    BranchesUpdated.BublicRevenue = branches.BublicRevenue;
                    BranchesUpdated.OtherRevenue = branches.OtherRevenue;

                    BranchesUpdated.UpdateUser = UserId;
                    BranchesUpdated.UpdateDate = DateTime.Now;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "  حفظ حسابات قائمة الدخل للفرع رقم " + branches.BranchId;
                _SystemAction.SaveAction("SaveBranchesAccsKD", "BranchesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حفظ حسابات قائمة الدخل للفرع رقم " + branches.BranchId;
                _SystemAction.SaveAction("SaveBranchesAccsKD", "BranchesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage SaveCSIDBranch(int BranchId, string CSR, string PrivateKey, string CSID, string SecretKey, int UserId, int BranchIdO)
        {
            try
            {
                var Branchobj = _BranchesRepository.GetById(BranchId);

                Branchobj.CSR = CSR;
                Branchobj.PrivateKey = PrivateKey;
                Branchobj.PublicKey = CSID;
                Branchobj.SecreteKey = SecretKey;

                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تم الحفظ ";
                _SystemAction.SaveAction("SaveCSIDBranch", "BranchesService", 1, "تم حفظ المفتاح العام", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                _SystemAction.SaveAction("SaveCSIDBranch", "BranchesService", 1, "فشل في حفظ المفتاح العام", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage DeleteBranches(int BranchId, int UserId, string Lang)
        {
            try
            {
                if (BranchId == 1)
                {
                    var message = "";
                    if (Lang == "rtl")
                    {
                        message = Resources.CanNotDeleteMainBranch;
                    }
                    else if (Lang == "ltr")
                    {
                        message = "You Can not Delete Main Branch";
                    }
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حذف الفرع";
                    _SystemAction.SaveAction("DeleteBranches", "BranchesService", 3, Resources.CanNotDeleteMainReason, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = message };
                }
                //var UserBranchs = _UserBranchesRepository.GetBranchByBranchId(Lang, BranchId).Count();

                var Customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && s.BranchId == BranchId);
                if (Customers != null && Customers.Count() > 0)
                {
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " فشل في حذف الفرع رقم " + BranchId; ;
                    _SystemAction.SaveAction("DeleteBranches", "BranchesService", 3, "يوجد عملاء علي هذا الفرع من فضلك قم بنقلهم اولا الي فرع اخر او قم بحذفهم", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "يوجد عملاء علي هذا الفرع من فضلك قم بنقلهم اولا الي فرع اخر او قم بحذفهم" };
                }



                var AccTrans = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.BranchId == BranchId);
                if (AccTrans != null && AccTrans.Count() > 0)
                {

                    foreach (var item in AccTrans)
                    {
                        var invv = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.InvoiceId == item.InvoiceId && s.Rad == false);
                        if (invv != null && invv.Count() > 0)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = " فشل في حذف الفرع رقم " + BranchId; ;
                            _SystemAction.SaveAction("DeleteBranches", "BranchesService", 3, Resources.Cannot_Delete_Branch_Financial_Transactions_Message_Error, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------

                            return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Cannot_Delete_Branch_Financial_Transactions_Message_Error };
                        }
                    }

                }


                var UserBranchs = 0;
                var UserBranch = _UsersRepository.GetUserByBranchId(BranchId).Result.Count();

                if (UserBranchs == 0 && UserBranch == 0)
                {

                    var Projco = _projectRepository.GetAllProjByBranch(Lang, BranchId).Result.Count();
                    if (Projco == 0)
                    {
                        Branch bran = _BranchesRepository.GetById(BranchId);
                        bran.IsDeleted = true;
                        bran.IsActive = false;
                        bran.DeleteDate = DateTime.Now;
                        bran.DeleteUser = UserId;


                        var CostCenterBranch = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId);
                        if (CostCenterBranch.Count() > 0)
                        {
                            CostCenterBranch.FirstOrDefault().IsDeleted = true;
                            CostCenterBranch.FirstOrDefault().DeleteDate = DateTime.Now;
                            CostCenterBranch.FirstOrDefault().DeleteUser = UserId;
                        }
                        var UserBranches = _TaamerProContext.UserBranches.Where(s => s.BranchId == BranchId);
                        if (UserBranches.Count() > 0)
                        {
                          var usrbrnsh= _TaamerProContext.UserBranches.Where(s => s.BranchId == BranchId); 
                            _TaamerProContext.UserBranches.RemoveRange(usrbrnsh);
                        }



                        _TaamerProContext.SaveChanges();
                        var message = "";
                        if (Lang == "rtl")
                        {
                            message = "تم الحذف الفرع بنجاح";
                        }
                        else if (Lang == "ltr")
                        {
                            message = "Deleted Succssfully";
                        }
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " حذف بند رقم " + BranchId;
                        _SystemAction.SaveAction("DeleteBranches", "BranchesService", 3, "تم الحذف الفرع بنجاح", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = message };
                    }
                    else
                    {
                        var message = "";
                        if (Lang == "rtl")
                        {
                            message = "يوجد مشاريع مرتبطين بالفرع، الرجاء نقل المشاريع إلي فرع آخر أولا";
                        }
                        else if (Lang == "ltr")
                        {
                            message = "There are Projects associated with the branch, please move the Projects to another branch first";
                        }
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " فشل في حذف بند رقم " + BranchId; ;
                        _SystemAction.SaveAction("DeleteBranches", "BranchesService", 3, "يوجد مشاريع مرتبطين بالفرع، الرجاء نقل المشاريع إلي فرع آخر أولا", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = message };
                    }
                }
                else
                {
                    var message = "";
                    if (Lang == "rtl")
                    {
                        message = "يوجد مستخدمين مرتبطين بالفرع، الرجاء نقل المستخدمين إلي فرع آخر أولا";
                    }
                    else if (Lang == "ltr")
                    {
                        message = "There are users associated with the branch, please move the users to another branch first";
                    }
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " فشل في حذف بند رقم " + BranchId; ;
                    _SystemAction.SaveAction("DeleteBranches", "BranchesService", 3, "يوجد مستخدمين مرتبطين بالفرع، الرجاء نقل المستخدمين إلي فرع آخر أولا", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = message };
                }
            }
            catch (Exception)
            {
                var message = "";
                if (Lang == "rtl")
                {
                    message = Resources.General_DeletedFailed;
                }
                else if (Lang == "ltr")
                {
                    message = "Deleted Falied";
                }
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف بند رقم " + BranchId; ;
                _SystemAction.SaveAction("DeleteClause", "BranchesService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = message };
            }
        }
        public GeneralMessage ActivateBranches(int BranchId, int UserId)
        {
            try
            {
                var ActivateBranches = _TaamerProContext.Branch.Where(s => s.IsDeleted == false && s.IsActive == true);
                foreach (var item in ActivateBranches)
                {
                    item.IsActive = false;
                }
                var BranchesUpdated = _BranchesRepository.GetById(BranchId);
                if (BranchesUpdated != null)
                {
                    BranchesUpdated.IsActive = true;
                    BranchesUpdated.UpdateUser = UserId;
                    BranchesUpdated.UpdateDate = DateTime.Now;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تحديد الفرع رقم  " + BranchId;
               _SystemAction.SaveAction("ActivateBranches", "BranchesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase= Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في تحديد الفرع رقم  " + BranchId;
                _SystemAction.SaveAction("ActivateBranches", "BranchesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public async Task<int> GenerateNextBranchNumber()
        {
            return await _BranchesRepository.GenerateNextBranchNumber();
        }

        public async Task<int> GetOrganizationId(int BranchId)
        {
            return await _BranchesRepository.GetOrganizationId(BranchId);
        }




        public  async Task<BranchesVM> GetActiveBranch()
        {
            var ActiveBranch = _TaamerProContext.Branch.Where(s => s.IsDeleted == false && s.IsActive == true).FirstOrDefault();
            if (ActiveBranch != null)
            {
                return new BranchesVM { BranchId = ActiveBranch.BranchId, BranchName = ActiveBranch.NameAr };
            }
            return null;
        }
        public async Task<Branch> GetBranchById(int BranchId)
        {
            return _BranchesRepository.GetById(BranchId);
        }
    }
}

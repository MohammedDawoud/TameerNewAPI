using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using System.Net;
using TaamerP.Service.LocalResources;
using System.Net.Mail;
using TaamerProject.Repository.Repositories;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Security.Cryptography;

namespace TaamerProject.Service.Services
{
    public class OrganizationsService : IOrganizationsService
    {

        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IOrganizationsRepository _OrganizationsRepository;

        private readonly IBranchesService _branchesService;
        private readonly ILicencesRepository _LicencesRepository;
        private readonly IEmailSettingRepository _EmailSettingRepository;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly IUsersRepository _UsersRepository;
        private readonly ILicencesService _LicencesService;


        public OrganizationsService(TaamerProjectContext dataContext, ISystemAction systemAction, IOrganizationsRepository organizationsRepository,
            IBranchesService branchesService, ILicencesRepository licencesRepository, IEmailSettingRepository emailSettingRepository,
            IBranchesRepository branchesRepository, IUsersRepository usersRepository, ILicencesService licencesService)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _OrganizationsRepository = organizationsRepository;
            _branchesService = branchesService;
            _LicencesRepository = licencesRepository;
            _EmailSettingRepository = emailSettingRepository;
            _BranchesRepository = branchesRepository;
            _UsersRepository = usersRepository;
            _LicencesService = licencesService;
        }

        public async Task<OrganizationsVM> GetOrganizationDataLogin(string Lang)
        {
            var Organizations =await _OrganizationsRepository.GetOrganizationDataLogin(Lang);
            return Organizations;
        }
        public async Task<OrganizationsVM> GetBranchOrganization(int? BranchId)
        {
            var Organizations = await _OrganizationsRepository.GetBranchOrganization(BranchId);
            return Organizations;
        }

        public async Task<OrganizationsVM> GetBranchOrganization()
        {
            var Organizations = await _OrganizationsRepository.GetBranchOrganization();
            return Organizations;
        }



        public async Task<OrganizationsVM> GetBranchOrganizationData(int orgId)
        {
            var Organizations = await _OrganizationsRepository.GetBranchOrganizationData(orgId);
            return Organizations;
        }
        public async Task<OrganizationsVM> GetComDomainLink_Org(int orgId)
        {
            var Organizations = await _OrganizationsRepository.GetComDomainLink_Org(orgId);
            return Organizations;
        }
        public async Task<OrganizationsVM> GetApplicationVersion_Org(int orgId)
        {
            var Organizations = await _OrganizationsRepository.GetApplicationVersion_Org(orgId);
            return Organizations;
        }

        public async Task<OrganizationsVM> GetBranchOrganizationDataInvoice(int orgId)
        {
            var Organizations = await _OrganizationsRepository.GetBranchOrganizationDataInvoice(orgId);
            return Organizations;
        }
        public async Task<OrganizationsVM> GetOrganizationData(int branchId)
        {
            int orgId = await _branchesService.GetOrganizationId(branchId);
            var Organizations = await _OrganizationsRepository.GetBranchOrganizationData(orgId);
            return Organizations;
        }

        public async Task<string> GetDefaultLogoOfOrganization()
        {
            string logoURL = await _OrganizationsRepository.GetDefaultLogoOfOrganization();
            return logoURL;
        }
        public GeneralMessage SaveOrganizations(Organizations organizations, int UserId, int BranchId)
        {
            try
            {
                var BranchOrganization = _OrganizationsRepository.GetById(organizations.OrganizationId);
                if (BranchOrganization == null)
                {
                    organizations.BranchId = BranchId;
                    organizations.AddUser = UserId;
                    organizations.AddDate = DateTime.Now;
                    _TaamerProContext.Organizations.Add(organizations);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة بيانات منشأة جديد";
                   _SystemAction.SaveAction("SaveOrganizations", "OrganizationsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };


                }
                else
                {
                    BranchOrganization.NameEn = organizations.NameEn;
                    if (BranchOrganization.Mobile != organizations.Mobile)
                    {
                        var Licences = _LicencesRepository.GetAllLicences("").Result.FirstOrDefault();
                        var LicencesData = _TaamerProContext.Licences.Where(x=>x.LicenceId==Licences.LicenceId).FirstOrDefault();
                        LicencesData.Mobile = organizations.Mobile;
                        _TaamerProContext.SaveChanges();

                    }
                    BranchOrganization.Mobile = organizations.Mobile;
                    BranchOrganization.Email = organizations.Email;
                    if (organizations.LogoUrl != null)
                    {
                        BranchOrganization.LogoUrl = organizations.LogoUrl;
                    }
                    BranchOrganization.ServerName = organizations.ServerName;
                    BranchOrganization.Address = organizations.Address;
                    BranchOrganization.WebSite = organizations.WebSite;
                    BranchOrganization.Fax = organizations.Fax;
                    BranchOrganization.CityId = organizations.CityId;
                    BranchOrganization.ReportType = organizations.ReportType;
                    BranchOrganization.TaxCode = organizations.TaxCode;
                    BranchOrganization.NotificationsMail = organizations.NotificationsMail;
                    BranchOrganization.PostalCode = organizations.PostalCode;
                    BranchOrganization.PostalCodeFinal = organizations.PostalCodeFinal;
                    BranchOrganization.ExternalPhone = organizations.ExternalPhone;
                    BranchOrganization.Country = organizations.Country;
                    BranchOrganization.Neighborhood = organizations.Neighborhood;
                    BranchOrganization.StreetName = organizations.StreetName;
                    BranchOrganization.BuildingNumber = organizations.BuildingNumber;
                    BranchOrganization.AccountBank = organizations.AccountBank;
                    BranchOrganization.AccountBank2 = organizations.AccountBank2;
                    BranchOrganization.Engineering_License = organizations.Engineering_License;
                    BranchOrganization.Engineering_LicenseDate = organizations.Engineering_LicenseDate;


                    BranchOrganization.UpdateUser = UserId;
                    BranchOrganization.UpdateDate = DateTime.Now;

                    BranchOrganization.Password = organizations.Password;
                    BranchOrganization.SenderName = organizations.SenderName;
                    BranchOrganization.Host = organizations.Host;
                    BranchOrganization.Port = organizations.Port;
                    BranchOrganization.SSL = organizations.SSL;
                    BranchOrganization.IsFooter = organizations.IsFooter;
                    BranchOrganization.RepresentorEmpId = organizations.RepresentorEmpId;
                    BranchOrganization.BankId = organizations.BankId;
                    BranchOrganization.BankId2 = organizations.BankId2;

                    //BranchOrganization.CSR = organizations.CSR;
                    //BranchOrganization.PrivateKey = organizations.PrivateKey;
                    //BranchOrganization.SecreteKey = organizations.SecreteKey;
                    //BranchOrganization.PublicKey = organizations.PublicKey;
                    if (BranchOrganization.NameAr != organizations.NameAr)
                    {
                        try
                        {
                            updateCustomer_Labik();
                        }
                        catch (Exception ex) { }
                    }
                    BranchOrganization.NameAr = organizations.NameAr;

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "تعديل بيانات المنشأة   " + organizations.NameAr;
                    _SystemAction.SaveAction("SaveOrganizations", "OrganizationsService", 1, "تم التعديل", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };


                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ بيانات المنشأة";
                _SystemAction.SaveAction("SaveOrganizations", "OrganizationsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage SaveComDomainLink(Organizations organizations, int UserId, int BranchId)
        {
            try
            {
                var BranchOrganization = _OrganizationsRepository.GetById(organizations.OrganizationId);
                if (BranchOrganization != null)
                {
                    BranchOrganization.ComDomainLink = organizations.ComDomainLink;
                    BranchOrganization.ComDomainAddress = organizations.ComDomainAddress;
                    BranchOrganization.ApiBaseUri = organizations.ApiBaseUri;
                    BranchOrganization.TameerAPIURL=organizations.TameerAPIURL; 
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "حفظ دومين العميل منشأه  " + organizations.NameAr;
                    _SystemAction.SaveAction("SaveComDomainLink", "OrganizationsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };


                }
                else
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ دومين العميل ";
                _SystemAction.SaveAction("SaveComDomainLink", "OrganizationsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }



        public GeneralMessage SaveAppVersion(Organizations organizations, int UserId, int BranchId)
        {
            try
            {
                var BranchOrganization = _OrganizationsRepository.GetById(organizations.OrganizationId);
                if (BranchOrganization != null)
                {
                    BranchOrganization.LastvesionAndroid = organizations.LastvesionAndroid;
                    BranchOrganization.LastversionIOS = organizations.LastversionIOS;
                    BranchOrganization.MessageUpdateAr = organizations.MessageUpdateAr;
                    BranchOrganization.MessageUpdateEn = organizations.MessageUpdateEn;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "حفظ اصدار التطبيق " + organizations.NameAr;
                    _SystemAction.SaveAction("SaveAppVersion", "OrganizationsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };


                }
                else
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ دومين العميل ";
                _SystemAction.SaveAction("SaveComDomainLink", "OrganizationsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }



        public GeneralMessage SaveSupport(Organizations organizations, int UserId, int BranchId)
        {
            try
            {
                var BranchOrganization = _OrganizationsRepository.GetById(organizations.OrganizationId);
                if (BranchOrganization != null)
                {
                    BranchOrganization.SupportMessageAr = organizations.SupportMessageAr;
                    BranchOrganization.SupportMessageEn = organizations.SupportMessageEn;
                    BranchOrganization.RetentionMonths = organizations.RetentionMonths;
                    BranchOrganization.RootFolder = organizations.RootFolder;

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "حفظ رسائل الدعم الفني  " + organizations.NameAr;
                    _SystemAction.SaveAction("SaveSupport", "OrganizationsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };


                }
                else
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ دومين العميل ";
                _SystemAction.SaveAction("SaveSupport", "OrganizationsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }



        public GeneralMessage SaveOrganizationSettings(Organizations organizations, int UserId, int BranchId)
        {
            try
            {
                var BranchOrganization = _OrganizationsRepository.GetById(organizations.OrganizationId);
                if (BranchOrganization == null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "فشل في حفظ بيانات المنشأة";
                    _SystemAction.SaveAction("SaveOrganizationSettings", "OrganizationsService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }
                else
                {
                    BranchOrganization.Email = organizations.Email;

                    BranchOrganization.UpdateUser = UserId;
                    BranchOrganization.UpdateDate = DateTime.Now;

                    BranchOrganization.Password = organizations.Password;
                    BranchOrganization.SenderName = organizations.SenderName;
                    BranchOrganization.Host = organizations.Host;
                    BranchOrganization.Port = organizations.Port;
                    BranchOrganization.SSL = organizations.SSL;
                    BranchOrganization.SendCustomerMail= organizations.SendCustomerMail;


                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تم الحفظ ";
                _SystemAction.SaveAction("SaveOrganizationSettings", "OrganizationsService", 1, "حفظ بيانات المنشأة", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                _SystemAction.SaveAction("SaveOrganizationSettings", "OrganizationsService", 1, "فشل في حفظ بيانات المنشأة", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage SavepartialOrganizations(Organizations organizations, int UserId, int BranchId, decimal VAT, int VATset)
        {
            try
            {
                var BranchOrganization = _OrganizationsRepository.GetById(organizations.OrganizationId);

                BranchOrganization.NameAr = BranchOrganization.NameAr;
                BranchOrganization.NameEn = BranchOrganization.NameEn;
                BranchOrganization.Mobile = BranchOrganization.Mobile;
                BranchOrganization.Email = BranchOrganization.Email;
                if (organizations.LogoUrl != null)
                {
                    BranchOrganization.LogoUrl = organizations.LogoUrl;
                }
                BranchOrganization.ServerName = BranchOrganization.ServerName;
                BranchOrganization.Address = BranchOrganization.Address;
                BranchOrganization.WebSite = BranchOrganization.WebSite;
                BranchOrganization.Fax = BranchOrganization.Fax;
                BranchOrganization.CityId = BranchOrganization.CityId;
                BranchOrganization.ReportType = BranchOrganization.ReportType;
                BranchOrganization.TaxCode = BranchOrganization.TaxCode;
                BranchOrganization.NotificationsMail = BranchOrganization.NotificationsMail;
                BranchOrganization.UpdateUser = UserId;
                BranchOrganization.UpdateDate = DateTime.Now;

                BranchOrganization.VAT = VAT;
                BranchOrganization.VATSetting = VATset;

                BranchOrganization.Password = organizations.Password;
                BranchOrganization.SenderName = organizations.SenderName;
                BranchOrganization.Host = organizations.Host;
                BranchOrganization.Port = organizations.Port;
                BranchOrganization.SSL = organizations.SSL??false;

                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تم الحفظ ";
                _SystemAction.SaveAction("SavepartialOrganizations", "OrganizationsService", 1, "تم حفظ بيانات الفرع", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully};
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                _SystemAction.SaveAction("SavepartialOrganizations", "OrganizationsService", 1, "فشل في حفظ بيانات الفرع", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage SaveRetentionBackup(Organizations organizations, int UserId, int BranchId, decimal VAT, int VATset)
        {
            try
            {
                var BranchOrganization = _OrganizationsRepository.GetById(organizations.OrganizationId);
                BranchOrganization.RetentionMonths = BranchOrganization.RetentionMonths;
                BranchOrganization.RootFolder = BranchOrganization.RootFolder;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تم الحفظ ";
                _SystemAction.SaveAction("SaveRetentionBackup", "OrganizationsService", 1, "تم حفظ بيانات اسم فولدر الباك اب وعدد الشهور", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                _SystemAction.SaveAction("SaveRetentionBackup", "OrganizationsService", 1, "فشل في حفظ بيانات اسم فولدر الباك اب وعدد الشهور", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage SaveCSIDOrganizations(int OrganizationId, string CSR, string PrivateKey, string CSID, string SecretKey, int UserId, int BranchId)
        {
            try
            {
                var BranchOrganization = _OrganizationsRepository.GetById(OrganizationId);

                BranchOrganization.CSR = CSR;
                BranchOrganization.PrivateKey = PrivateKey;
                BranchOrganization.PublicKey = CSID;
                BranchOrganization.SecreteKey = SecretKey;

                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تم الحفظ ";
                _SystemAction.SaveAction("SaveCSIDOrganizations", "OrganizationsService", 1, "تم حفظ المفتاح العام", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                _SystemAction.SaveAction("SaveCSIDOrganizations", "OrganizationsService", 1, "فشل في حفظ المفتاح العام", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage SaveErrorMessageCSIDOrganizations(int OrganizationId, string ErrorMessage, int UserId, int BranchId)
        {
            try
            {
                var BranchOrganization = _OrganizationsRepository.GetById(OrganizationId);

                BranchOrganization.CSR = ErrorMessage;
                _TaamerProContext.SaveChanges();
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage DeleteOrganizations(int OrganizationId, int UserId, int BranchId)
        {
            try
            {
                Organizations organizations = _OrganizationsRepository.GetById(OrganizationId);
                organizations.IsDeleted = true;
                organizations.DeleteDate = DateTime.Now;
                organizations.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف المنشاة  " + organizations.NameAr;
                _SystemAction.SaveAction("DeleteOrganizations", "OrganizationsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف المنشاة رقم " + OrganizationId; ;
                _SystemAction.SaveAction("DeleteOrganizations", "OrganizationsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
               
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public async Task<OrganizationsVM> CheckEmailOrganization(int? OrganizationId)
        {
            var Organization = await _OrganizationsRepository.CheckEmailOrganization(OrganizationId);
            return Organization;
        }
        public async Task<List<FillSelectVM>> FillSelect_Proc(string Con, int? UserId, int Type, int? Param, int? Status, int? BranchId)
        {
            var Organizations = await _OrganizationsRepository.FillSelect_Proc(Con, UserId,Type,Param, Status, BranchId);
            return Organizations;
        }
        public async Task<List<FillSelectVM>> FillSelect_Cust(string Con, int? UserId, int Type, int? Param, int? Status, int? BranchId)
        {
            var Organizations = await _OrganizationsRepository.FillSelect_Cust(Con, UserId, Type, Param, Status, BranchId);
            return Organizations;
        }
        public GeneralMessage SendMail_test(int BranchId, string ReceivedUser, string Subject, string textBody, bool IsBodyHtml = false)
        {
            try
            {
                var org = _OrganizationsRepository.GetBranchOrganization(BranchId).Result;// _BranchesRepository.GetById(BranchId).OrganizationId;


                var mail = new MailMessage();
                var email = org.Email;// _EmailSettingRepository.GetEmailSetting(branch).Result.SenderEmail;
                                      //var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(branch).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch).Result.Password);
                var loginInfo = new NetworkCredential(org.SenderName, org.Password);

                // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);

                if (org.SenderName != null)
                {
                    mail.From = new MailAddress(email, "jjk");// org.SenderName);
                }
                else
                {
                    mail.From = new MailAddress(email, "لديك اشعار من نظام تعمير السحابي");
                }
               

                mail.To.Add(new MailAddress(ReceivedUser));
                
                mail.Subject = Subject;

                mail.Body = textBody;
                mail.IsBodyHtml = IsBodyHtml;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(org.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(org.Port);

                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم ارسال الميل" };

            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedToSendEmail,ReturnedStr=ex.Message };

            }
        }


        public async Task<bool> updateCustomer_Labik()
        {
            try
            {
                try
                {
                    var licences = _TaamerProContext.Licences.FirstOrDefault(x=>x.IsDeleted==false);
                    var licence = _LicencesService.CheckLicenceG_UID(licences);
                    var db = _TaamerProContext.GetDatabaseName();
                    if (licence.StatusCode == HttpStatusCode.OK)
                    {
                        //var uri = "https://api2.tameercloud.com/"; //"https://localhost:44334/";// "http://164.68.110.173:8080/";
                        var uri = "https://localhost:44334/";// "http://164.68.110.173:8080/";
                        if (uri != null && uri != "")
                        {
                            //Generate Token
                            var token = getapitoken(uri);

                            var org = GetBranchOrganization();

                            var formData = new MultipartFormDataContent();

                            // Add the fields to the formdata
                            formData.Add(new StringContent((licences.LicenceId).ToString()), "LicenceId");
                            formData.Add(new StringContent(licence.ReasonPhrase ?? ""), "G_UID");
                            formData.Add(new StringContent(DecryptValue(licences.LicenceContractNo) ?? ""), "LicenceContractNo");
                            formData.Add(new StringContent(DecryptValue(licences.NoOfUsers) ?? ""), "NoOfUsers");
                            formData.Add(new StringContent(licences.Mobile ?? ""), "Mobile");
                            formData.Add(new StringContent(org.Result.NameAr ?? ""), "CustomerName");
                            formData.Add(new StringContent(DecryptValue(licences.Hosting_Expiry_Date) ?? ""), "Hosting_Expiry_Date");
                            formData.Add(new StringContent(DecryptValue(licences.Support_Expiry_Date) ?? ""), "Support_Expiry_Date");
                            formData.Add(new StringContent((licences.Type).ToString() ?? ""), "Type");
                            formData.Add(new StringContent(org.Result.ComDomainLink ?? ""), "Domain");
                            formData.Add(new StringContent(licences.Email3 ?? ""), "ComputerName");
                            formData.Add(new StringContent((org.Result.TameerAPIURL ?? "").ToString()), "CustomerULR");
                            formData.Add(new StringContent(licences.Support_Start_Date ?? ""), "Support_Start_Date");
                            formData.Add(new StringContent(org.Result.ComDomainAddress ?? ""), "IPAdress");
                            formData.Add(new StringContent(licences.Subscrip_Domain.ToString().ToLower()), "Subscrip_Domain");
                            formData.Add(new StringContent(licences.Subscrip_Hosting.ToString().ToLower()), "Subscrip_Hosting");
                            formData.Add(new StringContent(db ?? ""), "DBName");





                            using(var client = new HttpClient())
                            {
                                //Base API URI
                                client.BaseAddress = new Uri(uri);
                                //JWT TOKEN
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                client.DefaultRequestHeaders
                                .Accept
                                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                //HTTP POST API
                                //var responseTask = client.PostAsync("api/ServiceRequest/SaveServiceRequest", null);

                                var res = await client.PostAsync("api/Licences/SaveLicence", formData);
                                res.EnsureSuccessStatusCode();  // This will throw an exception for non-success status codes

                                string responseBody = await res.Content.ReadAsStringAsync();
                                dynamic data = JsonConvert.DeserializeObject(responseBody);

                                // Access the 'token' property

                                //res.Wait();
                                Console.WriteLine(data);
                                if (data != null && data.statusCode == HttpStatusCode.OK)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;


                                }

                            }

                        }
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    return false;

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string getapitoken(string URI)
        {
            var result = "";
            try
            {

                using (var client = new HttpClient())
                {
                    //Base URI
                    client.BaseAddress = new Uri(URI);
                    //HTTP GET


                    //Http GET 
                    //Get Token API
                    var responseTask = client.GetAsync("api/Users/gettoken?userid=1");
                    responseTask.Wait();

                    var reslt = responseTask.Result;

                    result = reslt.Content.ReadAsStringAsync().Result;

                }



            }
            catch (Exception ex)
            {

            }
            return result;
        }

        private string DecryptValue(string value)
        {
            string hash = "f0xle@rn";
            byte[] data = Convert.FromBase64String(value); ;
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDesc = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform cryptoTransform = tripDesc.CreateDecryptor();
                    byte[] result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);
                    return Encoding.UTF8.GetString(result);
                }
            }
        }

    }
}

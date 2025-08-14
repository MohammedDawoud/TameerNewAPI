using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TaamerProject.API.pdfHandler;
using TaamerProject.Models.Common;
using QRCoder;
using System.Drawing;
using Dropbox.Api;
using Newtonsoft.Json.Linq;
using ZatcaIntegrationSDK;
using ZatcaIntegrationSDK.BLL;
using ZatcaIntegrationSDK.HelperContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaamerProject.Models.DBContext;
using OtpNet;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsersService _UsersService;
        private readonly ISys_UserLoginService _UserLoginService;
        private readonly IVersionService _Versionservice;
        private readonly IBranchesService _branchesService;
        private readonly IOrganizationsService _OrganizationService;
        private readonly IFiscalyearsService _FiscalyearsService;
        private readonly ICustomerService _CustomerService;
        private readonly IUserNotificationPrivilegesService _userNotificationPrivilegesService;
        private IFileService _fileservice;
        private string AdminUsername;
        private string AdminPassword;
        private string displayStringMiladi = "";
        private string Calnderformat = "dd/MM/yyyy";
        private string displayformat = "dd/MM/yyyy";
        private IServicesPricingFormService _servicesPricingFormService;
        private IOrganizationsService _organizationsservice;
        private ISocialMediaLinksService _socialMediaLinksService;
        private IContact_BranchesService _Contact_Branches;
        private IOffersPricesService _offersPricesService;
        private IFilesAuthService _filesAuthService;
        private INewsService _NewsService;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        string Con;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private byte[] ReportPDF;
        private readonly IConfiguration _configuration;
        private readonly TaamerProjectContext _TaamerProContext;

        public int userGroup = 1;
        public LoginController(TaamerProjectContext dataContext, IOffersPricesService offersPricesService, IFilesAuthService filesAuthService, IUsersService usersService, IVersionService versionService,
         IBranchesService branchesService, IOrganizationsService organizationsService, IFiscalyearsService fiscalyearsService, ICustomerService customerService,
         IUserNotificationPrivilegesService userNotificationPrivilegesService, ISys_UserLoginService userLoginService, IServicesPricingFormService servicesPricingFormService, IOrganizationsService organizations,
         UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ISocialMediaLinksService socialMediaLinksService, IContact_BranchesService contact_BranchesService, INewsService newsService, IFileService fileService, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _offersPricesService = offersPricesService;
            _UserLoginService = userLoginService;
            _filesAuthService = filesAuthService;
            _UsersService = usersService;
            _Versionservice = versionService;
            _branchesService = branchesService;
            _OrganizationService = organizationsService;
            _FiscalyearsService = fiscalyearsService;
            _userNotificationPrivilegesService = userNotificationPrivilegesService;
            this._CustomerService = customerService;
            this._servicesPricingFormService = servicesPricingFormService;
            this._organizationsservice = organizations;

            this._fileservice = fileService;

            this._socialMediaLinksService = socialMediaLinksService;
            this._Contact_Branches = contact_BranchesService;
            this._NewsService = newsService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            _hostingEnvironment = webHostEnvironment;

            this.AdminUsername = "tadmin";
            this.AdminPassword = "NFn7A3xMqqHH";
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _TaamerProContext = dataContext;

        }

        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(string password, string Link)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            //var link = _hostingEnvironment.WebRootPath;
            return Ok(_UsersService.ChangePasswordLink(Link, password, _globalshared.UserId_G, _globalshared.BranchId_G));
        }
        [HttpGet("Login")]
        public IActionResult Login(string username, string password, string? activationCode, string? remember, string? branch, string? returnUrl)
        {

            //try
            //{
            //    CreateFile();

            //    if (ChechVaild() == false)
            //    {
            //        // FirstTime.ShowDialog();

            //        //if (Lang == "ar")
            //        //{
            //        //    TempData["msg"] = "يجب تفعيل البرنامج ، من فضلك إتصل بالدعم الفني للتفعيل";
            //        //}
            //        //else
            //        //{
            //        //    TempData["msg"] = "The program must be activated, please contact technical support for activation";
            //        //}


            //        //return RedirectToAction("Index", "Login");
            //    }
            //    if (VaildationDemo() == false)
            //    {
            //        if (Lang == "ar")
            //        {
            //            TempData["msg"] = "يجب تفعيل البرنامج ، من فضلك إتصل بالدعم الفني للتفعيل";
            //        }
            //        else
            //        {
            //            TempData["msg"] = "The program must be activated, please contact technical support for activation";
            //        }
            //        return RedirectToAction("Index", "Login");
            //    }
            //    else if (programfulltime == true)
            //    {
            //        if (Lang == "ar")
            //        {
            //            TempData["msg"] = "يجب تفعيل البرنامج ، من فضلك إتصل بالدعم الفني للتفعيل";
            //        }
            //        else
            //        {
            //            TempData["msg"] = "The program must be activated, please contact technical support for activation";
            //        }
            //        return RedirectToAction("Index", "Login");
            //    }
            //}
            //catch (Exception)
            //{
            //    TempData["msg"] = "يجب تفعيل البرنامج ، من فضلك إتصل بالدعم الفني للتفعيل";
            //    return RedirectToAction("Index", "Login");
            //}
            //UsersData.License = "تم الترخيص";
            UsersData UsersData = new UsersData();

            if (username == AdminUsername && password == AdminPassword)
            {
                try
                {
                    var user = _UsersService.GetUser_tadmin("admin").Result;
                    var result = true;
                    string DateNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (result)
                    {
                        UsersData.UserName = user.UserName;
                        UsersData.UserId = user.UserId;
                        var ActiveYear = _FiscalyearsService.GetActiveYear();
                        UsersData.FiscalId_G = ActiveYear.FiscalId;
                        UsersData.YearId_G = ActiveYear.YearId;
                        string ExpireUserDate = user.ExpireDate;
                        UsersData.CurrentBranch = user.BranchName.ToString();
                        //Session["FullName"] = Lang == "ar" ? user.FullNameAr == null ? user.FullNameEn : user.FullNameAr : user.FullNameEn;
                        var BranchId_V = 0;
                        BranchId_V = user.BranchId ?? 0;
                        // BranchId = user.BranchId ?? 0;
                        var version = _Versionservice.GetVersion();
                        var Company = _OrganizationService.GetBranchOrganizationData(_branchesService.GetOrganizationId(Convert.ToInt32(BranchId_V)).Result).Result;
                        var pr = Privileges.PrivilegesList;
                        List<int> nvalues = new List<int>();
                        foreach (var item in pr)
                        {
                            nvalues.Add(item.Id);

                        }

                        var userPriv = _UsersService.GetPrivilegesIdsByUserId(user.UserId);
                        //UsersData.UserPrivileges = userPriv;
                        UsersData.UserPrivileges = nvalues;
                        var userNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(user.UserId).Result;
                        UsersData.UserNotifPrivileges = userNotifPriv;
                        UsersData.LogoUrl = Company.LogoUrl;
                        UsersData.CompanyID = Company.OrganizationId;
                        if (_globalshared.Lang_G == "ar")
                        {
                            UsersData.CompanyName = Company.NameAr;
                        }
                        else
                        {
                            UsersData.CompanyName = Company.NameEn;
                        }
                        UsersData.OrgVAT = Company.VAT ?? 0;
                        UsersData.BranchId = user.BranchId;
                        UsersData.DepartmentId = user.DepartmentId;
                        UsersData.DepartmentName = user.DepartmentName;

                        UsersData.UserId = user.UserId;
                        UsersData.UserName = AdminUsername;
                        UsersData.FullName = "الأدمن العام";
                        UsersData.Password = AdminPassword;
                        UsersData.IsAdmin = user.IsAdmin ?? false;
                        UsersData.Session = user.Session ?? 2;



                        UsersData.Token = Token(user.UserId, user.Password, user.UserName).ToString();


                        _UsersService.UpdateOnlineStatus2(true, user.UserId, user.UserId, BranchId_V);
                        return Ok(UsersData);

                    }
                    else
                    {
                        string msg;
                        if (_globalshared.Lang_G == "ar")
                        {
                            msg = "تاكد من بيانات الدخول من فضلك";
                        }
                        else
                        {
                            msg = "Please, Insure the data you used to log in";
                        }

                        return Ok(msg);
                    }

                }
                catch (Exception ex)
                {
                    string msg;

                    if (_globalshared.Lang_G == "ar")
                    {
                        msg = "خطأ فالإتصال بالانترنت";
                    }
                    else
                    {
                        msg = "There is an error in internet connction";
                    }
                    //Console.Write(ex.Message);
                    //Console.Write(ex.InnerException);

                    return Ok(ex.Message + "-----------" + ex.InnerException);
                }

            }
            else
            {
                try
                {



                    var user = _UsersService.GetUser(username).Result;
                    var pass = DecryptValue(user.Password);
                    bool IsOnline = false;
                    var result = _UsersService.ValidateUserCofidential(username, password, activationCode);
                    string DateNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    var min = 0;
                    var min2 = 0;
                    if (result)
                    {

                        if (user.ActiveTime != null)
                        {
                            DateTime ActiveUsertime = user.ActiveTime ?? DateTime.Now;
                            TimeSpan ts = DateTime.Now - ActiveUsertime;
                            min2 = ts.Minutes;
                            min = Math.Abs(min2);

                            if (min > 5)
                            {
                                IsOnline = false;
                            }
                            else
                            {
                                if (user.ISOnlineNew == null)
                                {
                                    IsOnline = false;
                                }
                                else
                                {
                                    IsOnline = user.ISOnlineNew ?? false;
                                }
                            }
                        }
                        else
                        {
                            if (user.ISOnlineNew == null)
                            {
                                IsOnline = false;
                            }
                            else
                            {
                                //IsOnline = user.ISOnlineNew ?? false;
                                IsOnline = false;
                            }
                        }
                        //hna l active year 
                        var ActiveYear = _FiscalyearsService.GetActiveYear();
                        UsersData.FiscalId_G = ActiveYear.FiscalId;
                        UsersData.YearId_G = ActiveYear.YearId;
                        string ExpireUserDate = user.ExpireDate;
                        UsersData.Session = user.Session ?? 2;

                        UsersData.Token = Token(user.UserId, user.Password, user.UserName).ToString();

                        if (ExpireUserDate != "0")
                        {
                            if (DateTime.ParseExact(ExpireUserDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) > DateTime.ParseExact(DateNow, "yyyy-MM-dd", CultureInfo.InvariantCulture))
                            {
                                string msg;
                                if (_globalshared.Lang_G != "en")
                                {
                                    msg = "صلاحية الحساب منتهية، الرجاء مراجعة مدير النظام";
                                }
                                else
                                {
                                    msg = "the account is expaired, please connect with the admin of the system";
                                }
                                return Ok(msg);
                            }
                            else
                            {
                                _UsersService.ClearExpireDate(user.UserId);
                            }
                        }
                        if (user.Status == 0)
                        {
                            string msg;
                            if (_globalshared.Lang_G != "en")
                            {
                                msg = "الحساب تم إيقافه، الرجاء مراجعة مدير النظام";
                            }
                            else
                            {
                                msg = "the account is stopped, please connect with the admin of the system";
                            }
                            return Ok(msg);
                        }
                        //&& username != "admin"
                        if (IsOnline != false)
                        {
                            string msg;
                            if (_globalshared.Lang_G != "en")
                            {
                                msg = "الحساب الذي تحاول الدخول به مستخدم ،وهناك احتمالان لهذه الرسالة،تأكد منها: الاول : احتمال ان تكون اغلقت البرنامج دون تسجيل خروج، عليك الانتظار لمدة 5 دقائق كاجراء امني. الثاني:قد يكون الحساب الذي تحاول الدخول به قد تم تسجيل الدخول به من خلال التطبيق، عليك تسجيل الخروج";
                                msg = "الحساب الذي تحاول الدخول به مستخدم ،وهناك احتمالان لهذه الرسالة،تأكد منها: الاول : احتمال ان تكون اغلقت البرنامج دون تسجيل خروج، عليك الانتظار لمدة 5 دقائق كاجراء امني. الثاني:قد يكون الحساب الذي تحاول الدخول به قد تم تسجيل الدخول به من خلال التطبيق، عليك تسجيل الخروج";
                            }
                            else
                            {
                                msg = "the account is Used now, please connect with the admin of the system";
                            }
                            return Ok(msg);
                        }
                        var BranchId_V = 0;
                        BranchId_V = user.BranchId ?? 0;
                        //BranchId = user.BranchId ?? 0;
                        var version = _Versionservice.GetVersion();
                        var Company = _OrganizationService.GetBranchOrganizationData(_branchesService.GetOrganizationId(Convert.ToInt32(BranchId_V)).Result).Result;

                        var userPriv = _UsersService.GetPrivilegesIdsByUserId(user.UserId);
                        UsersData.UserPrivileges = userPriv;
                        var userNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(user.UserId).Result;
                        UsersData.UserNotifPrivileges = userNotifPriv;

                        UsersData.LogoUrl = Company.LogoUrl;
                        _UsersService.UpdateLastLoginDate(user.UserId);
                        user = _UsersService.GetUser(username).Result;
                        UsersData.CompanyID = Company.OrganizationId;

                        if (_globalshared.Lang_G != "en")
                        {
                            //UsersData.BranchName = Branch.NameAr;
                            UsersData.CompanyName = Company.NameAr;
                        }
                        else
                        {
                            //  UsersData.BranchName = Branch.NameEn;
                            UsersData.CompanyName = Company.NameEn;

                        }
                        UsersData.OrgVAT = Company.VAT ?? 0;
                        UsersData.BranchId = user.BranchId;
                        UsersData.DepartmentId = user.DepartmentId;
                        UsersData.DepartmentName = user.DepartmentName;
                        UsersData.UserId = user.UserId;
                        UsersData.UserName = user.UserName ?? "";
                        UsersData.FullName = user.FullName ?? "";
                        UsersData.Password = DecryptValue(user.Password ?? "");
                        UsersData.IsAdmin = user.IsAdmin ?? false;

                        if (!string.IsNullOrEmpty(returnUrl))
                        {

                            _UsersService.UpdateOnlineStatus2(true, user.UserId, user.UserId, BranchId_V);
                            return Ok(UsersData);

                        }
                        else
                        {
                            string msg;

                            if (_globalshared.Lang_G != "en")
                            {
                                msg = "تاكد من بيانات الدخول من فضلك";
                            }
                            else
                            {
                                msg = "Please, Insure the data you used to log in";
                            }

                            return Ok(msg);
                        }
                    }
                    string msglo;

                    if (_globalshared.Lang_G != "en")
                    {
                        msglo = "تاكد من بيانات الدخول من فضلك";
                    }
                    else
                    {
                        msglo = "Please, Insure the data you used to log in";
                    }
                    //return BadRequest();
                    return Ok(msglo);
                }
                //System.Data.SqlClient.SqlException
                catch (Exception ex)
                {
                    string msg;
                    if (_globalshared.Lang_G != "en")
                    {
                        msg = "تاكد من بيانات الدخول من فضلك";
                    }
                    else
                    {
                        msg = "Please, Insure the data you used to log in";
                    }

                    return Ok(msg);
                }

            }
        }

        [HttpGet("LoginNew")]
        public IActionResult LoginNew(string username, string password, int? type)
        {

            //try
            //{
            //    CreateFile();

            //    if (ChechVaild() == false)
            //    {
            //        // FirstTime.ShowDialog();

            //        //if (Lang == "ar")
            //        //{
            //        //    TempData["msg"] = "يجب تفعيل البرنامج ، من فضلك إتصل بالدعم الفني للتفعيل";
            //        //}
            //        //else
            //        //{
            //        //    TempData["msg"] = "The program must be activated, please contact technical support for activation";
            //        //}


            //        //return RedirectToAction("Index", "Login");
            //    }
            //    if (VaildationDemo() == false)
            //    {
            //        if (Lang == "ar")
            //        {
            //            TempData["msg"] = "يجب تفعيل البرنامج ، من فضلك إتصل بالدعم الفني للتفعيل";
            //        }
            //        else
            //        {
            //            TempData["msg"] = "The program must be activated, please contact technical support for activation";
            //        }
            //        return RedirectToAction("Index", "Login");
            //    }
            //    else if (programfulltime == true)
            //    {
            //        if (Lang == "ar")
            //        {
            //            TempData["msg"] = "يجب تفعيل البرنامج ، من فضلك إتصل بالدعم الفني للتفعيل";
            //        }
            //        else
            //        {
            //            TempData["msg"] = "The program must be activated, please contact technical support for activation";
            //        }
            //        return RedirectToAction("Index", "Login");
            //    }
            //}
            //catch (Exception)
            //{
            //    TempData["msg"] = "يجب تفعيل البرنامج ، من فضلك إتصل بالدعم الفني للتفعيل";
            //    return RedirectToAction("Index", "Login");
            //}
            //UsersData.License = "تم الترخيص";
            UsersData UsersData = new UsersData();

            if (username == AdminUsername && password == AdminPassword)
            {
                try
                {
                    var user = _UsersService.GetUserLogin(username,"").Result;
                    var result = true;
                    string DateNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (result)
                    {
                        UsersData.UserName = user.UserName;
                        UsersData.UserId = user.UserId;
                        var ActiveYear = _FiscalyearsService.GetActiveYear();
                        UsersData.FiscalId_G = ActiveYear.FiscalId;
                        UsersData.YearId_G = ActiveYear.YearId;
                        string ExpireUserDate = user.ExpireDate;
                        UsersData.CurrentBranch = user.BranchName.ToString();
                        //Session["FullName"] = Lang == "ar" ? user.FullNameAr == null ? user.FullNameEn : user.FullNameAr : user.FullNameEn;
                        var BranchId_V = 0;
                        BranchId_V = user.BranchId ?? 0;
                        // BranchId = user.BranchId ?? 0;
                        var version = _Versionservice.GetVersion();
                        var Company = _OrganizationService.GetBranchOrganizationData(_branchesService.GetOrganizationId(Convert.ToInt32(BranchId_V)).Result).Result;
                        var pr = Privileges.PrivilegesList;
                        List<int> nvalues = new List<int>();
                        foreach (var item in pr)
                        {
                            nvalues.Add(item.Id);

                        }

                        var userPriv = _UsersService.GetPrivilegesIdsByUserId(user.UserId);
                        //UsersData.UserPrivileges = userPriv;
                        UsersData.UserPrivileges = nvalues;
                        var userNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(user.UserId).Result;
                        UsersData.UserNotifPrivileges = userNotifPriv;
                        UsersData.LogoUrl = Company.LogoUrl;
                        UsersData.CompanyID = Company.OrganizationId;
                        if (_globalshared.Lang_G == "ar")
                        {
                            UsersData.CompanyName = Company.NameAr;
                        }
                        else
                        {
                            UsersData.CompanyName = Company.NameEn;
                        }
                        UsersData.OrgVAT = Company.VAT ?? 0;
                        UsersData.BranchId = user.BranchId;
                        UsersData.DepartmentId = user.DepartmentId;
                        UsersData.DepartmentName = user.DepartmentName;

                        UsersData.UserId = user.UserId;
                        UsersData.UserName = AdminUsername;
                        UsersData.FullName = "الأدمن العام";
                        UsersData.Password = AdminPassword;
                        UsersData.IsAdmin = user.IsAdmin ?? false;
                        UsersData.Session = user.Session ?? 2;


                        var secretBytes = KeyGeneration.GenerateRandomKey(20); // 160-bit key
                        var base32Secret = Base32Encoding.ToString(secretBytes);
                        var issuer = "TameerCloudApp";
                        var qrCodeUrl = $"otpauth://totp/{issuer}:{user.Email}?secret={base32Secret}&issuer={issuer}&digits=6";

                        UsersData.base32Secret = base32Secret;
                        UsersData.qrCodeUrl = qrCodeUrl;
                        UsersData.Token = GenerateJwtToken(user.UserId, user.Password, user.UserName);

                        //UsersData.Token = Token(user.UserId, user.Password, user.UserName).ToString();


                        _UsersService.UpdateOnlineStatus2(true, user.UserId, user.UserId, BranchId_V);
                        return Ok(UsersData);

                    }
                    else
                    {
                        string msg;
                        if (_globalshared.Lang_G == "ar")
                        {
                            msg = "تاكد من بيانات الدخول من فضلك";
                        }
                        else
                        {
                            msg = "Please, Insure the data you used to log in";
                        }

                        return Ok(msg);
                    }

                }
                catch (Exception ex)
                {
                    string msg;

                    if (_globalshared.Lang_G == "ar")
                    {
                        msg = "خطأ فالإتصال بالانترنت";
                    }
                    else
                    {
                        msg = "There is an error in internet connction";
                    }
                    return Ok(ex.Message + "-----------" + ex.InnerException);
                }

            }
            else
            {
                try
                {
                    var pass = EncryptValue(password);
                    var user = new UsersLoginVM();
                    if (type==2 || type == 3)
                        user = _UserLoginService.GetUserLogin(username, pass, type ?? 0).Result;
                    else 
                        user = _UsersService.GetUserLogin(username, pass).Result;


                    bool IsOnline = false;
                    string DateNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    var min = 0;
                    //var min2 = 0;
                    if (user!=null)
                    {

                        if (user.ActiveTime != null)
                        {
                            DateTime ActiveUsertime = user.ActiveTime ?? DateTime.Now;
                            TimeSpan ts = DateTime.Now - ActiveUsertime;
                            min = ts.Minutes;
                            min = Math.Abs(min);

                            if (min > 5)
                            {
                                IsOnline = false;
                            }
                            else
                            {
                                if (user.ISOnlineNew == null)
                                {
                                    IsOnline = false;
                                }
                                else
                                {
                                    IsOnline = user.ISOnlineNew ?? false;
                                }
                            }
                        }
                        else
                        {
                            if (user.ISOnlineNew == null)
                            {
                                IsOnline = false;
                            }
                            else
                            {
                                //IsOnline = user.ISOnlineNew ?? false;
                                IsOnline = false;
                            }
                        }
                        //hna l active year 
                        var ActiveYear = _FiscalyearsService.GetActiveYear();
                        UsersData.FiscalId_G = ActiveYear.FiscalId;
                        UsersData.YearId_G = ActiveYear.YearId;
                        string ExpireUserDate = user.ExpireDate;
                        UsersData.Session = user.Session ?? 2;


                        var secretBytes = KeyGeneration.GenerateRandomKey(20); // 160-bit key
                        var base32Secret = Base32Encoding.ToString(secretBytes);
                        var issuer = "TameerCloudApp";
                        var qrCodeUrl = $"otpauth://totp/{issuer}:{user.Email}?secret={base32Secret}&issuer={issuer}&digits=6";

                        UsersData.base32Secret = base32Secret;
                        UsersData.qrCodeUrl = qrCodeUrl;
                        UsersData.Token = GenerateJwtToken(user.UserId, user.Password, user.UserName);

                        //UsersData.Token = Token(user.UserId, user.Password, user.UserName).ToString();

                        if (ExpireUserDate != "0")
                        {
                            if (DateTime.ParseExact(ExpireUserDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) > DateTime.ParseExact(DateNow, "yyyy-MM-dd", CultureInfo.InvariantCulture))
                            {
                                string msg;
                                if (_globalshared.Lang_G != "en")
                                {
                                    msg = "صلاحية الحساب منتهية، الرجاء مراجعة مدير النظام";
                                }
                                else
                                {
                                    msg = "the account is expaired, please connect with the admin of the system";
                                }
                                return Ok(msg);
                            }
                            else
                            {
                                _UsersService.ClearExpireDate(user.UserId);
                            }
                        }
                        if (user.Status == 0)
                        {
                            string msg;
                            if (_globalshared.Lang_G != "en")
                            {
                                msg = "الحساب تم إيقافه، الرجاء مراجعة مدير النظام";
                            }
                            else
                            {
                                msg = "the account is stopped, please connect with the admin of the system";
                            }
                            return Ok(msg);
                        }
                        if (IsOnline != false)
                        {
                            string msg;
                            if (_globalshared.Lang_G != "en")
                            {
                                msg = "الحساب الذي تحاول الدخول به مستخدم ،وهناك احتمالان لهذه الرسالة،تأكد منها: الاول : احتمال ان تكون اغلقت البرنامج دون تسجيل خروج، عليك الانتظار لمدة 5 دقائق كاجراء امني. الثاني:قد يكون الحساب الذي تحاول الدخول به قد تم تسجيل الدخول به من خلال التطبيق، عليك تسجيل الخروج";
                            }
                            else
                            {
                                msg = "the account is Used now, please connect with the admin of the system";
                            }
                            return Ok(msg);
                        }
                        var BranchId_V = 0;
                        BranchId_V = user.BranchId ?? 0;
                        //BranchId = user.BranchId ?? 0;
                        var version = _Versionservice.GetVersion();
                        var Company = _OrganizationService.GetBranchOrganizationData(_branchesService.GetOrganizationId(Convert.ToInt32(BranchId_V)).Result).Result;

                        var userPriv = _UsersService.GetPrivilegesIdsByUserId(user.UserId);
                        UsersData.UserPrivileges = userPriv;
                        var userNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(user.UserId).Result;
                        UsersData.UserNotifPrivileges = userNotifPriv;

                        UsersData.LogoUrl = Company.LogoUrl;
                        _UsersService.UpdateLastLoginDate(user.UserId);
                        UsersData.CompanyID = Company.OrganizationId;

                        if (_globalshared.Lang_G != "en")
                        {
                            //UsersData.BranchName = Branch.NameAr;
                            UsersData.CompanyName = Company.NameAr;
                        }
                        else
                        {
                            //  UsersData.BranchName = Branch.NameEn;
                            UsersData.CompanyName = Company.NameEn;

                        }
                        UsersData.OrgVAT = Company.VAT ?? 0;
                        UsersData.BranchId = user.BranchId;
                        UsersData.DepartmentId = user.DepartmentId;
                        UsersData.DepartmentName = user.DepartmentName;
                        UsersData.UserId = user.UserId;
                        UsersData.UserName = user.UserName ?? "";
                        UsersData.FullName = user.FullName ?? "";
                        UsersData.Password = DecryptValue(user.Password ?? "");
                        UsersData.IsAdmin = user.IsAdmin ?? false;

                        _UsersService.UpdateOnlineStatus2(true, user.UserId, user.UserId, BranchId_V);
                        return Ok(UsersData);

                    }

                    string msglo;

                    if (_globalshared.Lang_G != "en")
                    {
                        msglo = "تاكد من بيانات الدخول من فضلك";
                    }
                    else
                    {
                        msglo = "Please, Insure the data you used to log in";
                    }
                    return Ok(msglo);
                }
                catch (Exception ex)
                {
                    string msg;
                    if (_globalshared.Lang_G != "en")
                    {
                        msg = "تاكد من بيانات الدخول من فضلك";
                    }
                    else
                    {
                        msg = "Please, Insure the data you used to log in";
                    }

                    return Ok(msg);
                }

            }
        }


        [HttpPost("ValidateRecaptchaAsync")]
        public async Task<IActionResult> ValidateRecaptchaAsync([FromForm] string recaptchaResponse, [FromForm] string secretKey)
        {
            RecaptchaValidator _RecaptchaValidator = new RecaptchaValidator();
            var isValid = await _RecaptchaValidator.ValidateRecaptchaAsync(recaptchaResponse, secretKey);
            if (isValid)
            {
                // reCAPTCHA is valid, proceed with your application's logic
                return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "reCAPTCHA is valid, proceed with your application's logic" });
            }
            else
            {
                // reCAPTCHA is invalid, return an error
                return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "reCAPTCHA is invalid" });
            }
        }
        [HttpGet("UpdateActiveTime")]

        public void UpdateActiveTime()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            _UsersService.UpdateActiveTime(_globalshared.UserId_G, _globalshared.BranchId_G);
        }

        //-----------------------------------------------------------------------------
        #region
        [HttpPost("Setup2FA")] //Step-1
        public async Task<IActionResult> Setup2FA()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            _globalshared.UserId_G = 1;
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _TaamerProContext.Users.Where(s=>s.UserId== _globalshared.UserId_G).FirstOrDefaultAsync();

            // Generate TOTP key
            var secretBytes = KeyGeneration.GenerateRandomKey(20); // 160-bit key
            var base32Secret = Base32Encoding.ToString(secretBytes);

           // user.AuthenticatorSecret = base32Secret;
            //await _TaamerProContext.SaveChangesAsync();

            // QR Code URL for Google Authenticator
            var issuer = "TameerCloudApp";
            var qrCodeUrl = $"otpauth://totp/{issuer}:{user.Email}?secret={base32Secret}&issuer={issuer}&digits=6";

            return Ok(new { secret = base32Secret, qrCodeUrl });
        }

        //[HttpPost("Verify2FA")]
        //public async Task<IActionResult> Verify2FA([FromBody] string code)
        //{
        //    HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        //    _globalshared.UserId_G = 1;
        //    var user = await _TaamerProContext.Users.Where(s => s.UserId == _globalshared.UserId_G).FirstOrDefaultAsync();

        //    if (string.IsNullOrEmpty(user.AuthenticatorSecret))
        //        return BadRequest("2FA is not setup");

        //    var totp = new Totp(Base32Encoding.ToBytes(user.AuthenticatorSecret));
        //    bool isValid = totp.VerifyTotp(code, out long timeStepMatched, new VerificationWindow(2, 2));

        //    if (!isValid)
        //        return BadRequest("Invalid code");

        //    user.Is2FAEnabled = true;
        //    await _dbContext.SaveChangesAsync();

        //    return Ok("2FA enabled successfully");
        //}

        //[HttpPost("Login22")]
        //public async Task<IActionResult> Login22([FromBody] LoginDto dto)
        //{
        //    var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        //    if (user == null || !VerifyPassword(dto.Password, user.PasswordHash))
        //        return Unauthorized("Invalid credentials");

        //    if (user.Is2FAEnabled)
        //    {
        //        // Wait for client to send 2FA code
        //        return Ok(new { requires2FA = true, userId = user.Id });
        //    }

        //    var token = GenerateJwtToken(user);
        //    return Ok(new { token });
        //}
        //[HttpPost("2fa-login")]
        //public async Task<IActionResult> TwoFactorLogin([FromBody] TwoFactorLoginDto dto)
        //{
        //    var user = await _dbContext.Users.FindAsync(dto.UserId);

        //    var totp = new Totp(Base32Encoding.ToBytes(user.AuthenticatorSecret));
        //    bool isValid = totp.VerifyTotp(dto.Code, out _, new VerificationWindow(2, 2));

        //    if (!isValid)
        //        return Unauthorized("Invalid 2FA code");

        //    var token = GenerateJwtToken(user);
        //    return Ok(new { token });
        //}

        //public class TwoFactorLoginDto
        //{
        //    public Guid UserId { get; set; }
        //    public string Code { get; set; }
        //}
        #endregion
        //----------------------------------------------------------------------------------------------

        [HttpGet("enable-2fa")] //1
        public async Task<IActionResult> Enable2FA() 
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            //var user = _UsersService.GetUserById2(_globalshared.UserId_G, _globalshared.Lang_G).Result;
            var user = await _userManager.GetUserAsync(User);
            var key = await _userManager.GetAuthenticatorKeyAsync(user);
            //var key = "";
            if (string.IsNullOrEmpty(key))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                key = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            var qrCodeUrl = $"otpauth://totp/YourApp:{user.Email}?secret={key}&issuer=YourApp&digits=6";
            return Ok(new { key, qrCodeUrl });
        }
        [HttpPost("verify-2fa")] //2
        public async Task<IActionResult> Verify2FA([FromBody] Verify2FARequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            var isValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, TokenOptions.DefaultAuthenticatorProvider, request.Code);

            if (!isValid)
                return BadRequest("Invalid code");

            user.TwoFactorEnabled = true;
            await _userManager.UpdateAsync(user);

            return Ok("2FA enabled");
        }

        public class Verify2FARequest
        {
            public string Code { get; set; }
        }

        //[HttpPost("2fa-login")]
        //public async Task<IActionResult> TwoFactorLogin([FromBody] TwoFactorLoginRequest request)
        //{
        //    var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        //    var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(request.Code, false, false);

        //    if (!result.Succeeded)
        //        return Unauthorized();

        //    // Generate JWT token here
        //    var token = GenerateJwtToken(user);

        //    return Ok(new { token });
        //}

        public class TwoFactorLoginRequest
        {
            public string Code { get; set; }
        }
        private string GenerateJwtToken(int UserId, string Password, string UserName)
        {
            var apiResponse = "";

            try
            {

                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("2fa_verified", "true"),
                        new Claim("mfa", "true"),
                        new Claim("UserId",UserId.ToString()),
                        new Claim("Password", Password.ToString()),
                        new Claim("UserName", UserName.ToString())


                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddHours(9),
                    signingCredentials: signIn);

                string _token = new JwtSecurityTokenHandler().WriteToken(token);

                apiResponse = _token.ToString();


            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                apiResponse = ex.Message;
            }
            catch (Exception ex)
            {
                apiResponse = ex.Message;

            }

            return apiResponse;
        }

        //public bool Validate()
        //{

        //    var licenseKey = "JRGLL-ZMUQE-OJGJC-SYQDU1";
        //    var RSAPubKey = "<RSAKeyValue><Modulus>4RuSkcXpoCYbbyEriMth4Pyt5bQKNZaIptmVYWB8wrj7q4Aw6Se4q6BO0L+9mOeBSrObLlZtaDZSmZp9FlvM71nyxf6LVBlM8Tge38YHh7BX1OyCAkgvF+zxpuSVcTlJ100PS1aQjHHziHv9LiB8aF88tq3m9m6J7RjwF91dXF+urI4b4ycYOE6q2zGDZgWdr911cUURHFyZbNOF9rqJvcoojE4185D3xLW5yFo2kcsudT51uO4mUor4tCz3Iyxgt1+9UfFhFVMSjc6iSW5NNSI5EKRN6BCmV3IL4M0AAAugfYQI1iPAaOFUAIRhqr69Hg1BVgtOZMVaBXSeFkhFYQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        //    var auth = "WyIxODUwOTk4Iiwia291blpSVm5PSnI1NHpwRHlRcHB4UHVVcXNCbWh1eFZVYmFPRDZSayJd";
        //    var result = Key.Activate(token: auth, parameters: new ActivateModel()
        //    {
        //        Key = licenseKey,
        //        ProductId = 11270,
        //        Sign = true,
        //        MachineCode = Helpers.GetMachineCode()
        //    });

        //    if (result == null || result.Result == ResultType.Error || result.LicenseKey.HasValidSignature(RSAPubKey).IsValid())
        //    {

        //        var licensefile = new LicenseKey();

        //        if (licensefile.LoadFromFile("licensefile")
        //                      .HasValidSignature(RSAPubKey, 3)
        //                      .IsValid())
        //        {
        //            Console.WriteLine("The license is valid!");
        //        }
        //        else
        //        {
        //            Console.WriteLine("The license does not work.");
        //        }

        //        return false;
        //    }
        //    else
        //    {


        //        var StartDate = result.LicenseKey.Created.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        //        var ExpiredDate = result.LicenseKey.Expires.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        //        var maxmachno = result.LicenseKey.MaxNoOfMachines;
        //        var F1 = result.LicenseKey.F1;
        //        var F2 = result.LicenseKey.F2;
        //        var F3 = result.LicenseKey.F3;
        //        var F4 = result.LicenseKey.F4;
        //        var F5 = result.LicenseKey.F5;
        //        var F6 = result.LicenseKey.F6;
        //        var F7 = result.LicenseKey.F7;
        //        var F8 = result.LicenseKey.F8;
        //        var Period = result.LicenseKey.Period;
        //        var Block = result.LicenseKey.Block;
        //        var TrialActivation = result.LicenseKey.TrialActivation;


        //        result.LicenseKey.SaveToFile("licensefile");


        //        return true;
        //    }
        //}


        //public bool ValidateLicense()
        //{
        //    string validationKey = "AMAAMACCdFGysYZedxbq+OFr1suh4bqbGVRx9oC8C4yfLB6MSE+dRdo/jcmORXxojQp6XasDAAEAAQ==";

        //    CryptoLicense license = new CryptoLicense("<<license code>>", validationKey);
        //    if (license.Status != LicenseStatus.Valid)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }


        //}

        //public IActionResult ChangeBranch(int BranchID)
        //{
        //    var Branch = _branchesService.GetBranchById(Convert.ToInt32(BranchID));
        //    BranchId = Branch.BranchId;
        //    UsersData.BranchId = Branch.BranchId;
        //    UsersData.BranchName = Branch.NameAr;
        //    FormsAuthenticationTicket aut = new FormsAuthenticationTicket(1, UserId.ToString(), DateTime.Now, DateTime.Now.AddDays(1), false, Convert.ToString(BranchID));
        //    HttpCookie cookiee = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(aut));
        //    Response.Cookies.Add(new HttpCookie("branchId", Convert.ToString(BranchID)));
        //    string encTicket = FormsAuthentication.Encrypt(aut);

        //    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
        //    var massage = "";
        //    if (Lang == "ar")
        //    {
        //        massage = "تم تغيير الفرع بنجاح";
        //    }
        //    else
        //    {
        //        massage = "Branch Changed Successfully";
        //    }
        //    return Ok(true, massage, JsonRequestBehavior.AllowGet);
        //}

        //public IActionResult ChangeYear(int FiscalId, int BranchID)
        //{

        //    var YearId = Convert.ToInt32(_FiscalyearsService.GetYearID(Convert.ToInt32(FiscalId)));
        //    var x2 = Request.Cookies["ActiveYear"].Value;
        //    UsersData.FiscalId_G = FiscalId;
        //    UsersData.YearId_G = YearId;
        //    FormsAuthenticationTicket aut = new FormsAuthenticationTicket(1, UserId.ToString(), DateTime.Now, DateTime.Now.AddDays(1), false, Convert.ToString(BranchID));
        //    HttpCookie cookiee = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(aut));
        //    Response.Cookies.Add(new HttpCookie("ActiveYear", Convert.ToString(FiscalId)));
        //    string encTicket = FormsAuthentication.Encrypt(aut);

        //    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

        //    Request.Cookies["ActiveYear"].Value = Convert.ToString(FiscalId);
        //    var x = Request.Cookies["ActiveYear"].Value;


        //    var year = UsersData.YearId_G;


        //    var massage = "";
        //    if (Lang == "ar")
        //    {
        //        massage = "تم تغيير السنة المالية بنجاح";
        //    }
        //    else
        //    {
        //        massage = "Fiscal year Changed Successfully";
        //    }
        //    return Ok(true, massage, JsonRequestBehavior.AllowGet);
        //}


        [HttpGet("GetFileByBarcode")]
        public IActionResult GetFileByBarcode(string Barcode)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string TaxCode = _OrganizationService.GetBranchOrganization().Result.TaxCode;

            return Ok(_fileservice.GetFileByBarcode(Barcode, TaxCode));
        }
        [HttpGet("ADDFileComment")]
        public IActionResult ADDFileComment(int FileId, string Comment)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var url = Path.Combine("~/Email/MailStamp.html");

            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            //var file = Path.Combine("~") + org.LogoUrl;
            var file = "";
            if (org.LogoUrl != null && org.LogoUrl != "")
            {
                string resulta = org.LogoUrl.Remove(0, 1);
                file = Path.Combine(resulta);
            }
            var result = _fileservice.ADDFileComment(FileId, _globalshared.UserId_G, _globalshared.BranchId_G, Comment, url, file);
            if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
            {
                result.ReasonPhrase = "Saved Successfully";
            }
            else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
            {
                result.ReasonPhrase = "Saved Falied";
            }
            return Ok(new { result.StatusCode, result.ReasonPhrase });
        }

        [HttpGet("GetFileByBarcodeShare")]
        public IActionResult GetFileByBarcodeShare(string ProjectNo)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string TaxCode = _OrganizationService.GetBranchOrganization().Result.TaxCode;

            return Ok(_fileservice.GetFileByBarcodeShare(ProjectNo, TaxCode));
        }
        [HttpGet("GetFileByBarcode2")]
        public IActionResult GetFileByBarcode2(string Barcode)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            string TaxCode = _OrganizationService.GetBranchOrganization().Result.TaxCode;

            return Ok(_fileservice.GetFileByBarcode2(Barcode, TaxCode));
        }
        [HttpGet("GetOrgName")]
        public IActionResult GetOrgName()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            string Name = _OrganizationService.GetBranchOrganization().Result.NameAr;
            return Ok(Name);
        }

        [HttpGet("GetCustomerInfo")]
        public IActionResult GetCustomerInfo(string SearchText)
        {
            //dawoud
            var CustomerInfo = _CustomerService.GetCustomerInfo(SearchText);
            var check = 0;
            if (CustomerInfo == null)
            {
                check = 0;
            }
            else
            {
                check = 1;

            }
            return Ok(check);
        }
        [HttpPost("SavePricingForm")]
        public IActionResult SavePricingForm(ServicesPricingForm Form)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            try
            {
                var result = _servicesPricingFormService.SaveServicesPricingForm(Form, _globalshared.UserId_G, _globalshared.BranchId_G);

                if (result.ReturnedParm != 0)
                {
                    var FormData = _servicesPricingFormService.GetServicesPricingFormById(result.ReturnedParm ?? 0, _globalshared.BranchId_G).Result;

                    var objOrganization = _organizationsservice.GetBranchOrganization().Result;
                    string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email,
                objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
                    ReportPDF = ReportsOf7sabat.PrintServPriceForm(FormData, infoDoneTasksReport, "", "");

                    string existTemp = System.IO.Path.Combine("ServicePriceFormPDF/");
                    if (!Directory.Exists(existTemp))
                    {
                        Directory.CreateDirectory(existTemp);
                    }
                    string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
                    string FilePath = System.IO.Path.Combine("ServicePriceFormPDF/", FileName);
                    System.IO.File.WriteAllBytes(FilePath, ReportPDF);
                    string FilePathReturn = "/ServicePriceFormPDF/" + FileName;

                    Form.URLFile = FilePathReturn;
                    var result22 = _servicesPricingFormService.UpdateURL(Form, 1);


                    //}
                    if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
                    {
                        result.ReasonPhrase = "Saved Successfully";
                    }
                    else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
                    {
                        result.ReasonPhrase = "Saved Falied";
                    }
                    return Ok(result);
                }
                else
                {
                    var massege = "فشل في الحفظ";
                    return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massege });
                }
            }
            catch (Exception ex)
            {
                var massege = "فشل في الحفظ";
                return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massege, ReturnedStrNeeded = ex.Message });
            }

        }
        [HttpGet("EncryptValue")]
        private string EncryptValue(string value)
        {
            string hash = "f0xle@rn";
            byte[] data = Encoding.UTF8.GetBytes(value);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDesc = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform cryptoTransform = tripDesc.CreateEncryptor();
                    byte[] result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(result, 0, result.Length);
                }
            }
        }
        [HttpGet("DecryptValue")]
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

        [HttpPost("GetUserBranches")]
        public IActionResult GetUserBranches(string UserName)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_UsersService.GetAllBranchesByUserName(_globalshared.Lang_G, UserName));
        }

        [HttpPost("ProcessActivationCode")]
        public IActionResult ProcessActivationCode(string UserName, string PassWord)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_UsersService.ProcessActivationCode(UserName, PassWord, _globalshared.BranchId_G));
        }

        [HttpPost("RetrievePassword")]

        public IActionResult RetrievePassword(string Forgetusername, string email)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            var file = "";
            if (org.LogoUrl != null && org.LogoUrl != "")
            {
                string result = org.LogoUrl.Remove(0, 1);
                file = Path.Combine(result);
            }
            //var file = Server.MapPath("~/dist/assets/images/logo.png");
            return Ok(_UsersService.RetrievePassword(Forgetusername, email, file, PopulateBody(2, "", Path.Combine("Email/RetrievePassword.html"))));
        }
        [HttpPost("ForgetPassword")]
        public IActionResult ForgetPassword(string? Forgetusername, string email)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string resetCode = Guid.NewGuid().ToString();
            var verifyUrl = "Login/ResetPassword/" + resetCode;
            var link = Path.Combine(_hostingEnvironment.WebRootPath, verifyUrl);
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            var file = "";
            if (org.LogoUrl != null && org.LogoUrl != "")
            {
                string result = org.LogoUrl.Remove(0, 1);
                file = Path.Combine(result);
            }

            return Ok(_UsersService.ForgetPassword(email, link, file, PopulateBody(1, link, Path.Combine("Email/ForgetPassword.html"))));
        }
        [HttpPost("ForgetPasswordError")]
        public IActionResult ForgetPasswordError(string email, string Link)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            string resetCode = Guid.NewGuid().ToString();
            //var verifyUrl = "Login/ResetPassword/" + resetCode;
            var verifyUrl = Link + "/" + resetCode;

            //var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            //var link = Path.Combine(_hostingEnvironment.WebRootPath, verifyUrl);
            //var link = Path.Combine(verifyUrl);


            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            var file = "";
            if (org.LogoUrl != null && org.LogoUrl != "")
            {
                string result = org.LogoUrl.Remove(0, 1);
                file = Path.Combine(result);
            }


            return Ok(_UsersService.ForgetPasswordError(email, verifyUrl, file, PopulateBody(1, verifyUrl, Path.Combine("Email/ForgetPassword.html"))));
        }
        [HttpGet("PopulateBody")]
        public string PopulateBody(int type, string EmailUrl, string url)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string body = string.Empty;
            var org = _organizationsservice.GetOrganizationData(_globalshared.BranchId_G).Result;
            using (StreamReader reader = new StreamReader(Path.Combine(url)))
            {
                body = reader.ReadToEnd();
            }
            if (type == 1) { body = body.Replace("{EmailUrl}", EmailUrl); body = body.Replace("{orgname}", org.NameAr); }
            else if (type == 2) { body = body.Replace("{orgname}", org.NameAr); }



            return body;
        }


        //public IActionResult CheckISOnline1()
        //{
        ////authTicket.Name
        //HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

        //try
        //{
        //        var result = _UsersService.CheckISOnline(UserId);

        //        if (result != null)
        //        {
        //            if (result.ISOnlineNew == false)
        //            {
        //                _UsersService.UpdateOnlineStatus2(false, Convert.ToInt32(UserId), Convert.ToInt32(UserId), BranchId);
        //                HttpContext.Session.Abandon();
        //                FormsAuthentication.SignOut();
        //                return RedirectToAction("Index", "Login");
        //            }
        //            else
        //            {
        //                return Ok(0, JsonRequestBehavior.AllowGet);

        //            }
        //        }
        //        else
        //        {
        //            _UsersService.UpdateOnlineStatus2(false, Convert.ToInt32(UserId), Convert.ToInt32(UserId), BranchId);
        //            HttpContext.Session.Abandon();
        //            FormsAuthentication.SignOut();
        //            return RedirectToAction("Index", "Login");
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        HttpContext.Session.Abandon();
        //        FormsAuthentication.SignOut();
        //        return RedirectToAction("Index", "Login");
        //    }


        //}
        //public int CheckISOnline()
        //{
        //    try
        //    {
        //        var result = _UsersService.CheckISOnline(UserId);

        //        if (result != null)
        //        {
        //            if (result.ISOnlineNew == false)
        //            {
        //                HttpContext.Session.Abandon();
        //                var authCookie = Request.Cookies.Get((FormsAuthentication.FormsCookieName));
        //                if (authCookie != null)
        //                {
        //                    var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
        //                    _UsersService.UpdateOnlineStatus2(false, Convert.ToInt32(authTicket.Name), Convert.ToInt32(authTicket.Name), BranchId);
        //                }
        //                HttpContext.Session.Abandon();
        //                FormsAuthentication.SignOut();
        //                return 1;
        //            }
        //            else
        //            {
        //                return 0;

        //            }
        //        }
        //        else
        //        {
        //            _UsersService.UpdateOnlineStatus2(false, Convert.ToInt32(UserId), Convert.ToInt32(UserId), BranchId);
        //            HttpContext.Session.Abandon();
        //            FormsAuthentication.SignOut();
        //            return 1;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        FormsAuthentication.SignOut();
        //        return 1;
        //    }
        //}
        [HttpPost("LogOutUpdate")]

        public IActionResult LogOutUpdate()
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            _UsersService.UpdateOnlineStatus2(false, _globalshared.UserId_G, _globalshared.UserId_G, _globalshared.BranchId_G);
            //}
            //FormsAuthentication.SignOut();
            return Ok();

            //Session.Abandon();
            //var authCookie = Request.Cookies.Get((FormsAuthentication.FormsCookieName));
            //if (authCookie != null)
            //{
            //    var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            //    _UsersService.UpdateOnlineStatus2(false, _globalshared.UserId_G, _globalshared.UserId_G, _globalshared.BranchId_G);
            //}
            //FormsAuthentication.SignOut();
            //return RedirectToAction("Index", "Login");

        }
        [HttpPost("ChangeOnlineStatus")]

        public IActionResult ChangeOnlineStatus(bool? status)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            _UsersService.UpdateOnlineStatus2(status ?? false, _globalshared.UserId_G, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok();
        }
        [HttpGet("LogOut")]
        public IActionResult LogOut(int userid)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            if (userid != 0)
            {
                _UsersService.UpdateOnlineStatus2(false, userid, userid, _globalshared.BranchId_G);

            }
            return Ok();
        }
        //public IActionResult Lock()
        //{
        //    Session.Abandon();
        //    var authCookie = Request.Cookies.Get((FormsAuthentication.FormsCookieName));
        //    var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
        //    _UsersService.UpdateOnlineStatus2(false, Convert.ToInt32(authTicket.Name), Convert.ToInt32(authTicket.Name), BranchId);
        //    FormsAuthentication.SignOut();
        //    return RedirectToAction("LockUser", "Login");
        //}
        //public IActionResult Lock2()
        //{
        //    //Session.Abandon();
        //    var authCookie = Request.Cookies.Get((FormsAuthentication.FormsCookieName));
        //    var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
        //    _UsersService.UpdateOnlineStatus2(false, Convert.ToInt32(authTicket.Name), Convert.ToInt32(authTicket.Name), BranchId);
        //    //FormsAuthentication.SignOut();
        //    return RedirectToAction("LockUser", "Login");
        //}
        [HttpPost("Keepalive")]
        public IActionResult Keepalive()
        {
            return Ok("OK");
        }
        [HttpPost("ResetSessionTimeout")]
        public IActionResult ResetSessionTimeout()
        {
            //Session.Timeout = Session.Timeout + 1;

            return Ok("");
        }

        [HttpGet("FillBranchSelect")]
        public IActionResult FillBranchSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_branchesService.GetAllBranches(_globalshared.Lang_G).Result.Select(s => new
            {
                Id = s.BranchId,
                Name = s.BranchName
            }));
        }

        [HttpPost("ChangeLanguage")]
        public IActionResult ChangeLanguage(string lang)
        {
            //try
            //{
            //    var cultureInfo = new CultureInfo(lang);
            //    Thread.CurrentThread.CurrentUICulture = cultureInfo;
            //    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
            //    HttpCookie langCookie = new HttpCookie("culture", lang);
            //    langCookie.Expires = DateTime.Now.AddYears(1);
            //    HttpContext.Response.Cookies.Add(langCookie);
            //}
            //catch (Exception) { }
            return Ok();
        }




        private bool fileexist = false;
        private bool reg2 = false;
        private bool reg3 = false;
        private bool reg4 = false;
        private bool reg5 = false;
        private bool reg6 = false;
        private bool dbexist = false;
        public string regvalue1 = "";
        public string regvalue2 = "";
        public string regvalue3 = "";
        public string regvalue4 = "";
        public string regvalue5 = "";
        public string regvalue6 = "";
        public string LicenceTool = "";

        public string cpuInfo = string.Empty;

        private bool programdemo = false;
        private bool programfulltime = false;
        private System.Globalization.DateTimeFormatInfo DTFormat2 = new System.Globalization.CultureInfo("", false).DateTimeFormat;

        //private bool GoToActiveUser()
        //{

        //    string compu = Environment.MachineName;
        //    string cpuInfo = string.Empty;
        //    string motherinfo = string.Empty;
        //    ManagementClass mc = new ManagementClass("Win32_Processor");
        //    ManagementObjectCollection moc = mc.GetInstances();

        //    foreach (ManagementObject mo in moc)
        //    {
        //        if (cpuInfo == string.Empty)
        //            cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
        //        else
        //            break;
        //    }
        //    mc = new ManagementClass("Win32_BaseBoard");
        //    moc = mc.GetInstances();
        //    foreach (ManagementObject mo in moc)
        //    {
        //        if (motherinfo == string.Empty)
        //        {
        //            motherinfo = mo.Properties["SerialNumber"].Value.ToString();
        //            try
        //            {
        //                motherinfo = motherinfo + mo.Properties["Product"].Value.ToString();
        //            }
        //            catch
        //            {
        //            }
        //        }
        //        else
        //            break;
        //    }
        //    string finalreg = "";

        //    finalreg = "MHL" + cpuInfo.ToUpper() + motherinfo.ToUpper() + "T8KL";

        //    //try
        //    //{
        //    //    if (ComputerValidTableAdapter.Exist3(compu, finalreg))
        //    //    {
        //    //    }
        //    //    else if (ComputerValidTableAdapter.Exist(compu))
        //    //        ComputerValidTableAdapter.Updatee(finalreg, UserNameLogin, compu);
        //    //    else if (ComputerValidTableAdapter.Exist2(finalreg))
        //    //        ComputerValidTableAdapter.Updatee2(compu, UserNameLogin, finalreg);
        //    //    else
        //    //        ComputerValidTableAdapter.InsertNew(compu, finalreg, UserNameLogin);
        //    //}
        //    //catch
        //    //{
        //    //}
        //    return false;
        //}

        //private CodeProject.Chidi.Cryptography.SymCryptography code = new CodeProject.Chidi.Cryptography.SymCryptography();
        //    private bool CreateFile()
        //    {

        //        string asd = "";
        //        displayStringMiladi = DateTime.Now.ToString(displayformat, DTFormat2);
        //        try
        //        {
        //            asd = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\gh2\kh4").GetValue("ed").ToString();
        //        }
        //        catch
        //        {
        //            try
        //            {
        //                Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\gh2\kh4").SetValue("ed", code.Encrypt(displayStringMiladi.ToUpper()));
        //            }
        //            catch
        //            {
        //            }
        //        }

        //        try
        //        {
        //            asd = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\gh2\kh4").GetValue("sd").ToString();
        //        }
        //        catch
        //        {
        //            try
        //            {
        //                Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\gh2\kh4").SetValue("sd", code.Encrypt(displayStringMiladi.ToUpper()));
        //            }
        //            catch
        //            {
        //            }
        //        }

        //        try
        //        {
        //            asd = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\gh2\kh4").GetValue("dif").ToString();
        //        }
        //        catch
        //        {
        //            try
        //            {
        //                Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\gh2\kh4").SetValue("dif", code.Encrypt("0"));
        //            }
        //            catch
        //            {
        //            }
        //        }

        //        try
        //        {
        //            asd = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\gh2\kh4").GetValue("extent").ToString();
        //        }
        //        catch
        //        {
        //            try
        //            {
        //                Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\gh2\kh4").SetValue("extent", code.Encrypt("0"));
        //            }
        //            catch
        //            {
        //            }
        //        }

        //        try
        //        {
        //            asd = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\gh2\kh4").GetValue("si").ToString();
        //        }
        //        catch
        //        {
        //            try
        //            {
        //                Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\gh2\kh4").SetValue("si", code.Encrypt("a2015"));
        //            }
        //            catch
        //            {
        //            }
        //        }


        //        return true;
        //    }


        //    private bool ChechVaild()
        //    {

        //        regvalue2 = "No";
        //        try
        //        {
        //            regvalue2 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\gh2\kh4").GetValue("si").ToString();
        //        }
        //        catch
        //        {
        //        }

        //        if (regvalue2 != "No")
        //            reg2 = true;
        //        else
        //            reg2 = false;

        //        regvalue3 = "No";
        //        try
        //        {
        //            regvalue3 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\gh2\kh4").GetValue("ed").ToString();
        //        }
        //        catch
        //        {
        //        }

        //        if (regvalue3 != "No")
        //            reg3 = true;
        //        else
        //            reg3 = false;

        //        regvalue4 = "No";
        //        try
        //        {
        //            regvalue4 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\gh2\kh4").GetValue("sd").ToString();
        //        }
        //        catch
        //        {
        //        }

        //        if (regvalue4 != "No")
        //            reg4 = true;
        //        else
        //            reg4 = false;

        //        regvalue5 = "No";
        //        try
        //        {
        //            regvalue5 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\gh2\kh4").GetValue("dif").ToString();
        //        }
        //        catch
        //        {
        //        }

        //        if (regvalue5 != "No")
        //            reg5 = true;
        //        else
        //            reg5 = false;

        //        regvalue6 = "No";
        //        try
        //        {
        //            regvalue6 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\gh2\kh4").GetValue("extent").ToString();
        //        }
        //        catch
        //        {
        //        }

        //        if (regvalue6 != "No")
        //            reg6 = true;
        //        else
        //            reg6 = false;



        //        if ((reg2 | reg3 | reg4 | reg5 | reg6) == false)
        //            return false;
        //        else
        //            return true;

        //        return true;
        //    }

        //private bool VaildationDemo()
        //    {
        //        DTFormat2 = new System.Globalization.CultureInfo("ar-sa", false).DateTimeFormat;
        //        DTFormat2.Calendar = new System.Globalization.GregorianCalendar();
        //        DTFormat2.ShortDatePattern = Calnderformat;
        //        if ((reg2 & reg3 & reg4 & reg5 & reg6) == true)
        //        {
        //            CodeProject.Chidi.Cryptography.SymCryptography code = new CodeProject.Chidi.Cryptography.SymCryptography();
        //            code.Key = "*128*";

        //            string serial = "";
        //            try
        //            {
        //                serial = code.Decrypt(regvalue2);
        //            }
        //            catch
        //            {
        //                //MessageBox.Show("سيتم اغلاق النظام قم بالاتصال بالشركة للتأكد من عدم التلاعب بالبرنامج", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                return false;
        //            }
        //            cpuInfo = "";
        //            string motherinfo = "";
        //            if (serial == "a2015")
        //            {
        //                programdemo = true;
        //                programfulltime = false;
        //            }
        //            else
        //            {
        //                programdemo = false;
        //                ManagementClass mc = new ManagementClass("Win32_Processor");
        //                ManagementObjectCollection moc = mc.GetInstances();
        //                foreach (ManagementObject mo in moc)
        //                {
        //                    if (cpuInfo == string.Empty)
        //                        cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
        //                    else
        //                        break;
        //                }

        //                mc = new ManagementClass("Win32_BaseBoard");
        //                moc = mc.GetInstances();
        //                foreach (ManagementObject mo in moc)
        //                {
        //                    if (motherinfo == string.Empty)
        //                    {
        //                        motherinfo = mo.Properties["SerialNumber"].Value.ToString();
        //                        try
        //                        {
        //                            motherinfo = motherinfo + mo.Properties["Product"].Value.ToString();
        //                        }
        //                        catch
        //                        {
        //                        }
        //                    }
        //                    else
        //                        break;
        //                }
        //                cpuInfo = "MHL" + cpuInfo.ToUpper() + motherinfo.ToUpper() + "T8KL";

        //                if (cpuInfo != serial)
        //                {
        //                    // MessageBox.Show("سيتم اغلاق النظام قم بالاتصال بالشركة للتأكد من عدم التلاعب بالبرنامج", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    programdemo = true;
        //                    programfulltime = true;
        //                    return false;
        //                }
        //                else
        //                {
        //                    programfulltime = false;

        //                    return true;
        //                }
        //            }

        //            DateTime ed = DateTime.Now;
        //            try
        //            {
        //                ed = DateTime.Parse(code.Decrypt(regvalue3), DTFormat2);
        //            }
        //            catch
        //            {
        //                // MessageBox.Show("سيتم اغلاق النظام قم بالاتصال بالشركة للتأكد من عدم التلاعب بالبرنامج", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                return false;
        //            }
        //            DateTime sd = DateTime.Now;
        //            try
        //            {
        //                sd = DateTime.Parse(code.Decrypt(regvalue4), DTFormat2);
        //            }
        //            catch
        //            {
        //                // MessageBox.Show("سيتم اغلاق النظام قم بالاتصال بالشركة للتأكد من عدم التلاعب بالبرنامج", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                return false;
        //            }
        //            int dif = 0;
        //            try
        //            {
        //                dif = int.Parse(code.Decrypt(regvalue5));
        //            }
        //            catch
        //            {
        //                // MessageBox.Show("سيتم اغلاق النظام قم بالاتصال بالشركة للتأكد من عدم التلاعب بالبرنامج", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                return false;
        //            }
        //            int exten = 0;
        //            try
        //            {
        //                exten = int.Parse(code.Decrypt(regvalue6));
        //            }
        //            catch
        //            {
        //                // MessageBox.Show("سيتم اغلاق النظام قم بالاتصال بالشركة للتأكد من عدم التلاعب بالبرنامج", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                return false;
        //            }

        //            if (programdemo)
        //            {
        //                if (sd > DateTime.Now)
        //                {
        //                    //  MessageBox.Show("سيتم اغلاق النظام قم بالاتصال بالشركة للتأكد من عدم التلاعب بالبرنامج", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    return false;
        //                }
        //                if (ed > DateTime.Now)
        //                {
        //                    // MessageBox.Show("سيتم اغلاق النظام قم بالاتصال بالشركة للتأكد من عدم التلاعب بالبرنامج", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    return false;
        //                }
        //                if (((sd - DateTime.Now).Days) > (((exten + 1) * 1) + dif)) //(DateDiff(DateInterval.Day, sd, DateTime.Now) > (((exten + 1) * 15) + dif))
        //                {
        //                    // MessageBox.Show("تم انتهاء المدة التجريبية", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    programfulltime = true;
        //                }
        //                //if (exten != 0)
        //                //    LicenceTool = "Demo Version - " + ((((exten + 1) * 15) + dif) - DateDiff(DateInterval.Day, sd, ed)).ToString() + " Days - Extend " + exten.ToString();
        //                //else
        //                //    LicenceTool = "Demo Version - " + ((((exten + 1) * 15) + dif) - DateDiff(DateInterval.Day, sd, ed)).ToString() + " Days";


        //                if (exten != 0)
        //                    UsersData.License = "Demo Version - " + ((((exten + 1) * 1) + dif) - ((sd - ed).Days)).ToString() + " Hours - Extend " + exten.ToString();
        //                else
        //                    UsersData.License = "Demo Version - " + ((((exten + 1) * 1) + dif) - ((sd - ed).Days)).ToString() + " Hours";


        //                //LicenceTool.BackColor = Color.Red;
        //                //LicenceTool.ForeColor = Color.White;
        //                string displayStringMiladi = DateTime.Now.ToString(displayformat, DTFormat2);
        //                try
        //                {
        //                    Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\gh2\kh4").SetValue("ed", code.Encrypt(displayStringMiladi.ToUpper()));
        //                }
        //                catch
        //                {
        //                }
        //            }
        //        }
        //        else
        //        {
        //            // MessageBox.Show("سيتم اغلاق النظام قم بالاتصال بالشركة للتأكد من عدم التلاعب بالبرنامج", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return false;
        //        }
        //        return true;
        //    }

        [HttpPost("Customeraccept")]
        public ActionResult Customeraccept(int OffersPricesId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _offersPricesService.Customeraccept(OffersPricesId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetOfferByCustomerData")]
        public ActionResult GetOfferByCustomerData(int offerid, string NationalId, int ActivationCode)
        {
            var offers = _offersPricesService.GetOfferByCustomerData(offerid, NationalId, ActivationCode);
            return Ok(offers);
        }

        [HttpGet("GetOrganizationDataLogin")]
        public IActionResult GetOrganizationDataLogin()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G));
        }

        [HttpGet("authorizeDropBox")]

        public HttpResponseMessage authorizeDropBox([FromQuery] string? code)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            FilesAuth FileAuth = new FilesAuth();
            var FAuth = _filesAuthService.GetFilesAuthByTypeId(1).Result;
            if (FAuth == null)
            {
                message.ReasonPhrase = "تأكد من حفظ اعدادات الدروب بوكس";
                message.StatusCode = HttpStatusCode.BadRequest;
                return message;
            }
            var RedirectUri = FAuth.RedirectUri;
            var AppKey = FAuth.AppKey;
            var AppSecret = FAuth.AppSecret;
            try
            {
                HttpClient httpClient = new HttpClient();
                Dictionary<string, string> dictionary = new Dictionary<string, string>
                {
                    { "code", code },
                    { "grant_type", "authorization_code" },
                    { "redirect_uri", RedirectUri },
                    { "client_id", AppKey },
                    { "client_secret", AppSecret },
                };
                FormUrlEncodedContent content = new FormUrlEncodedContent(dictionary);
                HttpResponseMessage responsee = httpClient.PostAsync("https://api.dropbox.com/oauth2/token", content).Result;
                JObject jObject = JObject.Parse(responsee.Content.ReadAsStringAsync().Result);
                if (responsee.StatusCode != HttpStatusCode.OK)
                {
                    throw new OAuth2Exception(jObject["error"].ToString(), jObject.Value<string>("error_description"));
                }

                string refreshToken = null;
                if (jObject.Value<string>("refresh_token") != null)
                {
                    FileAuth.RefreshToken = jObject["refresh_token"].ToString();
                }

                int num = -1;
                if (jObject.Value<string>("expires_in") != null)
                {
                    FileAuth.ExpiresIn = jObject["expires_in"].ToObject<int>();
                }

                string[] scopeList = null;
                if (jObject.Value<string>("scope") != null)
                {
                    scopeList = jObject["scope"].ToString().Split(new char[1] { ' ' });
                }
                FileAuth.AccessToken = jObject["access_token"].ToString();
                var result = _filesAuthService.UpdateTokenData(FileAuth, 1);
                message.ReasonPhrase = "تم الحفظ";
                message.StatusCode = HttpStatusCode.OK;
                return message;

            }
            catch (Exception e)
            {
                message.ReasonPhrase = "فشل في الحفظ";
                message.StatusCode = HttpStatusCode.BadRequest;
                return message;
            }

        }

        [HttpGet("authorizeDrive")]
        public HttpResponseMessage authorizeDrive([FromQuery] string? code, [FromQuery] string? scope)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            FilesAuth FileAuth = new FilesAuth();
            var FAuth = _filesAuthService.GetFilesAuthByTypeId(2).Result;
            if (FAuth == null)
            {
                message.ReasonPhrase = "تأكد من حفظ اعدادات الدروب بوكس";
                message.StatusCode = HttpStatusCode.BadRequest;
                return message;
            }
            var RedirectUri = FAuth.RedirectUri;
            var AppKey = FAuth.AppKey;
            var AppSecret = FAuth.AppSecret;
            try
            {
                HttpClient httpClient = new HttpClient();
                Dictionary<string, string> dictionary = new Dictionary<string, string>
                {
                    { "code", code },
                    { "grant_type", "authorization_code" },
                    { "redirect_uri", RedirectUri },
                    { "client_id", AppKey },
                    { "client_secret", AppSecret },
                };
                FormUrlEncodedContent content = new FormUrlEncodedContent(dictionary);
                HttpResponseMessage responsee = httpClient.PostAsync("https://oauth2.googleapis.com/token", content).Result;
                JObject jObject = JObject.Parse(responsee.Content.ReadAsStringAsync().Result);
                if (responsee.StatusCode != HttpStatusCode.OK)
                {
                    throw new OAuth2Exception(jObject["error"].ToString(), jObject.Value<string>("error_description"));
                }

                string refreshToken = null;
                if (jObject.Value<string>("refresh_token") != null)
                {
                    FileAuth.RefreshToken = jObject["refresh_token"].ToString();
                }

                int num = -1;
                if (jObject.Value<string>("expires_in") != null)
                {
                    FileAuth.ExpiresIn = jObject["expires_in"].ToObject<int>();
                }

                string[] scopeList = null;
                if (jObject.Value<string>("scope") != null)
                {
                    scopeList = jObject["scope"].ToString().Split(new char[1] { ' ' });
                }
                FileAuth.AccessToken = jObject["access_token"].ToString();
                var result = _filesAuthService.UpdateTokenData(FileAuth, 2);
                message.ReasonPhrase = "تم الحفظ";
                message.StatusCode = HttpStatusCode.OK;
                return message;

            }
            catch (Exception e)
            {
                message.ReasonPhrase = "فشل في الحفظ";
                message.StatusCode = HttpStatusCode.BadRequest;
                return message;
            }

        }
        [HttpGet("authorizeOneDrive2")]
        public HttpResponseMessage authorizeOneDrive2([FromQuery] string? code, [FromQuery] string? scope)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            FilesAuth FileAuth = new FilesAuth();
            var FAuth = _filesAuthService.GetFilesAuthByTypeId(3).Result;
            if (FAuth == null)
            {
                message.ReasonPhrase = "تأكد من حفظ اعدادات ون درايف";
                message.StatusCode = HttpStatusCode.BadRequest;
                return message;
            }
            var RedirectUri = FAuth.RedirectUri;
            var AppKey = FAuth.AppKey;
            var AppSecret = FAuth.AppSecret;

            try
            {

                string TOKEN_ENDPOINT_URL3 = "https://login.microsoftonline.com/{0}/oauth2/v2.0/token";
                string TOKEN_ENDPOINT_URL2 = "https://login.live.com/oauth20_token.srf";
                string TOKEN_ENDPOINT_URL = "https://login.microsoftonline.com/common/oauth2/v2.0/token";
                string ALL_SCOPE_AUTHORIZATIONS = "user.read calendars.readwrite calendars.readwrite.shared offline_access openid place.read.all";
                string tenantId = "bb64d0f4-3b70-401b-bc7f-394ad813a572";
                string url = string.Format(TOKEN_ENDPOINT_URL, tenantId);
                Dictionary<string, string> values = new Dictionary<string, string>
                {
                    { "client_id", AppKey },
                    //{ "scope", ALL_SCOPE_AUTHORIZATIONS },
                    { "client_secret", AppSecret },
                    { "grant_type", "authorization_code" },
                    { "code", code }
                };

                if (!string.IsNullOrEmpty(RedirectUri))
                {
                    values.Add("redirect_uri", RedirectUri);
                }
                FormUrlEncodedContent data = new FormUrlEncodedContent(values);

                HttpClient client = new HttpClient();

                HttpResponseMessage response = client.PostAsync(url, data).Result;

                if (response.IsSuccessStatusCode)
                {
                    string jsonToken = response.Content.ReadAsStringAsync().Result;
                    TokenModel tokenModel = ExtractToken(jsonToken).Result;
                    tokenModel.RedirectUri = RedirectUri;
                    FileAuth.AccessToken = tokenModel.AccessToken;
                    FileAuth.RefreshToken = tokenModel.RefreshToken;
                    FileAuth.ExpiresIn = tokenModel.ExpiresIn;
                    var result = _filesAuthService.UpdateTokenData(FileAuth, 3);
                    message.ReasonPhrase = "تم الحفظ";
                    message.StatusCode = HttpStatusCode.OK;
                    return message;

                    //return tokenModel;
                }
                else
                {
                    message.ReasonPhrase = "فشل في الحفظ";
                    message.StatusCode = HttpStatusCode.BadRequest;
                    return message;
                }

                //HttpClient httpClient = new HttpClient();
                //Dictionary<string, string> dictionary = new Dictionary<string, string>
                //{
                //    { "code", code },
                //    { "grant_type", "authorization_code" },
                //    { "redirect_uri", RedirectUri },
                //    { "client_id", AppKey },
                //    { "client_secret", AppSecret },
                //};
                //FormUrlEncodedContent content = new FormUrlEncodedContent(dictionary);
                //HttpResponseMessage responsee = httpClient.PostAsync("https://login.live.com/oauth20_token.srf", content).Result;
                //JObject jObject = JObject.Parse(responsee.Content.ReadAsStringAsync().Result);
                //if (responsee.StatusCode != HttpStatusCode.OK)
                //{
                //    throw new OAuth2Exception(jObject["error"].ToString(), jObject.Value<string>("error_description"));
                //}

                //string refreshToken = null;
                //if (jObject.Value<string>("refresh_token") != null)
                //{
                //    FileAuth.RefreshToken = jObject["refresh_token"].ToString();
                //}

                //int num = -1;
                //if (jObject.Value<string>("expires_in") != null)
                //{
                //    FileAuth.ExpiresIn = jObject["expires_in"].ToObject<int>();
                //}

                //string[] scopeList = null;
                //if (jObject.Value<string>("scope") != null)
                //{
                //    scopeList = jObject["scope"].ToString().Split(new char[1] { ' ' });
                //}
                //FileAuth.AccessToken = jObject["access_token"].ToString();
                //var result = _filesAuthService.UpdateTokenData(FileAuth, 3);
                //message.ReasonPhrase = "تم الحفظ";
                //message.StatusCode = HttpStatusCode.OK;
                //return message;

            }
            catch (Exception e)
            {
                message.ReasonPhrase = "فشل في الحفظ";
                message.StatusCode = HttpStatusCode.BadRequest;
                return message;
            }

        }
        [HttpGet("authorizeOneDrive")]
        public HttpResponseMessage authorizeOneDrive([FromQuery] string? code, [FromQuery] string? scope)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            FilesAuth FileAuth = new FilesAuth();
            var FAuth = _filesAuthService.GetFilesAuthByTypeId(3).Result;
            if (FAuth == null)
            {
                message.ReasonPhrase = "تأكد من حفظ اعدادات ون درايف";
                message.StatusCode = HttpStatusCode.BadRequest;
                return message;
            }
            var RedirectUri = FAuth.RedirectUri;
            var AppKey = FAuth.AppKey;
            var AppSecret = FAuth.AppSecret;

            try
            {
                string ALL_SCOPE_AUTHORIZATIONS = "user.read calendars.readwrite calendars.readwrite.shared offline_access openid place.read.all";
                HttpClient httpClient = new HttpClient();
                Dictionary<string, string> dictionary = new Dictionary<string, string>
                {
                    { "code", code },
                    { "grant_type", "authorization_code" },
                    //{ "scope", ALL_SCOPE_AUTHORIZATIONS },
                    { "redirect_uri", RedirectUri },
                    { "client_id", AppKey },
                    { "client_secret", AppSecret },
                };
                FormUrlEncodedContent content = new FormUrlEncodedContent(dictionary);
                //HttpResponseMessage responsee = httpClient.PostAsync("https://login.live.com/oauth20_token.srf", content).Result;
                HttpResponseMessage responsee = httpClient.PostAsync("https://login.microsoftonline.com/common/oauth2/v2.0/token", content).Result;

                JObject jObject = JObject.Parse(responsee.Content.ReadAsStringAsync().Result);
                if (responsee.StatusCode != HttpStatusCode.OK)
                {
                    throw new OAuth2Exception(jObject["error"].ToString(), jObject.Value<string>("error_description"));
                }

                string refreshToken = null;
                if (jObject.Value<string>("refresh_token") != null)
                {
                    FileAuth.RefreshToken = jObject["refresh_token"].ToString();
                }

                int num = -1;
                if (jObject.Value<string>("expires_in") != null)
                {
                    FileAuth.ExpiresIn = jObject["expires_in"].ToObject<int>();
                }

                string[] scopeList = null;
                if (jObject.Value<string>("scope") != null)
                {
                    scopeList = jObject["scope"].ToString().Split(new char[1] { ' ' });
                }
                FileAuth.AccessToken = jObject["access_token"].ToString();
                var result = _filesAuthService.UpdateTokenData(FileAuth, 3);
                message.ReasonPhrase = "تم الحفظ";
                message.StatusCode = HttpStatusCode.OK;
                return message;

            }
            catch (Exception e)
            {
                message.ReasonPhrase = "فشل في الحفظ";
                message.StatusCode = HttpStatusCode.BadRequest;
                return message;
            }

        }
        [HttpGet("ExtractToken")]

        private static async Task<TokenModel> ExtractToken(string jsonToken)
        {
            JObject jsonData = JObject.Parse(jsonToken);
            TokenModel tokenModel = new TokenModel();
            tokenModel.AccessToken = jsonData.SelectToken("access_token").ToString();
            tokenModel.RefreshToken = jsonData.SelectToken("refresh_token").ToString();
            tokenModel.Scope = jsonData.SelectToken("scope").ToString().Split(' ').ToList<string>();
            int duration = jsonData.SelectToken("expires_in").Value<int>();
            tokenModel.ExpiresIn = duration;
            if (duration > 0)
            {
                tokenModel.ExpirationDate = DateTime.Now.AddSeconds(duration);
            }
            tokenModel.TokenType = jsonData.SelectToken("token_type").ToString();
            return tokenModel;
        }


        [HttpPost("SendMail_SysContact")]
        public IActionResult SendMail_SysContact(int ContactId, string Name, string Textboby, string MobileNumber)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _Contact_Branches.SendMail_SysContact(ContactId, Name, Textboby, MobileNumber);
            if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
            {
                result.ReasonPhrase = "Saved Successfully";
            }
            else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
            {
                result.ReasonPhrase = "Saved Falied";
            }
            return Ok(new { result.StatusCode, result.ReasonPhrase });
        }


        [HttpGet("Token")]

        public string Token(int UserId, string Password, string UserName)
        {
            var apiResponse = "";

            try
            {

                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId",UserId.ToString()),
                        new Claim("Password", Password.ToString()),
                        new Claim("UserName", UserName.ToString())


                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(500),
                    signingCredentials: signIn);

                string _token = new JwtSecurityTokenHandler().WriteToken(token);

                apiResponse = _token.ToString();


            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                apiResponse = ex.Message;
            }
            catch (Exception ex)
            {
                apiResponse = ex.Message;

            }

            return apiResponse;
        }

        [HttpGet("ZatcaInvoiceIntegration")]
        public async Task<ActionResult<InvoiceReportingResponse>> ZatcaInvoiceIntegration()
        {

            UBLXML ubl = new UBLXML();
            Invoice inv = new Invoice();
            ZatcaIntegrationSDK.Result res = new ZatcaIntegrationSDK.Result();
            InvoiceReportingResponse invoicereportingmodel = new InvoiceReportingResponse();
            inv.ID = "1230"; // ãËÇá SME00010
            inv.UUID = Guid.NewGuid().ToString(); //"540b4b4c-5af5-4e07-8d3e-78170793c480"; // Guid.NewGuid().ToString();
            inv.IssueDate = "2023-05-14";
            inv.IssueTime = "11:25:55";

            inv.invoiceTypeCode.id = 388;

            inv.invoiceTypeCode.Name = "0200000";
            inv.DocumentCurrencyCode = "SAR";//ÇáÚãáÉ
            inv.TaxCurrencyCode = "SAR"; ////Ýì ÍÇáÉ ÇáÏæáÇÑ áÇÈÏ Çä Êßæä ÚãáÉ ÇáÖÑíÈÉ ÈÇáÑíÇá ÇáÓÚæÏì
                                         //inv.CurrencyRate = decimal.Parse("3.75"); // ÞíãÉ ÇáÏæáÇÑ ãÞÇÈá ÇáÑíÇá
                                         // Ýì ÍÇáÉ Çä ÇÔÚÇÑ ÏÇÆä Çæ ãÏíä ÝÞØ åÇäßÊÈ ÑÞã ÇáÝÇÊæÑÉ Çááì ÇÕÏÑäÇ ÇáÇÔÚÇÑ áíåÇ
            if (inv.invoiceTypeCode.id == 383 || inv.invoiceTypeCode.id == 381)
            {
                // فى حالة ان اشعار دائن او مدين فقط هانكتب رقم الفاتورة اللى اصدرنا الاشعار ليها
                InvoiceDocumentReference invoiceDocumentReference = new InvoiceDocumentReference();
                invoiceDocumentReference.ID = "Invoice Number: 354; Invoice Issue Date: 2021-02-10"; // اجبارى
                inv.billingReference.invoiceDocumentReferences.Add(invoiceDocumentReference);
            }
            // åäÇ ããßä ÇÖíÝ Çá pih ãä ÞÇÚÏÉ ÇáÈíÇäÇÊ  
            inv.AdditionalDocumentReferencePIH.EmbeddedDocumentBinaryObject = "NWZlY2ViNjZmZmM4NmYzOGQ5NTI3ODZjNmQ2OTZjNzljMmRiYzIzOWRkNGU5MWI0NjcyOWQ3M2EyN2ZiNTdlOQ==";
            // ÞíãÉ ÚÏÇÏ ÇáÝÇÊæÑÉ
            inv.AdditionalDocumentReferenceICV.UUID = 123456; // áÇÈÏ Çä íßæä ÇÑÞÇã ÝÞØ

            //Ýì ÍÇáÉ ÝÇÊæÑÉ ãÈÓØÉ æÝÇÊæÑÉ ãáÎÕÉ åÇäßÊÈ ÊÇÑíÎ ÇáÊÓáíã æÇÎÑ ÊÇÑíÎ ÇáÊÓáíã
            //
            //ÈíÇäÇÊ ÇáÏÝÚ 
            // ÇßæÇÏ ãÚíä
            // ÇÎÊíÇÑì ßæÏ ÇáÏÝÚ
            PaymentMeans paymentMeans = new PaymentMeans();
            paymentMeans.PaymentMeansCode = "10";//ÇÎÊíÇÑì
            //PaymentMeans paymentMeans1 = new PaymentMeans();
            //paymentMeans1.PaymentMeansCode = "42";//ÇÎÊíÇÑì
            //paymentMeans1.InstructionNote = "Payment Notes"; //ÇÌÈÇÑì Ýì ÇáÇÔÚÇÑÇÊ
            //inv.paymentmeans.payeefinancialaccount.ID = "";//ÇÎÊíÇÑì
            //inv.paymentmeans.payeefinancialaccount.paymentnote = "Payment by credit";//ÇÎÊíÇÑì
            inv.paymentmeans.Add(paymentMeans);
            //inv.paymentmeans.Add(paymentMeans1);
            //ÈíÇäÇÊ ÇáÈÇÆÚ 
            inv.SupplierParty.partyIdentification.ID = "2050012095"; // ÑÞã ÇáÓÌá ÇáÊÌÇÑì ÇáÎÇÖ ÈÇáÈÇÆÚ
            inv.SupplierParty.partyIdentification.schemeID = "CRN"; //ÑÞã ÇáÓÌá ÇáÊÌÇÑì
            inv.SupplierParty.postalAddress.StreetName = "streetnumber";// ÇÌÈÇÑì
            inv.SupplierParty.postalAddress.AdditionalStreetName = "ststtstst"; //ÇÎÊíÇÑì
            inv.SupplierParty.postalAddress.BuildingNumber = "3724"; // ÇÌÈÇÑì ÑÞã ÇáãÈäì
            inv.SupplierParty.postalAddress.PlotIdentification = "9833";//ÇÎÊíÇÑì ÑÞã ÇáÞØÚÉ
            inv.SupplierParty.postalAddress.CityName = "gaddah"; //ÇÓã ÇáãÏíäÉ
            inv.SupplierParty.postalAddress.PostalZone = "15385";//ÇáÑÞã ÇáÈÑíÏí
            inv.SupplierParty.postalAddress.CountrySubentity = "makka";//ÇÓã ÇáãÍÇÝÙÉ Çæ ÇáãÏíäÉ ãËÇá (ãßÉ) ÇÎÊíÇÑì
            inv.SupplierParty.postalAddress.CitySubdivisionName = "flassk";// ÇÓã ÇáãäØÞÉ Çæ ÇáÍì 
            inv.SupplierParty.postalAddress.country.IdentificationCode = "SA";
            inv.SupplierParty.partyLegalEntity.RegistrationName = "ãÄÓÓÉ ÎáíÌ ÌÇÒÇä ááÊÌÇÑÉ"; // ÇÓã ÇáÔÑßÉ ÇáãÓÌá Ýì ÇáåíÆÉ
            inv.SupplierParty.partyTaxScheme.CompanyID = "310901645200003";// ÑÞã ÇáÊÓÌíá ÇáÖÑíÈí

            // ÈíÇäÇÊ ÇáãÔÊÑì
            inv.CustomerParty.partyIdentification.ID = "123456";// ÑÞã ÇáÓÌá ÇáÊÌÇÑì ÇáÎÇÖ ÈÇáãÔÊÑì
            inv.CustomerParty.partyIdentification.schemeID = "NAT";//ÑÞã ÇáÓÌá ÇáÊÌÇÑì
            inv.CustomerParty.postalAddress.StreetName = "Kemarat Street,";// ÇÌÈÇÑì
            inv.CustomerParty.postalAddress.AdditionalStreetName = "";//ÇÎÊíÇÑì
            inv.CustomerParty.postalAddress.BuildingNumber = "3724";// ÇÌÈÇÑì ÑÞã ÇáãÈäì
            inv.CustomerParty.postalAddress.PlotIdentification = "9833";//ÇÎÊíÇÑì ÑÞã ÇáÞØÚÉ
            inv.CustomerParty.postalAddress.CityName = "Jeddah"; //ÇÓã ÇáãÏíäÉ
            inv.CustomerParty.postalAddress.PostalZone = "15385";//ÇáÑÞã ÇáÈÑíÏí
            inv.CustomerParty.postalAddress.CountrySubentity = "Makkah";//ÇÓã ÇáãÍÇÝÙÉ Çæ ÇáãÏíäÉ ãËÇá (ãßÉ) ÇÎÊíÇÑì
            inv.CustomerParty.postalAddress.CitySubdivisionName = "Alfalah";// ÇÓã ÇáãäØÞÉ Çæ ÇáÍì 
            inv.CustomerParty.postalAddress.country.IdentificationCode = "SA";
            inv.CustomerParty.partyLegalEntity.RegistrationName = "buyyername";// ÇÓã ÇáÔÑßÉ ÇáãÓÌá Ýì ÇáåíÆÉ
            inv.CustomerParty.partyTaxScheme.CompanyID = "301121971100003";// ÑÞã ÇáÊÓÌíá ÇáÖÑíÈí


            inv.legalMonetaryTotal.PayableAmount = 0;

            InvoiceLine invline = new InvoiceLine();
            //Product Quantity
            invline.InvoiceQuantity = 10;
            //Product Name
            invline.item.Name = "Item1";


            invline.item.classifiedTaxCategory.ID = "S"; // كود الضريبة
                                                         //item Tax code
            invline.taxTotal.TaxSubtotal.taxCategory.ID = "S"; // كود الضريبة

            //item Tax percentage
            invline.item.classifiedTaxCategory.Percent = 15; // نسبة الضريبة
            invline.taxTotal.TaxSubtotal.taxCategory.Percent = 15; // نسبة الضريبة

            invline.price.EncludingVat = false;
            //Product Price
            invline.price.PriceAmount = 10;

            // incase there is discount in invoice line level
            AllowanceCharge allowanceCharge = new AllowanceCharge();

            allowanceCharge.ChargeIndicator = false;

            allowanceCharge.AllowanceChargeReason = "discount"; // سبب الخصم على مستوى المنتج
                                                                // allowanceCharge.AllowanceChargeReasonCode = "90"; // سبب الخصم على مستوى المنتج
            allowanceCharge.Amount = 0; // قيم الخصم discount amount or charge amount

            allowanceCharge.MultiplierFactorNumeric = 0;
            allowanceCharge.BaseAmount = 0;
            invline.allowanceCharges.Add(allowanceCharge);

            inv.InvoiceLines.Add(invline);


            string publickey = @"MIICIzCCAcqgAwIBAgIGAZGe3NBoMAoGCCqGSM49BAMCMBUxEzARBgNVBAMMCmVJbnZvaWNpbmcwHhcNMjQwODI5MTU1OTEyWhcNMjkwODI4MjEwMDAwWjBcMQswCQYDVQQGEwJTQTEWMBQGA1UECwwNUml5YWRoIEJyYW5jaDEMMAoGA1UECgwDVFNUMScwJQYDVQQDDB5UU1QtMjA1MDAxMjA5NS0zMDA1ODkyODQ5MDAwMDMwVjAQBgcqhkjOPQIBBgUrgQQACgNCAASUSsHO+x6hNHMtO6eG3B6VUOd2jfPJ+2v5tKxiuzFcadVQ8f7X6O2Bll3DtC+EXmvGCSwKUawCH2DmSPx7MHa3o4HBMIG+MAwGA1UdEwEB/wQCMAAwga0GA1UdEQSBpTCBoqSBnzCBnDE7MDkGA1UEBAwyMS1UU1R8Mi1UU1R8My05OGI3ZjE2YS1hYmYwLTQ2Y2UtYWM0Yi01OTYyZGJiMWEyM2UxHzAdBgoJkiaJk/IsZAEBDA8zMTA5MDE2NDUyMDAwMDMxDTALBgNVBAwMBDExMDAxDjAMBgNVBBoMBU1ha2thMR0wGwYDVQQPDBRNZWRpY2FsIExhYm9yYXRvcmllczAKBggqhkjOPQQDAgNHADBEAiA+UuQ02k0tOFbVQIp0fzjYpOG1wb1TOzpHQlB0EGtK5AIgKrauZcHYNozfNSxGOChZxWOwgY5W8T7/Lhc0iOk0Fv8=";
            string privateKey = @"MHQCAQEEIJMOc02tEA+HncIbHPxKHxNVx6mMIZSIJjJcCAp6ZOGIoAcGBSuBBAAKoUQDQgAElErBzvseoTRzLTunhtwelVDndo3zyftr+bSsYrsxXGnVUPH+1+jtgZZdw7QvhF5rxgksClGsAh9g5kj8ezB2tw==";
            string secretkey = "xZLpAHj8bg6VY4brdKJC7eC/EoCVHCu3MISF/MW2gLU=";
            inv.cSIDInfo.CertPem = publickey;

            inv.cSIDInfo.PrivateKey = privateKey;

            InvoiceTotal CalculateInvoiceTotal = ubl.CalculateInvoiceTotal(inv.InvoiceLines, inv.allowanceCharges);
            res = ubl.GenerateInvoiceXML(inv, Directory.GetCurrentDirectory());
            if (res.IsValid)
            {

            }
            else
            {
                invoicereportingmodel.ErrorMessage = res.ErrorMessage;
                return BadRequest(invoicereportingmodel);

            }

            ZatcaIntegrationSDK.APIHelper.Mode mode = ZatcaIntegrationSDK.APIHelper.Mode.developer;


            ApiRequestLogic apireqlogic = new ApiRequestLogic(mode);
            InvoiceReportingRequest invrequestbody = new InvoiceReportingRequest();
            invrequestbody.invoice = res.EncodedInvoice;
            invrequestbody.invoiceHash = res.InvoiceHash;
            invrequestbody.uuid = res.UUID;
            if (mode == ZatcaIntegrationSDK.APIHelper.Mode.developer)
            {

                invoicereportingmodel = apireqlogic.CallComplianceInvoiceAPI(ZatcaIntegrationSDK.Utility.ToBase64Encode(publickey), secretkey, invrequestbody);

                if (invoicereportingmodel.IsSuccess)
                    return Ok(invoicereportingmodel);
                else
                    return BadRequest(invoicereportingmodel);

            }
            else
            {
                invoicereportingmodel = apireqlogic.CallReportingAPI(ZatcaIntegrationSDK.Utility.ToBase64Encode(publickey), secretkey, invrequestbody);
                if (invoicereportingmodel.IsSuccess)
                    return Ok(invoicereportingmodel);
                else
                    return BadRequest(invoicereportingmodel);
            }

        }

        [HttpGet("GenerateCompanyQR")]
        public ActionResult GenerateCompanyQR()
        {
            string fileName = "ComDomainLink.Jpeg";

            if (System.IO.File.Exists("Uploads/Organizations/DomainLink/" + fileName))
            {
                string fileLocation = Path.Combine("Uploads/Organizations/DomainLink/", fileName);


                System.IO.File.Delete(fileLocation);
                //return Ok(savedqrcode);
                //return Ok(new { HttpStatusCode.OK, result });
            }
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var org = _organizationsservice.GetComDomainLink_Org(orgId).Result.ApiBaseUri;
            var qrstring = org;


            try
            {
                // string fileLocation = Path.Combine("/Uploads/Organizations/DomainLink/") + ;
                string fileLocation = Path.Combine("Uploads/Organizations/DomainLink/", fileName);
                //string fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads/Organizations/DomainLink/");

                string ImgReturn = "";
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrstring, QRCodeGenerator.ECCLevel.Q, true);
                QRCode qrCode = new QRCode(qrCodeData);
                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] byteImage = ms.ToArray();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(ms);

                        ImgReturn = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }
                        img.Save(fileLocation, System.Drawing.Imaging.ImageFormat.Jpeg);

                    }

                }

                var result = "/Uploads/Organizations/DomainLink/" + fileName;
                //return Ok(savedqrcode);
                return Ok(new { HttpStatusCode.OK, result });

            }
            catch (Exception ex)
            {
                return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في الحفظ" });

            }




        }
        public class TokenModel
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
            public List<string> Scope { get; set; }
            public DateTime ExpirationDate { get; set; }
            public int ExpiresIn { get; set; }
            public string TokenType { get; set; }
            public string RedirectUri { get; set; }
        }
    }
}

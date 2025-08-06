﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
//using System.Web.Mvc;
using TaamerProject.Service.Interfaces;
using TaamerProject.Models;
using TaamerProject.API.Helper;
using TaamerProject.Models.Common;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using static TaamerProject.API.Controllers.UsersController;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors.Infrastructure;
using DocumentFormat.OpenXml.InkML;
using TaamerProject.Models.DBContext;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class UsersController : ControllerBase
    {
            private readonly IUsersService _usersservice;
            private readonly IOrganizationsService _organizationservice;
            private readonly INotificationService _notificationservice;
            private readonly IProjectPhasesTasksService _ProjectPhasesTasksService;
            private readonly IProjectWorkersService _projectWorkersservice;
            private readonly IFileService _fileservice;
            private readonly IUserMailsService _userMailsService;
            private readonly IPrivilegesService _privilegesService;
            private readonly IBranchesService _branchesService;
            private readonly ILicencesService _LicencesService;
            private IOrganizationsService _organizationsservice;

            private IConfiguration Configuration;
            public GlobalShared _globalshared;
            private readonly IWebHostEnvironment _hostingEnvironment;
            private string? Con;
        private object context;
        private readonly TaamerProjectContext _taamerProjectContext;
        public UsersController(IUsersService usersService, IOrganizationsService organizationsService, INotificationService notificationService, IProjectPhasesTasksService projectPhasesTasksService,
                IProjectWorkersService projectWorkers, IFileService fileService, IUserMailsService userMailsService, IPrivilegesService privilegesService, IBranchesService branchesService,
                ILicencesService licencesService, IOrganizationsService organizationsService1, IConfiguration _configuration, IWebHostEnvironment webHostEnvironment,
                TaamerProjectContext taamerProjectContext)
            {
                _usersservice = usersService;
                _organizationservice = organizationsService;
                _notificationservice = notificationService;
                _projectWorkersservice = projectWorkers;
                _ProjectPhasesTasksService = projectPhasesTasksService;
                _privilegesService = privilegesService;
                _fileservice = fileService;
                _userMailsService = userMailsService;
                _branchesService = branchesService;
                _LicencesService = licencesService;
                this._organizationsservice = organizationsService1;


            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");
            HttpContext httpContext = HttpContext;

            _globalshared = new GlobalShared(httpContext);
            _hostingEnvironment = webHostEnvironment;
            _taamerProjectContext = taamerProjectContext;
        }
        //public ActionResult Index()
        //{
        //    return View();
        //}
        //public ActionResult UsersN()
        //{
        //    var NoOfUsersUsed = _usersservice.GetAllUsersCount().Count();
        //    var TheUsers = Convert.ToInt32(_LicencesService.GetAllLicences("").FirstOrDefault().NoOfUsers);
        //    ViewBag.NoOfUsersUsed = NoOfUsersUsed;
        //    if ((TheUsers - NoOfUsersUsed) >= 0)
        //    {
        //        ViewBag.TheRestOfUsers = TheUsers - NoOfUsersUsed;
        //        ViewBag.NoOfUnauthorized = 0;
        //    }
        //    else
        //    {
        //        ViewBag.TheRestOfUsers = 0;
        //        ViewBag.NoOfUnauthorized = NoOfUsersUsed - TheUsers;
        //    }

        //    return View();
        //}
        //public ActionResult OnlineUsers()
        //{
        //    return View();
        //}
        //public ActionResult UserPrivReport()
        //{
        //    return View();
        //}
        //public ActionResult UserProfile()
        //{
        //    try
        //    {
        //        GenerateUserQR();
        //    }
        //    catch
        //    {

        //    }
        //    ViewBag.Users = _usersservice.GetAllUsers();
        //    int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G);

        //    var user = _usersservice.GetUserById(_globalshared.UserId_G, _globalshared.Lang_G);
        //    //Encrypt Stamp Image
        //    if (!string.IsNullOrEmpty(user.StampUrl))
        //    {
        //        string ImageIn = Server.MapPath("~" + user.StampUrl);
        //        string ImageOut = ImageIn.Replace("\\Encrypted", "");
        //        bool flag = RijndaelHelper.DecryptFile(ImageIn, ImageOut);
        //        if (flag)
        //        {
        //            //Not encrypted
        //            user.StampUrl = user.StampUrl.Replace("/Encrypted", "");
        //            Session["DecryptedStamp"] = ImageOut;
        //        }
        //    }
        //    ViewBag.User = user;
        //    ViewBag.PassWord = _usersservice.DecryptValue1(_usersservice.GetUserById(_globalshared.UserId_G, _globalshared.Lang_G).Password);
        //    ViewBag.ComDomainAddres = _organizationservice.GetComDomainLink_Org(orgId).ComDomainAddress;
        //    ViewBag.AllUserNotifications = _notificationservice.GetNotificationReceived(_globalshared.UserId_G);
        //    ViewBag.AllUserAlerts = _notificationservice.GetUserAlert(_globalshared.UserId_G);
        //    ViewBag.AllUserTasks = _ProjectPhasesTasksService.GetTasksByUserId(_globalshared.UserId_G, 0, _globalshared.BranchId_G);
        //    ViewBag.TasksCount = _ProjectPhasesTasksService.GetUserTaskCount(_globalshared.UserId_G, _globalshared.BranchId_G);
        //    ViewBag.ProjectWorkerCount = _projectWorkersservice.GetUserProjectWorkerCount(_globalshared.UserId_G, _globalshared.BranchId_G);
        //    ViewBag.FileUploadCount = _fileservice.GetUserFileUploadCount(_globalshared.UserId_G);
        //    ViewBag.NotificationsSent = _notificationservice.NotificationsSent(_globalshared.UserId_G);
        //    // return PartialView("~/Views/Menus/_Profile.cshtml");



        //    return View();
        //}
        [HttpGet("GenerateUserQR")]
        public ActionResult GenerateUserQR()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
                var org = _organizationservice.GetComDomainLink_Org(orgId).Result.ApiBaseUri; 
                var user = _usersservice.GetUserById(_globalshared.UserId_G, _globalshared.Lang_G).Result;
                var qrstring = org + "/" + user.Email + "/" + user.UserId;


                try
                {
                    string fileName = user.UserId + "EmpQrCodeImg.Jpeg";
                   // string fileLocation = Path.Combine("/Uploads/Organizations/DomainLink/") + ;
                  string fileLocation = Path.Combine("Uploads/Organizations/DomainLink/",fileName);
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

                    var savedqrcode = "/Uploads/Organizations/DomainLink/" + fileName;
                    _usersservice.UpdateQrCodeUser(user.UserId, savedqrcode);

                    return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم الحفظ" } );


                }
                catch (Exception ex)
                {
                    return Ok(new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في الحفظ" } );

                }




            }
        [HttpGet("UserProfile2")]
        public ActionResult UserProfile2()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var user = _usersservice.GetUserById(_globalshared.UserId_G, _globalshared.Lang_G).Result;
            //Encrypt Stamp Image
            string ImageIn = Path.Combine("~" + user.StampUrl);
            string ImageOut = ImageIn.Replace("\\Encrypted", "");
            bool flag = RijndaelHelper.DecryptFile(ImageIn, ImageOut);
            if (flag)
            {
                //Not encrypted
                user.StampUrl = ImageOut;
            }
            return Ok(new
            {
             Users = _usersservice.GetAllUsers(),

             User = user,
             AllUserNotifications = _notificationservice.GetNotificationReceived(_globalshared.UserId_G),
             AllUserAlerts = _notificationservice.GetUserAlert(_globalshared.UserId_G),
             AllUserTasks = _ProjectPhasesTasksService.GetTasksByUserId(_globalshared.UserId_G, 0, _globalshared.BranchId_G),
             TasksCount = _ProjectPhasesTasksService.GetUserTaskCount(_globalshared.UserId_G, _globalshared.BranchId_G),
             ProjectWorkerCount = _projectWorkersservice.GetUserProjectWorkerCount(_globalshared.UserId_G, _globalshared.BranchId_G),
             FileUploadCount = _fileservice.GetUserFileUploadCount(_globalshared.UserId_G),
             NotificationsSent = _notificationservice.NotificationsSent(_globalshared.UserId_G),
        });
            }
        [HttpGet("GetAllUsers")]
        public ActionResult GetAllUsers()
            {
                return Ok(_usersservice.GetAllUsers() );
            }
        [HttpGet("GetAllUserscount")]
        public ActionResult GetAllUserscount()
            {
                int count = _usersservice.GetAllUsers().Result.Count();
                return Ok(count );
            }
        [HttpGet("GetAllUsersNotOpenUser")]
        public ActionResult GetAllUsersNotOpenUser()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_usersservice.GetAllUsersNotOpenUser(_globalshared.UserId_G) );
            }
        [HttpGet("GetAllOtherUsers")]
        public ActionResult GetAllOtherUsers()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_usersservice.GetAllOtherUsers(_globalshared.UserId_G) );
            }
        //.oudedit
        [HttpGet("GetAllOnlineUsers")]
        public ActionResult GetAllOnlineUsers()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_usersservice.GetAllOnlineUsers(_globalshared.UserId_G) );
            }
        [HttpGet("CheckEmail")]
        public ActionResult CheckEmail(string Email)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int resultEmail = _usersservice.CheckEmail(_globalshared.UserId_G, Email);
                return Ok(resultEmail );
            }
        [HttpPost("SaveUsers")]
        public ActionResult SaveUsers([FromForm]Users users, [FromForm] string Link)
            {
            try
            {
                HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
                var barnchData = _branchesService.GetBranchById(users.BranchId ?? 0).Result;

                var CheckEmailOrganization = _organizationservice.CheckEmailOrganization(barnchData.OrganizationId);
                if (CheckEmailOrganization != null)
                {
                    string resetCode = Guid.NewGuid().ToString();
                    var verifyUrl = Link + "/" + resetCode;

                    //var verifyUrl = "Login/ResetPassword/" + resetCode;

                    //var UrlS = _hostingEnvironment.WebRootPath;

                    //StringBuilder sb = new StringBuilder(UrlS);

                    //// sb.ToString().Contains()
                    //sb.Replace("SaveUsers", "ResetPassword/");
                    //sb.Replace("Users", "login");

                    //var link = sb.ToString() + resetCode;// "http://144.91.68.47:8090/login/ResetPassword/" + resetCode;
                    //var link2 = "http://144.91.68.47:1010/login/ResetPassword/" + resetCode;

                    //  var file = Server.MapPath("~/dist/assets/images/logo.png");
                    var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
                    //var file = Path.Combine("~") + org.LogoUrl;
                    var file = "";
                    if (org.LogoUrl != null && org.LogoUrl != "")
                    {
                        string resulta = org.LogoUrl.Remove(0, 1);
                        file = Path.Combine(resulta);
                    }
                    //int UserSession = int.Parse(Request.
                    //var UserSession = Request["USession"];


                    if (users.Session == 1)
                        users.Session = 2;
                    var result = _usersservice.SaveUsers(users, _globalshared.UserId_G, verifyUrl, file, PopulateBody(1, verifyUrl, Path.GetFullPath("Email/ChangePassword.html"), _globalshared.UserId_G, resetCode),
                        resetCode, Con, _globalshared.BranchId_G);
                    return Ok(result);
                }
                else
                {
                    return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "يلزم وجود بريد إلكتروني للمؤسسة!" });
                }
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
            }
        [HttpPost("SaveUsersProfile")]
        public ActionResult SaveUsersProfile([FromForm] Users users, [FromForm] string Link)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var barnchData = _branchesService.GetBranchById(users.BranchId ?? 0).Result;

                var CheckEmailOrganization = _organizationservice.CheckEmailOrganization(barnchData.OrganizationId);
                if (CheckEmailOrganization != null)
                {
                    string resetCode = Guid.NewGuid().ToString();
                var verifyUrl = Link + "/" + resetCode;

                //var verifyUrl = "Login/ResetPassword/" + resetCode;

                //var UrlS = _hostingEnvironment.WebRootPath;

                //StringBuilder sb = new StringBuilder(UrlS);

                //// sb.ToString().Contains()
                //sb.Replace("SaveUsers", "ResetPassword/");
                //sb.Replace("Users", "login");

                //var link = sb.ToString() + resetCode;// "http://144.91.68.47:8090/login/ResetPassword/" + resetCode;
                //var link2 = "http://144.91.68.47:1010/login/ResetPassword/" + resetCode;

                var file = Path.Combine("distnew/images/logo.png");
                    //var file = "";
                    //if (org.LogoUrl != null && org.LogoUrl != "")
                    //{
                    //    string result = org.LogoUrl.Remove(0, 1);
                    //    file = Path.Combine(result);
                    //}
                //int UserSession = int.Parse(Request.
                //var UserSession = Request["USession"];

                if (users.Session == 1)
                        users.Session = 2;
                    var result = _usersservice.SaveUsersProfile(users, _globalshared.UserId_G, verifyUrl, file, PopulateBody(1, verifyUrl, Path.Combine("Email/ChangePassword.html"), _globalshared.UserId_G, resetCode),
                        resetCode, Con, _globalshared.BranchId_G);
                    return Ok(result );
                }
                else
                {
                    return Ok(new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "يلزم وجود بريد إلكتروني للمؤسسة!" } );
                }
            }

        //public ActionResult LogoutUser(int userId)
        //{
        //    GeneralMessage generalMessage;
        //    if (_globalshared.UserId_G != 1 && userId != _globalshared.UserId_G)
        //    {
        //        var result = _usersservice.LogoutUser(false, userId, userId, _globalshared.BranchId_G);
        //        //----------------
        //        System.Web.HttpContext.Current.Application.Lock();
        //        List<int> LogoutUsersArr = (List<int>)System.Web.HttpContext.Current.Application["ForcedLogoutUsers"];
        //        if (LogoutUsersArr == null)
        //        {
        //            LogoutUsersArr = new List<int>();
        //        }
        //        LogoutUsersArr.Add(_globalshared.UserId_G);
        //        System.Web.HttpContext.Current.Application["ForcedLogoutUsers"] = LogoutUsersArr;
        //        System.Web.HttpContext.Current.Application.UnLock();
        //        //-------------------
        //        if (result > 0)
        //            generalMessage = new GeneralMessage() {StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم استبعاد المستخدم بنجاح" };
        //        else
        //            generalMessage = new GeneralMessage() { Result = false, Message = "فشل في استبعاد المستخدم" };
        //        return Ok(generalMessage);
        //    }
        //    else
        //    {
        //        string msg = _globalshared.Lang_G == "rtl" ? "لا يمكن استبعاد الأدمن" : "Can't force Admin to logout";
        //        generalMessage = new GeneralMessage() { Result = false, Message = msg };
        //        return Ok(generalMessage);
        //    }
        //}

        //public string GetIp()
        //{
        //    string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //    if (string.IsNullOrEmpty(ip))
        //    {
        //        ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //    }
        //    return ip;
        //}

        [HttpPost("PopulateBody")]
        public string PopulateBody(int type, string EmailUrl, string url, int userid, string resetCode)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            string body = string.Empty;
                //var userdata = _usersservice.GetUserById(_globalshared.UserId_G, _globalshared.Lang_G);
                //var OrganizationData = _organizationservice.GetBranchOrganization((int)userdata._globalshared.BranchId_G);
             string path = System.IO.Path.Combine(url);
            using (StreamReader reader = new StreamReader(path))
                {
                    body = reader.ReadToEnd();
                }
                if (type == 1)
                {
                    body = body.Replace("{EmailUrl}", EmailUrl);

                }
                else if (type == 2) { }



                return body;
            }
        [HttpPost("ChangePassword")]
        public ActionResult ChangePassword(Users users)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _usersservice.ChangePassword(users, _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok( result );
            }

        [HttpPost("Disappearewelcomeuser")]
        public ActionResult Disappearewelcomeuser()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _usersservice.Disappearewelcomeuser(_globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(result );
            }
        [HttpPost("ChangeUserImage")]
        public ActionResult ChangeUserImage(IFormFile? UploadedFile,[FromForm] Users users)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            //HttpPostedFileBase file = Request.Files["UploadedFile"];
            //if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            //{
            //    if (Request.Files["UploadedFile"].ContentLength > 0)
            //    {
            //        string fileLocation = Server.MapPath("~/Uploads/Users/") + Request.Files["UploadedFile"].FileName;
            //        try
            //        {
            //            if (System.IO.File.Exists(fileLocation))
            //            {
            //                System.IO.File.Delete(fileLocation);
            //            }
            //            Request.Files["UploadedFile"].SaveAs(fileLocation);
            //            users.ImgUrl = "/Uploads/Users/" + Request.Files["UploadedFile"].FileName;
            //        }
            //        catch
            //        {
            //            return Ok(new GeneralMessage { Result = false, Message = "فشل في رفع الصوره" } );
            //        }
            //    }
            //}

            if (UploadedFile != null)
            {
                System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


                string path = System.IO.Path.Combine("Uploads/", "Users/");
                string pathW = System.IO.Path.Combine("/Uploads/", "Users/");

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                List<string> uploadedFiles = new List<string>();
                string pathes = "";
                //foreach (IFormFile postedFile in postedFiles)
                //{
                string fileName = System.IO.Path.GetFileName(Guid.NewGuid() + UploadedFile.FileName);
                //string fileName = System.IO.Path.GetFileName(postedFiles.FileName);

                var path2 = Path.Combine(path, fileName);
                if (System.IO.File.Exists(path2))
                {
                    System.IO.File.Delete(path2);
                }
                using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(path, fileName), System.IO.FileMode.Create))
                {


                    UploadedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    // string returnpath = host + path + fileName;
                    //pathes.Add(pathW + fileName);
                    pathes = pathW + fileName;
                }


                if (pathes != null)
                {
                    users.ImgUrl = pathes;
                }
            }


            var result = _usersservice.ChangeUserImage(users, _globalshared.UserId_G, _globalshared.BranchId_G);
                //if (result.Result && users.UserId == UserId)
                //{
                //    Request.Cookies["ImgUrl"].Value = users.ImgUrl;
                //}
                return Ok(result );
            }
        [HttpPost("ChangeStampImage")]
        public ActionResult ChangeStampImage(IFormFile? UploadedFile, [FromForm] Users users)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            //HttpPostedFileBase file = Request.Files["UploadedFile2"];
            //if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            //{
            //    if (Request.Files["UploadedFile2"].ContentLength > 0)
            //    {
            //        string fileLocation = Server.MapPath("~/Uploads/Users/") + Request.Files["UploadedFile2"].FileName;
            //        string fileLocationOut = Server.MapPath("~/Uploads/Users/Encrypted/") + Request.Files["UploadedFile2"].FileName;
            //        try
            //        {
            //            if (System.IO.File.Exists(fileLocation))
            //            {
            //                System.IO.File.Delete(fileLocation);
            //            }
            //            Request.Files["UploadedFile2"].SaveAs(fileLocation);
            //            bool flag = RijndaelHelper.EncryptFile(fileLocation, fileLocationOut);

            //            users.StampUrl = "/Uploads/Users/Encrypted/" + Request.Files["UploadedFile2"].FileName;
            //            if (System.IO.File.Exists(fileLocation))
            //            {
            //                GC.Collect();
            //                GC.WaitForPendingFinalizers();

            //                System.IO.File.Delete(fileLocation);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            return Ok(new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في رفع الصوره" } );
            //        }
            //    }
            //}
            if (UploadedFile != null)
            {
                System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


                string path = System.IO.Path.Combine("Uploads/", "Users/");
                string pathW = System.IO.Path.Combine("/Uploads/", "Users/");
                string pathencrypt = System.IO.Path.Combine("/Uploads/", "Users/Encrypted/");
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                List<string> uploadedFiles = new List<string>();
                string pathes = "";
                //foreach (IFormFile postedFile in postedFiles)
                //{
                string fileName = System.IO.Path.GetFileName(Guid.NewGuid() + UploadedFile.FileName);
                string fileLocationOut = Path.Combine(pathencrypt, fileName);
                //string fileName = System.IO.Path.GetFileName(postedFiles.FileName);

                var path2 = Path.Combine(path, fileName);
                if (System.IO.File.Exists(path2))
                {
                    System.IO.File.Delete(path2);
                }
                using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(path, fileName), System.IO.FileMode.Create))
                {


                    UploadedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    // string returnpath = host + path + fileName;
                    //pathes.Add(pathW + fileName);
                    pathes = pathW + fileName;
                }
                if (pathes != null)
                {
                    users.StampUrl = "/Uploads/Users/Encrypted/" + fileName;
                }
                bool flag = RijndaelHelper.EncryptFile(path2, fileLocationOut);
                if (System.IO.File.Exists(path2))
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    System.IO.File.Delete(path2);
                }
               
            }

            var result = _usersservice.ChangeUserStamp(users, _globalshared.UserId_G, _globalshared.BranchId_G);
                //if (result.Result && users.UserId == UserId)
                //{
                //    Request.Cookies["ImgUrl"].Value = users.ImgUrl;
                //}
                return Ok(result );
            }
        [HttpPost("DeleteUsers")]
        public ActionResult DeleteUsers(int userId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _usersservice.DeleteUsers2(userId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveUserPriv")]
        public ActionResult SaveUserPriv(PrivList privList)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _usersservice.SaveUserPrivilegesUsers(privList.UserId??0, privList.PrivIds, _globalshared.UserId_G, _globalshared.BranchId_G, Con);
                return Ok(result);
            }
        [HttpPost("SaveGroupUsersPriv")]
        public ActionResult SaveGroupUsersPriv(PrivList privList)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _usersservice.SaveGroupPrivilegesUsers(privList.GroupId??0, privList.PrivIds, _globalshared.UserId_G, _globalshared.BranchId_G,Con);
                return Ok(result);
            }
        [HttpGet("GetPrivilegesIdsByUserId")]
        public ActionResult GetPrivilegesIdsByUserId(int UserId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_usersservice.GetPrivilegesIdsByUserId(UserId) );
            }
        [HttpGet("GetBranchesByUserId")]
        public ActionResult GetBranchesByUserId(int UserId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_usersservice.GetBranchesByUserId(UserId) );
            }
        [HttpGet("GetUserById")]
        public ActionResult GetUserById(int UserId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_usersservice.GetUserById(UserId, _globalshared.Lang_G) );
            }
        [HttpGet("GetCurrentUserById")]
        public ActionResult GetCurrentUserById()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_usersservice.GetUserById(_globalshared.UserId_G, _globalshared.Lang_G) );
            }
        [HttpGet("GetUserById2")]
        public ActionResult GetUserById2(int UserId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_usersservice.GetUserById2(UserId, _globalshared.Lang_G) );
            }
        [HttpGet("GetUserCounts")]
        public ActionResult GetUserCounts(int UserId)
        {
        HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        return Ok(new
            {
                TasksCount = _ProjectPhasesTasksService.GetUserTaskCount(UserId, _globalshared.BranchId_G),
                ProjectWorkerCount = _projectWorkersservice.GetUserProjectWorkerCount(UserId, _globalshared.BranchId_G),
                FileUploadCount = _fileservice.GetUserFileUploadCount(UserId),
                MessageCount = _userMailsService.GetAllUserMails(UserId, _globalshared.BranchId_G).Result.Count(),
            } );
        }

         [HttpGet("FillUsersSelect")]
        public ActionResult FillUsersSelect(bool param = false)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_usersservice.GetAllUsers().Result.Select(s => new
                {
                    Id = s.UserId,
                    Name = s.FullName,
                    s.BranchId,
                    s.UserId
                }).Where(s => param ? true : s.UserId != _globalshared.UserId_G && !param) );

            }



        [HttpGet("FillAllUsersTodropdown")]
        public ActionResult FillAllUsersTodropdown(bool param = false)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_usersservice.GetAllUsers().Result.Select(s => new
                {
                    Id = s.UserId,
                    Name = s.FullName,
                    s.BranchId,
                    s.UserId
                }).Where(s => param ? true : s.UserId != 1 && !param) );

            }
        [HttpGet("FillSomeUsers")]
        public ActionResult FillSomeUsers()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_usersservice.GetSomeusers().Select(s => new
                {
                    Id = s.UserId,
                    Name = s.FullName,
                    s.UserId
                }).Where(s => s.UserId != _globalshared.UserId_G) );



            }


        [HttpGet("GetMaxOrderNumber")]
        public ActionResult GetMaxOrderNumber()
            {
                return Ok(_usersservice.GetMaxOrderNumber() );
            }

        //.
        [HttpGet("GetCurrentUser")]
        public ActionResult GetCurrentUser()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_usersservice.GetUserById(_globalshared.UserId_G, _globalshared.Lang_G) );
        }
        [HttpGet("GetPrivUserReport")]
        public ActionResult GetPrivUserReport(int UserId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var priviledgeIds = _usersservice.GetPrivilegesIdsByUserId(_globalshared.UserId_G);
                var priviledges = Privileges.PrivilegesList.Where(t => t.ParentId != null).Select(s => new
                {
                    s.Id,
                    PriviLedgeName = s.Description,
                    Status = priviledgeIds.Contains(s.Id),
                    Insert = priviledgeIds.Contains(s.Id),
                    Update = priviledgeIds.Contains(s.Id),
                    Delete = priviledgeIds.Contains(s.Id),
                }).OrderBy(s => s.Id);
                return Ok(priviledges );
            }
        [HttpGet("FillPriv")]
        public ActionResult FillPriv()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var priviledgeIds = _usersservice.GetPrivilegesIdsByUserId(_globalshared.UserId_G);
                var priviledges = Privileges.PrivilegesList.Where(t => t.ParentId != null).Select(s => new
                {
                    Id = s.Id,
                    Name = s.Description,
                }).OrderBy(s => s.Id);
                return Ok(priviledges );
            }





        [HttpPost("CloseUser")]
        public ActionResult CloseUser(Users users)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _usersservice.CloseUser(users, _globalshared.UserId_G);
                return Ok(result );
            }
        [HttpPost("SaveLicence")]
        public ActionResult SaveLicence(Licences Licence)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _LicencesService.SaveLicence(Licence, _globalshared.UserId_G, _globalshared.BranchId_G);
                if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
                {
                    result.ReasonPhrase = "Saved Successfully";
                }
                else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
                {
                    result.ReasonPhrase = "Saved Falied";
                }
                return Ok(result );
            }


        [HttpPost("UpdateLicenceLabaik")]
        public ActionResult UpdateLicenceLabaik([FromForm]Licences Licence)
        {
            var result = _LicencesService.UpdateLicenceLabaik(Licence);
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

        [HttpPost("DeleteDeviceId")]
        public ActionResult DeleteDeviceId(int user)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _usersservice.DeleteDeviceId(user, _globalshared.UserId_G, _globalshared.BranchId_G);
            if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
            {
                result.ReasonPhrase = "Saved Successfully";
            }
            else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
            {
                result.ReasonPhrase = "Saved Falied";
            }
            return Ok(result );
            }
        [HttpPost("SaveLicenceAlerts")]
        public ActionResult SaveLicenceAlerts(Licences Licence)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _LicencesService.SaveLicenceAlerts(Licence, _globalshared.UserId_G, _globalshared.BranchId_G);
            if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
            {
                result.ReasonPhrase = "Saved Successfully";
            }
            else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
            {
                result.ReasonPhrase = "Saved Falied";
            }
            return Ok(result );
            }
        [HttpPost("SaveLicenceAlertsBtn")]
        public ActionResult SaveLicenceAlertsBtn(Licences Licence)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _LicencesService.SaveLicenceAlertsBtn(Licence, _globalshared.UserId_G, _globalshared.BranchId_G);
            if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
            {
                result.ReasonPhrase = "Saved Successfully";
            }
            else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
            {
                result.ReasonPhrase = "Saved Falied";
            }
            return Ok(result );
            }
        [HttpGet("GetAllLicences")]
        public ActionResult GetAllLicences(string? SearchText)
            {
                return Ok(_LicencesService.GetAllLicences(SearchText??"") );
            }
        [HttpGet("SetNoOfUsers_Rest")]
        public ActionResult SetNoOfUsers_Rest()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            SetNoOfUsers_Rest_Class Obj = new SetNoOfUsers_Rest_Class();
                var NoOfUsersUsed = _usersservice.GetAllUsersCount().Count();
                var TheUsers = Convert.ToInt32(_LicencesService.GetAllLicences("").Result.FirstOrDefault().NoOfUsers);
                Obj.NoOfUsersUsed = NoOfUsersUsed;
                if ((TheUsers - NoOfUsersUsed) >= 0)
                {
                    Obj.TheRestOfUsers = TheUsers - NoOfUsersUsed;
                    Obj.NoOfUnauthorized = 0;
                }
                else
                {
                    Obj.TheRestOfUsers = 0;
                    Obj.NoOfUnauthorized = NoOfUsersUsed - TheUsers;
                }
                return Ok(Obj );

            }

        

        [HttpPost("UpdateAppActiveStatus")]
        public ActionResult UpdateAppActiveStatus(bool IsActive, int user)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _usersservice.UpdateAppActiveStatus(IsActive, user, _globalshared.UserId_G, _globalshared.BranchId_G);;
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

        [HttpPost("SaveLabaikLicences")]
        public async Task<IActionResult> SaveLabaikLicences(Licences licences)
        {
            try
            {
                try
                {
                    var licence = _LicencesService.CheckLicenceG_UID(licences);
                    var db = _taamerProjectContext.GetDatabaseName();
                    if (licence.StatusCode == HttpStatusCode.OK)
                    {
                        var uri = "https://api2.tameercloud.com/"; //"https://localhost:44334/";// "http://164.68.110.173:8080/";
                        //var uri = "https://localhost:44334/";// "http://164.68.110.173:8080/";
                        if (uri != null && uri != "")
                        {
                            //Generate Token
                            var token = getapitoken(uri);

                            var org = _organizationservice.GetBranchOrganization();

                            var formData = new MultipartFormDataContent();
                            
                            // Add the fields to the formdata
                            formData.Add(new StringContent((licences.LicenceId).ToString()), "LicenceId");
                            formData.Add(new StringContent(licence.ReasonPhrase ?? ""), "G_UID");
                            formData.Add(new StringContent(licences.LicenceContractNo ?? ""), "LicenceContractNo");
                            formData.Add(new StringContent(licences.NoOfUsers ?? ""), "NoOfUsers");
                            formData.Add(new StringContent(licences.Mobile ?? ""), "Mobile");
                            formData.Add(new StringContent(org.Result.NameAr ?? ""), "CustomerName");
                            formData.Add(new StringContent(licences.Hosting_Expiry_Date ?? ""), "Hosting_Expiry_Date");
                            formData.Add(new StringContent(licences.Support_Expiry_Date ?? ""), "Support_Expiry_Date");
                            formData.Add(new StringContent((licences.Type).ToString() ?? ""), "Type");
                            formData.Add(new StringContent(org.Result.ComDomainLink ?? ""), "Domain");
                            formData.Add(new StringContent(licences.Email3 ?? ""), "ComputerName");
                            formData.Add(new StringContent(licences.Email3 ?? ""), "Email3");
                            formData.Add(new StringContent((org.Result.TameerAPIURL ?? "").ToString()), "CustomerULR");
                            formData.Add(new StringContent(licences.Support_Start_Date ?? ""), "Support_Start_Date");
                            formData.Add(new StringContent(org.Result.ComDomainAddress ?? ""), "IPAdress");
                            formData.Add(new StringContent(licences.Subscrip_Domain.ToString().ToLower()), "Subscrip_Domain");
                            formData.Add(new StringContent(licences.Subscrip_Hosting.ToString().ToLower()), "Subscrip_Hosting");
                            formData.Add(new StringContent(db ?? ""), "DBName");
                            formData.Add(new StringContent(licences.Email ?? ""), "Email");
                            formData.Add(new StringContent(licences.Email2 ?? ""), "Email2");
                            formData.Add(new StringContent(licences.Mobile2 ?? ""), "Mobile2");
                            formData.Add(new StringContent(licences.TotalCost ?? ""), "TotalOprationalCost");
                            formData.Add(new StringContent(licences.Tax ?? ""), "Tax");
                            formData.Add(new StringContent(licences.Cost ?? ""), "Cost");
                            formData.Add(new StringContent(licences.ServerStorage ?? ""), "ServerStorage");






                            using (var client = new HttpClient())
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
                                    return Ok();
                                }
                                else
                                {   
                                    return BadRequest();
                                    

                                }

                            }

                        }
                    }
                    return BadRequest();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        //[HttpPost("SaveSupportDate")]
        //public async Task<IActionResult> SaveSupportDate([FromForm]Licences licences)
        //{
        //    try
        //    {
        //        try
        //        {
        //            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

        //            var licence = _LicencesService.UpdatSupportDate(licences, _globalshared.UserId_G, _globalshared.BranchId_G);
        //            if (licence.StatusCode == HttpStatusCode.OK)
        //            {
        //                //var uri = "https://api2.tameercloud.com/"; //"https://localhost:44334/";// "http://164.68.110.173:8080/";
        //                var uri = "https://localhost:44334/";// "http://164.68.110.173:8080/";
        //                if (uri != null && uri != "")
        //                {
        //                    //Generate Token
        //                    var token = getapitoken(uri);

        //                    var org = _organizationservice.GetBranchOrganization();

        //                    var formData = new MultipartFormDataContent();

        //                    // Add the fields to the formdata
        //                    formData.Add(new StringContent((licences.LicenceId).ToString()), "LicenceId");
        //                    formData.Add(new StringContent(licence.ReasonPhrase ?? ""), "G_UID");
        //                    formData.Add(new StringContent(licences.LicenceContractNo ?? ""), "LicenceContractNo");
        //                    formData.Add(new StringContent(licences.NoOfUsers ?? ""), "NoOfUsers");
        //                    formData.Add(new StringContent(licences.Mobile ?? ""), "Mobile");
        //                    formData.Add(new StringContent(org.Result.NameAr ?? ""), "CustomerName");
        //                    formData.Add(new StringContent(licences.Hosting_Expiry_Date ?? ""), "Hosting_Expiry_Date");
        //                    formData.Add(new StringContent(licences.Support_Expiry_Date ?? ""), "Support_Expiry_Date");
        //                    formData.Add(new StringContent((licences.Type).ToString() ?? ""), "Type");
        //                    formData.Add(new StringContent(org.Result.ComDomainAddress ?? ""), "Domain");
        //                    formData.Add(new StringContent(licences.Email3 ?? ""), "ComputerName");
        //                    formData.Add(new StringContent((org.Result.TameerAPIURL ?? "").ToString()), "CustomerULR");
        //                    formData.Add(new StringContent(licences.Support_Start_Date ?? ""), "Support_Start_Date");




        //                    using (var client = new HttpClient())
        //                    {
        //                        //Base API URI
        //                        client.BaseAddress = new Uri(uri);
        //                        //JWT TOKEN
        //                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //                        client.DefaultRequestHeaders
        //                        .Accept
        //                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //                        //HTTP POST API
        //                        //var responseTask = client.PostAsync("api/ServiceRequest/SaveServiceRequest", null);

        //                        var res = await client.PostAsync("api/Licences/SaveLicence", formData);
        //                        res.EnsureSuccessStatusCode();  // This will throw an exception for non-success status codes

        //                        string responseBody = await res.Content.ReadAsStringAsync();
        //                        dynamic data = JsonConvert.DeserializeObject(responseBody);

        //                        // Access the 'token' property

        //                        //res.Wait();
        //                        Console.WriteLine(data);
        //                        if (data != null && data.statusCode == HttpStatusCode.OK)
        //                        {
        //                            return Ok();
        //                        }
        //                        else
        //                        {
        //                            return BadRequest();


        //                        }

        //                    }

        //                }
        //            }
        //            return BadRequest();
        //        }
        //        catch (Exception ex)
        //        {
        //            return BadRequest(ex.Message);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}

        [HttpPost("getapitoken")]
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



    }
    public class SetNoOfUsers_Rest_Class
    {
        public int NoOfUsersUsed { set; get; }
        public int TheRestOfUsers { set; get; }
        public int NoOfUnauthorized { set; get; }
    }
}

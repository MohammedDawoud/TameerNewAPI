using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Require2FA")]
    
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationservice;
        private readonly IFileService _fileService;
        private readonly IProjectService _projectService;
        private string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        public NotificationController(IConfiguration _configuration, INotificationService notificationservice,
            IFileService fileService, IProjectService projectService
            )
        {
            _notificationservice = notificationservice;
            _fileService = fileService;
            _projectService = projectService;
            Configuration = _configuration;
            Con = this.Configuration.GetConnectionString("DBConnection");
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }


        [HttpGet("GetAllNotifications")]
        public IActionResult GetAllNotifications(int ProjectId)
        {

            var someNoti = _notificationservice.GetAllNotifications(ProjectId);
            return Ok(someNoti);
        }
        [HttpGet("GetAllNotificationsBackup")]
        public IActionResult GetAllNotificationsBackup()
        {
            var someNoti = _notificationservice.GetAllNotificationsBackup();
            return Ok(someNoti);
        }
        [HttpGet("GetAllAlerts")]
        public IActionResult GetAllAlerts()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someNoti = _notificationservice.GetAllAlerts(_globalshared.BranchId_G);
            return Ok(someNoti);
        }
        [HttpPost("SaveNotification")]
        public IActionResult SaveNotification(Notification notification)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _notificationservice.SaveNotification(notification, _globalshared.UserId_G, _globalshared.BranchId_G);
         
            return Ok(result);
        }


        private readonly Random _random = new Random();
        [HttpPost("RandomNumber")]
        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
        [HttpPost("SaveAlert")]
        public IActionResult SaveAlert(Notification alert)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _notificationservice.SaveAlert(alert, _globalshared.UserId_G, _globalshared.BranchId_G);
          
            return Ok(result );
        }
        [HttpPost("DeleteNotification")]
        public IActionResult DeleteNotification(int NotificationId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _notificationservice.DeleteNotification(NotificationId, _globalshared.UserId_G, _globalshared.BranchId_G);

            return Ok(result);
        }
        [HttpPost("DeleteAlert")]
        public IActionResult DeleteAlert(int NotificationId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _notificationservice.DeleteAlert(NotificationId, _globalshared.UserId_G, _globalshared.BranchId_G);

            return Ok(result);
        }
        [HttpGet("GetUserNotification")]
        public IActionResult GetUserNotification()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someNoti = _notificationservice.GetUserNotification(_globalshared.UserId_G);
            return Ok(someNoti);
        }

        [HttpGet("GetUnReadUserNotification")]
        public IActionResult GetUnReadUserNotification()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someNoti = _notificationservice.GetUnReadUserNotification(_globalshared.UserId_G);
            return Ok(someNoti);
        }

        [HttpGet("GetUnReadUserNotificationcount")]
        public IActionResult GetUnReadUserNotificationcount()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someNoti = _notificationservice.GetUnReadUserNotificationcount(_globalshared.UserId_G);
            return Ok(someNoti);
        }


        [HttpGet("GetUserAlert")]
        public IActionResult GetUserAlert()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someNoti = _notificationservice.GetUserAlert(_globalshared.UserId_G);
            return Ok(someNoti);
        }
        [HttpGet("NotificationsSent")]
        public IActionResult NotificationsSent()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someNoti = _notificationservice.NotificationsSent(_globalshared.UserId_G);
            return Ok(someNoti);
        }
        [HttpGet("NotificationsSent2")]
        public IActionResult NotificationsSent2()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someNoti = _notificationservice.NotificationsSent2(_globalshared.UserId_G);
            return Ok(someNoti);
        }
        [HttpGet("GetNotificationReceived")]
        public async Task<IActionResult> GetNotificationReceived()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someNoti =await _notificationservice.GetNotificationReceived(_globalshared.UserId_G);
            return Ok(someNoti);

        }

        [HttpGet("GetNotificationTasksStart")]
        public IActionResult GetNotificationTasksStart()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_notificationservice.GetNotificationTasksStart(_globalshared.UserId_G) );
        }
        [HttpGet("GetNotificationTasksStartC")]
        public int GetNotificationTasksStartC()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Count = 0;
            var resultRe = _notificationservice.GetNotificationTasksStart(_globalshared.UserId_G);

            if (resultRe.Result.Count() > 0)
            {
                Count = 1;
            }
            else
            {
                Count = 0;
            }
            return Count;
        }
        [HttpPost("ReadUserNotification")]
        public IActionResult ReadUserNotification()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var res = _notificationservice.ReadUserNotification(_globalshared.UserId_G);
            if (res)
            {
                //UsersData.UserNotifications = null;
            }
            return Ok(_notificationservice.ReadUserNotification(_globalshared.UserId_G) );
        }
        [HttpPost("ReadNotification")]
        public IActionResult ReadNotification(int NotiID)
        {

            var res = _notificationservice.ReadNotification(NotiID);
            return Ok(res );
        }

        [HttpPost("ReadNotificationList")]
        public IActionResult ReadNotificationList([FromBody]List<int> NotiID)
        {

            var res = _notificationservice.ReadNotificationList(NotiID);
            return Ok(res);
        }
        [HttpPost("SetNotificationStatus")]
        public IActionResult SetNotificationStatus(int NotiID)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var res = _notificationservice.SetNotificationStatus(NotiID, Con ?? "", _globalshared.UserId_G, _globalshared.BranchId_G);

            return Ok(res);
        }
        [HttpPost("SetNotificationStatus2")]
        public IActionResult SetNotificationStatus2(List<int> NotiID)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var res = _notificationservice.SetNotificationStatus2(NotiID, Con ?? "", _globalshared.UserId_G, _globalshared.BranchId_G);

            return Ok(res);
        }
        [HttpPost("DeleteNotificationbyProjId")]
        public IActionResult DeleteNotificationbyProjId(int Notid)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _notificationservice.DeleteNotificationById(Notid, _globalshared.UserId_G, _globalshared.BranchId_G);

            return Ok(result);

        }
        [HttpGet("GetUserHomeNotes")]
        public IActionResult GetUserHomeNotes(int NoteType)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _notificationservice.GetUserHomeNotes(_globalshared.UserId_G, NoteType);
            return Ok(result );
        }
        [HttpGet("GetUserBackupNotes")]
        public IActionResult GetUserBackupNotes(int NoteType)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _notificationservice.GetUserBackupNotes(_globalshared.UserId_G, NoteType);
            return Ok(result );
        }
        [HttpGet("GetUserBackupNotesAlert")]
        public IActionResult GetUserBackupNotesAlert()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _notificationservice.GetUserBackupNotesAlert(_globalshared.UserId_G);
            return Ok(result );
        }
        [HttpPost("GenerateRandomNo")]
        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }


        [HttpPost("SaveNotificationwithfile")]
        public IActionResult SaveNotificationwithfile([FromForm] Notification2 notifications, IFormFile? postedFiles)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            

            if (postedFiles != null)
            {
                System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


                string path = System.IO.Path.Combine("Uploads/", "ProjectRequirements/");
                string pathW = System.IO.Path.Combine("/Uploads/", "ProjectRequirements/");

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                List<string> uploadedFiles = new List<string>();
                string pathes = "";
                //foreach (IFormFile postedFile in postedFiles)
                //{
                string fileName = System.IO.Path.GetFileName(Guid.NewGuid() + postedFiles.FileName);
                //string fileName = System.IO.Path.GetFileName(postedFiles.FileName);

                var path2 = Path.Combine(path, fileName);
                if (System.IO.File.Exists(path2))
                {
                    System.IO.File.Delete(path2);
                }
                using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(path, fileName), System.IO.FileMode.Create))
                {


                    postedFiles.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    // string returnpath = host + path + fileName;
                    //pathes.Add(pathW + fileName);
                    pathes = pathW + fileName;
                }


                if (pathes != null)
                {
                    notifications.AttachmentUrl = pathes;
                }
            }
            var result = _notificationservice.SaveNotification2(notifications, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);



        }

    }
}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class MyNotificationsController : ControllerBase
    {
        private readonly IUsersService _usersservice;
        private readonly INotificationService _notificationservice;
        private readonly IFileService _fileservice;
        private readonly IProjectPhasesTasksService _ProjectPhasesTasksService;
        private readonly IProjectWorkersService _projectWorkersservice;
        public GlobalShared _globalshared;
        public MyNotificationsController(IUsersService usersservice, INotificationService notificationservice
            , IFileService fileservice, IProjectPhasesTasksService projectPhasesTasksService
            , IProjectWorkersService projectWorkersservice)
        {
            _usersservice = usersservice;
            _notificationservice = notificationservice;
            _fileservice = fileservice;
            _ProjectPhasesTasksService = projectPhasesTasksService;
            _projectWorkersservice = projectWorkersservice;

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpPost("Index")]
        public IActionResult Index()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            MyNotifi noti = new MyNotifi();
            noti.User = _usersservice.GetUserById(_globalshared.UserId_G, _globalshared.Lang_G);
            noti.AllUserNotifications = _notificationservice.GetNotificationReceived(_globalshared.UserId_G);
            noti.AllUserAlerts = _notificationservice.GetUserAlert(_globalshared.UserId_G);
            noti.NotificationsSent = _notificationservice.NotificationsSent(_globalshared.UserId_G);
            noti.FileUploadCount = _fileservice.GetUserFileUploadCount(_globalshared.UserId_G);
            noti.TasksCount = _ProjectPhasesTasksService.GetUserTaskCount(_globalshared.UserId_G, _globalshared.BranchId_G);
            noti.ProjectWorkerCount = _projectWorkersservice.GetUserProjectWorkerCount(_globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(noti);
        }
      
    }
    public class MyNotifi
    {
        public Task<UsersVM>? User { get; set; }
        public Task<IEnumerable<NotificationVM>>? AllUserNotifications { get; set; }
        public Task<IEnumerable<NotificationVM>>? AllUserAlerts { get; set; }
        public Task<IEnumerable<NotificationVM>>? NotificationsSent { get; set; }
        public Task<int>? FileUploadCount { get; set; }
        public Task<int>? TasksCount { get; set; }
        public Task<int>? ProjectWorkerCount { get; set; }

    }
}

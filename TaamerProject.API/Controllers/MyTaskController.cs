using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Drawing;
using TaamerProject.API.Helper;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    
    public class MyTaskController : ControllerBase
    {
        private readonly IUsersService _usersservice;
        private readonly IProjectPhasesTasksService _ProjectPhasesTasksService;
        private readonly IProjectWorkersService _projectWorkersservice;
        private readonly IFileService _fileservice;
        private string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        public MyTaskController(IConfiguration _configuration, IFileService fileservice,
            IProjectWorkersService projectWorkersservice, IProjectPhasesTasksService ProjectPhasesTasksService,
            IUsersService usersservice )
        {
            _usersservice = usersservice;
            _projectWorkersservice = projectWorkersservice;
            _ProjectPhasesTasksService = ProjectPhasesTasksService;
            _fileservice = fileservice;
            Configuration = _configuration;Con = this.Configuration.GetConnectionString("DBConnection");
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }

        //public ActionResult Index()
        //{
        //    ViewBag.User = _usersservice.GetUserById(UserId, Lang);
        //    ViewBag.AllUserTasks = _ProjectPhasesTasksService.GetTasksByUserId(UserId, 0, BranchId);
        //    ViewBag.TasksCount = _ProjectPhasesTasksService.GetUserTaskCount(UserId, BranchId);
        //    ViewBag.ProjectWorkerCount = _projectWorkersservice.GetUserProjectWorkerCount(UserId, BranchId);
        //    ViewBag.FileUploadCount = _fileservice.GetUserFileUploadCount(UserId);

        //    ViewBag.LateTasks = _ProjectPhasesTasksService.GetLateTasksByUserId(UserId, 0, BranchId).Count();

        //    return View();
        //}


        //public ActionResult MyTasks()
        //{
        //    ViewBag.User = _usersservice.GetUserById(UserId, Lang);
        //    ViewBag.AllUserTasks = _ProjectPhasesTasksService.GetTasksByUserId(UserId, 0, BranchId);
        //    ViewBag.TasksCount = _ProjectPhasesTasksService.GetUserTaskCount(UserId, BranchId);
        //    ViewBag.ProjectWorkerCount = _projectWorkersservice.GetUserProjectWorkerCount(UserId, BranchId);
        //    ViewBag.FileUploadCount = _fileservice.GetUserFileUploadCount(UserId);

        //    ViewBag.LateTasks = _ProjectPhasesTasksService.GetLateTasksByUserId(UserId, 0, BranchId).Count();
        //    return View();
        //}
        [HttpGet("GetDoneTasksDGV")]
        public IActionResult GetDoneTasksDGV(string FromDate, string ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProjectPhasesTasksService.GetDoneTasksDGV(FromDate, ToDate, _globalshared.UserId_G, Con??"");
            return Ok(result );
        }
        [HttpGet("GetUndergoingTasksDGV")]
        public IActionResult GetUndergoingTasksDGV(string FromDate, string ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProjectPhasesTasksService.GetUndergoingTasksDGV(FromDate, ToDate, _globalshared.UserId_G, Con ?? "");
            return Ok(result );
        }
        [HttpGet("GetDelayedTasksDGV")]
        public IActionResult GetDelayedTasksDGV(string FromDate, string ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProjectPhasesTasksService.GetEmpDelayedTasksDGV(FromDate, ToDate, _globalshared.UserId_G, Con ?? "");
            return Ok(result);
        }
        [HttpGet("GetEmpDoneWOsDGV")]
        public IActionResult GetEmpDoneWOsDGV()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProjectPhasesTasksService.GetEmpDoneWOsDGV(_globalshared.UserId_G, Con ?? "");
            return Ok(result);
        }
        [HttpGet("GetEmpUnderGoingWOsDGV")]
        public IActionResult GetEmpUnderGoingWOsDGV()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProjectPhasesTasksService.GetEmpUnderGoingWOsDGV(_globalshared.UserId_G, Con ?? "");
            return Ok(result);
        }
        [HttpGet("GetEmpDelayedWOsDGV")]
        public IActionResult GetEmpDelayedWOsDGV()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProjectPhasesTasksService.GetEmpDelayedWOsDGV(_globalshared.UserId_G, Con ?? "");
            return Ok(result);
        }
        [HttpGet("GetLateTasksByUserId")]
        public IActionResult GetLateTasksByUserId()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProjectPhasesTasksService.GetLateTasksByUserId(_globalshared.UserId_G, 0, _globalshared.BranchId_G).Result.Count();
            return Ok(result);
        }

        [HttpGet("GetDayWeekMonth_Tasks")]
        public int GetDayWeekMonth_Tasks(int flag, string? startdate, string? enddate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProjectPhasesTasksService.GetDayWeekMonth_Tasks(_globalshared.UserId_G, 0, _globalshared.BranchId_G, flag, startdate, enddate).Result.Count();
            return result;
        }

    }
}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ControllingTaskController : ControllerBase
    {  
        private readonly IProjectPhasesTasksService _ProjectPhasesTasksService;
        private readonly IProjectService _ProjectService;
        private IUsersService _UsersService;
        private IBranchesService _branchesService;
        private string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        public ControllingTaskController(IProjectPhasesTasksService ProjectPhasesTasksService
            , IBranchesService branchesService, IConfiguration _configuration, IUsersService UsersService
            , IProjectService ProjectService)
        {
            _ProjectPhasesTasksService= ProjectPhasesTasksService;
            this._branchesService = branchesService;
            _ProjectService = ProjectService;
            _UsersService = UsersService;
            Configuration = _configuration;
            Con = this.Configuration.GetConnectionString("DBConnection");
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
         }
        
        //public IActionResult Index()
        //{
        //    ViewBag.Projects = _ProjectService.GetUserProjects(UserId, BranchId, "");
        //    ViewBag.Projects = _ProjectService.GetUserProjects2(UserId, BranchId, "");

        //    var Tasks = _ProjectPhasesTasksService.GetTasksByUserId(UserId, 0, BranchId);
        //    ViewBag.AllProjectTasks = Tasks;
        //    ViewBag.NotStartedCount = Tasks.Count(s => s.Status == 1);
        //    ViewBag.InProgressCount = Tasks.Count(s => s.Status == 2);
        //    ViewBag.StoppedCount = Tasks.Count(s => s.Status == 3);
        //    ViewBag.FinishedCount = Tasks.Count(s => s.Status == 4);
        //    ViewBag.CanceledCount = Tasks.Count(s => s.Status == 5);
        //    ViewBag.DeletedCount = Tasks.Count(s => s.Status == 6);
        //    ViewBag.WasConverted = Tasks.Count(s => s.Status == 7);
        //    return View();
        //}
         
        [HttpGet("GetMyProjects")]
        public IActionResult GetMyProjects()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var obj = new ProjectVM();
            obj.BranchId = _globalshared.BranchId_G;
            obj.Status = 0;
            var MyProjects = _ProjectService.GetAllProjectsNew(Con ?? "", obj, _globalshared.UserId_G, 0,0, _globalshared.BranchId_G).Result.ToList();

            if (MyProjects.Count() > 0)
            {
                MyProjects = MyProjects.Where(a => a.MangerId == _globalshared.UserId_G).ToList();
            }


            return Ok(MyProjects );
        }
        
        [HttpGet("FillMainPhasesSelect")]
        public IActionResult FillMainPhasesSelect(int param)
        {
            return Ok(_ProjectPhasesTasksService.FillProjectMainPhases(param));
        }
        [HttpGet("FillProjectSubPhases")]
        public IActionResult FillProjectSubPhases(int param)
        {
            return Ok(_ProjectPhasesTasksService.FillProjectSubPhases(param));
        }

        [HttpGet("GetAllSubPhasesTasks")]
        public IActionResult GetAllSubPhasesTasks(int SubPhaseId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_ProjectPhasesTasksService.GetAllSubPhasesTasks(SubPhaseId, _globalshared.Lang_G));
        }
        [HttpGet("GetAllSubPhasesTasksbyUserId")]
        public IActionResult GetAllSubPhasesTasksbyUserId(int SubPhaseId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_ProjectPhasesTasksService.GetAllSubPhasesTasksbyUserId(SubPhaseId,  _globalshared.UserId_G));
        }
        [HttpGet("FillUsershaveRunningTasks")]
        public IActionResult FillUsershaveRunningTasks()
        {
            return Ok(_ProjectPhasesTasksService.FillUsershaveRunningTasks(   ) );
        }


        [HttpGet("FillUsershaveRunningTasksByBranch")]
        public IActionResult FillUsershaveRunningTasksByBranch()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_ProjectPhasesTasksService.FillUsershaveRunningTasks(_globalshared.BranchId_G));
        }


        [HttpGet("FillUsershaveRunningTasksWithBranch")]
        public IActionResult FillUsershaveRunningTasksWithBranch()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_ProjectPhasesTasksService.FillUsershaveRunningTasksWithBranch(_globalshared.BranchId_G));
        }


        [HttpGet("GetAllSubPhasesTasksbyUserId2")]
        public IActionResult GetAllSubPhasesTasksbyUserId2(int SubPhaseId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_ProjectPhasesTasksService.GetAllSubPhasesTasksbyUserId2(SubPhaseId, _globalshared.UserId_G) );
        }
        [HttpGet("FillUsersSelect")]
        public IActionResult FillUsersSelect(int param)
        {
            return Ok(_UsersService.FillUserSelect(param ) );
        }
        [HttpGet("FillAllUsersSelect")]
        public IActionResult FillAllUsersSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_UsersService.FillAllUserSelect(  _globalshared.UserId_G) );
        }
        [HttpGet("FillAllUsersSelectAll")]
        public IActionResult FillAllUsersSelectAll()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_UsersService.FillAllUsersSelectAll(  _globalshared.UserId_G) );
        }
        [HttpGet("FillAllUsersSelectAllGrantt")]
        public IActionResult FillAllUsersSelectAllGrantt(string param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_UsersService.FillAllUsersSelectAllGrantt(_globalshared.UserId_G, param));
        }

        [HttpGet("FillAllUsersSelectAllByBranch")]
        public IActionResult FillAllUsersSelectAllByBranch()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_UsersService.FillAllUsersSelectAll(_globalshared.UserId_G,_globalshared.BranchId_G));
        }

        [HttpGet("FillAllUsersSelectsomeByBranch")]
        public IActionResult FillAllUsersSelectsomeByBranch()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result.ToList();
            var someUser = _UsersService.FillAllUsersSelectAll(_globalshared.UserId_G, 0).ToList();
            foreach (var userBranch in userBranchs)
            {
                var AllPojects = _UsersService.FillAllUsersSelectAll(_globalshared.UserId_G, userBranch.BranchId).ToList();
                var Projects = someUser.Union(AllPojects);
                someUser = Projects.ToList();
            }
            return Ok(someUser);
        }


        [HttpGet("FillWOStatusSelect")]
        public IActionResult FillWOStatusSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_UsersService.FillAllUserSelect(  _globalshared.UserId_G));
        }
        [HttpGet("GetUserAndBranch")]
        public IActionResult GetUserAndBranch()
        {
            return Ok(_UsersService.GetAllUsers( ));
        }
        [HttpGet("GetUserAndBranchByNameSearch")]
        public IActionResult GetUserAndBranchByNameSearch(Users users)
        {
            return Ok(_UsersService.GetUserAndBranchByNameSearch(users ) );
        }
        [HttpGet("GetAllWOStatuses")]
        public IActionResult GetAllWOStatuses()
        {
 
            return Ok(_ProjectService.GetAllWOStatuses(Con ) );
        }
    }
}

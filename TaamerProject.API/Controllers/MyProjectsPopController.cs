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

    public class MyProjectsPopController : ControllerBase
    {

        private IProjectPhasesTasksService _ProjectPhasesTasksService;
        private IProjectService _ProjectService;
        private IUsersService _UsersService;
        public GlobalShared _globalshared;
        public MyProjectsPopController(IProjectPhasesTasksService projectPhasesTasksService
            , IProjectService projectService, IUsersService usersService)
        {
            _ProjectPhasesTasksService = projectPhasesTasksService;
            _ProjectService = projectService;
            _UsersService = usersService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
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
            return Ok(_ProjectPhasesTasksService.GetAllSubPhasesTasksbyUserId(SubPhaseId, _globalshared.UserId_G));
        }
        [HttpGet("FillUsersSelect")]
        public IActionResult FillUsersSelect(int param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_UsersService.FillUserSelect(_globalshared.UserId_G));
        }
        [HttpGet("FillAllUsersSelect")]
        public IActionResult FillAllUsersSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_UsersService.FillAllUserSelect(_globalshared.UserId_G));
        }
        [HttpGet("GetUserAndBranch")]
        public IActionResult GetUserAndBranch()
        {
            return Ok(_UsersService.GetAllUsers());
        }
        [HttpGet("GetUserAndBranchByNameSearch")]
        public IActionResult GetUserAndBranchByNameSearch(Users users)
        {
            return Ok(_UsersService.GetUserAndBranchByNameSearch(users));
        }
        [HttpGet("FillAllUsersSelectExcept")]
        public IActionResult FillAllUsersSelectExcept(int ExceptUserId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_UsersService.FillAllUserSelect(ExceptUserId));
        }
    }
}

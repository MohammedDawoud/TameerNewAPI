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

    public class ProjectStepsController : ControllerBase
    {
        private readonly IPro_ProjectStepsService _Pro_ProjectStepsService;
        private readonly IPro_ProjectAchievementsService _Pro_ProjectAchievementsService;
        private readonly IPro_ProjectChallengesService _Pro_ProjectChallengesService;

        public GlobalShared _globalshared;

        public ProjectStepsController(IPro_ProjectStepsService Pro_ProjectStepsService, IPro_ProjectAchievementsService Pro_ProjectAchievementsService
            , IPro_ProjectChallengesService Pro_ProjectChallengesService)
        {
            _Pro_ProjectStepsService = Pro_ProjectStepsService;
            _Pro_ProjectAchievementsService = Pro_ProjectAchievementsService;
            _Pro_ProjectChallengesService = Pro_ProjectChallengesService;

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllProjectSteps")]

        public IActionResult GetAllProjectStep()
        {
            var ProjectSteps = _Pro_ProjectStepsService.GetAllProjectSteps();
            return Ok(ProjectSteps);
        }
        [HttpGet("GetProjectStepsbyprojectid")]

        public IActionResult GetProjectStepsbyprojectid(int projectid, int stepid)
        {
            var ProjectSteps = _Pro_ProjectStepsService.GetProjectStepsbyprojectid(projectid, stepid);
            return Ok(ProjectSteps);
        }
        [HttpGet("GetProjectStepsbyprojectidOnly")]

        public IActionResult GetProjectStepsbyprojectidOnly(int projectid)
        {
            var ProjectSteps = _Pro_ProjectStepsService.GetProjectStepsbyprojectidOnly(projectid);
            return Ok(ProjectSteps);
        }
        [HttpGet("GetAllProjectAchievementsbyprojectid")]

        public IActionResult GetAllProjectAchievementsbyprojectid(int projectid, int stepid)
        {
            var ProjectSteps = _Pro_ProjectAchievementsService.GetAllProjectAchievementsbyprojectid(projectid, stepid);
            return Ok(ProjectSteps);
        }
        [HttpGet("GetAllProjectAchievementsbyprojectidOnly")]

        public IActionResult GetAllProjectAchievementsbyprojectidOnly(int projectid)
        {
            var ProjectSteps = _Pro_ProjectAchievementsService.GetAllProjectAchievementsbyprojectidOnly(projectid);
            return Ok(ProjectSteps);
        }
        [HttpGet("GetAllProjectChallengesbyprojectid")]

        public IActionResult GetAllProjectChallengesbyprojectid(int projectid, int stepid)
        {
            var ProjectSteps = _Pro_ProjectChallengesService.GetAllProjectChallengesbyprojectid(projectid, stepid);
            return Ok(ProjectSteps);
        }
        [HttpGet("GetAllProjectChallengesbyprojectidOnly")]

        public IActionResult GetAllProjectChallengesbyprojectidOnly(int projectid)
        {
            var ProjectSteps = _Pro_ProjectChallengesService.GetAllProjectChallengesbyprojectidOnly(projectid);
            return Ok(ProjectSteps);
        }
        [HttpPost("UpdateProjectStepStatus")]

        public IActionResult UpdateProjectStepStatus(Pro_ProjectSteps ProjectStep)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_ProjectStepsService.UpdateProjectStepStatus(ProjectStep, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("ReturnProjectStepStatus")]

        public IActionResult ReturnProjectStepStatus(Pro_ProjectSteps ProjectStep)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_ProjectStepsService.ReturnProjectStepStatus(ProjectStep, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveProjectStep")]

        public IActionResult SaveProjectStep(int projectId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_ProjectStepsService.SaveProjectStep(projectId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }

        [HttpPost("SaveProjectAchievement")]
        public IActionResult SaveProjectAchievement(List<Pro_ProjectAchievements>? ProjectAchievements)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_ProjectAchievementsService.SaveProjectAchievement(ProjectAchievements ?? new List<Pro_ProjectAchievements>(), _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveProjectChallenge")]
        public IActionResult SaveProjectChallenge(List<Pro_ProjectChallenges>? ProjectChallenges)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_ProjectChallengesService.SaveProjectChallenge(ProjectChallenges ?? new List<Pro_ProjectChallenges>(), _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteProjectStep")]

        public IActionResult DeleteProjectStep(int ProjectStepid)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_ProjectStepsService.DeleteProjectStep(ProjectStepid, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }

        public class ProjectAchCha_C
        {
            public List<Pro_ProjectAchievements>? ProjectAchievements { get; set; }
            public List<Pro_ProjectChallenges>? ProjectChallenges { get; set; }
        }
    }
}

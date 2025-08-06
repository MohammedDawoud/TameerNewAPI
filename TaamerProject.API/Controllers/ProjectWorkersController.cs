using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ProjectWorkersController : ControllerBase
    {
        private IProjectWorkersService _projectWorkersservice;
        public GlobalShared _globalshared;
        public ProjectWorkersController(IProjectWorkersService projectWorkersservice)
        {
            _projectWorkersservice = projectWorkersservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllProjectWorkers")]
        public IActionResult GetAllProjectWorkers(int? ProjectId, string? SearchText)
        {
            return Ok(_projectWorkersservice.GetAllProjectWorkers(ProjectId, SearchText ?? ""));
        }
        [HttpPost("SaveProjectWorker")]
        public IActionResult SaveProjectWorker(ProjectWorkers ProjectWorkers)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectWorkersservice.SaveProjectWorker(ProjectWorkers, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteProjectWorker")]
        public IActionResult DeleteProjectWorker(int WorkerId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectWorkersservice.DeleteProjectWorker(WorkerId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}

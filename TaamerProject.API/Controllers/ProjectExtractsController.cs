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

    public class ProjectExtractsController : ControllerBase
    {
        private IProjectExtractsService _projectExtractsservice;
        public GlobalShared _globalshared;
        public ProjectExtractsController(IProjectExtractsService projectExtractsservice)
        {
            _projectExtractsservice = projectExtractsservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllProjectExtracts")]
        public IActionResult GetAllProjectExtracts(int? ProjectId)
        {
            return Ok(_projectExtractsservice.GetAllProjectExtracts(ProjectId));
        }
        [HttpPost("SaveProjectExtracts")]
        public IActionResult SaveProjectExtracts(ProjectExtracts projectExtracts)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectExtractsservice.SaveProjectExtracts(projectExtracts, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteProjectExtracts")]
        public IActionResult DeleteProjectExtracts(int ExtractId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectExtractsservice.DeleteProjectExtracts(ExtractId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}

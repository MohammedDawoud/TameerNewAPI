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
    public class ProjectTypeController : ControllerBase
    {
        private IProjectTypeService _projectTypeservice;
        private IRequirementsandGoalsService _requirementsandGoalsService;
        private IProjectRequirementsGoalsService _projectRequirementsGoalsService;
        public GlobalShared _globalshared;
        public ProjectTypeController(IProjectTypeService projectTypeservice
            , IRequirementsandGoalsService requirementsandGoalsService
            , IProjectRequirementsGoalsService projectRequirementsGoalsService)
        {
            _projectTypeservice = projectTypeservice;
            _requirementsandGoalsService = requirementsandGoalsService;
            _projectRequirementsGoalsService = projectRequirementsGoalsService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllProjectType")]
        public IActionResult GetAllProjectType(string? SearchText)
        {
            return Ok(_projectTypeservice.GetAllProjectType(SearchText??""));
        }
        [HttpGet("GetAllRequirmentbyprojecttype")]
        public IActionResult GetAllRequirmentbyprojecttype(int projecttypeid)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_requirementsandGoalsService.GetAllrequirmentbyProjecttype(_globalshared.Lang_G, projecttypeid));
        }
        [HttpPost("deleteprojectrequirment")]
        public IActionResult deleteprojectrequirment(int requirmentid)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_requirementsandGoalsService.deleteprojectrequirment(requirmentid, _globalshared.UserId_G, _globalshared.BranchId_G));
        }
        [HttpGet("GetAllrequirmentbyProjectid")]
        public IActionResult GetAllrequirmentbyProjectid(int projectid)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectRequirementsGoalsService.GetAllrequirmentbyProjectid(_globalshared.Lang_G, projectid));
        }
        [HttpGet("GetAllrequirmentbyrequireid")]
        public IActionResult GetAllrequirmentbyrequireid(int RequirmentId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectRequirementsGoalsService.GetAllrequirmentbyrequireid(_globalshared.Lang_G, RequirmentId));
        }
        [HttpGet("FillProjectRequirmentSelect")]
        public IActionResult FillProjectRequirmentSelect(int Param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectRequirementsGoalsService.GetFilterdrequirmentbyProjectid(_globalshared.Lang_G, Param));
        }
        [HttpPost("SaveProjectType")]
        public IActionResult SaveProjectType(ProjectType projectType)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectTypeservice.SaveProjectType(projectType, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteProjectType")]
        public IActionResult DeleteProjectType(int ProjectTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _projectTypeservice.DeleteProjectType(ProjectTypeId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillProjectTypeSelect")]
        public IActionResult FillProjectTypeSelect(string? SearchText)
        {
            return Ok(_projectTypeservice.GetAllProjectType(SearchText??"").Select(s => new {
                Id = s.TypeId,
                Name = s.NameAr,
                NameEn= s.NameEn,

            }));
        }
        [HttpGet("FillProjectTypeRequirmentSelect")]
        public IActionResult FillProjectTypeRequirmentSelect(int param, int param2)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_requirementsandGoalsService.GetAllrequirmentbyProjecttype2(_globalshared.Lang_G, param, param2).Result.Select(s => new {
                Id = s.RequirementId,
                Name = s.RequirmentName

            }));
        }
        [HttpGet("FillProjectTypeRequirmentSelect2")]
        public IActionResult FillProjectTypeRequirmentSelect2(int param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_requirementsandGoalsService.GetAllrequirmentbyProjecttype(_globalshared.Lang_G, param).Result.Select(s => new {
                Id = s.RequirementId,
                Name = s.RequirmentName

            }));
        }
        [HttpGet("FillAllrequirmentbyProjectid")]
        public IActionResult FillAllrequirmentbyProjectid(int Param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectRequirementsGoalsService.GetAllrequirmentbyProjectid(_globalshared.Lang_G, Param).Result.Select(s => new {
                Id = s.RequirementId,
                Name = s.RequirmentName

            }));
        }

    }
}

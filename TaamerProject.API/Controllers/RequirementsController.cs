using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class RequirementsController : ControllerBase
    {
        private IRequirementsService _requirementsservice;
        public GlobalShared _globalshared;
        public RequirementsController(IRequirementsService requirementsservice)
        {
            _requirementsservice = requirementsservice;
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllRequirements")]
        public IActionResult GetAllRequirements()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_requirementsservice.GetAllRequirements(_globalshared.BranchId_G));
        }
        [HttpGet("GetAllRequirementsByProjectId")]
        public IActionResult GetAllRequirementsByProjectId(int ProjectId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_requirementsservice.GetAllRequirementsByProjectId(ProjectId,_globalshared.BranchId_G));
        }

        [HttpPost("ConfirmRequirementStatus")]
        public IActionResult ConfirmRequirementStatus(int RequirementId, bool Status)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _requirementsservice.ConfirmRequirementStatus(RequirementId, Status, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveRequirements")]
        public IActionResult SaveRequirements(Requirements requirements)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _requirementsservice.SaveRequirements(requirements, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveRequirementsbyProjectId")]
        public IActionResult SaveRequirementsbyProjectId(int ProjectId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _requirementsservice.SaveRequirementsbyProjectId(ProjectId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteRequirement")]
        public IActionResult DeleteRequirement(int RequirementId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _requirementsservice.DeleteRequirement(RequirementId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }

        [HttpGet("FillRequirementsSelect")]
        public IActionResult FillRequirementsSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var req = _requirementsservice.FillRequirementsSelect(_globalshared.BranchId_G).Result.Select(s => new
            {
                Id = s.RequirementId,
                Name = s.NameAr
            });
            return Ok(req);
        }
    }
}

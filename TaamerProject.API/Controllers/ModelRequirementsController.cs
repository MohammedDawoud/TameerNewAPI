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

    public class ModelRequirementsController : ControllerBase
    {
        private IModelRequirementsService _requirementsservice;
        public GlobalShared _globalshared;
        public ModelRequirementsController(IModelRequirementsService requirementsservice)
        {
            _requirementsservice = requirementsservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllModelRequirements")]
        public IActionResult GetAllModelRequirements()
        {
            return Ok(_requirementsservice.GetAllModelRequirements(_globalshared.BranchId_G));
        }
        [HttpPost("SaveModelRequirements")]
        public IActionResult SaveModelRequirements(ModelRequirements modelRequirements)
        {
            var result = _requirementsservice.SaveModelRequirements(modelRequirements, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteModelRequirement")]
        public IActionResult DeleteModelRequirements(int ModelReqId)
        {
            var result = _requirementsservice.DeleteModelRequirement(ModelReqId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetAllModelRequirementsByModelId")]
        public IActionResult GetAllModelRequirementsByModelId(int ModelId)
        {
            return Ok(_requirementsservice.GetAllModelRequirementsByModelId(ModelId));
        }
    }
}

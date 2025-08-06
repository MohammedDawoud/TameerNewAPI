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

    public class ModelTypeController : ControllerBase
    {
        private IModelTypeService _modelTypeervice;
        public GlobalShared _globalshared;
        public ModelTypeController(IModelTypeService modelTypeervice)
        {
            _modelTypeervice = modelTypeervice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllModelTypes")]
        public IActionResult GetAllModelTypes()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_modelTypeervice.GetAllModelTypes(_globalshared.BranchId_G));
        }
        [HttpPost("SaveModelType")]
        public IActionResult SaveModelType(ModelType modelType)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _modelTypeervice.SaveModelType(modelType, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteModelType")]
        public IActionResult DeleteModelType(int ModelTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _modelTypeervice.DeleteModelType(ModelTypeId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillModelTypeSelect")]
        public IActionResult FillModelTypeSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_modelTypeervice.FillModelTypeSelect(_globalshared.BranchId_G));
        }
    }
}

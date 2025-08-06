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
    
    public class OutInBoxTypeController : ControllerBase
    {
        private IOutInBoxTypeService _OutInBoxTypeservice;
        public GlobalShared _globalshared;
        public OutInBoxTypeController(IOutInBoxTypeService OutInBoxTypeservice)
        {
             _OutInBoxTypeservice = OutInBoxTypeservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllOutInBoxTypes")]
        public IActionResult GetAllOutInBoxTypes(string? SearchText)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_OutInBoxTypeservice.GetAllOutInBoxTypes(SearchText??"", _globalshared.BranchId_G) );
        }
        [HttpPost("SaveOutInBoxType")]
        public IActionResult SaveOutInBoxType(OutInBoxType OutInBoxType)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _OutInBoxTypeservice.SaveOutInBoxType(OutInBoxType, _globalshared.UserId_G, _globalshared.BranchId_G);
            
            return Ok(result);
        }
        [HttpPost("DeleteOutInBoxType")]
        public IActionResult DeleteOutInBoxType(int TypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _OutInBoxTypeservice.DeleteOutInBoxType(TypeId, _globalshared.UserId_G, _globalshared.BranchId_G);

            return Ok(result);
        }
        [HttpGet("FillOutInBoxTypeSelect")]
        public IActionResult FillOutInBoxTypeSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_OutInBoxTypeservice.FillOutInBoxTypeSelect(_globalshared.BranchId_G) );
        }
    }
}

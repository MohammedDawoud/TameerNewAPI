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
    
    public class OutInBoxSerialController : ControllerBase
    {
        private IOutInBoxSerialService _outInBoxSerialservice;
        public GlobalShared _globalshared;
        public OutInBoxSerialController(IOutInBoxSerialService outInBoxSerialservice)
        {
             _outInBoxSerialservice = outInBoxSerialservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllOutInBoxSerial")]
        public IActionResult GetAllOutInBoxSerial(int Type)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_outInBoxSerialservice.GetAllOutInBoxSerial(Type, _globalshared.BranchId_G) );
        }
        [HttpGet("GenerateOutInBoxNumber")]
        public IActionResult GenerateOutInBoxNumber(int outInSerialId)
        {
            return Ok(_outInBoxSerialservice.GenerateOutInBoxNumber(outInSerialId) );
        }
        [HttpPost("SaveOutInBoxSerial")]
        public IActionResult SaveOutInBoxSerial(OutInBoxSerial OutInBoxSerial)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _outInBoxSerialservice.SaveOutInBoxSerial(OutInBoxSerial, _globalshared.UserId_G, _globalshared.BranchId_G);
            
            return Ok(result);
        }
        [HttpPost("DeleteOutInBoxSerial")]
        public IActionResult DeleteOutInBoxSerial(int OutInSerialId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _outInBoxSerialservice.DeleteOutInBoxSerial(OutInSerialId, _globalshared.UserId_G, _globalshared.BranchId_G);
            
            return Ok(result);
        }
    }
}

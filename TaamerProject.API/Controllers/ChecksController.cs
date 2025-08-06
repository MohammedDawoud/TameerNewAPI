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

    public class ChecksController : ControllerBase
    {
        private readonly IChecksService _ChecksService;
        public GlobalShared _globalshared;
        public ChecksController(IChecksService checksService)
        {
            _ChecksService = checksService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllChecks")]
        public IActionResult GetAllChecks(int Type)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_ChecksService.GetAllChecks(Type, _globalshared.BranchId_G));
        }
        [HttpPost("SaveCheck")]
        public IActionResult SaveCheck(Checks Check)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ChecksService.SaveCheck(Check, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteCheck")]
        public IActionResult DeleteCheck(int CheckId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ChecksService.DeleteCheck(CheckId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetAllCheckSearch")]
        public IActionResult GetAllCheckSearch(ChecksVM checkSearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_ChecksService.GetAllCheckSearch(checkSearch, _globalshared.BranchId_G));

        }
    }
}

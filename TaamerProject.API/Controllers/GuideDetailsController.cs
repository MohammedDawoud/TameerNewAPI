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

    public class GuideDetailsController : ControllerBase
    {
        private IGuideDepartmentDetailsService _guideDepartmentDetailsService;
        public GlobalShared _globalshared;
        public GuideDetailsController(IGuideDepartmentDetailsService guideDepartmentDetailsService)
        {
            _guideDepartmentDetailsService = guideDepartmentDetailsService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllDepDetails")]
        public IActionResult GetAllDepDetails(int DepId, string searchStr, int? DepDetailId = null)
        {
            var Deps = _guideDepartmentDetailsService.GetAllDepDetails(DepId, searchStr, DepDetailId);
            return Ok(Deps);
        }

        [HttpGet("GetAllDepDetails2")]
        public IActionResult GetAllDepDetails2(string? searchStr)
        {
            var Deps = _guideDepartmentDetailsService.GetAllDepDetails2(searchStr);
            return Ok(Deps);
        }
        [HttpPost("SaveDetails")]
        public IActionResult SaveDetails(GuideDepartmentDetails DepDetails)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _guideDepartmentDetailsService.SaveDetails(DepDetails, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteDetails")]
        public IActionResult DeleteDetails(int DepDetailId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _guideDepartmentDetailsService.DeleteDetails(DepDetailId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}

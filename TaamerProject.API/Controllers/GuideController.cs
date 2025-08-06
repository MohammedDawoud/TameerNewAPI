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

    public class GuideController : ControllerBase
    {
        private IGuideDepartmentsService _guideDepartmentsService;
        public GlobalShared _globalshared;
        public GuideController(IGuideDepartmentsService guideDepartmentsService)
        {
            _guideDepartmentsService = guideDepartmentsService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllDeps")]
        public IActionResult GetAllDeps(int? DepId = null)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Deps = _guideDepartmentsService.GetAllDeps(_globalshared.Lang_G, DepId);
            return Ok(Deps);
        }
        [HttpPost("SaveGroups")]
        public IActionResult SaveGroups(GuideDepartments Dep)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _guideDepartmentsService.SaveDepartment(Dep, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }

        [HttpPost("DeleteDept")]
        public IActionResult DeleteDept(int DepId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _guideDepartmentsService.DeleteDepartment(DepId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}

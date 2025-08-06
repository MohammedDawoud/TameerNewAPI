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

    public class ExternalEmployeesController : ControllerBase
    {
        private IExternalEmployeesService _externalEmployeesservice;
        public GlobalShared _globalshared;
        public ExternalEmployeesController(IExternalEmployeesService externalEmployeesservice)
        {
            _externalEmployeesservice = externalEmployeesservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllExternalEmployees")]
        public IActionResult GetAllExternalEmployees(int? DepartmentId, string SearchText)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_externalEmployeesservice.GetAllExternalEmployees(DepartmentId, SearchText ?? "", _globalshared.BranchId_G));
        }
        [HttpPost("SaveExternalEmployees")]
        public IActionResult SaveExternalEmployees(ExternalEmployees externalEmployees)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _externalEmployeesservice.SaveExternalEmployees(externalEmployees, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteExternalEmployees")]
        public IActionResult DeleteExternalEmployees(int EmpId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _externalEmployeesservice.DeleteExternalEmployees(EmpId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillExternalEmployeeSelect")]
        public IActionResult FillExternalEmployeeSelect(int? param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_externalEmployeesservice.FillExternalEmployeeSelect(param, _globalshared.BranchId_G));
        }
    }
}

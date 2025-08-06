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

    public class ExpensesGovernmentController : ControllerBase
    {
        private IExpensesGovernmentService _expensesGovernmentservice;
        public GlobalShared _globalshared;
        public ExpensesGovernmentController(IExpensesGovernmentService expensesGovernmentservice)
        {
            _expensesGovernmentservice = expensesGovernmentservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllExpensesGovernment")]
        public IActionResult GetAllExpensesGovernment(int? EmpId, string SearchText)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_expensesGovernmentservice.GetAllExpensesGovernment(EmpId, SearchText ?? ""));
        }
        [HttpPost("SaveExpensesGovernment")]
        public IActionResult SaveExpensesGovernment(ExpensesGovernment expensesGovernment)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _expensesGovernmentservice.SaveExpensesGovernment(expensesGovernment, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteExpensesGovernment")]
        public IActionResult DeleteExpensesGovernment(int ExpensesId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _expensesGovernmentservice.DeleteExpensesGovernment(ExpensesId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}

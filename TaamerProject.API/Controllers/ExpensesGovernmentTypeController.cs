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

    public class ExpensesGovernmentTypeController : ControllerBase
    {
        private IExpensesGovernmentTypeService _ExpensesGovernmentTypeservice;
        public GlobalShared _globalshared;
        public ExpensesGovernmentTypeController(IExpensesGovernmentTypeService expensesGovernmentTypeservice)
        {
            _ExpensesGovernmentTypeservice = expensesGovernmentTypeservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllExpensesGovernmentTypes")]
        public IActionResult GetAllExpensesGovernmentTypes(string SearchText)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_ExpensesGovernmentTypeservice.GetAllExpensesGovernmentTypes(SearchText, _globalshared.BranchId_G));
        }
        [HttpPost("SaveExpensesGovernmentType")]
        public IActionResult SaveExpensesGovernmentType(ExpensesGovernmentType expensesGovernmentType)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ExpensesGovernmentTypeservice.SaveExpensesGovernmentType(expensesGovernmentType, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteExpensesGovernmentType")]
        public IActionResult DeleteExpensesGovernmentType(int ExpensesGovernmentTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ExpensesGovernmentTypeservice.DeleteExpensesGovernmentType(ExpensesGovernmentTypeId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillExpensesGovernmentTypeSelect")]
        public IActionResult FillExpensesGovernmentTypeSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_ExpensesGovernmentTypeservice.FillExpensesGovernmentTypeSelect(_globalshared.BranchId_G));
        }
    }
}

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

    public class ExpRevenuExpensesController : ControllerBase
    {
        private readonly IExpRevenuExpensesService _ExpRevenuExpensesService;
        string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        public ExpRevenuExpensesController(IExpRevenuExpensesService expRevenuExpensesService, IConfiguration _configuration)
        {
            _ExpRevenuExpensesService = expRevenuExpensesService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");

        }
        [HttpGet("GetAllExpRevenuExpenses")]
        public IActionResult GetAllExpRevenuExpenses()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_ExpRevenuExpensesService.GetAllExpRevenuExpenses(_globalshared.BranchId_G));
        }
        [HttpGet("getallExpBysearchobject")]
        public IActionResult getallExpBysearchobject(ExpRevenuExpensesVM expsearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_ExpRevenuExpensesService.GetAllExpBysearchObject(expsearch, _globalshared.BranchId_G));
        }
        [HttpPost("SaveExpRevenuExpenses")]
        public IActionResult SaveExpRevenuExpenses(ExpRevenuExpenses expRevenuExpenses)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ExpRevenuExpensesService.SaveExpRevenuExpenses(expRevenuExpenses, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteRevenuExpenses")]
        public IActionResult DeleteRevenuExpenses(int ExpectedId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ExpRevenuExpensesService.DeleteRevenuExpenses(ExpectedId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FinishRestoreRevenuExpenses")]
        public IActionResult FinishRestoreRevenuExpenses(int ExpectedId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ExpRevenuExpensesService.FinishRestoreRevenuExpenses(ExpectedId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetTotalExpRevByCC")]
        public IActionResult GetTotalExpRevByCC()
        {
            return Ok(_ExpRevenuExpensesService.GetTotalExpRevByCC(Con));
        }
    }
}

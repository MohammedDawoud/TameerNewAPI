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

    public class TransactionTypesController : ControllerBase
    {
        private ITransactionTypesService _TransactionTypesservice;
        public GlobalShared _globalshared;
        public TransactionTypesController(ITransactionTypesService transactionTypesservice)
        {
            _TransactionTypesservice = transactionTypesservice;
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllTransactionTypes")]

        public IActionResult GetAllTransactionTypes(string SearchText)
        {
            return Ok(_TransactionTypesservice.GetAllTransactionTypes(SearchText));
        }
        [HttpGet("FillTransactionTypesSelect")]

        public IActionResult FillTransactionTypesSelect()
        {
            return Ok(_TransactionTypesservice.GetAllTransactionTypes("").Result.Select(s => new {
                Id = s.TransactionTypeId,
                Name = s.NameAr
            }));
        }
        [HttpPost("SaveTransactionTypes")]

        public IActionResult SaveTransactionTypes(TransactionTypes transactionTypes)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _TransactionTypesservice.SaveTransactionTypes(transactionTypes, _globalshared.UserId_G, _globalshared. BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteTransactionTypes")]

        public IActionResult DeleteTransactionTypes(int TransactionTypesId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _TransactionTypesservice.DeleteTransactionTypes(TransactionTypesId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}

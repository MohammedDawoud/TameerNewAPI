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

    public class CurrencyController : ControllerBase
    {

        private ICurrencyService _currencyservice;
        private IBranchesService _branchesService;
        public GlobalShared _globalshared;
        public CurrencyController(ICurrencyService currencyservice, IBranchesService branchesService)
        {
            _currencyservice = currencyservice;
            _branchesService = branchesService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllCurrency")]
        public IActionResult GetAllCurrency()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_currencyservice.GetAllCurrency(_globalshared.BranchId_G));
        }
        [HttpGet("GetAllCurrency2")]
        public IActionResult GetAllCurrency2(string SearchText)
        {
            return Ok(_currencyservice.GetAllCurrency2(SearchText));
        }
        [HttpGet("FillCurrencySelect")]
        public IActionResult FillCurrencySelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
            var someCurrency = _currencyservice.GetAllCurrency(0).Result.Select(s => new
            {
                Id = s.CurrencyId,
                Name = s.CurrencyNameAr
            });
            foreach (var userBranch in userBranchs)
            {
                var AllCurrency = _currencyservice.GetAllCurrency(userBranch.BranchId).Result.Select(s => new
                {
                    Id = s.CurrencyId,
                    Name = s.CurrencyNameAr
                });
                var Currencies = someCurrency.Union(AllCurrency);
                someCurrency = Currencies.ToList();
            }
            return Ok(someCurrency);
        }
        [HttpPost("SaveCurrency")]
        public IActionResult SaveCurrency(Currency currency)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _currencyservice.SaveCurrency(currency, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteCurrency")]
        public IActionResult DeleteCurrency(int CurrencyId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _currencyservice.DeleteCurrency(CurrencyId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GenerateCurrencyNumber")]
        public IActionResult GenerateCurrencyNumber()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_currencyservice.GenerateCurrencyNumber(_globalshared.BranchId_G));

        }
    }
}

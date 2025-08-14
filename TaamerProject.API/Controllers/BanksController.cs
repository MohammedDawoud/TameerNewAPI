using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Require2FA")]

    public class BanksController : ControllerBase
    {
        private readonly IBanksService _banksservice;
        public GlobalShared _globalshared;

        public BanksController(IBanksService banksservice)
        {
            _banksservice = banksservice;
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllBanks")]

        public IActionResult GetAllBanks(string? SearchText)
        {
            return Ok(_banksservice.GetAllBanks(SearchText ?? ""));
        }
        [HttpGet("FillBankSelect")]

        public IActionResult FillBankSelect()
        {
            return Ok(_banksservice.FillBankSelect());
        }
        [HttpPost("Savebanks")]

        public IActionResult Savebanks(Banks banks)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _banksservice.SaveBanks(banks, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteBanks")]

        public IActionResult DeleteBanks(int BankId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _banksservice.DeleteBanks(BankId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}

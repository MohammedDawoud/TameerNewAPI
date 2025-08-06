using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class DiscountRewardController : ControllerBase
    {
        private IDiscountRewardService _discountRewardservice;
        public GlobalShared _globalshared;
        public DiscountRewardController(IDiscountRewardService discountRewardservice)
        {
            _discountRewardservice = discountRewardservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllDiscountRewards")]
        public IActionResult GetAllDiscountRewards(int? EmpId, string? SearchText)
        {
            return Ok(_discountRewardservice.GetAllDiscountRewards(EmpId, SearchText ?? ""));
        }
        [HttpPost("SaveDiscountReward")]
        public IActionResult SaveDiscountReward(DiscountReward discountReward)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _discountRewardservice.SaveDiscountReward(discountReward, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G);
            return Ok(result);
        }
        [HttpPost("DeleteDiscountReward")]
        public IActionResult DeleteDiscountReward(int DiscountRewardId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _discountRewardservice.DeleteDiscountReward(DiscountRewardId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}

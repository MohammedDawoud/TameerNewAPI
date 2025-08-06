using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class VoucherSettingsController : ControllerBase
    {
        private IVoucherSettingsService _VoucherSettingService;
        public GlobalShared _globalshared;
        public VoucherSettingsController(IVoucherSettingsService voucherSettingService)
        {
            _VoucherSettingService = voucherSettingService;
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllVoucherSettings")]

        public IActionResult GetAllVoucherSettings()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_VoucherSettingService.GetAllVoucherSettings(_globalshared.BranchId_G));
        }
        [HttpPost("SaveVoucherSettings")]

        public IActionResult SaveVoucherSettings(int Type, List<int> AccountIds)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _VoucherSettingService.SaveVoucherSettings(Type, AccountIds, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(new { result});
        }
        [HttpPost("DeleteVoucherSettings")]

        public IActionResult DeleteVoucherSettings(int SettingId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _VoucherSettingService.DeleteVoucherSettings(SettingId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(new { result});
        }
        [HttpGet("GetAccountIdsByType")]

        public IActionResult GetAccountIdsByType(int Type)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_VoucherSettingService.GetAccountIdsByType(Type, _globalshared.BranchId_G).Result);
        }
    }
}

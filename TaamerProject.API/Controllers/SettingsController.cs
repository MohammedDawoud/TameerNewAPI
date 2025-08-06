using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using System.Web.Script.Serialization;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class SettingsController : ControllerBase
    {
        private ISettingsService _settingsservice;
        string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        public SettingsController(ISettingsService settingsservice, IConfiguration _configuration)
        {
            _settingsservice = settingsservice;
            Configuration = _configuration;
            Con = this.Configuration.GetConnectionString("DBConnection");
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpPost("SaveSettings")]
        public IActionResult SaveSettings(Settings settings)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _settingsservice.SaveSettings(settings, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetSetting_Statment")]
        public IActionResult GetSetting_Statment(int UserId)
        {
            var SelectStetment = "select se.SettingId,se.DescriptionAr,det.ProSettingNo,ProSettingNote,se.UserId  from Pro_Settings se join Pro_ProSettingDetails det on se.ProjSubTypeId=det.ProjectSubtypeId where se.IsDeleted=0 and det.IsDeleted=0 and se.Type=3 and se.UserId=" + UserId + "";
            var result = _settingsservice.GetSetting_Statment(Con, SelectStetment).ToList();
            return Ok(result);

        }
        [HttpPost("ConvertUserSettingsSome")]
        public IActionResult ConvertUserSettingsSome([FromForm]string SettingId, [FromForm] string FromUserId, [FromForm] string ToUserId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _settingsservice.ConvertUserSettingsSome(Convert.ToInt32(SettingId), Convert.ToInt32(FromUserId), Convert.ToInt32(ToUserId), _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }


        [HttpPost("ConvertMoreUserSettings")]
        public IActionResult ConvertMoreUserSettings(List<int> SettingIds, int FromUserId, int ToUserId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _settingsservice.ConvertMoreUserSettings(SettingIds, Convert.ToInt32(FromUserId), Convert.ToInt32(ToUserId), _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}

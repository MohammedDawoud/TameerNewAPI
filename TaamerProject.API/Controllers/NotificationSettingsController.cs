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
    public class NotificationSettingsController : ControllerBase
    {

        private INotificationSettingsService _notificationSettingsservice;
        public GlobalShared _globalshared;
        public NotificationSettingsController(INotificationSettingsService notificationSettingsservice)
        {
            _notificationSettingsservice = notificationSettingsservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllNotificationSettings")]
        public IActionResult GetAllNotificationSettings()
        {
            return Ok(_notificationSettingsservice.GetAllNotificationSettings());
        }

        [HttpPost("UpdateNotificationSettings")]
        public IActionResult UpdateNotificationSettings(NotificationSettings notificationSettings)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _notificationSettingsservice.UpdateNotificationSettings(notificationSettings, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}

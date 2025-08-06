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

    public class AlertController : ControllerBase
    {
        private INotificationService _notificationservice;
        public GlobalShared _globalshared;
        public AlertController(INotificationService notificationservice)
        {
            _notificationservice = notificationservice;
            HttpContext httpContext = HttpContext;_globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllAlerts")]
        public IActionResult GetAllAlerts()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext); //alerts
            return Ok(_notificationservice.GetAllAlerts(_globalshared.BranchId_G));
        }
        [HttpGet("GetUserAlerts_Dashboard")]
        public IActionResult GetUserAlerts_Dashboard() //alertsdash
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _notificationservice.GetUserlAlerts_Dashboard(_globalshared.BranchId_G, _globalshared.UserId_G);
            return Ok(result);
        }
        [HttpPost("SaveAlert")]
        public IActionResult SaveAlert([FromBody]Notification alert)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _notificationservice.SaveAlert(alert, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("EndAlert")]
        public IActionResult EndAlert(int AlertId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _notificationservice.EndAlert(AlertId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteAlert")]
        public IActionResult DeleteAlert(int NotificationId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _notificationservice.DeleteAlert(NotificationId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }

        [HttpPost("HideAlert")]
        public IActionResult HideAlert(int NotificationId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _notificationservice.HideAlert(NotificationId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetUserAlert")]
        public IActionResult GetUserAlert()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_notificationservice.GetUserAlert(_globalshared.UserId_G));
        }
    }
}

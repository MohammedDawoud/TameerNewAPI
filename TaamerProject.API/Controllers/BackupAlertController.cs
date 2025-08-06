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

    public class BackupAlertController : ControllerBase
    {
        private readonly IBackupAlertService _backupAlertService;
        public GlobalShared _globalshared;

        public BackupAlertController(IBackupAlertService backupAlertService)
        {
            _backupAlertService = backupAlertService;
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllBackupAlert")]

        public IActionResult GetAllBackupAlert()
        {
            return Ok(_backupAlertService.GetAllBackupAlert());
        }
        [HttpPost("SaveBackupalert")]

        public IActionResult SaveBackupalert([FromBody]List<BackupAlert> notifications)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _backupAlertService.SaveBackupalert(notifications, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteBackupalert")]


        public IActionResult DeleteBackupalert(int AlertId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _backupAlertService.DeleteBackupalert(AlertId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}

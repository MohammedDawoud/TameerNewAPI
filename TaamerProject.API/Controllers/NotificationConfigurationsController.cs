using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationConfigurationsController : ControllerBase
    {
        private readonly INotificationConfigurationService _configurationService;
        public GlobalShared _globalshared;

        public NotificationConfigurationsController(INotificationConfigurationService configurationService)
        {
            _configurationService = configurationService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

        }

        [HttpGet("GetAllConfigurations")]
        public IActionResult GetAllConfigurations(string? SearchText)
        {
            return Ok(_configurationService.GetAll(SearchText));
        }
        [HttpPost("SaveConfigurations")]
        public IActionResult SaveConfigurations(NotificationConfiguration configuration)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _configurationService.Save(configuration, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }



    }
}

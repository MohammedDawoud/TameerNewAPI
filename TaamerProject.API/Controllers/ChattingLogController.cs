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

    public class ChattingLogController : ControllerBase
    {
        private IChattingLogService _chattingLogService;
        public GlobalShared _globalshared;
        public ChattingLogController(IChattingLogService chattingLogService)
        {
            _chattingLogService = chattingLogService;
            HttpContext httpContext = HttpContext;_globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllChattingLog")]
        public IActionResult GetAllChattingLog()
        {
            return Ok(_chattingLogService.GetAllChattingLog());
        }

    }
}

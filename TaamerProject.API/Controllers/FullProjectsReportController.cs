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

    public class FullProjectsReportController : ControllerBase
    {
        private readonly IFullProjectsReportService _fullProjectsReportService;
        public GlobalShared _globalshared;
        public FullProjectsReportController(IFullProjectsReportService fullProjectsReportService)
        {
            _fullProjectsReportService = fullProjectsReportService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllprojectsreport")]
        public IActionResult GetAllprojectsreport()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_fullProjectsReportService.GetAllprojectsreport(_globalshared.Lang_G));
        }
        [HttpGet("GetPhaseTaskData")]
        public IActionResult GetPhaseTaskData(int PhaseId)
        {
            return Ok(_fullProjectsReportService.GetPhaseTaskData(PhaseId));
        }
        [HttpGet("GetProjectDataRE")]
        public IActionResult GetProjectDataRE(int ProjectId)
        {
            return Ok(_fullProjectsReportService.GetProjectDataRE(ProjectId));
        }
    }
}

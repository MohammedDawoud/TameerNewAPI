using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using TaamerProject.API.Helper;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class VacationsAboutToStartController : ControllerBase
    {
        private readonly IVacationService _VacationService;
        string? Con;
        private IConfiguration Configuration;

        public GlobalShared _globalshared;
        public VacationsAboutToStartController(IVacationService vacationService, IConfiguration _configuration)
        {
            _VacationService = vacationService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");

        }
        [HttpGet("GetVacationsAboutToStart")]
        public IActionResult GetVacationsAboutToStart()
        {
            var result = _VacationService.GetVacationsAboutToStart(Con??"");
            return Ok(result);
        }
    }
}

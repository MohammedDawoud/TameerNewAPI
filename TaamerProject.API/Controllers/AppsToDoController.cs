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

    public class AppsToDoController : ControllerBase
    {
        private readonly IProjectService _projectservice;
        public GlobalShared _globalshared;
        public AppsToDoController(IProjectService projectservice)
        {
            _projectservice = projectservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllHirearchialProject")]
        public IActionResult GetAllHirearchialProject()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_projectservice.GetAllHirearchialProject(_globalshared.BranchId_G, _globalshared.UserId_G));
        }
    }
}

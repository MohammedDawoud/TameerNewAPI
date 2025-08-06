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

    public class DeservedServicesController : ControllerBase
    {
        private readonly IServiceService _Serv;
        string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        public DeservedServicesController(IServiceService serv, IConfiguration _configuration)
        {
            _Serv = serv;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");
        }
        [HttpGet("GetDeservedServices")]
        public IActionResult GetDeservedServices()
        {
            var result = _Serv.GetDeservedServices(Con??"");
            return Ok(result);
        }
    }
}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class EmpLoansController : ControllerBase
    {
        private readonly IEmployeesService _Serv;
        string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        public EmpLoansController(IEmployeesService serv, IConfiguration _configuration)
        {
            _Serv = serv;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");

        }
        [HttpGet("GetEmpLoans")]
        public IActionResult GetEmpLoans()
        {
            var result = _Serv.GetEmpLoans(Con??"");
            return Ok(result);
        }
    }
}

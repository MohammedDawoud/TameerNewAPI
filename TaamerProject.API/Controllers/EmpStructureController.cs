using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Require2FA")]

    public class EmpStructureController : ControllerBase
    {
        private IEmpStructureService _empStructureservice;
        public GlobalShared _globalshared;
        public EmpStructureController(IEmpStructureService empStructureservice)
        {
            _empStructureservice = empStructureservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllNodesEmps")]
        public IActionResult GetAllNodesEmps()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _empStructureservice.GetAllNodesEmps(_globalshared.Lang_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}

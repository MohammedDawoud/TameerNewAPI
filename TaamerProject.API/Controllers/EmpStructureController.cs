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

    public class EmpStructureController : ControllerBase
    {
        private IEmpStructureService _empStructureservice;
        public GlobalShared _globalshared;
        public EmpStructureController(IEmpStructureService empStructureservice)
        {
            _empStructureservice = empStructureservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        //[HttpPost("SaveEmpStructure")]
        //public IActionResult SaveEmpStructure(List<EmpStructure> EmpLinkList, List<NodeLocations> NodeLocList)
        //{
        //    HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        //    var result = _empStructureservice.SaveEmpStructure(EmpLinkList, NodeLocList, _globalshared.UserId_G, _globalshared.BranchId_G);
        //    return Ok(result);
        //}
        [HttpGet("GetAllNodesEmps")]
        public IActionResult GetAllNodesEmps()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _empStructureservice.GetAllNodesEmps(_globalshared.Lang_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}

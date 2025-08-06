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

    public class GuranteesController : ControllerBase
    {
        private IGuranteesService _GuranteesService;
        private readonly IFiscalyearsService _FiscalyearsService;
        public GlobalShared _globalshared;
        public GuranteesController(IGuranteesService guranteesService, IFiscalyearsService fiscalyearsService)
        {
            _GuranteesService = guranteesService;
            _FiscalyearsService = fiscalyearsService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllGurantees")]
        public IActionResult GetAllGurantees()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_GuranteesService.GetAllGurantees(_globalshared.BranchId_G));
        }
        [HttpPost("SaveGurantee")]
        public IActionResult SaveGurantee([FromForm] Guarantees guarantees)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _GuranteesService.SaveGurantee(guarantees, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G);
            return Ok(result);
        }
        [HttpPost("DeleteGurantee")]
        public IActionResult DeleteGurantee(int GuranteeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _GuranteesService.DeleteGurantee(GuranteeId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}

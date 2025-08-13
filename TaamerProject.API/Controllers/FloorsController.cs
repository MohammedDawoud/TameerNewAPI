using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Require2FA")]

    public class FloorsController : ControllerBase
    {
        private IAcc_FloorsService _Floorsservice;
        public GlobalShared _globalshared;
        public FloorsController(IAcc_FloorsService floorsservice)
        {
            _Floorsservice = floorsservice;
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllFloors")]

        public IActionResult GetAllFloors(string? SearchText)
        {
            return Ok(_Floorsservice.GetAllFloors(SearchText??""));
        }
        [HttpPost("SaveFloor")]

        public IActionResult SaveFloor(Acc_Floors Floors)
        {
            HttpContext httpContext = HttpContext;_globalshared = new GlobalShared(httpContext);
            var result = _Floorsservice.SaveFloor(Floors, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(new { result });
        }
        [HttpPost("DeleteFloor")]

        public IActionResult DeleteFloor(int FloorsId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Floorsservice.DeleteFloor(FloorsId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(new { result });
        }
    }
}

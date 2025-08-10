using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class AttendenceLocationSettingsController : ControllerBase
    {
        private readonly IAttendenceLocationSettingsService _attendenceLocationSettings;
            public GlobalShared _globalshared;

            public AttendenceLocationSettingsController(IAttendenceLocationSettingsService attendenceLocationSettings)
            {
            _attendenceLocationSettings=attendenceLocationSettings;
                HttpContext httpContext = HttpContext;
                _globalshared = new GlobalShared(httpContext);
            }



        [HttpGet("GetAllAttendencelocations")]

        public IActionResult GetAllAttendencelocations(string? SearchText)
        {
            return Ok(_attendenceLocationSettings.GetAllAttendencelocations(SearchText ?? ""));
        }
        [HttpGet("GetlAttendencelocationbyId")]

        public IActionResult GetlAttendencelocationbyId(int attendenceLocationSettingsId)
        {
            return Ok(_attendenceLocationSettings.GetlAttendencelocationbyId(attendenceLocationSettingsId));
        }

        [HttpGet("FillAttendenceLocation")]

        public IActionResult FillAttendenceLocation()
        {
            return Ok(_attendenceLocationSettings.FillAttendenceLocation());
        }
        [HttpPost("SaveAttendenceLocation")]

        public IActionResult SaveAttendenceLocation(AttendenceLocationSettings attLocations)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendenceLocationSettings.SaveAttendenceLocation(attLocations, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteAttendenceLocation")]

        public IActionResult DeleteAttendenceLocation(int AttLocationId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendenceLocationSettings.DeleteAttendenceLocation(AttLocationId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }

    }
}

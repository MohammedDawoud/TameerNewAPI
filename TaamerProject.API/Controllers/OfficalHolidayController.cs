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

    public class OfficalHolidayController : ControllerBase
    {
        private IOfficalHolidayService _OfficalHolidayService;
        public GlobalShared _globalshared;
        public OfficalHolidayController(IOfficalHolidayService officalHolidayService)
        {
            _OfficalHolidayService = officalHolidayService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllOfficalHolidaySearch")]
        public IActionResult GetAllOfficalHolidaySearch(OfficalHolidayVM Search)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_OfficalHolidayService.GetAllOfficalHolidaySearch(Search, _globalshared.BranchId_G));
        }
        [HttpGet("GetAllOfficalHoliday")]
        public IActionResult GetAllOfficalHoliday()
        {
            return Ok(_OfficalHolidayService.GetAllOfficalHoliday());
        }
        [HttpPost("SaveOfficalHoliday")]
        public IActionResult SaveOfficalHoliday(OfficalHoliday Data)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _OfficalHolidayService.SaveOfficalHoliday(Data, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteOfficalHoliday")]
        public IActionResult DeleteOfficalHoliday(int Id)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _OfficalHolidayService.DeleteOfficalHoliday(Id, _globalshared.UserId_G, _globalshared. BranchId_G);
            return Ok(result);
        }
    }
}

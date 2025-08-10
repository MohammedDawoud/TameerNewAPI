using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class AttendaceTimeController : ControllerBase
    {
        private IAttendaceTimeService _attendaceTimeservice;
        private IAttTimeDetailsService _attTimeDetailsservice;
        public GlobalShared _globalshared;
        public AttendaceTimeController(IAttendaceTimeService attendaceTimeservice, IAttTimeDetailsService attTimeDetailsservice)
        {
            _attendaceTimeservice = attendaceTimeservice;
            _attTimeDetailsservice = attTimeDetailsservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllAttendaceTime")]
        public IActionResult GetAllAttendaceTime()
        {
            return Ok(_attendaceTimeservice.GetAllAttendaceTime());
        }
        [HttpGet("GetAllAttTimeDetailsScreen")]
        public IActionResult GetAllAttTimeDetailsScreen()
        {
            return Ok(_attTimeDetailsservice.GetAllAttTimeDetails());
        }
        [HttpGet("GetAllAttTimeDetailsScreenByid")]
        public IActionResult GetAllAttTimeDetailsByid(int AttTimeId)
        {
            return Ok(_attTimeDetailsservice.GetAllAttTimeDetailsByid(AttTimeId).Result);
        }
        [HttpGet("GetAllAttTimeDetails")]
        public IActionResult GetAllAttTimeDetails(string SearchText, int AttTimeId)
        {
            return Ok(_attTimeDetailsservice.GetAllAttTimeDetails(SearchText, AttTimeId));
        }
        [HttpGet("GetAllAttTimeDetails2")]
        public IActionResult GetAllAttTimeDetails2(int AttTimeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_attTimeDetailsservice.GetAllAttTimeDetails2(AttTimeId, _globalshared.BranchId_G));
        }
        [HttpGet("CheckUserPerDawamUserExist")]
        public IActionResult CheckUserPerDawamUserExist(int UserId, string TimeFrom, string TimeTo, int DayNo)
        {
            return Ok(_attTimeDetailsservice.CheckUserPerDawamUserExist(UserId, TimeFrom, TimeTo, DayNo));
        }

        [HttpGet("CalculateTaskHoursForEmployee")]
        public IActionResult CalculateTaskHoursForEmployee(int UserId, DateTime TimeFrom, DateTime TimeTo)
        {
            try
            {
                DateTime localTimeFrom = TimeFrom.ToLocalTime();
                DateTime localTimeTo = TimeTo.ToLocalTime();
                return Ok(_attTimeDetailsservice.CalculateTaskHoursForEmployee(UserId, localTimeFrom, localTimeTo));
            }
            catch (FormatException ex)
            {
                return BadRequest($"Invalid date format: {ex.Message}. Expected format: 'yyyy-MM-dd h:mm tt'.");
            }
        }

        [HttpPost("SaveAttendaceTime")]
        public IActionResult SaveAttendaceTime(AttendaceTime attendaceTime)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendaceTimeservice.SaveAttendaceTime(attendaceTime, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G);
            return Ok(result);
        }
        [HttpPost("SaveAttTimeDetails")]
        public IActionResult SaveAttTimeDetails(AttTimeDetails attTimeDetails)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attTimeDetailsservice.SaveAttTimeDetails(attTimeDetails, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G);
            return Ok(result);
        }
        [HttpPost("DeleteAttTimeDetails")]
        public IActionResult DeleteAttTimeDetails(int TimeDetailsId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attTimeDetailsservice.DeleteAttTimeDetails(TimeDetailsId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteAttendaceTime")]
        public IActionResult DeleteAttendaceTime(int TimeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendaceTimeservice.DeleteAttendaceTime(TimeId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GenerateNextAttendaceTimeNumber")]
        public IActionResult GenerateNextAttendaceTimeNumber()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_attendaceTimeservice.GenerateNextAttendaceTimeNumber(_globalshared.BranchId_G));
        }
        [HttpGet("FillAttendanceTimeSelect")]
        public IActionResult FillAttendanceTimeSelect()
        {
            return Ok(_attendaceTimeservice.GetAllAttendaceTime().Result.Select(s => new {
                Id = s.TimeId,
                Name = s.NameAr
            }));
        }
    }
}

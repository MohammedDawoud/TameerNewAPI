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

    public class InstrumentsController : ControllerBase
    {
        private IInstrumentsService _Instrumentsservice;
        public GlobalShared _globalshared;
        public InstrumentsController(IInstrumentsService instrumentsservice)
        {
            _Instrumentsservice = instrumentsservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllInstruments")]
        public IActionResult GetAllInstruments(int ProjectId)
        {
            return Ok(_Instrumentsservice.GetAllInstruments(ProjectId));
        }
        [HttpGet("FillInstrumentsSelect")]
        public IActionResult FillInstrumentsSelect(int ProjectId)
        {
            return Ok(_Instrumentsservice.GetAllInstruments(ProjectId).Result.Select(s => new {
                Id = s.InstrumentId,
                Name = s.InstrumentNo
            }));
        }
        [HttpPost("SaveInstruments")]
        public IActionResult SaveInstruments(List<Instruments> instruments)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Instrumentsservice.SaveInstruments(instruments, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveInstrument")]
        public IActionResult SaveInstrument(Instruments instrument)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Instrumentsservice.SaveInstrument(instrument, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteInstruments")]
        public IActionResult DeleteInstruments(int instrumentId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Instrumentsservice.DeleteInstruments(instrumentId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}

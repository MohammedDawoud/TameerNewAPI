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

    public class InstrumentSourcesController : ControllerBase
    {
        private IInstrumentSourcesService _InstrumentSourcesservice;
        public GlobalShared _globalshared;
        public InstrumentSourcesController(IInstrumentSourcesService instrumentSourcesservice)
        {
            _InstrumentSourcesservice = instrumentSourcesservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllInstrumentSources")]
        public IActionResult GetAllInstrumentSources(string? SearchText)
        {
            return Ok(_InstrumentSourcesservice.GetAllInstrumentSources(SearchText?? ""));
        }
        [HttpGet("FillInstrumentSourcesSelect")]
        public IActionResult FillInstrumentSourcesSelect(string? SearchText)
        {
            return Ok(_InstrumentSourcesservice.GetAllInstrumentSources(SearchText ?? "").Result.Select(s => new {
                Id = s.SourceId,
                Name = s.NameAr,
                NameEn = s.NameEn,
            }));
        }
        [HttpPost("SaveInstrumentSources")]
        public IActionResult SaveInstrumentSources(InstrumentSources instrumentSources)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _InstrumentSourcesservice.SaveInstrumentSources(instrumentSources, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteInstrumentSources")]
        public IActionResult DeleteInstrumentSources(int instrumentSourcesId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _InstrumentSourcesservice.DeleteInstrumentSources(instrumentSourcesId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}

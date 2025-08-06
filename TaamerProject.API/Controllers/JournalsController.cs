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

    public class JournalsController : ControllerBase
    {
        private IJournalsService _journalsservice;
        public GlobalShared _globalshared;
        public JournalsController(IJournalsService journalsservice)
        {
            _journalsservice = journalsservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllJournals")]
        public IActionResult GetAllJournals()
        {
            return Ok(_journalsservice.GetAllJournals());
        }
        [HttpPost("SaveJournals")]
        public IActionResult SaveJournals(Journals journals)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _journalsservice.SaveJournals(journals, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteJournals")]
        public IActionResult DeleteJournals(int JournalId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _journalsservice.DeleteJournals(JournalId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}

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

    public class TotalSpacesRangeController : ControllerBase
    {
        private IAcc_TotalSpacesRangeService _TotalSpacesRangeservice;
        public GlobalShared _globalshared;
        public TotalSpacesRangeController(IAcc_TotalSpacesRangeService totalSpacesRangeservice)
        {
            _TotalSpacesRangeservice = totalSpacesRangeservice;
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllTotalSpacesRange")]

        public IActionResult GetAllTotalSpacesRange(string? SearchText)
        {
            return Ok(_TotalSpacesRangeservice.GetAllTotalSpacesRange(SearchText??""));
        }
        [HttpPost("SaveTotalSpacesRange")]

        public IActionResult SaveTotalSpacesRange(Acc_TotalSpacesRange TotalSpacesRange)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _TotalSpacesRangeservice.SaveTotalSpacesRange(TotalSpacesRange, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteTotalSpacesRange")]

        public IActionResult DeleteTotalSpacesRange(int TotalSpacesRangeId)
        {
            var result = _TotalSpacesRangeservice.DeleteTotalSpacesRange(TotalSpacesRangeId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}

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

    public class NationalityController : ControllerBase
    {
        private INationalityService _nationalityservice;
        public GlobalShared _globalshared;
        public NationalityController(INationalityService nationalityservice)
        {
            _nationalityservice = nationalityservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllNationalities")]
        public IActionResult GetAllNationalities(string SearchText)
        {
            return Ok(_nationalityservice.GetAllNationalities(SearchText));
        }

        [HttpGet("GetAllNationalities2")]
        public IActionResult GetAllNationalities2()
        {
            return Ok(_nationalityservice.GetAllNationalities(""));
        }
        [HttpPost("SaveNationality")]
        public IActionResult SaveNationality(Nationality nationality)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _nationalityservice.SaveNationality(nationality, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteNationality")]
        public IActionResult DeleteNationality(int NationalityId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _nationalityservice.DeleteNationality(NationalityId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillNationalitySelect")]
        public IActionResult FillNationalitySelect()
        {
            return Ok(_nationalityservice.FillNationalitySelect());
        }
    }
}

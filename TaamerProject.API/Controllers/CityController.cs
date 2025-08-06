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

    public class CityController : ControllerBase
    {
        private ICityService _cityservice;
        public GlobalShared _globalshared;
        public CityController(ICityService cityservice)
        {
            _cityservice = cityservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllCities")]
        public IActionResult GetAllCities(string SearchText)
        {
            return Ok(_cityservice.GetAllCities(SearchText));
        }

        [HttpGet("GetAllCities2")]
        public IActionResult GetAllCities2()
        {
            return Ok(_cityservice.GetAllCities(""));
        }
        [HttpPost("SaveCity")]
        public IActionResult SaveCity([FromBody]City city)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _cityservice.SaveCity(city, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteCity")]
        public IActionResult DeleteCity(int CityId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _cityservice.DeleteCity(CityId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillCitySelect")]
        public IActionResult FillCitySelect()
        {
            return Ok(_cityservice.GetAllCities("").Result.Select(s => new {
                Id = s.CityId,
                Name = s.NameAr,
                NameEn=s.NameEn,
            }));
        }
        [HttpGet("GetCityById")]
        public IActionResult GetCityById(int CityId)
        {
            return Ok(_cityservice.GetCityById(CityId));
        }
    }
}

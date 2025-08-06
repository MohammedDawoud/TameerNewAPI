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

    public class CityPassController : ControllerBase
    {
        private ICityPassService _cityservice;
        public GlobalShared _globalshared;
        public CityPassController(ICityPassService cityservice)
        {
            _cityservice = cityservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllCities")]
        public IActionResult GetAllCities(string SearchText)
        {
            return Ok(_cityservice.GetAllCities(SearchText));
        }
        [HttpPost("SaveCity")]
        public IActionResult SaveCity(CityPass city)
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
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            if (_globalshared.Lang_G == "ltr")
            {
                return Ok(_cityservice.GetAllCities("").Result.Select(s => new {
                    Id = s.CityId,
                    Name = s.NameEn
                }));
            }
            else
            {
                return Ok(_cityservice.GetAllCities("").Result.Select(s => new {
                    Id = s.CityId,
                    Name = s.NameAr
                }));
            }

        }
    }
}

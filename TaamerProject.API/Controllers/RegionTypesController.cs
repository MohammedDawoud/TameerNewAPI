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

    public class RegionTypesController : ControllerBase
    {
        private IRegionTypesService _RegionTypesservice;
        public GlobalShared _globalshared;
        public RegionTypesController(IRegionTypesService regionTypesservice)
        {
            _RegionTypesservice = regionTypesservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllRegionTypes")]
        public IActionResult GetAllRegionTypes(string? SearchText)
        {
            return Ok(_RegionTypesservice.GetAllRegionTypes(SearchText ?? ""));
        }
        [HttpGet("FillRegionTypesSelect")]
        public IActionResult FillRegionTypesSelect()
        {
            return Ok(_RegionTypesservice.GetAllRegionTypes("").Result.Select(s => new {
                Id = s.RegionTypeId,
                Name = s.NameAr,
                NameEn = s.NameEn
            }));
        }
        [HttpPost("SaveRegionTypes")]
        public IActionResult SaveRegionTypes(RegionTypes regionTypes)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _RegionTypesservice.SaveRegionTypes(regionTypes, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteRegionTypes")]
        public IActionResult DeleteRegionTypes(int RegionTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _RegionTypesservice.DeleteRegionTypes(RegionTypeId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}

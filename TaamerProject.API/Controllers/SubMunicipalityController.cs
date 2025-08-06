using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SubMunicipalityController : ControllerBase
    {
        private IPro_SubMunicipalityService _Pro_SubMunicipalityService;
        public GlobalShared _globalshared;
        public SubMunicipalityController(IPro_SubMunicipalityService pro_SubMunicipalityService)
        {
            _Pro_SubMunicipalityService = pro_SubMunicipalityService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllSubMunicipalitys")]
        public IActionResult GetAllSubMunicipalitys(string? SearchText)
        {
            return Ok(_Pro_SubMunicipalityService.GetAllSubMunicipalitys(SearchText ?? ""));
        }
        [HttpPost("SaveSubMunicipality")]
        public IActionResult SaveSubMunicipality(Pro_SubMunicipality SubMunicipality)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_SubMunicipalityService.SaveSubMunicipality(SubMunicipality, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteSubMunicipality")]
        public IActionResult DeleteSubMunicipality(int SubMunicipalityId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_SubMunicipalityService.DeleteSubMunicipality(SubMunicipalityId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillSubMunicipalitysSelect")]
        public IActionResult FillSubMunicipalitysSelect(string? SearchText)
        {
            return Ok(_Pro_SubMunicipalityService.GetAllSubMunicipalitys(SearchText ?? "").Result.Select(s => new {
                Id = s.SubMunicipalityId,
                Name = s.NameAr,
                NameEn = s.NameEn,
            }));
        }
    }
}

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

    public class MunicipalController : ControllerBase
    {
        private IPro_MunicipalService _Pro_MunicipalService;
        public GlobalShared _globalshared;
        public MunicipalController(IPro_MunicipalService pro_MunicipalService)
        {
            _Pro_MunicipalService = pro_MunicipalService;
            HttpContext httpContext = HttpContext;_globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllMunicipals")]
        public IActionResult GetAllMunicipals(string? SearchText)
        {
            return Ok(_Pro_MunicipalService.GetAllMunicipals(SearchText ?? ""));
        }
        [HttpPost("SaveMunicipal")]
        public IActionResult SaveMunicipal(Pro_Municipal Municipal)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_MunicipalService.SaveMunicipal(Municipal, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteMunicipal")]
        public IActionResult DeleteMunicipal(int MunicipalId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_MunicipalService.DeleteMunicipal(MunicipalId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillMunicipalsSelect")]
        public IActionResult FillMunicipalsSelect(string? SearchText)
        {
            return Ok(_Pro_MunicipalService.GetAllMunicipals(SearchText ?? "").Result.Select(s => new {
                Id = s.MunicipalId,
                Name = s.NameAr,
                NameEn = s.NameEn,
            }));
        }
    }
}

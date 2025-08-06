using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ProjectSubTypesController : ControllerBase
    {
        private IProjectSubTypesService _proSubTypesservice;
        public GlobalShared _globalshared;
        public ProjectSubTypesController(IProjectSubTypesService proSubTypesservice)
        {
            _proSubTypesservice = proSubTypesservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllProjectSubType")]
        public IActionResult GetAllProjectSubType()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_proSubTypesservice.GetAllProjectSubType(_globalshared.BranchId_G));
        }
        [HttpPost("SaveProjectSubType")]
        public IActionResult SaveProjectSubType(ProjectSubTypes subType)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _proSubTypesservice.SaveProjectSubTypes(subType, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteProjectSubTypes")]
        public IActionResult DeleteProjectSubTypes(int SubTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _proSubTypesservice.DeleteSubTypes(SubTypeId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetTimePeriordBySubTypeId")]
        public IActionResult GetTimePeriordBySubTypeId(int SubTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var SubType = _proSubTypesservice.GetTimePeriordBySubTypeId(SubTypeId);
            return Ok(SubType);
        }
        [HttpGet("FillProjectSubTypesSelect")]
        public IActionResult FillProjectSubTypesSelect(int param, string? SearchText)
        {
            var Subtype = _proSubTypesservice.GetAllProjectSubsByProjectTypeId(param, SearchText??"", 0).Result.Where(s => s.ProjectTypeId == param).Select(s => new {
                Id = s.SubTypeId,
                Name = s.NameAr,
                NameEn=s.NameEn,
                TimePeriodStr=s.TimePeriodStr,
                TimePeriod = s.TimePeriod,
            });
            return Ok(Subtype); 
        }
        [HttpGet("GetAllProjectSubsByProjectTypeId")]
        public IActionResult GetAllProjectSubsByProjectTypeId(int ProjectTypeId, string? SearchText)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AllSubType = _proSubTypesservice.GetAllProjectSubsByProjectTypeId(ProjectTypeId, SearchText ?? "", _globalshared.BranchId_G).Result
                ;
            return Ok(AllSubType);
        }
    }
}

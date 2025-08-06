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

    public class BuildTypesController : ControllerBase
    {
        private readonly IBuildTypesService _buildTypeservice;
        public GlobalShared _globalshared;

        public BuildTypesController(IBuildTypesService buildTypeservice)
        {
            _buildTypeservice = buildTypeservice;
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllBuildTypes")]

        public IActionResult GetAllBuildTypes(string SearchText)
        {
            return Ok(_buildTypeservice.GetAllBuildTypes(SearchText));
        }
        [HttpPost("SaveBuildTypes")]

        public IActionResult SaveBuildTypes(BuildTypes build)
        {
            var result = _buildTypeservice.SaveBuildType(build, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteBuildTypes")]

        public IActionResult DeleteBuildTypes(int BuildTypesId)
        {
            var result = _buildTypeservice.DeleteBuildTypes(BuildTypesId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillBuildTypeSelect")]

        public IActionResult FillBuildTypeSelect()
        {
            return Ok(_buildTypeservice.GetAllBuildTypes("").Result.Select(s => new {
                Id = s.BuildTypeId,
                Name = s.NameAr,
                NameEn= s.NameEn,
            }));
        }
    }
}

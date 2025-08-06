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

    public class ProjectTrailingController : ControllerBase
    {
        private IProjectTrailingService _ProjectTrailingservice;
        public GlobalShared _globalshared;
        public ProjectTrailingController(IProjectTrailingService projectTrailingservice)
        {
            _ProjectTrailingservice = projectTrailingservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetProjectTrailingInOfficeArea")]
        public IActionResult GetProjectTrailingInOfficeArea()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_ProjectTrailingservice.GetProjectTrailingInOfficeArea(_globalshared.BranchId_G));
        }
        [HttpGet("GetProjectTrailingInExternalSide")]
        public IActionResult GetProjectTrailingInExternalSide()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_ProjectTrailingservice.GetProjectTrailingInExternalSide(_globalshared.BranchId_G));
        }
        [HttpPost("SaveProjectTrailing")]
        public IActionResult SaveProjectTrailing(ProjectTrailing ProjectTrailing)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProjectTrailingservice.SaveProjectTrailing(ProjectTrailing, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G);
            return Ok(result);
        }
        [HttpPost("DeleteProjectTrailing")]
        public IActionResult DeleteProjectTrailing(int TrailingId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProjectTrailingservice.DeleteProjectTrailing(TrailingId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetTrailingFilesByTrailingId")]
        public IActionResult GetTrailingFilesByTrailingId(int? TrailingId, string SearchText)
        {
            return Ok(_ProjectTrailingservice.GetTrailingFilesByTrailingId(TrailingId, SearchText ?? ""));
        }
        [HttpPost("ReceiveProjectTrailing")]
        public IActionResult ReceiveProjectTrailing(ProjectTrailing ProjectTrailing)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProjectTrailingservice.ReceiveProjectTrailing(ProjectTrailing, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("RejectProjectTrailing")]
        public IActionResult RejectProjectTrailing(ProjectTrailing ProjectTrailing)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProjectTrailingservice.RejectProjectTrailing(ProjectTrailing, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("AcceptProjectTrailing")]
        public IActionResult AcceptProjectTrailing(int? TrailingId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProjectTrailingservice.AcceptProjectTrailing(TrailingId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}

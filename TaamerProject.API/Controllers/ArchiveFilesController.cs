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

    public class ArchiveFilesController : ControllerBase
    {
        private IArchiveFilesService _ArchiveFilesservice;
        public GlobalShared _globalshared;

        public ArchiveFilesController(IArchiveFilesService archiveFilesService)
        {
            _ArchiveFilesservice = archiveFilesService;
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllArchiveFiles")]

        public IActionResult GetAllArchiveFiles(string? SearchText)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_ArchiveFilesservice.GetAllArchiveFiles(SearchText ?? "", _globalshared.BranchId_G));
        }
        [HttpPost("SaveArchiveFiles")]

        public IActionResult SaveArchiveFiles(ArchiveFiles ArchiveFiles)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ArchiveFilesservice.SaveArchiveFiles(ArchiveFiles, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteArchiveFiles")]

        public IActionResult DeleteArchiveFiles(int ArchiveFileId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ArchiveFilesservice.DeleteArchiveFiles(ArchiveFileId, _globalshared.UserId_G, _globalshared. BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillArchiveFilesSelect")]

        public IActionResult FillArchiveFilesSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_ArchiveFilesservice.FillArchiveFilesSelect(_globalshared.BranchId_G));
        }
    }
}

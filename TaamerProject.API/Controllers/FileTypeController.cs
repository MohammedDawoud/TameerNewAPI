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

    public class FileTypeController : ControllerBase
    {
        private IFileTypeService _fileTypeservice;
        public GlobalShared _globalshared;
        public FileTypeController(IFileTypeService fileTypeservice)
        {
            _fileTypeservice = fileTypeservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllFileTypes")]
        public IActionResult GetAllFileTypes(string? SearchText)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_fileTypeservice.GetAllFileTypes(SearchText ?? "", _globalshared.BranchId_G));
        }
        [HttpPost("SaveFileType")]
        public IActionResult SaveFileType(FileType fileType)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _fileTypeservice.SaveFileType(fileType, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteFileType")]
        public IActionResult DeleteFileType(int FileTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _fileTypeservice.DeleteFileType(FileTypeId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillFileTypeSelect")]
        public IActionResult FillFileTypeSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_fileTypeservice.FillFileTypeSelect(_globalshared.BranchId_G));
        }
    }
}

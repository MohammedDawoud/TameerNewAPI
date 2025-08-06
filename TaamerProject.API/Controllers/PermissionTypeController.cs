using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionTypeController : ControllerBase
    {
        private readonly IPermissionTypeService _permissionTypeService;
        public GlobalShared _globalshared;
        public PermissionTypeController(IPermissionTypeService permissionTypeService)
        {
            _permissionTypeService = permissionTypeService;
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllPermissionTypes")]
        public IActionResult GetAllPermissionTypes(string SearchText)
        {
            return Ok(_permissionTypeService.GetAllPermissionTypes(SearchText));
        }

        [HttpGet("GetAllPermissionTypes2")]
        public IActionResult GetAllPermissionTypes2()
        {
            return Ok(_permissionTypeService.GetAllPermissionTypes(""));
        }

        [HttpPost("SavePermissionType")]
        public IActionResult SavePermissionType(PermissionType permissionType)
        {
            var result = _permissionTypeService.SavePermissionType(permissionType, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeletePermissionType")]
        public IActionResult DeletePermissionType(int permtype)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _permissionTypeService.DeletePermissionType(permtype, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillPermissionTypeSelect")]

        public IActionResult FillPermissionTypeSelect()
        {
            var perm = _permissionTypeService.FillPermissionTypeSelect().Result.Select(s => new
            {
                Id = s.TypeId,
                Name = s.NameAr
            });
            return Ok(perm);
        }
    }
}
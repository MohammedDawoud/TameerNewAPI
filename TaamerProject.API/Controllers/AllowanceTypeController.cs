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

    public class AllowanceTypeController : ControllerBase
    {
        private IAllowanceTypeService _AllowanceTypeservice;
        public GlobalShared _globalshared;

        public AllowanceTypeController(IAllowanceTypeService allowanceTypeservice)
        {
            _AllowanceTypeservice = allowanceTypeservice;
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllAllowancesTypes")]

        public IActionResult GetAllAllowancesTypes(string? SearchText)
        {
            return Ok(_AllowanceTypeservice.GetAllAllowancesTypes(SearchText??"", false));
        }
        [HttpPost("SaveAllowanceType")]

        public IActionResult SaveAllowanceType(AllowanceType allowanceType)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _AllowanceTypeservice.SaveAllowanceType(allowanceType, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteAllowanceType")]

        public IActionResult DeleteAllowanceType(int AllowanceTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _AllowanceTypeservice.DeleteAllowanceType(AllowanceTypeId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillAllowanceTypeSelect")]

        public IActionResult FillAllowanceTypeSelect()
        {
            return Ok(_AllowanceTypeservice.FillAllowanceTypeSelect());
        }
    }
}

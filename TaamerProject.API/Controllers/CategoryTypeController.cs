using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Security.Claims;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class CategoryTypeController : ControllerBase
    {
        private readonly IAcc_CategorTypeService _Acc_CategorTypeService;
        public GlobalShared _globalshared;
        public CategoryTypeController(IAcc_CategorTypeService acc_CategorTypeService)
        {
            _Acc_CategorTypeService = acc_CategorTypeService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllCategoryType")]

        public IActionResult GetAllCategoryType(string SearchText = "")
        {
            var cate = _Acc_CategorTypeService.GetAllCategorytype(SearchText);
            return cate == null ? NotFound() : Ok(cate);
        }
        [HttpPost("SaveCategoryType")]

        public IActionResult SaveCategoryType(Acc_CategorType category)
        {
            HttpContext httpContext = HttpContext;_globalshared = new GlobalShared(httpContext);
            var result = _Acc_CategorTypeService.SaveCategory(category, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteCategoryType")]

        public IActionResult DeleteCategoryType(int Categoryid)
        {
            HttpContext httpContext = HttpContext;_globalshared = new GlobalShared(httpContext);
            var result = _Acc_CategorTypeService.DeleteCategory(Categoryid, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FilltAllCategoryType")]

        public IActionResult FilltAllCategoryType()
        {
            var cate = _Acc_CategorTypeService.GetAllCategorytype("").Result.Select(s => new
            {
                Id = s.CategorTypeId,
                Name = s.NAmeAr
            });
            return Ok(cate);
        }
    }
}

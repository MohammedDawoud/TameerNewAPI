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

    public class CategoriesController : ControllerBase
    {
        private readonly IAcc_CategoriesService _Acc_CategoriesService;
        public GlobalShared _globalshared;

        public CategoriesController(IAcc_CategoriesService acc_CategoriesService)
        {
            _Acc_CategoriesService = acc_CategoriesService;
            HttpContext httpContext = HttpContext;_globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllCategory")]

        public IActionResult GetAllCategory(string SearchText = "")
        {
            var cate = _Acc_CategoriesService.GetAllCategories(SearchText);
            return cate == null ? NotFound() : Ok(cate);
        }
        [HttpPost("SaveCategory")]

        public IActionResult SaveCategory(Acc_Categories Category)
        {
            HttpContext httpContext = HttpContext;_globalshared = new GlobalShared(httpContext);
            var result = _Acc_CategoriesService.SaveCategory(Category, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteCategory")]

        public IActionResult DeleteCategory(int Categoryid)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Acc_CategoriesService.DeleteCategory(Categoryid, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }

    }
}

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

    public class ItemTypeController : ControllerBase
    {
        private IItemTypeService _ItemTypeservice;
        public GlobalShared _globalshared;
        public ItemTypeController(IItemTypeService itemTypeservice)
        {
            _ItemTypeservice = itemTypeservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllItemTypes")]
        public IActionResult GetAllItemTypes(string? SearchText)
        {
            return Ok(_ItemTypeservice.GetAllItemTypes(SearchText??""));
        }
        [HttpPost("SaveItemType")]
        public IActionResult SaveItemType(ItemType itemType)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ItemTypeservice.SaveItemType(itemType, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteItemType")]
        public IActionResult DeleteItemType(int ItemTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ItemTypeservice.DeleteItemType(ItemTypeId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillItemTypeSelect")]
        public IActionResult FillItemTypeSelect()
        {
            return Ok(_ItemTypeservice.FillItemTypeSelect());
        }
    }
}

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

    public class CarMovementsTypeController : ControllerBase
    {
        private ICarMovementsTypeService _carMovementsTypeservice;

        public GlobalShared _globalshared;

        public CarMovementsTypeController(ICarMovementsTypeService carMovementsTypeservice)
        {
            _carMovementsTypeservice = carMovementsTypeservice;
            HttpContext httpContext = HttpContext;_globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllItemTypes")]
        public IActionResult GetAllItemTypes(string? SearchText)
        {
            return Ok(_carMovementsTypeservice.GetAllTypes(SearchText));
        }
        [HttpPost("SaveItemType")]
        public IActionResult SaveItemType(CarMovementsType itemType)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            itemType.Code = itemType.TypeId == 0 ? null : itemType.TypeId.ToString();
            var result = _carMovementsTypeservice.SaveItemType(itemType, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteItemType")]
        public IActionResult DeleteItemType(int ItemTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _carMovementsTypeservice.DeleteType(ItemTypeId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillItemTypeSelect")]
        public IActionResult FillItemTypeSelect()
        {
            return Ok(_carMovementsTypeservice.FillCarMovmentsTypeSelect());
        }
    }
}

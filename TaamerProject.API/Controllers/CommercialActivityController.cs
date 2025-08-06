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
    public class CommercialActivityController : ControllerBase
    {
        private ICommercialActivityService  _activityService;
        public GlobalShared _globalshared;
        public CommercialActivityController(ICommercialActivityService activityService)
        {
            _activityService = activityService;
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetCommercialActivities")]

        public IActionResult GetCommercialActivities(string? SearchText,int Type)
        {
            return Ok(_activityService.GetCommercialActivities(SearchText??"", Type));
        }
        [HttpGet("FillTCommercialActivitiesSelect")]

        public IActionResult FillCommercialActivitiesSelect(int Type)
        {
            return Ok(_activityService.GetCommercialActivities("", Type).Result.Select(s => new {
                Id = s.CommercialActivityId,
                Name = s.NameAr
            }));
        }
        [HttpPost("SaveCommercialActivity")]

        public IActionResult SaveCommercialActivity(CommercialActivity commercialActivity)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _activityService.SaveCommercialActivity(commercialActivity, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteCommercialActivity")]

        public IActionResult DeleteCommercialActivity(int Id)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _activityService.DeleteCommercialActivity(Id, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
    }
}

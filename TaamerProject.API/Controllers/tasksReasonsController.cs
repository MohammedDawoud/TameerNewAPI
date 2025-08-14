using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tasksReasonsController : ControllerBase
    {
        private readonly IPro_tasksReasonsService _Pro_tasksReasonsService;
        public GlobalShared _globalshared;

        public tasksReasonsController(IPro_tasksReasonsService pro_tasksReasonsService)
        {
            _Pro_tasksReasonsService = pro_tasksReasonsService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAlltasksReasons")]

        public IActionResult GetAlltasksReasons()
        {
            var result = _Pro_tasksReasonsService.GetAlltasksReasons();
            return Ok(result);
        }
        [HttpPost("SaveReason")]

        public IActionResult SaveReason(Pro_tasksReasons Reason)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_tasksReasonsService.SaveReason(Reason, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteReason")]

        public IActionResult DeleteReason(int ReasonsId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_tasksReasonsService.DeleteReason(ReasonsId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillReasonsSelect")]

        public IActionResult FillReasonsSelect()
        {
            var act = _Pro_tasksReasonsService.GetAlltasksReasons().Result.Select(s => new
            {
                Id = s.ReasonsId,
                Name = s.NameAr,
                NameEn = s.NameEn
            });
            return Ok(act);
        }
    }
}

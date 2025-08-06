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
    public class projectsReasonsController : ControllerBase
    {
        private readonly IPro_projectsReasonsService _Pro_projectsReasonsService;
        public GlobalShared _globalshared;

        public projectsReasonsController(IPro_projectsReasonsService pro_projectsReasonsService)
        {
            _Pro_projectsReasonsService = pro_projectsReasonsService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllprojectsReasons")]

        public IActionResult GetAllprojectsReasons()
        {
            var result = _Pro_projectsReasonsService.GetAllprojectsReasons();
            return Ok(result);
        }
        [HttpPost("SaveReason")]

        public IActionResult SaveReason(Pro_projectsReasons Reason)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_projectsReasonsService.SaveReason(Reason, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteReason")]

        public IActionResult DeleteReason(int ReasonsId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_projectsReasonsService.DeleteReason(ReasonsId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillReasonsSelect")]

        public IActionResult FillReasonsSelect()
        {
            var act = _Pro_projectsReasonsService.GetAllprojectsReasons().Result.Select(s => new
            {
                Id = s.ReasonsId,
                Name = s.NameAr,
                NameEn=s.NameEn
            });
            return Ok(act);
        }
    }
}

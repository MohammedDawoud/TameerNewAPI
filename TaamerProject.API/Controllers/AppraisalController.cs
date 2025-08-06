using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class AppraisalController : ControllerBase
    {
        private IAppraisalService _appraisalservice;

        public GlobalShared _globalshared;

        public AppraisalController(IAppraisalService appraisalservice)
        {
            _appraisalservice = appraisalservice;
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);

        }
        [HttpPost("GetAllAppraisal")]
        public IActionResult GetAllAppraisal()
        {
            return Ok(_appraisalservice.GetAllAppraisal());
        }
        [HttpPost("SaveAppraisal")]

        public IActionResult SaveAppraisal(Appraisal appraisal)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _appraisalservice.SaveAppraisal(appraisal, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteAppraisal")]

        public IActionResult DeleteAppraisal(int AppraisalId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _appraisalservice.DeleteAppraisal(AppraisalId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("SearchAppraisal")]

        public IActionResult SearchAppraisal(AppraisalVM AppraisalySearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_appraisalservice.SearchAppraisal(AppraisalySearch, "", _globalshared.BranchId_G));
        }

    }
}

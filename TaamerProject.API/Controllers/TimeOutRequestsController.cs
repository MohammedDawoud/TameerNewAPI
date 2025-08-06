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

    public class TimeOutRequestsController : ControllerBase
    {
        private ITimeOutRequestsService _TimeOutRequestsservice;
        public GlobalShared _globalshared;
        public TimeOutRequestsController(ITimeOutRequestsService timeOutRequestsservice)
        {
            _TimeOutRequestsservice = timeOutRequestsservice;
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetTimeOutRequests")]

        public IActionResult GetTimeOutRequests()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_TimeOutRequestsservice.GetTimeOutRequests(_globalshared.BranchId_G));
        }
        [HttpPost("SaveTimeOutRequests")]

        public IActionResult SaveTimeOutRequests(TimeOutRequests TimeOutRequests)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _TimeOutRequestsservice.SaveTimeOutRequests(TimeOutRequests, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteTimeOutRequests")]

        public IActionResult DeleteTimeOutRequests(int RequestId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _TimeOutRequestsservice.DeleteTimeOutRequests(RequestId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("ApproveRequest")]

        public IActionResult ApproveRequest(int RequestId, string Comment)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _TimeOutRequestsservice.ApproveRequest(RequestId, _globalshared.UserId_G, _globalshared.BranchId_G, Comment);
            return Ok(result);
        }
        [HttpPost("RejectRequest")]

        public IActionResult RejectRequest(int RequestId, string Comment)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _TimeOutRequestsservice.RejectRequest(RequestId, _globalshared.UserId_G, _globalshared.BranchId_G, Comment);
            return Ok(result);
        }
    }
}

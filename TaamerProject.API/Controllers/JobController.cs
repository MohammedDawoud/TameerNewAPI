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

    public class JobController : ControllerBase
    {
        private IJobService _jobservice;
        public GlobalShared _globalshared;
        public JobController(IJobService jobservice)
        {
            _jobservice = jobservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllJobs")]
        public IActionResult GetAllJobs(string? SearchText)
        {
            return Ok(_jobservice.GetAllJobs(SearchText??""));
        }

        [HttpGet("GetAllJobs2")]
        public IActionResult GetAllJobs2()
        {
            return Ok(_jobservice.GetAllJobs(""));
        }
        [HttpPost("SaveJob")]
        public IActionResult SaveJob(Job job)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _jobservice.SaveJob(job, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteJob")]
        public IActionResult DeleteJob(int JobId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _jobservice.DeleteJob(JobId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillJobSelect")]
        public IActionResult FillJobSelect()
        {
            return Ok(_jobservice.FillJobSelect());
        }
    }
}

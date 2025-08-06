using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class TaskTypeController : ControllerBase
    {
        private readonly ITaskTypeService _taskTypeservice;

        public GlobalShared _globalshared;

        public TaskTypeController(ITaskTypeService taskTypeservice)
        {
            _taskTypeservice = taskTypeservice;
            HttpContext httpContext = HttpContext;
            _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllTaskType")]

        public IActionResult GetAllTaskType()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_taskTypeservice.GetAllTaskType(_globalshared.BranchId_G));
        }
        [HttpGet("GetAllTaskType2")]

        public IActionResult GetAllTaskType2(string SearchText)
        {
            return Ok(_taskTypeservice.GetAllTaskType2(SearchText));
        }
        [HttpPost("SaveTaskType")]

        public IActionResult SaveTaskType(TaskType taskType)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _taskTypeservice.SaveTaskType(taskType, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }

        [HttpPost("DeleteTaskType")]

        public IActionResult DeleteTaskType(int TaskTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _taskTypeservice.DeleteTaskType(TaskTypeId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }

        [HttpGet("FillTaskTypeSelectAE")]

        public IActionResult FillTaskTypeSelectAE()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var tasltype = _taskTypeservice.FillTaskTypeSelect(_globalshared.BranchId_G).Result.Select(s => new
            {
                Id = s.TaskTypeId,
                Name = s.NameAr
            });
            return Ok(tasltype);
        }
        [HttpGet("FillTaskTypeSelect")]

        public IActionResult FillTaskTypeSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var tasltype = _taskTypeservice.FillTaskTypeSelectAE(_globalshared.BranchId_G).Result.Select(s => new
            {
                Id = s.TaskTypeId,
                Name = s.NameAr,
                NameE = s.NameEn
            });
            return Ok(tasltype);

        }

    }
}

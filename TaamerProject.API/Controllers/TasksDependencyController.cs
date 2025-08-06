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

    public class TasksDependencyController : ControllerBase
    {
        private ITasksDependencyService _tasksDependencyservice;
        public GlobalShared _globalshared;
        public TasksDependencyController(ITasksDependencyService tasksDependencyservice)
        {
            _tasksDependencyservice = tasksDependencyservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpPost("SaveTasksDependency")]
        public IActionResult SaveTasksDependency(TasksDependency TasksDependency)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _tasksDependencyservice.SaveTasksDependency(TasksDependency, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveDependencyPhasesTask")]
        public IActionResult SaveDependencyPhasesTask(TasksDependencyData _tasksDependencyDataint)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _tasksDependencyservice.SaveDependencyPhasesTask(_tasksDependencyDataint.ProjectId, _tasksDependencyDataint.TaskLinkList??new List<TasksDependency>(), _tasksDependencyDataint.NodeLocList??new List<NodeLocations>(), _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveDependencyPhasesTaskNew")]
        public IActionResult SaveDependencyPhasesTaskNew(TasksDependencyDataNew _tasksDependencyDataint)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _tasksDependencyservice.SaveDependencyPhasesTaskNew(_tasksDependencyDataint.ProjectId, _tasksDependencyDataint.TaskLinkList ?? new List<TasksDependency>(), _tasksDependencyDataint.TasksList ?? new List<ProjectPhasesTasks>(), _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteTasksDependency")]
        public IActionResult DeleteTasksDependency(int DependencyId)
        {
            var result = _tasksDependencyservice.DeleteTasksDependency(DependencyId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetAllNodesTasks")]
        public IActionResult GetAllNodesTasks(int ProjectId)
        {
            var result = _tasksDependencyservice.GetTasksNodeByProjectId(ProjectId);
            return Ok(result);
        }
        [HttpGet("GetAllNodesTasksNew")]
        public IActionResult GetAllNodesTasksNew(int ProjectId)
        {
            var result = _tasksDependencyservice.GetTasksNodeByProjectIdNew(ProjectId);
            return Ok(result);
        }
        [HttpGet("GetProjectPhasesTaskTree")]
        public IActionResult GetProjectPhasesTaskTree(int ProjectId)
        {
            var treeAccounts = _tasksDependencyservice.GetProjectPhasesTaskTree(ProjectId);
            return Ok(treeAccounts);
        }
        public class TasksDependencyData
        {
            public int ProjectId { get; set; }
            public List<TasksDependency>? TaskLinkList { get; set; }

            public List<NodeLocations>? NodeLocList { get; set; }

        }
        public class TasksDependencyDataNew
        {
            public int ProjectId { get; set; }
            public List<ProjectPhasesTasks>? TasksList { get; set; }
            public List<TasksDependency>? TaskLinkList { get; set; }
        }

    }
}

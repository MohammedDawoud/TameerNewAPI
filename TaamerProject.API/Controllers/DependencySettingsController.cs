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

    public class DependencySettingsController : ControllerBase
    {
        private IDependencySettingsService _DependencySettingsservice;
        private IBranchesService _branchesService;
        public GlobalShared _globalshared;
        public DependencySettingsController(IDependencySettingsService dependencySettingsservice, IBranchesService branchesService)
        {
            _DependencySettingsservice = dependencySettingsservice;
            _branchesService = branchesService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllDependencySettings")]
        public IActionResult GetAllDependencySettings(int? SuccessorId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_DependencySettingsservice.GetAllDependencySettings(SuccessorId, _globalshared.BranchId_G));
        }
        [HttpGet("GetSubProjectSettingTree")]
        public IActionResult GetSubProjectSettingTree(int ProSubTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var treeAccounts = _DependencySettingsservice.GetProjSubTypeIdSettingTree(ProSubTypeId, _globalshared.BranchId_G);
            return Ok(treeAccounts);
        }
        [HttpPost("SaveDependencySettings")]
        public IActionResult SaveDependencySettings(DependencyData _dependencyData)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _DependencySettingsservice.SaveDependencySettings(_dependencyData.ProjSubTypeId, _dependencyData.TaskLinkList ??new List<DependencySettings>(), _dependencyData.NodeLocList??new List<NodeLocations>(), _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveDependencySettingsNew")]
        public IActionResult SaveDependencySettingsNew(DependencyDataNew _dependencyData)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _DependencySettingsservice.SaveDependencySettingsNew(_dependencyData.ProjSubTypeId, _dependencyData.TaskLinkList ?? new List<DependencySettingsNew>(), _dependencyData.TasksList ?? new List<SettingsNew>(), _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
            //return Ok("");
        }

        [HttpGet("GetAllNodesTasks")]
        public IActionResult GetAllNodesTasks(int ProSubTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _DependencySettingsservice.GetTasksNodeByProSubTypeId(ProSubTypeId, _globalshared.BranchId_G, _globalshared.UserId_G);
            return Ok(result);
        }
        [HttpGet("GetAllNodesTasksNew")]
        public IActionResult GetAllNodesTasksNew(int ProSubTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _DependencySettingsservice.GetTasksNodeByProSubTypeIdNew(ProSubTypeId, _globalshared.BranchId_G, _globalshared.UserId_G);
            return Ok(result);
        }
        [HttpPost("TransferSetting")]
        public IActionResult TransferSetting(int ProjSubTypeFromId, int ProjSubTypeToId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _DependencySettingsservice.TransferSettingNEW(ProjSubTypeFromId, ProjSubTypeToId, _globalshared.BranchId_G, _globalshared.UserId_G);
            return Ok(result);
        }
        [HttpPost("TransferSettingNew")]
        public IActionResult TransferSettingNewGrantt(int ProjSubTypeFromId, int ProjSubTypeToId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _DependencySettingsservice.TransferSettingNewGrantt(ProjSubTypeFromId, ProjSubTypeToId, _globalshared.BranchId_G, _globalshared.UserId_G);
            return Ok(result);
        }
        public class DependencyData
        {
            public int ProjSubTypeId { get; set; }
            public List<DependencySettings>? TaskLinkList { get; set; }

            public List<NodeLocations>? NodeLocList { get; set; }

        }
        public class DependencyDataNew
        {
            public int ProjSubTypeId { get; set; }
            public List<SettingsNew>? TasksList { get; set; }
            public List<DependencySettingsNew>? TaskLinkList { get; set; }

        }
    }
}

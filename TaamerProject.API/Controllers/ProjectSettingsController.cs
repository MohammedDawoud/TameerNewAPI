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

    public class ProjectSettingsController : ControllerBase
    {
        private ISettingsService _settingsservice;
        private readonly IPrivFollowersServices _PrivFollowersservice;
        private readonly IProSettingDetailsService _ProSettingDetailsService;
        private IBranchesService _branchesService;
        public GlobalShared _globalshared;
        public ProjectSettingsController(IBranchesService branchesService, IProSettingDetailsService ProSettingDetailsService,
            IPrivFollowersServices PrivFollowersservice, ISettingsService settingsservice
            )
        {
             _settingsservice = settingsservice;
            _PrivFollowersservice = PrivFollowersservice;
             _ProSettingDetailsService = ProSettingDetailsService;
             _branchesService = branchesService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        //public IActionResult EditProjectSettings(int ProjectSettingId)
        //{
        //    var AllProject = _ProSettingDetailsService.GetProSettingById(ProjectSettingId);
        //    return View(AllProject);
        //}
        [HttpGet("GetAllMainPhases")]
        public IActionResult GetAllMainPhases(int? ProSubTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AllMainPhases = _settingsservice.GetAllMainPhases(ProSubTypeId, _globalshared.BranchId_G).Result;
            return Ok(AllMainPhases);
        }
        [HttpGet("GetAllSubPhases")]
        public IActionResult GetAllSubPhases(int? ParentId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AllSettings = _settingsservice.GetAllSubPhases(ParentId, _globalshared.BranchId_G).Result;
            return Ok(AllSettings );
        }
        [HttpGet("GetAllSettingsByProjectID")]
        public IActionResult GetAllSettingsByProjectID(int ProjectID)
        {
            return Ok(_settingsservice.GetAllSettingsByProjectID(ProjectID) );
        }
        [HttpGet("GetAllSettingsByProjectIDwithoutmain")]
        public IActionResult GetAllSettingsByProjectIDwithoutmain(int ProjectID)
        {
            return Ok(_settingsservice.GetAllSettingsByProjectID(ProjectID).Result.Where(s => s.TimeType != null && s.TimeMinutes != null));
        }
        [HttpGet("GetAllSettingsByProjectIDwithoutmainNew")]
        public IActionResult GetAllSettingsByProjectIDwithoutmainNew(int ProjectID)
        {
            return Ok(_settingsservice.GetAllSettingsByProjectID(ProjectID).Result.Where(s => s.TimeType != null && s.TimeMinutes != null));
        }
        [HttpGet("GetAllSettingsByProjectIDwithoutmainNewGrantt")]
        public IActionResult GetAllSettingsByProjectIDwithoutmainNewGrantt(int ProjectID)
        {
            return Ok(_settingsservice.GetAllSettingsByProjectIDNew(ProjectID).Result.Where(s => s.TimeType != null && s.TimeMinutes != null));
        }
        [HttpGet("GetAllSettingsByProjectIDwithoutmainAll")]
        public IActionResult GetAllSettingsByProjectIDwithoutmainAll()
        {
            return Ok(_settingsservice.GetAllSettingsByProjectIDAll().Result.Where(s => s.TimeType != null && s.TimeMinutes != null));
        }
        [HttpGet("GetAllSettingsByProjectIDwithoutmainAllNew")]
        public IActionResult GetAllSettingsByProjectIDwithoutmainAllNew()
        {
            return Ok(_settingsservice.GetAllSettingsByProjectIDAllNew().Result.Where(s => s.TimeType != null && s.TimeMinutes != null));
        }
        [HttpGet("GetAllTasks")]
        public IActionResult GetAllTasks(int? ProSubTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G);
            var someTasks = _settingsservice.GetAllTasks(ProSubTypeId, 0).Result;
            foreach (var userBranch in userBranchs.Result)
            {

                var AllTasks = _settingsservice.GetAllTasks(ProSubTypeId, userBranch.BranchId).Result.ToList();
                var Tasks = someTasks.Union(AllTasks);
                someTasks = Tasks.ToList();
            }
            return Ok(someTasks);
        }
        [HttpPost("SaveSettings")]
        public IActionResult SaveSettings(Settings settings)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _settingsservice.SaveSettings(settings, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveSettingsList")]
        public IActionResult SaveSettingsList(List<Settings> settings)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _settingsservice.SaveSettingsList(settings, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveSettings2")]
        public IActionResult SaveSettings2(Settings settings)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _settingsservice.SaveSettings2(settings, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SavePrivFollowers")]
        public IActionResult SavePrivFollowers(PrivFollowers privFollowers)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _PrivFollowersservice.SavePrivFollowers(privFollowers, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteSettings")]
        public IActionResult DeleteSettings(int SettingId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _settingsservice.DeleteSettings(SettingId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("ConvertTasksSubToMain")]
        public IActionResult ConvertTasksSubToMain(int SettingId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _settingsservice.ConvertTasksSubToMain(SettingId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillMainPhases")]
        public IActionResult FillMainPhases(int? Param)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _settingsservice.FillMainPhases(Param, _globalshared.BranchId_G);
            return Ok(result);

        }
        [HttpGet("checkHaveMainPhases")]
        public Boolean checkHaveMainPhases(int? Param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return (_settingsservice.checkHaveMainPhases(Param, _globalshared.BranchId_G));
        }
        [HttpGet("FillSubPhases")]
        public IActionResult FillSubPhases(int? Param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _settingsservice.FillSubPhases(Param, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GenerateNextProSettingNumber")]
        public IActionResult GenerateNextProSettingNumber()
        {
            var result = _ProSettingDetailsService.GenerateNextProSettingNumber();
            return Ok(result);
        }
        [HttpGet("GetProSettingsDetails")]
        public IActionResult GetProSettingsDetails(int ProjectTypeId, int ProjectSubTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProSettingDetailsService.CheckProSettingData(ProjectTypeId, ProjectSubTypeId, _globalshared.BranchId_G).Result;
            var NewNo = _ProSettingDetailsService.GenerateNextProSettingNumber();
            if (result == null)
            {
                var result1 = new ProSettingDetails();
                result1.ProSettingNo = _ProSettingDetailsService.GenerateNextProSettingNumber().ToString();
                result1.ProSettingNote = "";
                result1.ProSettingId = 0;
                return Ok(result1);
            }
            else
            {
                return Ok(result);
            }
        }
        [HttpGet("GetProSettingsDetailsNew")]
        public IActionResult GetProSettingsDetailsNew(int ProjectTypeId, int ProjectSubTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext); 
            var result = _ProSettingDetailsService.CheckProSettingDataNew(ProjectTypeId, ProjectSubTypeId, _globalshared.BranchId_G).Result;
            var NewNo = _ProSettingDetailsService.GenerateNextProSettingNumberNew();
            if (result == null)
            {
                var result1 = new ProSettingDetailsNew();
                result1.ProSettingNo = _ProSettingDetailsService.GenerateNextProSettingNumberNew().ToString(); 
                result1.ProSettingNote = "";
                result1.ProSettingId = 0;
                return Ok(result1);
            }
            else
            {
                return Ok(result);
            }
        }
        [HttpGet("GetProjectSettingsDetails")]
        public IActionResult GetProjectSettingsDetails(int ProjectTypeId, int ProjectSubTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProSettingDetailsService.CheckProSettingData(ProjectTypeId, ProjectSubTypeId, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetProjectSettingsDetailsNew")]
        public IActionResult GetProjectSettingsDetailsNew(int ProjectTypeId, int ProjectSubTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProSettingDetailsService.CheckProSettingDataNew(ProjectTypeId, ProjectSubTypeId, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetProjectSettingsDetailsIFExist")]
        public IActionResult GetProjectSettingsDetailsIFExist(int ProjectId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProSettingDetailsService.GetProjectSettingsDetailsIFExist(ProjectId, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetProjectSettingsDetailsIFExistNew")]
        public IActionResult GetProjectSettingsDetailsIFExistNew(int ProjectId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProSettingDetailsService.GetProjectSettingsDetailsIFExistNew(ProjectId, _globalshared.BranchId_G); 
            return Ok(result);
        }
        [HttpGet("GetProjectSettingsDetailsIFExistNew2")]
        public IActionResult GetProjectSettingsDetailsIFExistNew2(int ProjectId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProSettingDetailsService.GetProjectSettingsDetailsIFExistNew2(ProjectId, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteProjectSettingsDetails")]
        public IActionResult DeleteProjectSettingsDetails(int SettingId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProSettingDetailsService.DeleteProjectSettingsDetails(SettingId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteProjectSettingsDetailsNew")]
        public IActionResult DeleteProjectSettingsDetailsNew(int SettingId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProSettingDetailsService.DeleteProjectSettingsDetailsNew(SettingId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveProSettingsDetails")]
        public IActionResult SaveProSettingsDetails(ProSettingDetails proSettingDetails)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProSettingDetailsService.SaveProSettingData(proSettingDetails, _globalshared.BranchId_G, _globalshared.UserId_G);
            return Ok(result);
        }
        [HttpPost("SaveProSettingsDetailsNew")]
        public IActionResult SaveProSettingsDetailsNew(ProSettingDetailsNew proSettingDetails)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProSettingDetailsService.SaveProSettingDataNew(proSettingDetails, _globalshared.BranchId_G, _globalshared.UserId_G);
            return Ok(result);
        }
        [HttpPost("EditProSettingsDetails")]
        public IActionResult EditProSettingsDetails(ProSettingDetails proSettingDetails)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _ProSettingDetailsService.EditProSettingsDetails(proSettingDetails, _globalshared.BranchId_G, _globalshared.UserId_G);
            return Ok(result);
        }
        [HttpGet("GetAllTasksByPhaseId")]
        public IActionResult GetAllTasksByPhaseId(int id)
        {
            var result = _settingsservice.GetAllTasksByPhaseId(id);
            return Ok(result);
        }
        [HttpGet("FillProSettingNo")]
        public IActionResult FillProSettingNo()
        {
            return Ok(_ProSettingDetailsService.FillProSettingNo());
        }
        [HttpGet("FillProSettingNoNew")]
        public IActionResult FillProSettingNoNew()
        {
            return Ok(_ProSettingDetailsService.FillProSettingNoNew());
        }
        [HttpPost("MerigTasks")]
        public IActionResult MerigTasks(SettingDetailsData _settingDetailsData)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _settingsservice.MerigTasks(_settingDetailsData.TasksIdArray ?? new int[] { }, _settingDetailsData.Description ?? "", _settingDetailsData.Note ?? "", _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetProSettingById")]
        public IActionResult GetProSettingById(int ProSettingId)
        {
            var result = _ProSettingDetailsService.GetProSettingById(ProSettingId);
            return Ok(result);
        }
        public class SettingDetailsData{

            public int[]? TasksIdArray { get; set; }
            public string? Description { get; set; }
            public string? Note { get; set; }

        }
    }
}

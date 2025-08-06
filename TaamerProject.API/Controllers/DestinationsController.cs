using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class DestinationsController : ControllerBase
    {
        private readonly IPro_filesReasonsService _Pro_filesReasonsService;
        private readonly IPro_DestinationTypesService _Pro_DestinationTypesService;
        private readonly IPro_DestinationDepartmentsService _Pro_DestinationDepartmentsService;
        private readonly IOrganizationsService _organizationsservice;
        private readonly IPro_DestinationsService _Pro_DestinationsService;

        public GlobalShared _globalshared;

        public DestinationsController(IPro_filesReasonsService pro_filesReasonsService,
            IPro_DestinationTypesService pro_DestinationTypesService, IPro_DestinationDepartmentsService pro_DestinationDepartmentsService,
            IPro_DestinationsService pro_DestinationsService, IOrganizationsService organizationsService)
        {
            _Pro_filesReasonsService = pro_filesReasonsService;
            _Pro_DestinationTypesService = pro_DestinationTypesService;
            _Pro_DestinationDepartmentsService = pro_DestinationDepartmentsService;
            _Pro_DestinationsService = pro_DestinationsService;

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            _organizationsservice = organizationsService;
        }
        [HttpPost("GetAllDestinations")]

        public IActionResult GetAllDestinations(ProjectVM ProjectsSearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AllUserBranch = 0;
            if (ProjectsSearch.BranchId == -1) AllUserBranch = 0;
            else AllUserBranch = _globalshared.BranchId_G;
            var AccBranchesList = ProjectsSearch.BranchesList ?? new List<int>();
            if (AccBranchesList.Count()==0)
            {
                AccBranchesList.Add(_globalshared.BranchId_G);
            }
            
            var result = _Pro_DestinationsService.GetAllDestinations(AllUserBranch, AccBranchesList);
            return Ok(result);
        }
        [HttpGet("GetDestinationByProjectId")]

        public IActionResult GetDestinationByProjectId(int ProjectId)
        {
            var result = _Pro_DestinationsService.GetDestinationByProjectId(ProjectId);
            return Ok(result);
        }
        [HttpGet("GetDestinationByProjectIdToReplay")]

        public IActionResult GetDestinationByProjectIdToReplay(int ProjectId)
        {
            var result = _Pro_DestinationsService.GetDestinationByProjectIdToReplay(ProjectId);
            return Ok(result);
        }
        [HttpGet("GetAllDestinationsProjects")]

        public IActionResult GetAllDestinationsProjects()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AccBranchesList = new List<int>();
            if (AccBranchesList.Count() == 0)
            {
                AccBranchesList.Add(_globalshared.BranchId_G);
            }
            AccBranchesList.Add(_globalshared.BranchId_G);
            var result = _Pro_DestinationsService.GetAllDestinations(_globalshared.BranchId_G, AccBranchesList);
            var List=result.Result.Select(s => new
            {
                ProjectId = s.ProjectId,
                ProjectNo = s.ProjectNo,
            }).ToList();
            var DestinationsProjects = List.Distinct().ToList();
            return Ok(DestinationsProjects);
        }
        [HttpPost("SaveDestination")]

        public IActionResult SaveDestination(Pro_Destinations Destination)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/MailStamp.html");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            var result = _Pro_DestinationsService.SaveDestination(Destination, _globalshared.UserId_G, _globalshared.BranchId_G, org, url, file);
            return Ok(result);
        }
        [HttpPost("SaveDestinationNotifi")]

        public IActionResult SaveDestinationNotifi(Pro_Destinations Destination)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/MailStamp.html");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            var result = _Pro_DestinationsService.SaveDestinationNotifi(Destination, _globalshared.UserId_G, _globalshared.BranchId_G, org, url, file);
            return Ok(result);
        }
        [HttpPost("SaveDestinationReplay")]
        public IActionResult SaveDestinationReplay(Pro_Destinations Destination)
        {


            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/MailStamp.html");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            var result = _Pro_DestinationsService.SaveDestinationReplay(Destination, _globalshared.UserId_G, _globalshared.BranchId_G, org, url, file);
            return Ok(result);
        }
        [HttpPost("SaveDestinationReplayNotifi")]
        public IActionResult SaveDestinationReplayNotifi(Pro_Destinations Destination)
        {


            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/MailStamp.html");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string resultLogoUrl = org.LogoUrl.Remove(0, 1);
            var file = Path.Combine(resultLogoUrl);
            var result = _Pro_DestinationsService.SaveDestinationReplayNotifi(Destination, _globalshared.UserId_G, _globalshared.BranchId_G, org, url, file);
            return Ok(result);
        }
        [HttpPost("DeleteDestination")]

        public IActionResult DeleteDestination(int DestinationId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_DestinationsService.DeleteDestination(DestinationId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillDestinationsSelect")]

        public IActionResult FillDestinationsSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var AccBranchesList = new List<int>();
            if (AccBranchesList.Count() == 0)
            {
                AccBranchesList.Add(_globalshared.BranchId_G);
            }
            var act = _Pro_DestinationsService.GetAllDestinations(_globalshared.BranchId_G, AccBranchesList).Result.Select(s => new
            {
                Id = s.DestinationId,
                TransactionNumber = s.TransactionNumber,
            });
            return Ok(act);
        }

        //-----------------------------------------------------------------------------------------------

        [HttpGet("GetAllDestinationTypes")]

        public IActionResult GetAllDestinationTypes()
        {
            var result = _Pro_DestinationTypesService.GetAllDestinationTypes();
            return Ok(result);
        }
        [HttpPost("SaveDestinationType")]

        public IActionResult SaveDestinationType(Pro_DestinationTypes DestinationType)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_DestinationTypesService.SaveDestinationType(DestinationType, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteDestinationType")]

        public IActionResult DeleteDestinationType(int DestinationTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_DestinationTypesService.DeleteDestinationType(DestinationTypeId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillDestinationTypesSelect")]

        public IActionResult FillDestinationTypesSelect()
        {
            var act = _Pro_DestinationTypesService.GetAllDestinationTypes().Result.Select(s => new
            {
                Id = s.DestinationTypeId,
                Name = s.NameAr,
                NameAr = s.NameAr,
                NameEn = s.NameEn,
                DepartmentName = s.DepartmentName,

            });
            return Ok(act);
        }

        //---------------------------------------------------------------------------------------------


        [HttpGet("GetAllDestinationDepartments")]

        public IActionResult GetAllDestinationDepartments()
        {
            var result = _Pro_DestinationDepartmentsService.GetAllDestinationDepartments();
            return Ok(result);
        }
        [HttpPost("SaveDestinationDepartment")]

        public IActionResult SaveDestinationDepartment(Pro_DestinationDepartments DestinationDepartment)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_DestinationDepartmentsService.SaveDestinationDepartment(DestinationDepartment, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteDestinationDepartment")]

        public IActionResult DeleteDestinationDepartment(int DepartmentId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_DestinationDepartmentsService.DeleteDestinationDepartment(DepartmentId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillDestinationDepartmentsSelect")]

        public IActionResult FillDestinationDepartmentsSelect()
        {
            var act = _Pro_DestinationDepartmentsService.GetAllDestinationDepartments().Result.Select(s => new
            {
                Id = s.DepartmentId,
                Name = s.NameAr,
                NameAr = s.NameAr,
                NameEn = s.NameEn,

            });
            return Ok(act);
        }
        [HttpGet("FillDestinationDepartmentsByTypeIdSelect")]

        public IActionResult FillDestinationDepartmentsByTypeIdSelect(int typeId)
        {
            var act = _Pro_DestinationDepartmentsService.GetAllDestinationDepartmentsByTypeId(typeId).Result.Select(s => new
            {
                Id = s.DepartmentId,
                Name = s.NameAr,
                NameAr = s.NameAr,
                NameEn = s.NameEn,

            });
            return Ok(act);
        }

        //---------------------------------------------------------------------------------------------

        [HttpGet("GetAllprojectsReasons")]

        public IActionResult GetAllprojectsReasons()
        {
            var result = _Pro_filesReasonsService.GetAllfilesReasons();
            return Ok(result);
        }
        [HttpPost("SaveReasonFile")]

        public IActionResult SaveReasonFile(Pro_filesReasons Reason)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_filesReasonsService.SaveReason(Reason, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteReasonFile")]

        public IActionResult DeleteReasonFile(int ReasonsId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Pro_filesReasonsService.DeleteReason(ReasonsId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillReasonsSelectFile")]

        public IActionResult FillReasonsSelectFile()
        {
            var act = _Pro_filesReasonsService.GetAllfilesReasons().Result.Select(s => new
            {
                Id = s.ReasonsId,
                Name = s.NameAr,
                NameEn = s.NameEn
            });
            return Ok(act);
        }
    }
}

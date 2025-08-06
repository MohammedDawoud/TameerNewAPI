using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private IPermissionService _permissionService;
        private string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        private IOrganizationsService _organizationsservice;
        private readonly IFiscalyearsService _FiscalyearsService;
        private readonly IEmployeesService _employeesService;
        public PermissionController(IPermissionService permissionService, IOrganizationsService organizationsservice,
            IFiscalyearsService FiscalyearsService, IConfiguration _configuration, IEmployeesService employeesService
            )
        {
            _permissionService = permissionService;
            _employeesService = employeesService;

            _organizationsservice = organizationsservice;
            _FiscalyearsService = FiscalyearsService;
            Configuration = _configuration;
            Con = this.Configuration.GetConnectionString("DBConnection");
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }

        [HttpGet("GetAllPermissions")]
        public async Task<IActionResult> GetAllPermissions(int? EmpId, int? Type, int? Status, string? FromDate,string? ToDate, string? SearchText)
        {
            return Ok( await _permissionService.GetAllPermissions(EmpId,Type,Status,FromDate,ToDate, SearchText ?? ""));
        }

        [HttpGet("GetAllPermissions_DashBoard")]
        public async Task<IActionResult> GetAllPermissions_DashBoard()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var EmpId = _employeesService.GetEmployeeByUserid(_globalshared.UserId_G).Result.FirstOrDefault();
            return Ok(await _permissionService.GetAllPermissions(EmpId.EmployeeId));
        }

        [HttpPost("SavePermission")]
        public IActionResult SavePermission(Permissions permissions)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.GetFullPath("Email/MailStamp.html");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G);
            var file = Path.GetFullPath(org.Result.LogoUrl.TrimStart('/'));
            var result = _permissionService.SavePermission(permissions, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, url, file);
            return Ok(result);
        }


        [HttpPost("DeletePermissions")]
        public IActionResult DeletePermissions(int PermissionId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _permissionService.DeletePermissions(PermissionId, _globalshared.UserId_G, _globalshared.BranchId_G);

            return Ok(result);
        }
        [HttpPost("Updatepermission")]
        public IActionResult Updatepermission(int PermissionId, int Type, string? Reason)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.GetFullPath("Email/MailStamp.html");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G);
            var file = Path.GetFullPath(org.Result.LogoUrl.TrimStart('/'));
            var result = _permissionService.Updatepermission(PermissionId, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, Type, Con, url, file, Reason);
            return Ok(result);
        }
    }
}

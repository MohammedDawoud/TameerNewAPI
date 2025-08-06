using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class VacationController : ControllerBase
    {
        private IVacationService _vacationservice;
        private string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        private IOrganizationsService _organizationsservice;
        private readonly IFiscalyearsService _FiscalyearsService;
        public VacationController(IVacationService vacationservice, IOrganizationsService organizationsservice,
            IFiscalyearsService FiscalyearsService, IConfiguration _configuration
            )
        {
             _vacationservice = vacationservice;
           
            _organizationsservice = organizationsservice;
             _FiscalyearsService = FiscalyearsService;
            Configuration = _configuration;
            Con = this.Configuration.GetConnectionString("DBConnection");
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }

        [HttpGet("GetAllVacations")]
        public IActionResult GetAllVacations(int? EmpId, string? SearchText)
        {
            return Ok(_vacationservice.GetAllVacations(EmpId, SearchText ?? "") );
        }

        [HttpGet("GetAllVacationsArchived")]
        public IActionResult GetAllVacationsArchived(int? EmpId, string? SearchText)
        {
            return Ok(_vacationservice.GetAllVacationsArchived(EmpId, SearchText ?? ""));
        }


        [HttpGet("GetAllVacations2")]
        public IActionResult GetAllVacations2(string? SearchText)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_vacationservice.GetAllVacations2(_globalshared.UserId_G, SearchText ?? "") );
        }
        [HttpGet("GetAllVacationsw")]
        public IActionResult GetAllVacationsw(string? SearchText)
        {
            return Ok(_vacationservice.GetAllVacationsw(SearchText ?? "") );
        }

        [HttpGet("GetAllVacationsw_Accepted")]
        public IActionResult GetAllVacationsw_Accepted(string? SearchText)
        {
            return Ok(_vacationservice.GetAllVacationsw(SearchText ?? "").Result.Where(x=>x.VacationStatus==2 && x.BackToWorkDate ==null));
        }
        [HttpGet("GetAllVacationsw2")]
        public IActionResult GetAllVacationsw2(string? SearchText, int? status)
        {
            return Ok(_vacationservice.GetAllVacationsw2(SearchText ?? "", status??0));
        }
        [HttpPost("GetAllVacationsSearch")]
        public IActionResult GetAllVacationsSearch(VacationVM VacationSearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var vac = _vacationservice.GetAllVacationsSearch(VacationSearch, _globalshared.BranchId_G);
            return Ok(vac);
        }
        [HttpPost("SaveVacation")]
        public IActionResult SaveVacation(Vacation vacation)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            // var url = Server.MapPath("~/Email/MailStamp.html");
            var url = Path.GetFullPath("Email/MailStamp.html");
            // var file = Server.MapPath("~/dist/assets/images/logo.png");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G);
            //var file = Server.MapPath("~") + org.LogoUrl;
            var file = Path.GetFullPath(org.Result.LogoUrl.TrimStart('/'));
            var result = _vacationservice.SaveVacation(vacation, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, url, file);
            return Ok(result);
        }
        [HttpPost("SaveVacation_Management")]
        public IActionResult SaveVacation_Management(Vacation vacation)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            // var url = Server.MapPath("~/Email/MailStamp.html");
            var url = Path.Combine("/Email/MailStamp.html");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G);
            // var file = Server.MapPath("~") + org.LogoUrl;
            var file = Path.Combine(org.Result?.LogoUrl ?? "");
            var result = _vacationservice.SaveVacation_Management(vacation, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, url, file);
            return Ok(result);
        }
        [HttpGet("CheckVacationTasks")]
        public IActionResult CheckVacationTasks(int VacationId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _vacationservice.CheckIfHaveTasks(VacationId, _globalshared.Lang_G);
            return Ok(result);
        }
        [HttpPost("SaveVacation2")]
        public IActionResult SaveVacation2(Vacation vacation)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            // var url = Server.MapPath("~/Email/MailStamp.html");
            var url = Path.Combine("/Email/MailStamp.html");
           var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G);
            //var file = Server.MapPath("~") + org.LogoUrl;
            var file = Path.GetFullPath(org.Result.LogoUrl.TrimStart('/'));
            var result = _vacationservice.SaveVacation2(vacation, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, url, file);
            return Ok(result);
        }
        [HttpPost("SaveVacationWorkers")]
        public IActionResult SaveVacationWorkers(Vacation vacation)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            // var url = Server.MapPath("~/Email/MailStamp.html");
            var url = Path.GetFullPath("Email/MailStamp.html");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G);
            //var file = Server.MapPath("~") + org.LogoUrl;
            var file = Path.GetFullPath(org.Result.LogoUrl.TrimStart('/'));

            var result = _vacationservice.SaveVacationWorkers(vacation, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, url, file);
            return Ok(result);
        }
        [HttpPost("DeleteVacation")]
        public IActionResult DeleteVacation(int VacationId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _vacationservice.DeleteVacation(VacationId, _globalshared.UserId_G, _globalshared.BranchId_G);

            return Ok(result);
        }
        [HttpPost("UpdateVacation")]
        public IActionResult UpdateVacation(int VacationId, int Type,string? Reason)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.GetFullPath("Email/MailStamp.html");
            // var file = Server.MapPath("~/dist/assets/images/logo.png");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G);
            //var file = Server.MapPath("~") + org.LogoUrl;
            var file = Path.GetFullPath(org.Result.LogoUrl.TrimStart('/'));

            var result = _vacationservice.UpdateVacation(VacationId, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, Type, Con, url, file, Reason);
            return Ok(result);
        }
        [HttpPost("CheckLoan")]
        public IActionResult CheckLoan(int VacationId, int Type, string? Reason)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            // var url = Server.MapPath("~/Email/MailStamp.html");
            var url = Path.GetFullPath("Email/MailStamp.html");
            // var file = Server.MapPath("~/dist/assets/images/logo.png");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G);
            //var file = Server.MapPath("~") + org.LogoUrl;
            var file = Path.GetFullPath(org.Result.LogoUrl.TrimStart('/'));

            var result = _vacationservice.CheckLoan(VacationId, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, Type, Con, url, file, Reason);
            return Ok(result);
        }
        [HttpPost("UpdateDecisionType_V")]
        public IActionResult UpdateDecisionType_V(int VacationId, int DecisionType)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _vacationservice.UpdateDecisionType_V(VacationId, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, DecisionType);
            return Ok(result);
        }
        [HttpPost("UpdateBackToWork_V")]
        public IActionResult UpdateBackToWork_V(int vacationId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _vacationservice.UpdateBackToWork_V(vacationId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetNetVacationDays")]
        public IActionResult GetNetVacationDays(string StartDate, string EndDate, int EmpId, int VacationTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int days = _vacationservice.GetVacationDays_WithoutHolidays(StartDate, EndDate, EmpId, _globalshared.Lang_G, Con??"", VacationTypeId).Count();
            return Ok(days );
        }
        [HttpPost("UploadVacationImage")]
        public IActionResult UploadVacationImage([FromForm]Vacation vac, IFormFile postedFiles)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            string fileName = "";
            string pathes = "";
            if (postedFiles != null)
            {
                System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


                string path = System.IO.Path.Combine("Uploads/", "Attachment/");
                string pathW = System.IO.Path.Combine("/Uploads/", "Attachment/");

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                List<string> uploadedFiles = new List<string>();
               
                //foreach (IFormFile postedFile in postedFiles)
                //{
                 fileName = System.IO.Path.GetFileName(Guid.NewGuid() + postedFiles.FileName);
                //string fileName = System.IO.Path.GetFileName(postedFiles.FileName);

                var path2 = Path.Combine(path, fileName);
                if (System.IO.File.Exists(path2))
                {
                    System.IO.File.Delete(path2);
                }
                using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(path, fileName), System.IO.FileMode.Create))
                {


                    postedFiles.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    // string returnpath = host + path + fileName;
                    //pathes.Add(pathW + fileName);
                    pathes = pathW + fileName;
                }


                if (pathes != null)
                {
                    vac.FileUrl = pathes;
                }
            }

            var result = _vacationservice.SaveVacationImage(vac.VacationId, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, fileName, pathes);
            return Ok(result);

        }
        [HttpPost("GenerateRandomNo")]

        public int GenerateRandomNo()
        {
            int _min = 100000;
            int _max = 999999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
    }
}

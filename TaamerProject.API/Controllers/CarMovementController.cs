using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Data;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class CarMovementController : ControllerBase
    {
        private IBranchesService _branchesService;
        private ICarMovementsService _carMovementService;
        private IOrganizationsService _organizationsservice;
        private ICarMovementsTypeService _carMovementsTypeservice;
        private readonly IFiscalyearsService _FiscalyearsService;
        private byte[] ReportPDF;
        string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        public CarMovementController(IBranchesService branchesService, ICarMovementsService carMovementService
            , IOrganizationsService organizationsservice, ICarMovementsTypeService carMovementsTypeservice
            , IFiscalyearsService fiscalyearsService, IConfiguration _configuration)
        {
            _branchesService = branchesService;
            _carMovementService= carMovementService;
            _organizationsservice = organizationsservice;
            _carMovementsTypeservice= carMovementsTypeservice;
            _FiscalyearsService = fiscalyearsService;
            HttpContext httpContext = HttpContext;_globalshared = new GlobalShared(httpContext);
            Configuration = _configuration;Con = this.Configuration.GetConnectionString("DBConnection");
        }
        [HttpGet("GetAllCarMovements")]
        public IActionResult GetAllCarMovements()
        {
            return Ok(_carMovementService.GetAllCarMovements());
        }
        [HttpGet("GetTypeList")]
        public static IEnumerable<SelectListItem> GetTypeList()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem{Text = "Resources.General_petrol", Value = "1"},
                new SelectListItem{Text = "Resources.General_oil", Value = "2"},
                new SelectListItem{Text = "Resources.General_maintenance", Value = "3"},
                new SelectListItem{Text = "Resources.General_Trafficviolation", Value = "4"},
                new SelectListItem{Text = "Resources.Car_Minsurance", Value = "5"}

            };
            return items;
        }

        [HttpPost("GetAllCarMovementsSearch")]
        public IActionResult GetAllCarMovementsSearch(CarMovementsVM Search)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_carMovementService.GetAllCarMovementsSearch(Search, _globalshared.BranchId_G));
        }
        [HttpPost("SaveCarMovement")]
        public IActionResult SaveCarMovement(CarMovements carMovement)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _carMovementService.SaveCarMovement(carMovement, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G);
            return Ok(result);
        }
        [HttpPost("DeleteCarMovement")]
        public IActionResult DeleteCarMovement(int MovementId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _carMovementService.DeleteCarMovement(MovementId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("SearchCarMovements")]
        public IActionResult SearchCarMovements(CarMovementsVM CarMovementsSearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_carMovementService.SearchCarMovements(CarMovementsSearch, _globalshared.BranchId_G));
        }
        [HttpGet("GetAllCarMovementsByDateSearch")]
        public IActionResult GetAllCarMovementsByDateSearch(CarMovementsVM carMovements)
        {
            var result = _carMovementService.GetAllCarMovementsByDateSearch(carMovements?.Date??"", carMovements?.EndDate??"");
            return Ok(result);
        }

        [HttpGet("PrintCarMovementAll")]
        public IActionResult PrintCarMovementAll()
        {
            List<AllCrarMovementReport> allCrarMovements = new List<AllCrarMovementReport>();
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var type = _carMovementsTypeservice.FillCarMovmentsTypeSelect();
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
            DataTable CarMovementsVMs = _carMovementService.GetAllCarMovementsByDate("", "", Con).Result;
                foreach (DataRow dr in CarMovementsVMs.Rows)
            {
                allCrarMovements.Add(new AllCrarMovementReport
                {
                    CarName = _globalshared.Lang_G=="en"? (dr["NameEn"]).ToString(): (dr["NameAr"]).ToString(),
                    Oil = Convert.ToDecimal((dr["Oil"]).ToString()),
                    Zait = Convert.ToDecimal((dr["Zait"]).ToString()),
                    Repairr = Convert.ToDecimal((dr["Repairr"]).ToString()),
                    Mokhalfa = Convert.ToDecimal((dr["Mokhalfa"]).ToString()),
                    Taamen = Convert.ToDecimal((dr["Taamen"]).ToString()),
                    Others = Convert.ToDecimal((dr["Others"]).ToString()),
                    Total = Convert.ToDecimal((dr["Oil"]).ToString())+ Convert.ToDecimal((dr["Zait"]).ToString())+ Convert.ToDecimal((dr["Repairr"]).ToString())
                    +Convert.ToDecimal((dr["Mokhalfa"]).ToString())+ Convert.ToDecimal((dr["Taamen"]).ToString())
                    + Convert.ToDecimal((dr["Others"]).ToString()),


                });
             }

            //var json = new JsonResult(CarMovementsVMs);
            return Ok(allCrarMovements);
        }
        [HttpGet("PrintCarMovement")]
        public ActionResult PrintCarMovement(string CarType, string CarId, string StartDate, string EndDate, string EmpId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
            DataTable CarMovementsVMs = _carMovementService.GetAllCarMovementsByDateSearch(CarType, CarId, StartDate, EndDate, EmpId, Con).Result;
            
            return Ok(CarMovementsVMs);
            //ReportPDF = humanResourcesReports.PrintCarMovement(CarMovementsVMs, StartDate, EndDate, infoDoneTasksReport);
            //string existTemp = HttpContext.Server.MapPath(@"~\TempFiles\");

            //if (!Directory.Exists(existTemp))
            //{
            //    Directory.CreateDirectory(existTemp);
            //}
            ////File  
            //string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
            //string FilePath = HttpContext.Server.MapPath(@"~\TempFiles\") + FileName;

            ////create and set PdfReader  
            //System.IO.File.WriteAllBytes(FilePath, ReportPDF);
            ////return file 
            //string FilePathReturn = @"TempFiles/" + FileName;
            //return Content(FilePathReturn);
        }
    }

    public class AllCrarMovementReport
    {
        public string CarName{ get; set; }
        public decimal Oil { get; set; }
        public decimal Zait { get; set; }
        public decimal Repairr { get; set; }
        public decimal Mokhalfa { get; set; }
        public decimal Taamen { get; set; }
        public decimal Others { get; set; }
        public decimal Total { get; set; }
    }
}

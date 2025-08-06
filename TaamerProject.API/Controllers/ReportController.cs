using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private IAccountsService _accountsService;
        private IBranchesService _branchesService;
        private IOrganizationsService _organizationsservice;
        private string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;

        public ReportController(IAccountsService accountsService, IBranchesService branchesService, IOrganizationsService organizationsservice
            , IConfiguration _configuration)
        {
            _accountsService = accountsService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            _branchesService = branchesService;
            _organizationsservice = organizationsservice;
            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");

        }
        //public ActionResult GetReport(string StartDate, string ToDate, string CCID, string Zerocheck, string LVL)
        //{
        //    int costID = Convert.ToInt32(CCID == "" ? "0" : CCID);
        //    var FiscalId = Request.Cookies["ActiveYear"].Value;
        //    var YearNEW = Convert.ToInt32(_fiscalyearsservice.GetYearID(Convert.ToInt32(FiscalId)));
        //    int ZerocheckValue = Convert.ToInt32(Zerocheck == "" ? "0" : Zerocheck);
        //    var result = _accountsService.GetIncomeStatmentDGVNew(StartDate, ToDate, costID, BranchId, Lang, Con, YearNEW, ZerocheckValue, LVL);

        //    List<IncomeStatmentVM> res = result.ToList();
        //    int orgId = _branchesService.GetOrganizationId(BranchId);
        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);
        //    string[] infoDoneTasksReport = { Lang == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

        //    //ReportPDF = Bayanatech.TameerUI.pdfHandler.ReportsOf7sabat.incomeList(res, StartDate, ToDate, infoDoneTasksReport);//المجاميع
        //    ViewData["result"] = res;

        //    ViewData["FromDate"] = StartDate;

        //    ViewData["ToDate"] = ToDate;


        //    string Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
        //    ViewData["Date"] = Date;

        //    var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId);
        //    if (objOrganization2 != null)
        //        ViewData["Org_VD"] = objOrganization2;
        //    else
        //        ViewData["Org_VD"] = null;


        //    return PartialView("_IncomeStatePDF");
        //}
        [HttpPost("GetReportIncomeLevels")]

        public IActionResult GetReportIncomeLevels([FromForm] string? FromDate, [FromForm] string? ToDate, [FromForm] string? CostCenter, [FromForm] int? ZeroCheck, [FromForm] string? LVL, [FromForm] int? FilteringType, [FromForm] int? FilteringTypeAll, [FromForm] string? FilteringTypeStr, [FromForm] string? FilteringTypeAllStr, [FromForm] string? AccountIds, [FromForm] int? PeriodFillterType, [FromForm] int? PeriodCounter, [FromForm] int? TypeF, [FromForm] string? Filtertxt)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            ReportIncomeLevelsVM _reportIncomeLevelsVM = new ReportIncomeLevelsVM();
            int costID = Convert.ToInt32(CostCenter ?? "0");
            var result = _accountsService.GetIncomeStatmentDGVLevels(FromDate ?? "", ToDate ?? "", costID, _globalshared.BranchId_G, _globalshared.Lang_G, Con ?? "", _globalshared.YearId_G, ZeroCheck ?? 0, LVL, FilteringType ?? 0, FilteringTypeAll ?? 0, FilteringTypeStr, FilteringTypeAllStr, AccountIds, PeriodFillterType ?? 0, PeriodCounter ?? 0, TypeF ?? 0).Result.ToList();

            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };


            _reportIncomeLevelsVM.result = result;
            _reportIncomeLevelsVM.StartDate = FromDate;
            _reportIncomeLevelsVM.EndDate = ToDate;

            _reportIncomeLevelsVM.DateTimeNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            _reportIncomeLevelsVM.Filtertxt = Filtertxt;

            if (Filtertxt != null)
            {
                _reportIncomeLevelsVM.StartDate = null;
                _reportIncomeLevelsVM.EndDate = null;
            }
            var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
            var branch = _branchesService.GetBranchById(_globalshared.BranchId_G).Result;
            _reportIncomeLevelsVM.BranchName = branch.NameAr;
            _reportIncomeLevelsVM.Org_VD = objOrganization2;
            return Ok(_reportIncomeLevelsVM);
        }



        //public ActionResult GetReportAllIncome(string StartDate, string ToDate, string CCID, string Zerocheck, string LVL)
        //{
        //    int costID = Convert.ToInt32(CCID == "" ? "0" : CCID);
        //    int ZerocheckValue = Convert.ToInt32(Zerocheck == "" ? "0" : Zerocheck);
        //    var FiscalId = Request.Cookies["ActiveYear"].Value;
        //    var YearNEW = Convert.ToInt32(_fiscalyearsservice.GetYearID(Convert.ToInt32(FiscalId)));
        //    var result = _accountsService.GetAllIncomeStatmentDGVNew(StartDate, ToDate, costID, BranchId, Lang, Con, YearNEW, ZerocheckValue, LVL);

        //    List<IncomeStatmentVM> res = result.ToList();
        //    int orgId = _branchesService.GetOrganizationId(BranchId);
        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);

        //    ViewData["result"] = res;

        //    ViewData["FromDate"] = StartDate;

        //    ViewData["ToDate"] = ToDate;


        //    string Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
        //    ViewData["Date"] = Date;

        //    var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId);
        //    if (objOrganization2 != null)
        //        ViewData["Org_VD"] = objOrganization2;
        //    else
        //        ViewData["Org_VD"] = null;


        //    return PartialView("_IncomeStatePDF");
        //}
    }
}

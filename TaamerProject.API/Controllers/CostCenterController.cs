using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class CostCenterController : ControllerBase
    {
        private ICostCenterService _CostCenterservice;
        private IBranchesService _BranchesService;
        private IOrganizationsService _organizationsservice;
        private readonly IFiscalyearsService _FiscalyearsService;
        private IProjectService _projectservice;

        private byte[] ReportPDF;
        private string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        public CostCenterController(ICostCenterService costCenterservice, IBranchesService branchesService
            , IOrganizationsService organizationsservice, IFiscalyearsService fiscalyearsService
            , IProjectService projectservice, IConfiguration _configuration)
        {
            _CostCenterservice= costCenterservice;
            _BranchesService= branchesService;
            _organizationsservice = organizationsservice;
            _FiscalyearsService = fiscalyearsService;
            _projectservice = projectservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Configuration = _configuration;
            Con = this.Configuration.GetConnectionString("DBConnection");
        }
        [HttpGet("GetAllCostCenters")]
        public IActionResult GetAllCostCenters(string? SearchText)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_CostCenterservice.GetAllCostCenters(SearchText??"", _globalshared.Lang_G, _globalshared.BranchId_G));
        }
        [HttpPost("SaveCostCenter")]
        public IActionResult SaveCostCenter(CostCenters costCenter)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _CostCenterservice.SaveCostCenter(costCenter, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteCostCenter")]
        public IActionResult DeleteCostCenter(int CostCenterId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _CostCenterservice.DeleteCostCenter(CostCenterId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetCostCenterTree")]
        public IActionResult GetCostCenterTree()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var treeAccounts = _CostCenterservice.GetCostCenterTree(_globalshared.Lang_G, _globalshared.BranchId_G);
            return Ok(treeAccounts);
        }
        [HttpGet("GetCostCenterById")]
        public IActionResult GetCostCenterById(int CostCenterId)
        {
            var Account = _CostCenterservice.GetCostCenterById(CostCenterId);
            return Ok(Account);
        }
        [HttpGet("GetCostCenterByProId")]
        public IActionResult GetCostCenterByProId(int ProjectId)
        {
            var Account = _CostCenterservice.GetCostCenterByProId(ProjectId);
            return Ok(Account);
        }
        [HttpGet("GetCostCenterByCode")]
        public IActionResult GetCostCenterByCode(string Code)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Account = _CostCenterservice.GetCostCenterByCode(Code, _globalshared.Lang_G, _globalshared.BranchId_G);
            return Ok(Account);
        }
      
        [HttpGet("FillCostCenterSelect_Invoices")]
        public IActionResult FillCostCenterSelect_Invoices(int param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            try
            {
                List<LoadCostCenterData> listofCostCenter = new List<LoadCostCenterData>();

                if (param != 0)
                {
                    try
                    {
                        var CostCenterFromProject = _projectservice.GetCostCenterByProId(_globalshared.Lang_G, param).Result;
                        if (CostCenterFromProject.CostCenterId > 0)
                        {
                            LoadCostCenterData CostCenteritem = new LoadCostCenterData();
                            CostCenteritem.Id = CostCenterFromProject.CostCenterId ?? 0;
                            CostCenteritem.Name = CostCenterFromProject.CostCenterName;
                            CostCenteritem.Code = CostCenterFromProject.CostCenterCode;
                            listofCostCenter.Add(CostCenteritem);
                        }
                    }
                    catch (Exception)
                    {
                        listofCostCenter = new List<LoadCostCenterData>();
                    }

                    try
                    {
                        var CostCenterProject = _CostCenterservice.GetCostCenterByProId(param).Result;
                        if (CostCenterProject.CostCenterId > 0)
                        {
                            LoadCostCenterData CostCenteritem2 = new LoadCostCenterData();
                            CostCenteritem2.Id = CostCenterProject.CostCenterId;
                            CostCenteritem2.Name = CostCenterProject.NameAr;
                            CostCenteritem2.Code = CostCenterProject.Code;

                            listofCostCenter.Add(CostCenteritem2);
                        }
                    }
                    catch (Exception)
                    {
                        listofCostCenter = new List<LoadCostCenterData>();
                    }


                    var costCenters = listofCostCenter.Select(s => new {
                        Id = s.Id,
                        Name = s.Code + " - " + s.Name
                    });
                    return Ok(costCenters);
                }
                else
                {
                    return Ok(new List<LoadCostCenterData>());
                }

            }
            catch (Exception ex)
            {
                return Ok(new List<LoadCostCenterData>());
            }


        }
        [HttpGet("FillCostCenterSelect")]
        public IActionResult FillCostCenterSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var costCenters = _CostCenterservice.GetAllCostCenters("", _globalshared.Lang_G, _globalshared.BranchId_G).Result.Select(s => new {
                Id = s.CostCenterId,
                Name = s.Code + " - " + s.CostCenterName
            });
            return Ok(costCenters);
        }
        [HttpGet("FillCostCenterSelect_B")]
        public IActionResult FillCostCenterSelect_B()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var costCenters = _CostCenterservice.GetAllCostCenters_B("", _globalshared.Lang_G, _globalshared.BranchId_G).Result.Select(s => new {
                Id = s.CostCenterId,
                Name = s.Code + " - " + s.CostCenterName
            });
            return Ok(costCenters);
        }
        [HttpGet("FillCostCenterSelectBranch")]
        public IActionResult FillCostCenterSelectBranch(int param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var costCenters = _CostCenterservice.GetAllCostCentersByCostBranch("", _globalshared.Lang_G, _globalshared.BranchId_G, param).Result.Select(s => new {
                Id = s.CostCenterId,
                Name = s.Code + " - " + s.CostCenterName
            });
            return Ok(costCenters);
        }
        [HttpGet("GetBranch_Costcenter")]
        public IActionResult GetBranch_Costcenter()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var costCenters = _CostCenterservice.GetBranch_Costcenter(_globalshared.Lang_G, _globalshared.BranchId_G);
            return Ok(costCenters);
        }
        [HttpGet("GetBranch_CostcenterByBranchId")]
        public IActionResult GetBranch_CostcenterByBranchId(int Branchid)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var costCenters = _CostCenterservice.GetBranch_Costcenter(_globalshared.Lang_G, _globalshared.BranchId_G);
            return Ok(costCenters);
        }
        [HttpGet("FillCostCenterSelectWithCode")]
        public IActionResult FillCostCenterSelectWithCode()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var costCenters = _CostCenterservice.GetAllCostCenters("", _globalshared.Lang_G, _globalshared.BranchId_G).Result.Select(s => new {
                Id = s.Code,
                Name = s.Code + " - " + s.CostCenterName
            });
            return Ok(costCenters);
        }
        [HttpGet("GetAllCostCentersTransactions")]
        public IActionResult GetAllCostCentersTransactions(string FromDate, string ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_CostCenterservice.GetAllCostCentersTransactions(FromDate, ToDate, _globalshared.Lang_G, _globalshared.BranchId_G, _globalshared.YearId_G));
        }
        [HttpGet("GetCostCenterReport")]
        public IActionResult GetCostCenterReport(string FromDate, string ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_CostCenterservice.GetCostCenterReport(_globalshared.BranchId_G, _globalshared.Lang_G, FromDate, ToDate, _globalshared.YearId_G));
        }
        [HttpPost("GetAllCostCenterTrans")]
        public IActionResult GetAllCostCenterTrans([FromForm]int CostCenterId, [FromForm] string? FromDate, [FromForm] string? ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_CostCenterservice.GetCostCenterTransaction(_globalshared.BranchId_G, _globalshared.Lang_G, CostCenterId, FromDate??"", ToDate??"", _globalshared.YearId_G));
        }
        [HttpGet("GetCostCenterAccountTransaction")]
        public IActionResult GetCostCenterAccountTransaction(int CostCenterId, string FromDate, string ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_CostCenterservice.GetCostCenterAccountTransaction(_globalshared.BranchId_G, _globalshared.Lang_G, CostCenterId, FromDate, ToDate, _globalshared.YearId_G));
        }
        [HttpGet("FillAllCostCenterSelect")]
        public IActionResult FillAllCostCenterSelect(int? Param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var costCenters = _CostCenterservice.GetAllCostCentersByCustId(_globalshared.Lang_G, _globalshared.BranchId_G, Param).Result.Select(s => new {
                Id = s.CostCenterId,
                Name = s.Code + " - " + s.CostCenterName
            });
            return Ok(costCenters);

        }
        //public ActionResult TreeViewOfCostCenterReport()
        //{
        //    HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        //    int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);
        //    string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
        //    DataTable CostCenters = _CostCenterservice.TreeViewOfCostCenter(Con).Result;
        //    ReportPDF = ReportsOf7sabat.PrintTreeViewOfCostCenterReport(CostCenters, infoDoneTasksReport);
        //    string existTemp = HttpContext.Server.MapPath(@"~\TempFiles\");

        //    if (!Directory.Exists(existTemp))
        //    {
        //        Directory.CreateDirectory(existTemp);
        //    }
        //    //File  
        //    string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
        //    string FilePath = HttpContext.Server.MapPath(@"~\TempFiles\") + FileName;

        //    //create and set PdfReader  
        //    System.IO.File.WriteAllBytes(FilePath, ReportPDF);
        //    //return file 
        //    string FilePathReturn = @"TempFiles/" + FileName;
        //    return Content(FilePathReturn);
        //}
        [HttpPost("CostCenterMovementReport")]

        public IActionResult CostCenterMovementReport([FromForm]int CostCenterId, [FromForm] string? CostCenterName, [FromForm] string? FromDate, [FromForm] string? ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            CostCentersPrintVM _costCentersPrintVM = new CostCentersPrintVM();
            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
            List<CostCentersVM> CostCenters = _CostCenterservice.GetCostCenterTransaction(_globalshared.BranchId_G, _globalshared.Lang_G, CostCenterId, FromDate??"", ToDate??"", _globalshared.YearId_G).Result.ToList();

            _costCentersPrintVM.result = CostCenters;
            _costCentersPrintVM.StartDate = FromDate;
            _costCentersPrintVM.EndDate = ToDate;
            _costCentersPrintVM.CostCenterName = CostCenterName;
            _costCentersPrintVM.DateTimeNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
            _costCentersPrintVM.Org_VD = objOrganization2;
            var branch = _BranchesService.GetBranchById(_globalshared.BranchId_G).Result;
            _costCentersPrintVM.BranchName = branch.NameAr;
            return Ok(_costCentersPrintVM);
        }

    }
    public class LoadCostCenterData
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }

    }
}

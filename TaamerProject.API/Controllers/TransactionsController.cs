using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Require2FA")]

    public class TransactionsController : ControllerBase
    {
        private ITransactionsService _TransactionsService;
        private IBranchesService _BranchesService;
        private IOrganizationsService _organizationsservice;
        private IFiscalyearsService _fiscalyearsservice;
        private IAccountsService _accountsService;
        private byte[] ReportPDF;
        private string? Con;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;

        public TransactionsController(ITransactionsService transactionsService, IBranchesService branchesService
            ,IOrganizationsService organizationsservice, IFiscalyearsService fiscalyearsservice
            , IAccountsService accountsService, IConfiguration _configuration)
        {
            _TransactionsService = transactionsService;
            _BranchesService = branchesService;
            _organizationsservice = organizationsservice;
            _fiscalyearsservice = fiscalyearsservice;
            _accountsService = accountsService;
            HttpContext httpContext = HttpContext;_globalshared = new GlobalShared(httpContext);
            Configuration = _configuration;Con = this.Configuration.GetConnectionString("DBConnection");
        }
        [HttpPost("GetValueNeeded")]

        public IActionResult GetValueNeeded([FromForm] string? FromDate, [FromForm] string? ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _TransactionsService.GetValueNeeded(_globalshared.BranchId_G, _globalshared.Lang_G, FromDate??"", ToDate??"", Con??"", _globalshared.YearId_G);
            return Ok(result);
        }
        [HttpGet("GetTransbyid")]

        public IActionResult GetTransbyid(int jornal)
        {
            var result = _TransactionsService.gettransbyid(jornal);
            var any = result;
            return Ok(result);
        }

        [HttpGet("GetAllTransByAccountId")]

        public IActionResult GetAllTransByAccountId(int? AccountId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_TransactionsService.GetAllTransByAccountId(AccountId, "", "", _globalshared.YearId_G).Result);
        }
        [HttpGet("GetAllTransactionsSearch")]

        public IActionResult GetAllTransactionsSearch(TransactionsVM TransactionsSearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_TransactionsService.GetAllTransactionsSearch(TransactionsSearch, _globalshared.BranchId_G, _globalshared.YearId_G).Result);
        }
        [HttpGet("GetAllTransSearch")]

        public IActionResult GetAllTransSearch(int? AccountId, string? FromDate, string? ToDate, int? CostCenterId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_TransactionsService.GetAllTransSearch(AccountId, FromDate??"", ToDate??"", CostCenterId, _globalshared.BranchId_G, _globalshared.YearId_G).Result);
        }
        [HttpPost("GetAllTransSearch_New")]

        public IActionResult GetAllTransSearch_New([FromForm] int? AccountId, [FromForm] string? FromDate, [FromForm] string? ToDate, [FromForm] int? CostCenterId, [FromForm] bool? isCheckedYear, [FromForm] bool? isCheckedBranch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int YearIDCheck = _globalshared.YearId_G;
            if(isCheckedYear == true) YearIDCheck = 0;

            return Ok(_TransactionsService.GetAllTransSearch_New(AccountId, FromDate??"", ToDate??"", CostCenterId, _globalshared.BranchId_G, YearIDCheck, isCheckedBranch).Result);
        }
        [HttpGet("GetAllTransSearchByAccIDandCostId")]

        public IActionResult GetAllTransSearchByAccIDandCostId(int? AccountId, string? FromDate, string? ToDate, int? CostCenterId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_TransactionsService.GetAllTransSearchByAccIDandCostId(AccountId, FromDate??"", ToDate ?? "", CostCenterId, _globalshared.BranchId_G, _globalshared.YearId_G).Result);
        }
        [HttpGet("GetAllTransSearchByAccIDandCostId_New")]

        public IActionResult GetAllTransSearchByAccIDandCostId_New(int? AccountId, string? FromDate, string? ToDate, int? CostCenterId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_TransactionsService.GetAllTransSearchByAccIDandCostId_New(AccountId, FromDate ?? "", ToDate ?? ""  , CostCenterId, _globalshared.BranchId_G, _globalshared.YearId_G).Result);
        }
        
        [HttpPost("GetReportGrid")]
        public ActionResult GetReportGrid([FromForm]int? AccountId, [FromForm] string? FromDate, [FromForm] string? ToDate, [FromForm] int? CostCenterId, [FromForm] string? RasedBefore, [FromForm] string[] Sortedlist, [FromForm] bool? isCheckedYear, [FromForm] bool? isCheckedBranch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            ReportGridVM _reportGrid = new ReportGridVM();
            int YearIDCheck = _globalshared.YearId_G;
            if (isCheckedYear == true) YearIDCheck = 0;
            var transactions = _TransactionsService.GetAllTransSearch_New(AccountId, FromDate??"", ToDate??"", CostCenterId, _globalshared.BranchId_G, YearIDCheck, isCheckedBranch).Result.ToList();
            var account = _accountsService.GetAccountById(AccountId??0).Result;
            string s = Sortedlist[0];
            string[] values = s.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            List<string> vale = new List<string>();
            List<TransactionsVM> _TransactionsVM = new List<TransactionsVM>();
            foreach (var item in values)
            {
                string Intitem = string.Empty;
                for (int i = 0; i < item.Length; i++)
                {
                    if (Char.IsDigit(item[i]))
                        Intitem += item[i];
                }
                vale.Add(Intitem);
            }
            int GridLength = 0;

            GridLength = transactions.Count();
            for (int i = 0; i < GridLength; i++)
            {
                _TransactionsVM.Add(transactions.Where(d => d.TransactionId == Convert.ToInt32(vale[i])).FirstOrDefault());
            }
            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

            Decimal result = Decimal.Parse(RasedBefore??"0", CultureInfo.InvariantCulture.NumberFormat);

            _reportGrid.TransactionVM= _TransactionsVM.ToList();
            _reportGrid.StartDate = FromDate;
            _reportGrid.EndDate = ToDate;
            _reportGrid.RasedBefore = RasedBefore;
            _reportGrid.Result = result;
            _reportGrid.AccountName = account!=null? account.NameAr:"";
            _reportGrid.AccountCode = account != null ? account.Code:"";
            _reportGrid.DateTimeNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

            var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
            var branch = _BranchesService.GetBranchById(_globalshared.BranchId_G).Result;
            _reportGrid.BranchName=branch.NameAr;
            _reportGrid.Org_VD = objOrganization2;
            return Ok(_reportGrid);
        }


        [HttpPost("GetReportGrid_Customer")]
        public ActionResult GetReportGrid_Customer([FromBody]reportParameters reportParameters)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            ReportGridVM _reportGrid = new ReportGridVM();
            var FiscalId = _globalshared.YearId_G;
            var YearNEW = _globalshared.YearId_G;// Convert.ToInt32(_fiscalyearsservice.GetYearID(Convert.ToInt32(FiscalId)));
            //reportParameters.FromDate = reportParameters.FromDate == "" ? null : reportParameters.FromDate;
            //reportParameters.ToDate = reportParameters.ToDate == "" ? null : reportParameters.ToDate;
            var transactions = //_TransactionsService.GetAllTransSearch_New(reportParameters.AccountId, reportParameters.FromDate , reportParameters.ToDate, reportParameters.CostCenterId, _globalshared.BranchId_G, YearNEW).Result.ToList();
            _TransactionsService.GetAllTransSearch_New(reportParameters.AccountId, reportParameters.FromDate ?? "", reportParameters.ToDate ?? "", null, _globalshared.BranchId_G, _globalshared.YearId_G,false).Result;
            var account = _accountsService.GetAccountById((int)reportParameters.AccountId).Result;
            string s = reportParameters.Sortedlist[0];
            string[] values = s.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            List<string> vale = new List<string>();
            List<TransactionsVM> _TransactionsVM = new List<TransactionsVM>();
            foreach (var item in values)
            {
                string Intitem = string.Empty;
                for (int i = 0; i < item.Length; i++)
                {
                    if (Char.IsDigit(item[i]))
                        Intitem += item[i];
                }
                vale.Add(Intitem);
            }
            int GridLength = 0;

            GridLength = transactions.Count();
            //for (int i = 0; i < GridLength; i++)
            //{
            //    _TransactionsVM.Add(transactions.Where(d => d.TransactionId == Convert.ToInt32(vale[i])).FirstOrDefault());
            //}
            _TransactionsVM = transactions.ToList();

            Decimal result = Decimal.Parse(reportParameters.RasedBefore ?? "0", CultureInfo.InvariantCulture.NumberFormat);
            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };


            _reportGrid.TransactionVM = _TransactionsVM.ToList();
            _reportGrid.StartDate = reportParameters.FromDate;
            _reportGrid.EndDate = reportParameters.ToDate;
            _reportGrid.RasedBefore = reportParameters.RasedBefore;
            _reportGrid.Result = result;
            _reportGrid.AccountName = account.NameAr;
            _reportGrid.AccountCode = account.Code;
            _reportGrid.DateTimeNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

            var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
            var branch = _BranchesService.GetBranchById(_globalshared.BranchId_G).Result;
            _reportGrid.BranchName = branch.NameAr;
            _reportGrid.Org_VD = objOrganization2;

            decimal Totalres = 0;
            decimal ResultCre = 0;
            decimal ResultDep = 0;

            foreach (var item in _TransactionsVM)
            {
                Totalres = Totalres + (Decimal.Parse((item.Depit != null ? item.Depit : 0).ToString()) - Decimal.Parse((item.Credit != null ? item.Credit : 0).ToString()));
                ResultCre = ResultCre + Decimal.Parse((item.Credit != null ? item.Credit : 0).ToString());
                ResultDep = ResultDep + Decimal.Parse((item.Depit != null ? item.Depit : 0).ToString());
                item.TotalRes = Totalres.ToString();
                
            }
            _reportGrid.Totalres = Totalres.ToString();
            _reportGrid.ResultCre = ResultCre.ToString();
            _reportGrid.ResultDep = ResultDep.ToString();

            return Ok(_reportGrid);
        }
       
        [HttpGet("GetAccountStatmentDGV")]

        public IActionResult GetAccountStatmentDGV(string? FromDate, string? ToDate, string AccountId, string CostCenterId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _TransactionsService.GetFullAccountStatmentDGV(FromDate??"", ToDate?? "", AccountId, CostCenterId, Con??"", _globalshared.BranchId_G, _globalshared.YearId_G).Result;
            return Ok(result);
        }
        [HttpPost("PrintAddtionalTaxesPDFFile")]
        public ActionResult PrintAddtionalTaxesPDFFile([FromForm]string? FromDate, [FromForm] string? ToDate, [FromForm] int? DateId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            ValueNeededVM _valueNeededVM = new ValueNeededVM();
            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
            List<double> result = _TransactionsService.GetValueNeeded(_globalshared.BranchId_G, _globalshared.Lang_G, FromDate??"", ToDate??"", Con ?? "", _globalshared.YearId_G).Result;
            //ReportPDF = ReportsOf7sabat.ValueAdded(result, FromDate, ToDate, infoDoneTasksReport, _globalshared.YearId_G, DateId);

            _valueNeededVM.Result= result;
            _valueNeededVM.StartDate = FromDate;
            _valueNeededVM.EndDate = ToDate;

            string rob3 = "";
            if (DateId != null && DateId != 0)
            {

                if (DateId == 1)
                {
                    rob3 = "الاول";
                }
                else if (DateId == 2)
                {
                    rob3 = "الثاني";
                }
                else if (DateId == 3)
                {
                    rob3 = "الثالث";
                }
                else if (DateId == 4)
                {
                    rob3 = "الرابع";
                }

            }
            _valueNeededVM.DateTimeNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

            _valueNeededVM.rob3 = rob3;
            var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
            _valueNeededVM.Org_VD = objOrganization2;
            var branch = _BranchesService.GetBranchById(_globalshared.BranchId_G).Result;
            _valueNeededVM.BranchName = branch.NameAr;
            return Ok(_valueNeededVM);
        }
        [HttpGet("GetAccCredit_Depit")]

        public IActionResult GetAccCredit_Depit(int AccountId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var SelectStetment = "select ISNULL(sum(acc.Credit),0),ISNULL(sum(acc.Depit),0)  from Acc_Transactions acc left join Acc_Invoices Inv on acc.InvoiceId=Inv.InvoiceId"
               + " where acc.IsDeleted = 0 and acc.IsDeleted = 0 and acc.IsPost = 1 and acc.AccountId = " + AccountId + " and acc.YearId = " + _globalshared.YearId_G + " and acc.Type not in(12)";
            var GetAccCredit_Depit = _TransactionsService.GetAccCredit_Depit(Con ?? "", SelectStetment);
            return Ok(GetAccCredit_Depit.FirstOrDefault());
        }

    }
    public class reportParameters
    {
        public int? AccountId { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public int? CostCenterId { get; set; }
        public string? RasedBefore { get; set; }
        public string[]? Sortedlist { get; set; }
    }
}

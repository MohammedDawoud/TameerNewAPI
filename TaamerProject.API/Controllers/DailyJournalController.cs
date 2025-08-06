using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;
using TaamerProject.API.Helper;
using TaamerProject.API.pdfHandler;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;
using static TaamerProject.API.Controllers.TransactionsController;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class DailyJournalController : ControllerBase
    {

  
            // GET: DailyJournal
            private IVoucherService _Voucherservice;
            private IAccountsService _accountsService;
            private IBranchesService _branchesService;
            private IOrganizationsService _organizationsservice;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        string Con;
        byte[] ReportPDF;
        public DailyJournalController(IVoucherService voucherService, IAccountsService accountsService, IBranchesService branchesService, IOrganizationsService organizationsService
                ,IConfiguration _configuration)
        {
                this._Voucherservice = voucherService;
                this._accountsService = accountsService;
                this._branchesService = branchesService;
                this._organizationsservice = organizationsService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

        }
        [HttpPost("GetAllJournals")]
        public IActionResult GetAllJournals([FromForm]  int? FromJournal, [FromForm] int? ToJournal, [FromForm] string? FromDate, [FromForm] string? ToDate)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var someVoucher = _Voucherservice.GetAllJournals(FromJournal, ToJournal, FromDate??"", ToDate??"", _globalshared.BranchId_G, _globalshared.YearId_G);
                return Ok(someVoucher);
                //return Ok(_Voucherservice.GetAllJournals(FromJournal, ToJournal, FromDate, ToDate,_globalshared.BranchId_G,_globalshared.YearId_G));
        }

        [HttpGet("GetAllTotalJournals")]
        public IActionResult GetAllTotalJournals(int? FromJournal, int? ToJournal, string FromDate, string ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            var someVoucher = _Voucherservice.GetAllTotalJournals(FromJournal, ToJournal, FromDate, ToDate,_globalshared.BranchId_G,_globalshared.YearId_G);
            return Ok(someVoucher);
            //return Ok(_Voucherservice.GetAllTotalJournals(FromJournal, ToJournal, FromDate, ToDate,_globalshared.BranchId_G,_globalshared.YearId_G));
        }

        [HttpGet("GetAllJournalsByInvID")]
        public IActionResult GetAllJournalsByInvID(int? invId)
        {
        HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


        return Ok(_Voucherservice.GetAllJournalsByInvID(invId,_globalshared.BranchId_G,_globalshared.YearId_G));
        }

        [HttpGet("GetAllJournalsByInvIDPurchase")]
        public IActionResult GetAllJournalsByInvIDPurchase(int? invId)
        {
        HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


        return Ok(_Voucherservice.GetAllJournalsByInvIDPurchase(invId,_globalshared.BranchId_G,_globalshared.YearId_G));
        }
        [HttpGet("GetAllJournalsByInvIDPurchaseOrder")]
        public IActionResult GetAllJournalsByInvIDPurchaseOrder(int? invId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            return Ok(_Voucherservice.GetAllJournalsByInvIDPurchaseOrder(invId, _globalshared.BranchId_G, _globalshared.YearId_G));
        }

        [HttpGet("GetAllJournalsByReVoucherID")]
        public IActionResult GetAllJournalsByReVoucherID(int? invId)
        {
        HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


        return Ok(_Voucherservice.GetAllJournalsByReVoucherID(invId,_globalshared.BranchId_G,_globalshared.YearId_G));
        }

        [HttpGet("GetAllJournalsByPayVoucherID")]
        public IActionResult GetAllJournalsByPayVoucherID(int? invId)
        {
        HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


        return Ok(_Voucherservice.GetAllJournalsByPayVoucherID(invId,_globalshared.BranchId_G,_globalshared.YearId_G));
        }

        [HttpGet("GetAllJournalsByDailyID")]
        public IActionResult GetAllJournalsByDailyID(int? invId)
        {
        HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


        return Ok(_Voucherservice.GetAllJournalsByDailyID(invId,_globalshared.BranchId_G,_globalshared.YearId_G));
        }


        [HttpGet("GetAllJournalsByDailyID_Custody")]
        public IActionResult GetAllJournalsByDailyID_Custody(int? invId)
        {

        HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

        return Ok(_Voucherservice.GetAllJournalsByDailyID_Custody(invId,_globalshared.BranchId_G,_globalshared.YearId_G));
        }

        [HttpGet("GetAllJournalsByClosingID")]
        public IActionResult GetAllJournalsByClosingID(int? invId)
        {
        HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


        return Ok(_Voucherservice.GetAllJournalsByClosingID(invId,_globalshared.BranchId_G,_globalshared.YearId_G));
        }
        [HttpGet("PrintJournalsVyInvId")]

        public IActionResult PrintJournalsVyInvId(int? InvId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;

            List<TransactionsVM> dt = _Voucherservice.GetAllJournalsByInvID(InvId, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            ReportPDF = inVoiceReports.GetAllJournalsByInvID(dt, objOrganization);

            string existTemp = System.IO.Path.Combine("TempFiles/");
            if (!Directory.Exists(existTemp))
            {
                Directory.CreateDirectory(existTemp);
            }
            string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
            string FilePath = System.IO.Path.Combine("TempFiles/", FileName);
            System.IO.File.WriteAllBytes(FilePath, ReportPDF);
            string FilePathReturn = "/TempFiles/" + FileName;
            return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = FilePathReturn });

        }
        [HttpGet("PrintJournalsVyInvIdPurchase")]

        public IActionResult PrintJournalsVyInvIdPurchase(int? InvId)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;

            List<TransactionsVM> dt = _Voucherservice.GetAllJournalsByInvIDPurchase(InvId, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            ReportPDF = inVoiceReports.GetAllJournalsByInvID(dt, objOrganization);

            string existTemp = System.IO.Path.Combine("TempFiles/");
            if (!Directory.Exists(existTemp))
            {
                Directory.CreateDirectory(existTemp);
            }
            string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
            string FilePath = System.IO.Path.Combine("TempFiles/", FileName);
            System.IO.File.WriteAllBytes(FilePath, ReportPDF);
            string FilePathReturn = "/TempFiles/" + FileName;
            return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = FilePathReturn });
        }
        [HttpGet("PrintJournalsVyInvIdRet")]

        public IActionResult PrintJournalsVyInvIdRet(int? InvId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;

            List<TransactionsVM> dt = _Voucherservice.GetAllJournalsByInvIDRet(InvId, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            ReportPDF = inVoiceReports.GetAllJournalsByInvID(dt, objOrganization);

            string existTemp = System.IO.Path.Combine("TempFiles/");
            if (!Directory.Exists(existTemp))
            {
                Directory.CreateDirectory(existTemp);
            }
            string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
            string FilePath = System.IO.Path.Combine("TempFiles/", FileName);
            System.IO.File.WriteAllBytes(FilePath, ReportPDF);
            string FilePathReturn = "/TempFiles/" + FileName;
            return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = FilePathReturn });
        }
        [HttpGet("PrintJournalsVyInvIdRetPurchase")]

        public IActionResult PrintJournalsVyInvIdRetPurchase(int? InvId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;

            List<TransactionsVM> dt = _Voucherservice.GetAllJournalsByInvIDRetPurchase(InvId, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            ReportPDF = inVoiceReports.GetAllJournalsByInvID(dt, objOrganization);

            string existTemp = System.IO.Path.Combine("TempFiles/");
            if (!Directory.Exists(existTemp))
            {
                Directory.CreateDirectory(existTemp);
            }
            string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
            string FilePath = System.IO.Path.Combine("TempFiles/", FileName);
            System.IO.File.WriteAllBytes(FilePath, ReportPDF);
            string FilePathReturn = "/TempFiles/" + FileName;
            return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = FilePathReturn });
        }

        [HttpPost("PrintJournalsByReVoucherId")]

        public IActionResult PrintJournalsByReVoucherId(int? InvId)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;

            List<TransactionsVM> dt = _Voucherservice.GetAllJournalsByReVoucherID(InvId, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            ReportPDF = inVoiceReports.GetAllJournalsByInvID(dt, objOrganization);

            string existTemp = System.IO.Path.Combine("TempFiles/");
            if (!Directory.Exists(existTemp))
            {
                Directory.CreateDirectory(existTemp);
            }
            string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
            string FilePath = System.IO.Path.Combine("TempFiles/", FileName);
            System.IO.File.WriteAllBytes(FilePath, ReportPDF);
            string FilePathReturn = "/TempFiles/" + FileName;
            return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = FilePathReturn });
        }


        [HttpPost("PrintJournalsByPayVoucherId")]

        public IActionResult PrintJournalsByPayVoucherId(int? InvId)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;

            List<TransactionsVM> dt = _Voucherservice.GetAllJournalsByPayVoucherID(InvId, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            ReportPDF = inVoiceReports.GetAllJournalsByInvID(dt, objOrganization);

            string existTemp = System.IO.Path.Combine("TempFiles/");
            if (!Directory.Exists(existTemp))
            {
                Directory.CreateDirectory(existTemp);
            }
            string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
            string FilePath = System.IO.Path.Combine("TempFiles/", FileName);
            System.IO.File.WriteAllBytes(FilePath, ReportPDF);
            string FilePathReturn = "/TempFiles/" + FileName;
            return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = FilePathReturn });
        }

        [HttpGet("PrintJournalsByDailyId")]

        public IActionResult PrintJournalsByDailyId(int? InvId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;

            List<TransactionsVM> dt = _Voucherservice.GetAllJournalsByDailyID(InvId, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            ReportPDF = inVoiceReports.GetAllJournalsByInvID(dt, objOrganization);

            string existTemp = System.IO.Path.Combine("TempFiles/");
            if (!Directory.Exists(existTemp))
            {
                Directory.CreateDirectory(existTemp);
            }
            string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
            string FilePath = System.IO.Path.Combine("TempFiles/", FileName);
            System.IO.File.WriteAllBytes(FilePath, ReportPDF);
            string FilePathReturn = "/TempFiles/" + FileName;
            return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = FilePathReturn });
        }
        [HttpGet("PrintJournalsByDailyId_Custody")]

        public IActionResult PrintJournalsByDailyId_Custody(int? InvId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;

            List<TransactionsVM> dt = _Voucherservice.GetAllJournalsByDailyID_Custody(InvId, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            ReportPDF = inVoiceReports.GetAllJournalsByInvID(dt, objOrganization);

            string existTemp = System.IO.Path.Combine("TempFiles/");
            if (!Directory.Exists(existTemp))
            {
                Directory.CreateDirectory(existTemp);
            }
            string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
            string FilePath = System.IO.Path.Combine("TempFiles/", FileName);
            System.IO.File.WriteAllBytes(FilePath, ReportPDF);
            string FilePathReturn = "/TempFiles/" + FileName;
            return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = FilePathReturn });
        }

        [HttpGet("PrintJournalsByClosingId")]

        public IActionResult PrintJournalsByClosingId(int? InvId)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;

            List<TransactionsVM> dt = _Voucherservice.GetAllJournalsByClosingID(InvId, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            ReportPDF = inVoiceReports.GetAllJournalsByInvID(dt, objOrganization);

            string existTemp = System.IO.Path.Combine("TempFiles/");
            if (!Directory.Exists(existTemp))
            {
                Directory.CreateDirectory(existTemp);
            }
            string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
            string FilePath = System.IO.Path.Combine("TempFiles/", FileName);
            System.IO.File.WriteAllBytes(FilePath, ReportPDF);
            string FilePathReturn = "/TempFiles/" + FileName;
            return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = FilePathReturn });
        }
        [HttpGet("GetAllJournalsByInvIDRet")]
        public IActionResult GetAllJournalsByInvIDRet(int? invId)
            {


            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_Voucherservice.GetAllJournalsByInvIDRet(invId,_globalshared.BranchId_G,_globalshared.YearId_G));
            }

        [HttpGet("GetAllJournalsByInvIDCreditDepitNoti")]
        public IActionResult GetAllJournalsByInvIDCreditDepitNoti(int? invId)
            {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            return Ok(_Voucherservice.GetAllJournalsByInvIDCreditDepitNoti(invId,_globalshared.BranchId_G,_globalshared.YearId_G));
            }

        [HttpGet("GetAllJournalsByInvIDRetPurchase")]
        public IActionResult GetAllJournalsByInvIDRetPurchase(int? invId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            return Ok(_Voucherservice.GetAllJournalsByInvIDRetPurchase(invId,_globalshared.BranchId_G,_globalshared.YearId_G));
            }

        [HttpGet("GetAllPayJournalsByInvIDRet")]
        public IActionResult GetAllPayJournalsByInvIDRet(int? invId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            return Ok(_Voucherservice.GetAllPayJournalsByInvIDRet(invId,_globalshared.BranchId_G,_globalshared.YearId_G));
            }
        //public IActionResult GetReport(string StartDate, string EndDate, int? fromJournal, int? toJournal)
        //{


        //    var result = _Voucherservice.GetAllTotalJournals(fromJournal, toJournal, StartDate, EndDate, _globalshared.BranchId_G, _globalshared.YearId_G).ToList();
        //    List<TransactionsVM> res = result.ToList();
        //    int orgId = _branchesService.GetOrganizationId(BranchId);
        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);
        //    string[] infoDoneTasksReport = { Lang == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

        //    ReportPDF = Bayanatech.TameerUI.pdfHandler.ReportsOf7sabat.dailyReport(res, StartDate, EndDate, infoDoneTasksReport);//الارصده

        //    string existTemp = System.IO.Path.Combine("TempFiles/");
        //    if (!Directory.Exists(existTemp))
        //    {
        //        Directory.CreateDirectory(existTemp);
        //    }
        //    string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
        //    string FilePath = System.IO.Path.Combine("TempFiles/", FileName);
        //    System.IO.File.WriteAllBytes(FilePath, ReportPDF);
        //    string FilePathReturn = "/TempFiles/" + FileName;
        //    return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = FilePathReturn });
        //}

        ////public IActionResult GetReportGrid(string StartDate, string EndDate, int? fromJournal, int? toJournal, string[] SortedlistJournalNo, string[] SortedlistAccountCode)
        ////{
        ////    //var result2 = _Voucherservice.GetAllTotalJournals(fromJournal, toJournal, StartDate, EndDate,_globalshared.BranchId_G,_globalshared.YearId_G).ToList();
        ////    var result = _Voucherservice.GetAllJournals(fromJournal, toJournal, StartDate, EndDate,_globalshared.BranchId_G,_globalshared.YearId_G);

        ////    List<TransactionsVM> res = result.ToList();
        ////    //result = result.OrderBy(d => Sortedlist.IndexOf(d.InvoiceReference)).ToList();
        ////    string sJournalNo = SortedlistJournalNo[0];
        ////    //string sAccountCode = SortedlistAccountCode[0];
        ////    string[] valuessJournalNo = sJournalNo.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        ////    List<string> valeJournalNo = new List<string>();
        ////    //string[] valuesAccountCode = sAccountCode.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        ////    //List<string> valeAccountCode = new List<string>();
        ////    List<TransactionsVM> _TransactionsVM = new List<TransactionsVM>();
        ////    foreach (var item in valuessJournalNo)
        ////    {
        ////        string Intitem = string.Empty;
        ////        for (int i = 0; i < item.Length; i++)
        ////        {
        ////            if (Char.IsDigit(item[i]))
        ////                Intitem += item[i];
        ////        }
        ////        valeJournalNo.Add(Intitem);
        ////    }
        ////    //foreach (var item in valuesAccountCode)
        ////    //{
        ////    //    string Intitem = string.Empty;
        ////    //    for (int i = 0; i < item.Length; i++)
        ////    //    {
        ////    //        if (Char.IsDigit(item[i]))
        ////    //            Intitem += item[i];
        ////    //    }
        ////    //    valeAccountCode.Add(Intitem);
        ////    //}
        ////    int GridLength = 0;
        ////    GridLength = result.Count();
        ////    //if (result.Count() > 100)
        ////    //{
        ////    //    GridLength = 100;
        ////    //}
        ////    //else
        ////    //{
        ////    //    GridLength = result.Count();
        ////    //}
        ////    for (int i = 0; i < GridLength; i++)
        ////    {
        ////        //_TransactionsVM.Add(result.Where(d => d.JournalNo == Convert.ToInt32(valeJournalNo[i]) && d.AccountCode == valeAccountCode[i]).FirstOrDefault());
        ////        //_TransactionsVM.Add(result.Where(d => d.JournalNo == Convert.ToInt32(valeJournalNo[i]) && d.AccountCode == valeAccountCode[i]).FirstOrDefault());
        ////        _TransactionsVM.Add(result.Where(d => d.TransactionId == Convert.ToInt32(valeJournalNo[i])).FirstOrDefault());

        ////    }
        ////    int orgId = _branchesService.GetOrganizationId(BranchId);
        ////    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);
        ////    string[] infoDoneTasksReport = { Lang == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email,
        ////        objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
        ////    //string taxType = "";
        ////    //if (Convert.ToInt32(ExpenseType) == 2)
        ////    //    taxType = "خاضعة للضريبة";
        ////    //else if (Convert.ToInt32(ExpenseType) == 3)
        ////    //    taxType = "غير خاضعة للضريبة";
        ////    ReportPDF = Bayanatech.TameerUI.pdfHandler.ReportsOf7sabat.dailyReport(_TransactionsVM.ToList(), StartDate, EndDate, infoDoneTasksReport);

        ////    string existTemp = HttpContext.Server.MapPath(@"~\TempFiles\");

        ////    if (!Directory.Exists(existTemp))
        ////    {
        ////        Directory.CreateDirectory(existTemp);
        ////    }
        ////    //File  
        ////    string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
        ////    string FilePath = HttpContext.Server.MapPath(@"~\TempFiles\") + FileName;

        ////    //create and set PdfReader  
        ////    System.IO.File.WriteAllBytes(FilePath, ReportPDF);
        ////    //return file 
        ////    string FilePathReturn = @"TempFiles/" + FileName;
        ////    return Content(FilePathReturn);
        ////}




        [HttpPost("GetReportGrid")]

        public IActionResult GetReportGrid([FromForm] string? StartDate, [FromForm] string? EndDate, [FromForm] int? fromJournal, [FromForm] int? toJournal, [FromForm] string[] SortedlistJournalNo, [FromForm] string[] SortedlistAccountCode)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            ReportGridVM _reportGrid = new ReportGridVM();
            var result = _Voucherservice.GetAllJournals(fromJournal, toJournal, StartDate??"", EndDate??"", _globalshared.BranchId_G, _globalshared.YearId_G).Result;

            List<TransactionsVM> res = result.ToList();
            string sJournalNo = SortedlistJournalNo[0];
            string[] valuessJournalNo = sJournalNo.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            List<string> valeJournalNo = new List<string>();
            List<TransactionsVM> _TransactionsVM = new List<TransactionsVM>();
            foreach (var item in valuessJournalNo)
            {
                string Intitem = string.Empty;
                for (int i = 0; i < item.Length; i++)
                {
                    if (Char.IsDigit(item[i]))
                        Intitem += item[i];
                }
                valeJournalNo.Add(Intitem);
            }
            int GridLength = 0;
            GridLength = result.Count();
            for (int i = 0; i < GridLength; i++)
            {
                _TransactionsVM.Add(result.Where(d => d.TransactionId == Convert.ToInt32(valeJournalNo[i])).FirstOrDefault());
            }
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = {_globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email,
                objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

            _reportGrid.TransactionVM = _TransactionsVM.ToList();
            _reportGrid.StartDate = StartDate;
            _reportGrid.EndDate = EndDate;
            _reportGrid.DateTimeNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")); 

            var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
            var branch = _branchesService.GetBranchById(_globalshared.BranchId_G).Result;
            _reportGrid.BranchName = branch.NameAr;
            _reportGrid.Org_VD = objOrganization2;
            return Ok(_reportGrid);
        }





    }
    
}

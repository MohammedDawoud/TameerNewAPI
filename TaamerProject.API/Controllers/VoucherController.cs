using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Drawing;
using System.Globalization;
using Spire.Doc;
using TaamerProject.Service.Interfaces;
using TaamerProject.Models;
using TaamerProject.API.Helper;
using TaamerProject.Models.Common;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaamerProject.API.pdfHandler;
using static TaamerProject.Models.ReportGridVM;
using TaamerP.Service.LocalResources;
using ZatcaIntegrationSDK;
using ZatcaIntegrationSDK.BLL;
using ZatcaIntegrationSDK.HelperContracts;
using TaamerProject.Models.DBContext;
using TaamerProject.Service.IGeneric;
using ZXing.Common;
using ZXing;
using ZatcaIntegrationSDK.GeneralLogic;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Require2FA")]

    public class VoucherController : ControllerBase
    {
        // GET: Voucher
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly IVoucherService _voucherService;
        private readonly IAcc_InvoicesRequestsService _invoicesRequestsService;

        private readonly IAcc_SuppliersService _acc_SuppliersService;

        private readonly IServicesPriceService _servicesPriceService;
        private readonly IServicesPriceOfferService _servicesPriceOfferService;

        private IBranchesService _BranchesService;
        private IOrganizationsService _organizationsservice;
        private ICostCenterService _CostCenterservice;
        private IProjectService _projectservice;
        private ICustomerService _customerService;
        private ISystemSettingsService _systemSettingsService;
        private readonly IFiscalyearsService _FiscalyearsService;
        private IEmployeesService _employesService;
        private readonly ICustomerSMSService _sMSService;
        private readonly ISystemAction _SystemAction;


        private byte[] ReportPDF;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private string Con;
        private ZatcaIntegrationSDK.APIHelper.Mode mode = ZatcaIntegrationSDK.APIHelper.Mode.developer;
        public VoucherController(TaamerProjectContext dataContext, IVoucherService voucherService, ISystemAction systemAction, IAcc_SuppliersService acc_SuppliersService, IServicesPriceService servicesPriceService, IServicesPriceOfferService servicesPriceOfferService, IBranchesService branchesService,
            IOrganizationsService organizationsService, ICostCenterService costCenterService, IProjectService projectService, ICustomerService customerService,
            IAcc_InvoicesRequestsService InvoicesRequestsService, ISystemSettingsService systemSettingsService, ICustomerSMSService sMSService, IFiscalyearsService fiscalyearsService, IEmployeesService employeesService, IConfiguration _configuration, IWebHostEnvironment webHostEnvironment)
        {
            _SystemAction = systemAction;

            _projectservice = projectService;
            _TaamerProContext = dataContext;
            this._BranchesService = branchesService;
            this._organizationsservice = organizationsService;
            _voucherService = voucherService;
            _invoicesRequestsService = InvoicesRequestsService;

            _acc_SuppliersService = acc_SuppliersService;
            this._customerService = customerService;
            this._systemSettingsService = systemSettingsService;
            this._FiscalyearsService = fiscalyearsService;
            this._employesService = employeesService;
            _sMSService = sMSService;
            this._CostCenterservice = costCenterService;
            _servicesPriceService = servicesPriceService;
            _servicesPriceOfferService = servicesPriceOfferService;
            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");
            HttpContext httpContext = HttpContext;

            _globalshared = new GlobalShared(httpContext);
            _hostingEnvironment = webHostEnvironment;
        }

        //اخر سند قبض
        [HttpPost("GetLastVouchersre")]
        public IActionResult GetLastVouchersre(VoucherFilterVM voucherFilterVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var someVoucher = _voucherService.GetAllVouchers(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            var lastinvoice = someVoucher.Max(p => p.InvoiceId);
            var lastone = _voucherService.GetVoucherById(lastinvoice);
            return Ok(lastone);
        }
        [HttpPost("GetAllVouchersNew")]
        public IActionResult GetAllVouchersNew(VoucherFilterVM voucherFilterVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someVoucher = _voucherService.GetAllVouchers(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            return Ok(someVoucher);

        }
        public class InvoicemailWhatsapp
        {
            public int InvoiceId { get; set; }
            public string? Notes { get; set; }

            public string? AttachmentFile { get; set; }
        }
        [HttpPost("SendWInvoice2")]
        public IActionResult SendWInvoice2(IFormFile? UploadedFile, [FromForm] int InvoiceId, [FromForm] string? Notes, [FromForm] string? environmentURL, [FromForm] string? fileTypeUpload)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int ReportType = 4;
            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;

            DataTable voucher = _voucherService.ReceiptCashingPaying(InvoiceId, Con).Result;
            InvoicesVM InvoicesVM = _voucherService.GetVoucherById(InvoiceId).Result;
            //costCenterNam = _CostCenterservice.GetCostCenterByProId(InvoicesVM.ProjectId.Value);

            var costCenterNam = "";
            if (InvoicesVM.VoucherDetails[0].CostCenterId != null)
            {

                costCenterNam = _CostCenterservice.GetCostCenterById(InvoicesVM.VoucherDetails[0].CostCenterId ?? 0).Result.NameAr;
            }
            else
            {
                costCenterNam = "بدون";
            }
            var ReqInv = _invoicesRequestsService.GetInvoiceReq(InvoiceId).Result;

            List<VoucherDetailsVM> VoucherDetailsVM = _voucherService.GetAllDetailsByInvoiceId(InvoiceId).Result.ToList();
            if (ReportType == 1)//النموذج الاول للفاتورة
                ReportPDF = inVoiceReports.GenInvoice(VoucherDetailsVM, objOrganization, InvoicesVM, voucher);
            else if (ReportType == 2)///النموذج التانى للفاتورة
                ReportPDF = inVoiceReports.GenInvoice2(VoucherDetailsVM, objOrganization, InvoicesVM, voucher);
            else if (ReportType == 4)///فاتورة صغيره
                ReportPDF = inVoiceReports.GenInvoiceN_S(VoucherDetailsVM, objOrganization, InvoicesVM, voucher, costCenterNam ?? "", ReqInv != null ? ReqInv.QRCode : null);
            else //New
                ReportPDF = inVoiceReports.GenInvoiceN(VoucherDetailsVM, objOrganization, InvoicesVM, voucher, costCenterNam ?? "");

            //dawoudprint
            string existTemp = System.IO.Path.Combine("TempFiles/");
            if (!Directory.Exists(existTemp))
            {
                Directory.CreateDirectory(existTemp);
            }
            string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
            string FilePath = System.IO.Path.Combine("TempFiles/", FileName);
            System.IO.File.WriteAllBytes(FilePath, ReportPDF);
            string FilePathReturn = "/TempFiles/" + FileName;




            var FileUrl = "";

            if (UploadedFile != null)
            {
                string path = System.IO.Path.Combine("TempFiles/");
                string pathW = System.IO.Path.Combine("/TempFiles/");

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                List<string> uploadedFiles = new List<string>();
                string pathes = "";
                string fileName = System.IO.Path.GetFileName(Guid.NewGuid() + UploadedFile.FileName);
                var path2 = Path.Combine(path, fileName);
                if (System.IO.File.Exists(path2))
                {
                    System.IO.File.Delete(path2);
                }
                using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(path, fileName), System.IO.FileMode.Create))
                {
                    UploadedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    pathes = pathW + fileName;
                }


                if (pathes != "")
                {
                    FileName = fileName;
                    FileUrl = "/TempFiles/" + fileName;
                }
            }
            var FileAtt = "";
            if (fileTypeUpload=="2")
            {
                FileAtt = FilePathReturn;
            }
            else
            {
                FileAtt = FileUrl;
            }


            //FilePathReturn

            var result = _voucherService.SendWInvoice(InvoiceId, _globalshared.UserId_G, _globalshared.BranchId_G, FileAtt, environmentURL??"", fileTypeUpload??"");
            return Ok(result);
        }

        [HttpPost("SendWInvoice")]
        public IActionResult SendWInvoice(IFormFile? UploadedFile, [FromForm] int InvoiceId, [FromForm] string? Notes, [FromForm] string? environmentURL, [FromForm] string? fileTypeUpload)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int ReportType = 4;
            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;

            DataTable voucher = _voucherService.ReceiptCashingPaying(InvoiceId, Con).Result;
            InvoicesVM InvoicesVM = _voucherService.GetVoucherById(InvoiceId).Result;
            //costCenterNam = _CostCenterservice.GetCostCenterByProId(InvoicesVM.ProjectId.Value);

            var costCenterNam = "";
            if (InvoicesVM.VoucherDetails[0].CostCenterId != null)
            {

                costCenterNam = _CostCenterservice.GetCostCenterById(InvoicesVM.VoucherDetails[0].CostCenterId ?? 0).Result.NameAr;
            }
            else
            {
                costCenterNam = "بدون";
            }
            var ReqInv = _invoicesRequestsService.GetInvoiceReq(InvoiceId).Result;

            List<VoucherDetailsVM> VoucherDetailsVM = _voucherService.GetAllDetailsByInvoiceId(InvoiceId).Result.ToList();
            if (ReportType == 1)//النموذج الاول للفاتورة
                ReportPDF = inVoiceReports.GenInvoice(VoucherDetailsVM, objOrganization, InvoicesVM, voucher);
            else if (ReportType == 2)///النموذج التانى للفاتورة
                ReportPDF = inVoiceReports.GenInvoice2(VoucherDetailsVM, objOrganization, InvoicesVM, voucher);
            else if (ReportType == 4)///فاتورة صغيره
                ReportPDF = inVoiceReports.GenInvoiceN_S(VoucherDetailsVM, objOrganization, InvoicesVM, voucher, costCenterNam ?? "", ReqInv != null ? ReqInv.QRCode : null);
            else //New
                ReportPDF = inVoiceReports.GenInvoiceN(VoucherDetailsVM, objOrganization, InvoicesVM, voucher, costCenterNam ?? "");

            //dawoudprint
            string existTemp = System.IO.Path.Combine("TempFiles/");
            if (!Directory.Exists(existTemp))
            {
                Directory.CreateDirectory(existTemp);
            }
            string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
            string FilePath = System.IO.Path.Combine("TempFiles/", FileName);
            System.IO.File.WriteAllBytes(FilePath, ReportPDF);
            string FilePathReturn = "/TempFiles/" + FileName;




            var FileUrl = "";

            if (UploadedFile != null)
            {
                string path = System.IO.Path.Combine("TempFiles/");
                string pathW = System.IO.Path.Combine("/TempFiles/");

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                List<string> uploadedFiles = new List<string>();
                string pathes = "";
                string fileName = System.IO.Path.GetFileName(Guid.NewGuid() + UploadedFile.FileName);
                var path2 = Path.Combine(path, fileName);
                if (System.IO.File.Exists(path2))
                {
                    System.IO.File.Delete(path2);
                }
                using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(path, fileName), System.IO.FileMode.Create))
                {
                    UploadedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    pathes = pathW + fileName;
                }


                if (pathes != "")
                {
                    FileName = fileName;
                    FileUrl = "/TempFiles/" + fileName;
                }
            }
            var FileAtt = "";
            if (fileTypeUpload == "2")
            {
                FileAtt = FilePathReturn;
            }
            else
            {
                FileAtt = FileUrl;
            }


            //FilePathReturn
            var Message = Notes;
            //var result2 = _voucherService.SendWInvoice(InvoiceId, _globalshared.UserId_G, _globalshared.BranchId_G, FileAtt, environmentURL ?? "", fileTypeUpload ?? "");
            var result = _sMSService.SendWhatsApp_Notification(InvoicesVM.CustomerMobile ?? "", Message??"", _globalshared.UserId_G, _globalshared.BranchId_G, environmentURL??"", FileAtt);
            return Ok(result);
        }




        [HttpGet("GetInvoiceByCustomer")]
        public IActionResult GetInvoiceByCustomer(int CustomerId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someVoucher = _voucherService.GetInvoiceByCustomer(CustomerId, _globalshared.YearId_G).Result.ToList();
            return Ok(someVoucher);

        }

        [HttpGet("GetInvoiceByNo")]
        public IActionResult GetInvoiceByNo(string VoucherNo)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someVoucher = _voucherService.GetInvoiceByNo(VoucherNo, _globalshared.YearId_G).Result;
            return Ok(someVoucher);

        }

        [HttpGet("GetInvoiceByNo_purches")]
        public IActionResult GetInvoiceByNo_purches(string VoucherNo)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someVoucher = _voucherService.GetInvoiceByNo_purches(VoucherNo, _globalshared.YearId_G).Result;
            return Ok(someVoucher);

        }

        [HttpPost("GetAllVouchers")]
        public IActionResult GetAllVouchers(VoucherFilterVM voucherFilterVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            decimal TotalCredit = 0;
            decimal TotalDepit = 0;


            var someVoucher = _voucherService.GetAllVouchers(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            if (someVoucher.Count() > 0)
            {
                foreach (var item in someVoucher)
                {
                    TotalCredit = 0;
                    TotalDepit = 0;

                    TotalCredit = GetAllCreditNotiTotalValue(item.InvoiceId).Item1;
                    TotalDepit = GetAllCreditNotiTotalValue(item.InvoiceId).Item2;

                    item.CreditNotiTotal = TotalCredit;
                    item.DepitNotiTotal = TotalDepit;

                }
            }
            return Ok(someVoucher);

        }
        [HttpPost("GetAllVouchersLastMonth")]
        public IActionResult GetAllVouchersLastMonth(VoucherFilterVM voucherFilterVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string DateFrom = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            string DateTo = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            if (!(voucherFilterVM.IsSearch??false))
            {
                voucherFilterVM.dateFrom = DateFrom;
                voucherFilterVM.dateTo = DateTo;
            }


            var someVoucher = _voucherService.GetAllVouchersLastMonth(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            return Ok(someVoucher);
        }
        [HttpPost("GetAllVouchersSearch")]
        public IActionResult GetAllVouchersSearch(VoucherFilterVM voucherFilterVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someVoucher = _voucherService.GetAllVouchersSearch(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            return Ok(someVoucher);
        }

        [HttpGet("GetAllCreditNotiTotalValue")]
        public Tuple<decimal, decimal> GetAllCreditNotiTotalValue(int invoiceid)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            decimal TotalValueCredit = 0;
            decimal TotalValueDepit = 0;

            try
            {
                var Voucher = _voucherService.GetVoucherByIdNoti(invoiceid).Result.ToList();
                if (Voucher.Count() > 0)
                {
                    foreach (var item in Voucher)
                    {
                        if (item.Type == 29)
                        {
                            TotalValueCredit += item.CreditNotiTotal ?? 0;
                        }
                        if (item.Type == 30)
                        {
                            TotalValueDepit += item.DepitNotiTotal ?? 0;
                        }

                    }
                }
                else
                {
                    TotalValueCredit = 0;
                    TotalValueDepit = 0;
                }
            }
            catch (Exception ex)
            {
                TotalValueCredit = 0;
                TotalValueDepit = 0;
            }
            return System.Tuple.Create(TotalValueCredit, TotalValueDepit);
        }
        [HttpGet("GetAllCreditNotiTotalValuePurchase")]
        public Tuple<decimal, decimal> GetAllCreditNotiTotalValuePurchase(int invoiceid)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            decimal TotalValueCredit = 0;
            decimal TotalValueDepit = 0;

            try
            {
                var Voucher = _voucherService.GetVoucherByIdNoti(invoiceid).Result.ToList();
                if (Voucher.Count() > 0)
                {
                    foreach (var item in Voucher)
                    {
                        if (item.Type == 32)
                        {
                            TotalValueCredit += item.CreditNotiTotal ?? 0;
                        }
                        if (item.Type == 33)
                        {
                            TotalValueDepit += item.DepitNotiTotal ?? 0;
                        }

                    }
                }
                else
                {
                    TotalValueCredit = 0;
                    TotalValueDepit = 0;
                }
            }
            catch (Exception ex)
            {
                TotalValueCredit = 0;
                TotalValueDepit = 0;
            }
            return System.Tuple.Create(TotalValueCredit, TotalValueDepit);
        }

        [HttpPost("GetAllVouchersSearchInvoice")]
        public IActionResult GetAllVouchersSearchInvoice(VoucherFilterVM voucherFilterVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            decimal TotalCredit = 0;
            decimal TotalDepit = 0;


            var someVoucher = _voucherService.GetAllVouchersSearch(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            if (someVoucher.Count() > 0)
            {
                foreach (var item in someVoucher)
                {
                    TotalCredit = 0;
                    TotalDepit = 0;

                    TotalCredit = GetAllCreditNotiTotalValue(item.InvoiceId).Item1;
                    TotalDepit = GetAllCreditNotiTotalValue(item.InvoiceId).Item2;

                    item.CreditNotiTotal = TotalCredit;
                    item.DepitNotiTotal = TotalDepit;

                }
            }
            return Ok(someVoucher);
        }
        [HttpGet("GetVouchersSearchInvoiceByID")]

        public IActionResult GetVouchersSearchInvoiceByID(int InvoiceId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            decimal TotalCredit = 0;
            decimal TotalDepit = 0;


            var someVoucher = _voucherService.GetVouchersSearchInvoiceByID(InvoiceId, _globalshared.BranchId_G, _globalshared.YearId_G).Result;
            if (someVoucher != null)
            {
                TotalCredit = GetAllCreditNotiTotalValue(someVoucher.InvoiceId).Item1;
                TotalDepit = GetAllCreditNotiTotalValue(someVoucher.InvoiceId).Item2;

                someVoucher.CreditNotiTotal = TotalCredit;
                someVoucher.DepitNotiTotal = TotalDepit;
            }
            return Ok(someVoucher);
        }
        [HttpGet("GetVouchersSearchInvoicePurchaseByID")]
        public IActionResult GetVouchersSearchInvoicePurchaseByID(int InvoiceId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            //decimal TotalCredit = 0;
            decimal TotalDepit = 0;


            var someVoucher = _voucherService.GetVouchersSearchInvoicePurchaseByID(InvoiceId, _globalshared.BranchId_G, _globalshared.YearId_G).Result;
            if (someVoucher != null)
            {
                //TotalCredit = GetAllCreditNotiTotalValue(someVoucher.InvoiceId).Item1;
                TotalDepit = GetAllCreditNotiTotalValuePurchase(someVoucher.InvoiceId).Item2;

                //someVoucher.CreditNotiTotal = TotalCredit;
                someVoucher.DepitNotiTotal = TotalDepit;
            }
            return Ok(someVoucher);

        }
        [HttpPost("GetAllVouchersRetSearch")]
        public IActionResult GetAllVouchersRetSearch(VoucherFilterVM voucherFilterVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someVoucher = _voucherService.GetAllVouchersRetSearch(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            return Ok(someVoucher);

        }
        [HttpGet("GetAllVouchersDelegate")]
        public IActionResult GetAllVouchersDelegate()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someVoucher = _voucherService.GetAllVouchersDelegate(_globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            return Ok(someVoucher);

        }
        [HttpPost("GetAllVouchersfromcontractSearch")]
        public IActionResult GetAllVouchersfromcontractSearch(VoucherFilterVM voucherFilterVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someVoucher = _voucherService.GetAllVouchersfromcontractSearch(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            return Ok(someVoucher);

        }


        [HttpPost("GetAllVouchersPurchase")]
        public IActionResult GetAllVouchersPurchase(VoucherFilterVM voucherFilterVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            // decimal TotalCredit = 0;
            decimal TotalDepit = 0;


            var someVoucher = _voucherService.GetAllVouchersPurchase(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            if (someVoucher.Count() > 0)
            {
                foreach (var item in someVoucher)
                {
                    //TotalCredit = 0;
                    TotalDepit = 0;

                    //TotalCredit = GetAllCreditNotiTotalValue(item.InvoiceId).Item1;
                    TotalDepit = GetAllCreditNotiTotalValuePurchase(item.InvoiceId).Item2;

                    //item.CreditNotiTotal = TotalCredit;
                    item.DepitNotiTotal = TotalDepit;

                }
            }
            return Ok(someVoucher);
        }
        [HttpPost("GetAllVouchersRetSearchPurchase")]
        public IActionResult GetAllVouchersRetSearchPurchase(VoucherFilterVM voucherFilterVM)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someVoucher = _voucherService.GetAllVouchersRetSearchPurchase(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();

            return Ok(someVoucher);
        }

        //اخر فاتورة
        [HttpGet("GetLastVoucher")]
        public IActionResult GetLastVoucher()
        {
            var invoic = _voucherService.GetAllVouchersback().Result;
            var lastinvoice = invoic.Max(p => p.InvoiceId);
            var lastone = _voucherService.GetVoucherById(lastinvoice);
            return Ok(lastone);
        }
        [HttpPost("GetAllVouchersQR")]
        public IActionResult GetAllVouchersQR(VoucherFilterVM voucherFilterVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_voucherService.GetAllVouchersQR(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G));
        }
        [HttpGet("GetAllVouchersProject")]
        public IActionResult GetAllVouchersProject()
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_voucherService.GetAllVouchersProject(_globalshared.BranchId_G, _globalshared.YearId_G));
        }
        [HttpPost("GetLastVouchersRet")]
        public IActionResult GetLastVouchersRet(VoucherFilterVM voucherFilterVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);



            var someVoucher = _voucherService.GetAllVouchersRet(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();

            return Ok(someVoucher);

        }
        [HttpPost("GetAllVouchersRet")]
        public IActionResult GetAllVouchersRet(VoucherFilterVM voucherFilterVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var someVoucher = _voucherService.GetAllVouchersRet(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();

            return Ok(someVoucher);
        }
        [HttpPost("GetAllVouchersRetPurchase")]

        public IActionResult GetAllVouchersRetPurchase(VoucherFilterVM voucherFilterVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            var someVoucher = _voucherService.GetAllVouchersRetPurchase(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();

            return Ok(someVoucher);
        }
        [HttpPost("GetAllCreditDepitNotiReport")]
        public IActionResult GetAllCreditDepitNotiReport(VoucherFilterVM voucherFilterVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            var someVoucher = _voucherService.GetAllCreditDepitNotiReport(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();

            return Ok(someVoucher);

        }
        [HttpPost("GetAllVouchersRetReport")]
        public IActionResult GetAllVouchersRetReport(VoucherFilterVM voucherFilterVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            var someVoucher = _voucherService.GetAllVouchersRetReport(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();

            return Ok(someVoucher);

        }
        [HttpPost("GetAllVouchersRetReport_Pur")]
        public IActionResult GetAllVouchersRetReport_Pur(VoucherFilterVM voucherFilterVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var someVoucher = _voucherService.GetAllVouchersRetReport_Pur(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();

            return Ok(someVoucher);

        }
        [HttpPost("GetAllPayVouchersRet")]
        public IActionResult GetAllPayVouchersRet(VoucherFilterVM voucherFilterVM)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var someVoucher = _voucherService.GetAllPayVouchersRet(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();

            return Ok(someVoucher);
        }

        [HttpGet("GetVoucherRpt")]
        public IActionResult GetVoucherRpt()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_voucherService.GetVoucherRpt(_globalshared.BranchId_G, _globalshared.YearId_G));
        }
        [HttpGet("GetCustRevenueExpensesDetails")]
        public IActionResult GetCustRevenueExpensesDetails(string FromDate, string ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_voucherService.GetCustRevenueExpensesDetails(FromDate, ToDate, _globalshared.BranchId_G, _globalshared.YearId_G));
        }
        [HttpGet("PrintReceiptExchangeVoucher")]
        public IActionResult PrintReceiptExchangeVoucher(int VoucherId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var voucher = _voucherService.GetVoucherById(VoucherId);
            return Ok(voucher);
        }
        [HttpGet("GetInvoiceById")]
        public IActionResult GetInvoiceById(int VoucherId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var voucher = _voucherService.GetVoucherById(VoucherId);
            return Ok(voucher);
        }
        [HttpGet("GetInvoiceById_Tran")]
        public IActionResult GetInvoiceById_Tran(int VoucherId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            decimal TotalCredit = 0;
            decimal TotalDepit = 0;

            var voucher = _voucherService.GetVoucherById(VoucherId).Result;
            if (voucher != null)
            {
                TotalCredit = 0;
                TotalDepit = 0;

                TotalCredit = GetAllCreditNotiTotalValue(voucher.InvoiceId).Item1;
                TotalDepit = GetAllCreditNotiTotalValue(voucher.InvoiceId).Item2;

                voucher.CreditNotiTotal = TotalCredit;
                voucher.DepitNotiTotal = TotalDepit;
            }

            return Ok(voucher);
        }
        [HttpGet("GetInvoiceDateById")]
        public IActionResult GetInvoiceDateById(int VoucherId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var voucher = _voucherService.GetInvoiceDateById(VoucherId);
            return Ok(voucher);
        }
       
        [HttpPost("VouchersCreditDepitNotiReport")]
        public IActionResult VouchersCreditDepitNotiReport(VoucherFilterVM param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _voucherService.GetAllCreditDepitNotiReport(param, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();

            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email,
                objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
            var branch = _BranchesService.GetBranchById(_globalshared.BranchId_G).Result;
            param.BranchName = branch.NameAr;
            ReportPDF = ReportsOf7sabat.VouchersCreditDepitNotiReport(result.ToList(), param, infoDoneTasksReport);

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

        
        [HttpGet("PrintDailyVoucher")]
        public IActionResult PrintDailyVoucher(int VoucherId)
        {
            var voucher = _voucherService.GetVoucherById(VoucherId);
            return Ok(voucher);
        }
        [HttpGet("GetAllDetailsByVoucherId")]
        public IActionResult GetAllDetailsByVoucherId(int? voucherId)
        {
            return Ok(_voucherService.GetAllDetailsByVoucherId(voucherId));
        }
        [HttpGet("GetInvoiceIDByProjectID")]
        public IActionResult GetInvoiceIDByProjectID(int? ProjectId)
        {
            return Ok(_voucherService.GetInvoiceIDByProjectID(ProjectId));
        }
        [HttpGet("GetAllDetailsByInvoiceId")]
        public IActionResult GetAllDetailsByInvoiceId(int? voucherId)
        {
            return Ok(_voucherService.GetAllDetailsByInvoiceId(voucherId));
        }
        [HttpGet("GetAllDetailsByInvoiceIdFirstOrDef")]
        public IActionResult GetAllDetailsByInvoiceIdFirstOrDef(int? voucherId)
        {
            var Result = _voucherService.GetAllDetailsByInvoiceIdFirstOrDef(voucherId);
            return Ok(Result);
        }
        [HttpGet("GetAllTransByLineNo")]
        public IActionResult GetAllTransByLineNo(int LineNo)
        {
            return Ok(_voucherService.GetAllTransByLineNo(LineNo));

        }
        [HttpGet("GetAllTrans")]
        public IActionResult GetAllTrans(int VouDetailsID)
        {
            return Ok(_voucherService.GetAllTrans(VouDetailsID));

        }
        [HttpPost("SaveVoucher")]
        public IActionResult SaveVoucher(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SaveVoucher(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(new { result.StatusCode, result.ReasonPhrase, result.ReturnedParm });
        }
        [HttpPost("SaveandPostVoucher")]
        public IActionResult SaveandPostVoucher(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SaveandPostVoucher(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(result);
        }
        [HttpPost("SaveVoucherP")]
        public IActionResult SaveVoucherP(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext); //global

            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SaveVoucherP(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(new { result.StatusCode, result.ReasonPhrase, result.ReturnedParm });
        }



        [HttpPost("UploadPayVoucherImage")] 
        public IActionResult UploadPayVoucherImage(IFormFile? UploadedFile,[FromForm] int? InvoiceId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            string FileName = "";
            string FileUrl = "";
            if (InvoiceId > 0)
            {
                if (UploadedFile != null)
                {
                    string path = System.IO.Path.Combine("Uploads/", "Financefile/");
                    string pathW = System.IO.Path.Combine("/Uploads/", "Financefile/");

                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }

                    List<string> uploadedFiles = new List<string>();
                    string pathes = "";
                    //foreach (IFormFile postedFile in postedFiles)
                    //{
                    string fileName = System.IO.Path.GetFileName(Guid.NewGuid() + UploadedFile.FileName);
                    //string fileName = System.IO.Path.GetFileName(postedFiles.FileName);

                    var path2 = Path.Combine(path, fileName);
                    if (System.IO.File.Exists(path2))
                    {
                        System.IO.File.Delete(path2);
                    }
                    using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(path, fileName), System.IO.FileMode.Create))
                    {


                        UploadedFile.CopyTo(stream);
                        uploadedFiles.Add(fileName);
                        // string returnpath = host + path + fileName;
                        //pathes.Add(pathW + fileName);
                        pathes = pathW + fileName;
                    }


                    if (pathes != "")
                    {
                        FileName = fileName;
                        FileUrl = "/Uploads/Financefile/" + fileName;
                    }
                }
                var result = _voucherService.SaveVoucherPUpdateImage(InvoiceId??0, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, FileName, FileUrl);
                return Ok(result);
            }
            else
            {
                return Ok(new GeneralMessage{ StatusCode= HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed });
            }



        }


        [HttpPost("SaveandPostVoucherP")]
        public IActionResult SaveandPostVoucherP(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SaveandPostVoucherP(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(new { result.StatusCode, result.ReasonPhrase, result.ReturnedParm });
        }
        [HttpPost("UpdateVoucher")]
        public IActionResult UpdateVoucher(int InvoiceId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _voucherService.UpdateVoucher(InvoiceId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("UpdateVoucherDraft")]
        public IActionResult UpdateVoucherDraft(int InvoiceId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _voucherService.UpdateVoucherDraft(InvoiceId, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G,Con??"");
            return Ok(result);
        }

        [HttpPost("UpdateVoucher_recipient")]
        public IActionResult UpdateVoucher_recipient(string InvoiceId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _voucherService.UpdateVoucherRecepient(InvoiceId, _globalshared.UserId_G, _globalshared.BranchId_G,_globalshared.YearId_G);
            return Ok(result);
        }
        [HttpPost("UpdateVoucher_payed")]
        public IActionResult UpdateVoucher_payed(int InvoiceId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _voucherService.UpdateVoucher_payed(InvoiceId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }

        [HttpPost("UpdateVoucher_payed_bySupp")]
        public IActionResult UpdateVoucher_payed_bySupp(string SupplierInvoiceNo)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _voucherService.UpdateVoucher_payed_by(SupplierInvoiceNo, _globalshared.UserId_G, _globalshared.BranchId_G,_globalshared.YearId_G);
            return Ok(result);
        }
        [HttpPost("VousherRe_Sum")]
        public IActionResult VousherRe_Sum(int InvoiceId)
        {
            var result = _voucherService.VousherRe_Sum(InvoiceId);
            var res = new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = result.ToString() };
            return Ok(res);
        }


        [HttpPost("PayVousher_Sum")]
        public IActionResult PayVousher_Sum(int InvoiceId)
        {
            var result = _voucherService.PayVousher_Sum(InvoiceId);
            var res = new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = result.ToString() };
            return Ok(res);
        }


        [HttpPost("SaveInvoice")]
        public IActionResult SaveInvoice(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SaveInvoice(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(result);
        }
        [HttpPost("SaveInvoiceForServices")]
        public IActionResult SaveInvoiceForServices(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SaveInvoiceForServices(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            
            return Ok(result);
        }
        [HttpPost("SaveInvoiceForServicesDraft")]
        public IActionResult SaveInvoiceForServicesDraft(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SaveInvoiceForServicesDraft(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(result);
        }
        [HttpPost("SaveInvoiceForServicesNoti")]
        public IActionResult SaveInvoiceForServicesNoti(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            //var result2 = _voucherService.GetVoucherById(voucher.InvoiceId);


            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SaveInvoiceForServicesNoti(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
           
            return Ok(result);
        }


        [HttpPost("SaveInvoiceForServicesNotiDepit")]
        public IActionResult SaveInvoiceForServicesNotiDepit(Invoices voucher)
        {
            //var result2 = _voucherService.GetVoucherById(voucher.InvoiceId);

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SaveInvoiceForServicesNotiDepit(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(result);
        }
        [HttpPost("SaveandPostInvoiceForServices")]
        public IActionResult SaveandPostInvoiceForServices(Invoices voucher)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SaveandPostInvoiceForServices(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");

            return Ok(result);
        }
        [HttpPost("SaveInvoiceForServicesRet")]
        public IActionResult SaveInvoiceForServicesRet(Invoices voucher)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _voucherService.SaveInvoiceForServicesRet(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(result);
        }
        [HttpPost("SaveInvoiceForServicesRetNEW")]
        public IActionResult SaveInvoiceForServicesRetNEW(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Acc_voucher = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.InvoiceId == voucher.InvoiceId).FirstOrDefault();
            //var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (DateTime.Now.Year != Acc_voucher!.YearId)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SaveInvoiceForServicesRetNEW_func(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, _globalshared.Lang_G, Con);

            return Ok(result);
        }
        [HttpPost("ReturnNotiCreditBack")]
        public IActionResult ReturnNotiCreditBack(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _voucherService.ReturnNotiCreditBack(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(result);
        }
        [HttpPost("ReturnNotiDepitBack")]
        public IActionResult ReturnNotiDepitBack(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _voucherService.ReturnNotiDepitBack(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(result);
        }
        [HttpPost("SaveInvoiceForServicesRet_Back")]
        public IActionResult SaveInvoiceForServicesRet_Back(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _voucherService.SaveInvoiceForServicesRet_Back(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(result);
        }
        //Start Purchase
        [HttpPost("SavePurchaseForServices")]
        public IActionResult SavePurchaseForServices(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SavePurchaseForServices(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(new { result.StatusCode, result.ReasonPhrase, result.ReturnedParm });
        }
        [HttpPost("SavePurchaseForServicesNotiDepit")]
        public IActionResult SavePurchaseForServicesNotiDepit(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SavePurchaseForServicesNotiDepit(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(new { result.StatusCode, result.ReasonPhrase, result.ReturnedParm });
        }
        [HttpPost("SaveandPostPurchaseForServices")]
        public IActionResult SaveandPostPurchaseForServices(Invoices voucher)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SaveandPostPurchaseForServices(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(new { result.StatusCode, result.ReasonPhrase, result.ReturnedParm });
        }

        [HttpPost("SaveandPostPurchaseOrderForServices")]
        public IActionResult SaveandPostPurchaseOrderForServices(Invoices voucher)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SaveandPostPurchaseOrderForServices(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(new { result.StatusCode, result.ReasonPhrase, result.ReturnedParm });
        }

        [HttpPost("ConverOrderToInvoice")]
        public IActionResult ConverOrderToInvoice(int voucherId)
            {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            
            var result = _voucherService.ConverOrderToInvoice(voucherId, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(new { result.StatusCode, result.ReasonPhrase, result.ReturnedParm });
        }

        [HttpPost("SavePurchaseForServicesRet")]
        public IActionResult SavePurchaseForServicesRet(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _voucherService.SavePurchaseForServicesRet(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(result);
        }
        [HttpPost("SavePurchaseForServicesRetNew")]
        public IActionResult SavePurchaseForServicesRetNew(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _voucherService.SavePurchaseForServicesRetNEW_func(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, _globalshared.Lang_G, Con);
            return Ok(result);
        }
        //End Purchase
        [HttpPost("SavePayVoucherForServicesRet")]
        public IActionResult SavePayVoucherForServicesRet(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            var result = _voucherService.SavePayVoucherForServicesRet(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(result);
        }
        [HttpPost("SaveInvoiceForServices2")]
        public IActionResult SaveInvoiceForServices2(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SaveInvoiceForServices2(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(result);
        }
        [HttpPost("SaveandPostInvoiceForServices2")]
        public IActionResult SaveandPostInvoiceForServices2(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SaveandPostInvoiceForServices2(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(result);
        }
        [HttpPost("SaveConvertVoucher")]
        public IActionResult SaveConvertVoucher(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SaveConvertVoucher(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(result);
        }
        [HttpPost("SaveDailyVoucher")]
        public IActionResult SaveDailyVoucher(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SaveDailyVoucher(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(new { result.StatusCode, result.ReasonPhrase, result.ReturnedParm });
        }
        [HttpPost("SaveandPostDailyVoucher")]
        public IActionResult SaveandPostDailyVoucher(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SaveandPostDailyVoucher(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(new { result.StatusCode, result.ReasonPhrase, result.ReturnedParm });
        }
        [HttpPost("SaveClosingVoucher")]
        public IActionResult SaveClosingVoucher(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            int? dataVoucher = _voucherService.GenerateVoucherNumberClosing(25, _globalshared.BranchId_G, _globalshared.YearId_G).Result;

            var result = _voucherService.SaveClosingVoucher(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, dataVoucher, _globalshared.YearId_G, Con ?? "");
            return Ok(result);
        }
        [HttpPost("SaveEmpVoucher")]
        public IActionResult SaveEmpVoucher(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SaveEmpVoucher(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(result);
        }
        [HttpPost("SaveRecycleVoucher")]
        public IActionResult SaveRecycleVoucher(int YearID)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _voucherService.SaveRecycleVoucher(YearID, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con);
            return Ok(result);
        }
        [HttpPost("SaveRecycleReturnVoucher")]
        public IActionResult SaveRecycleReturnVoucher(int YearID)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _voucherService.SaveRecycleReturnVoucher(YearID, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con);
            return Ok(result);
        }
        [HttpPost("SaveOpeningVoucher")]
        public IActionResult SaveOpeningVoucher(Invoices voucher)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            int? dataVoucher = _voucherService.GenerateVoucherNumberOpening(10, _globalshared.BranchId_G, _globalshared.YearId_G)?.Result??0;

            var result = _voucherService.SaveOpeningVoucher(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, dataVoucher, _globalshared.YearId_G, Con ?? "");
            return Ok(result);
        }
        [HttpPost("SaveandPostOpeningVoucher")]
        public IActionResult SaveandPostOpeningVoucher(Invoices voucher)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var VoucherDatetime = DateTime.ParseExact(voucher.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            int? dataVoucher = _voucherService.GenerateVoucherNumberOpening(10, _globalshared.BranchId_G, _globalshared.YearId_G)?.Result??0;
            var result = _voucherService.SaveandPostOpeningVoucher(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, dataVoucher, _globalshared.YearId_G, Con ?? "");
            return Ok(result);
        }
        [HttpPost("SaveDailyVoucher2")]
        public IActionResult SaveDailyVoucher2(List<Transactions> voucher)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var VoucherDatetime = DateTime.ParseExact(voucher.FirstOrDefault().TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (_globalshared.YearId_G != VoucherDatetime.Year)
            {
                var Msg = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن حفظ تاريخ في سنة مالية مختلفة" };
                return Ok(Msg);
            }
            var result = _voucherService.SaveDailyVoucher2(voucher, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G, Con ?? "");
            return Ok(result);
        }
        [HttpGet("GenerateVoucherNumber")]
        public IActionResult GenerateVoucherNumber(int Type)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Value = _voucherService.GenerateVoucherNumber(Type, _globalshared.BranchId_G, _globalshared.YearId_G).Result;
            var NewValue = string.Format("{0:000000}", Value);
            var generatevalue = new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = NewValue };
            return Ok(generatevalue);
        }


        [HttpGet("GenerateVoucherNumberNew")]

        public ActionResult GenerateVoucherNumberNew(int Type)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _voucherService.GenerateVoucherNumberNewPro(Type, _globalshared.BranchId_G, _globalshared.YearId_G, Type, Con??"").Result;
            var generatevalue = new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = result };
            return Ok(generatevalue);
        }


        [HttpGet("GenerateVoucherNumberOpening")]
        public IActionResult GenerateVoucherNumberOpening(int Type)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Value = _voucherService.GenerateVoucherNumberOpening(Type, _globalshared.BranchId_G, _globalshared.YearId_G).Result;
            var NewValue = string.Format("{0:000000}", Value);
            var generatevalue = new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = NewValue };
            return Ok(generatevalue);
        }
        [HttpGet("GenerateVoucherNumberClosing")]
        public IActionResult GenerateVoucherNumberClosing(int Type)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Value = _voucherService.GenerateVoucherNumberClosing(Type, _globalshared.BranchId_G, _globalshared.YearId_G).Result;
            var NewValue = string.Format("{0:000000}", Value);
            var generatevalue = new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = NewValue };
            return Ok(generatevalue);
        }
        [HttpGet("GetAllTransByVoucherId")]
        public IActionResult GetAllTransByVoucherId(int? voucherId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            if (voucherId == 0)
            {
                return null;
            }
            else
            {


                return Ok(_voucherService.GetAllTransByVoucherId(voucherId));
            }
        }

        //عرض دائن اولا
        [HttpGet("GetAllTransByVoucherIddaen")]
        public IActionResult GetAllTransByVoucherIddaen(int? voucherId)
        {
            if (voucherId == 0)
            {
                return null;
                // return GetAllTransByVoucherIddaen(voucherId);
            }
            else
            {
                IEnumerable<TransactionsVM> test = _voucherService.GetAllTransByVoucherId(voucherId).Result;
                test = test.OrderBy(a => a.DepitOrCreditName);
                return Ok(test);
            }
            // return Ok(_voucherService.GetAllTransByVoucherId(voucherId));
        }
        //عرض مدين اولا
        [HttpGet("GetAllTransByVoucherIdmaden")]
        public IActionResult GetAllTransByVoucherIdmaden(int? voucherId, string Dipt = "")
        {

            IEnumerable<TransactionsVM> test = _voucherService.GetAllTransByVoucherId(voucherId).Result;
            test = test.OrderByDescending(a => a.DepitOrCreditName);

            return Ok(test);
            // return Ok(_voucherService.GetAllTransByVoucherId(voucherId));
        }
        [HttpPost("PostVouchers")]
        public IActionResult PostVouchers(List<Invoices> vouchers)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _voucherService.PostVouchers(vouchers, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G);
            return Ok(result);
        }
        [HttpPost("PostVouchersCustody")]
        public IActionResult PostVouchersCustody(List<Custody> vouchers)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _voucherService.PostVouchersCustody(vouchers, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G);
            return Ok(result);
        }
        [HttpPost("PostVouchersCheckBox")]
        public IActionResult PostVouchersCheckBox([FromForm]List<Int32> voucherIds)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _voucherService.PostVouchersCheckBox(voucherIds, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G);
            return Ok(result);
        }

        [HttpPost("PostBackVouchers")]
        public IActionResult PostBackVouchers(List<Invoices> vouchers)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _voucherService.PostBackVouchers(vouchers, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G);
            return Ok(result);
        }
        [HttpPost("PostBackVouchersCustody")]
        public IActionResult PostBackVouchersCustody(List<Custody> vouchers)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _voucherService.PostBackVouchersCustody(vouchers, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G);
            return Ok(result);
        }
        [HttpPost("CancelPostVouchers")]
        public IActionResult CancelPostVouchers(List<Invoices> vouchers)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            var result = _voucherService.CancelPostVouchers(vouchers, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G);
            return Ok(result);
        }
        [HttpPost("DeleteVoucher")]
        public IActionResult DeleteVoucher(int VoucherId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _voucherService.DeleteVoucher(VoucherId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveVoucherAlarmDate")]
        public IActionResult SaveVoucherAlarmDate(int VoucherId, string VoucherAlarmDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _voucherService.SaveVoucherAlarmDate(VoucherId, VoucherAlarmDate, _globalshared.UserId_G, _globalshared.BranchId_G);

            return Ok(result);
        }
        [HttpPost("SaveVueDate")]
        public IActionResult SaveVueDate(int VoucherId, string VueDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _voucherService.SaveVueDate(VoucherId, VueDate, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetProjectManagerRevene")]
        public IActionResult GetProjectManagerRevene(int? ManagerId, string FromDate, string ToDate)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_voucherService.GetProjectManagerRevene(ManagerId, FromDate, ToDate, _globalshared.BranchId_G, _globalshared.YearId_G));
        }

        [HttpPost("SaveServicesPrice")]
        public IActionResult SaveServicesPrice(Acc_Services_Price services_Price)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _servicesPriceService.SaveService(services_Price, _globalshared.UserId_G, _globalshared.BranchId_G,null);
            return Ok(result);
        }
        [HttpPost("SaveServicePriceWithDetails")]
        public IActionResult SaveServicePriceWithDetails(Services_PriceWithDetails services_PriceWithDetails)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _servicesPriceService.SaveService(services_PriceWithDetails?.services_price??new Acc_Services_Price(), _globalshared.UserId_G, _globalshared.BranchId_G, services_PriceWithDetails?.details);
            return Ok(result);
        }

        [HttpPost("DeleteService")]
        public IActionResult DeleteService(int? servicesId)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _servicesPriceService.DeleteService(servicesId, _globalshared.UserId_G, _globalshared.BranchId_G);

            return Ok(result);
        }
        [HttpGet("GetAllServicesPrice")]
        public IActionResult GetAllServicesPrice()
        {
            return Ok(_servicesPriceService.GetAllServicesPrice());
        }
        [HttpGet("GetServicesPriceByProjectId")]
        public IActionResult GetServicesPriceByProjectId(int? param)
        {
            return Ok(_servicesPriceService.GetServicesPriceByProjectId(param));
        }
        [HttpGet("GetServicesPriceByServiceId")]
        public IActionResult GetServicesPriceByServiceId(int ServiceId)
        {
            return Ok(_servicesPriceService.GetServicesPriceByServiceId(ServiceId));
        }
        [HttpGet("FillAllServicePrice")]
        public IActionResult FillAllServicePrice()
        {
            var someProject = _servicesPriceService.GetAllServicesPrice().Result.Select(s => new {
                Id = s.ServicesId,
                Name = s.ServicesName
            });
            return Ok(someProject);
        }
        [HttpPost("FillAllAlarmVoucher")]
        public IActionResult FillAllAlarmVoucher(VoucherFilterVM voucherFilterVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            voucherFilterVM.Type = 2;
            var someVoucher = _voucherService.GetAllAlarmVoucher(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            var AlarmVoucher = someVoucher.Select(s => new {
                Id = s.InvoiceId,
                Name = " فاتورة رقم  " + s.InvoiceNumber + " - " + s.CustomerName
            });
            return Ok(AlarmVoucher);

        }
        [HttpPost("FillAllNotiVoucher")]
        public IActionResult FillAllNotiVoucher(VoucherFilterVM voucherFilterVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            voucherFilterVM.Type = 2;

            var Accyear = _globalshared.YearId_G;
            if(voucherFilterVM.PrevInvoices==true)
            {
                Accyear = _globalshared.YearId_G-1;
            }
            var someVoucher = _voucherService.GetAllNotioucher(voucherFilterVM, _globalshared.BranchId_G, Accyear).Result.ToList();
            var AlarmVoucher = someVoucher.Select(s => new {
                Id = s.InvoiceId,
                Name = " فاتورة رقم  " + s.InvoiceNumber + " - " + s.CustomerName
            });
            return Ok(AlarmVoucher);
        }
        [HttpPost("FillAllNotiPurchaseVoucher")]
        public IActionResult FillAllNotiPurchaseVoucher(VoucherFilterVM voucherFilterVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            voucherFilterVM.Type = 1;
            var someVoucher = _voucherService.GetAllNotioucher(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();

            var Voucher = someVoucher.Select(s => new {
                Id = s.InvoiceId,
                Name = " فاتورة رقم  " + s.InvoiceNumber + " - " + s.SupplierName
            });

            return Ok(Voucher);
        }
        [HttpGet("FillServicesPriceByProjectId")]
        public IActionResult FillServicesPriceByProjectId(int Param)
        {
            var someProject = _servicesPriceService.GetServicesPriceByProjectId(Param).Result.Select(s => new {
                Id = s.ServicesId,
                Name = s.ServicesName
            });
            return Ok(someProject);
        }
        [HttpGet("GetServicesPriceByProjectId2")]
        public IActionResult GetServicesPriceByProjectId2(int? param, int? param2)
        {
            return Ok(_servicesPriceService.GetServicesPriceByProjectId2(param, param2));
        }
        [HttpGet("GetServicePriceByProject_Search")]
        public IActionResult GetServicePriceByProject_Search(int? Project1, int? Project2, string? ServiceName, string? ServiceDesc, decimal? Amount)
        {

            return Ok(_servicesPriceService.GetServicePriceByProject_Search(Project1, Project2, ServiceName??"", ServiceDesc??"", Amount));
        }
        [HttpGet("GetServicesPriceByParentId")]
        public IActionResult GetServicesPriceByParentId(int? ParentId)
        {
            return Ok(_servicesPriceService.GetServicesPriceByParentId(ParentId));
        }
        [HttpGet("GetServicesPriceVouByParentId")]
        public IActionResult GetServicesPriceVouByParentId(int? ParentId,int? offerid)
        {
            return Ok(_servicesPriceOfferService.GetServicesPriceByParentId(ParentId, offerid));
        }
        [HttpGet("GetServicesPriceVouByParentIdAndContract")]
        public IActionResult GetServicesPriceVouByParentIdAndContract(int? ParentId, int? ContractId)
        {
            return Ok(_servicesPriceOfferService.GetServicesPriceByParentIdAndContractId(ParentId, ContractId));
        }
        [HttpGet("GetServicesPriceVouByParentIdAndInvoiceId")]
        public IActionResult GetServicesPriceVouByParentIdAndInvoiceId(int? ParentId, int? InvoiceId)
        {
            return Ok(_servicesPriceOfferService.GetServicesPriceVouByParentIdAndInvoiceId(ParentId, InvoiceId));
        }
        [HttpGet("GetServicesPriceAmountByServicesId")]
        public IActionResult GetServicesPriceAmountByServicesId(int? ServicesId)
        {
            return Ok(_servicesPriceService.GetServicesPriceAmountByServicesId(ServicesId));
        }

        [HttpGet("PrintVoucher")]
        public IActionResult PrintVoucher(int VoucherId, int ReportType)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;

            DataTable voucher = _voucherService.ReceiptCashingPaying(VoucherId, Con).Result;
            InvoicesVM InvoicesVM = _voucherService.GetVoucherById(VoucherId).Result;
            //costCenterNam = _CostCenterservice.GetCostCenterByProId(InvoicesVM.ProjectId.Value);

            var costCenterNam = "";
            if (InvoicesVM.VoucherDetails[0].CostCenterId != null)
            {

                costCenterNam = _CostCenterservice.GetCostCenterById(InvoicesVM.VoucherDetails[0].CostCenterId ?? 0).Result.NameAr;
            }
            else
            {
                costCenterNam = "بدون";
            }
            var ReqInv = _invoicesRequestsService.GetInvoiceReq(VoucherId).Result;

            List<VoucherDetailsVM> VoucherDetailsVM = _voucherService.GetAllDetailsByInvoiceId(VoucherId).Result.ToList();
            if (ReportType == 1)//النموذج الاول للفاتورة
                ReportPDF = inVoiceReports.GenInvoice(VoucherDetailsVM, objOrganization, InvoicesVM, voucher);
            else if (ReportType == 2)///النموذج التانى للفاتورة
                ReportPDF = inVoiceReports.GenInvoice2(VoucherDetailsVM, objOrganization, InvoicesVM, voucher);
            else if (ReportType == 4)///فاتورة صغيره
                ReportPDF = inVoiceReports.GenInvoiceN_S(VoucherDetailsVM, objOrganization, InvoicesVM, voucher, costCenterNam??"", ReqInv!=null? ReqInv.QRCode:null);
            else if (ReportType == 5)///مسودة صغيره
                ReportPDF = inVoiceReports.GenInvoiceN_SDraft(VoucherDetailsVM, objOrganization, InvoicesVM, voucher, costCenterNam ?? "", ReqInv != null ? ReqInv.QRCode : null);
            else //New
                ReportPDF = inVoiceReports.GenInvoiceN(VoucherDetailsVM, objOrganization, InvoicesVM, voucher, costCenterNam??"");

            //dawoudprint
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

        [HttpGet("GetReport")]

        public IActionResult GetReport(int VoucherId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            VoucherReportVM _entryVoucherReportVM = new VoucherReportVM();
            InvoicesVM res = _voucherService.GetVoucherById(VoucherId).Result;
            DataTable res1 = _voucherService.ReceiptCashingPaying(VoucherId, Con).Result;
            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };


            List<VoucherVMDatatable> VMList = new List<VoucherVMDatatable>();
            for (int i = 0; i < res1.Rows.Count; i++)
            {
                VoucherVMDatatable invoice = new VoucherVMDatatable();
                invoice.InvoiceNumber = res1.Rows[i]["InvoiceNumber"].ToString();
                invoice.JoNo = res1.Rows[i]["JoNo"].ToString();
                invoice.Date = res1.Rows[i]["Date"].ToString();
                invoice.HijriDate = res1.Rows[i]["HijriDate"].ToString();
                invoice.CustomerName = res1.Rows[i]["CustomerName"].ToString();
                invoice.InvoiceValueText = res1.Rows[i]["InvoiceValueText"].ToString();
                invoice.TotalValue = res1.Rows[i]["TotalValue"].ToString();
                invoice.RecevierTxt = res1.Rows[i]["RecevierTxt"].ToString();
                invoice.PayType = res1.Rows[i]["PayType"].ToString();
                invoice.MoneyOrderDate = res1.Rows[i]["MoneyOrderDate"].ToString();
                invoice.CheckDate = res1.Rows[i]["CheckDate"].ToString();
                invoice.BankName = res1.Rows[i]["BankName"].ToString();
                invoice.Description = res1.Rows[i]["Description"].ToString();
                invoice.Notes = res1.Rows[i]["Notes"].ToString();
                invoice.CostCenterName = res1.Rows[i]["CostCenterName"].ToString();
                invoice.FullName = res1.Rows[i]["FullName"].ToString();
                invoice.SupplierName = res1.Rows[i]["SupplierName"].ToString();
                invoice.ToInvoiceId = res1.Rows[i]["ToInvoiceId"].ToString();

                VMList.Add(invoice);
            }
            string ValueNumString2 = ConvertNumToString(res1.Rows[0]["TotalValue"].ToString());
            _entryVoucherReportVM.NumString = ValueNumString2;
            _entryVoucherReportVM.VoucherVM = VMList;

            string Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            _entryVoucherReportVM.DateTimeNow = Date;

            var objBranch = _BranchesService.GetBranchByBranchId(_globalshared.Lang_G, _globalshared.BranchId_G).Result.FirstOrDefault();
            _entryVoucherReportVM.DateTimeNow = Date;
            _entryVoucherReportVM.Branch = objBranch;

            var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
            _entryVoucherReportVM.Org_VD = objOrganization2;
            return Ok(_entryVoucherReportVM);
        }


        [HttpPost("PrintPayVoucherRet")]
        public IActionResult PrintPayVoucherRet(int VoucherId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            // InvoicesVM res=_voucherService.GetVoucherById(VoucherId);
            DataTable res1 = _voucherService.ReceiptCashingPaying(VoucherId, Con).Result;
            int SupplierId = _voucherService.GetVoucherById(VoucherId).Result.SupplierId ?? 0;
            string SupplierName = _acc_SuppliersService.GetSuppNameBySuppId(SupplierId, "rtl", _globalshared.BranchId_G);
            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

            if (res1 != null && res1.Rows.Count > 0) ReportPDF = ReportsOf7sabat.PayVoucherRet(res1, infoDoneTasksReport, SupplierName);
            else ReportPDF = new byte[0];
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


        [HttpGet("PrintGetVoucherRpt")]

        public IActionResult PrintGetVoucherRpt()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            ReportGridVM _reportGrid = new ReportGridVM();
            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

            List<InvoicesVM> InvoicesVM = _voucherService.GetVoucherRpt(_globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();

            _reportGrid.InvoicesVM = InvoicesVM;

            string Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            _reportGrid.DateTimeNow = Date;

            var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
            _reportGrid.Org_VD = objOrganization2;
            var branch = _BranchesService.GetBranchById(_globalshared.BranchId_G).Result;
            _reportGrid.BranchName = branch.NameAr;
            return Ok(_reportGrid);
        }






        //تقرير سندات الصرف ، القبض ، واليومية   
      [HttpPost("Printdiffrentvoucher")]

        public IActionResult Printdiffrentvoucher(VoucherFilterVM voucherFilterVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };



            List<InvoicesVM> InvoicesVM = _voucherService.GetVoucherRpt(_globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            var someVoucher = _voucherService.GetAllVouchers(voucherFilterVM, _globalshared.BranchId_G, _globalshared.YearId_G).Result.ToList();
            if (voucherFilterVM.Type == 5)
            {
                ReportPDF = ReportsOf7sabat.PrintpayvoucherN(someVoucher, infoDoneTasksReport, voucherFilterVM.dateFrom, voucherFilterVM.dateTo);
            }
            else if (voucherFilterVM.Type == 6)
            {
                ReportPDF = ReportsOf7sabat.PrintRevoucherN(someVoucher, infoDoneTasksReport, voucherFilterVM.dateFrom, voucherFilterVM.dateTo);
            }
            else if (voucherFilterVM.Type == 8)
            {
                ReportPDF = ReportsOf7sabat.PrintEntryVoucher(someVoucher, infoDoneTasksReport, voucherFilterVM.dateFrom, voucherFilterVM.dateTo);
            }


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

        [HttpGet("DailyVoucherReport")]

        public IActionResult DailyVoucherReport(int VoucherId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            EntryVoucherReportVM _entryVoucherReportVM = new EntryVoucherReportVM();
            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
            DataTable InvoicesVM = _voucherService.DailyVoucherReport(VoucherId, Con).Result;
            List<EntryVoucherVMDatatable> InvoicesVMList = new List<EntryVoucherVMDatatable>();
            for (int i = 0; i < InvoicesVM.Rows.Count; i++)
            {
                EntryVoucherVMDatatable invoice = new EntryVoucherVMDatatable();
                invoice.InvoiceNumber = InvoicesVM.Rows[i]["InvoiceNumber"].ToString();
                invoice.InvoiceDate = InvoicesVM.Rows[i]["InvoiceDate"].ToString();
                invoice.InvoiceHijriDate = InvoicesVM.Rows[i]["InvoiceHijriDate"].ToString();
                invoice.AccountCode = InvoicesVM.Rows[i]["AccountCode"].ToString();
                invoice.AccountNameAr = InvoicesVM.Rows[i]["AccountNameAr"].ToString();
                invoice.Depit = InvoicesVM.Rows[i]["Depit"].ToString();
                invoice.Credit = InvoicesVM.Rows[i]["Credit"].ToString();
                invoice.Notes = InvoicesVM.Rows[i]["Notes"].ToString();
                invoice.CostCenterNameAr = InvoicesVM.Rows[i]["CostCenterNameAr"].ToString();
                InvoicesVMList.Add(invoice);
            }
            _entryVoucherReportVM.InvoicesVM = InvoicesVMList;

            string Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            _entryVoucherReportVM.DateTimeNow = Date;

            var objBranch = _BranchesService.GetBranchByBranchId(_globalshared.Lang_G, _globalshared.BranchId_G).Result.FirstOrDefault();
            _entryVoucherReportVM.Branch = objBranch;
            var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
            _entryVoucherReportVM.Org_VD = objOrganization2;
            return Ok(_entryVoucherReportVM);
        }

        [HttpGet("DailyVoucherReport_Custody")]
        public IActionResult DailyVoucherReport_Custody(int VoucherId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
            DataTable InvoicesVM = _voucherService.DailyVoucherReport(VoucherId, Con).Result;
            ReportPDF = inVoiceReports.PrintDailyVoucherReport_Custody(InvoicesVM, infoDoneTasksReport);
            //dawoudprint
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

        [HttpGet("ClosingVoucherReport")]

        public IActionResult ClosingVoucherReport(int VoucherId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            EntryVoucherReportVM _entryVoucherReportVM = new EntryVoucherReportVM();
            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
            DataTable InvoicesVM = _voucherService.DailyVoucherReport(VoucherId, Con).Result;
            List<EntryVoucherVMDatatable> InvoicesVMList = new List<EntryVoucherVMDatatable>();
            for (int i = 0; i < InvoicesVM.Rows.Count; i++)
            {
                EntryVoucherVMDatatable invoice = new EntryVoucherVMDatatable();
                invoice.InvoiceNumber = InvoicesVM.Rows[i]["InvoiceNumber"].ToString();
                invoice.InvoiceDate = InvoicesVM.Rows[i]["InvoiceDate"].ToString();
                invoice.InvoiceHijriDate = InvoicesVM.Rows[i]["InvoiceHijriDate"].ToString();
                invoice.AccountCode = InvoicesVM.Rows[i]["AccountCode"].ToString();
                invoice.AccountNameAr = InvoicesVM.Rows[i]["AccountNameAr"].ToString();
                invoice.Depit = InvoicesVM.Rows[i]["Depit"].ToString();
                invoice.Credit = InvoicesVM.Rows[i]["Credit"].ToString();
                invoice.Notes = InvoicesVM.Rows[i]["Notes"].ToString();
                invoice.CostCenterNameAr = InvoicesVM.Rows[i]["CostCenterNameAr"].ToString();
                InvoicesVMList.Add(invoice);
            }
            _entryVoucherReportVM.InvoicesVM = InvoicesVMList;

            string Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            _entryVoucherReportVM.DateTimeNow = Date;

            var objBranch = _BranchesService.GetBranchByBranchId(_globalshared.Lang_G, _globalshared.BranchId_G).Result.FirstOrDefault();
            _entryVoucherReportVM.Branch = objBranch;
            var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
            _entryVoucherReportVM.Org_VD = objOrganization2;
            return Ok(_entryVoucherReportVM);
        }

        [HttpGet("GetViewDetailsGrid")]
        public IActionResult GetViewDetailsGrid(int Type, int Status)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var SelectStetment = "";
            if (Status == 1)
            {
                SelectStetment = "select inv.InvoiceNumber as InvoiceNumber,trat.NameAr,sum(Credit) as Credit ,sum(Depit) as Depit , sum(Credit)-sum(Depit) as Diff from Acc_Transactions tra"
               + " join Acc_Invoices inv on tra.InvoiceId = inv.InvoiceId"
               + " join Acc_TransactionTypes trat on trat.TransactionTypeId=tra.Type"
               + " where tra.YearId = " + _globalshared.YearId_G + " and tra.IsDeleted = 0 and inv.IsDeleted = 0 group by tra.YearId,inv.InvoiceId,inv.InvoiceNumber,trat.NameAr";

            }
            else if (Status == 2)
            {
                SelectStetment = "select inv.InvoiceNumber as InvoiceNumber,trat.NameAr,sum(Credit) as Credit ,sum(Depit) as Depit , sum(Credit)-sum(Depit) as Diff from Acc_Transactions tra"
                + " join Acc_Invoices inv on tra.InvoiceId = inv.InvoiceId"
                + " join Acc_TransactionTypes trat on trat.TransactionTypeId=tra.Type"
                + " where tra.Type in(" + Type + ") and tra.YearId = " + _globalshared.YearId_G + " and tra.IsDeleted = 0 and inv.IsDeleted = 0 group by tra.YearId,inv.InvoiceId,inv.InvoiceNumber,trat.NameAr";

            }
            else if (Status == 3)
            {
                SelectStetment = "select inv.InvoiceNumber as InvoiceNumber,trat.NameAr,sum(Credit) as Credit ,sum(Depit) as Depit , sum(Credit)-sum(Depit) as Diff from Acc_Transactions tra"
                + " join Acc_Invoices inv on tra.InvoiceId = inv.InvoiceId"
                + " join Acc_TransactionTypes trat on trat.TransactionTypeId=tra.Type"
                + " where tra.Type in(" + Type + ") and tra.YearId = " + _globalshared.YearId_G + " and tra.IsDeleted = 0 and inv.IsDeleted = 0 group by tra.YearId,inv.InvoiceId,inv.InvoiceNumber,trat.NameAr"
                + " having sum(Credit)-sum(Depit) != 0";
            }
            else
            {
                SelectStetment = "select inv.InvoiceNumber as InvoiceNumber,trat.NameAr,sum(Credit) as Credit ,sum(Depit) as Depit , sum(Credit)-sum(Depit) as Diff from Acc_Transactions tra"
                + " join Acc_Invoices inv on tra.InvoiceId = inv.InvoiceId"
                + " join Acc_TransactionTypes trat on trat.TransactionTypeId=tra.Type"
                + " where tra.YearId = " + _globalshared.YearId_G + " and tra.IsDeleted = 0 and inv.IsDeleted = 0 group by tra.YearId,inv.InvoiceId,inv.InvoiceNumber,trat.NameAr"
                + " having sum(Credit)-sum(Depit) != 0";
            }

            var ViewDetails = _projectservice.GetViewDetailsGrid(Con, SelectStetment);

            return Ok(ViewDetails);

        }
        //سند الافتتاحى
        [HttpGet("OpeningVoucherReport")]

        public IActionResult OpeningVoucherReport(int VoucherId)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
            DataTable InvoicesVM = _voucherService.OpeningVoucherReport(VoucherId, Con).Result;
            ReportPDF = inVoiceReports.PrintOpeningVoucherReport(InvoicesVM, infoDoneTasksReport);

            //dawoudprint
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
        [HttpGet("ConvertNumToString")]
        public static string ConvertNumToString(string Num)
        {
            CurrencyInfo _currencyInfo = new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia);
            ToWord toWord = new ToWord(Convert.ToDecimal(Num), _currencyInfo);
            return (toWord.ConvertToArabic());


        }
        [HttpGet("ConvertToWord_NEW")]

        public static string ConvertToWord_NEW(string Num)
        {
            CurrencyInfo _currencyInfo = new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia);
            ToWord toWord = new ToWord(Convert.ToDecimal(Num), _currencyInfo);
            return toWord.ConvertToArabic();
        }
        [HttpGet("InvTest_PDF")]
        public IActionResult InvTest_PDF(int? InvoiceId, int? TempCheck)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            try
            {
                var url = Path.Combine("Email/testPrint.html");
                var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
                string resultLogoUrl = org.LogoUrl.Remove(0, 1);
                var file = Path.Combine(resultLogoUrl);
                var body = InvoicePopulate("test", "mohammeddawoud", "headertest", "footertest", url, org?.NameAr ?? "tgrebe");
                return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = body });
            }
            catch (Exception ex)
            {
                return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في الطباعة" });
            }

        }
        [HttpGet("InvoicePopulate")]
        public string InvoicePopulate(string bodytxt, string fullname, string header, string footer, string url, string orgname)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(url))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{FullName}", fullname);
            body = body.Replace("{Body}", bodytxt);
            body = body.Replace("{Header}", header);
            body = body.Replace("{Footer}", footer);
            body = body.Replace("{orgname}", orgname);
            return body;
        }
        public class Invoice_PDF
        {
            public InvoicesVM? InvoicesVM_VD { get; set; }
            public CustomerVM? CustomerVM_VD { get; set; }
            public Acc_SuppliersVM? SuppliersVM_VD { get; set; }          
            public string? costCenterNam_VD { get; set; }
            public List<VoucherDetailsVM>? VoucherDetailsVM_VD { get; set; }
            public OrganizationsVM? Org_VD { get; set; }
            public BranchesVM? Branch_VD { get; set; }
            public string? ValueNumString { get; set; }
            public string? QRCodeString_VD { get; set; }
            public bool? OrgIsRequired_VD { get; set; }
            public int? TempCheck { get; set; }
        }
        [HttpGet("QrCodeImage")]

        public Bitmap QrCodeImage(string Qrcode, int width = 250, int height = 250)
        {

            ZXing.Windows.Compatibility.BarcodeWriter barcodeWriter = new ZXing.Windows.Compatibility.BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Width = width,
                    Height = height
                }
            };
            Bitmap QrCode = barcodeWriter.Write(Qrcode);

            return QrCode;
        }
        [HttpGet("ChangeInvoice_PDF")]
        public IActionResult ChangeInvoice_PDF(int? InvoiceId, int? TempCheck)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Invoice_PDF _invoice_PDF = new Invoice_PDF();

            if (InvoiceId.HasValue)
            {
                InvoicesVM InvoicesVM = _voucherService.GetVoucherById(InvoiceId ?? 0).Result;
                CustomerVM CustomerVM = _customerService.GetCustomersByCustomerIdInvoice(InvoicesVM.CustomerId ?? 0, _globalshared.Lang_G).Result;

                var costCenterNam = "";
                if (InvoicesVM.VoucherDetails[0].CostCenterId != null || InvoicesVM.VoucherDetails[0].CostCenterId != 0)
                {
                    try
                    {
                        costCenterNam = _CostCenterservice.GetCostCenterById(InvoicesVM.VoucherDetails[0].CostCenterId ?? 0).Result.NameAr;
                    }
                    catch (Exception)
                    {
                        costCenterNam = "بدون";
                    }
                }
                else
                {
                    costCenterNam = "بدون";
                }
                //string ValueNumString2 = ConvertNumToString(InvoicesVM.TotalValue.ToString());
                string ValueNumString = ConvertToWord_NEW(InvoicesVM.TotalValue.ToString());

                List<VoucherDetailsVM> VoucherDetailsVM = _voucherService.GetAllDetailsByInvoiceId(InvoiceId).Result.ToList();
                int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
                var objOrganization = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
                _invoice_PDF.Org_VD = objOrganization;

                var objBranch = _BranchesService.GetBranchByBranchId(_globalshared.Lang_G, _globalshared.BranchId_G).Result.FirstOrDefault();
                var OrgIsRequired = _systemSettingsService.GetSystemSettingsByBranchId(_globalshared.BranchId_G).Result.OrgDataIsRequired;
                if (OrgIsRequired == true) OrgIsRequired = false; else OrgIsRequired = true;
                foreach (var item in VoucherDetailsVM)
                {
                    var servicesPriOffer = _servicesPriceOfferService.GetServicesPriceVouByParentIdAndInvoiceId(item.ServicesPriceId, item.InvoiceId).Result.ToList();
                    item.ServicesPricesOffer = servicesPriOffer;
                }


                _invoice_PDF.Branch_VD = objBranch;
                _invoice_PDF.CustomerVM_VD = CustomerVM;
                _invoice_PDF.InvoicesVM_VD = InvoicesVM;
                _invoice_PDF.costCenterNam_VD = costCenterNam;
                _invoice_PDF.VoucherDetailsVM_VD = VoucherDetailsVM;
                _invoice_PDF.ValueNumString = ValueNumString;
                _invoice_PDF.OrgIsRequired_VD = OrgIsRequired;
                _invoice_PDF.TempCheck = TempCheck;


                string Time_V = DateTime.Now.ToString("hh:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                string FullDate = InvoicesVM.Date + " " + Time_V;
                
                DateTime parsedDate;
                bool isValidDate = DateTime.TryParse(FullDate, out parsedDate);
                if (isValidDate)
                    Console.WriteLine(parsedDate);
                else
                    parsedDate = DateTime.Now;

                var SupplierName = objOrganization.NameAr.TrimStart();
                SupplierName = SupplierName.TrimEnd();

                var ReqInv= _invoicesRequestsService.GetInvoiceReq(InvoiceId??0).Result;
                Bitmap QrCodeV = null;
                if (ReqInv!=null)
                {
                    QrCodeV= QrCodeImage(ReqInv.QRCode, 600, 600);
                }
                try
                {
                    string ImgReturn = "";
                    TLVCls tlv = new TLVCls(SupplierName, objOrganization.TaxCode, parsedDate, (double)InvoicesVM.TotalValue, (double)InvoicesVM.TaxAmount);
                    if(QrCodeV==null)
                    {
                        QrCodeV = tlv.toQrCode();
                    }
                    using (Bitmap bitMap = QrCodeV)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            byte[] byteImage = ms.ToArray();
                            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                            ImgReturn = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                        }
                    }
                    _invoice_PDF.QRCodeString_VD = ImgReturn;

                }
                catch (Exception ex)
                {
                    _invoice_PDF.QRCodeString_VD = null;
                }
                //Convert Images To Base64
                #region
                string base64String = "";
                if (_invoice_PDF.Org_VD.LogoUrl != null && _invoice_PDF.Org_VD.LogoUrl != "")
                {
                    base64String = "";
                    _invoice_PDF.Org_VD.LogoUrl = _invoice_PDF.Org_VD.LogoUrl.Substring(1);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(_invoice_PDF.Org_VD.LogoUrl);
                    base64String = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    _invoice_PDF.Org_VD.LogoUrl = base64String;
                }
                if (_invoice_PDF.Org_VD.BankIdImgURL != null && _invoice_PDF.Org_VD.BankIdImgURL != "")
                {
                    base64String = "";
                    _invoice_PDF.Org_VD.BankIdImgURL = _invoice_PDF.Org_VD.BankIdImgURL.Substring(1);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(_invoice_PDF.Org_VD.BankIdImgURL);
                    base64String = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    _invoice_PDF.Org_VD.BankIdImgURL = base64String;
                }
                if (_invoice_PDF.Org_VD.BankId2ImgURL != null && _invoice_PDF.Org_VD.BankId2ImgURL != "")
                {
                    base64String = "";
                    _invoice_PDF.Org_VD.BankId2ImgURL = _invoice_PDF.Org_VD.BankId2ImgURL.Substring(1);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(_invoice_PDF.Org_VD.BankId2ImgURL);
                    base64String = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    _invoice_PDF.Org_VD.BankId2ImgURL = base64String;
                }
                if (_invoice_PDF.Branch_VD!.BranchLogoUrl != null && _invoice_PDF.Branch_VD!.BranchLogoUrl != "")
                {
                    base64String = "";
                    _invoice_PDF.Branch_VD.BranchLogoUrl = _invoice_PDF.Branch_VD.BranchLogoUrl.Substring(1);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(_invoice_PDF.Branch_VD.BranchLogoUrl);
                    base64String = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    _invoice_PDF.Branch_VD.BranchLogoUrl = base64String;
                }
                if (_invoice_PDF.Branch_VD!.HeaderLogoUrl != null && _invoice_PDF.Branch_VD!.HeaderLogoUrl != "")
                {
                    base64String = "";
                    _invoice_PDF.Branch_VD.HeaderLogoUrl = _invoice_PDF.Branch_VD.HeaderLogoUrl.Substring(1);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(_invoice_PDF.Branch_VD.HeaderLogoUrl);
                    base64String = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    _invoice_PDF.Branch_VD.HeaderLogoUrl = base64String;
                }
                if (_invoice_PDF.Branch_VD!.FooterLogoUrl != null && _invoice_PDF.Branch_VD!.FooterLogoUrl != "")
                {
                    base64String = "";
                    _invoice_PDF.Branch_VD.FooterLogoUrl = _invoice_PDF.Branch_VD.FooterLogoUrl.Substring(1);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(_invoice_PDF.Branch_VD.FooterLogoUrl);
                    base64String = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    _invoice_PDF.Branch_VD.FooterLogoUrl = base64String;
                }
                if (_invoice_PDF.Branch_VD!.BankIdImgURL != null && _invoice_PDF.Branch_VD!.BankIdImgURL != "")
                {
                    base64String = "";
                    _invoice_PDF.Branch_VD.BankIdImgURL = _invoice_PDF.Branch_VD.BankIdImgURL.Substring(1);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(_invoice_PDF.Branch_VD.BankIdImgURL);
                    base64String = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    _invoice_PDF.Branch_VD.BankIdImgURL = base64String;
                }
                if (_invoice_PDF.Branch_VD!.BankId2ImgURL != null && _invoice_PDF.Branch_VD!.BankId2ImgURL != "")
                {
                    base64String = "";
                    _invoice_PDF.Branch_VD.BankId2ImgURL = _invoice_PDF.Branch_VD.BankId2ImgURL.Substring(1);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(_invoice_PDF.Branch_VD.BankId2ImgURL);
                    base64String = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    _invoice_PDF.Branch_VD.BankId2ImgURL = base64String;
                }
                #endregion
                // End Convert Images To Base64

            }
            else
            {
                _invoice_PDF.Org_VD = null;
                _invoice_PDF.CustomerVM_VD = null;
                _invoice_PDF.InvoicesVM_VD = null;
                _invoice_PDF.costCenterNam_VD = null;
                _invoice_PDF.VoucherDetailsVM_VD = null;
                _invoice_PDF.ValueNumString = null;
                _invoice_PDF.QRCodeString_VD = null;
                _invoice_PDF.Branch_VD = null;
            }
            return Ok(_invoice_PDF);
        }

        [HttpGet("ChangeInvoice_PDFCredit")]
        public ActionResult ChangeInvoice_PDFCredit(int? InvoiceId, int? TempCheck)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Invoice_PDF _invoice_PDF = new Invoice_PDF();
            if (InvoiceId.HasValue)
            {
                DataTable voucher = new DataTable();
                if (TempCheck == 29)
                    voucher = _voucherService.ReceiptCashingPayingNoti(InvoiceId ?? 0, Con).Result;
                else if (TempCheck == 30)
                    voucher = _voucherService.ReceiptCashingPayingNotiDepit(InvoiceId ?? 0, Con).Result;
                else
                    voucher = _voucherService.ReceiptCashingPayingNoti(InvoiceId ?? 0, Con).Result;

                var InvId2 = Convert.ToInt32(voucher.Rows[0]["InvoiceId"].ToString());
                List<VoucherDetailsVM> VoucherDetailsVM = _voucherService.GetAllDetailsByInvoiceId(InvId2).Result.ToList();


                InvoicesVM InvoicesVM = _voucherService.GetVoucherById(InvId2).Result;
                CustomerVM CustomerVM = _customerService.GetCustomersByCustomerIdInvoice(InvoicesVM.CustomerId ?? 0, _globalshared.Lang_G).Result;

                var costCenterNam = "";
                if (InvoicesVM.VoucherDetails[0].CostCenterId != null || InvoicesVM.VoucherDetails[0].CostCenterId != 0)
                {
                    try
                    {
                        costCenterNam = _CostCenterservice.GetCostCenterById(InvoicesVM.VoucherDetails[0].CostCenterId ?? 0).Result.NameAr;
                    }
                    catch (Exception)
                    {
                        costCenterNam = "بدون";
                    }
                }
                else
                {
                    costCenterNam = "بدون";
                }
                string ValueNumString = ConvertToWord_NEW(InvoicesVM.TotalValue.ToString());


                int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
                var objOrganization = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
                _invoice_PDF.Org_VD = objOrganization;



                var objBranch = _BranchesService.GetBranchByBranchId(_globalshared.Lang_G, _globalshared.BranchId_G).Result.FirstOrDefault();
                var OrgIsRequired = _systemSettingsService.GetSystemSettingsByBranchId(_globalshared.BranchId_G).Result.OrgDataIsRequired;
                if (OrgIsRequired == true) OrgIsRequired = false; else OrgIsRequired = true;

                foreach (var item in VoucherDetailsVM)
                {
                    var servicesPriOffer = _servicesPriceOfferService.GetServicesPriceVouByParentIdAndInvoiceId(item.ServicesPriceId, item.InvoiceId).Result.ToList();
                    item.ServicesPricesOffer = servicesPriOffer;
                }

                _invoice_PDF.Branch_VD = objBranch;
                _invoice_PDF.CustomerVM_VD = CustomerVM;
                _invoice_PDF.InvoicesVM_VD = InvoicesVM;
                _invoice_PDF.costCenterNam_VD = costCenterNam;
                _invoice_PDF.VoucherDetailsVM_VD = VoucherDetailsVM;
                _invoice_PDF.ValueNumString = ValueNumString;
                _invoice_PDF.OrgIsRequired_VD = OrgIsRequired;
                _invoice_PDF.TempCheck = TempCheck;


                string Time_V = DateTime.Now.ToString("hh:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                string FullDate = InvoicesVM.Date + " " + Time_V;

                DateTime parsedDate;
                bool isValidDate = DateTime.TryParse(FullDate, out parsedDate);
                if (isValidDate)
                    Console.WriteLine(parsedDate);
                else
                    parsedDate = DateTime.Now;

                var SupplierName = objOrganization.NameAr.TrimStart();
                SupplierName = SupplierName.TrimEnd();
                var ReqInv = _invoicesRequestsService.GetInvoiceReq(InvoiceId??0).Result;
                Bitmap QrCodeV = null;
                if (ReqInv != null)
                {
                    QrCodeV = QrCodeImage(ReqInv.QRCode, 600, 600);
                }
                try
                {
                    string ImgReturn = "";
                    TLVCls tlv = new TLVCls(SupplierName, objOrganization.TaxCode, parsedDate, (double)InvoicesVM.TotalValue, (double)InvoicesVM.TaxAmount);
                    if (QrCodeV == null)
                    {
                        QrCodeV = tlv.toQrCode();
                    }
                    using (Bitmap bitMap = QrCodeV)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            byte[] byteImage = ms.ToArray();
                            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                            ImgReturn = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                        }
                    }
                    _invoice_PDF.QRCodeString_VD = ImgReturn;

                }
                catch (Exception ex)
                {
                    _invoice_PDF.QRCodeString_VD = null;
                }

                //Convert Images To Base64
                #region
                string base64String = "";
                if (_invoice_PDF.Org_VD.LogoUrl != null && _invoice_PDF.Org_VD.LogoUrl != "")
                {
                    base64String = "";
                    _invoice_PDF.Org_VD.LogoUrl = _invoice_PDF.Org_VD.LogoUrl.Substring(1);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(_invoice_PDF.Org_VD.LogoUrl);
                    base64String = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    _invoice_PDF.Org_VD.LogoUrl = base64String;
                }
                if (_invoice_PDF.Org_VD.BankIdImgURL != null && _invoice_PDF.Org_VD.BankIdImgURL != "")
                {
                    base64String = "";
                    _invoice_PDF.Org_VD.BankIdImgURL = _invoice_PDF.Org_VD.BankIdImgURL.Substring(1);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(_invoice_PDF.Org_VD.BankIdImgURL);
                    base64String = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    _invoice_PDF.Org_VD.BankIdImgURL = base64String;
                }
                if (_invoice_PDF.Org_VD.BankId2ImgURL != null && _invoice_PDF.Org_VD.BankId2ImgURL != "")
                {
                    base64String = "";
                    _invoice_PDF.Org_VD.BankId2ImgURL = _invoice_PDF.Org_VD.BankId2ImgURL.Substring(1);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(_invoice_PDF.Org_VD.BankId2ImgURL);
                    base64String = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    _invoice_PDF.Org_VD.BankId2ImgURL = base64String;
                }
                if (_invoice_PDF.Branch_VD!.BranchLogoUrl != null && _invoice_PDF.Branch_VD!.BranchLogoUrl != "")
                {
                    base64String = "";
                    _invoice_PDF.Branch_VD.BranchLogoUrl = _invoice_PDF.Branch_VD.BranchLogoUrl.Substring(1);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(_invoice_PDF.Branch_VD.BranchLogoUrl);
                    base64String = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    _invoice_PDF.Branch_VD.BranchLogoUrl = base64String;
                }
                if (_invoice_PDF.Branch_VD!.HeaderLogoUrl != null && _invoice_PDF.Branch_VD!.HeaderLogoUrl != "")
                {
                    base64String = "";
                    _invoice_PDF.Branch_VD.HeaderLogoUrl = _invoice_PDF.Branch_VD.HeaderLogoUrl.Substring(1);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(_invoice_PDF.Branch_VD.HeaderLogoUrl);
                    base64String = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    _invoice_PDF.Branch_VD.HeaderLogoUrl = base64String;
                }
                if (_invoice_PDF.Branch_VD!.FooterLogoUrl != null && _invoice_PDF.Branch_VD!.FooterLogoUrl != "")
                {
                    base64String = "";
                    _invoice_PDF.Branch_VD.FooterLogoUrl = _invoice_PDF.Branch_VD.FooterLogoUrl.Substring(1);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(_invoice_PDF.Branch_VD.FooterLogoUrl);
                    base64String = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    _invoice_PDF.Branch_VD.FooterLogoUrl = base64String;
                }
                if (_invoice_PDF.Branch_VD!.BankIdImgURL != null && _invoice_PDF.Branch_VD!.BankIdImgURL != "")
                {
                    base64String = "";
                    _invoice_PDF.Branch_VD.BankIdImgURL = _invoice_PDF.Branch_VD.BankIdImgURL.Substring(1);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(_invoice_PDF.Branch_VD.BankIdImgURL);
                    base64String = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    _invoice_PDF.Branch_VD.BankIdImgURL = base64String;
                }
                if (_invoice_PDF.Branch_VD!.BankId2ImgURL != null && _invoice_PDF.Branch_VD!.BankId2ImgURL != "")
                {
                    base64String = "";
                    _invoice_PDF.Branch_VD.BankId2ImgURL = _invoice_PDF.Branch_VD.BankId2ImgURL.Substring(1);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(_invoice_PDF.Branch_VD.BankId2ImgURL);
                    base64String = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    _invoice_PDF.Branch_VD.BankId2ImgURL = base64String;
                }
                #endregion
                // End Convert Images To Base64
            }
            else
            {
                _invoice_PDF.Org_VD = null;
                _invoice_PDF.CustomerVM_VD = null;
                _invoice_PDF.InvoicesVM_VD = null;
                _invoice_PDF.costCenterNam_VD = null;
                _invoice_PDF.VoucherDetailsVM_VD = null;
                _invoice_PDF.ValueNumString = null;
                _invoice_PDF.QRCodeString_VD = null;
                _invoice_PDF.Branch_VD = null;
            }
            return Ok(_invoice_PDF);
        }



        [HttpGet("ChangeInvoice_PDFCreditPurchase")]

        public IActionResult ChangeInvoice_PDFCreditPurchase(int? InvoiceId, int? TempCheck)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Invoice_PDF _invoice_PDF = new Invoice_PDF();

            if (InvoiceId.HasValue)
            {
                var InvId2 = _voucherService.GetVoucherByIdNotiDepit_Purchase(InvoiceId ?? 0).InvoiceId;
                List<VoucherDetailsVM> VoucherDetailsVM = _voucherService.GetAllDetailsByInvoiceIdPurchase(InvId2).Result.ToList();
                InvoicesVM InvoicesVM = _voucherService.GetVoucherByIdPurchase(InvId2).Result;
                Acc_SuppliersVM SuppliersVM = _acc_SuppliersService.GetSupplierByID(InvoicesVM.SupplierId ?? 0).Result;

                var costCenterNam = "";
                if (InvoicesVM.VoucherDetails[0].CostCenterId != null || InvoicesVM.VoucherDetails[0].CostCenterId != 0)
                {
                    try
                    {
                        costCenterNam = _CostCenterservice.GetCostCenterById(InvoicesVM.VoucherDetails[0].CostCenterId ?? 0).Result.NameAr;
                    }
                    catch (Exception)
                    {
                        costCenterNam = "بدون";
                    }
                }
                else
                {
                    costCenterNam = "بدون";
                }
                string ValueNumString = ConvertToWord_NEW(InvoicesVM.TotalValue.ToString());
                int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
                var objOrganization = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
                _invoice_PDF.Org_VD = objOrganization;

                var objBranch = _BranchesService.GetBranchByBranchId(_globalshared.Lang_G, _globalshared.BranchId_G).Result.FirstOrDefault();
                var OrgIsRequired = _systemSettingsService.GetSystemSettingsByBranchId(_globalshared.BranchId_G).Result.OrgDataIsRequired;
                if (OrgIsRequired == true) OrgIsRequired = false; else OrgIsRequired = true;

                _invoice_PDF.Branch_VD = objBranch;
                _invoice_PDF.SuppliersVM_VD = SuppliersVM;
                _invoice_PDF.InvoicesVM_VD = InvoicesVM;
                _invoice_PDF.costCenterNam_VD = costCenterNam;
                _invoice_PDF.VoucherDetailsVM_VD = VoucherDetailsVM;
                _invoice_PDF.ValueNumString = ValueNumString;
                _invoice_PDF.OrgIsRequired_VD = OrgIsRequired;
                _invoice_PDF.TempCheck = TempCheck;

                string Time_V = DateTime.Now.ToString("hh:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                string FullDate = InvoicesVM.Date + " " + Time_V;

                DateTime parsedDate;
                bool isValidDate = DateTime.TryParse(FullDate, out parsedDate);
                if (isValidDate)
                    Console.WriteLine(parsedDate);
                else
                    parsedDate = DateTime.Now;

                var SupplierName = SuppliersVM.NameAr.TrimStart();
                SupplierName = SupplierName.TrimEnd();
                try
                {
                    string ImgReturn = "";
                    TLVCls tlv = new TLVCls(SupplierName, objOrganization.TaxCode, parsedDate, (double)InvoicesVM.TotalValue, (double)InvoicesVM.TaxAmount);
                    using (Bitmap bitMap = tlv.toQrCode())
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            byte[] byteImage = ms.ToArray();
                            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                            ImgReturn = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                        }
                    }
                    _invoice_PDF.QRCodeString_VD = ImgReturn;

                }
                catch (Exception ex)
                {
                    _invoice_PDF.QRCodeString_VD = null;
                }
            }
            else
            {
                _invoice_PDF.Org_VD = null;
                _invoice_PDF.SuppliersVM_VD = null;
                _invoice_PDF.InvoicesVM_VD = null;
                _invoice_PDF.costCenterNam_VD = null;
                _invoice_PDF.VoucherDetailsVM_VD = null;
                _invoice_PDF.ValueNumString = null;
                _invoice_PDF.QRCodeString_VD = null;
                _invoice_PDF.Branch_VD = null;
            }
            return Ok(_invoice_PDF);
        }

        [HttpGet("ChangePurchase_PDF")]
        public IActionResult ChangePurchase_PDF(int? InvoiceId, int? TempCheck)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            Invoice_PDF _invoice_PDF = new Invoice_PDF();

            if (InvoiceId.HasValue)
            {
                InvoicesVM InvoicesVM = _voucherService.GetVoucherByIdPurchase(InvoiceId ?? 0).Result;
                Acc_SuppliersVM SuppliersVM = _acc_SuppliersService.GetSupplierByID(InvoicesVM.SupplierId ?? 0).Result;

                var costCenterNam = "";
                if (InvoicesVM.VoucherDetails[0].CostCenterId != null || InvoicesVM.VoucherDetails[0].CostCenterId != 0)
                {
                    try
                    {
                        costCenterNam = _CostCenterservice.GetCostCenterById(InvoicesVM.VoucherDetails[0].CostCenterId ?? 0).Result.NameAr;
                    }
                    catch (Exception)
                    {
                        costCenterNam = "بدون";
                    }
                }
                else
                {
                    costCenterNam = "بدون";
                }
                //string ValueNumString2 = ConvertNumToString(InvoicesVM.TotalValue.ToString());
                string ValueNumString = ConvertToWord_NEW(InvoicesVM.TotalValue.ToString());

                List<VoucherDetailsVM> VoucherDetailsVM = _voucherService.GetAllDetailsByInvoiceIdPurchase(InvoiceId).Result.ToList();
                int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
                var objOrganization = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
                _invoice_PDF.Org_VD = objOrganization;


                var objBranch = _BranchesService.GetBranchByBranchId(_globalshared.Lang_G, _globalshared.BranchId_G).Result.FirstOrDefault();
                var OrgIsRequired = _systemSettingsService.GetSystemSettingsByBranchId(_globalshared.BranchId_G).Result.OrgDataIsRequired;
                if (OrgIsRequired == true) OrgIsRequired = false; else OrgIsRequired = true;

                _invoice_PDF.Branch_VD = objBranch;
                _invoice_PDF.SuppliersVM_VD = SuppliersVM;
                _invoice_PDF.InvoicesVM_VD = InvoicesVM;
                _invoice_PDF.costCenterNam_VD = costCenterNam;
                _invoice_PDF.VoucherDetailsVM_VD = VoucherDetailsVM;
                _invoice_PDF.ValueNumString = ValueNumString;
                _invoice_PDF.OrgIsRequired_VD = OrgIsRequired;
                _invoice_PDF.TempCheck = TempCheck;


                string Time_V = DateTime.Now.ToString("hh:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                string FullDate = InvoicesVM.Date + " " + Time_V;

                DateTime parsedDate;
                bool isValidDate = DateTime.TryParse(FullDate, out parsedDate);
                if (isValidDate)
                    Console.WriteLine(parsedDate);
                else
                    parsedDate = DateTime.Now;

                var SupplierName = SuppliersVM.NameAr.TrimStart();
                SupplierName = SupplierName.TrimEnd();
                try
                {
                    string ImgReturn = "";
                    TLVCls tlv = new TLVCls(SupplierName, objOrganization.TaxCode, parsedDate, (double)InvoicesVM.TotalValue, (double)InvoicesVM.TaxAmount);
                    using (Bitmap bitMap = tlv.toQrCode())
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            byte[] byteImage = ms.ToArray();
                            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                            ImgReturn = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                        }
                    }
                    _invoice_PDF.QRCodeString_VD = ImgReturn;

                }
                catch (Exception ex)
                {
                    _invoice_PDF.QRCodeString_VD = null;
                }
            }
            else
            {
                _invoice_PDF.Org_VD = null;
                _invoice_PDF.SuppliersVM_VD = null;
                _invoice_PDF.InvoicesVM_VD = null;
                _invoice_PDF.costCenterNam_VD = null;
                _invoice_PDF.VoucherDetailsVM_VD = null;
                _invoice_PDF.ValueNumString = null;
                _invoice_PDF.QRCodeString_VD = null;
                _invoice_PDF.Branch_VD = null;
            }
            return Ok(_invoice_PDF);
        }

        [HttpGet("GenerateRandomNo")]
        public int GenerateRandomNo()
        {
            int _min = 100000;
            int _max = 999999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }




        // Convert HTML TO XML
        [HttpGet("ConvertHTMLTOXML")]
        public void ConvertHTMLTOXML()
        {
            Spire.Doc.Document doc = new Spire.Doc.Document();
            doc.LoadFromFile("Sample.html");
            doc.SaveToFile("test.xml", FileFormat.Xml);
        }

        //END



        [HttpGet("FillFilteringSelect")]
        public IActionResult FillFilteringSelect(int Param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            if (Param == 2)
            {
                var Customers = _customerService.GetAllCustomerForDrop(_globalshared.Lang_G).Result.Select(s => new {
                    Id = s.Id,
                    Name = s.Name
                });
                return Ok(Customers);
            }
            else if (Param == 3)
            {
                var Suppliers = _acc_SuppliersService.GetAllSuppliers("", _globalshared.BranchId_G, _globalshared.YearId_G).Result.Select(s => new {
                    Id = s.SupplierId,
                    Name = s.NameAr
                });
                return Ok(Suppliers);
            }
            else if (Param == 1)
            {
                var someProject = _projectservice.GetAllProjectAllBranches().Result.Select(s => new {
                    Id = s.ProjectId,
                    Name = s.ProjectNo
                });
                return Ok(someProject);
            }
            else if (Param == 5)
            {
                var Employees = _employesService.GetAllBranchEmployees(_globalshared.Lang_G).Result.Select(s => new {
                    Id = s.EmployeeId,
                    Name = s.EmployeeName
                });
                return Ok(Employees);
            }
            else if (Param == 4)
            {

                var Branches = _BranchesService.GetAllBranches(_globalshared.Lang_G).Result.Select(s => new {
                    Id = s.BranchId,
                    Name = s.BranchName
                });
                return Ok(Branches);

            }
            else if (Param == 6)
            {
                var ServicePrices = _servicesPriceService.GetAllServicesPrice().Result.Select(s => new {
                    Id = s.ServicesId,
                    Name = s.ServicesName
                });
                return Ok(ServicePrices);
            }
            else
            {
                List<Branch> branch = new List<Branch>();
                return Ok(branch);
            }

        }
        [HttpGet("FillFilteringIncomeStateSelect")]
        public IActionResult FillFilteringIncomeStateSelect(int Param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            if (Param > 0 && Param <= 10)
            {
                var PeriodCounterList = GetPeriodCounterList();
                return Ok(PeriodCounterList);
            }
            else if (Param == 12)
            {
                var Customers = _customerService.GetAllCustomerForDrop(_globalshared.Lang_G).Result.Select(s => new {
                    Id = s.Id,
                    Name = s.Name
                });
                return Ok(Customers);
            }
            else if (Param == 13)
            {
                var Suppliers = _acc_SuppliersService.GetAllSuppliers("", _globalshared.BranchId_G, _globalshared.YearId_G).Result.Select(s => new {
                    Id = s.SupplierId,
                    Name = s.NameAr
                });
                return Ok(Suppliers);
            }
            else if (Param == 11)
            {
                var someProject = _projectservice.GetAllProjectAllBranches().Result.Select(s => new {
                    Id = s.ProjectId,
                    Name = s.ProjectNo
                });
                return Ok(someProject);
            }
            else if (Param == 15)
            {
                var Employees = _employesService.GetAllBranchEmployees(_globalshared.Lang_G).Result.Select(s => new {
                    Id = s.EmployeeId,
                    Name = s.EmployeeName
                });
                return Ok(Employees);
            }
            else if (Param == 14)
            {

                var Branches = _BranchesService.GetAllBranches(_globalshared.Lang_G).Result.Select(s => new {
                    Id = s.BranchId,
                    Name = s.BranchName
                });
                return Ok(Branches);

            }
            else if (Param == 16)
            {
                var ServicePrices = _servicesPriceService.GetAllServicesPrice().Result.Select(s => new {
                    Id = s.ServicesId,
                    Name = s.ServicesName
                });
                return Ok(ServicePrices);
            }
            else
            {
                List<PeriodCounter> PeriodCounterv = new List<PeriodCounter>();
                return Ok(PeriodCounterv);
            }

        }
        [HttpGet("GetPeriodCounterList")]
        public List<PeriodCounter> GetPeriodCounterList()
        {
            try
            {
                List<PeriodCounter> PeriodCounterList = new List<PeriodCounter>
                {
                    new PeriodCounter { Id = 0, Name = "--أختر--"},
                    new PeriodCounter { Id = 1, Name = "فترة"},
                    new PeriodCounter { Id = 2, Name = "فترتين"},
                    new PeriodCounter { Id = 3, Name = "3 فترات"},
                    new PeriodCounter { Id = 4, Name = "4 فترات"},
                    new PeriodCounter { Id = 5, Name = "5 فترات"},
                    new PeriodCounter { Id = 6, Name = "6 فترات"},
                    new PeriodCounter { Id = 7, Name = "7 فترات"},
                    new PeriodCounter { Id = 8, Name = "8 فترات"},
                    new PeriodCounter { Id = 9, Name = "9 فترات"},
                    new PeriodCounter { Id = 10, Name = "10 فترات"},
                    new PeriodCounter { Id = 11, Name = "11 فترة"},
                    new PeriodCounter { Id = 12, Name = "12 فترة"},
                    new PeriodCounter { Id = 13, Name = "13 فترة"}
                };
                return PeriodCounterList;
            }
            catch (Exception)
            {
                List<PeriodCounter> PeriodCounterv = new List<PeriodCounter>();
                return PeriodCounterv;
            }

        }


        [HttpPost("GetFinancialfollowup")]
        public IActionResult GetFinancialfollowup([FromForm] FinancialfollowupVM _financialfollowupVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            _financialfollowupVM.BranchId = _globalshared.BranchId_G;
            _financialfollowupVM.YearId = _globalshared.YearId_G;

            var AllInvoices = _voucherService.GetFinancialfollowup(Con ?? "", _financialfollowupVM).Result.ToList();
            return Ok(AllInvoices);
        }





        [HttpPost("GetFinancialfollowup_pageing")]
        public IActionResult GetFinancialfollowup_pageing([FromForm] FinancialfollowupVM _financialfollowupVM)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            _financialfollowupVM.BranchId = _globalshared.BranchId_G;
            _financialfollowupVM.YearId = _globalshared.YearId_G;
            if (_financialfollowupVM.SearchText == null || _financialfollowupVM.SearchText == "")
            {
                var AllInvoices = _voucherService.GetFinancialfollowup(Con ?? "", _financialfollowupVM).Result.ToList();
                var data = GeneratePagination<InvoicesVM>.ToPagedList(AllInvoices.ToList(), _financialfollowupVM.Page.Value, _financialfollowupVM.PageSize.Value);
                var result = new PagedLists<InvoicesVM>(data.MetaData, data);
                return Ok(result);
            }
            else
            {
                var AllInvoices = _voucherService.GetFinancialfollowup(Con ?? "", _financialfollowupVM).Result.ToList().Where(x => x.InvoiceNumber.Contains(_financialfollowupVM.SearchText) ||
           x.InvoiceValue.ToString().Contains(_financialfollowupVM.SearchText) || x.PayTypeName.Contains(_financialfollowupVM.SearchText) ||
            x.CustomerName.Contains(_financialfollowupVM.SearchText) || x.ProjectNo.Contains(_financialfollowupVM.SearchText) || _financialfollowupVM.SearchText == null || _financialfollowupVM.SearchText == "");
                var data = GeneratePagination<InvoicesVM>.ToPagedList(AllInvoices.ToList(), _financialfollowupVM.Page.Value, _financialfollowupVM.PageSize.Value);
                var result = new PagedLists<InvoicesVM>(data.MetaData, data);
                return Ok(result);
            }
        }

        [HttpGet("GetInvoiceReq")]
        public IActionResult GetInvoiceReq(int InvoiceId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var InvoiceRequest = _invoicesRequestsService.GetInvoiceReq(InvoiceId).Result;
            return Ok(InvoiceRequest);
        }

        [HttpGet("GetAllInvoiceRequests")]
        public IActionResult GetAllInvoiceRequests()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var InvoiceRequest = _invoicesRequestsService.GetAllInvoiceRequests(_globalshared.BranchId_G).Result.ToList();
            return Ok(InvoiceRequest);
        }
        [HttpGet("GetAllInvoiceRequestsByInvoiceId")]
        public IActionResult GetAllInvoiceRequestsByInvoiceId(int InvoiceId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var InvoiceRequest = _invoicesRequestsService.GetAllInvoiceRequests(InvoiceId, _globalshared.BranchId_G).Result.ToList();
            return Ok(InvoiceRequest);
        }

        [HttpPost("ZatcaInvoiceIntegrationFunc")]
        public IActionResult ZatcaInvoiceIntegrationFunc(InvoiceObjDet voucherDetObj)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var syssetting = _systemSettingsService.GetSystemSettingsByBranchId(_globalshared.BranchId_G).Result;
            if (syssetting.UploadInvZatca == true)
            {
                var Result = ZatcaInvoiceIntegration(voucherDetObj, _globalshared.BranchId_G, voucherDetObj.voucherDetObjRet??new List<ObjRet>(),0);
                return Ok(Result);
            }
            else
            {
                var resu=new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = " لم يتم التفعيل" };
                return Ok(resu);
            }
        }

        [HttpGet("ZatcaInvoiceIntegration")]
        private GeneralMessage ZatcaInvoiceIntegration(InvoiceObjDet voucherDet, int Branchid,List<ObjRet> vouDetailsRet, int? InvoiceReqId)
        {
            try
            {
                //var voucher = voucherDet.voucherDetObj.FirstOrDefault();
                List<VoucherDetailsVM> VoucherDetailsVM = new List<VoucherDetailsVM>();
                var invoiceIdV = 0; decimal TotalValueDetailes = 0; int type = 0;
                if (vouDetailsRet.Count() > 0)
                {
                    foreach (var ii in vouDetailsRet)
                    {
                        var vouDet = new VoucherDetailsVM();
                        vouDet.InvoiceId = ii.InvoiceId;
                        vouDet.Qty = ii.Qty;
                        vouDet.ServicesPriceName = ii.ServicesPriceName;
                        vouDet.TaxAmount = ii.TaxAmount;
                        vouDet.Amount = ii.Amount;
                        vouDet.TotalAmount = ii.TotalAmount;
                        vouDet.DiscountValue_Det = ii.DiscountValue_Det;
                        vouDet.DiscountPercentage_Det = ii.DiscountPercentage_Det;
                        VoucherDetailsVM.Add(vouDet);
                        invoiceIdV = vouDet.InvoiceId ?? 0;
                        TotalValueDetailes = TotalValueDetailes + vouDet.TotalAmount ?? 0;
                        type = ii.Type ?? 0;
                    }
                }
                else
                {
                    foreach (var ii in voucherDet.voucherDetObj)
                    {
                        var vouDet = _voucherService.GetAllDetailsByVoucherDetailsId(ii).Result;
                        VoucherDetailsVM.Add(vouDet);
                        invoiceIdV = vouDet.InvoiceId ?? 0;
                        TotalValueDetailes = TotalValueDetailes + vouDet.TotalAmount ?? 0;
                    }
                }

                //var voucher = new InvoicesVM();
                if (invoiceIdV == 0)
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "أختر فاتورة" };
                }
                //----------------------------------------------
                InvoicesVM InvoicesVM = _voucherService.GetVoucherById(invoiceIdV).Result;
                if (type != 0)
                {
                    InvoicesVM.Type = type;
                }
                Invoices VoucherCredit = new Invoices();
                if (InvoicesVM.Type == 29)
                {
                    VoucherCredit = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.InvoiceId == InvoicesVM.CreditNotiId)!.FirstOrDefault();
                }
                if (InvoicesVM.Type == 4)
                {
                    VoucherCredit = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.InvoiceId == invoiceIdV)!.FirstOrDefault();
                }
                CustomerVM CustomerVM = _customerService.GetCustomersByCustomerIdInvoice(InvoicesVM.CustomerId ?? 0, _globalshared.Lang_G).Result;

                var objBranch = _BranchesService.GetBranchByBranchId("rtl", Branchid).Result.FirstOrDefault();
                var objOrganization = _organizationsservice.GetBranchOrganizationData(objBranch.OrganizationId ?? 1).Result;
                ZatcaKeys zatcakeys = new ZatcaKeys();
                var OrgIsRequired = _systemSettingsService.GetSystemSettingsByBranchId(_globalshared.BranchId_G).Result.OrgDataIsRequired;
                if (OrgIsRequired == true) OrgIsRequired = false; else OrgIsRequired = true;

                string Address1 = "";
                string BuildingNumber1 = "";
                string StreetName1 = "";
                string Neighborhood1 = "";
                string CityName1 = "";
                string Country1 = "";
                string PostalCode1 = "";
                string PostalCodeFinal1 = "";
                string ExternalPhone1 = "";
                string TaxCode1 = "";
                if (OrgIsRequired == true)
                {
                    Address1 = objOrganization.Address!.Trim();
                    BuildingNumber1 = objOrganization.BuildingNumber!.Trim();
                    StreetName1 = objOrganization.StreetName!.Trim();
                    Neighborhood1 = objOrganization.Neighborhood!.Trim();
                    CityName1 = objOrganization.CityName!.Trim();
                    Country1 = objOrganization.Country!.Trim();
                    PostalCode1 = objOrganization.PostalCode!.Trim();
                    PostalCodeFinal1 = objOrganization.PostalCodeFinal!.Trim();
                    ExternalPhone1 = objOrganization.ExternalPhone!.Trim();
                    TaxCode1 = objOrganization.TaxCode!.Trim();
                    zatcakeys.CSR = objOrganization.CSR;
                    zatcakeys.PrivateKey = objOrganization.PrivateKey;
                    zatcakeys.PublicKey = objOrganization.PublicKey;
                    zatcakeys.SecreteKey = objOrganization.SecreteKey;
                }
                else
                {

                    Address1 = objBranch.Address!.Trim();
                    BuildingNumber1 = objBranch.BuildingNumber!.Trim();
                    StreetName1 = objBranch.StreetName!.Trim();
                    Neighborhood1 = objBranch.Neighborhood!.Trim();
                    CityName1 = objBranch.CityName!.Trim();
                    Country1 = objBranch.Country!.Trim();
                    PostalCode1 = objBranch.PostalCode!.Trim();
                    PostalCodeFinal1 = objBranch.PostalCodeFinal!.Trim();
                    ExternalPhone1 = objBranch.ExternalPhone!.Trim();
                    TaxCode1 = objBranch.TaxCode!.Trim();


                    if (objBranch.Address == null || objBranch.Address == "")
                        Address1 = objOrganization.Address!.Trim();
                    if (objBranch.BuildingNumber == null || objBranch.BuildingNumber == "")
                        BuildingNumber1 = objOrganization.BuildingNumber!.Trim();
                    if (objBranch.StreetName == null || objBranch.StreetName == "")
                        StreetName1 = objOrganization.StreetName!.Trim();
                    if (objBranch.Neighborhood == null || objBranch.Neighborhood == "")
                        Neighborhood1 = objOrganization.Neighborhood!.Trim();
                    if (objBranch.CityName == null || objBranch.CityName == "")
                        CityName1 = objOrganization.CityName!.Trim();
                    if (objBranch.Country == null || objBranch.Country == "")
                        Country1 = objOrganization.Country!.Trim();
                    if (objBranch.PostalCode == null || objBranch.PostalCode == "")
                        PostalCode1 = objOrganization.PostalCode!.Trim();
                    if (objBranch.PostalCodeFinal == null || objBranch.PostalCodeFinal == "")
                        PostalCodeFinal1 = objOrganization.PostalCodeFinal!.Trim();
                    if (objBranch.ExternalPhone == null || objBranch.ExternalPhone == "")
                        ExternalPhone1 = objOrganization.ExternalPhone!.Trim();
                    if (objBranch.TaxCode == null || objBranch.TaxCode == "")
                        TaxCode1 = objOrganization.TaxCode!.Trim();
                    zatcakeys.CSR = objBranch.CSR;
                    zatcakeys.PrivateKey = objBranch.PrivateKey;
                    zatcakeys.PublicKey = objBranch.PublicKey;
                    zatcakeys.SecreteKey = objBranch.SecreteKey;
                    if (objBranch.CSR == null || objBranch.CSR == "")
                    {
                        zatcakeys.CSR = objOrganization.CSR;
                    }
                    if (objBranch.PrivateKey == null || objBranch.PrivateKey == "")
                    {
                        zatcakeys.PrivateKey = objOrganization.PrivateKey;
                    }
                    if (objBranch.PublicKey == null || objBranch.PublicKey == "")
                    {
                        zatcakeys.PublicKey = objOrganization.PublicKey;
                    }
                    if (objBranch.SecreteKey == null || objBranch.SecreteKey == "")
                    {
                        zatcakeys.SecreteKey = objOrganization.SecreteKey;
                    }

                }
                var invoicetypecode = InvoiceTypeEnums.Standared_Invoice;
                var invoicetypecodeName = InvoiceTypeNameEnums.Standared_Invoice;
                if (TotalValueDetailes > 1000)
                {
                    if (InvoicesVM.Type == 2)
                    {
                        invoicetypecode = InvoiceTypeEnums.Standared_Invoice;
                        invoicetypecodeName = InvoiceTypeNameEnums.Standared_Invoice;
                    }
                    if (InvoicesVM.Type == 29 || InvoicesVM.Type == 4)
                    {
                        invoicetypecode = InvoiceTypeEnums.Standard_CreditNote;
                        invoicetypecodeName = InvoiceTypeNameEnums.Standard_CreditNote;
                    }
                    if (InvoicesVM.Type == 30)
                    {
                        invoicetypecode = InvoiceTypeEnums.Standared_DebitNote;
                        invoicetypecodeName = InvoiceTypeNameEnums.Standared_DebitNote;
                    }
                }
                else
                {
                    if (InvoicesVM.Type == 2)
                    {
                        invoicetypecode = InvoiceTypeEnums.Simplified_Invoice;
                        invoicetypecodeName = InvoiceTypeNameEnums.Simplified_Invoice;
                    }
                    if (InvoicesVM.Type == 29 || InvoicesVM.Type == 4)
                    {
                        invoicetypecode = InvoiceTypeEnums.Simplified_CreditNote;
                        invoicetypecodeName = InvoiceTypeNameEnums.Simplified_CreditNote;
                    }
                    if (InvoicesVM.Type == 30)
                    {
                        invoicetypecode = InvoiceTypeEnums.Simplified_DebitNote;
                        invoicetypecodeName = InvoiceTypeNameEnums.Simplified_DebitNote;
                    }
                }

                var NationalID = CustomerVM.CustomerNationalId;
                var schemeID = "NAT";
                if (CustomerVM.CustomerTypeId == 1)//مواطن
                {
                    NationalID = CustomerVM.CustomerNationalId ?? "";
                    schemeID = "NAT";
                    //NAT
                }
                else if (CustomerVM.CustomerTypeId == 2)//مستثمر
                {
                    NationalID = CustomerVM.CommercialRegister ?? "";
                    schemeID = "CRN";
                    //CRN
                }
                else if (CustomerVM.CustomerTypeId == 3)//جهه حكومية
                {
                    NationalID = "";
                    schemeID = "CRN";
                    //مختلفة
                }

                var PreviousPIH = "NWZlY2ViNjZmZmM4NmYzOGQ5NTI3ODZjNmQ2OTZjNzljMmRiYzIzOWRkNGU5MWI0NjcyOWQ3M2EyN2ZiNTdlOQ==";
                int? zatcaVoucherNumber = _voucherService.GenerateVoucherZatcaNumber(_globalshared.BranchId_G, _globalshared.YearId_G)?.Result ?? 0;
                var prevNum = (zatcaVoucherNumber - 1);
                if (prevNum > 0)
                {
                    var prevInvoice = _TaamerProContext.Acc_InvoicesRequests.Where(s => s.InvoiceNoRequest == prevNum && s.BranchId == Branchid);
                    if (prevInvoice.Count() > 0)
                    {
                        PreviousPIH = prevInvoice.FirstOrDefault().InvoiceHash;
                    }
                    else
                    {
                        PreviousPIH = "NWZlY2ViNjZmZmM4NmYzOGQ5NTI3ODZjNmQ2OTZjNzljMmRiYzIzOWRkNGU5MWI0NjcyOWQ3M2EyN2ZiNTdlOQ==";
                    }
                }
                else
                {
                    PreviousPIH = "NWZlY2ViNjZmZmM4NmYzOGQ5NTI3ODZjNmQ2OTZjNzljMmRiYzIzOWRkNGU5MWI0NjcyOWQ3M2EyN2ZiNTdlOQ==";
                }
                //generate

                //----------------------------------------------
                //DateTime Datet = InvoicesVM.AddDate ?? DateTime.Now;
                //string ActionDatet = Date.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));


                ZatcaIntegrationSDK.UBLXML ubl = new ZatcaIntegrationSDK.UBLXML();
                ZatcaIntegrationSDK.Invoice inv = new ZatcaIntegrationSDK.Invoice();
                ZatcaIntegrationSDK.Result res = new ZatcaIntegrationSDK.Result();
                if (InvoicesVM.Type == 29)
                {
                    inv.ID = InvoicesVM.InvoiceRetId;
                }
                else
                {
                    inv.ID = InvoicesVM.InvoiceNumber;
                }
                inv.UUID = Guid.NewGuid().ToString();
                //inv.IssueDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                inv.IssueDate = DateTime.Now.ToString("yyyy-MM-dd");
                inv.IssueTime = DateTime.Now.ToString("HH:mm:ss");

                inv.invoiceTypeCode.id = invoicetypecode;

                inv.invoiceTypeCode.Name = invoicetypecodeName;
                inv.DocumentCurrencyCode = "SAR";
                inv.TaxCurrencyCode = "SAR";

                if (inv.invoiceTypeCode.id == 383 || inv.invoiceTypeCode.id == 381)
                {
                    // فى حالة ان اشعار دائن او مدين فقط هانكتب رقم الفاتورة اللى اصدرنا الاشعار ليها
                    InvoiceDocumentReference invoiceDocumentReference = new InvoiceDocumentReference();
                    invoiceDocumentReference.ID = "Invoice Number: " + VoucherCredit.InvoiceNumber + "; Invoice Issue Date: " + inv.IssueDate + ""; // اجبارى
                    inv.billingReference.invoiceDocumentReferences.Add(invoiceDocumentReference);
                }
                // هنا ممكن اضيف ال pih من قاعدة البيانات  
                //this is previous invoice hash (the invoice hash of last invoice ) res.InvoiceHash
                // for the first invoice and because there is no previous hash we must write this code "NWZlY2ViNjZmZmM4NmYzOGQ5NTI3ODZjNmQ2OTZjNzljMmRiYzIzOWRkNGU5MWI0NjcyOWQ3M2EyN2ZiNTdlOQ=="

                inv.AdditionalDocumentReferencePIH.EmbeddedDocumentBinaryObject = PreviousPIH;

                inv.AdditionalDocumentReferenceICV.UUID = Convert.ToInt64(zatcaVoucherNumber);

                if (inv.invoiceTypeCode.Name.Substring(0, 2) == "01")
                {
                    //supply date mandatory only for standard invoices
                    // فى حالة فاتورة مبسطة وفاتورة ملخصة هانكتب تاريخ التسليم واخر تاريخ التسليم
                    inv.delivery.ActualDeliveryDate = InvoicesVM.Date;
                    // inv.delivery.LatestDeliveryDate = InvoicesVM.Date;
                }
                // 
                // بيانات الدفع 
                // اكواد معين
                // اختيارى كود الدفع
                // payment methods mandatory for return invoice and debit notes and optional for invoices
                string paymentcode = PaymentMethodEnums.Incash;
                if (!string.IsNullOrEmpty(paymentcode))
                {
                    PaymentMeans paymentMeans = new PaymentMeans();
                    paymentMeans.PaymentMeansCode = paymentcode; // optional for invoices - mandatory for return invoice - debit notes
                    if (inv.invoiceTypeCode.id == 383 || inv.invoiceTypeCode.id == 381)
                    {
                        paymentMeans.InstructionNote = "dameged items"; //the reason of return invoice - debit notes // manatory only for return invoice - debit notes 
                    }
                    inv.paymentmeans.Add(paymentMeans);
                }

                // بيانات البائع 
                inv.SupplierParty.partyIdentification.ID = PostalCode1 ?? ""; //هنا رقم السجل التجارى للشركة
                inv.SupplierParty.partyIdentification.schemeID = "CRN";
                inv.SupplierParty.postalAddress.StreetName = StreetName1 ?? ""; // اجبارى
                                                                                //inv.SupplierParty.postalAddress.AdditionalStreetName = "شارع اضافى"; // اختيارى
                inv.SupplierParty.postalAddress.BuildingNumber = BuildingNumber1 ?? "0000"; // اجبارى رقم المبنى
                                                                                            //inv.SupplierParty.postalAddress.PlotIdentification = "9833";
                inv.SupplierParty.postalAddress.CityName = CityName1 ?? "";
                inv.SupplierParty.postalAddress.PostalZone = PostalCodeFinal1 ?? "00000"; // الرقم البريدي
                inv.SupplierParty.postalAddress.CountrySubentity = CityName1 ?? ""; // اسم المحافظة او المدينة مثال (مكة) اختيارى
                inv.SupplierParty.postalAddress.CitySubdivisionName = Neighborhood1 ?? ""; // اسم المنطقة او الحى 
                inv.SupplierParty.postalAddress.country.IdentificationCode = "SA";
                inv.SupplierParty.partyLegalEntity.RegistrationName = objOrganization.NameAr; // "شركة الصناعات الغذائية المتحده"; // اسم الشركة المسجل فى الهيئة
                inv.SupplierParty.partyTaxScheme.CompanyID = TaxCode1;// "300518376300003";  // رقم التسجيل الضريبي
                if (inv.invoiceTypeCode.Name.Substring(0, 2) == "01")
                {
                    //خدمة تعليمية تقدم للمواطن السعودى - خدمة صحية تقدم للمواطن السعودى
                    if (!(NationalID == null || NationalID == ""))
                    {
                        inv.CustomerParty.partyIdentification.ID = NationalID ?? ""; // رقم القومى الخاض بالمشترى
                        inv.CustomerParty.partyIdentification.schemeID = schemeID; // الرقم القومى
                    }

                    inv.CustomerParty.postalAddress.StreetName = CustomerVM.StreetName ?? ""; // اجبارى
                    inv.CustomerParty.postalAddress.AdditionalStreetName = "شارع اضافى"; // اختيارى
                    inv.CustomerParty.postalAddress.BuildingNumber = CustomerVM.BuildingNumber ?? "0000"; // اجبارى رقم المبنى
                    inv.CustomerParty.postalAddress.PlotIdentification = "9833"; // اختيارى رقم القطعة
                    inv.CustomerParty.postalAddress.CityName = CustomerVM.CityName ?? ""; // اسم المدينة
                    inv.CustomerParty.postalAddress.PostalZone = CustomerVM.PostalCodeFinal ?? ""; // الرقم البريدي
                    inv.CustomerParty.postalAddress.CountrySubentity = CustomerVM.CityName ?? ""; // اسم المحافظة او المدينة مثال (مكة) اختيارى
                    inv.CustomerParty.postalAddress.CitySubdivisionName = CustomerVM.Neighborhood ?? ""; // اسم المنطقة او الحى 
                    inv.CustomerParty.postalAddress.country.IdentificationCode = "SA";
                    inv.CustomerParty.partyLegalEntity.RegistrationName = CustomerVM.CustomerNameAr; // اسم الشركة المسجل فى الهيئة

                    if (!(CustomerVM.CommercialRegInvoice == null || CustomerVM.CommercialRegInvoice == ""))
                    {
                        inv.CustomerParty.partyTaxScheme.CompanyID = CustomerVM.CommercialRegInvoice; // رقم التسجيل الضريبي
                    }
                }


                decimal invoicediscount = InvoicesVM.DiscountValue ?? 0;
                if (invoicediscount > 0)
                {
                    //this code incase of there is a discount in invoice level 
                    AllowanceCharge allowance = new AllowanceCharge();
                    //ChargeIndicator = false means that this is discount
                    //ChargeIndicator = true means that this is charges(like cleaning service - transportation)
                    allowance.ChargeIndicator = false;
                    //write this lines in case you will make discount as percentage
                    allowance.MultiplierFactorNumeric = 0; //dscount percentage like 10
                    allowance.BaseAmount = 0; // the amount we will apply percentage on example (MultiplierFactorNumeric=10 ,BaseAmount=1000 then AllowanceAmount will be 100 SAR)

                    // in case we will make discount as Amount 
                    allowance.Amount = invoicediscount; // 
                                                        // allowance.AllowanceChargeReasonCode = "95"; //discount or charge reason code
                    allowance.AllowanceChargeReason = "discount"; //discount or charge reson
                    allowance.taxCategory.ID = "S";// كود الضريبة tax code (S Z O E )
                    allowance.taxCategory.Percent = 15;// نسبة الضريبة tax percentage (0 - 15 - 5 )
                                                       //فى حالة عندى اكثر من خصم بعمل loop على الاسطر السابقة
                    inv.allowanceCharges.Add(allowance);
                }

                //this is the invoice total amount (invoice total with vat) and you can set its value with Zero and i will calculate it from sdk
                inv.legalMonetaryTotal.PayableAmount = TotalValueDetailes;
                // فى حالة فى اكتر من منتج فى الفاتورة هانعمل ليست من invoiceline مثال الكود التالى
                //here we will mention all invoice lines data





                foreach (var item in VoucherDetailsVM)
                {
                    InvoiceLine invline = new InvoiceLine();
                    //Product Quantity
                    invline.InvoiceQuantity = item.Qty ?? 1;
                    //Product Name
                    invline.item.Name = item.ServicesPriceName;
                    //var VatPercentage = objOrganization.VAT?? Convert.ToDecimal(15);
                    var VatPercentage = Convert.ToDecimal(15);

                    if (VatPercentage == 0)
                    {
                        //item Tax code
                        invline.item.classifiedTaxCategory.ID = "Z"; // كود الضريبة
                                                                     //item Tax code
                        invline.taxTotal.TaxSubtotal.taxCategory.ID = "Z"; // كود الضريبة
                                                                           //item Tax Exemption Reason Code mentioned in zatca pdf page(32-33)
                        invline.taxTotal.TaxSubtotal.taxCategory.TaxExemptionReasonCode = "VATEX-SA-35"; // كود الضريبة
                                                                                                         //item Tax Exemption Reason mentioned in zatca pdf page(32-33)
                        invline.taxTotal.TaxSubtotal.taxCategory.TaxExemptionReason = "Medicines and medical equipment"; // كود الضريبة

                    }
                    else
                    {
                        //item Tax code
                        invline.item.classifiedTaxCategory.ID = "S"; // كود الضريبة
                                                                     //item Tax code
                        invline.taxTotal.TaxSubtotal.taxCategory.ID = "S"; // كود الضريبة
                    }
                    //item Tax percentage
                    invline.item.classifiedTaxCategory.Percent = VatPercentage; // نسبة الضريبة
                    invline.taxTotal.TaxSubtotal.taxCategory.Percent = VatPercentage; // نسبة الضريبة
                                                                                      //EncludingVat = false this flag will be false in case you will give me Product Price not including vat
                                                                                      //EncludingVat = true this flag will be true in case you will give me Product Price including vat
                    invline.price.EncludingVat = false;
                    //Product Price
                    invline.price.PriceAmount = item.Amount ?? 0;

                    if (item.DiscountValue_Det > 0)
                    {
                        // incase there is discount in invoice line level
                        AllowanceCharge allowanceCharge = new AllowanceCharge();
                        // فى حالة الرسوم incase of charges
                        // allowanceCharge.ChargeIndicator = true;
                        // فى حالة الخصم incase of discount
                        allowanceCharge.ChargeIndicator = false;

                        allowanceCharge.AllowanceChargeReason = "discount"; // سبب الخصم على مستوى المنتج
                                                                            // allowanceCharge.AllowanceChargeReasonCode = "90"; // سبب الخصم على مستوى المنتج
                        allowanceCharge.Amount = item.DiscountValue_Det ?? 0; // قيم الخصم discount amount or charge amount

                        allowanceCharge.MultiplierFactorNumeric = 0;
                        allowanceCharge.BaseAmount = 0;
                        invline.allowanceCharges.Add(allowanceCharge);
                    }
                    inv.InvoiceLines.Add(invline);
                }

                //inv.cSIDInfo.CertPem = @"MIICoTCCAkegAwIBAgIGAZGd+J7KMAoGCCqGSM49BAMCMBUxEzARBgNVBAMMCmVJbnZvaWNpbmcwHhcNMjQwODI5MTE0OTU3WhcNMjkwODI4MjEwMDAwWjCBljELMAkGA1UEBhMCU0ExIzAhBgNVBAsMGtin2YTZgdix2LkgINin2YTYsdim2YrYs9mJMU8wTQYDVQQKDEbYtNix2YPYqSDYp9io2K/Yp9i5INin2YTYqtmF2YrYsiDZhNmE2KfYs9iq2LTYp9ix2KfYqiDYp9mE2YfZhtiv2LPZitipMREwDwYDVQQDDAhDb21wYW55MTBWMBAGByqGSM49AgEGBSuBBAAKA0IABEHHq9d2AmkGJBm8csZTCKzwpKLtoJS7CrvyEkWDFCQH75FbqY5pSvX34bUA+8X3iL3BvPXnPex/I6Ns4jRWt+ujggECMIH/MAwGA1UdEwEB/wQCMAAwge4GA1UdEQSB5jCB46SB4DCB3TFHMEUGA1UEBAw+MS1UYW1lZXJ8Mi12ZXJzaW9uMi4wLjF8My01ZGQ3Nzk1My0wY2ZjLTQ3NGItOTU3ZS0xNjViNDk4MzEzNTkxHzAdBgoJkiaJk/IsZAEBDA8zMTEzNjg3MTUxMDAwMDMxDTALBgNVBAwMBDExMDAxQTA/BgNVBBoMONin2YTYr9mF2KfZhSAtINi32LHZitmCINin2YTZhdmE2YMg2YHZh9ivIC0g2K3ZiiDYo9it2K8gMR8wHQYDVQQPDBZFbmdpbmVlcmluZyBjb25zdWx0YW50MAoGCCqGSM49BAMCA0gAMEUCIEIkC/sl/Tr7LmtJhBcvn2du9KqXjJUs1kqS81CKcIJ5AiEApIaqOKqdUCBkfwNsiTQBuAk0yNEbINMyd0cg0WxQ2Vo=";
                //inv.cSIDInfo.PrivateKey = @"MHQCAQEEIO2JfpYk9bQmscmEdy41bML3muCylfxsZndFvoncay3GoAcGBSuBBAAKoUQDQgAEQcer13YCaQYkGbxyxlMIrPCkou2glLsKu/ISRYMUJAfvkVupjmlK9ffhtQD7xfeIvcG89ec97H8jo2ziNFa36w==";
                inv.cSIDInfo.CertPem = zatcakeys.PublicKey;
                inv.cSIDInfo.PrivateKey = zatcakeys.PrivateKey;

                // InvoiceTotal CalculateInvoiceTotal = ubl.CalculateInvoiceTotal(inv.InvoiceLines, inv.allowanceCharges);
                bool savexml = true;
                var path = Path.Combine("Invoices");
                res = ubl.GenerateInvoiceXML(inv, path, savexml);
                //res = ubl.GenerateInvoiceXML(inv, Directory.GetCurrentDirectory(),true);
                var result = new GeneralMessage();


                if (res.IsValid)
                {
                    result = _invoicesRequestsService.SaveInvoicesRequest(InvoiceReqId ?? 0, invoiceIdV, InvoicesVM.Type, res.InvoiceHash, "", res.EncodedInvoice, res.UUID, res.QRCode, res.PIH, res.SingedXMLFileName, zatcaVoucherNumber ?? 0, false, null, null, null, null, null, Branchid);
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "GenerateInvoiceXML Valid";
                    _SystemAction.SaveAction("ZatcaInvoiceIntegration", "VoucherController", 3, "GenerateInvoiceXML Valid", "", "", ActionDate, 1, 1, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }
                else
                {
                    var errormsg = res.ErrorMessage;
                    result = _invoicesRequestsService.SaveInvoicesRequest(InvoiceReqId ?? 0, invoiceIdV, InvoicesVM.Type, res.InvoiceHash, "", res.EncodedInvoice, res.UUID, res.QRCode, res.PIH, res.SingedXMLFileName, zatcaVoucherNumber ?? 0, false,400, null, null, null, errormsg, Branchid);
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "GenerateInvoiceXML InValid";
                    _SystemAction.SaveAction("ZatcaInvoiceIntegration", "VoucherController", 1, errormsg, "", "", ActionDate, 1, 1, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = errormsg };

                }
                invReqCl invReqClobj = new invReqCl();
                invReqClobj.InvoiceReqId = result.ReturnedParm ?? 0;
                invReqClobj.InvoiceId = invoiceIdV;
                invReqClobj.Type = InvoicesVM.Type;

                var resultSend = SendToZatcaAPI(inv, res, objOrganization, invReqClobj, zatcakeys, Branchid);
                return resultSend;
            }
            catch (Exception ex)
            {

                var errormsg = ex.Message;
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "GenerateInvoiceXML InValid";
                _SystemAction.SaveAction("ZatcaInvoiceIntegration", "VoucherController", 1, errormsg, "", "", ActionDate, 1, 1, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = errormsg };
            }


        }

        [HttpGet("SendToZatcaAPI")]
        private GeneralMessage SendToZatcaAPI(Invoice inv, ZatcaIntegrationSDK.Result res, OrganizationsVM org, invReqCl invReqCl ,ZatcaKeys zatcakeys,int BranchId)
        {
            if(org.ModeType==2){mode = ZatcaIntegrationSDK.APIHelper.Mode.Simulation;}
            else if (org.ModeType == 3){mode = ZatcaIntegrationSDK.APIHelper.Mode.Production;}
            else{mode = ZatcaIntegrationSDK.APIHelper.Mode.developer;}

            string warningmessage = "";
            string errormessage = "";
            bool IsSent = false;
            string SendingStatus = "";
            string QRCode = res.QRCode;
            int statusCode = 0;
            string ClearedInvoice = "";
            var path = Path.Combine("Invoices");
            ApiRequestLogic apireqlogic = new ApiRequestLogic(mode,path,true);
            InvoiceReportingRequest invrequestbody = new InvoiceReportingRequest();
            invrequestbody.invoice = res.EncodedInvoice;
            invrequestbody.invoiceHash = res.InvoiceHash;
            invrequestbody.uuid = res.UUID;
            if (mode == ZatcaIntegrationSDK.APIHelper.Mode.developer)
            {
                ComplianceCsrResponse tokenresponse = new ComplianceCsrResponse();
                string csr = zatcakeys.CSR ?? "";
                tokenresponse = apireqlogic.GetComplianceCSIDAPI("123456", csr);
                if (String.IsNullOrEmpty(tokenresponse.ErrorMessage))
                {
                    InvoiceReportingResponse responsemodel = apireqlogic.CallComplianceInvoiceAPI(tokenresponse.BinarySecurityToken, tokenresponse.Secret, invrequestbody);
                    IsSent = responsemodel.IsSuccess;
                    statusCode = responsemodel.StatusCode;
                    SendingStatus = responsemodel.ReportingStatus + responsemodel.ClearanceStatus;
                    if (responsemodel.IsSuccess)
                    {
                        if (responsemodel.StatusCode == 202)
                        {
                            warningmessage = responsemodel.WarningMessage;
                        }
                        var result = _invoicesRequestsService.SaveInvoicesRequest(invReqCl.InvoiceReqId, invReqCl.InvoiceId??0, invReqCl.Type??0, res.InvoiceHash, "", res.EncodedInvoice, res.UUID, res.QRCode, res.PIH, res.SingedXMLFileName, 0, IsSent, statusCode, SendingStatus, warningmessage,null, null, BranchId);
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = responsemodel.ReportingStatus + responsemodel.ClearanceStatus };
                    }
                    else
                    {
                        errormessage = responsemodel.ErrorMessage;
                        var result = _invoicesRequestsService.SaveInvoicesRequest(invReqCl.InvoiceReqId, invReqCl.InvoiceId ?? 0, invReqCl.Type ?? 0, res.InvoiceHash, "", res.EncodedInvoice, res.UUID, res.QRCode, res.PIH, res.SingedXMLFileName, 0, IsSent, statusCode, SendingStatus, null, null, errormessage, BranchId);
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = responsemodel.ErrorMessage };
                    }
                }
                else
                {
                    var result = _invoicesRequestsService.SaveInvoicesRequest(invReqCl.InvoiceReqId, invReqCl.InvoiceId ?? 0, invReqCl.Type ?? 0, res.InvoiceHash, "", res.EncodedInvoice, res.UUID, res.QRCode, res.PIH, res.SingedXMLFileName, 0, IsSent, statusCode, SendingStatus, null, null, tokenresponse.ErrorMessage, BranchId);
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = tokenresponse.ErrorMessage };
                }
            }
            else
            {
                string secretkey = zatcakeys.SecreteKey;
                if (inv.invoiceTypeCode.Name.Substring(0, 2) == "01")
                {
                    InvoiceClearanceResponse responsemodel = apireqlogic.CallClearanceAPI(Utility.ToBase64Encode(inv.cSIDInfo.CertPem), secretkey, invrequestbody);
                    IsSent = responsemodel.IsSuccess;
                    statusCode = responsemodel.StatusCode;
                    SendingStatus = responsemodel.ClearanceStatus;
                    if (responsemodel.IsSuccess)
                    {
                        if (responsemodel.StatusCode == 202)
                        {
                            warningmessage = responsemodel.WarningMessage;
                        }
                        QRCode = responsemodel.QRCode;
                        ClearedInvoice = responsemodel.ClearedInvoice;
                        var result = _invoicesRequestsService.SaveInvoicesRequest(invReqCl.InvoiceReqId, invReqCl.InvoiceId ?? 0, invReqCl.Type ?? 0, res.InvoiceHash, "", res.EncodedInvoice, res.UUID, QRCode, res.PIH, res.SingedXMLFileName, 0, IsSent, statusCode, SendingStatus, warningmessage, ClearedInvoice, null, BranchId);
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = responsemodel.ClearanceStatus };
                    }
                    else
                    {
                        errormessage = responsemodel.ErrorMessage;
                        var result = _invoicesRequestsService.SaveInvoicesRequest(invReqCl.InvoiceReqId, invReqCl.InvoiceId ?? 0, invReqCl.Type ?? 0, res.InvoiceHash, "", res.EncodedInvoice, res.UUID, res.QRCode, res.PIH, res.SingedXMLFileName, 0, IsSent, statusCode, SendingStatus, null, null, errormessage, BranchId);
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = responsemodel.ErrorMessage };
                    }
                }
                else
                {
                    InvoiceReportingResponse responsemodel = apireqlogic.CallReportingAPI(Utility.ToBase64Encode(inv.cSIDInfo.CertPem), secretkey, invrequestbody);
                    IsSent = responsemodel.IsSuccess;
                    statusCode = responsemodel.StatusCode;
                    SendingStatus = responsemodel.ReportingStatus;
                    if (responsemodel.IsSuccess)
                    {
                        if (responsemodel.StatusCode == 202)
                        {
                            warningmessage = responsemodel.WarningMessage;
                        }
                        var result = _invoicesRequestsService.SaveInvoicesRequest(invReqCl.InvoiceReqId, invReqCl.InvoiceId ?? 0, invReqCl.Type ?? 0, res.InvoiceHash, "", res.EncodedInvoice, res.UUID, res.QRCode, res.PIH, res.SingedXMLFileName, 0, IsSent, statusCode, SendingStatus, warningmessage, null, null, BranchId);
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = responsemodel.ReportingStatus };
                    }
                    else
                    {
                        errormessage = responsemodel.ErrorMessage;
                        var result = _invoicesRequestsService.SaveInvoicesRequest(invReqCl.InvoiceReqId, invReqCl.InvoiceId ?? 0, invReqCl.Type ?? 0, res.InvoiceHash, "", res.EncodedInvoice, res.UUID, res.QRCode, res.PIH, res.SingedXMLFileName, 0, IsSent, statusCode, SendingStatus, null, null, errormessage, BranchId);
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = responsemodel.ErrorMessage };
                    }
                }
            }

        }

        [HttpPost("ReSendToZatcaAPI_Func")]
        public IActionResult ReSendToZatcaAPI_Func(int InvoiceReqId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var org = _organizationsservice.GetBranchOrganization().Result;
            var InvoiceReqData = _invoicesRequestsService.GetInvoiceReqByReqId(InvoiceReqId).Result;
            var BranchId = (InvoiceReqData.BranchId ?? _globalshared.BranchId_G);
            var objBranch = _BranchesService.GetBranchByBranchId("rtl", BranchId).Result.FirstOrDefault();
            ZatcaKeys zatcakeys = new ZatcaKeys();
            var OrgIsRequired = _systemSettingsService.GetSystemSettingsByBranchId(BranchId).Result.OrgDataIsRequired;
            if (OrgIsRequired == true) OrgIsRequired = false; else OrgIsRequired = true;
            if (OrgIsRequired == true)
            {
                zatcakeys.CSR = org.CSR;
                zatcakeys.PrivateKey = org.PrivateKey;
                zatcakeys.PublicKey = org.PublicKey;
                zatcakeys.SecreteKey = org.SecreteKey;
            }
            else
            {
                zatcakeys.CSR = objBranch.CSR;
                zatcakeys.PrivateKey = objBranch.PrivateKey;
                zatcakeys.PublicKey = objBranch.PublicKey;
                zatcakeys.SecreteKey = objBranch.SecreteKey;
                if (objBranch.CSR == null || objBranch.CSR == "")
                {
                    zatcakeys.CSR = org.CSR;
                }
                if (objBranch.PrivateKey == null || objBranch.PrivateKey == "")
                {
                    zatcakeys.PrivateKey = org.PrivateKey;
                }
                if (objBranch.PublicKey == null || objBranch.PublicKey == "")
                {
                    zatcakeys.PublicKey = org.PublicKey;
                }
                if (objBranch.SecreteKey == null || objBranch.SecreteKey == "")
                {
                    zatcakeys.SecreteKey = org.SecreteKey;
                }

            }
            var invoicetypecode = InvoiceTypeEnums.Standared_Invoice;
            var invoicetypecodeName = InvoiceTypeNameEnums.Standared_Invoice;
            if (InvoiceReqData.InvoiceValue > 1000)
            {
                if (InvoiceReqData.Type == 2)
                {
                    invoicetypecode = InvoiceTypeEnums.Standared_Invoice;
                    invoicetypecodeName = InvoiceTypeNameEnums.Standared_Invoice;
                }
                if (InvoiceReqData.Type == 29 || InvoiceReqData.Type == 4)
                {
                    invoicetypecode = InvoiceTypeEnums.Standard_CreditNote;
                    invoicetypecodeName = InvoiceTypeNameEnums.Standard_CreditNote;
                }
                if (InvoiceReqData.Type == 30)
                {
                    invoicetypecode = InvoiceTypeEnums.Standared_DebitNote;
                    invoicetypecodeName = InvoiceTypeNameEnums.Standared_DebitNote;
                }
            }
            else
            {
                if (InvoiceReqData.Type == 2)
                {
                    invoicetypecode = InvoiceTypeEnums.Simplified_Invoice;
                    invoicetypecodeName = InvoiceTypeNameEnums.Simplified_Invoice;
                }
                if (InvoiceReqData.Type == 29 || InvoiceReqData.Type == 4)
                {
                    invoicetypecode = InvoiceTypeEnums.Simplified_CreditNote;
                    invoicetypecodeName = InvoiceTypeNameEnums.Simplified_CreditNote;
                }
                if (InvoiceReqData.Type == 30)
                {
                    invoicetypecode = InvoiceTypeEnums.Simplified_DebitNote;
                    invoicetypecodeName = InvoiceTypeNameEnums.Simplified_DebitNote;
                }
            }

            Invoice inv = new Invoice();
            ZatcaIntegrationSDK.Result res = new ZatcaIntegrationSDK.Result();
            res.EncodedInvoice = InvoiceReqData.EncodedInvoice;
            res.InvoiceHash = InvoiceReqData.InvoiceHash;
            res.UUID = InvoiceReqData.ZatcaUUID;

            inv.invoiceTypeCode.id = invoicetypecode;
            inv.invoiceTypeCode.Name = invoicetypecodeName;
            inv.cSIDInfo.CertPem = zatcakeys.PublicKey;
            inv.cSIDInfo.PrivateKey = zatcakeys.PrivateKey;


            invReqCl invReqClobj = new invReqCl();
            invReqClobj.InvoiceReqId = InvoiceReqId;
            invReqClobj.InvoiceId = InvoiceReqData.InvoiceId;
            invReqClobj.Type = InvoiceReqData.Type;

            var resultSend = SendToZatcaAPI(inv, res, org, invReqClobj, zatcakeys, BranchId);
            return Ok(resultSend);
        }

        [HttpPost("ReSendZatcaInvoiceIntegrationFunc")]
        public IActionResult ReSendZatcaInvoiceIntegrationFunc(int InvoiceReqId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var org = _organizationsservice.GetBranchOrganization().Result;
            var InvoiceReqData = _invoicesRequestsService.GetInvoiceReqByReqId(InvoiceReqId).Result;
            InvoiceObjDet InvoiceObjDet = new InvoiceObjDet();

            if (InvoiceReqData.Type==2 || InvoiceReqData.Type == 29)
            {
                var voucherDet = _TaamerProContext.VoucherDetails.Where(s => s.IsDeleted == false && s.InvoiceId == InvoiceReqData.InvoiceId).ToList();
                List<int> voDetIds = new List<int>();
                foreach (var itemV in voucherDet)
                {
                    voDetIds.Add(itemV.VoucherDetailsId);
                }
                InvoiceObjDet.voucherDetObj = voDetIds;
            }
            else if (InvoiceReqData.Type == 4)
            {
                var VoucherDetailsV = _voucherService.GetAllDetailsByInvoiceId(InvoiceReqData.InvoiceId).Result;
                var ObjDet = new List<ObjRet>();
                var VoucherDetCredit = new List<VoucherDetails>();
                var VoucherCredit = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.CreditNotiId == InvoiceReqData.InvoiceId).ToList();
                if (VoucherCredit.Count() > 0)
                {
                    VoucherDetCredit = _TaamerProContext.VoucherDetails.Where(s => s.InvoiceId == VoucherCredit.FirstOrDefault().InvoiceId).ToList();
                }
                if (VoucherDetCredit.Count() > 0)
                {
                    foreach (var item in VoucherDetailsV)
                    {
                        var ObjDetInst = new ObjRet();
                        foreach (var itemC in VoucherDetCredit)
                        {
                            if (item.ServicesPriceId == itemC.ServicesPriceId)
                            {

                                item.TaxAmount = item.TaxAmount - itemC.TaxAmount;
                                item.Amount = item.Amount - itemC.Amount;
                                item.TotalAmount = item.TotalAmount - itemC.TotalAmount;

                            }
                        }
                        ObjDetInst.TaxAmount = item.TaxAmount;
                        ObjDetInst.Amount = item.Amount;
                        ObjDetInst.TotalAmount = item.TotalAmount;
                        ObjDetInst.Qty = item.Qty;
                        ObjDetInst.ServicesPriceName = item.ServicesPriceName;
                        ObjDetInst.DiscountValue_Det = item.DiscountValue_Det;
                        ObjDetInst.DiscountPercentage_Det = item.DiscountPercentage_Det;
                        ObjDetInst.InvoiceId = item.InvoiceId;
                        ObjDetInst.VoucherDetailsId = item.VoucherDetailsId;
                        ObjDetInst.Type = 4;
                        ObjDet.Add(ObjDetInst);
                    }
                }
                else
                {
                    foreach (var item in VoucherDetailsV)
                    {
                        var ObjDetInst = new ObjRet();
                        ObjDetInst.TaxAmount = item.TaxAmount;
                        ObjDetInst.Amount = item.Amount;
                        ObjDetInst.TotalAmount = item.TotalAmount;
                        ObjDetInst.Qty = item.Qty;
                        ObjDetInst.ServicesPriceName = item.ServicesPriceName;
                        ObjDetInst.DiscountValue_Det = item.DiscountValue_Det;
                        ObjDetInst.DiscountPercentage_Det = item.DiscountPercentage_Det;
                        ObjDetInst.InvoiceId = item.InvoiceId;
                        ObjDetInst.VoucherDetailsId = item.VoucherDetailsId;
                        ObjDetInst.Type = 4;
                        ObjDet.Add(ObjDetInst);
                    }
                }
                InvoiceObjDet.voucherDetObjRet = ObjDet;
            }
            var Result = ZatcaInvoiceIntegration(InvoiceObjDet, _globalshared.BranchId_G, InvoiceObjDet.voucherDetObjRet ?? new List<ObjRet>(), InvoiceReqId);
            return Ok(Result);
        }

        [HttpPost("ReSendZatcaInvoiceIntegrationFuncByInvoiceId")]
        public IActionResult ReSendZatcaInvoiceIntegrationFuncByInvoiceId(int InvoiceId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var org = _organizationsservice.GetBranchOrganization().Result;
            var InvoiceReqData = _voucherService.GetInvoiceDateById(InvoiceId).Result;
            InvoiceObjDet InvoiceObjDet = new InvoiceObjDet();

            if (InvoiceReqData.Type == 2 || InvoiceReqData.Type == 29)
            {
                var voucherDet = _TaamerProContext.VoucherDetails.Where(s => s.IsDeleted == false && s.InvoiceId == InvoiceReqData.InvoiceId).ToList();
                List<int> voDetIds = new List<int>();
                foreach (var itemV in voucherDet)
                {
                    voDetIds.Add(itemV.VoucherDetailsId);
                }
                InvoiceObjDet.voucherDetObj = voDetIds;
            }
            else if (InvoiceReqData.Type == 4)
            {
                var VoucherDetailsV = _voucherService.GetAllDetailsByInvoiceId(InvoiceReqData.InvoiceId).Result;
                var ObjDet = new List<ObjRet>();
                var VoucherDetCredit = new List<VoucherDetails>();
                var VoucherCredit = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.CreditNotiId == InvoiceReqData.InvoiceId).ToList();
                if (VoucherCredit.Count() > 0)
                {
                    VoucherDetCredit = _TaamerProContext.VoucherDetails.Where(s => s.InvoiceId == VoucherCredit.FirstOrDefault().InvoiceId).ToList();
                }
                if (VoucherDetCredit.Count() > 0)
                {
                    foreach (var item in VoucherDetailsV)
                    {
                        var ObjDetInst = new ObjRet();
                        foreach (var itemC in VoucherDetCredit)
                        {
                            if (item.ServicesPriceId == itemC.ServicesPriceId)
                            {

                                item.TaxAmount = item.TaxAmount - itemC.TaxAmount;
                                item.Amount = item.Amount - itemC.Amount;
                                item.TotalAmount = item.TotalAmount - itemC.TotalAmount;

                            }
                        }
                        ObjDetInst.TaxAmount = item.TaxAmount;
                        ObjDetInst.Amount = item.Amount;
                        ObjDetInst.TotalAmount = item.TotalAmount;
                        ObjDetInst.Qty = item.Qty;
                        ObjDetInst.ServicesPriceName = item.ServicesPriceName;
                        ObjDetInst.DiscountValue_Det = item.DiscountValue_Det;
                        ObjDetInst.DiscountPercentage_Det = item.DiscountPercentage_Det;
                        ObjDetInst.InvoiceId = item.InvoiceId;
                        ObjDetInst.VoucherDetailsId = item.VoucherDetailsId;
                        ObjDetInst.Type = 4;
                        ObjDet.Add(ObjDetInst);
                    }
                }
                else
                {
                    foreach (var item in VoucherDetailsV)
                    {
                        var ObjDetInst = new ObjRet();
                        ObjDetInst.TaxAmount = item.TaxAmount;
                        ObjDetInst.Amount = item.Amount;
                        ObjDetInst.TotalAmount = item.TotalAmount;
                        ObjDetInst.Qty = item.Qty;
                        ObjDetInst.ServicesPriceName = item.ServicesPriceName;
                        ObjDetInst.DiscountValue_Det = item.DiscountValue_Det;
                        ObjDetInst.DiscountPercentage_Det = item.DiscountPercentage_Det;
                        ObjDetInst.InvoiceId = item.InvoiceId;
                        ObjDetInst.VoucherDetailsId = item.VoucherDetailsId;
                        ObjDetInst.Type = 4;
                        ObjDet.Add(ObjDetInst);
                    }
                }
                InvoiceObjDet.voucherDetObjRet = ObjDet;
            }
            var Result = ZatcaInvoiceIntegration(InvoiceObjDet, _globalshared.BranchId_G, InvoiceObjDet.voucherDetObjRet ?? new List<ObjRet>(), null);
            return Ok(Result);
        }

        [HttpPost("PDFDownloadZatca")]
        public IActionResult PDFDownloadZatca(IFormFile? UploadedFile, [FromForm] int InvoiceId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            byte[] pdfByte = { 0 };
            if(UploadedFile!=null)
            {
                using (var ms = new MemoryStream())
                {
                    UploadedFile.CopyTo(ms);
                    pdfByte = ms.ToArray();
                    // Process the byte array
                }
            }

            //dawoudprint
            string existTemp = System.IO.Path.Combine("TempFiles/");
            if (!Directory.Exists(existTemp))
            {
                Directory.CreateDirectory(existTemp);
            }
            string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
            string FilePath = System.IO.Path.Combine("TempFiles/", FileName);
            System.IO.File.WriteAllBytes(FilePath, pdfByte);
            string FilePathReturn = "/TempFiles/" + FileName;
            return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = FilePathReturn });
        }

        [HttpPost("ConvertEncodedXMLToPDFA3ByteArray")]
        public IActionResult ConvertEncodedXMLToPDFA3ByteArray(IFormFile? UploadedFile, [FromForm] int InvoiceId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            byte[] pdfByte = { 0 };
            if (UploadedFile != null)
            {
                using (var ms = new MemoryStream())
                {
                    UploadedFile.CopyTo(ms);
                    pdfByte = ms.ToArray();
                }
            }
            var InvoiceReqData = _invoicesRequestsService.GetInvoiceReq(InvoiceId).Result;

            //
            PDFA3Result pdfresult = new PDFA3Result();
            XMLPDF xMLPDF = new XMLPDF();
            // pdfresult = xMLPDF.ConvertXMLToPDFA3ByteArray(txt_xmlpath.Text, txt_pdfpath.Text);
            pdfresult = xMLPDF.ConvertEncodedXMLToPDFA3ByteArray(InvoiceReqData.ClearedInvoice, pdfByte, "TempFiles", true);
            if (pdfresult.IsValid)
            {
                // MessageBox.Show("File Saved Successfuly at ");
                if (pdfresult.PDFA3ContentFile != null && pdfresult.PDFA3ContentFile.Length > 0)
                {

                    ////dawoudprint
                    //string existTemp = System.IO.Path.Combine("TempFiles/");
                    //if (!Directory.Exists(existTemp))
                    //{
                    //    Directory.CreateDirectory(existTemp);
                    //}
                    //string FileName = "PDFFile_" + DateTime.Now.Ticks.ToString() + ".pdf";
                    //string FilePath = System.IO.Path.Combine("TempFiles/", FileName);
                    //System.IO.File.WriteAllBytes(FilePath, pdfresult.PDFA3ContentFile);
                    //string FilePathReturn = "/TempFiles/" + FileName;
                    string FilePathReturn = pdfresult.PDFA3FileNameFullPath;
                    FilePathReturn = FilePathReturn.Replace("\\", "/");
                    FilePathReturn = "/" + FilePathReturn;

                    return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = FilePathReturn });
                }
                else
                {
                    return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "PDF-A3 Not Generated !" });
                }

            }
            else
            {
                return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = pdfresult.ErrorMessage });
            }

        }
        private void FillSellerOtherIdentification()
        {
            Dictionary<string, string> schemes = new Dictionary<string, string>()
                    {
                        {"رقم السجل التجارى","CRN" },
                        {"رخصة وزارة الشؤون البلدية والقروية والإسكان","MOM" },
                        {"رخصة وزارة الموارد البشرية والتنمية الاجتماعية","MLS" },
                        {"رخصة وزارة الاستثمار","SAG" },
                        {"معرف آخر","OTH" }
                        };
        }
        private void FillBuyerOtherIdentification()
        {
            Dictionary<string, string> schemes = new Dictionary<string, string>()
                    {
                        {"رقم السجل التجارى","CRN" },
                        {"رخصة وزارة الشؤون البلدية والقروية والإسكان","MOM" },
                        {"رخصة وزارة الموارد البشرية والتنمية الاجتماعية","MLS" },
                        {"رخصة وزارة الاستثمار","SAG" },
                        {"معرف آخر","OTH" },
                        {"الرقم المميز","TIN" },
                        {" مكتب العمل700 Number","700" },
                        {"رقم الهوية","NAT" },
                        {"مجلس التعاون الخليجى","GCC" },
                        {"رقم الاقامة","IQA" },
                         {"رقم الباسبور","PAS" },
                        };
        }

    }



    public class InvoiceObjDet
    {
        public List<int>? voucherDetObj { get; set; }
        public List<ObjRet>? voucherDetObjRet { get; set; }
    }

    public class InvoiceItems
    {
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal TotalPriceAfterDiscount { get; set; }
        public decimal VatPercentage { get; set; }
        public decimal VatValue { get; set; }
        public decimal TotalWithVat { get; set; }
    }

    public class PeriodCounter
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public class ZatcaKeys
    {
        public string CSR { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public string SecreteKey { get; set; }
    }

    public class invReqCl
    {
        public int InvoiceReqId { get; set; }
        public int? InvoiceId { get; set; }
        public int? Type { get; set; }
    }

}

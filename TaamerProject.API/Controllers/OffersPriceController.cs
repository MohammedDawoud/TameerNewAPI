using Microsoft.AspNetCore.Mvc;
using TaamerProject.Service.Interfaces;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Globalization;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Require2FA")]

    public class OffersPriceController : ControllerBase
    {
            private readonly IOffersPricesService _offersPricesService;
        private readonly IServicesPriceOfferService _servicesPriceOfferService;

        private readonly IOfferpriceconditionService _offerpriceconditionService;
            private readonly ICustomerService _customerService;
            private readonly IProjectService _projectService;
            private readonly ICustomerPaymentsService _customerPaymentsService;
            private IOrganizationsService _organizationsservice;
            private IBranchesService _BranchesService;
            private readonly IServicesPriceService _servicesPriceService;
            private readonly IOfferserviceService _offerserviceService;
            private ISystemSettingsService _systemSettingsService;
        private readonly ICustomerSMSService _sMSService;


        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private string? Con;
        private byte[] ReportPDF;

            public OffersPriceController(IOffersPricesService offersPricesService, IServicesPriceOfferService oervicesPriceOfferService, IOfferpriceconditionService offerpriceconditionService, ICustomerService customerService,
                IProjectService projectService, ICustomerPaymentsService customerPaymentsService, IOrganizationsService organizationsService, IBranchesService branchesService,
                IServicesPriceService servicesPriceService, IOfferserviceService offerserviceService, ISystemSettingsService systemSettingsService
                , IConfiguration _configuration, ICustomerSMSService sMSService, IWebHostEnvironment webHostEnvironment)
            {
                _offersPricesService = offersPricesService;
            _servicesPriceOfferService = oervicesPriceOfferService;

            this._offerpriceconditionService = offerpriceconditionService;
                this._customerService = customerService;
                this._projectService = projectService;
                this._customerPaymentsService = customerPaymentsService;

                this._organizationsservice = organizationsService;
                this._BranchesService = branchesService;
                _servicesPriceService = servicesPriceService;
                this._offerserviceService = offerserviceService;
                this._systemSettingsService = systemSettingsService;
            _sMSService = sMSService;

            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");
            HttpContext httpContext = HttpContext;

            _globalshared = new GlobalShared(httpContext);
            _hostingEnvironment = webHostEnvironment;

        }
        [HttpGet("GetAllOffers")]
        public ActionResult GetAllOffers()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var offers = _offersPricesService.GetAllOffers(_globalshared.BranchId_G).Result.ToList();
                return Ok(offers.ToList() );
            }
        [HttpGet("Getofferconestintroduction")]
        public ActionResult Getofferconestintroduction()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var offers = _offersPricesService.Getofferconestintroduction(_globalshared.BranchId_G);
                return Ok(offers );
            }
        [HttpGet("Fillcustomerhavingoffer")]
        public ActionResult Fillcustomerhavingoffer()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var offers = _offersPricesService.Fillcustomerhavingoffer(_globalshared.BranchId_G).ToList();
                return Ok(offers.ToList() );
            }
        [HttpGet("GetOfferconditionconst")]
        public ActionResult GetOfferconditionconst()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var offers = _offerpriceconditionService.GetOfferconditionconst(_globalshared.BranchId_G).Result.ToList();
                return Ok(offers.ToList() );
            }
        [HttpGet("GetAllCustomerPaymentsconst")]
        public ActionResult GetAllCustomerPaymentsconst()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var offers = _customerPaymentsService.GetAllCustomerPaymentsconst(_globalshared.BranchId_G).Result.ToList();
                return Ok(offers.ToList() );
            }
        [HttpGet("GetOfferConditionbyid")]
        public ActionResult GetOfferConditionbyid(int OfferId)
            {
                var offer = _offerpriceconditionService.GetOfferconditionByid(OfferId);
                return Ok(offer );
            }
        [HttpGet("GetOfferservicenByid")]
        public ActionResult GetOfferservicenByid(int OfferId)
            {
                var offerservice = _offerserviceService.GetOfferservicenByid(OfferId);

                return Ok(offerservice );
            }

        [HttpGet("GetOfferservicenByProjectId")]
        public ActionResult GetOfferservicenByProjectId(int ProjectId)
        {
            var lsOfferService = new List<OfferServiceVM>();
            var offers = _offersPricesService.GetAllOffersByProjectId(ProjectId).Result.ToList();
            foreach( var offer in offers )
            {
                var offerservice = _offerserviceService.GetOfferservicenByid(offer.OffersPricesId).Result.ToList();
                var serv = lsOfferService.Union(offerservice);
                lsOfferService = serv.ToList();
            }
            return Ok(lsOfferService);
        }
        [HttpGet("GetOfferByid")]
        public ActionResult GetOfferByid(int offerid)
        {
            var offers = _offersPricesService.GetOfferByid(offerid);
            return Ok(offers );
        }

        [HttpGet("GetOfferCode_S")]
        public IActionResult GetOfferCode_S()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_offersPricesService.GetOfferCode_S(_globalshared.BranchId_G));
        }

        [HttpGet("GetAllOffersByCustomerId")]
        public ActionResult GetAllOffersByCustomerId(int CustomerId)
            {
                var offers = _offersPricesService.GetOfferByCustomerId(CustomerId);
                return Ok(offers );
            }
        [HttpGet("GetAllOffersByProjectId")]
        public ActionResult GetAllOffersByProjectId(int ProjectId)
        {
            var offers = _offersPricesService.GetAllOffersByProjectId(ProjectId);
            return Ok(offers);
        }
        [HttpGet("GetAllOffersByCustomerId2")]
        public ActionResult GetAllOffersByCustomerId2(int CustomerId)
            {
                var offers = _offersPricesService.GetAllOfferByCustomerId(CustomerId);
                return Ok(offers );
            }
        [HttpGet("FillAllOfferTodropdown")]
        public ActionResult FillAllOfferTodropdown(int param)
            {
                return Ok(_offersPricesService.GetOfferByCustomerId(param).Select(s => new
                {
                    Id = s.OffersPricesId,
                    Name = s.OfferNo,

                }) );

            }
        [HttpGet("FillAllOfferTodropdownProject")]
        public ActionResult FillAllOfferTodropdownProject(int customerid, int? projectid)
        {
            return Ok(_offersPricesService.GetOfferByCustomerIdProject(customerid, projectid??0).Select(s => new
            {
                Id = s.OffersPricesId,
                Name = s.OfferNo,

            }));

        }
        [HttpGet("FillAllOfferTodropdownOld")]
        public ActionResult FillAllOfferTodropdownOld(int param)
            {
                return Ok(_offersPricesService.GetOfferByCustomerIdOld(param).Result.Select(s => new
                {
                    Id = s.OffersPricesId,
                    Name = s.OfferNo,

                }) );

            }
        [HttpGet("FillAllOfferTodropdown2")]
        public ActionResult FillAllOfferTodropdown2(int param, int param2)
            {
                return Ok(_offersPricesService.GetOfferByCustomerId2(param, param2).Select(s => new
                {
                    Id = s.OffersPricesId,
                    Name = s.OfferNo,

                }) );

            }
        [HttpGet("GetOfferPrice_Search")]
        public ActionResult GetOfferPrice_Search(string offerno, string Date, string customername, string presenter, decimal? Amount, int BranchId)
            {
                return Ok(_offersPricesService.GetOfferPrice_Search(offerno, Date, customername, presenter, Amount, BranchId) );
            }
        [HttpPost("saveoffer")]
        public ActionResult saveoffer(OffersPrices offers)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

                var result = _offersPricesService.SaveOffer(offers, _globalshared.UserId_G, _globalshared.BranchId_G, 0);

                return Ok(result);
            }

        [HttpPost("Intoduceoffer")]
        public ActionResult Intoduceoffer(int OffersPricesId, string Link)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/MailStampOfferPrice.html");
            var result = _offersPricesService.Intoduceoffer(OffersPricesId,_globalshared.UserId_G, _globalshared.BranchId_G, url, Link);
            return Ok(result );
        }
        [HttpPost("IntoduceofferManual")]
        public ActionResult IntoduceofferManual(int OffersPricesId, string Link)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/MailStampOfferPrice.html");
            var result = _offersPricesService.IntoduceofferManual(OffersPricesId, _globalshared.UserId_G, _globalshared.BranchId_G, url, Link);
            return Ok(result);
        }
        [HttpPost("DeleteOffer")]
        public ActionResult DeleteOffer(int OffersPricesId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _offersPricesService.DeleteOffer(OffersPricesId,_globalshared.UserId_G, _globalshared.BranchId_G);

            return Ok(result );
        }
        [HttpPost("Customeraccept")]
        public ActionResult Customeraccept(int OffersPricesId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _offersPricesService.Customeraccept(OffersPricesId,_globalshared.UserId_G, _globalshared.BranchId_G);

            return Ok(result);
        }
        [HttpGet("Getnextoffernum")]

        public ActionResult Getnextoffernum()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _offersPricesService.GenerateNextOfferNumber(_globalshared.BranchId_G);
            return Ok(result );
        }
        [HttpGet("GetProgressValues")]
        public ActionResult GetProgressValues(int offerid)
            {
                ProgressValues Obj = new ProgressValues();

                var offers = _offersPricesService.GetOfferByid(offerid).Result;
                if (offers.Count() > 0)
                {
                    if (offers?.FirstOrDefault()?.OfferStatus == 1)
                    { Obj.OfferStatus = true; }
                    else
                    { Obj.OfferStatus = false; }

                    if (offers?.FirstOrDefault()?.CustomerStatus == 1)
                    { Obj.CustStatus = true; }
                    else
                    { Obj.CustStatus = false; }
                }
                else
                {
                    Obj.OfferStatus = false;
                    Obj.CustStatus = false;
                }
                var Proj = _projectService.GetProjectByOfferId(offerid.ToString());
                if (Proj != null)
                {
                    Obj.ProjStatus = true;

                    if (Proj.ContractId > 0)
                    {
                        Obj.ContractStatus = true;
                        var PaymentPayed = _customerPaymentsService.GetAllCustomerPaymentsPaid(Proj.ContractId ?? 0);
                        if (PaymentPayed.Count() > 0)
                        {
                            Obj.PayInvStatus = true;

                        }
                        else
                        {
                            Obj.PayInvStatus = false;

                        }
                    }
                    else
                    {
                        Obj.ContractStatus = false;
                        Obj.PayInvStatus = false;
                    }

                }
                else
                {
                    Obj.ProjStatus = false;
                    Obj.ContractStatus = false;
                    Obj.PayInvStatus = false;
                }
                return Ok(Obj);

            }


        [HttpPost("CertifyOffer")]
        public ActionResult CertifyOffer(int OffersPricesId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var url = Path.Combine("Email/MailStampOfferPrice.html");

            var result = _offersPricesService.CertifyOffer(OffersPricesId, _globalshared.UserId_G, _globalshared.BranchId_G, url);

            return Ok(result);
        }

        [HttpPost("ConfirmCertifyOffer")]
        public ActionResult ConfirmCertifyOffer(int OffersPricesId,string Code)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _offersPricesService.ConfirmCertifyOffer(OffersPricesId, _globalshared.UserId_G, _globalshared.BranchId_G, Code);

            return Ok(result);
        }
      
        [HttpGet("ChangeOffer_PDF")]

        public IActionResult ChangeOffer_PDF(int OfferId)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            OfferReportVM _offerReportVM = new OfferReportVM();
            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
            var offers = _offersPricesService.GetOfferByid2(OfferId).Result.FirstOrDefault();
            var offercondition = _offerpriceconditionService.GetOfferconditionByid(OfferId).Result.ToList();


            CustomerVM customer = new CustomerVM();
            if (offers != null && offers.CustomerId != 0)
            {
                customer = _customerService.GetCustomersByCustomerId(offers.CustomerId, _globalshared.Lang_G).Result;
            }
            else
            {
                customer.CustomerNameAr = offers!.CustomerName??"";
                customer.CustomerEmail = offers!.CustomerEmail??"";
                customer.CustomerMobile = offers!.Customerphone??"";
            }

            var offerservice = _offerserviceService.GetOfferservicenByid((int)offers.OffersPricesId).Result.ToList();
            foreach (var item in offerservice)
            {
                var servicesPriOffer = _servicesPriceOfferService.GetServicesPriceByParentId(item.ServiceId, item.OfferId).Result.ToList();
                servicesPriOffer= servicesPriOffer.Where(s=>s.SureService==1).ToList();
                item.ServicesPricesOffer = servicesPriOffer; 
            }
            var payment = _customerPaymentsService.GetAllCustomerPaymentsboffer(OfferId).Result.ToList();

            var vat = _organizationsservice.GetBranchOrganizationData(orgId).Result.VAT;

            _offerReportVM.TaxAmount= vat.ToString();
            _offerReportVM.Customer = customer;
            _offerReportVM.Offerservce = offerservice;
            _offerReportVM.payment = payment;
            _offerReportVM.offercondition = offercondition;
            //_offerReportVM.ServicesPricesOffer = servicesPriOffer;
            _offerReportVM.offers = offers;

            _offerReportVM.DateTimeNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

            CurrencyInfo _currencyInfo = new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia);
            ToWord toWord = new ToWord(Convert.ToDecimal(offers.OfferValue.ToString()), _currencyInfo);
            var txt = toWord.ConvertToArabic();
            _offerReportVM.offersvaltxt = txt;


            var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
            _offerReportVM.Org_VD = objOrganization2;


            var objBranch = _BranchesService.GetBranchByBranchId(_globalshared.Lang_G, _globalshared.BranchId_G).Result.FirstOrDefault();
            var OrgIsRequired = _systemSettingsService.GetSystemSettingsByBranchId(_globalshared.BranchId_G).Result.OrgDataIsRequired;
            if (OrgIsRequired == true) OrgIsRequired = false; else OrgIsRequired = true;
            _offerReportVM.Branch_VD = objBranch;
            _offerReportVM.OrgIsRequired_VD = OrgIsRequired;

            return Ok(_offerReportVM);
        }

        [HttpPost("SendWOfferPrice")]
        public IActionResult SendWOfferPrice(IFormFile? UploadedFile, [FromForm] int OfferId, [FromForm] string? Notes, [FromForm] string? environmentURL, [FromForm] string? customerPhoneOffer)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

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
                    FileUrl = "/TempFiles/" + fileName;
                }
            }
            var FileAtt = FileUrl;


            //FilePathReturn
            var Message = Notes;
            //var result2 = _voucherService.SendWInvoice(InvoiceId, _globalshared.UserId_G, _globalshared.BranchId_G, FileAtt, environmentURL ?? "", fileTypeUpload ?? "");
            var result = _sMSService.SendWhatsApp_Notification(customerPhoneOffer ?? "", Message ?? "", _globalshared.UserId_G, _globalshared.BranchId_G, environmentURL ?? "", FileAtt);
            return Ok(result);
        }
       
    }
    public class ProgressValues
    {
        public bool OfferStatus { set; get; }
        public bool CustStatus { set; get; }
        public bool ProjStatus { set; get; }
        public bool ContractStatus { set; get; }
        public bool PayInvStatus { set; get; }
    }
}

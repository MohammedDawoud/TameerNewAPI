using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.Service.Interfaces;
using TaamerProject.API.Helper;
//using System.Drawing;
using iTextSharp.text;
using TaamerProject.Models;
using System.Net;
using iTextSharp.text.pdf;
using TaamerProject.Models.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Globalization;
using TaamerProject.Service.Services;
using TaamerProject.API.pdfHandler;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

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


        //public ActionResult PrintOfferPrices(int offerId, string isinclude)
        //{
        //    int orgId = _BranchesService.GetOrganizationId(BranchId);

        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);
        //    string[] infoDoneTasksReport = { Lang == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
        //    var offers = _offersPricesService.GetOfferByid(offerId).FirstOrDefault();
        //    var offercondition = _offerpriceconditionService.GetOfferconditionByid(offerId).ToList();
        //    CustomerVM customer = new CustomerVM();
        //    if (offers.CustomerId != 0)
        //    {
        //        customer = _customerService.GetCustomersByCustomerId(offers.CustomerId, Lang);
        //    }
        //    else
        //    {
        //        customer.CustomerNameAr = offers.CustomerName;

        //    }

        //    var offerservice = _offerserviceService.GetOfferservicenByid((int)offers.OffersPricesId).ToList();

        //    var payment = _customerPaymentsService.GetAllCustomerPaymentsboffer(offerId).ToList();

        //    var vat = _organizationsservice.GetBranchOrganizationData(orgId).VAT;
        //    ReportPDF = Bayanatech.TameerUI.pdfHandler.ReportsOf7sabat.Offerprices(offers, offercondition, customer, offerservice, payment, vat.ToString(), isinclude, infoDoneTasksReport);

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







        //        public GeneralMessage UploadFile_Draft(int ContractId)/*IFormFile file*//*ContractId*/
        //{
        //    Spire.Doc.Document doc = new Spire.Doc.Document();

        //    List<string> fileUrls = new List<string>();
        //    //foreach (var item in contracts)
        //    //{
        //    var pathh = "";
        //    string path = Server.MapPath("~/Reports/Contract/");
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    string fileName = "OfferPrice.docx";
        //    pathh = path + fileName;

        //    // var pathh = "";
        //    int? custid = 0;
        //    var massage = "";
        //    string fileLocation = "";
        //    string fileDir = "";
        //    string fileNamed;
        //    string filDown;
        //    String URII;
        //    try
        //    {
        //        var CustContract = _contractservice.GetAllContracts().Where(w => w.ContractId == ContractId).FirstOrDefault();
        //        custid = CustContract.CustomerId;
        //        var customerData = _customerservice.GetCustomersByCustomerId(custid, Lang);
        //        var branchObj = _branchservice.GetBranchById(CustContract.BranchId);
        //        int branchdata = branchObj.OrganizationId;
        //        var OrgData = _organizationservice.GetBranchOrganizationData(branchdata);
        //        string BranchId = Convert.ToString(branchObj.BranchId);
        //        string CustomerId = Convert.ToString(CustContract.CustomerId);
        //        string ProjectId = Convert.ToString(CustContract.ProjectId);
        //        string contanme = "offer_" + CustContract.ProjectNo;

        //        var Emp = _employeeService.GetEmployeeById(CustContract.OrgEmpId.Value, Lang);
        //        //var Service = _servicesPriceService.GetAllServicesPrice().Where(x => x.ServicesId == CustContract.ServiceId.Value).FirstOrDefault();
        //        //string serviceDetails = "";
        //        //var ServiceDetObjs = _servicesPriceService.GetServicesPriceByParentId(Service.ServicesId);
        //        //foreach (var item in ServiceDetObjs)
        //        //{
        //        //    serviceDetails = serviceDetails + (item.ServicesName + " - " + item.AccountName) + "\r\n";
        //        //}

        //        fileLocation = Server.MapPath("~/Uploads/Drafts/")
        //                   + contanme + ".docx";

        //        fileDir = Server.MapPath("~/Uploads/Drafts/");
        //        filDown = "Uploads/Drafts/";

        //        if (System.IO.Directory.Exists(fileDir))
        //        {
        //            if (System.IO.File.Exists(fileLocation))
        //            {
        //                System.IO.File.Delete(fileLocation);
        //            }
        //        }
        //        else
        //        {
        //            System.IO.Directory.CreateDirectory(fileDir);
        //            if (System.IO.File.Exists(fileLocation))
        //            {
        //                System.IO.File.Delete(fileLocation);
        //            }
        //        }


        //        fileNamed = contanme + ".docx";
        //        string sourcePath = path;
        //        string targetPath = fileDir;

        //        // Use Path class to manipulate file and directory paths.
        //        string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
        //        string destFile = System.IO.Path.Combine(targetPath, fileNamed);

        //        if (System.IO.File.Exists(destFile))
        //        {
        //            System.IO.File.Delete(destFile);
        //        }
        //        System.IO.File.Copy(sourceFile, destFile, true);

        //        Spire.Doc.Document doc2 = new Spire.Doc.Document();
        //        WebClient webClient = new WebClient();
        //        using (MemoryStream ms = new MemoryStream(webClient.DownloadData(destFile)))
        //        {
        //            doc.LoadFromStream(ms, FileFormat.Docx);

        //            doc.Replace("#LOGO", "jkhkj", false, true);



        //            //doc.Replace("#ContractDetails", ContractDetailsStr, false, true);

        //           // doc.Replace("#ProjectNo", CustContract.ProjectNo, false, true);

        //            /////////////////////


        //            Section section3 = doc.Sections[0];

        //            TextSelection selection1 = doc.FindString("#5dmat", true, true);

        //            TextRange range1 = selection1.GetAsOneRange();

        //            Spire.Doc.Documents.Paragraph paragraph1 = range1.OwnerParagraph;

        //            Spire.Doc.Body body1 = paragraph1.OwnerTextBody;

        //            int index1 = body1.ChildObjects.IndexOf(paragraph1);

        //            var conservices = _offerserviceService.GetOfferservicenByid(ContractId);// _contractService.GetContractservicenByid(ContractId);
        //            if (conservices.Count() > 0)
        //            {

        //                Spire.Doc.Table table2 = section3.AddTable(true);
        //                var countS = conservices.Count();
        //                table2.ResetCells(1, 6);
        //                // table.AddRow(false, 2);
        //                //table2.Rows[0].Cells[0].Width = table2.Rows[0].Cells[1].Width = 60F;

        //                table2.TableFormat.Bidi = true;

        //                int i = 1;
        //                foreach (var Serv in conservices)
        //                {

        //                    //string serviceDetails1 = "";
        //                    //var ServiceDetObjs1 = _servicesPriceService.GetServicesPriceByServiceId((int)S.ServiceId);



        //                    table2.AddRow(true, 6);
        //                    table2[i, 0].AddParagraph().AppendText(i.ToString());
        //                    table2[i, 1].AddParagraph().AppendText(Serv.ServicesName.ToString());
        //                    table2[i, 2].AddParagraph().AppendText(Serv.ServiceQty.ToString());
        //                    table2[i, 3].AddParagraph().AppendText(Serv.ServiceAmount.ToString());
        //                    table2[i, 4].AddParagraph().AppendText(Serv.TaxType.ToString());
        //                    table2[i, 5].AddParagraph().AppendText(Serv.Serviceamountval.ToString());
        //                    i++;
        //                }

        //                body1.ChildObjects.Remove(paragraph1);

        //                body1.ChildObjects.Insert(index1, table2);
        //            }



        //            Section section2 = doc.Sections[0];

        //            TextSelection selection = doc.FindString("#Dof3at", true, true);

        //            TextRange range = selection.GetAsOneRange();

        //            Spire.Doc.Documents.Paragraph paragraph = range.OwnerParagraph;

        //            Spire.Doc.Body body = paragraph.OwnerTextBody;

        //            int index = body.ChildObjects.IndexOf(paragraph);



        //            //var payment = _customerPaymentservice.GetAllCustomerPayments(Convert.ToInt32(ContractId));

        //            //if (payment.Count() > 0)
        //            //{
        //            //    Spire.Doc.Table table2 = section2.AddTable(true);
        //            //    var countp = payment.Count();
        //            //    table2.ResetCells(1, 2);
        //            //    // table.AddRow(false, 2);
        //            //    //table2.Rows[0].Cells[0].Width = table2.Rows[0].Cells[1].Width = 60F;

        //            //    table2.TableFormat.Bidi = true;
        //            //    table2[0, 0].AddParagraph().AppendText("المبلغ");
        //            //    string text = "تاريخ الدفعة";
        //            //    table2[0, 1].AddParagraph().AppendText(text);

        //            //    int i = 1;
        //            //    foreach (var p in payment)
        //            //    {

        //            //        table2.AddRow(true, 2);
        //            //        table2[i, 0].AddParagraph().AppendText(p.TotalAmount.ToString());
        //            //        table2[i, 1].AddParagraph().AppendText(p.PaymentDate.ToString());
        //            //        i++;
        //            //    }

        //            //    body.ChildObjects.Remove(paragraph);

        //            //    body.ChildObjects.Insert(index, table2);

        //            //}
        //            //else
        //            //{
        //            //    body.ChildObjects.Remove(paragraph);

        //            //    selection = doc.FindString("جدول الدفعات:", true, true);

        //            //    range = selection.GetAsOneRange();

        //            //    paragraph = range.OwnerParagraph;

        //            //    body.ChildObjects.Remove(paragraph);
        //            //}
        //            /////////////////////////


        //            ////payments
        //            ////Save Word
        //            ////Section section2 = doc.AddSection();

        //            ////var payment = _customerPaymentservice.GetAllCustomerPayments(Convert.ToInt32(ContractId));
        //            ////if (payment.Count() > 0)
        //            ////{
        //            ////    Spire.Doc.Table table2 = section2.AddTable(true);
        //            ////    var countp = payment.Count();
        //            ////    table2.ResetCells(1, 2);
        //            ////    // table.AddRow(false, 2);
        //            ////    //table2.Rows[0].Cells[0].Width = table2.Rows[0].Cells[1].Width = 60F;

        //            ////    table2.TableFormat.Bidi = true;
        //            ////    table2[0, 0].AddParagraph().AppendText("المبلغ");
        //            ////    string text = "تاريخ الدفعة";
        //            ////    table2[0, 1].AddParagraph().AppendText(text);

        //            ////    int i = 1;
        //            ////    foreach (var p in payment)
        //            ////    {

        //            ////        table2.AddRow(true, 2);
        //            ////        table2[i, 0].AddParagraph().AppendText(p.TotalAmount.ToString());
        //            ////        table2[i, 1].AddParagraph().AppendText(p.PaymentDate.ToString());
        //            ////        i++;
        //            ////    }
        //            ////}

        //            //Spire.Doc.Table table = section2.AddTable(true);

        //            //table.ResetCells(3, 2);

        //            //table[0, 0].AddParagraph().AppendText(string.Format("الطرف الأول"));
        //            //table[0, 0].Paragraphs[0].Format.IsBidi = true;
        //            //table.Rows[0].Cells[0].CellFormat.Borders.Color = System.Drawing.Color.Transparent;

        //            //table[0, 1].AddParagraph().AppendText(string.Format("الطرف الثاني"));
        //            //table[0, 1].Paragraphs[0].Format.IsBidi = true;
        //            //table.Rows[0].Cells[1].CellFormat.Borders.Color = System.Drawing.Color.Transparent;

        //            ////table.AddRow(true, 2);
        //            //table[1, 0].AddParagraph().AppendText(string.Format("الاسم: {0}", OrgData.NameAr));
        //            //table[1, 0].Paragraphs[0].Format.IsBidi = true;
        //            //table.Rows[1].Cells[0].CellFormat.Borders.Color = System.Drawing.Color.Transparent;

        //            //table[1, 1].AddParagraph().AppendText(string.Format("الاسم: {0}", customerData.CustomerName));
        //            //table[1, 1].Paragraphs[0].Format.IsBidi = true;
        //            //table.Rows[1].Cells[1].CellFormat.Borders.Color = System.Drawing.Color.Transparent;

        //            ////table.AddRow(true, 3);
        //            //table[2, 0].AddParagraph().AppendText(string.Format("التوقيع:.................................."));
        //            //table[2, 0].Paragraphs[0].Format.IsBidi = true;
        //            //table.Rows[2].Cells[0].CellFormat.Borders.Color = System.Drawing.Color.Transparent;

        //            //table[2, 1].AddParagraph().AppendText(string.Format("التوقيع:.................................."));
        //            //table[2, 1].Paragraphs[0].Format.IsBidi = true;
        //            //table.Rows[2].Cells[1].CellFormat.Borders.Color = System.Drawing.Color.Transparent;

        //            //table.TableFormat.Borders.Color = System.Drawing.Color.Transparent;
        //            //table.TableFormat.Bidi = true;
        //            //table.TableFormat.HorizontalAlignment = Spire.Doc.Documents.RowAlignment.Center;

        //        }


        //        doc.SaveToFile(destFile, FileFormat.Docx);
        //        fileUrls.Add(destFile);
        //        TempData["Linkurl"] = destFile;


        //        RemoveRedLine(destFile);
        //        var UrlS = Request.Url.AbsoluteUri;

        //        StringBuilder sb = new StringBuilder(UrlS);

        //        sb.Replace("Contract", filDown);
        //        sb.Replace("UploadFile", fileNamed);
        //        URII = sb.ToString();
        //        TempData["Linkurl"] = URII;


        //        URII = URII.Replace("//SaveUploads/Drafts", "") + fileNamed;

        //        //Save as a draft
        //        int projectType = _projectService.GetTypeOfProjct(CustContract.ProjectId.Value);
        //        Draft draft = new Draft() { DraftUrl = URII, Name = fileNamed, ProjectTypeId = projectType };
        //        _draftService.SaveDraft(draft,_globalshared.UserId_G, this.BranchId);
        //        var result = _draftDetailsService.SaveDraftDetails(new DraftDetails()
        //        {
        //            DraftId = draft.DraftId,
        //            ProjectId = CustContract.ProjectId.Value
        //        },_globalshared.UserId_G, this.BranchId);
        //        return result;
        //    }

        //    catch (Exception ex)
        //    {

        //        if (Lang == "rtl")
        //        {
        //            massage = "فشل في تحميل العقد";
        //        }
        //        else
        //        {
        //            massage = "Failed To Download Contract Files";
        //        }
        //        return new GeneralMessage { Result = false, Message = massage };
        //    }
        //}

        //public bool RemoveRedLine(string filePath)
        //{
        //    try
        //    {
        //        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, true))
        //        {
        //            string docText = null;
        //            using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
        //            {
        //                docText = sr.ReadToEnd();
        //            }

        //            Regex regexText = new Regex("Evaluation Warning: The document was created with Spire.Doc for .NET.");
        //            docText = regexText.Replace(docText, "");

        //            using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
        //            {
        //                sw.Write(docText);
        //            }
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

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

        //public ActionResult ChangeOffer_PDFEN(int OfferId)
        //{
        //    int orgId = _BranchesService.GetOrganizationId(BranchId);

        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);
        //    string[] infoDoneTasksReport = { Lang == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
        //    var offers = _offersPricesService.GetOfferByid(OfferId).FirstOrDefault();
        //    var offercondition = _offerpriceconditionService.GetOfferconditionByid(OfferId).ToList();
        //    CustomerVM customer = new CustomerVM();
        //    if (offers != null && offers.CustomerId != 0)
        //    {
        //        customer = _customerService.GetCustomersByCustomerId(offers.CustomerId, Lang);
        //    }
        //    else
        //    {
        //        customer.CustomerNameAr = offers.CustomerName;
        //        customer.CustomerEmail = offers.CustomerEmail;
        //        customer.CustomerMobile = offers.Customerphone;
        //        customer.CustomerNameEn = offers.CUstomerName_EN == "" ? offers.CustomerName : offers.CUstomerName_EN;
        //    }

        //    var offerservice = _offerserviceService.GetOfferservicenByid((int)offers.OffersPricesId).ToList();

        //    var payment = _customerPaymentsService.GetAllCustomerPaymentsboffer(OfferId).ToList();

        //    var vat = _organizationsservice.GetBranchOrganizationData(orgId).VAT;
        //    ViewData["TaxAmount"] = vat.ToString();
        //    ViewData["Customer"] = customer;

        //    ViewData["Offerservce"] = offerservice;

        //    ViewData["payment"] = payment;

        //    ViewData["offercondition"] = offercondition;



        //    ViewData["offers"] = offers;
        //    string Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
        //    ViewData["Date"] = Date;

        //    CurrencyInfo _currencyInfo = new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia);
        //    ToWord toWord = new ToWord(Convert.ToDecimal(offers.OfferValue.ToString()), _currencyInfo);
        //    var txt = toWord.ConvertToEnglish();
        //    ViewData["offersvaltxt"] = txt;

        //    var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId);
        //    if (objOrganization2 != null)
        //        ViewData["Org_VD"] = objOrganization2;
        //    else
        //        ViewData["Org_VD"] = null;

        //    var objBranch = _BranchesService.GetBranchByBranchId(Lang, BranchId).FirstOrDefault();
        //    var OrgIsRequired = _systemSettingsService.GetSystemSettingsByBranchId(BranchId).OrgDataIsRequired;
        //    ViewData["Branch_VD"] = objBranch;

        //    ViewData["OrgIsRequired_VD"] = OrgIsRequired;

        //    return PartialView("_OfferPricePDF_EN");

        //}
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

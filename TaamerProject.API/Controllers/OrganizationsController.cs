using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using Spire.Pdf.Barcode;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Net;
using System.Runtime.ConstrainedExecution;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;
using ZatcaIntegrationSDK;
using ZatcaIntegrationSDK.APIHelper;
using ZatcaIntegrationSDK.HelperContracts;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class OrganizationsController : ControllerBase
    {
        private IOrganizationsService _organizationsservice;
        private IBranchesService _BranchesService;
        private IEmailSettingService _EmailSettingservice;
        private ISMSSettingsService _SMSSettingsService;
        private IWhatsAppSettingsService _WhatsAppSettingsService;
        private IAttDeviceService _attDeviceService;
        private IUsersService _userService;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private string Con;
        private Mode mode = Mode.developer;
        private ISystemSettingsService _systemSettingsService;

        public OrganizationsController(IOrganizationsService organizationsService, IBranchesService branchesService, IEmailSettingService emailSettingService, ISMSSettingsService sMSSettingsService,
                IWhatsAppSettingsService whatsAppSettingsService, ISystemSettingsService systemSettingsService, IAttDeviceService attDeviceService, IUsersService usersService, IConfiguration _configuration, IWebHostEnvironment webHostEnvironment)
        {
            this._organizationsservice = organizationsService;
            this._BranchesService = branchesService;
            this._EmailSettingservice = emailSettingService;
            this._SMSSettingsService = sMSSettingsService;
            this._WhatsAppSettingsService = whatsAppSettingsService;
            this._systemSettingsService = systemSettingsService;

            this._attDeviceService = attDeviceService;
            this._userService = usersService;
            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");
            HttpContext httpContext = HttpContext;

            _globalshared = new GlobalShared(httpContext);
            _hostingEnvironment = webHostEnvironment;
        }
        [HttpGet("GetBranchOrganization")]
        public ActionResult GetBranchOrganization()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_organizationsservice.GetBranchOrganization());
        }
        [HttpGet("GetBranchOrganizationZatca")]
        public ActionResult GetBranchOrganizationZatca()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var OrgData = _organizationsservice.GetBranchOrganization().Result;
            var branchData = _BranchesService.GetBranchByBranchId("rtl",_globalshared.BranchId_G).Result!.FirstOrDefault()??new BranchesVM();
            if(branchData.PrivateKey == "" || branchData.PrivateKey==null)
            {
                return Ok(OrgData);
            }
            else
            {
                branchData.ModeType = OrgData.ModeType;
                return Ok(branchData);
            }
        }
        [HttpGet("GetComDomainLink_Org")]
        public ActionResult GetComDomainLink_Org()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            return Ok(_organizationsservice.GetComDomainLink_Org(orgId));
        }
        [HttpGet("GetApplicationVersion_Org")]
        public ActionResult GetApplicationVersion_Org()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            return Ok(_organizationsservice.GetApplicationVersion_Org(orgId));
        }
        [HttpGet("GetEmailOrganization")]
        public ActionResult GetEmailOrganization()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.GetBranchOrganization().Result;
            return Ok(result);
        }
        [HttpPost("SavepartialOrganizations")]
        public ActionResult SavepartialOrganizations(Organizations organizations)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.SavepartialOrganizations(organizations, _globalshared.UserId_G, _globalshared.BranchId_G, organizations.VAT??0, organizations.VATSetting??0);
            if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
            {
                result.ReasonPhrase = "Saved Successfully";
            }
            else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
            {
                result.ReasonPhrase = "Saved Falied";
            }
            return Ok(result);
        }
        //Edit by Mohamed Nasser
        [HttpPost("SaveOrganizations")]
        public ActionResult SaveOrganizations(IFormFile? UploadedFile, [FromForm] Organizations organizations)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            //HttpPostedFileBase file = Request.Files["UploadedImage"];
            //    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            //    {
            //        if (Request.Files["UploadedImage"].ContentLength > 0)
            //        {

            //            string fileName = System.IO.Path.GetFileName(GenerateRandomNo() + Request.Files["UploadedImage"].FileName);

            //            string fileLocation = Server.MapPath("~/Uploads/Organizations/pictures/") + fileName;
            //            try
            //            {
            //                if (System.IO.File.Exists(fileLocation))
            //                {
            //                    System.IO.File.Delete(fileLocation);
            //                }
            //                string width = Request.Form["imgwidth"];
            //                string hight = Request.Form["imghight"];

            //                double intwidth = Convert.ToDouble(width);// int.Parse(width);// Convert.ToInt32(width);
            //                double inthight = Convert.ToDouble(hight);// Convert.ToInt32(hight);

            //                //edit by M.Salah
            //                var scaleImage = ScaleImage(Bitmap.FromStream(Request.Files["UploadedImage"].InputStream), (int)inthight, (int)intwidth);

            //                scaleImage.Save(fileLocation);


            //                //WebImage img = new WebImage(Request.Files["UploadedImage"].InputStream);

            //                //    img.Resize(100, 100);

            //                //img.Save(fileLocation);
            //                //Request.Files["UploadedImage"].SaveAs(fileLocation);
            //                organizations.LogoUrl = "/Uploads/Organizations/pictures/" + fileName;
            //            }
            //            catch (Exception ex)
            //            {
            //                var massage = "";
            //                if (_globalshared.Lang_G == "rtl")
            //                {
            //                    massage = "فشل في رفع الصورة";
            //                }
            //                else
            //                {
            //                    massage = "Failed To Upload Image";
            //                }
            //                return Ok(new GeneralMessage { Result = false, Message = massage } );
            //            }
            //        }
            //    }
            if (UploadedFile != null)
            {
                System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


                string path = System.IO.Path.Combine("Uploads/", "Organizations/pictures/");
                string pathW = System.IO.Path.Combine("/Uploads/", "Organizations/pictures/");

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


                if (pathes != null)
                {
                    organizations.LogoUrl = pathes;
                }
            }
            var result = _organizationsservice.SaveOrganizations(organizations, _globalshared.UserId_G, _globalshared.BranchId_G);
            if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
            {
                result.ReasonPhrase = "Saved Successfully";
            }
            else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
            {
                result.ReasonPhrase = "Saved Falied";
            }
            return Ok(result);
        }
        [HttpPost("SaveOrganizationSettings")]
        public ActionResult SaveOrganizationSettings(Organizations organizations)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.SaveOrganizationSettings(organizations, _globalshared.UserId_G, _globalshared.BranchId_G);

            return Ok(result);
        }

        [HttpPost("DeleteOrganizations")]
        public ActionResult DeleteOrganizations(int OrganizationId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.DeleteOrganizations(OrganizationId, _globalshared.UserId_G, _globalshared.BranchId_G);
            if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
            {
                result.ReasonPhrase = "Deleted Successfully";
            }
            else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
            {
                result.ReasonPhrase = "Deleted Falied";
            }
            return Ok(result);
        }
        [HttpPost("GenerateCSID")]
        public IActionResult GenerateCSID(int OrganizationId, string OTP) 
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var objBranch = _BranchesService.GetBranchByBranchId("rtl", _globalshared.BranchId_G).Result.FirstOrDefault();
            var objOrganization = _organizationsservice.GetBranchOrganization().Result;

            //string Address1 = "";
            //string BuildingNumber1 = "";
            //string StreetName1 = "";
            //string Neighborhood1 = "";
            //string CityName1 = "";
            //string Country1 = "";
            //string PostalCode1 = "";
            //string PostalCodeFinal1 = "";
            //string ExternalPhone1 = "";
            //string TaxCode1 = "";

            //Address1 = objBranch.Address ?? objOrganization.Address;
            //BuildingNumber1 = objBranch.BuildingNumber ?? objOrganization.BuildingNumber;
            //StreetName1 = objBranch.StreetName ?? objOrganization.StreetName;
            //Neighborhood1 = objBranch.Neighborhood ?? objOrganization.Neighborhood;
            //CityName1 = objBranch.CityName ?? objOrganization.CityName;
            //Country1 = objBranch.Country ?? objOrganization.Country;
            //PostalCode1 = objBranch.PostalCode ?? objOrganization.PostalCode;
            //PostalCodeFinal1 = objBranch.PostalCodeFinal ?? objOrganization.PostalCodeFinal;
            //ExternalPhone1 = objBranch.ExternalPhone ?? objOrganization.ExternalPhone;
            //TaxCode1 = objBranch.TaxCode ?? objOrganization.TaxCode;

            Invoice inv = new Invoice();
            inv.ID = "INV00001"; // مثال SME00010

            //inv.IssueDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            inv.IssueDate = DateTime.Now.ToString("yyyy-MM-dd");
            inv.IssueTime = DateTime.Now.ToString("HH:mm:ss"); // "09:32:40"

            inv.DocumentCurrencyCode = "SAR";
            inv.TaxCurrencyCode = "SAR";
            //if (inv.invoiceTypeCode.id == 383 || inv.invoiceTypeCode.id == 381)
            //{
            //    // فى حالة ان اشعار دائن او مدين فقط هانكتب رقم الفاتورة اللى اصدرنا الاشعار ليها
            //    InvoiceDocumentReference invoiceDocumentReference = new InvoiceDocumentReference();
            //    invoiceDocumentReference.ID = "Invoice Number: 354; Invoice Issue Date: 2021-02-10"; // اجبارى
            //    inv.billingReference.invoiceDocumentReferences.Add(invoiceDocumentReference);
            //}
            inv.AdditionalDocumentReferencePIH.EmbeddedDocumentBinaryObject = "NWZlY2ViNjZmZmM4NmYzOGQ5NTI3ODZjNmQ2OTZjNzljMmRiYzIzOWRkNGU5MWI0NjcyOWQ3M2EyN2ZiNTdlOQ==";

            inv.AdditionalDocumentReferenceICV.UUID = 1;
            PaymentMeans paymentMeans = new PaymentMeans();
            paymentMeans.PaymentMeansCode = "10";
            paymentMeans.InstructionNote = "Payment Notes";
            inv.paymentmeans.Add(paymentMeans);
            // بيانات البائع 
            inv.SupplierParty.partyIdentification.ID = objOrganization.PostalCode; //هنا رقم السجل التجارى للشركة
            inv.SupplierParty.partyIdentification.schemeID = "CRN";
            inv.SupplierParty.postalAddress.StreetName = objOrganization.StreetName; // اجبارى
            inv.SupplierParty.postalAddress.AdditionalStreetName = "شارع اضافى"; // اختيارى
            inv.SupplierParty.postalAddress.BuildingNumber = objOrganization.BuildingNumber; // اجبارى رقم المبنى
            inv.SupplierParty.postalAddress.PlotIdentification = "9833";
            inv.SupplierParty.postalAddress.CityName = objOrganization.CityName;
            inv.SupplierParty.postalAddress.PostalZone = objOrganization.PostalCodeFinal; // الرقم البريدي
            inv.SupplierParty.postalAddress.CountrySubentity = objOrganization.CityName; // اسم المحافظة او المدينة مثال (مكة) اختيارى
            inv.SupplierParty.postalAddress.CitySubdivisionName = objOrganization.Neighborhood; // اسم المنطقة او الحى 
            inv.SupplierParty.postalAddress.country.IdentificationCode = "SA";
            inv.SupplierParty.partyLegalEntity.RegistrationName = objOrganization.NameAr; // "شركة الصناعات الغذائية المتحده"; // اسم الشركة المسجل فى الهيئة
            inv.SupplierParty.partyTaxScheme.CompanyID = objOrganization.TaxCode;// "300518376300003";  // رقم التسجيل الضريبي

            inv.CustomerParty.partyIdentification.ID = "1234567"; // رقم القومى الخاض بالمشترى
            inv.CustomerParty.partyIdentification.schemeID = "CRN"; // الرقم القومى
            inv.CustomerParty.postalAddress.StreetName = "شارع تجريبي"; // اجبارى
            inv.CustomerParty.postalAddress.AdditionalStreetName = "شارع اضافى"; // اختيارى
            inv.CustomerParty.postalAddress.BuildingNumber = "1234"; // اجبارى رقم المبنى
            inv.CustomerParty.postalAddress.PlotIdentification = "9833"; // اختيارى رقم القطعة
            inv.CustomerParty.postalAddress.CityName = "Jeddah"; // اسم المدينة
            inv.CustomerParty.postalAddress.PostalZone = "12345"; // الرقم البريدي
            inv.CustomerParty.postalAddress.CountrySubentity = "Makkah"; // اسم المحافظة او المدينة مثال (مكة) اختيارى
            inv.CustomerParty.postalAddress.CitySubdivisionName = "اسم المنطقة او الحى "; // اسم المنطقة او الحى 
            inv.CustomerParty.postalAddress.country.IdentificationCode = "SA";
            inv.CustomerParty.partyLegalEntity.RegistrationName = "اسم شركة المشترى"; // اسم الشركة المسجل فى الهيئة
            inv.CustomerParty.partyTaxScheme.CompanyID = "310424415000003"; // رقم التسجيل الضريبي


            inv.legalMonetaryTotal.PrepaidAmount = 0;
            inv.legalMonetaryTotal.PayableAmount = 0;

            InvoiceLine invline = new InvoiceLine();
            invline.InvoiceQuantity = 1;
            invline.item.Name = "منتج تجريبي";
            invline.item.classifiedTaxCategory.ID = "S"; // كود الضريبة
            invline.taxTotal.TaxSubtotal.taxCategory.ID = "S"; // كود الضريبة
            invline.item.classifiedTaxCategory.Percent = 15; // نسبة الضريبة
            invline.taxTotal.TaxSubtotal.taxCategory.Percent = 15; // نسبة الضريبة
            invline.price.PriceAmount = 1;
            inv.InvoiceLines.Add(invline);


            CertificateRequest certrequest = GetCSRRequest(OTP, objOrganization, objBranch);

            if (objOrganization.ModeType == 2) { mode = Mode.Simulation; }
            else if (objOrganization.ModeType == 3) { mode = Mode.Production; }
            else { mode = Mode.developer; }

            CSIDGenerator generator = new CSIDGenerator(mode);
            //var path = Directory.GetCurrentDirectory();
            //path = path + "\\" + "cert";

            var path = Path.Combine("cert");

            //path = "D:\\Bayanatech\\Web Application\\APINew\\Bayanatech.OnionArchitecture\\TaamerProject.API\\cert";
            //path = System.IO.Path.Combine("cert");
            CertificateResponse response = generator.GenerateCSID(certrequest, inv, path);
            //CertificateResponse response = generator.GenerateCSR(certrequest);

            if (response.IsSuccess)
            {

                var result = _organizationsservice.SaveCSIDOrganizations(OrganizationId, response.CSR, response.PrivateKey, response.CSID, response.SecretKey, _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(result);
            }
            else
            {
                var result = _organizationsservice.SaveErrorMessageCSIDOrganizations(OrganizationId, response.ErrorMessage, _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = response.ErrorMessage });
            }
        }
        [HttpGet("GetCSRRequest")]
        private CertificateRequest GetCSRRequest(string otp, OrganizationsVM ORG, BranchesVM branch)
        {
            //فاتورة ضريبية & مبسطة 1100
            //فاتورة ضريبية فقط 1000
            //فاتورة مبسطة فقط 0100

            //var Serial = "1-Tameer" + "|" + "2-version2.0.1" + "|" + "3-" + Guid.NewGuid().ToString();
            var Serial = "1-"+ ORG.NameEn + "|" + "2-version2.0.1" + "|" + "3-" + Guid.NewGuid().ToString();

            CertificateRequest certrequest = new CertificateRequest();
            certrequest.OTP = otp;
            certrequest.CommonName = branch.NameEn!.Trim() + GenerateRandomNo();//System.Guid.NewGuid();
            certrequest.OrganizationName = ORG.NameAr!.Trim();
            certrequest.OrganizationUnitName = branch.NameEn!.Trim(); //branch name
            certrequest.CountryName = "SA";
            certrequest.SerialNumber = Serial!.Trim();
            certrequest.OrganizationIdentifier = ORG.TaxCode!.Trim();
            certrequest.Location = ORG.Address!.Trim();
            certrequest.BusinessCategory = "Engineering consultant"; 
            certrequest.InvoiceType = "1100";
            return certrequest;
        }

        [HttpPost("GenerateCSID_Branch")]
        public IActionResult GenerateCSID_Branch(int BranchId, string OTP)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var objBranch = _BranchesService.GetBranchByBranchId("rtl", BranchId).Result.FirstOrDefault();
            var objOrganization = _organizationsservice.GetBranchOrganization().Result;

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


            if (objBranch.Address == null || objBranch.Address == "")
                Address1 = objOrganization.Address!.Trim();
            else
            Address1 = objBranch.Address!.Trim();
            if (objBranch.BuildingNumber == null || objBranch.BuildingNumber == "")
                BuildingNumber1 = objOrganization.BuildingNumber!.Trim();
            else
                BuildingNumber1 = objBranch.BuildingNumber!.Trim();
            if (objBranch.StreetName == null || objBranch.StreetName == "")
                StreetName1 = objOrganization.StreetName!.Trim();
            else
                StreetName1 = objBranch.StreetName!.Trim();
            if (objBranch.Neighborhood == null || objBranch.Neighborhood == "")
                Neighborhood1 = objOrganization.Neighborhood!.Trim();
            else
                Neighborhood1 = objBranch.Neighborhood!.Trim();
            if (objBranch.CityName == null || objBranch.CityName == "")
                CityName1 = objOrganization.CityName!.Trim();
            else
                CityName1 = objBranch.CityName!.Trim();
            if (objBranch.Country == null || objBranch.Country == "")
                Country1 = objOrganization.Country!.Trim();
            else
                Country1 = objBranch.Country!.Trim();
            if (objBranch.PostalCode == null || objBranch.PostalCode == "")
                PostalCode1 = objOrganization.PostalCode!.Trim();
            else
                PostalCode1 = objBranch.PostalCode.Trim();
            if (objBranch.PostalCodeFinal == null || objBranch.PostalCodeFinal == "")
                PostalCodeFinal1 = objOrganization.PostalCodeFinal!.Trim();
            else
                PostalCodeFinal1 = objBranch.PostalCodeFinal!.Trim();
            if (objBranch.ExternalPhone == null || objBranch.ExternalPhone == "")
                ExternalPhone1 = objOrganization.ExternalPhone!.Trim();
            else
                ExternalPhone1 = objOrganization.ExternalPhone!.Trim();
            if (objBranch.TaxCode == null || objBranch.TaxCode == "")
            {
                TaxCode1 = objOrganization.TaxCode!.Trim();
            }
            else
                TaxCode1 = objBranch.TaxCode.Trim();






            Invoice inv = new Invoice();
            inv.ID = "INV00001"; // مثال SME00010

            //inv.IssueDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            inv.IssueDate = DateTime.Now.ToString("yyyy-MM-dd");
            inv.IssueTime = DateTime.Now.ToString("HH:mm:ss"); // "09:32:40"

            inv.DocumentCurrencyCode = "SAR";
            inv.TaxCurrencyCode = "SAR";
            //if (inv.invoiceTypeCode.id == 383 || inv.invoiceTypeCode.id == 381)
            //{
            //    // فى حالة ان اشعار دائن او مدين فقط هانكتب رقم الفاتورة اللى اصدرنا الاشعار ليها
            //    InvoiceDocumentReference invoiceDocumentReference = new InvoiceDocumentReference();
            //    invoiceDocumentReference.ID = "Invoice Number: 354; Invoice Issue Date: 2021-02-10"; // اجبارى
            //    inv.billingReference.invoiceDocumentReferences.Add(invoiceDocumentReference);
            //}
            inv.AdditionalDocumentReferencePIH.EmbeddedDocumentBinaryObject = "NWZlY2ViNjZmZmM4NmYzOGQ5NTI3ODZjNmQ2OTZjNzljMmRiYzIzOWRkNGU5MWI0NjcyOWQ3M2EyN2ZiNTdlOQ==";

            inv.AdditionalDocumentReferenceICV.UUID = 1;
            PaymentMeans paymentMeans = new PaymentMeans();
            paymentMeans.PaymentMeansCode = "10";
            paymentMeans.InstructionNote = "Payment Notes";
            inv.paymentmeans.Add(paymentMeans);

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


            inv.CustomerParty.partyIdentification.ID = "1234567"; // رقم القومى الخاض بالمشترى
            inv.CustomerParty.partyIdentification.schemeID = "CRN"; // الرقم القومى
            inv.CustomerParty.postalAddress.StreetName = "شارع تجريبي"; // اجبارى
            inv.CustomerParty.postalAddress.AdditionalStreetName = "شارع اضافى"; // اختيارى
            inv.CustomerParty.postalAddress.BuildingNumber = "1234"; // اجبارى رقم المبنى
            inv.CustomerParty.postalAddress.PlotIdentification = "9833"; // اختيارى رقم القطعة
            inv.CustomerParty.postalAddress.CityName = "Jeddah"; // اسم المدينة
            inv.CustomerParty.postalAddress.PostalZone = "12345"; // الرقم البريدي
            inv.CustomerParty.postalAddress.CountrySubentity = "Makkah"; // اسم المحافظة او المدينة مثال (مكة) اختيارى
            inv.CustomerParty.postalAddress.CitySubdivisionName = "اسم المنطقة او الحى "; // اسم المنطقة او الحى 
            inv.CustomerParty.postalAddress.country.IdentificationCode = "SA";
            inv.CustomerParty.partyLegalEntity.RegistrationName = "اسم شركة المشترى"; // اسم الشركة المسجل فى الهيئة
            inv.CustomerParty.partyTaxScheme.CompanyID = "310424415000003"; // رقم التسجيل الضريبي


            inv.legalMonetaryTotal.PrepaidAmount = 0;
            inv.legalMonetaryTotal.PayableAmount = 0;

            InvoiceLine invline = new InvoiceLine();
            invline.InvoiceQuantity = 1;
            invline.item.Name = "منتج تجريبي";
            invline.item.classifiedTaxCategory.ID = "S"; // كود الضريبة
            invline.taxTotal.TaxSubtotal.taxCategory.ID = "S"; // كود الضريبة
            invline.item.classifiedTaxCategory.Percent = 15; // نسبة الضريبة
            invline.taxTotal.TaxSubtotal.taxCategory.Percent = 15; // نسبة الضريبة
            invline.price.PriceAmount = 1;
            inv.InvoiceLines.Add(invline);


            CertificateRequest certrequest = GetCSRRequest_Branch(OTP, objOrganization, objBranch);

            if (objOrganization.ModeType == 2) { mode = Mode.Simulation; }
            else if (objOrganization.ModeType == 3) { mode = Mode.Production; }
            else { mode = Mode.developer; }
            CSIDGenerator generator = new CSIDGenerator(mode);
            //var path = Directory.GetCurrentDirectory();
            //path = path + "\\" + "cert";
            var path = Path.Combine("cert");
            //path = "D:\\Bayanatech\\Web Application\\APINew\\Bayanatech.OnionArchitecture\\TaamerProject.API\\cert";
            //path = System.IO.Path.Combine("cert");
            CertificateResponse response = generator.GenerateCSID(certrequest, inv, path);
            //CertificateResponse response = generator.GenerateCSR(certrequest);

            if (response.IsSuccess)
            {

                //var result = _organizationsservice.SaveCSIDOrganizations(OrganizationId, response.CSR, response.PrivateKey, response.CSID, response.SecretKey, _globalshared.UserId_G, _globalshared.BranchId_G);
                var result = _BranchesService.SaveCSIDBranch(BranchId, response.CSR, response.PrivateKey, response.CSID, response.SecretKey, _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(result);
            }
            else
            {
                return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = response.ErrorMessage });
            }
        }
        [HttpGet("GetCSRRequest_Branch")]
        private CertificateRequest GetCSRRequest_Branch(string otp, OrganizationsVM ORG, BranchesVM branch)
        {
            //فاتورة ضريبية & مبسطة 1100
            //فاتورة ضريبية فقط 1000
            //فاتورة مبسطة فقط 0100

            //var Serial = "1-Tameer" + "|" + "2-version2.0.1" + "|" + "3-" + Guid.NewGuid().ToString();
            var Serial = "1-" + ORG.NameEn + "|" + "2-version2.0.1" + "|" + "3-" + Guid.NewGuid().ToString();

            CertificateRequest certrequest = new CertificateRequest();
            certrequest.OTP = otp;
            certrequest.CommonName = branch.NameEn!.Trim() + GenerateRandomNo(); //+ System.Guid.NewGuid();
            certrequest.OrganizationName = ORG.NameAr!.Trim();
            certrequest.OrganizationUnitName = branch.NameEn!.Trim(); //branch name
            certrequest.CountryName = "SA";
            certrequest.SerialNumber = Serial!.Trim();
            certrequest.OrganizationIdentifier = (branch.TaxCode?? ORG.TaxCode)!.Trim();
            certrequest.Location = (branch.Address ?? ORG.Address)!.Trim();
            certrequest.BusinessCategory = "Engineering consultant";
            certrequest.InvoiceType = "1100";
            return certrequest;
        }

        [HttpPost("SaveEmailSetting")]
        public ActionResult SaveEmailSetting(EmailSetting EmailSetting)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _EmailSettingservice.SaveEmailSetting(EmailSetting, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveComDomainLink")]
        public ActionResult SaveComDomainLink(Organizations organizations)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.SaveComDomainLink(organizations, _globalshared.UserId_G, _globalshared.BranchId_G);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    string fileName = "ComDomainLink.Jpeg";
                    string fileLocation = Path.Combine("Uploads/Organizations/DomainLink/") + fileName;

                    string ImgReturn = "";
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(organizations.ComDomainLink, QRCodeGenerator.ECCLevel.Q, true);
                    QRCode qrCode = new QRCode(qrCodeData);
                    using (Bitmap bitMap = qrCode.GetGraphic(20))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            byte[] byteImage = ms.ToArray();
                            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);

                            ImgReturn = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                            if (System.IO.File.Exists(fileLocation))
                            {
                                System.IO.File.Delete(fileLocation);
                            }
                            img.Save(fileLocation, System.Drawing.Imaging.ImageFormat.Jpeg);

                        }

                    }
                }
                catch (Exception ex)
                {

                }

            }

            if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
            {

                result.ReasonPhrase = "Saved Successfully";
            }
            else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
            {
                result.ReasonPhrase = "Saved Falied";
            }
            return Ok(result);
        }

        [HttpPost("SaveAppVersion")]
        public ActionResult SaveAppVersion(Organizations organizations)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.SaveAppVersion(organizations, _globalshared.UserId_G, _globalshared.BranchId_G);



            if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
            {

                result.ReasonPhrase = "Saved Successfully";
            }
            else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
            {
                result.ReasonPhrase = "Saved Falied";
            }
            return Ok(result);
        }

        [HttpPost("SaveSupport")]
        public ActionResult SaveSupport(Organizations organizations)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _organizationsservice.SaveSupport(organizations, _globalshared.UserId_G, _globalshared.BranchId_G);



            if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
            {

                result.ReasonPhrase = "Saved Successfully";
            }
            else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
            {
                result.ReasonPhrase = "Saved Falied";
            }
            return Ok(result);
        }
        [HttpPost("SavesmsSetting")]
        public ActionResult SavesmsSetting(SMSSettings sMSSettings)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _SMSSettingsService.SavesmsSetting(sMSSettings, _globalshared.UserId_G, _globalshared.BranchId_G);
            if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
            {
                result.ReasonPhrase = "Saved Successfully";
            }
            else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
            {
                result.ReasonPhrase = "Saved Falied";
            }
            return Ok(result);
        }
        [HttpPost("SaveWhatsAppSetting")]
        public ActionResult SaveWhatsAppSetting(WhatsAppSettings whatsAppSettings)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _WhatsAppSettingsService.SaveWhatsAppSetting(whatsAppSettings, _globalshared.UserId_G, _globalshared.BranchId_G);
            if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
            {
                result.ReasonPhrase = "Saved Successfully";
            }
            else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
            {
                result.ReasonPhrase = "Saved Falied";
            }
            return Ok(result);
        }
        [HttpPost("GetEmailSetting")]
        public ActionResult GetEmailSetting()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_EmailSettingservice.GetEmailSetting(_globalshared.BranchId_G));
        }
        [HttpGet("GetAttDeviceSetting")]

        public ActionResult GetAttDeviceSetting()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_attDeviceService.GetDevicesetting(_globalshared.BranchId_G));
        }
        [HttpPost("SaveAttDeviceSetting")]

        public ActionResult SaveAttDeviceSetting(AttDeviceSitting attDeviceSitting)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attDeviceService.SaveAttdeviceSetting(attDeviceSitting, _globalshared.UserId_G, _globalshared.BranchId_G);
            if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
            {
                result.ReasonPhrase = "Saved Successfully";
            }
            else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
            {
                result.ReasonPhrase = "Saved Falied";
            }
            return Ok(result);
        }
        [HttpGet("GetsmsSetting")]
        public ActionResult GetsmsSetting()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_SMSSettingsService.GetsmsSetting(_globalshared.BranchId_G));
        }
        [HttpGet("GetWhatsAppSetting")]
        public ActionResult GetWhatsAppSetting()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_WhatsAppSettingsService.GetWhatsAppSetting(_globalshared.BranchId_G));
        }
        [HttpGet("CheckEmailOrganization")]
        public ActionResult CheckEmailOrganization(int? OrganizationId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_organizationsservice.CheckEmailOrganization(OrganizationId));
        }


        [HttpGet("GenerateRandomNo")]
        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        [HttpGet("GetOrganizationDataLogin")]
        public ActionResult GetOrganizationDataLogin()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G));
        }
        [HttpGet("GenerateUserQR")]
        public ActionResult GenerateUserQR()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var org = _organizationsservice.GetOrganizationData(_globalshared.BranchId_G).Result;
            var user = _userService.GetUserById(_globalshared.UserId_G, _globalshared.Lang_G).Result;
            var qrstring = org.ComDomainLink + "/" + user.Email;


            try
            {
                string fileName = "EmpQrCodeImg.Jpeg";
                string fileLocation = Path.Combine("~/Uploads/Organizations/DomainLink/") + fileName;

                string ImgReturn = "";
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrstring, QRCodeGenerator.ECCLevel.Q, true);
                QRCode qrCode = new QRCode(qrCodeData);
                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] byteImage = ms.ToArray();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(ms);

                        ImgReturn = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }
                        img.Save(fileLocation, System.Drawing.Imaging.ImageFormat.Jpeg);

                    }

                }
                return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم الحفظ" });


            }
            catch (Exception ex)
            {
                return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في الحفظ" });

            }




        }
        [HttpGet("GetOrganizationData")]
        public IActionResult GetOrganizationData()
        {            
            var objOrganization = _organizationsservice.GetBranchOrganization().Result;
            return Ok(objOrganization);

        }

        [HttpGet("SendMail_test")]
        public IActionResult SendMail_test(string Email)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var objOrganization = _organizationsservice.SendMail_test(_globalshared.BranchId_G,Email, "اميل تجريبي", "اذا استطعت قراءة هذا البريد، فإن اعدادات البريد لديك صحيحة", true);
            return Ok(objOrganization);

        }

        [HttpGet("SendSMS_test")]
        public ActionResult SendSMS_test(string Mobile)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_SMSSettingsService.SendSMS_Test(_globalshared.UserId_G,_globalshared.BranchId_G, Mobile,"test"));
        }
        [HttpGet("SendWhatsApp_test")]
        public ActionResult SendWhatsApp_test(string Mobile, string? environmentURL)
        {
            string PDFURL = "";
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_WhatsAppSettingsService.SendWhatsApp_Test(_globalshared.UserId_G, _globalshared.BranchId_G, Mobile, "test", environmentURL??"", PDFURL??""));
        }
        public static Image ScaleImage(Image image, int height, int width)
        {
            //double ratio = (double)height / image.Height;
            //int newWidth = (int)(image.Width * ratio);
            //int newHeight = (int)(image.Height * ratio);
            int newWidth = width;
            int newHeight = height;
            Bitmap newImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            image.Dispose();
            return newImage;
        }
        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }
    }
}

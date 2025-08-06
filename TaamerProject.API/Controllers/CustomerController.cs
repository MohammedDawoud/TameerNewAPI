using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.Globalization;
using System.Net;
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

    public class CustomerController : ControllerBase
    {

            private ICustomerService _customerservice;
            private ICustomerMailService _CustomerMailService;
            private ICustomerSMSService _CustomerSMSService;
            private IBranchesService _branchesService;
            private IOrganizationsService _organizationsservice;
            private ISMSSettingsService _sMSSettingsService;
             private ISystemSettingsService _systemSettingsService;
            private string? Con;
            private IConfiguration Configuration;
            public GlobalShared _globalshared;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CustomerController(ICustomerService customerService, ICustomerMailService customerMailService, ICustomerSMSService customerSMSService, IBranchesService branchesService,
                IOrganizationsService organizationsService, ISystemSettingsService systemSettingsService, ISMSSettingsService sMSSettingsService, IConfiguration _configuration, IWebHostEnvironment webHostEnvironment)
            {
                this._customerservice = customerService;
                this._CustomerMailService = customerMailService;
            this._systemSettingsService = systemSettingsService;
            this._CustomerSMSService = customerSMSService;
                this._branchesService = branchesService;
                this._organizationsservice = organizationsService;
                this._sMSSettingsService = sMSSettingsService;
            Configuration = _configuration; Con = this.Configuration.GetConnectionString("DBConnection");
            HttpContext httpContext = HttpContext;

            _globalshared = new GlobalShared(httpContext);
            _hostingEnvironment = webHostEnvironment;

        }

        [HttpGet("GetAllCustomerForDrop")]

        public IActionResult GetAllCustomerForDrop()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_customerservice.GetAllCustomerForDrop(_globalshared.Lang_G) );
            }
        [HttpGet("GetAllCustomerForDropWithBranch")]

        public IActionResult GetAllCustomerForDropWithBranch()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            //var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G,_globalshared.UserId_G).Where(s=>s._globalshared.BranchId_G==1);
            //if(userBranchs.Count()>0)
            //{
            //    return Ok(_customerservice.GetAllCustomerForDrop(_globalshared.Lang_G) );
            //}
            //else
            //{
            //    return Ok(_customerservice.GetAllCustomerForDropWithBranch(_globalshared.Lang_G, _globalshared.BranchId_G) );
            //}
            return Ok(_customerservice.GetAllCustomerForDropWithBranch(_globalshared.Lang_G, _globalshared.BranchId_G) );

            }

        [HttpGet("GetLastCustomerAdded")]

        public IActionResult GetLastCustomerAdded()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            var someCustomer = _customerservice.GetAllCustomerExist(_globalshared.Lang_G);
                var custid = someCustomer.Result.Max(M => M.CustomerId);
                var cust = _customerservice.GetCustomersByCustomerId(custid, _globalshared.Lang_G);

                return Ok(cust );
            }

        [HttpGet("GetAllCustomers")]
        public IActionResult GetAllCustomers()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            List<CustomerVM> someCustomer = GatAllCustomers_List();
                //var serializer = new JavaScriptSerializer();
                //serializer.MaxJsonLength = Int32.MaxValue;
                //var result = new ContentResult
                //{
                //    Content = serializer.Serialize(someCustomer),
                //    ContentType = "application/json"
                //};
                //return result;
               return Ok(someCustomer );
            }

        [HttpGet("GetAllCustomers_Branch")]
        public IActionResult GetAllCustomers_Branch()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            List<CustomerVM> someCustomer = _customerservice.GetAllCustomers(_globalshared.Lang_G, _globalshared.BranchId_G, _globalshared.Privilliges_G.Contains(12101015)).Result.ToList();
            return Ok(someCustomer);
        }

        [HttpGet("GatAllCustomers_List")]
        private List<CustomerVM> GatAllCustomers_List()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G,_globalshared.UserId_G).Result;
           
            var someCustomer = _customerservice.GetAllCustomers(_globalshared.Lang_G, 0, _globalshared.Privilliges_G.Contains(12101015)).Result.ToList();

            foreach (var userBranch in userBranchs)
                {

                    var AllCustomers = _customerservice.GetAllCustomers(_globalshared.Lang_G, userBranch.BranchId, _globalshared.Privilliges_G.Contains(12101015)).Result.ToList();
                    var Customers = someCustomer.Union(AllCustomers);
                    someCustomer = Customers.ToList();
                }

                return someCustomer.DistinctBy(x=>x.CustomerId).ToList();
            }
        [HttpGet("GetAllCustomersCount")]

        public int GetAllCustomersCount()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
                var someCustomer = _customerservice.GetAllCustomersCount(0).Result.Count();
                foreach (var userBranch in userBranchs)
                {
                    var AllCustomers = _customerservice.GetAllCustomersCount(userBranch.BranchId).Result.Count();
                    var Customers = someCustomer + AllCustomers;
                    someCustomer = Customers;
                }
                return someCustomer;
            }

        [HttpGet("GetAllCustomersProj")]
        public IActionResult GetAllCustomersProj(int? BranchId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_customerservice.GetAllCustomersProj(_globalshared.Lang_G, _globalshared.BranchId_G) );
            }
        [HttpGet("CustomerIntervals")]
        public IActionResult CustomerIntervals(string? FromDate, string? ToDate)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_customerservice.CustomerInterval(FromDate??"", ToDate??"", _globalshared.BranchId_G, _globalshared.Lang_G) );
            }

        [HttpGet("GetAllCustomersByCustomerType")]

        public IActionResult GetAllCustomersByCustomerType(string? FromDate, string? ToDate, int CustomerType)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            //dawoud
            return Ok(_customerservice.GetAllCustomersByCustomerTypeId(FromDate ?? "", ToDate ?? "", CustomerType, _globalshared.BranchId_G, _globalshared.Lang_G) );
            }


        [HttpGet("GetCustomerExpensesRevenue")]
        public IActionResult GetCustomerExpensesRevenue(string? FromDate, string? ToDate)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_customerservice.GetCustomerExpensesRevenue(FromDate ?? "", ToDate ?? "", _globalshared.BranchId_G, _globalshared.Lang_G) );
            }

        [HttpGet("GetCustomerExpensesRevenueDGV")]
        public IActionResult GetCustomerExpensesRevenueDGV(string? FromDate, string? ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_customerservice.GetCustomerExpensesRevenueDGV(FromDate ?? "", ToDate ?? "", _globalshared.BranchId_G, _globalshared.Lang_G, Con) );
            }
        [HttpGet("GetAllCustomersByCustomerTypeId")]

        public IActionResult GetAllCustomersByCustomerTypeId(int? CustomerTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            List<CustomerVM> someCustomer = GetAllCustomersByCustomerTypeId_List(CustomerTypeId);
                return Ok(someCustomer );
            }
        [HttpGet("GetAllCustomersByCustomerTypeId_List")]
        private List<CustomerVM> GetAllCustomersByCustomerTypeId_List(int? CustomerTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
                var someCustomer = _customerservice.GetAllCustomersByCustomerTypeId(CustomerTypeId, _globalshared.Lang_G, 0, _globalshared.Privilliges_G.Contains(12101015)).Result.ToList();
                foreach (var userBranch in userBranchs)
                {

                    var AllCustomers = _customerservice.GetAllCustomersByCustomerTypeId(CustomerTypeId, _globalshared.Lang_G, userBranch.BranchId, _globalshared.Privilliges_G.Contains(12101015)).Result.ToList();
                    var Customers = someCustomer.Union(AllCustomers);
                    someCustomer = Customers.ToList();
                }

                return someCustomer;
            }
        [HttpGet("GetCustFinancialDetails")]

        public IActionResult GetCustFinancialDetails(int? CustomerId, string FromDate, string ToDate, int? DateType)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            if (FromDate != "" && ToDate != "" && DateType == 1)
                {
                    CultureInfo arCul = new CultureInfo("ar-SA");
                    HijriCalendar h = new HijriCalendar();
                    arCul.DateTimeFormat.Calendar = h;
                    DateTime fromdate = DateTime.ParseExact(FromDate, "yyyy-MM-dd", arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                    DateTime todate = DateTime.ParseExact(ToDate, "yyyy-MM-dd", arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                    FromDate = fromdate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    ToDate = todate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                }
                return Ok(_customerservice.GetCustFinancialDetails(FromDate, ToDate, CustomerId ?? 0, _globalshared.YearId_G) );
            }
        [HttpGet("GetCustTransactions")]

        public IActionResult GetCustTransactions(string FromDate, string ToDate, int? DateType)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            if (FromDate != "" && ToDate != "" && DateType == 1)
                {
                    CultureInfo arCul = new CultureInfo("ar-SA");
                    HijriCalendar h = new HijriCalendar();
                    arCul.DateTimeFormat.Calendar = h;
                    DateTime fromdate = DateTime.ParseExact(FromDate, "yyyy-MM-dd", arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                    DateTime todate = DateTime.ParseExact(ToDate, "yyyy-MM-dd", arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                    FromDate = fromdate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    ToDate = todate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                }

                var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
                var someCustomer = _customerservice.GetAllCustomerTrans(FromDate, ToDate, 0, _globalshared.YearId_G).Result;
                foreach (var userBranch in userBranchs)
                {

                    var AllCustomers = _customerservice.GetAllCustomerTrans(FromDate, ToDate, userBranch.BranchId, _globalshared.YearId_G).Result;
                    var Customers = someCustomer.Union(AllCustomers);
                    someCustomer = Customers.ToList();
                }

                return Ok(someCustomer );
            }

            [HttpPost]
            public async Task<IActionResult> Upload(IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                var fileName = Guid.NewGuid().ToString() + extension; //Create a new Name
                if (!System.IO.Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files")))
                {
                    System.IO.Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files"));
                }
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", fileName);

                using (var bits = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(bits);
                }

                return Ok(new
                {
                    fileName = $"{fileName}"
                });
           }
    

         [HttpPost("SaveCustomer")]
        public IActionResult SaveCustomer([FromForm] Customer customer, IFormFile? UploadedCustomerImage, IFormFile? UploadedCustomerFile,IFormFile? UploadedAgentImage)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            bool CheckExist = CheckFileExist("/Uploads/Customers");
                var massage = "";
                if (CheckExist == true)
                {
                if (UploadedCustomerImage != null)
                {
                    System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


                    string path = System.IO.Path.Combine("Uploads/", "Customers/");
                    string pathW = System.IO.Path.Combine("/Uploads/", "Customers/");

                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }

                    List<string> uploadedFiles = new List<string>();
                    string pathes = "";
                    //foreach (IFormFile postedFile in postedFiles)
                    //{
                    string fileName = System.IO.Path.GetFileName(Guid.NewGuid() + UploadedCustomerImage.FileName);
                    //string fileName = System.IO.Path.GetFileName(postedFiles.FileName);

                    var path2 = Path.Combine(path, fileName);
                    if (System.IO.File.Exists(path2))
                    {
                        System.IO.File.Delete(path2);
                    }
                    using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(path, fileName), System.IO.FileMode.Create))
                    {


                        UploadedCustomerImage.CopyTo(stream);
                        uploadedFiles.Add(fileName);
                        // string returnpath = host + path + fileName;
                        //pathes.Add(pathW + fileName);
                        pathes = pathW + fileName;
                    }

                
                    if (pathes != null)
                    {
                        customer.LogoUrl = pathes;
                    }
                }

                if (UploadedCustomerFile != null)
                {
                    System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


                    string path = System.IO.Path.Combine("Uploads/", "Customers/");
                    string pathW = System.IO.Path.Combine("/Uploads/", "Customers/");

                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }

                    List<string> uploadedFiles = new List<string>();
                    string pathes = "";
                    //foreach (IFormFile postedFile in postedFiles)
                    //{
                    string fileName = System.IO.Path.GetFileName(Guid.NewGuid() + UploadedCustomerFile.FileName);
                    //string fileName = System.IO.Path.GetFileName(postedFiles.FileName);

                    var path2 = Path.Combine(path, fileName);
                    if (System.IO.File.Exists(path2))
                    {
                        System.IO.File.Delete(path2);
                    }
                    using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(path, fileName), System.IO.FileMode.Create))
                    {


                        UploadedCustomerFile.CopyTo(stream);
                        uploadedFiles.Add(fileName);
                        // string returnpath = host + path + fileName;
                        //pathes.Add(pathW + fileName);
                        pathes = pathW + fileName;
                    }


                    if (pathes != null)
                    {
                        customer.AttachmentUrl = pathes;
                    }
                }


                if (UploadedAgentImage != null)
                {
                    System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


                    string path = System.IO.Path.Combine("Uploads/", "Customers/");
                    string pathW = System.IO.Path.Combine("/Uploads/", "Customers/");

                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }

                    List<string> uploadedFiles = new List<string>();
                    string pathes = "";
                    //foreach (IFormFile postedFile in postedFiles)
                    //{
                    string fileName = System.IO.Path.GetFileName(Guid.NewGuid() + UploadedAgentImage.FileName);
                    //string fileName = System.IO.Path.GetFileName(postedFiles.FileName);

                    var path2 = Path.Combine(path, fileName);
                    if (System.IO.File.Exists(path2))
                    {
                        System.IO.File.Delete(path2);
                    }
                    using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(path, fileName), System.IO.FileMode.Create))
                    {


                        UploadedAgentImage.CopyTo(stream);
                        uploadedFiles.Add(fileName);
                        // string returnpath = host + path + fileName;
                        //pathes.Add(pathW + fileName);
                        pathes = pathW + fileName;
                    }


                    if (pathes != null)
                    {
                        customer.AgentAttachmentUrl = pathes;
                    }
                }  
                }
                else
                {
                    if (_globalshared.Lang_G == "rtl")
                    {
                        massage = "فشل في إنشاء المجلد على السيرفر ";
                    }
                    else
                    {
                        massage = "Failed to create folder on server";
                    }
                    return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massage } );
                }
            string url2 = System.IO.Path.Combine("Email/MailStamp.html");

          //  var url2 = Server.MapPath("~/Email/MailStamp.html");

            //var file2 = Server.MapPath("~/dist/assets/images/logo.png");
            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
              //  var file2 = Server.MapPath("~") + org.LogoUrl;
            string file2 = System.IO.Path.Combine(org.LogoUrl);

            var result = _customerservice.SaveCustomer(customer, _globalshared.UserId_G, _globalshared.BranchId_G, url2, file2);
                //if (_globalshared.Lang_G == "ltr" && result.Result == false && result.Message == "قم بتغيير اسم الميل، فهو موجود بالفعل!")
                //{
                //    result.Message = "Change the name of the slope, it already exists!";
                //}
                if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "خطأ في الحفظ, فشل في انشاء حساب للعميل تأكد من انشاء حساب رئيسي للعملاء وربطه بالفرع الحالي ")
                {
                    result.ReasonPhrase = "Error saving, failed to create an account for the client. Make sure to create a master account for clients and link it to the current branch";
                }
                else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "فشل في الحفظ")
                {
                    result.ReasonPhrase = "Saved Falied";
                }
                else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
                {
                    result.ReasonPhrase = "Saved Successfully";
                }
            var result2 = _systemSettingsService.MaintenanceFunc(Con, _globalshared.Lang_G, _globalshared.BranchId_G, _globalshared.UserId_G, 0);
            return Ok(result);
            }
        [HttpGet("CheckFileExist")]
        public bool CheckFileExist(string path)
            {
                try
                {
                    if (!Directory.Exists(System.IO.Path.Combine(path)))
                    {
                        System.IO.Directory.CreateDirectory(System.IO.Path.Combine(path));

                    }
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        [HttpPost("DeleteCustomer")]
        public IActionResult DeleteCustomer(int CustomerId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _customerservice.DeleteCustomer(CustomerId, _globalshared.UserId_G, _globalshared.BranchId_G);
                if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "العميل عليه مشروع يجب ألغاءه لإتمام عملية الحذف")
                {
                    result.ReasonPhrase = "The customer has a project that must be canceled to complete the deletion.";
                }
                else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
                {
                    result.ReasonPhrase = "Deleted Successfully";
                }
                else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest && result.ReasonPhrase == "فشل في الحذف")
                {
                    result.ReasonPhrase = "Deleted Falied";
                }
                return Ok(new { result.StatusCode, result.ReasonPhrase });
            }

        [HttpGet("FillCustomerSelect")]
        public IActionResult FillCustomerSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            //var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
            //    var someCustomer = _customerservice.GetAllCustomers(_globalshared.Lang_G, 0, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
            //    {
            //        Id = s.CustomerId,
            //        Name = s.CustomerName
            //    });
            //    foreach (var userBranch in userBranchs)
            //    {

            //        var AllCustomers = _customerservice.GetAllCustomers(_globalshared.Lang_G, userBranch.BranchId, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
            //        {
            //            Id = s.CustomerId,
            //            Name = s.CustomerName
            //        });
            //        var Customers = someCustomer.Union(AllCustomers);
            //        someCustomer = Customers;
            //    }

            //    return Ok(someCustomer );


            var result = _organizationsservice.FillSelect_Cust(Con, _globalshared.UserId_G, 1, null, 0, _globalshared.BranchId_G).Result;
            return Ok(result);

        }


        //[HttpGet("FillCustomerSelectNew")]
        //public IActionResult FillCustomerSelectNew()
        //{
        //    HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        //    var result = _organizationsservice.FillSelect_Cust(Con, _globalshared.UserId_G, 1, null, 0, _globalshared.BranchId_G).Result;
        //    return Ok(result);
        //}


        [HttpGet("FillCustomerSelect2")]
        public IActionResult FillCustomerSelect2()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
                var someCustomer = _customerservice.GetAllCustomers(_globalshared.Lang_G, 0,_globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
                {
                    Id = s.CustomerId,
                    Name = s.CustomerName,
                    CustomerMobile = s.CustomerMobile
                });
                foreach (var userBranch in userBranchs)
                {

                    var AllCustomers = _customerservice.GetAllCustomers(_globalshared.Lang_G, userBranch.BranchId, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
                    {
                        Id = s.CustomerId,
                        Name = s.CustomerName,
                        CustomerMobile = s.CustomerMobile
                    });
                    var Customers = someCustomer.Union(AllCustomers);
                    someCustomer = Customers;
                }

                return Ok(someCustomer );
            }


        [HttpGet("FillCustomerSelect2Mails")]
        public IActionResult FillCustomerSelect2Mails()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
                var someCustomer = _customerservice.GetAllCustomers(_globalshared.Lang_G, 0,_globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
                {
                    Id = s.CustomerId,
                    Name = s.CustomerName,
                    CustomerMail = s.CustomerEmail,
                    CustomerMobile = s.CustomerMobile,
                });
                foreach (var userBranch in userBranchs)
                {

                    var AllCustomers = _customerservice.GetAllCustomers(_globalshared.Lang_G, userBranch.BranchId, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
                    {
                        Id = s.CustomerId,
                        Name = s.CustomerName,
                        CustomerMail = s.CustomerEmail,
                        CustomerMobile = s.CustomerMobile,
                    });
                    var Customers = someCustomer.Union(AllCustomers);
                    someCustomer = Customers;
                }

                return Ok(someCustomer );
            }


        [HttpGet("FillCustomerSelect2SearchMail")]
        public IActionResult FillCustomerSelect2SearchMail(string SearchText)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G,_globalshared.UserId_G).Result;
                var someCustomer = _customerservice.GetAllCustomersSearch(SearchText, _globalshared.Lang_G, 0, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
                {
                    Id = s.CustomerId,
                    Name = s.CustomerName,
                    CustomerMail = s.CustomerEmail
                });
                foreach (var userBranch in userBranchs)
                {

                    var AllCustomers = _customerservice.GetAllCustomersSearch(SearchText, _globalshared.Lang_G, userBranch.BranchId, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
                    {
                        Id = s.CustomerId,
                        Name = s.CustomerName,
                        CustomerMail = s.CustomerEmail
                    });
                    var Customers = someCustomer.Union(AllCustomers);
                    someCustomer = Customers;
                }

                return Ok(someCustomer );
            }

        [HttpGet("FillCustomerSelect2Search")]
        public IActionResult FillCustomerSelect2Search(string SearchText)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G,_globalshared.UserId_G).Result;
                var someCustomer = _customerservice.GetAllCustomersSearch(SearchText, _globalshared.Lang_G, 0, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
                {
                    Id = s.CustomerId,
                    Name = s.CustomerName,
                    CustomerMobile = s.CustomerMobile
                });
                foreach (var userBranch in userBranchs)
                {

                    var AllCustomers = _customerservice.GetAllCustomersSearch(SearchText, _globalshared.Lang_G, userBranch.BranchId, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
                    {
                        Id = s.CustomerId,
                        Name = s.CustomerName,
                        CustomerMobile = s.CustomerMobile
                    });
                    var Customers = someCustomer.Union(AllCustomers);
                    someCustomer = Customers;
                }

                return Ok(someCustomer );
            }

        [HttpGet("FillCustomersOwnProjects")]
        public IActionResult FillCustomersOwnProjects()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G,_globalshared.UserId_G).Result;
                var someCustomer = _customerservice.GetCustomersOwnProjects(_globalshared.Lang_G, 0, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
                {
                    Id = s.CustomerId,
                    Name = s.CustomerName
                });
                foreach (var userBranch in userBranchs)
                {

                    var AllCustomers = _customerservice.GetCustomersOwnProjects(_globalshared.Lang_G, userBranch.BranchId, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
                    {
                        Id = s.CustomerId,
                        Name = s.CustomerName
                    });
                    var Customers = someCustomer.Union(AllCustomers);
                    someCustomer = Customers;
                }

                return Ok(someCustomer );
            }

        [HttpGet("FillCustomersOwnProjectsByBranch")]
        public IActionResult FillCustomersOwnProjectsByBranch()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var someCustomer = _customerservice.GetCustomersOwnProjects(_globalshared.Lang_G, _globalshared.BranchId_G, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
            {
                Id = s.CustomerId,
                Name = s.CustomerName
            });
         

            return Ok(someCustomer);
        }


        [HttpGet("GetCustomersOwnNotArcheivedProjects")]
        public IActionResult GetCustomersOwnNotArcheivedProjects()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G,_globalshared.UserId_G).Result;
                var someCustomer = _customerservice.GetCustomersOwnNotArcheivedProjects(_globalshared.Lang_G, 0, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
                {
                    Id = s.CustomerId,
                    Name = s.CustomerName
                });
                foreach (var userBranch in userBranchs)
                {

                    var AllCustomers = _customerservice.GetCustomersOwnNotArcheivedProjects(_globalshared.Lang_G, userBranch.BranchId, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
                    {
                        Id = s.CustomerId,
                        Name = s.CustomerName
                    });
                    var Customers = someCustomer.Union(AllCustomers);
                    someCustomer = Customers;
                }

                return Ok(someCustomer );
            }


        [HttpGet("GetCustomersOwnNotArcheivedProjectsByBranch")]
        public IActionResult GetCustomersOwnNotArcheivedProjectsByBranch()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
            var someCustomer = _customerservice.GetCustomersOwnNotArcheivedProjects(_globalshared.Lang_G, _globalshared.BranchId_G, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
            {
                Id = s.CustomerId,
                Name = s.CustomerName
            });
          

                
            

            return Ok(someCustomer);
        }

        [HttpGet("FillCustomersSelect_ArchivesProjects")]

        public IActionResult FillCustomersSelect_ArchivesProjects()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G,_globalshared.UserId_G).Result;
                var someCustomer = _customerservice.GetCustomersArchiveProjects(_globalshared.Lang_G, 0, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
                {
                    Id = s.CustomerId,
                    Name = s.CustomerName
                });
                foreach (var userBranch in userBranchs)
                {

                    var AllCustomers = _customerservice.GetCustomersArchiveProjects(_globalshared.Lang_G, userBranch.BranchId, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
                    {
                        Id = s.CustomerId,
                        Name = s.CustomerName
                    });
                    var Customers = someCustomer.Union(AllCustomers);
                    someCustomer = Customers;
                }

                return Ok(someCustomer );
            }


        [HttpGet("FillCustomerSelectW")]
        public IActionResult FillCustomerSelectW()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G,_globalshared.UserId_G).Result;
                var someCustomer = _customerservice.GetAllCustomersW(_globalshared.Lang_G, 0, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
                {
                    Id = s.CustomerId,
                    Name = s.CustomerName
                });
                foreach (var userBranch in userBranchs)
                {

                    var AllCustomers = _customerservice.GetAllCustomersW(_globalshared.Lang_G, userBranch.BranchId, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
                    {
                        Id = s.CustomerId,
                        Name = s.CustomerName
                    });
                    var Customers = someCustomer.Union(AllCustomers);
                    someCustomer = Customers;
                }

                return Ok(someCustomer );
            }

        [HttpGet("FillCustomerSelectByType")]
        public IActionResult FillCustomerSelectByType(int? param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G,_globalshared.UserId_G).Result;

                var someCustomer = _customerservice.GetAllCustomersByCustomerTypeId(param, _globalshared.Lang_G, 0, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
                {
                    Id = s.CustomerId,
                    Name = s.CustomerName
                });
                foreach (var userBranch in userBranchs)
                {

                    var AllCustomers = _customerservice.GetAllCustomersByCustomerTypeId(param, _globalshared.Lang_G, userBranch.BranchId, _globalshared.Privilliges_G.Contains(12101015)).Result.Select(s => new
                    {
                        Id = s.CustomerId,
                        Name = s.CustomerName
                    });
                    var Customers = someCustomer.Union(AllCustomers);
                    someCustomer = Customers;
                }

                return Ok(someCustomer );
            }

        [HttpPost("SearchCustomers")]
        public IActionResult SearchCustomers(CustomerVM CustomersSearch)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G,_globalshared.UserId_G).Result;
                var someCustomer = _customerservice.SearchCustomers(CustomersSearch, _globalshared.Lang_G, 0).Result;
                foreach (var userBranch in userBranchs)
                {
                    var AllCustomers = _customerservice.SearchCustomers(CustomersSearch, _globalshared.Lang_G, userBranch.BranchId).Result.ToList();
                    var Customers = someCustomer.Union(AllCustomers);
                    someCustomer = Customers.ToList();
                }
                return Ok(someCustomer );


        }
        [HttpPost("GenerateNextCustomerCodeNumber")]
        public IActionResult GenerateNextCustomerCodeNumber()
            {
                return Ok(_customerservice.GenerateNextCustomerCodeNumber() );
            }
            private readonly Random _random = new Random();
        [HttpPost("RandomNumber")]

        public int RandomNumber(int min, int max)
            {
                return _random.Next(min, max);
            }
        ////Customer Mails////


        [HttpPost("SaveCustomerMail")]
        public IActionResult SaveCustomerMail([FromForm] CustomerMail CustomerMail, [FromForm] IFormFile? UploadedFile, [FromForm] bool? IsOrgEmail = null)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var filePath = "";

            if (UploadedFile != null)
            {
                System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


                string path = System.IO.Path.Combine("Uploads/", "CustomersMails/");
                string pathW = System.IO.Path.Combine("/Uploads/", "CustomersMails/");

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
                    CustomerMail.FileUrl = pathes;
                    filePath = System.IO.Path.Combine(path, fileName);
                }

            }
            //HttpPostedFileBase file = Request.Files["UploadedFile"];

            //if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            //{
            //    if (file.ContentLength > 0)
            //    {
            //        string fileLocation = Server.MapPath("~/Uploads/CustomersMails");
            //        try
            //        {
            //            if (!System.IO.Directory.Exists(fileLocation))
            //            {

            //                Directory.CreateDirectory(fileLocation).Delete();
            //                //System.IO.Directory.CreateDirectory(fileLocation);
            //            }
            //            string Ran_C = RandomNumber(1, 10000).ToString();
            //            filePath = Server.MapPath("~/Uploads/CustomersMails/") + Ran_C + file.FileName;
            //            file.SaveAs(filePath);

            //            CustomerMail.FileUrl = "/Uploads/CustomersMails/" + Ran_C + file.FileName;

            //            //file.InputStream.Close();
            //            //file.InputStream.Dispose();
            //            //file.InputStream.Dispose();


            //        }
            //        catch (Exception ex)
            //        {
            //            filePath = "";
            //            var massage = "";
            //            if (_globalshared.Lang_G == "rtl")
            //            {
            //                massage = "فشل في رفع المرفقات";
            //            }
            //            else
            //            {
            //                massage = "Failed To Upload Files";
            //            }
            //            return Ok(new GeneralMessage { Result = false, Message = massage } );
            //        }
            //    }
            //}
            //if(filePath!="")
            //{
            //    var File = filePath;

            //}
            var result = _CustomerMailService.SaveCustomerMail(CustomerMail,_globalshared.UserId_G, _globalshared.BranchId_G, filePath, IsOrgEmail);
                if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
                {
                    result.ReasonPhrase = "send Successfully";
                }
                else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
                {
                    result.ReasonPhrase = "Failed to send, check e-mail settings.";
                }
                return Ok(new { result.StatusCode, result.ReasonPhrase } );
            }


        [HttpPost("SaveCustomerMailOfferPrice")]
        public ActionResult SaveCustomerMailOfferPrice([FromForm]CustomerMailVM? CustomerMail, [FromForm] IFormFile? UploadedFile, [FromForm] string? body, [FromForm] bool? IsOrgEmail = null)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            var filePath = "";

            if (UploadedFile != null)
            {
                System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


                string path = System.IO.Path.Combine("Uploads/", "CustomersMails/");
                string pathW = System.IO.Path.Combine("/Uploads/", "CustomersMails/");

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
                filePath = path2;
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
                    CustomerMail.FileUrl = pathes;
                }
            }

            //HttpPostedFileBase file = Request.Files["UploadedFile"];

            //if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            //{
            //    if (file.ContentLength > 0)
            //    {
            //        string fileLocation = Server.MapPath("~/Uploads/CustomersMails");
            //        try
            //        {
            //            if (!System.IO.Directory.Exists(fileLocation))
            //            {

            //                Directory.CreateDirectory(fileLocation).Delete();
            //                //System.IO.Directory.CreateDirectory(fileLocation);
            //            }
            //            string Ran_C = RandomNumber(1, 10000).ToString();
            //            filePath = Server.MapPath("~/Uploads/CustomersMails/") + Ran_C + file.FileName;
            //            file.SaveAs(filePath);

            //            CustomerMail.FileUrl = "/Uploads/CustomersMails/" + Ran_C + file.FileName;

            //            //file.InputStream.Close();
            //            //file.InputStream.Dispose();
            //            //file.InputStream.Dispose();


            //        }
            //        catch (Exception ex)
            //        {
            //            filePath = "";
            //            var massage = "";
            //            if (_globalshared.Lang_G == "rtl")
            //            {
            //                massage = "فشل في رفع المرفقات";
            //            }
            //            else
            //            {
            //                massage = "Failed To Upload Files";
            //            }
            //            return Ok(new GeneralMessage { Result = false, Message = massage } );
            //        }
            //    }
            //}
            //if(filePath!="")
            //{
            //    var File = filePath;

            //}
            var result = _CustomerMailService.SaveCustomerMailOfferPrice(CustomerMail,_globalshared.UserId_G, _globalshared.BranchId_G, filePath, body, IsOrgEmail);
                if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
                {
                    result.ReasonPhrase = "send Successfully";
                }
                else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
                {
                    result.ReasonPhrase = "Failed to send, check e-mail settings.";
                }
                return Ok(new { result.StatusCode, result.ReasonPhrase } );
            }


        [HttpPost("SaveManyCustomerMail")]
        public IActionResult SaveManyCustomerMail(CustomerMail CustomerMail, IFormFile UploadedFile, bool? IsOrgEmail = null)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var filePath = "";

            if (UploadedFile != null)
            {
                System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


                string path = System.IO.Path.Combine("Uploads/", "CustomersMails/");
                string pathW = System.IO.Path.Combine("/Uploads/", "CustomersMails/");

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
                    CustomerMail.FileUrl = pathes;
                }
            }
            //HttpPostedFileBase file = Request.Files["UploadedFile"];

            //if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            //{
            //    if (file.ContentLength > 0)
            //    {
            //        string fileLocation = Server.MapPath("~/Uploads/CustomersMails");
            //        try
            //        {
            //            if (!System.IO.Directory.Exists(fileLocation))
            //            {

            //                Directory.CreateDirectory(fileLocation).Delete();
            //                //System.IO.Directory.CreateDirectory(fileLocation);
            //            }
            //            string Ran_C = RandomNumber(1, 10000).ToString();
            //            filePath = Server.MapPath("~/Uploads/CustomersMails/") + Ran_C + file.FileName;
            //            file.SaveAs(filePath);

            //            CustomerMail.FileUrl = "/Uploads/CustomersMails/" + Ran_C + file.FileName;

            //            //file.InputStream.Close();
            //            //file.InputStream.Dispose();
            //            //file.InputStream.Dispose();


            //        }
            //        catch (Exception ex)
            //        {
            //            filePath = "";
            //            var massage = "";
            //            if (_globalshared.Lang_G == "rtl")
            //            {
            //                massage = "فشل في رفع المرفقات";
            //            }
            //            else
            //            {
            //                massage = "Failed To Upload Files";
            //            }
            //            return Ok(new GeneralMessage { Result = false, Message = massage } );
            //        }
            //    }
            //}
            //if(filePath!="")
            //{
            //    var File = filePath;

            //}
            var result = _CustomerMailService.SaveCustomerMail(CustomerMail,_globalshared.UserId_G, _globalshared.BranchId_G, filePath, IsOrgEmail);
            if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
            {
                result.ReasonPhrase = "send Successfully";
            }
            else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
            {
                result.ReasonPhrase = "Failed to send, check e-mail settings.";
            }
            return Ok(new { result.StatusCode, result.ReasonPhrase });
        }

        [HttpPost("SaveCustomerMailInv")]
        public IActionResult SaveCustomerMailInv(CustomerMail CustomerMail, IFormFile UploadedFile, bool? IsOrgEmail = null)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            // var file1 = Export(CustomerMail.MailText);
            var filePath = "";

            if (UploadedFile != null)
            {
                System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


                string path = System.IO.Path.Combine("Uploads/", "CustomersMails/");
                string pathW = System.IO.Path.Combine("/Uploads/", "CustomersMails/");

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
                    CustomerMail.FileUrl = pathes;
                }
            }
            //HttpPostedFileBase file = Request.Files["UploadedFile"];
            //string Ran_C = RandomNumber(1, 10000).ToString();

            //if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            //{
            //    if (file.ContentLength > 0)
            //    {
            //        string fileLocation = Server.MapPath("~/Uploads/CustomersMails");
            //        try
            //        {
            //            if (!System.IO.Directory.Exists(fileLocation))
            //            {

            //                Directory.CreateDirectory(fileLocation).Delete();
            //                //System.IO.Directory.CreateDirectory(fileLocation);
            //            }
            //            filePath = Server.MapPath("~/Uploads/CustomersMails/") + Ran_C + file.FileName;
            //            file.SaveAs(filePath);

            //            CustomerMail.FileUrl = "/Uploads/CustomersMails/" + Ran_C + file.FileName;
            //        }
            //        catch (Exception ex)
            //        {
            //            filePath = "";
            //            var massage = "";
            //            if (_globalshared.Lang_G == "rtl")
            //            {
            //                massage = "فشل في رفع المرفقات";
            //            }
            //            else
            //            {
            //                massage = "Failed To Upload Files";
            //            }
            //            return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massage } );
            //        }
            //    }
            //}
            //if (filePath != "")
            //{
            //    var File = filePath;

            //}

            //CustomerMail.FileUrl = "/Uploads/CustomersMails/" + Ran_C + file.FileName;
            //filePath = CustomerMail.FileUrl;
            //CustomerMail.FileUrl = "/"+CustomerMail.FileUrl;
            // filePath = HttpContext.Server.MapPath(@"~\") + CustomerMail.FileUrl;
            filePath = _hostingEnvironment.WebRootPath + CustomerMail.FileUrl;

             var replacementPath = filePath.Replace('/', '\\');


           var result = _CustomerMailService.SaveCustomerMail(CustomerMail,_globalshared.UserId_G, _globalshared.BranchId_G, replacementPath, IsOrgEmail);
            if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.OK)
            {
                result.ReasonPhrase = "send Successfully";
            }
            else if (_globalshared.Lang_G == "ltr" && result.StatusCode == HttpStatusCode.BadRequest)
            {
                result.ReasonPhrase = "Failed to send, check e-mail settings.";
            }
            return Ok(new { result.StatusCode, result.ReasonPhrase });
        }

        //public class ViewAspdf2 : PartialViewAsPdf
        //{
        //    public ViewAspdf2(string Viewname) : base(Viewname) { }
        //    public byte[] GetByte(ControllerContext context)
        //    {
        //        base.PrepareResponse(context.HttpContext.Response);
        //        base.ExecuteResult(context);
        //        return base.CallTheDriver(context);
        //    }

        //}


        //[HttpPost]
        //[ValidateInput(false)]
        //public FileResult Export(string GridHtml)
        //{
        //    using (MemoryStream stream = new System.IO.MemoryStream())
        //    {
        //        StringReader sr = new StringReader(GridHtml);
        //        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
        //        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
        //        pdfDoc.Open();
        //        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
        //        pdfDoc.Close();
        //        return File(stream.ToArray(), "application/pdf", "Grid.pdf");
        //    }
        //}
        //public static void SaveStreamAsFile(string filePath, Stream inputStream, string fileName)
        //{
        //    DirectoryInfo info = new DirectoryInfo(filePath);
        //    if (!info.Exists)
        //    {
        //        info.Create();
        //    }

        //    string path = Path.Combine(filePath, fileName);
        //    using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
        //    {
        //        inputStream.CopyTo(outputFileStream);

        //    }
        //}
        //private void WriteFile(string nid, byte[] contractByte)
        //{
        //    try
        //    {
        //        string savePath = Server.MapPath(@"~/TempFiles/") + nid + "_" + "REDF Contract_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
        //        System.IO.File.WriteAllBytes(savePath, contractByte);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        //public IActionResult SaveCustomerMailInv_2(CustomerMail CustomerMail)
        //{
        //    bool? IsOrgEmail = true;
        //    var filePath = "";
        //    int InvoiceId = 780;
        //    //var file_pdf = PrintPartialViewToPdf(invoiceid, 3);
        //    int TempCheck = 3;
        //    var pdffileName = "PDFFFFF_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".pdf";
        //    string fullPath = Path.Combine(Server.MapPath("~/TempFiles"), pdffileName);
        //    try
        //    { 
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(CustomerMail.MailSubject);
        //    StringReader sr = new StringReader(sb.ToString());

        //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        //    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
        //        pdfDoc.Open();
        //        htmlparser.Parse(sr);
        //        pdfDoc.Close();
        //        byte[] bytes = memoryStream.ToArray();
        //        memoryStream.Close();
        //         WriteFile("inv",bytes);
        //            //SaveStreamAsFile(fullPath, memoryStream, pdffileName);
        //    }
        //    }catch(Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }

        //    /////////////////////////////////////////////////////////////////////////////////////////////////////////
        //    CustomerMail.MailSubject = "gguldg";
        //    //    try
        //    //{
        //    //    if (InvoiceId != 0)
        //    //    {
        //    //        InvoicesVM InvoicesVM = _voucherService.GetVoucherById(InvoiceId);
        //    //        CustomerVM CustomerVM = _customerservice.GetCustomersByCustomerIdInvoice(InvoicesVM.CustomerId ?? 0, _globalshared.Lang_G);

        //    //        var costCenterNam = "";
        //    //        if (InvoicesVM.VoucherDetails[0].CostCenterId != null || InvoicesVM.VoucherDetails[0].CostCenterId != 0)
        //    //        {
        //    //            try
        //    //            {
        //    //                costCenterNam = _CostCenterservice.GetCostCenterById(InvoicesVM.VoucherDetails[0].CostCenterId ?? 0).NameAr;
        //    //            }
        //    //            catch (Exception)
        //    //            {
        //    //                costCenterNam = "بدون";
        //    //            }
        //    //        }
        //    //        else
        //    //        {
        //    //            costCenterNam = "بدون";
        //    //        }
        //    //        //string ValueNumString2 = ConvertNumToString(InvoicesVM.TotalValue.ToString());
        //    //        string ValueNumString = ConvertToWord_NEW(InvoicesVM.TotalValue.ToString());

        //    //        List<VoucherDetailsVM> VoucherDetailsVM = _voucherService.GetAllDetailsByInvoiceId(InvoiceId).ToList();
        //    //        int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G);
        //    //        var objOrganization = _organizationsservice.GetBranchOrganizationDataInvoice(orgId);
        //    //        if (objOrganization != null)
        //    //            ViewData["Org_VD"] = objOrganization;
        //    //        else
        //    //            ViewData["Org_VD"] = null;


        //    //        var objBranch = _branchesService.GetBranchBy_globalshared.BranchId_G(_globalshared.Lang_G, _globalshared.BranchId_G).FirstOrDefault();
        //    //        var OrgIsRequired = _systemSettingsService.GetSystemSettingsBy_globalshared.BranchId_G(_globalshared.BranchId_G).OrgDataIsRequired;

        //    //        ViewData["Branch_VD"] = objBranch;
        //    //        ViewData["CustomerVM_VD"] = CustomerVM;
        //    //        ViewData["InvoicesVM_VD"] = InvoicesVM;
        //    //        ViewData["costCenterNam_VD"] = costCenterNam;
        //    //        ViewData["VoucherDetailsVM_VD"] = VoucherDetailsVM;
        //    //        ViewData["ValueNumString_VD"] = ValueNumString;
        //    //        ViewData["OrgIsRequired_VD"] = OrgIsRequired;
        //    //        ViewData["TempCheck"] = TempCheck;

        //    //        string Time_V = DateTime.Now.ToString("hh:mm:ss", CultureInfo.CreateSpecificCulture("en"));
        //    //        string FullDate = InvoicesVM.Date + " " + Time_V;
        //    //        //DateTime Date = InvoicesVM.AddDate ?? DateTime.Now;
        //    //        //string ActionDate = Date.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.CreateSpecificCulture("en"));


        //    //        var SupplierName = objOrganization.NameAr.TrimStart();
        //    //        SupplierName = SupplierName.TrimEnd();
        //    //        string qrBarcodeHash = QRCodeEncoder.encode(
        //    //            new Seller_Inv(SupplierName),
        //    //            new TaxNumber_Inv(objOrganization.TaxCode),
        //    //            new InvoiceDate_Inv(FullDate),
        //    //            new TotalAmount_Inv(InvoicesVM.TotalValue.ToString()),
        //    //            new TaxAmount_Inv(InvoicesVM.TaxAmount.ToString())
        //    //        );
        //    //        try
        //    //        {
        //    //            string ImgReturn = "";
        //    //            QRCodeGenerator qrGenerator = new QRCodeGenerator();
        //    //            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrBarcodeHash, QRCodeGenerator.ECCLevel.Q, true);
        //    //            QRCode qrCode = new QRCode(qrCodeData);
        //    //            using (Bitmap bitMap = qrCode.GetGraphic(20))
        //    //            {
        //    //                using (MemoryStream ms = new MemoryStream())
        //    //                {
        //    //                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        //    //                    byte[] byteImage = ms.ToArray();
        //    //                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
        //    //                    ImgReturn = "data:image/png;base64," + Convert.ToBase64String(byteImage);
        //    //                }
        //    //            }
        //    //            ViewData["QRCodeString_VD"] = ImgReturn;

        //    //        }
        //    //        catch (Exception ex)
        //    //        {
        //    //            ViewData["QRCodeString_VD"] = null;
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        ViewData["Org_VD"] = null;
        //    //        ViewData["CustomerVM_VD"] = null;
        //    //        ViewData["InvoicesVM_VD"] = null;
        //    //        ViewData["costCenterNam_VD"] = null;
        //    //        ViewData["VoucherDetailsVM_VD"] = null;
        //    //        ViewData["ValueNumString_VD"] = null;
        //    //        ViewData["QRCodeString_VD"] = null;
        //    //        ViewData["Branch_VD"] = null;
        //    //    }

        //    //    //if (TempCheck == 1)
        //    //    //    return PartialView("_InvoicePrintPDF");
        //    //    //else if (TempCheck == 3) //Another Invoice Form No.2
        //    //    //    return PartialView("_Invoice_N_Report");
        //    //    //else
        //    //    //    return PartialView("_InvoicePrintPDF");

        //    //    string dir = "~/Views/Shared/";
        //    //    string ext = ".cshtml";

        //    //    //add code to save the pdf to the temp folder and return the filename to client
        //    //    var fileName = "PDFFFFF_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".pdf";
        //    //    string fullPath = Path.Combine(Server.MapPath("~/TempFiles"), fileName);

        //    //    var filepdf = new Rotativa.PartialViewAsPdf(dir + "_Invoice_N_Report" + ext);
        //    //    var byteArray = filepdf.BuildPdf(ControllerContext);
        //    //    var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
        //    //    fileStream.Write(byteArray, 0, byteArray.Length);
        //    //    fileStream.Close();
        //    //    return Ok(new { fileName = fileName });


        //    //    //var report = new ViewAspdf2("_Invoice_N_Report");
        //    //    //var rep = new Rotativa.PartialViewAsPdf("_Invoice_N_Report");

        //    //    //byte[] pdfbytearray = rep.BuildFile(ControllerContext);
        //    //    //MemoryStream filpdf = new MemoryStream(pdfbytearray);
        //    //    //filpdf.Seek(0, SeekOrigin.Begin);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Console.WriteLine(ex.Message);
        //    //}

        //    //////////////////////////////////////////////////////////////////////////////////////////////////////////



        //    HttpPostedFileBase file = Request.Files["UploadedFile"];
        //    string Ran_C = RandomNumber(1, 10000).ToString();

        //    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
        //    {
        //        if (file.ContentLength > 0)
        //        {
        //            string fileLocation = Server.MapPath("~/Uploads/CustomersMails");
        //            try
        //            {
        //                if (!System.IO.Directory.Exists(fileLocation))
        //                {

        //                    Directory.CreateDirectory(fileLocation).Delete();
        //                    //System.IO.Directory.CreateDirectory(fileLocation);
        //                }
        //                filePath = Server.MapPath("~/Uploads/CustomersMails/") + Ran_C + file.FileName;
        //                file.SaveAs(filePath);

        //                CustomerMail.FileUrl = "/Uploads/CustomersMails/" + Ran_C + file.FileName;
        //            }
        //            catch (Exception ex)
        //            {
        //                filePath = "";
        //                var massage = "";
        //                if (_globalshared.Lang_G == "rtl")
        //                {
        //                    massage = "فشل في رفع المرفقات";
        //                }
        //                else
        //                {
        //                    massage = "Failed To Upload Files";
        //                }
        //                return Ok(new GeneralMessage { Result = false, Message = massage } );
        //            }
        //        }
        //    }
        //    if (filePath != "")
        //    {
        //        var File = filePath;

        //    }

        //    //CustomerMail.FileUrl = "/Uploads/CustomersMails/" + Ran_C + file.FileName;
        //    //filePath = CustomerMail.FileUrl;
        //    //CustomerMail.FileUrl = "/"+CustomerMail.FileUrl;
        //    filePath = HttpContext.Server.MapPath(@"~\") + CustomerMail.FileUrl;
        //    var replacementPath = filePath.Replace('/', '\\');

        //    var url2 = Server.MapPath("~/Email/MailStamp.html");
        //    //var file2 = Server.MapPath("~/dist/assets/images/logo.png");
        //    var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G);
        //    var file2 = Server.MapPath("~") + org.LogoUrl;

        //    var result = _CustomerMailService.SaveCustomerMail_2(CustomerMail,_globalshared.UserId_G, _globalshared.BranchId_G, replacementPath, url2, file2, IsOrgEmail);
        //    if (_globalshared.Lang_G == "ltr" && result.Result == true)
        //    {
        //        result.Message = "send Successfully";
        //    }
        //    else if (_globalshared.Lang_G == "ltr" && result.Result == false)
        //    {
        //        result.Message = "Failed to send, check e-mail settings.";
        //    }
        //    return Ok(new { result.Result, result.Message } );
        //}
        //[HttpPost]
        //[ValidateInput(false)]
        //public FileResult Export(string GridHtml)
        //{
        //    using (MemoryStream stream = new System.IO.MemoryStream())
        //    {
        //        StringReader sr = new StringReader(GridHtml);
        //        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
        //        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
        //        pdfDoc.Open();
        //        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
        //        pdfDoc.Close();
        //        return File(stream.ToArray(), "application/pdf", "Grid.pdf");
        //    }
        //}

        ////public FileResult GetHTMLPageAsPDF(string sb)
        ////{
        ////    StringReader sr = new StringReader(sb.ToString());
        ////    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        ////    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        ////    using (MemoryStream memoryStream = new MemoryStream())
        ////    {
        ////        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
        ////        pdfDoc.Open();

        ////        htmlparser.Parse(sr);
        ////        pdfDoc.Close();

        ////        byte[] bytes = memoryStream.ToArray();
        ////        memoryStream.Close();
        ////    }
        ////}

        //public IActionResult PrintPartialViewToPdf(int? InvoiceId, int? TempCheck)
        //{


        //    if (InvoiceId.HasValue)
        //    {
        //        InvoicesVM InvoicesVM = _voucherService.GetVoucherById(InvoiceId ?? 0);
        //        CustomerVM CustomerVM = _customerservice.GetCustomersByCustomerIdInvoice(InvoicesVM.CustomerId ?? 0, _globalshared.Lang_G);

        //        var costCenterNam = "";
        //        if (InvoicesVM.VoucherDetails[0].CostCenterId != null || InvoicesVM.VoucherDetails[0].CostCenterId != 0)
        //        {
        //            try
        //            {
        //                costCenterNam = _CostCenterservice.GetCostCenterById(InvoicesVM.VoucherDetails[0].CostCenterId ?? 0).NameAr;
        //            }
        //            catch (Exception)
        //            {
        //                costCenterNam = "بدون";
        //            }
        //        }
        //        else
        //        {
        //            costCenterNam = "بدون";
        //        }
        //        //string ValueNumString2 = ConvertNumToString(InvoicesVM.TotalValue.ToString());
        //        string ValueNumString = ConvertToWord_NEW(InvoicesVM.TotalValue.ToString());

        //        List<VoucherDetailsVM> VoucherDetailsVM = _voucherService.GetAllDetailsByInvoiceId(InvoiceId).ToList();
        //        int orgId = _BranchesService.GetOrganizationId(_globalshared.BranchId_G);
        //        var objOrganization = _organizationsservice.GetBranchOrganizationDataInvoice(orgId);
        //        if (objOrganization != null)
        //            ViewData["Org_VD"] = objOrganization;
        //        else
        //            ViewData["Org_VD"] = null;


        //        var objBranch = _branchesService.GetBranchBy_globalshared.BranchId_G(_globalshared.Lang_G, _globalshared.BranchId_G).FirstOrDefault();
        //        var OrgIsRequired = _systemSettingsService.GetSystemSettingsBy_globalshared.BranchId_G(_globalshared.BranchId_G).OrgDataIsRequired;

        //        ViewData["Branch_VD"] = objBranch;
        //        ViewData["CustomerVM_VD"] = CustomerVM;
        //        ViewData["InvoicesVM_VD"] = InvoicesVM;
        //        ViewData["costCenterNam_VD"] = costCenterNam;
        //        ViewData["VoucherDetailsVM_VD"] = VoucherDetailsVM;
        //        ViewData["ValueNumString_VD"] = ValueNumString;
        //        ViewData["OrgIsRequired_VD"] = OrgIsRequired;
        //        ViewData["TempCheck"] = TempCheck;

        //        string Time_V = DateTime.Now.ToString("hh:mm:ss", CultureInfo.CreateSpecificCulture("en"));
        //        string FullDate = InvoicesVM.Date + " " + Time_V;
        //        //DateTime Date = InvoicesVM.AddDate ?? DateTime.Now;
        //        //string ActionDate = Date.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.CreateSpecificCulture("en"));


        //        var SupplierName = objOrganization.NameAr.TrimStart();
        //        SupplierName = SupplierName.TrimEnd();
        //        string qrBarcodeHash = QRCodeEncoder.encode(
        //            new Seller_Inv(SupplierName),
        //            new TaxNumber_Inv(objOrganization.TaxCode),
        //            new InvoiceDate_Inv(FullDate),
        //            new TotalAmount_Inv(InvoicesVM.TotalValue.ToString()),
        //            new TaxAmount_Inv(InvoicesVM.TaxAmount.ToString())
        //        );
        //        try
        //        {
        //            string ImgReturn = "";
        //            QRCodeGenerator qrGenerator = new QRCodeGenerator();
        //            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrBarcodeHash, QRCodeGenerator.ECCLevel.Q, true);
        //            QRCode qrCode = new QRCode(qrCodeData);
        //            using (Bitmap bitMap = qrCode.GetGraphic(20))
        //            {
        //                using (MemoryStream ms = new MemoryStream())
        //                {
        //                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        //                    byte[] byteImage = ms.ToArray();
        //                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
        //                    ImgReturn = "data:image/png;base64," + Convert.ToBase64String(byteImage);
        //                }
        //            }
        //            ViewData["QRCodeString_VD"] = ImgReturn;

        //        }
        //        catch (Exception ex)
        //        {
        //            ViewData["QRCodeString_VD"] = null;
        //        }
        //    }
        //    else
        //    {
        //        ViewData["Org_VD"] = null;
        //        ViewData["CustomerVM_VD"] = null;
        //        ViewData["InvoicesVM_VD"] = null;
        //        ViewData["costCenterNam_VD"] = null;
        //        ViewData["VoucherDetailsVM_VD"] = null;
        //        ViewData["ValueNumString_VD"] = null;
        //        ViewData["QRCodeString_VD"] = null;
        //        ViewData["Branch_VD"] = null;
        //    }

        //    if (TempCheck == 1)
        //        return PartialView("_InvoicePrintPDF");
        //    else if (TempCheck == 3) //Another Invoice Form No.2
        //        return PartialView("_Invoice_N_Report");
        //    else
        //        return PartialView("_InvoicePrintPDF");


        //    //var report = new ViewAspdf2("~/Views/Shared/_Invoice_N_Report");
        //    //byte[] pdfbytearray = report.GetByte(ControllerContext);
        //    //MemoryStream file = new MemoryStream(pdfbytearray);
        //    //file.Seek(0, SeekOrigin.Begin);
        //    //    return report;

        //}
        [HttpGet("ConvertToWord_NEW")]
        public static string ConvertToWord_NEW(string Num)
            {
                CurrencyInfo _currencyInfo = new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia);
                ToWord toWord = new ToWord(Convert.ToDecimal(Num), _currencyInfo);
                return toWord.ConvertToArabic();
            }

        [HttpGet("GetAllCustomerMails")]
        public IActionResult GetAllCustomerMails()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
                var someCustomer = _CustomerMailService.GetAllCustomerMails(0).Result;
                foreach (var userBranch in userBranchs)
                {
                    var AllCustomers = _CustomerMailService.GetAllCustomerMails(userBranch.BranchId).Result;
                    var Customers = someCustomer.Union(AllCustomers);
                    someCustomer = Customers;
                }
                return Ok(someCustomer );
            }

        [HttpGet("GetMailsByCustomerId")]
        public IActionResult GetMailsByCustomerId(int? CustomerId)
            {
                return Ok(_CustomerMailService.GetMailsByCustomerId(CustomerId) );
            }


        [HttpPost("DeleteCustomerMail")]
        public IActionResult DeleteCustomerMail(int MailId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _CustomerMailService.DeleteCustomerMail(MailId, _globalshared.UserId_G, _globalshared.BranchId_G);
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
        ///////////////////////////////cistomersms////////////////////
        ///       
        [HttpPost("SaveCustomerSMS")]
        public IActionResult SaveCustomerSMS(CustomerSMS CustomerSMS)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            GeneralMessage result = null;

                result = _CustomerSMSService.SaveCustomerSMS(CustomerSMS, _globalshared.UserId_G, _globalshared.BranchId_G);

                //if (CustomerSMS.AssignedCustomersSMSIds.Count() > 0)
                //{
                //    foreach (var CustomersmsId in CustomerSMS.AssignedCustomersSMSIds)
                //    {
                //        var smsSettings = _sMSSettingsService.GetsmsSetting(_globalshared.BranchId_G);

                //        SMSProviders Provider = new SMSProviders();
                //        string apiLink = smsSettings.ApiUrl;
                //        string apiKey = smsSettings.Password; 
                //        string numbers = _customerservice.GetCustomersByCustId(CustomersmsId).FirstOrDefault().CustomerMobile; // in a comma seperated list
                //        string message = CustomerSMS.SMSText; 
                //        string sender = smsSettings.SenderName; 
                //        string userName = smsSettings.UserName;

                //        if (smsSettings.ApiUrl.Contains("msegat"))
                //        {
                //            var baseAddress = new Uri("https://www.msegat.com");

                //            if (numbers.Substring(0, 1) == "0")
                //                numbers = numbers.Substring(1, numbers.Length - 1);

                //            //var httpClient = new HttpClient { BaseAddress = baseAddress };

                //            string contectStr = @"{" +
                //                  "\"userName\": \"" + userName + "\"," +
                //                  "\"numbers\": \"966" + numbers + "\"," +
                //                  "\"userSender\": \"" + sender + "\"," +
                //                  "\"apiKey\": \"" + apiKey + "\"," +
                //                  "\"msg\": \"" + message + "\"," +
                //                  "\"msgEncoding\": \"UTF8\"" +
                //                "}";
                //            var content = new StringContent(contectStr); //, System.Text.Encoding.Default, "application/json");

                //            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                //            var response = httpClient.PostAsync("/gw/sendsms.php", content).Result;

                //            string responseHeaders = response.Headers.ToString();
                //            string responseData = response.Content.ReadAsStringAsync().Result;

                //            string[] resData = responseData.Split('"');

                //            if ((int)response.StatusCode == 200 && resData[3] == "1")
                //            {
                //                //insert sms to our system
                //                result = _CustomerSMSService.SaveCustomerSMS(CustomerSMS,_globalshared.UserId_G, _globalshared.BranchId_G);
                //            }
                //            else
                //                result = new GeneralMessage() { Result = false, Message = resData[7] };
                //        }
                //        else
                //        {
                //            result = _CustomerSMSService.SaveCustomerSMS(CustomerSMS,_globalshared.UserId_G, _globalshared.BranchId_G, new HttpClient());
                //        }
                //    }
                //}


                //if (result.Result == false)
                //{
                //    result.Message = "Failed to send, check your text message service settings.";
                //}

                return Ok(result );
            }




        [HttpGet("SaveCustomerSMS2")]
        public IActionResult SaveCustomerSMS2(CustomerSMS CustomerSMS)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            GeneralMessage result = null;

                result = _CustomerSMSService.SaveCustomerSMS2(CustomerSMS, _globalshared.UserId_G, _globalshared.BranchId_G);

                return Ok(result);
            }


        [HttpGet("GetAllCustomerSMS")]
        public IActionResult GetAllCustomerSMS()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
                var someCustomer = _CustomerSMSService.GetAllCustomerSMS(0).Result;
                foreach (var userBranch in userBranchs)
                {
                    var AllCustomers = _CustomerSMSService.GetAllCustomerSMS(userBranch.BranchId).Result;
                    var Customers = someCustomer.Union(AllCustomers);
                    someCustomer = Customers;
                }
                return Ok(someCustomer );
            }

        [HttpGet("GetSMSByCustomerId")]
        public IActionResult GetSMSByCustomerId(int? CustomerId)
            {

            return Ok(_CustomerSMSService.GetSMSByCustomerId(CustomerId) );
            }

        [HttpPost("DeleteCustomerSMS")]

        public IActionResult DeleteCustomerSMS(int SMSId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _CustomerSMSService.DeleteCustomerSMS(SMSId, _globalshared.UserId_G, _globalshared.BranchId_G);
            
                return Ok(result);
            }

        [HttpGet("GetAllCustHaveRemainingMoney")]

        public IActionResult GetAllCustHaveRemainingMoney(CustomerVM CustomersSearch)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
                var someCustomer = _customerservice.GetAllCustHaveRemainingMoney(CustomersSearch, _globalshared.Lang_G, 0).Result;
                foreach (var userBranch in userBranchs)
                {
                    var AllCustomers = _customerservice.GetAllCustHaveRemainingMoney(CustomersSearch, _globalshared.Lang_G, userBranch.BranchId).Result;
                    var Customers = someCustomer.Union(AllCustomers);
                    someCustomer = Customers;
                }
                return Ok(someCustomer );
            }


        [HttpGet("GetCustomersByCustomerId")]
        public IActionResult GetCustomersByCustomerId(int? CustomerId)
            {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _customerservice.GetCustomersByCustomerId(CustomerId, _globalshared.Lang_G);
                return Ok(result );
            }
        [HttpGet("GetCustomersByProjectId")]
        public IActionResult GetCustomersByProjectId(int? ProjectId)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _customerservice.GetCustomersByProjectId(ProjectId, _globalshared.Lang_G);
            return Ok(result);
        }

        [HttpGet("GetCustomersByAccountId")]
        public IActionResult GetCustomersByAccountId(int? AccountId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _customerservice.GetCustomersByAccountId(AccountId, _globalshared.Lang_G);
                return Ok(result );
            }


        [HttpGet("GetCustomersByNationalId")]
        public IActionResult GetCustomersByNationalId(string NationalId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var customer = _customerservice.GetCustomersByNationalId(NationalId, _globalshared.Lang_G).Result;
                if (customer != null)
                {
                    return Ok(new { msg = customer.CustomerNameAr } );
                }
                else
                {
                    return Ok(new { msg = "" } );
                }
            }

        [HttpGet("GetCustomersByCommercialRegister")]
        public IActionResult GetCustomersByCommercialRegister(string CommercialRegister)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var customer = _customerservice.GetCustomersByCommercialRegister(CommercialRegister, _globalshared.Lang_G).Result;
                if (customer != null)
                {
                    return Ok(new { msg = customer.CustomerNameAr } );
                }
                else
                {
                    return Ok(new { msg = "" } );
                }
            }


        [HttpGet("FillAllCustomerSelect")]
        public IActionResult FillAllCustomerSelect()
            {
                return Ok(_customerservice.FillAllCustomerSelect() );
            }

        [HttpGet("FillAllCustomerSelectWithBranch")]
        public IActionResult FillAllCustomerSelectWithBranch()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            //var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G,_globalshared.UserId_G).Where(s => s._globalshared.BranchId_G == 1);
            //if (userBranchs.Count() > 0)
            //{
            //    return Ok(_customerservice.FillAllCustomerSelect() );
            //}
            //else
            //{
            //    return Ok(_customerservice.FillAllCustomerSelectWithBranch("", _globalshared.BranchId_G) );
            //}
            return Ok(_customerservice.FillAllCustomerSelectWithBranch("", _globalshared.BranchId_G) );


            }

        [HttpGet("FillAllCustomerSelectNotHaveProj")]
        public IActionResult FillAllCustomerSelectNotHaveProj()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_customerservice.FillAllCustomerSelectNotHaveProj(_globalshared.Lang_G, _globalshared.BranchId_G) );
            }

        [HttpGet("FillAllCustomerSelectNotHaveProjWithBranch")]
        public IActionResult FillAllCustomerSelectNotHaveProjWithBranch()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            //var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G,_globalshared.UserId_G).Where(s => s._globalshared.BranchId_G == 1);
            //if (userBranchs.Count() > 0)
            //{
            //    return Ok(_customerservice.FillAllCustomerSelectNotHaveProj(_globalshared.Lang_G, _globalshared.BranchId_G) );
            //}
            //else
            //{
            //    return Ok(_customerservice.FillAllCustomerSelectNotHaveProjWithBranch(_globalshared.Lang_G, _globalshared.BranchId_G) );
            //}
            return Ok(_customerservice.FillAllCustomerSelectNotHaveProjWithBranch(_globalshared.Lang_G, _globalshared.BranchId_G) );

            }


        [HttpGet("FillAllCustomerSelectNotHaveProjWithout")]
        public IActionResult FillAllCustomerSelectNotHaveProjWithout()
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_customerservice.FillAllCustomerSelectNotHaveProjWithout(_globalshared.Lang_G, _globalshared.BranchId_G) );
            }




        [HttpGet("GetCustomersNewTask")]
        public IActionResult GetCustomersNewTask()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var someCustomer = _customerservice.GetCustomersNewTask(_globalshared.Lang_G, _globalshared.BranchId_G, _globalshared.UserId_G).Result.Select(s => new
            {
                Id = s.CustomerId,
                Name = s.CustomerName
            });





            return Ok(someCustomer);
        }


        //public IActionResult PrintCustomerReport(int CustomerCase, string FromDate, string ToDate)
        //{
        //    int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G);

        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);
        //    string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

        //    List<CustomerVM> Customers = null;
        //    string title = "";
        //    switch (CustomerCase)
        //    {
        //        case 1: //Citizens
        //            {
        //                Customers = GetAllCustomersByCustomerTypeId_List(1);
        //                title = Resources.CustomerReport + " - " + Resources.Pro_Citizens;
        //                break;
        //            }
        //        case 2: //Inverstors
        //            {
        //                Customers = GetAllCustomersByCustomerTypeId_List(2);
        //                title = Resources.CustomerReport + " - " + Resources.General_Institutionsandcompanies;
        //                break;
        //            }
        //        case 3:
        //            { //Government
        //                Customers = GetAllCustomersByCustomerTypeId_List(3);
        //                title = Resources.CustomerReport + " - " + Resources.General_Governmentalentities;
        //                break;
        //            }
        //        case 4:
        //            { //AllCustomers

        //                Customers = GatAllCustomers_List();
        //                title = Resources.AllCustomerReport;
        //                break;
        //            }
        //        default:
        //            break;
        //    }
        //    Customers = Customers.ToList().Where(s => s._globalshared.BranchId_G == _globalshared.BranchId_G).ToList();

        //    if (!(FromDate == "" || ToDate == ""))
        //    {
        //        try
        //        {
        //            Customers = Customers.ToList().Where(s => s.AddDate >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) && s.AddDate <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).ToList();
        //        }
        //        catch (Exception)
        //        {
        //        }
        //    }
        //    ReportPDF = humanResourcesReports.PrintCustomersReport(Customers, infoDoneTasksReport, title, FromDate, ToDate);
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
        [HttpPost("GenerateRandomNo")]
            public int GenerateRandomNo()
            {
                int _min = 1000;
                int _max = 9999;
                Random _rdm = new Random();
                return _rdm.Next(_min, _max);
            }


        }
    
}

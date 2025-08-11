using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Service.Interfaces;


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


            var org = _organizationsservice.GetOrganizationDataLogin(_globalshared.Lang_G).Result;
            string file2 = System.IO.Path.Combine(org.LogoUrl);

            var result = _customerservice.SaveCustomer(customer, _globalshared.UserId_G, _globalshared.BranchId_G, url2, file2);
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
            var result = _organizationsservice.FillSelect_Cust(Con, _globalshared.UserId_G, 1, null, 0, _globalshared.BranchId_G).Result;
            return Ok(result);

        }

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

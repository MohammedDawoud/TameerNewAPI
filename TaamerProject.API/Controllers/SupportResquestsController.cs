using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net.Http.Headers;
using TaamerProject.API.Helper;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using System.Net;
using System.Net.Mail;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class SupportResquestsController : ControllerBase
    {
        private ISupportRequestsService _supportRequestsService;
        private readonly IVersionService _Versionservice;
        private readonly IOrganizationsService _orgService;
        public GlobalShared _globalshared;
        private readonly IUsersService _usersService;
        private IConfiguration Configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IBranchesService _branchesService;
        private readonly ILicencesService _licencesService;

        string ApiUrl;
        public SupportResquestsController(ISupportRequestsService supportRequestsService,
            IVersionService versionservice, IOrganizationsService orgService, IUsersService usersService, IConfiguration _configuration, IWebHostEnvironment webHostEnvironment
            , IBranchesService branchesService, ILicencesService licencesService)
        {
            _supportRequestsService = supportRequestsService;
            _Versionservice = versionservice;
            _orgService = orgService;
            Configuration = _configuration; ApiUrl = this.Configuration.GetConnectionString("APIURL");
            HttpContext httpContext = HttpContext;

            _globalshared = new GlobalShared(httpContext);
            _hostingEnvironment = webHostEnvironment;
            _usersService = usersService;
            _branchesService = branchesService;
            _licencesService = licencesService;
        }
        private readonly Random _random = new Random();
        [HttpPost("RandomNumber")]
        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
        [HttpPost("SaveSupportResquests")]
        public async Task<IActionResult> SaveSupportResquestsAsync([FromForm]SupportResquests supportResquests, List<IFormFile> postedFiles)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            //string path = Path.Combine("/Uploads/Organizations/pictures/");
            string path = System.IO.Path.Combine("Uploads/", "Organizations/pictures/");
            string pathW = System.IO.Path.Combine("/Uploads/", "Organizations/pictures/");
            string fileName = ""; string fname = ""; string fnamepath = "";

            foreach (IFormFile postedFile in postedFiles)
            {
                fname = postedFile.FileName;
                fileName = System.IO.Path.GetFileName(RandomNumber(1, 10000) + fname);
                fnamepath = Path.Combine(path, fileName);

                try
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(fnamepath, System.IO.FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                        string atturl = Path.Combine(path, fname);
                        supportResquests.AttachmentUrl = "/Uploads/Organizations/pictures/" + fileName;
                    }
                }
                catch (Exception)
                {
                    var massege = "فشل في رفع الملفات";
                    return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massege });
                }
            }


            var version = _Versionservice.GetVersion().Result;
            
            var result = _supportRequestsService.SaveSupportResquests(supportResquests, _globalshared.UserId_G, _globalshared.BranchId_G, version.VersionCode??"", fnamepath);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    try
                    {
                         var uri =  "https://api2.tameercloud.com/"; //"https://localhost:44334/";// "http://164.68.110.173:8080/";
                         // var uri = "https://localhost:44334/";// "http://164.68.110.173:8080/";

                        if (uri != null && uri != "")
                        {
                            //Generate Token
                            var token = getapitoken(uri);

                            var serviceobj = ServiceRequests(supportResquests, result.ReturnedStr??"");
                            var org = _orgService.GetBranchOrganization();
                            var user = _usersService.GetUserById(_globalshared.UserId_G,_globalshared.Lang_G);
                            var branch = _branchesService.GetBranchById(_globalshared.BranchId_G);
                            var formData = new MultipartFormDataContent();

                            // Add the fields to the form data
                            formData.Add(new StringContent(serviceobj.ServiesName ?? ""), "ServiesName");
                            formData.Add(new StringContent(serviceobj.visitDate ?? ""), "visitDate");
                            formData.Add(new StringContent(serviceobj.VisitTime ?? ""), "VisitTime");
                            formData.Add(new StringContent((serviceobj.ServiceType??0).ToString()), "ServiceType");
                            formData.Add(new StringContent((serviceobj.Priority??0).ToString()), "Priority");
                            formData.Add(new StringContent(serviceobj.RequeterMobileNumber ?? ""), "RequeterMobileNumber");
                            formData.Add(new StringContent((serviceobj.FromApp??0).ToString()), "FromApp");
                            formData.Add(new StringContent((org.Result.NameAr??"").ToString()), "ExternalOrgName");
                            formData.Add(new StringContent((user.Result.FullNameAr ==null || user.Result.FullNameAr=="" ? user.Result.FullName ?? "" : user.Result.FullNameAr ).ToString()), "ExternalCustomerName");
                            formData.Add(new StringContent((org.Result.Mobile ?? "").ToString()), "ExternalMobNumber");
                            formData.Add(new StringContent((supportResquests.Topic ?? "").ToString()), "Note");
                            formData.Add(new StringContent((org.Result.TameerAPIURL ?? "").ToString()), "CustomerULR");
                            formData.Add(new StringContent((result.ReturnedParm.ToString() ?? "").ToString()), "TameerServiceRequestId");

                            formData.Add(new StringContent((org.Result.ComDomainLink ?? "").ToString()), "DomainLink");
                            formData.Add(new StringContent((org.Result.ComDomainAddress ?? "").ToString()), "IPAddress");
                            formData.Add(new StringContent((user.Result.UserName ?? "").ToString()), "UserName");
                            formData.Add(new StringContent((branch.Result.NameAr ?? "").ToString()), "BranchName");
                            formData.Add(new StringContent((user.Result.Email ?? "").ToString()), "ExternalCustomerMail");

                            if (fnamepath != null && fnamepath != "")
                            {

                                /////////////////////////////////////////////////////////
                                //byte[] fileBytes;
                                //using (var stream = new FileStream(fnamepath, FileMode.Open))
                                //{
                                //    using (var memoryStream = new MemoryStream())
                                //    {
                                //        stream.CopyToAsync(memoryStream);
                                //        fileBytes = memoryStream.ToArray();
                                //    }
                                //}

                                // Create a ByteArrayContent object for the file data
                                //var fileContent = new ByteArrayContent(fileBytes);
                                //fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                                //string fileExtension = Path.GetExtension(fname);


                                // Read the file content
                                byte[] fileContent = await System.IO.File.ReadAllBytesAsync(fnamepath);

                                // Create HTTP client
                                using HttpClient client = new HttpClient();

                                // Create a MultipartFormDataContent
                                //var multipartContent = new MultipartFormDataContent();
                                var fileContentStream = new ByteArrayContent(fileContent);
                                formData.Add(fileContentStream, "file", Path.GetFileName(fnamepath));

                                // Add the file to the form data
                                //formData.Add(fileContent, "File", "file" + fileExtension);
                                /////////////////////////////////////////////////////////
                            }

                            using (var client = new HttpClient())
                            {
                                //Base API URI
                                client.BaseAddress = new Uri(uri);
                                //JWT TOKEN
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                client.DefaultRequestHeaders
                                .Accept
                                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                //HTTP POST API
                                //var responseTask = client.PostAsync("api/ServiceRequest/SaveServiceRequest", null);

                                var res = await client.PostAsync("api/ServiceRequest/SaveServiceRequestWithFileApplication", formData);
                                res.EnsureSuccessStatusCode();  // This will throw an exception for non-success status codes

                                string responseBody = await res.Content.ReadAsStringAsync();
                                dynamic data = JsonConvert.DeserializeObject(responseBody);

                                // Access the 'token' property

                                //res.Wait();
                                Console.WriteLine(data);
                                if (data != null && data.statusCode == HttpStatusCode.OK)
                                {
                                    string ticketno = data.reasonPhrase??"";
                                    _supportRequestsService.UpdateSupportResquestsNo(result.ReturnedParm.Value, ticketno, _globalshared.UserId_G, _globalshared.BranchId_G);
                                    string stat = "";
                                    supportResquests.TicketNo = ticketno;
                                    
                              
                                    _supportRequestsService.SendMail(supportResquests, _globalshared.BranchId_G, _globalshared.UserId_G, version.VersionCode, org.Result.NameAr, fnamepath, user.Result.Email);
                                    // _supportRequestsService.AutomationMail(supportResquests, _globalshared.BranchId_G);

                                }
                                else
                                {
                                    if (result.ReturnedParm.Value != null && result.ReturnedParm.Value != 0)
                                    {
                                       var delete= _supportRequestsService.Deleterequest(result.ReturnedParm.Value, _globalshared.UserId_G, _globalshared.BranchId_G);
                                        return BadRequest(delete);
                                    }
                                   
                                }

                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        if (result.ReturnedParm.Value != null && result.ReturnedParm.Value != 0)
                        {
                            var delete = _supportRequestsService.Deleterequest(result.ReturnedParm.Value, _globalshared.UserId_G, _globalshared.BranchId_G);
                            return BadRequest(delete);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (result.ReturnedParm.Value != null && result.ReturnedParm.Value != 0)
                    {
                        var delete = _supportRequestsService.Deleterequest(result.ReturnedParm.Value, _globalshared.UserId_G, _globalshared.BranchId_G);
                        return BadRequest(delete);
                    }
                }

            }
            return Ok(result);
        }
        [HttpPost("GetAllSupportResquests")]
        public IActionResult GetAllSupportResquests()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Result = _supportRequestsService.GetAllSupportResquests(_globalshared.Lang_G, _globalshared.BranchId_G, _globalshared.UserId_G).Result.ToArray();

            for (int j = 0; j < Result.Count(); j++)
            {
                var CorrectDate = (Result[j].Date??DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                Result[j].DateF = CorrectDate;

            }
            return Ok(Result);
        }


        [HttpPost("ServiceRequests")]
        public ServiceRequest ServiceRequests(SupportResquests support, string mobile)
        {
            ServiceRequest sr = new ServiceRequest();
            sr.ServiesName = support.Address??"";
            sr.visitDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            sr.VisitTime = "10:00-12:00";
            sr.ServiceType = 100;
            sr.Priority = getpriority(support.priority ?? "");
            sr.RequeterMobileNumber = mobile;
            sr.FromApp = 1;
            return sr;
        }

        [HttpPost("SaveLabaikrequest")]
        public GeneralMessage SaveLabaikrequest(SupportResquests support, string attachment, string mobile)
        {
            try
            {
                //labaik url
                var uri = "https://api2.tameercloud.com/";
                if (uri != null && uri != "")
                {
                    //Generate Token
                    var token = getapitoken(uri);

                    var serviceobj = ServiceRequests(support, mobile);

                    var formData = new MultipartFormDataContent();

                    // Add the fields to the form data
                    formData.Add(new StringContent(serviceobj.ServiesName ?? ""), "ServiesName");
                    formData.Add(new StringContent(serviceobj.visitDate ?? ""), "visitDate");
                    formData.Add(new StringContent(serviceobj.VisitTime ?? ""), "VisitTime");
                    formData.Add(new StringContent((serviceobj.ServiceType??0).ToString()), "ServiceType");
                    formData.Add(new StringContent((serviceobj.Priority??0).ToString()), "Priority");
                    formData.Add(new StringContent(serviceobj.RequeterMobileNumber ?? ""), "RequeterMobileNumber");
                    formData.Add(new StringContent(serviceobj.RequeterMobileNumber ?? ""), "postedFiles");


                    using (var client = new HttpClient())
                    {
                        //Base API URI
                        client.BaseAddress = new Uri(uri);
                        //JWT TOKEN
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        client.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //HTTP POST API
                        //var responseTask = client.PostAsync("api/ServiceRequest/SaveServiceRequest", null);

                        var res = client.PostAsync("api/ServiceRequest/SaveServiceRequest", formData);
                        res.Wait();
                        Console.WriteLine(res.Result);

                    }

                }
            }
            catch (Exception ex)
            {

            }
            return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم الارسال بنجاح" };
        }

        [HttpPost("getapitoken")]
        public string getapitoken(string URI)
        {
            var result = "";
            try
            {

                using (var client = new HttpClient())
                {
                    //Base URI
                    client.BaseAddress = new Uri(URI);
                    //HTTP GET


                    //Http GET 
                    //Get Token API
                    var responseTask = client.GetAsync("api/Users/gettoken?userid=1");
                    responseTask.Wait();

                    var reslt = responseTask.Result;

                    result = reslt.Content.ReadAsStringAsync().Result;

                }



            }
            catch (Exception ex)
            {

            }
            return result;
        }

       
        [HttpPost("getpriority")]
        public int getpriority(string strpriority)
        {
            int intptiority = 0;
            if (strpriority == "منخفضة")
            {
                intptiority = 1;
            }
            else if (strpriority == "متوسطة")
            {
                intptiority = 2;
            }
            else
            {
                intptiority = 3;
            }
            return intptiority;
        }

        [HttpPost("UpdateSupportResquests")]
        public IActionResult UpdateSupportResquests(int? RequestId,int? Status,string? Replay,string? SenderName,string? UserImg)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Result = _supportRequestsService.UpdateSupportResquests(RequestId.Value, Replay, Status??0, SenderName, UserImg, _globalshared.UserId_G, _globalshared.BranchId_G);

        
            return Ok(Result);
        }


        [HttpPost("UpdateSupportResquestsForm")]
        public IActionResult UpdateSupportResquestsForm(int? RequestId, int? Status, string? Replay, string? SenderName, string? UserImg,string? AttachmentUrl)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Result = _supportRequestsService.UpdateSupportResquests(RequestId.Value, Replay, Status ?? 0, SenderName, UserImg, _globalshared.UserId_G, _globalshared.BranchId_G,AttachmentUrl);


            return Ok(Result);
        }
        [HttpPost("UpdateLabaikTicketStatus")]
        public async Task<IActionResult> UpdateLabaikTicketStatus(string TameerServiceRequestId, int Status)
        {
            try
            {
                 var uri =  "https://api2.tameercloud.com/"; //"https://localhost:44334/";// "http://164.68.110.173:8080/";
                //var uri = "https://localhost:44334/";// "http://164.68.110.173:8080/";

                if (uri != null && uri != "")
                {
                    //Generate Token
                    var token = getapitoken(uri);

                   
                    using (var client = new HttpClient())
                    {
                        //Base API URI
                        client.BaseAddress = new Uri(uri);
                        //JWT TOKEN
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        client.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //HTTP POST API
                        //var responseTask = client.PostAsync("api/ServiceRequest/SaveServiceRequest", null);

                        var res = await client.PostAsync("api/ServiceRequest/UpdateRequestStatusFromTameer?TameerServiceRequestId="+ TameerServiceRequestId + "&&Status="+ Status + "", null);
                      

                    }

                }
                return Ok();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }
        }
        [HttpPost("SaveRequestReplayFromTameer")]
        public IActionResult SaveRequestReplayFromTameer(IFormFile? file ,[FromForm]int? RequestId, [FromForm] string? Replay)
        {

            System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();
            var attachment = "";
            if (file != null)
            {


                string path = System.IO.Path.Combine("Uploads/Ticket/", "Contact/");
                string pathW = System.IO.Path.Combine("/Uploads/Ticket/", "Contact/");

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                List<string> uploadedFiles = new List<string>();
                string pathes = "";
                //foreach (IFormFile postedFile in postedFiles)
                //{
                string fileName = System.IO.Path.GetFileName(Guid.NewGuid() + file.FileName);
                //string fileName = System.IO.Path.GetFileName(postedFiles.FileName);

                var path2 = Path.Combine(path, fileName);
                if (System.IO.File.Exists(path2))
                {
                    System.IO.File.Delete(path2);
                }
                using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(path, fileName), System.IO.FileMode.Create))
                {


                    file.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    // string returnpath = host + path + fileName;
                    //pathes.Add(pathW + fileName);
                    pathes = pathW + fileName;
                }


                if (pathes != null)
                {
                    attachment = pathes;
                }
            }
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Result = _supportRequestsService.SaveRequestReplayFromTameer(RequestId.Value, Replay, _globalshared.UserId_G, _globalshared.BranchId_G, attachment);

            Result.ReturnedStr = attachment.ToString();
            return Ok(Result);
        }


        [HttpPost("GetAllOpenSupportResquests")]
        public IActionResult GetAllOpenSupportResquests()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Result = _supportRequestsService.GetAllOpenSupportResquests(_globalshared.Lang_G, _globalshared.BranchId_G, _globalshared.UserId_G).Result.ToArray();

            for (int j = 0; j < Result.Count(); j++)
            {
                var CorrectDate = (Result[j].Date ?? DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                Result[j].DateF = CorrectDate;

            }
            return Ok(Result);
        }
        [HttpGet("GetAllOpenSupportResquestsWithReplay")]
        public IActionResult GetAllOpenSupportResquestsWithReplay()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Result = _supportRequestsService.GetAllOpenSupportResquestsWithReplay(_globalshared.UserId_G).Result.ToArray();

            for (int j = 0; j < Result.Count(); j++)
            {
                var CorrectDate = (Result[j].Date ?? DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                Result[j].DateF = CorrectDate;

            }
            return Ok(Result);
        }

        [HttpGet("GetAllReplyByServiceId")]
        public IActionResult GetAllReplyByServiceId(int RequestId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Result = _supportRequestsService.GetAllReplyByServiceId(RequestId).Result.ToArray();

            
            return Ok(Result);
        }




        [HttpGet("GetAllOpenSupportResquestsreplayesDashboard")]
        public IActionResult GetAllOpenSupportResquestsreplayesDashboard()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Result = _supportRequestsService.GetAllOpenSupportResquestsreplayesDashboard(_globalshared.UserId_G).Result.ToArray();

            
            return Ok(Result);
        }

        [HttpPost("ReadReplay")]
        public IActionResult ReadReplay(int? SupportReplayId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Result = _supportRequestsService.ReadReplay(SupportReplayId.Value, _globalshared.UserId_G, _globalshared.BranchId_G);


            return Ok(Result);
        }


    }
    public class ServiceRequest
    {
        public int ServiceRequestId { get; set; }
        public string? ServiesName { get; set; }
        public int? ServiceType { get; set; }
        public int? Priority { get; set; }
        public int? RequestStatus { get; set; }
        public string? VisitTime { get; set; }
        public string? visitDate { get; set; }
        public string? RequeterMobileNumber { get; set; }
        public string? RequeterMobileNumber2 { get; set; }
        public string? ServiceCode { get; set; }
        public string? TicketNumber { get; set; }

        public string? Note { get; set; }
        public string? AttachUrl { get; set; }
        public int? FromApp { get; set; }

    }
}

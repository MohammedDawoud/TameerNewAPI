using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Globalization;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class AttendenceDeviceController : ControllerBase
    {
        private IAttendenceDeviceService _attendencedeviceservice;
        private readonly IEmployeesRepository _employeeRepository;
        private string? Con;
        private readonly IAttendenceService _attendenceService;
        private readonly IAttDeviceService _attDeviceService;
        public GlobalShared _globalshared;
        private IConfiguration Configuration;

        public AttendenceDeviceController(IAttendenceDeviceService attendencedeviceservice, IEmployeesRepository employeeRepository
            , IAttendenceService attendenceService, IAttDeviceService attDeviceService, IConfiguration _configuration)
        {
            _attendencedeviceservice = attendencedeviceservice;
            _employeeRepository = employeeRepository;
            _attendenceService = attendenceService;
            _attDeviceService = attDeviceService;
            Configuration = _configuration;
            Con = this.Configuration.GetConnectionString("DBConnection");
            HttpContext httpContext = HttpContext;_globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllAttendenceDevicesSearch")]
        public IActionResult GetAllAttendenceDevicesSearch(AttendenceDeviceVM Search)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_attendencedeviceservice.GetAllAttendenceDevicesSearch(Search, _globalshared.Lang_G, _globalshared.BranchId_G));
        }
        [HttpGet("GetAllAttendenceDevices")]
        public IActionResult GetAllAttendenceDevices()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_attendencedeviceservice.GetAllAttendenceDevices(_globalshared.Lang_G, _globalshared.BranchId_G));
        }
        [HttpPost("SaveAttendenceDevice")]
        public IActionResult SaveAttendenceDevice(AttendenceDevice attendencedevice)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendencedeviceservice.SaveAttendenceDevice(attendencedevice, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteAttendenceDevice")]
        public IActionResult DeleteAttendenceDevice(int AttendenceDeviceId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendencedeviceservice.DeleteAttendenceDevice(AttendenceDeviceId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("FillAttendenceDeviceSelect")]
        public IActionResult FillAttendenceDeviceSelect(int SelectedBranchId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_attendencedeviceservice.GetAllAttendenceDevices(_globalshared.Lang_G, SelectedBranchId).Result.Select(s => new
            {
                Id = s.AttendenceDeviceId,
                Name = s.DeviceIP
            }));
        }
        [HttpGet("SearchAttendenceDevices")]
        public IActionResult SearchAttendenceDevices(AttendenceDeviceVM AttendenceDevicesSearch, string lang)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_attendencedeviceservice.SearchAttendenceDevices(AttendenceDevicesSearch, _globalshared.Lang_G, _globalshared.BranchId_G));
        }
        [HttpGet("SearchAttendenceDevicesById")]
        public IActionResult SearchAttendenceDevicesById(string AttendenceDeviceId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_attendencedeviceservice.SearchAttendenceDevicesById(AttendenceDeviceId, _globalshared.Lang_G));
        }
        [HttpGet("getattend")]
        public IActionResult getattend()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            IEnumerable<Dawamyattend> students = null;
            try
            {
                var devicedata = _attDeviceService.GetDevicesetting(_globalshared.BranchId_G).Result;
                if (devicedata != null)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://myapi.dawammy.com");
                        //HTTP GET
                        string today = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));


                        DateTime now = DateTime.Now;
                        var startDate = new DateTime(now.Year, now.Month, 1);
                        var firstday = startDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));


                        var responseTask = client.GetAsync("Service.asmx/AttendenceList?ArgCompanyCode=" + devicedata.ArgCompanyCode + "&ArgEmpUsername=" + devicedata.ArgEmpUsername + "&ArgEmpPassowrd=" + devicedata.ArgEmpPassowrd + "&ArgEmpIdentity=&ArgDateFromstr=" + firstday + "&ArgDateTostr=" + today + "&ArgDeviceName=&ArgRecognitionType=&ArgProcessType=&ArgPacket=1&ArgPayLoad=10000");
                        responseTask.Wait();

                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var readTask = result.Content.ReadAsStringAsync().Result;
                            //readTask.Wait();
                            var jo = JObject.Parse(readTask);
                            var id = jo["data"]["DT"].ToString();
                            // DataTable dt = JsonConvert.DeserializeObject<DataTable>(readTask);

                            students = JsonConvert.DeserializeObject<List<Dawamyattend>>(id.ToString());
                            //students = readTask.Result;

                            foreach (var item in students)
                            {
                                //DateTime checkIn = DateTime.Parse(item.PunchDateTime);
                                DateTime checkIn = DateTime.ParseExact(item.PunchDateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);


                                //var checkdate = DateTime.ParseExact(item.PunchDateTime, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                                var Emp = _employeeRepository.GetMatching(x => !x.IsDeleted && x.NationalId == item.UserID).FirstOrDefault();
                                DateTime? checkout = null;
                                int type;
                                string chtype = "";
                                if (item.ProcessType == "SIGNOUT")
                                {
                                    checkout = checkIn;// item.PunchDateTime;// DateTime.ParseExact(item.PunchDateTime, "yyyy-MM-dd HH:mm tt", null);// DateTime.Parse( item.PunchDateTime);
                                    type = 2;
                                    chtype = "خروج";
                                }
                                else
                                {
                                    //chechin = item.PunchDateTime.ToString();
                                    type = 1;
                                    chtype = "دخول";
                                }
                                try
                                {
                                    if (Emp != null)
                                    {
                                        var attendence = new Attendence()
                                        {
                                            EmpId = Emp.EmployeeId,
                                            RealEmpId = Emp.EmployeeId,
                                            CheckTime = checkIn,//DateTime.Parse( item.PunchDateTime),
                                            AttendenceDate = checkIn.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")),//checkIn.ToString("yyyy-MM-dd"),
                                            CheckIn = checkIn.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")),
                                            CheckOut = checkout,
                                            Type = type,
                                            Source = 1, //De
                                            ShiftTime = 1,
                                            Hint = "تسجيل حركة من الساعة",
                                            CheckType = chtype,
                                            WorkCode = "0",
                                            BranchId = Emp.BranchId ?? 0,
                                            AddUser = 1,
                                            AddDate = DateTime.Now,
                                        };
                                        var results = _attendenceService.SaveAttendence_FromDevice(attendence, 0, Emp.BranchId ?? 0);
                                    }
                                }
                                catch (Exception ex)
                                {

                                }


                            }


                        }
                        else //web api sent error response 
                        {
                            //log response status here..

                            students = Enumerable.Empty<Dawamyattend>();

                            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Ok(students);
        }
        [HttpGet("TestZTKODevice")]
        public IActionResult TestZTKODevice()
        {

            var date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
            DateTime checkIn = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            DateTime? checkout = null;
            var attendence = new Attendence()
            {
                EmpId = 1,
                RealEmpId = 1,
                CheckTime = checkIn,//DateTime.Parse( item.PunchDateTime),
                AttendenceDate = checkIn.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")),//checkIn.ToString("yyyy-MM-dd"),
                CheckIn = checkIn.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")),
                CheckOut = checkout,
                Type = 1,
                Source = 1, //De
                ShiftTime = 1,
                Hint = "تسجيل حركة من الساعة",
                CheckType = "دخول",
                WorkCode = "0",
                BranchId = 1,
                AddUser = 1,
                AddDate = DateTime.Now,
            };
            var results = _attendenceService.SaveAttendence_FromDevice(attendence, 0, 1);
            return Ok(results);


        }
    }
}

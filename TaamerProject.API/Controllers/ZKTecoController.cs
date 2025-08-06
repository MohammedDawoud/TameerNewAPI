using DocumentFormat.OpenXml.Packaging;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using System.Net;
//using System.Web.Http;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ZKTecoController : ControllerBase
    {
        private readonly IEmployeesRepository _employeeRepository;
        private readonly IAttendenceService _attendenceService;
        private readonly IAttTimeDetailsRepository _AttTimeDetailsRepository;
        public GlobalShared _globalshared;
        public ZKTecoController(IEmployeesRepository employeeRepository, IAttendenceService attendenceService
            , IAttTimeDetailsRepository attTimeDetailsRepository)
        {
            _employeeRepository = employeeRepository;
            _attendenceService = attendenceService;
            _AttTimeDetailsRepository = attTimeDetailsRepository;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Index(string EnrollNumber, int IsInValid, int AttState, int VerifyMethod, string time)
        {
            try
            {
                DateTime checkIn;
                if (EnrollNumber == null && time == null)
                {
                    return (IActionResult)Ok("BadRequest");
                }

                var info = new CultureInfo("en-US");
                if (DateTime.TryParseExact(time, "yyyy-M-d HH:mm:ss", info, DateTimeStyles.None, out DateTime parsedDate))
                {

                    // Format the parsed date as "yyyy-MM-dd" using the invariant culture

                    checkIn = parsedDate;// DateTime.ParseExact(parsedDate.ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en"));
                    //var ss = HijriToGreg(checkIn.ToString());
                    // Format the parsed date-time as desired
                    
                }
                else if (DateTime.TryParseExact(time, "yyyy-M-dd HH:mm:ss", info, DateTimeStyles.None, out DateTime parsedDate2))
                {

                    // Format the parsed date as "yyyy-MM-dd" using the invariant culture

                    checkIn = parsedDate2;
                }
                else
                {
                    checkIn = DateTime.ParseExact(time, "yyyy-MM-dd HH:mm:s", CultureInfo.CreateSpecificCulture("en"));

                }
                int CheckInMode = AttState + 1; //AttState: 0 In , 1 Out but CheckIn = 1, Checkout = 2
                DateTime? checkout = null;


                var Emp = _employeeRepository.GetMatching(x => !x.IsDeleted && x.EmployeeNo.Trim() == EnrollNumber.Replace("\"", "")).FirstOrDefault();
                if (Emp == null)
                    return (IActionResult)Ok("NotFound");

                var attendence = new Attendence()
                {
                    EmpId = Emp.EmployeeId,
                    RealEmpId = Emp.EmployeeId,
                    CheckTime = checkIn,
                    AttendenceDate = checkIn.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    CheckIn = CheckInMode == 0 ? checkIn.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : null,
                    CheckOut = checkout,
                    Type = CheckInMode,
                    Source = 1, //De
                    ShiftTime = 1,
                    Hint = "تسجيل حركة من الساعة",
                    CheckType = CheckInMode == 0 ? "دخول" : "خروج",
                    WorkCode = "0",
                    BranchId = Emp.BranchId ?? 0,
                    AddUser = 1,
                    AddDate = DateTime.Now,
                    Hour=checkIn.Hour,
                    Minute=checkIn.Minute,
                    
                };
                var result = _attendenceService.SaveAttendence_FromDevice(attendence, 0, Emp.BranchId ?? 0);

                if (result.StatusCode == HttpStatusCode.OK)
                    return (IActionResult)Ok("Ok");
                else
                    return (IActionResult)Ok(result.ReasonPhrase);
            }
            catch (Exception ex)
            {
                return (IActionResult)Ok("BadRequest");
            }
        }

        [AllowAnonymous]
        [HttpPost("AddBulk")]

        public IActionResult Index([FromBody] string dataHistory)
        {
            try
            {
                DateTime checkIn;
                //string dataHistory = "[{'User ID':'13','Verify Date':'2021-4-26 20:30:7','Verify Type':1,'Verify State':0,'WorkCode':0}," +
                //    "{'User ID':'13','Verify Date':'2021-4-26 20:30:39','Verify Type':1,'Verify State':0,'WorkCode':0}," +
                //    "{'User ID':'13','Verify Date':'2021-4-26 20:32:9','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-4-26 20:36:6','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-4-26 20:36:29','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-4-26 20:40:26','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-4-26 20:40:47','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-4-26 21:43:49','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-4-26 21:44:49','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-4-26 21:50:31','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-4-26 21:54:23','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-4-26 21:54:57','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-4-26 21:56:32','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-4-26 22:0:8','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-4-26 22:13:53','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-4-26 22:28:5','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-4-26 22:28:38','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-4-26 22:40:24','Verify Type':1,'Verify State':1,'WorkCode':0},{'User ID':'13','Verify Date':'2021-4-27 0:44:12','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-4-28 0:9:8','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-4-28 4:59:7','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-4-28 5:2:33','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-4-28 5:3:3','Verify Type':1,'Verify State':1,'WorkCode':0},{'User ID':'13','Verify Date':'2021-4-28 6:58:9','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-6 1:3:38','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-6 1:7:1','Verify Type':1,'Verify State':1,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-18 21:56:11','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-19 0:32:8','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-19 0:33:20','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-19 1:10:39','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-19 1:10:40','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-19 1:18:21','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-19 1:20:6','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-19 1:20:50','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-19 1:27:43','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-19 1:28:36','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-19 1:36:41','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-19 1:38:58','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-19 1:44:39','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-19 1:46:21','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-19 1:48:14','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-19 2:46:16','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-19 2:54:35','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-19 2:58:6','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-20 1:14:17','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-20 11:0:39','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-20 11:1:50','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-20 11:4:14','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-20 18:34:10','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-20 18:35:32','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-20 18:38:2','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-20 19:16:10','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-20 19:18:42','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-20 19:19:57','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-20 19:20:49','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-29 16:37:25','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-29 16:37:58','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-29 16:39:2','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-30 1:34:16','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-30 1:43:31','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-30 1:43:57','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-30 1:59:44','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-30 2:2:39','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-5-30 2:6:18','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-7-12 20:41:32','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-7-12 20:46:39','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-7-12 20:54:41','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-7-12 22:12:59','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-7-12 22:13:24','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-7-13 9:37:49','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-7-13 9:41:57','Verify Type':1,'Verify State':4,'WorkCode':0},{'User ID':'13','Verify Date':'2021-7-13 11:14:29','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-8-25 18:33:43','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-8-25 18:38:46','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-8-25 18:41:30','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-8-25 18:43:52','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-8-25 18:45:47','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-8-26 10:30:48','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-8-26 10:37:51','Verify Type':1,'Verify State':0,'WorkCode':0},{'User ID':'13','Verify Date':'2021-8-26 10:40:5','Verify Type':1,'Verify State':0,'WorkCode':0}]";
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(dataHistory);
                if (dataHistory == null)
                {
                    return (IActionResult)Ok("BadRequest");
                }
                int okResult = 0;
                int noResult = 0;
                string message = "";
                foreach (DataRow item in dt.Rows)
                {
                    //DateTime checkIn = DateTime.ParseExact((string)item["Verify Date"], "yyyy-MM-dd HH:mm:s", CultureInfo.InvariantCulture);


                    var time = (string)item["Verify Date"];
                    var info = new CultureInfo("en-US");
                    if (DateTime.TryParseExact(time, "yyyy-M-d HH:mm:ss", info, DateTimeStyles.None, out DateTime parsedDate))
                    {

                        // Format the parsed date as "yyyy-MM-dd" using the invariant culture

                        checkIn = parsedDate;// DateTime.ParseExact(parsedDate.ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en"));
                                             //var ss = HijriToGreg(checkIn.ToString());
                                             // Format the parsed date-time as desired

                    }
                    else if (DateTime.TryParseExact(time, "yyyy-M-dd HH:mm:s", info, DateTimeStyles.None, out DateTime parsedDate2))
                    {

                        // Format the parsed date as "yyyy-MM-dd" using the invariant culture

                        checkIn = parsedDate2;
                    }
                    else
                    {
                        checkIn = DateTime.ParseExact(time, "yyyy-MM-dd HH:mm:s", CultureInfo.CreateSpecificCulture("en"));

                    }

                    //DateTime checkIn = DateTime.Parse(((string)item["Verify Date"]).Replace("\"", ""));
                    int CheckInMode = int.Parse(item["Verify State"].ToString());
                    //if (checkIn.ToString("yyyy-MM-dd") == "2021-05-20")
                    //{
                    //    string x = "Hi";
                    //}
                    DateTime? checkout = null;
                    if (CheckInMode == 1)
                        checkout = checkIn;
                    var Emp = _employeeRepository.GetMatching(x => !x.IsDeleted && x.EmployeeNo.Trim() == ((string)item["User ID"])).FirstOrDefault();
                    if (Emp == null)
                    {
                        noResult++;
                        message = message + "\n" + "لا يوجد موظف بهذا الرقم";
                        continue;
                    }

                    var attendence = new Attendence()
                    {
                        EmpId = Emp.EmployeeId,
                        RealEmpId = Emp.EmployeeId,
                        CheckTime = checkIn,
                        AttendenceDate = checkIn.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                        CheckIn = CheckInMode == 0 ? checkIn.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : null,
                        CheckOut = checkout,
                        Type = CheckInMode + 1,
                        Source = 1, //De
                        ShiftTime = 1,
                        Hint = "تسجيل حركة من الساعة",
                        CheckType = CheckInMode == 0 ? "دخول" : "خروج",
                        WorkCode = "0",
                        BranchId = Emp.BranchId ?? 0,
                        AddUser = 1,
                        AddDate = DateTime.Now,
                        Hour = checkIn.Hour,
                        Minute = checkIn.Minute,
                    };
                    var result = _attendenceService.SaveAttendence_FromDevice(attendence, 0, Emp.BranchId ?? 0);
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        okResult++;
                        message = message + "\n" + result.ReasonPhrase;
                    }
                    else
                    {
                        noResult++;
                        message = message + "\n" + result.ReasonPhrase;
                    }
                }

                return (IActionResult)Ok(okResult.ToString() + " affected rows, " + noResult.ToString() + " failed rows from total rows: " + dt.Rows.Count.ToString() + " \n" + message);//+ 
            }
            catch (Exception ex)
            {
                return (IActionResult)Ok("BadRequest");
            }
        }

        static bool TryParseExact(string date, string format)
        {
            DateTime parsedDate;
            return DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);
        }
        [HttpGet("HijriToGreg")]
        public DateTime HijriToGreg(string hijri)
        {
            CultureInfo arCul = new CultureInfo("ar-SA");
            HijriCalendar h = new HijriCalendar();
            arCul.DateTimeFormat.Calendar = h;
            DateTime tempDate = DateTime.ParseExact(hijri, "yyyy-MM-dd HH:MM:ss", arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
            return tempDate;
        }

    }
}

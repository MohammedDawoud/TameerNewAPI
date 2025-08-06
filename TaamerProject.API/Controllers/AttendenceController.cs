using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using System.Net;
using System.Reflection;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;
using BioMetrixCore;
using Microsoft.AspNetCore.Http.HttpResults;
using DocumentFormat.OpenXml.Bibliography;


namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class AttendenceController : ControllerBase
    {
        private IOrganizationsService _organizationsservice;
        private IBranchesService _BranchesService;
        private IAttendenceService _attendenceservice;
        private IAttendeesService _Attendeesservice;
        private IAttendenceDeviceService _attendencedeviceservice;
        private IEmployeesService _EmployeesService;
        private IDeviceAttService _DeviceAttService;
        private IUserNotificationPrivilegesService _NotifprivilegesService;
        private IEmployeesRepository _employeeRepository;
        //protected ITameerProUnitOfWork _uow;
        private IAttendenceService _attendenceService;
        private readonly IFiscalyearsService _FiscalyearsService;


        private string? Con;
        private byte[] ReportPDF;
        //private int iMachineNumber = 1;

        public bool isDeviceConnected = false;
        public string MachineNumber;
        public ZkemClient objZkeeper;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        public AttendenceController(IOrganizationsService organizationsservice, IBranchesService branchesService
            , IAttendenceService attendenceservice, IAttendeesService attendeesservice
            , IAttendenceDeviceService attendencedeviceservice, IEmployeesService employeesService
            , IDeviceAttService deviceAttServic, IUserNotificationPrivilegesService notifprivilegesService
            , IEmployeesRepository employeeRepository, IAttendenceService attendenceService
            , IFiscalyearsService fiscalyearsService, IConfiguration _configuration)
        {
            _organizationsservice = organizationsservice;
            _BranchesService = branchesService;
            _attendenceservice = attendenceservice;
            _Attendeesservice = attendeesservice;
            _attendencedeviceservice = attendencedeviceservice;
            _EmployeesService = employeesService;
            _DeviceAttService = deviceAttServic;
            _NotifprivilegesService = notifprivilegesService;
            _employeeRepository = employeeRepository;
            _attendenceService = attendenceService;
            _FiscalyearsService = fiscalyearsService;

            Configuration = _configuration;
            Con = this.Configuration.GetConnectionString("DBConnection");

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }




        [HttpGet("ToggleControls")]
        private void ToggleControls(bool value)
        {


        }
        [HttpGet("GetAllAttendence")]
        public IActionResult GetAllAttendence()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_attendenceservice.GetAllAttendence(_globalshared.BranchId_G, Con??""));
        }
        [HttpGet("GetAllAttendence_Device")]
        public IActionResult GetAllAttendence_Device()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_attendenceservice.GetAllAttendence(_globalshared.BranchId_G));
        }
        [HttpGet("GetAllAttendenceSearch")]
        public IActionResult GetAllAttendenceSearch(AttendenceVM Search)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_attendenceservice.GetAllAttendenceSearch(Search, _globalshared.BranchId_G, Con ?? ""));
        }
        [HttpPost("SaveAttendence")]
        public IActionResult SaveAttendence(Attendence attendence)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendenceservice.SaveAttendence(attendence, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveAttendence_N")]
        public IActionResult SaveAttendence_N(Attendence attendence)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendenceservice.SaveAttendence_N(attendence, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("EmpAttendenceSearch")]
        public IActionResult EmpAttendenceSearch(AttendenceVM AttendenceSearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_attendenceservice.EmpAttendenceSearch(AttendenceSearch, _globalshared.BranchId_G));
        }

        [HttpPost("EmpAttendenceSearch2")]
        public IActionResult EmpAttendenceSearch2([FromBody]AttendenceVM AttendenceSearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_attendenceservice.EmpAttendenceSearch(AttendenceSearch, _globalshared.BranchId_G));
        }
        [HttpPost("ConfirmMonthAttendance")]
        public IActionResult ConfirmMonthAttendance(Attendence attendence)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendenceservice.ConFirmMonthAttendence(attendence, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G);
            return Ok(result);
        }
        [HttpPost("DeleteAttendence")]
        public IActionResult DeleteAttendence(int AttendenceId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendenceservice.DeleteAttendence(AttendenceId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteAttendence_N")]
        public IActionResult DeleteAttendence_N(string Date)
        {
            var result = _attendenceservice.DeleteAttendencesByDate(Date);
            return Ok(result);
        }
        [HttpGet("GetAllAttendees")]
        public IActionResult GetAllAttendees()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_Attendeesservice.GetAllAttendees(_globalshared.BranchId_G));
        }
        [HttpGet("GetAttendeesbyStatus")]
        public IActionResult GetAttendeesbyStatus(int Status, string Date)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_Attendeesservice.GetAttendeesbyStatus(Status, Date, _globalshared.BranchId_G));
        }
        [HttpGet("GetAttendeeslate")]
        public IActionResult GetAttendeeslate(bool IsLate, string Date)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_Attendeesservice.GetAttendeeslate(IsLate, Date, _globalshared.BranchId_G));
        }
        [HttpGet("GetAttendeesEarlyCheckOut")]
        public IActionResult GetAttendeesEarlyCheckOut(bool IsEarlyCheckOut, string Date)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_Attendeesservice.GetAttendeesEarlyCheckOut(IsEarlyCheckOut, Date, _globalshared.BranchId_G));
        }
        [HttpGet("GetAttendeesOut")]
        public IActionResult GetAttendeesOut(bool IsOut, string Date)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_Attendeesservice.GetAttendeesOut(IsOut, Date, _globalshared.BranchId_G));
        }
        [HttpGet("GetAllEmpAbsence")]
        public IActionResult GetAllEmpAbsence(int Status, string Date)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_Attendeesservice.GetAllEmpAbsence(Status, Date, _globalshared.BranchId_G));
        }
        [HttpGet("GetAllEmplate")]
        public IActionResult GetAllEmplate(bool IsLateCheckIn, string Date)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_Attendeesservice.GetAllEmplate(IsLateCheckIn, Date, _globalshared.BranchId_G));
        }
        [HttpGet("GetAllEmpAbsencelate")]
        public IActionResult GetAllEmpAbsencelate(string Date)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_Attendeesservice.GetAllEmpAbsencelate(Date, _globalshared.BranchId_G));
        }
        [HttpGet("RaiseDeviceEvent")]
        private void RaiseDeviceEvent(object sender, string actionType)
        {
            switch (actionType)
            {
                case UniversalStatic.acx_Disconnect:
                    {

                        ToggleControls(false);
                        break;
                    }

                default:
                    break;
            }

        }
        //[HttpGet("GetEmpAttendenceDevice")]
        //public IActionResult GetEmpAttendenceDevice(List<Thing> things)
        //{
        //    HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        //    var t = things;
        //    int iMachineNumber = -1;

        //    string ipAddress = t.Select(x => x.tbxDeviceIP).FirstOrDefault().Trim();

        //    string port = t.Select(x => x.tbxPort).FirstOrDefault().Trim();
        //    MachineNumber = t.Select(x => x.tbxMachineNumber).FirstOrDefault().Trim();
        //    string branchid = t.Select(x => x.tbxBranchId).FirstOrDefault().Trim();
        //    string id = t.Select(x => x.tbxAttendenceDeviceId).FirstOrDefault().Trim();

        //    var result = _attendencedeviceservice.GetAllAttendenceDevicesByID(_globalshared.Lang_G, Convert.ToInt32(branchid), Convert.ToInt32(id)).Result.ToList();

        //    try
        //    {
        //        if(true)// (IsDeviceConnected)
        //        {
        //            //IsDeviceConnected = false;
        //            // this.Cursor = Cursors.Default;
        //            return Ok(new { Result = false, Message = "Resources.MD_ConnectDisconnect" });
        //        }


        //        if (ipAddress == string.Empty || port == string.Empty)
        //            throw new Exception("The Device IP Address and Port is mandotory !!");

        //        int portNumber = 4370;
        //        if (!int.TryParse(port, out portNumber))
        //            throw new Exception("Not a valid port number");

        //        bool isValidIpA = UniversalStatic.ValidateIP(ipAddress);
        //        if (!isValidIpA)
        //            throw new Exception("The Device IP is invalid !!");

        //        isValidIpA = UniversalStatic.PingTheDevice(ipAddress);
        //        if (!isValidIpA)
        //            throw new Exception("The device at " + ipAddress + ":" + port + " did not respond!!");

        //        try
        //        {
        //            objZkeeper = new ZkemClient(RaiseDeviceEvent);
        //            IsDeviceConnected = objZkeeper.Connect_Net(ipAddress, portNumber);


        //        }
        //        catch
        //        {

        //        }
        //        if (IsDeviceConnected)
        //        {

        //            return Ok(new { Result = true, Message = "Resources.MD_ConnectSuccessfully" });

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { Result = false, Message = "Resources.MD_ConnectFailed" });

        //    }
        //    return Ok(new { Result = true, Message = "Resources.MD_ConnectSuccessfully" });

        //}
        [HttpPost("DownloadFingerPrint")]
        public IActionResult DownloadFingerPrint()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            int idwErrorCode = 0;string dwEnrollNumber1 = "";int dwVerifyMode = 0;int dwInOutMode = 0;
            int dwYear = 0;int dwMonth = 0;int dwDay = 0;int dwHour = 0;int dwMinute = 0;
            int dwSecond = 0;int dwWorkCode = 0;int count = 0;

            ZkemClient objZkeeper = new ZkemClient(RaiseDeviceEvent);


            var listOfDevices = _attendencedeviceservice.GetAllAttendenceDevices(_globalshared.Lang_G, _globalshared.BranchId_G).Result.ToList();
            foreach (var list in listOfDevices)
            {


                var employees = _EmployeesService.GetAllEmployees(_globalshared.Lang_G,(int) list.BranchId).Result.Select(s => s.EmployeeNo).ToList();//      .Employees.Select(x => x.File_id).ToList();

                var attendees = new List<Attendence>();
                var attendeesDevice = new List<AttendenceDevice>();
                //  var DeviceHasDate = _DeviceAttService.get.Where(x => x.DeviceId == DeviceId).FirstOrDefault();

                var DeviceHasDate = _attendencedeviceservice.GetAllDevicesByID(_globalshared.Lang_G, list.AttendenceDeviceId).Result.FirstOrDefault();
                //   axCZKEM1.EnableDevice(Convert.ToInt32( DeviceHasDate.MachineNumber), false);//disable the device

                DateTime? LastDate = DeviceHasDate != null ? DeviceHasDate.LastUpdate : new DateTime(DateTime.Now.Year, 1, 1);
                DateTime DeviceDate = LastDate?? new DateTime(DateTime.Now.Year, 1, 1);


                try
                {
                    {
                        string inputDate = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond).ToString();

                        if (!string.IsNullOrEmpty(dwEnrollNumber1))
                        {
                            int file_id = Convert.ToInt32(dwEnrollNumber1);
                            string idwInOutModee = "";
                            if (dwInOutMode.ToString() == "0" || dwInOutMode.ToString() == "o" || dwInOutMode == 0)
                            {
                                idwInOutModee = "دخول";
                            }
                            else
                            {
                                idwInOutModee = "خروج";
                            }


                            if (new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond) > LastDate && employees.Contains(file_id.ToString()))



                                attendees.Add(new Attendence
                                {
                                    EmpId = Convert.ToInt32(dwEnrollNumber1),
                                    CheckIn = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond).ToString("yyyy-MM-dd"),
                                    CheckTime = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond),
                                    Type = dwInOutMode == 0 ? 1 : 2,
                                    Source = 2,
                                    ShiftTime = 1,
                                    Hint = "تسجيل حركة من الساعة",
                                    CheckType = idwInOutModee.ToString(),
                                    WorkCode = dwWorkCode.ToString(),
                                    Done = false,
                                    BranchId = _globalshared.BranchId_G

                                });
                        }


                        DeviceDate = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);
                        _attendenceservice.SaveListAttendence(attendees, _globalshared.UserId_G, _globalshared.BranchId_G);

                        count++;
                    }

                    //axCZKEM1.EnableDevice(axCZKEM1.MachineNumber, true);

                    //axCZKEM1.RefreshData(axCZKEM1.MachineNumber);


                }

                catch (Exception e)
                {


                    // axCZKEM1.GetLastError(ref idwErrorCode);

                    if (idwErrorCode != 0)
                    {

                        return Ok(new { Result = false, Message = "Reading data from terminal failed,ErrorCode: " + idwErrorCode.ToString() });
                    }
                    else
                    {

                        return Ok(new { Result = false, Message = "No data from terminal returns!" });
                    }
                }
                finally
                {
                    if (count > 0)
                    {
                        if (DeviceHasDate != null)
                        {
                            attendeesDevice.Add(new AttendenceDevice
                            {
                                DeviceIP = DeviceHasDate.DeviceIP,
                                Port = DeviceHasDate.Port,
                                MachineNumber = DeviceHasDate.MachineNumber,
                                BranchId = DeviceHasDate.BranchId??0,
                                AttendenceDeviceId = DeviceHasDate.AttendenceDeviceId,
                                UpdateDate = DateTime.Now,
                            });
                            _attendencedeviceservice.SaveDevice(attendeesDevice, _globalshared.UserId_G, _globalshared.BranchId_G);

                        }
                    }

                }
            }
            if (count <= 0)
            {
                return Ok(new { Result = false, Message = "No data from terminal returns!" });
            }
            return Ok(new { Result = true, Message = "تم استيراد سجل الحضور بنجاح .." });
        }
        [HttpGet("GetCorrectMonth")]
        public string GetCorrectMonth(int month)
        {

            if (month > 9)
            {
                return Convert.ToString(month);
            }
            else
            {
                return "0" + month;
            }
        }

        [HttpGet("GetCorrectDay")]
        public string GetCorrectDay(int day)
        {

            if (day > 9)
            {
                return Convert.ToString(day);
            }
            else
            {
                return "0" + day;
            }
        }

        [HttpGet("GetAbsenceDataDGV")]
        public IActionResult GetAbsenceDataDGV(string? FromDate, string? ToDate, int? EmpId, int? BranchId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendenceservice.GetAbsenceData(FromDate, ToDate,(int)EmpId,(int)BranchId, _globalshared.Lang_G, Con??"", _globalshared.YearId_G);
            return Ok(result);

        }
        [HttpGet("GetAbsenceMoreThanTen")]
        public IActionResult GetAbsenceMoreThanTen()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var priv = _NotifprivilegesService.GetPrivilegesIdsByUserId(_globalshared.UserId_G).Result;
            if (priv.Contains(172))
            {
                var absentEmps = _attendenceservice.GetAbsentEmployeesNote(_globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.Lang_G, Con);
                return Ok(absentEmps);
            }
            else return null;
        }
        [HttpPost("InsertAbsentEmpSee")]
        public IActionResult InsertAbsentEmpSee()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendenceservice.InsertAbsentEmpSee(_globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetAbsenceDataTodayDGV")]
        public IActionResult GetAbsenceDataTodayDGV(string TodayDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendenceservice.GetAbsenceDataToday(TodayDate, _globalshared.BranchId_G, _globalshared.Lang_G, Con, _globalshared.YearId_G);
            return Ok(result);
        }
        [HttpGet("GetAbsenceDataTodayDGV2")]
        public IActionResult GetAbsenceDataTodayDGV2()
        {
            string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendenceservice.GetAbsenceDataToday(ActionDate, _globalshared.BranchId_G, _globalshared.Lang_G, Con, _globalshared.YearId_G);
            return Ok(result);
        }
        [HttpGet("GetAbsenceDataTodayWithBranchDGV")]
        public IActionResult GetAbsenceDataTodayWithBranchDGV(string TodayDate, int branchId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendenceservice.GetAbsenceDataToday(TodayDate, branchId, _globalshared.Lang_G, Con, _globalshared.YearId_G);
            return Ok(result);

        }
        [HttpGet("GetLateDataDGV")]
        public IActionResult GetLateDataDGV(string FromDate, string ToDate, int EmpId, int Shift, int BranchId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendenceservice.GetLateData(FromDate, ToDate, EmpId, Shift, BranchId, _globalshared.Lang_G, Con, _globalshared.YearId_G);
            return Ok(result);

        }
        [HttpGet("GetLateDataTodayDGV")]
        public IActionResult GetLateDataTodayDGV(string TodayDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int Shft = 0;
            var result = _attendenceservice.GetLateDataToday(TodayDate, Shft, _globalshared.BranchId_G, _globalshared.Lang_G, Con, _globalshared.YearId_G);
            return Ok(result);

        }

        [HttpGet("GetLateDataTodaywithBranchDGV")]
        public IActionResult GetLateDataTodaywithBranchDGV(string TodayDate, int branchId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int Shft = 0;
            var result = _attendenceservice.GetLateDataToday(TodayDate, Shft, branchId, _globalshared.Lang_G, Con, _globalshared.YearId_G);
            return Ok(result);

        }
        [HttpGet("GetEarlyDepartureDataDGV")]
        public IActionResult GetEarlyDepartureDataDGV(string FromDate, string ToDate, int EmpId, int Shift, int BranchId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendenceservice.GetEarlyDepartureData(FromDate, ToDate, EmpId, Shift, BranchId, _globalshared.Lang_G, Con, _globalshared.YearId_G);
            return Ok(result);

        }
        [HttpGet("GetEarlyDepartureDataTodayDGV")]
        public IActionResult GetEarlyDepartureDataTodayDGV(string TodayDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int Shft = 0;
            var result = _attendenceservice.GetEarlyDepartureDataToday(TodayDate, Shft, _globalshared.BranchId_G, _globalshared.Lang_G, Con, _globalshared.YearId_G);
            return Ok(result);

        }

        [HttpGet("GetNotLoggedOutDataDGV")]
        public IActionResult GetNotLoggedOutDataDGV(string FromDate, string ToDate, int BranchId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendenceservice.GetNotLoggedOutData(FromDate, ToDate, BranchId, _globalshared.Lang_G, Con ?? "", _globalshared.YearId_G);
            return Ok(result);

        }
        [HttpGet("GetAttendanceDataDGV")]
        public IActionResult GetAttendanceDataDGV(string FromDate, string ToDate, int Shift, int BranchId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendenceservice.GetAttendanceData(FromDate, ToDate, Shift, BranchId, _globalshared.Lang_G, Con ?? "", _globalshared.YearId_G);
            return Ok(result);

        }

        [HttpGet("GetAttendanceDataDGV_paging")]
        public IActionResult GetAttendanceDataDGV_paging(string FromDate, string ToDate, int Shift, int BranchId, int pageNumber, int pageSize)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendenceservice.GetAttendanceData(FromDate, ToDate, Shift, BranchId, _globalshared.Lang_G, Con ?? "", _globalshared.YearId_G, pageNumber, pageSize);
            return Ok(result);

        }
        [HttpGet("GetAttendance_Screen")]
        public IActionResult GetAttendance_Screen(string? FromDate, string ?ToDate, int? Shift, int? BrID, int? SwType, int? UserIDF)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendenceservice.GetAttendance_Screen(FromDate, ToDate, (int)Shift, _globalshared.BranchId_G, (int)SwType, _globalshared.Lang_G, Con ?? "", _globalshared.YearId_G, (int)UserIDF).Result;//.OrderBy(x => x.DawamId);
            return Ok(result);
        }
        [HttpGet("GetAttendance_Screen_M")]
        public IActionResult GetAttendance_Screen_M(int Year, int Month, int Shift, int BrID, int SwType, int UserIDF)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendenceservice.GetAttendance_Screen_M(Year, Month, Shift, _globalshared.BranchId_G, SwType, _globalshared.Lang_G, Con ?? "", _globalshared.YearId_G, UserIDF).Result.OrderBy(x => x.DawamId); ;
            return Ok(result);

        }
        [HttpGet("GetAttendance_Screen_W")]
        public IActionResult GetAttendance_Screen_W(int Year, int Month, int Shift, int BrID, int SwType, int UserIDF)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendenceservice.GetAttendance_Screen_W(Year, Month, Shift, _globalshared.BranchId_G, SwType, _globalshared.Lang_G, Con ?? "", _globalshared.YearId_G, UserIDF).Result.OrderBy(x => x.DawamId);
            return Ok(result);
        }

        [HttpGet("GetAttendanceDataDGV_application")]
        public IActionResult GetAttendanceDataDGV_application(string FromDate, string ToDate, int Shift, int BranchId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendenceservice.GetAttendanceData_Application(FromDate, ToDate, Shift, BranchId, _globalshared.Lang_G, Con, _globalshared.YearId_G).Result.Where(x => x.FromApplication == "1");
            return Ok(result);
        }
        //[HttpGet]
        //public FileResult PDFDownloadEmployeeAttanceToday(string TodayDate, int branchId, string branch = "", bool? All = false)
        //{
        //    var FiscalId = Request.Cookies["ActiveYear"].Value;
        //    var YearNEW = Convert.ToInt32(_FiscalyearsService.GetYearID(Convert.ToInt32(FiscalId)));
        //    byte[] pdfByte = { 0 };

        //    int orgId = _BranchesService.GetOrganizationId(BranchId);

        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);



        //    string[] infoEmployeeAbsensceReport = { Lang == "en" ? objOrganization.NameEn : objOrganization.NameAr, "الموظفين الغائبين اليوم", objOrganization.LogoUrl, branch };
        //    string[] columnEmployeeAbsensce = { "إسم الموظف", "رقم البصمة", "التاريخ", "اليوم", "الفرع" };

        //    int Shft = 0;

        //    List<LateVM> DataEmployeeAbsensce = All == true ? _attendenceservice.GetLateDataToday(TodayDate, Shft, BranchId, Lang, Con, YearNEW).ToList() :
        //                                                             _attendenceservice.GetLateDataToday(TodayDate, Shft, branchId, Lang, Con, YearNEW).ToList();

        //    DataTable listDataAbouutToExpire = ToDataTable(DataEmployeeAbsensce);

        //    pdfByte = pdfClass.DencesEmployeeAbsensce(TodayDate, infoEmployeeAbsensceReport, columnEmployeeAbsensce, listDataAbouutToExpire);
        //    return File(pdfByte, "application/pdf");

        //}

        //[HttpGet]
        //public FileResult PDFDownloadEmployeeAttanceLateToday(string TodayDate, int branchId, string branch = "", bool? All = false)
        //{
        //    var FiscalId = Request.Cookies["ActiveYear"].Value;
        //    var YearNEW = Convert.ToInt32(_FiscalyearsService.GetYearID(Convert.ToInt32(FiscalId)));
        //    byte[] pdfByte = { 0 };

        //    int orgId = _BranchesService.GetOrganizationId(BranchId);

        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);



        //    string[] infoEmployeeLateTodayReport = { Lang == "en" ? objOrganization.NameEn : objOrganization.NameAr, "الموظفين الغائبين اليوم", objOrganization.LogoUrl, branch };
        //    string[] columnEmployeeLateToday = { "إسم الموظف", "رقم البصمة", "التاريخ", "حضور فترة اولى", "زمن التأخير", "حضور فترة ثانية", "زمن التأخير " };

        //    int Shft = 0;

        //    List<LateVM> DataEmployeeLateToday = All == true ? _attendenceservice.GetLateDataToday(TodayDate, Shft, BranchId, Lang, Con, YearNEW).ToList() :
        //                                                             _attendenceservice.GetLateDataToday(TodayDate, Shft, branchId, Lang, Con, YearNEW).ToList();

        //    DataTable listDataLateToday = ToDataTable(DataEmployeeLateToday);

        //    pdfByte = pdfClass.DencesEmployeeLate(TodayDate, infoEmployeeLateTodayReport, columnEmployeeLateToday, listDataLateToday);
        //    return File(pdfByte, "application/pdf");

        //}
        [HttpGet("ToDataTable")]

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
        ////الموظفون المتاخرون
        //public IActionResult printLateMonthEmps(int EmpId, int BranchId, string From, string To, int SH)
        //{
        //    var FiscalId = Request.Cookies["ActiveYear"].Value;
        //    var YearNEW = Convert.ToInt32(_FiscalyearsService.GetYearID(Convert.ToInt32(FiscalId)));
        //    int orgId = _BranchesService.GetOrganizationId(BranchId);

        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);
        //    string[] infoDoneTasksReport = { Lang == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

        //    List<LateVM> Employees = _attendenceservice.GetLateData(From, To, EmpId, SH, BranchId, Lang, Con, YearNEW).ToList();
        //    ReportPDF = humanResourcesReports.printLateMonthEmps(Employees, From, To, infoDoneTasksReport);
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
        ////الموظفون الغائبون
        //public IActionResult printAbsenceMonthEmps(string FromDate, string ToDate, int EmpId, int BranchId)
        //{
        //    var FiscalId = Request.Cookies["ActiveYear"].Value;
        //    var YearNEW = Convert.ToInt32(_FiscalyearsService.GetYearID(Convert.ToInt32(FiscalId)));
        //    int orgId = _BranchesService.GetOrganizationId(BranchId);

        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);
        //    string[] infoDoneTasksReport = { Lang == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

        //    List<AbsenceVM> Employees = _attendenceservice.GetAbsenceData(FromDate, ToDate, EmpId, BranchId, Lang, Con, YearNEW).ToList();
        //    ReportPDF = humanResourcesReports.printAbsenceMonthEmps(Employees, FromDate, ToDate, infoDoneTasksReport);
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
        ////الموظفون الغائبون اليوم
        //public IActionResult printAbsenceTodayEmps(string TodayDate)
        //{
        //    var FiscalId = Request.Cookies["ActiveYear"].Value;
        //    var YearNEW = Convert.ToInt32(_FiscalyearsService.GetYearID(Convert.ToInt32(FiscalId)));
        //    int orgId = _BranchesService.GetOrganizationId(BranchId);

        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);
        //    string[] infoDoneTasksReport = { Lang == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

        //    List<AbsenceVM> Employees = _attendenceservice.GetAbsenceDataToday(TodayDate, BranchId, Lang, Con, YearNEW).ToList();
        //    ReportPDF = humanResourcesReports.printAbsenceEmpsToDay(Employees, infoDoneTasksReport);
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
        ////المتاخرون اليوم
        //public IActionResult printEmpsLateToday(string TodayDate)
        //{
        //    var FiscalId = Request.Cookies["ActiveYear"].Value;
        //    var YearNEW = Convert.ToInt32(_FiscalyearsService.GetYearID(Convert.ToInt32(FiscalId)));
        //    int orgId = _BranchesService.GetOrganizationId(BranchId);

        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);
        //    string[] infoDoneTasksReport = { Lang == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

        //    List<LateVM> Employees = _attendenceservice.GetLateDataToday(TodayDate, 0, BranchId, Lang, Con, YearNEW).ToList();
        //    ReportPDF = humanResourcesReports.printEmpsLateToday(Employees, infoDoneTasksReport);
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
        //// الخروج المبكر
        //public IActionResult printEmpsEarlyDeparture(int EmpId, int BranchId, string From, string To, int SH)
        //{
        //    var FiscalId = Request.Cookies["ActiveYear"].Value;
        //    var YearNEW = Convert.ToInt32(_FiscalyearsService.GetYearID(Convert.ToInt32(FiscalId)));
        //    int orgId = _BranchesService.GetOrganizationId(BranchId);

        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);
        //    string[] infoDoneTasksReport = { Lang == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

        //    List<LateVM> Employees = _attendenceservice.GetEarlyDepartureData(From, To, EmpId, SH, BranchId, Lang, Con, YearNEW).ToList();
        //    ReportPDF = humanResourcesReports.printEmpsEarlyDeparture(Employees, From, To, infoDoneTasksReport);
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
        //// لم يسجلوا خروج
        //public IActionResult printEmpsNotLoggedOut(string FromDate, string ToDate, int BranchId)
        //{
        //    var FiscalId = Request.Cookies["ActiveYear"].Value;
        //    var YearNEW = Convert.ToInt32(_FiscalyearsService.GetYearID(Convert.ToInt32(FiscalId)));
        //    int orgId = _BranchesService.GetOrganizationId(BranchId);

        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);
        //    string[] infoDoneTasksReport = { Lang == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

        //    List<NotLoggedOutVM> Employees = _attendenceservice.GetNotLoggedOutData(FromDate, ToDate, BranchId, Lang, Con, YearNEW).ToList();
        //    ReportPDF = humanResourcesReports.printEmpsNotLoggedOut(Employees, FromDate, ToDate, infoDoneTasksReport);
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
        //// حضور وانصراف الموظفين
        //public IActionResult printAttendanceData(string FromDate, string ToDate, int Shift, int BranchId)
        //{
        //    var FiscalId = Request.Cookies["ActiveYear"].Value;
        //    var YearNEW = Convert.ToInt32(_FiscalyearsService.GetYearID(Convert.ToInt32(FiscalId)));
        //    int orgId = _BranchesService.GetOrganizationId(BranchId);

        //    var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId);
        //    string[] infoDoneTasksReport = { Lang == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

        //    List<LateVM> Employees = _attendenceservice.GetAttendanceData(FromDate, ToDate, Shift, BranchId, Lang, Con, YearNEW).ToList();
        //    ReportPDF = humanResourcesReports.printAttendanceData(Employees, FromDate, ToDate, infoDoneTasksReport);
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

        [HttpPost("Adddtatmanually")]
        public IActionResult Adddtatmanually()
        {
            try
            {
                string dataHistory = "[{'User ID':'1021','Verify Date':'2021-10-27 20:30:7','Verify Type':1,'Verify State':0,'WorkCode':0}]";
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(dataHistory);
                if (dataHistory == null)
                {
                    return Ok("BadRequest");
                }
                int okResult = 0;
                int noResult = 0;
                string message = "";
                foreach (DataRow item in dt.Rows)
                {
                    //DateTime checkIn = Convert.ToDateTime((item["Verify Date"].ToString()).Replace("\"", ""));// DateTime.Parse(((string)item["Verify Date"]).Replace("\"", ""));
                    // DateTime checkIn = DateTime.ParseExact(DateTime.Now.ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                    DateTime checkIn = DateTime.ParseExact("2022-11-01 08:40:11", "yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));

                    int CheckInMode = int.Parse(item["Verify State"].ToString());
                    //if (checkIn.ToString("yyyy-MM-dd") == "2021-05-20")
                    //{
                    //    string x = "Hi";
                    //}
                    DateTime? checkout = null;
                    if (CheckInMode == 1)
                        checkout = checkIn;
                    var Emp = _employeeRepository.GetMatching(x => !x.IsDeleted && x.EmployeeNo == ((string)item["User ID"]).Replace("\"", "")).FirstOrDefault();
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
                        AttendenceDate = checkIn.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")),
                        CheckIn = CheckInMode == 0 ? checkIn.ToString("yyyy-MM-dd") : null,
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
                    };
                    var result = _attendenceService.SaveAttendence_FromDevice(attendence, 0, Emp.BranchId ?? 0);
                    if (result.StatusCode== HttpStatusCode.OK)
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

                return Ok(okResult.ToString() + " affected rows, " + noResult.ToString() + " failed rows from total rows: " + dt.Rows.Count.ToString() + " \n" + message);//+ 
            }
            catch (Exception ex)
            {
                return Ok("BadRequest");
            }
        }
        [HttpGet("GetEmployeeTotal")]
        public IActionResult GetEmployeeTotal(string FromDate, string ToDate, int EmpId, int Shift, int BranchId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attendenceservice.GetLateData_2(FromDate, ToDate, EmpId, Shift, BranchId, _globalshared.Lang_G, Con, _globalshared.YearId_G);

            var result2 = _attendenceservice.GetAbsenceData(FromDate, ToDate, EmpId, BranchId, _globalshared.Lang_G, Con, _globalshared.YearId_G).Count();
         
            AttendenceVM AttendenceSearch = new AttendenceVM();
            AttendenceSearch.StartDate = FromDate;
            AttendenceSearch.EndDate = ToDate;
            AttendenceSearch.EmpId = EmpId;


            //var result3 = _attendenceservice.EmpAttendenceSearch(AttendenceSearch, _globalshared.BranchId_G).Result.GroupBy(x => x.AttendenceDate).Select(x => x.FirstOrDefault()).Count();
            var result3 = _attendenceservice.EmpAttendenceSearch(AttendenceSearch, _globalshared.BranchId_G).Result.ToList().DistinctBy(x=>x.AttendenceDate).ToList().Count();


            empattTotals totals = new empattTotals();
            totals.attend = result3.ToString();
            totals.abcence = result2.ToString();
            totals.Late = result.ToString();
            return Ok(totals);

        }

        //[HttpPost("IsDeviceConnected")]
        //public bool IsDeviceConnected
        //{
        //    get { return isDeviceConnected; }
        //    set
        //    {
        //        isDeviceConnected = value;
        //        if (isDeviceConnected)
        //        {

        //        }
        //        else
        //        {

        //        }
        //    }
        //}

 


    }

    public class empattTotals
    {
        public string? attend { get; set; }
        public string? abcence { get; set; }
        public string? Late { get; set; }

    }
}

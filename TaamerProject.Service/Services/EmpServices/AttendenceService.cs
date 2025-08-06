using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using System.Net;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Drawing;
using Microsoft.Graph.Models;

namespace TaamerProject.Service.Services
{
    public class AttendenceService : IAttendenceService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IAttendenceRepository _AttendenceRepository;
        private readonly IVacationRepository _vacationRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IAttTimeDetailsRepository _AttTimeDetailsRepository;
        private readonly IOfficalHolidayRepository _icalHolidayRepository;

        public AttendenceService(TaamerProjectContext dataContext, ISystemAction systemAction, IAttendenceRepository attendenceRepository
            , IVacationRepository vacationRepository, IEmployeesRepository employeesRepository, IAttTimeDetailsRepository attTimeDetailsRepository, IOfficalHolidayRepository icalHolidayRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _AttendenceRepository = attendenceRepository;
            _vacationRepository = vacationRepository;
            _employeesRepository = employeesRepository;
            _AttTimeDetailsRepository = attTimeDetailsRepository;
            _icalHolidayRepository = icalHolidayRepository;
        }
        public async Task<IEnumerable<AttendenceVM>> GetAllAttendence(int BranchId, string Con = "")
        {
            var Attendence = _AttendenceRepository.GetAllAttendence(BranchId).Result;
            List<int> PassedEmps = new List<int>();
            foreach (var item in Attendence)
            {
                int y;
                if (PassedEmps.Contains(item.EmpId.Value))
                    continue;
                if (item.EmpId.Value == 10)
                    y = 10;
                var vacationDays = _vacationRepository.GetVacationDays(item.EmpId.Value, Con).Result;
                Attendence = Attendence.Where(x => !vacationDays.Contains(x.AttendenceDate)).ToList();
                PassedEmps.Add(item.EmpId.Value);
            }
            return Attendence.ToList();
        }
        public async Task<IEnumerable<AttendenceVM>> GetAllAttendenceSearch(AttendenceVM Search, int BranchId, string Con = "")
        {
            IEnumerable<AttendenceVM> Attendence = null;
            if ((bool)Search.IsSearch)
            {
                //List<int> Emps = _EmployeesRepository.GetAllActiveEmpsByDate(Search.StartDate, Search.EndDate);
                Attendence = await _AttendenceRepository.GetAllAttendenceBySearchObject(Search, BranchId);
                //Attendence = Attendence.Where(x => Emps.Contains(x.EmpId.Value));
            }
            else
            {
                Attendence =await _AttendenceRepository.GetAllAttendenceSearch(BranchId);
            }

            List<int> PassedEmps = new List<int>();
            foreach (var item in Attendence)
            {
                int y;
                if (PassedEmps.Contains(item.EmpId.Value))
                    continue;
                if (item.EmpId.Value == 10)
                    y = 10;
                var vacationDays = _vacationRepository.GetVacationDays(item.EmpId.Value, Con).Result;
                Attendence = Attendence.Where(x => !vacationDays.Contains(x.AttendenceDate)).ToList();
                PassedEmps.Add(item.EmpId.Value);
            }
            return Attendence.ToList();
        }

        public async Task<IEnumerable<AttendenceVM>> GetAllAttendence_Device(int BranchId)
        {
            var Attendence = _AttendenceRepository.GetAllAttendence(BranchId).Result;
            return Attendence.ToList();
        }

        public GeneralMessage SaveAttendence(Attendence attendence, int UserId, int BranchId)
        {
            try
            {
                if (attendence.AttendenceId == 0)
                {
                    attendence.BranchId = BranchId;
                    attendence.AddUser = UserId;
                    attendence.AddDate = DateTime.Now;
                    _TaamerProContext.Attendence.Add(attendence);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة حضور جديد";
                   _SystemAction.SaveAction("SaveAttendence", "AttendenceService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var AttendenceUpdated = _AttendenceRepository.GetById(attendence.AttendenceId);
                    if (AttendenceUpdated != null)
                    {
                        AttendenceUpdated.EmpId = attendence.EmpId;
                        AttendenceUpdated.Day = attendence.Day;
                        AttendenceUpdated.CheckIn = attendence.CheckIn;
                        AttendenceUpdated.CheckOut = attendence.CheckOut;
                        AttendenceUpdated.IsLate = attendence.IsLate;
                        AttendenceUpdated.LateDuration = attendence.LateDuration;
                        AttendenceUpdated.IsOverTime = attendence.IsOverTime;
                        AttendenceUpdated.SameDate = attendence.SameDate;
                        AttendenceUpdated.IsDone = attendence.IsDone;
                        AttendenceUpdated.AttendenceDate = attendence.AttendenceDate;
                        AttendenceUpdated.AttendenceHijriDate = attendence.AttendenceHijriDate;
                        AttendenceUpdated.UpdateUser = UserId;

                        AttendenceUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل حضور رقم " + attendence.AttendenceId;
                   _SystemAction.SaveAction("SaveAttendence", "AttendenceService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };
                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الحضور";
                _SystemAction.SaveAction("SaveAttendence", "AttendenceService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage SaveAttendence_N(Attendence attendence, int UserId, int BranchId)
        {
            try
            {
                DateTime attenDate = DateTime.ParseExact(attendence.AttendenceDate, "yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //int AttTimeId = _AttendaceTimeRepository.GetAll().Where(x => x.BranchId == BranchId && x.IsDeleted == false).FirstOrDefault().TimeId;
                var Emp = _TaamerProContext.Employees.Where(x=>x.EmployeeId==attendence.EmpId).FirstOrDefault();

                if (!string.IsNullOrEmpty(Emp.EndWorkDate) && string.IsNullOrEmpty(Emp.WorkStartDate))
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = string.Format("حساب الموظف {0} موقوف", Emp.EmployeeNameEn) };


                int AttTimeId = Emp.DawamId ?? 0;

                int BeforeLogin = (Emp.EarlyLogin ?? 0) * -1;
                int TimeDurationLate = (Emp.TimeDurationLate ?? 0) * -1;
                int LogoutDuration = (Emp.LogoutDuration ?? 0) * -1;
                int AfterLogoutDuration = (Emp.AfterLogoutTime ?? 0);


                if (AttTimeId == 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ الحضور";
                   _SystemAction.SaveAction("SaveAttendence_N", "AttendenceService", 1, string.Format("يرجى إدخال دوام الموظف {0}", Emp.EmployeeNameEn), "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = string.Format("يرجى إدخال دوام الموظف {0}", Emp.EmployeeNameEn) };
                }

                AttTimeDetailsVM AttTimeDetail = _AttTimeDetailsRepository.GetAllAttTimeDetails("", Emp.DawamId.Value).Result.Where(x => x.Day == ((((int)attenDate.DayOfWeek + 1) % 7) + 1)).FirstOrDefault();
                if (AttTimeDetail == null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ الحضور";
                    _SystemAction.SaveAction("SaveAttendence_N", "AttendenceService", 1, "لا يمكن إدخال الحضور أو الإنصراف في نهاية الأسبوع", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = "لا يمكن إدخال الحضور أو الإنصراف في نهاية الأسبوع" };

                }

                DateTime FirstInHour, FirstOutHour, SecondInHour, SecondOutHour;
                int Shift = 0;
                FirstInHour = FirstOutHour = SecondInHour = SecondOutHour = DateTime.MinValue;
                DateTime IdealTime;
                int TimeDefference;

                DateTime attHour = attenDate;
                if(attendence.Hour !=null && attendence.Hour != 0)
                {
                    attHour = attHour.AddHours((int)attendence.Hour);

                }
                else
                {
                    attHour = attHour.AddHours(attendence.CheckTime.Hour);

                }
                if (attendence.Minute != null && attendence.Minute != 0)
                {
                    attHour = attHour.AddMinutes((int)attendence.Minute);
                }
                else
                {
                    attHour = attHour.AddMinutes((int)attendence.CheckTime.Minute);

                }


                attendence.CheckTime = attHour;


                DateTime Today = attenDate;
                if (AttTimeDetail._1StFromHour.HasValue)
                {
                    FirstInHour = attenDate;
                    FirstInHour = FirstInHour.AddHours(AttTimeDetail._1StFromHour.Value.Hour);
                    FirstInHour = FirstInHour.AddMinutes(AttTimeDetail._1StFromHour.Value.Minute);

                    FirstInHour = FirstInHour.AddMinutes(BeforeLogin);
                }
                if (AttTimeDetail._1StToHour.HasValue)
                {
                    FirstOutHour = attenDate;
                    FirstOutHour = FirstOutHour.AddHours(AttTimeDetail._1StToHour.Value.Hour);
                    FirstOutHour = FirstOutHour.AddMinutes(AttTimeDetail._1StToHour.Value.Minute);

                    FirstOutHour = FirstOutHour.AddMinutes(LogoutDuration);
                }
                if (AttTimeDetail._2ndFromHour.HasValue)
                {
                    SecondInHour = attenDate;
                    SecondInHour = SecondInHour.AddHours(AttTimeDetail._2ndFromHour.Value.Hour);
                    SecondInHour = SecondInHour.AddMinutes(AttTimeDetail._2ndFromHour.Value.Minute);
                    SecondInHour = SecondInHour.AddMinutes(BeforeLogin);
                }
                if (AttTimeDetail._2ndToHour.HasValue)
                {
                    SecondOutHour = attenDate;
                    SecondOutHour = SecondOutHour.AddHours(AttTimeDetail._2ndToHour.Value.Hour);
                    SecondOutHour = SecondOutHour.AddMinutes(AttTimeDetail._2ndToHour.Value.Minute);

                    SecondOutHour = SecondOutHour.AddMinutes(LogoutDuration);
                }

                bool flag = false;

                //FirstIn : 9:00 - :20 , FirstOut: (17:00 - :20) --> (17:00 + :60) , 
                //secondIn: (20:00 - :20), secondOut: (23:00 - :20) --> (23:00 + 60)


                //att  : 8:45 am --> 16:40 
                int itrat = 0;
                if (itrat == 0)
                {
                    if (FirstInHour != DateTime.MinValue && attHour >= FirstInHour && attHour < FirstOutHour && FirstOutHour != DateTime.MinValue)
                    {
                        flag = true;
                        attendence.Type = 1;
                        Shift = 1;

                        attendence.CheckIn = attendence.CheckTime.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        IdealTime = new DateTime(attendence.CheckTime.Year, attendence.CheckTime.Month, attendence.CheckTime.Day, AttTimeDetail._1StFromHour.Value.Hour, AttTimeDetail._1StFromHour.Value.Minute, 0);
                        if (attendence.CheckTime < IdealTime)
                        {
                            //TimeDefference = (int)(attendence.CheckTime.AddMinutes(TimeDurationLate).Subtract(IdealTime).TotalSeconds / 60);
                            //TimeDefference = (TimeDefference >= TimeDurationLate && TimeDefference < 0) ? TimeDefference = 0 : TimeDefference;
                            //attendence.MoveTime = TimeDefference;
                            attendence.MoveTime = 0;

                        }
                        else
                        {
                            TimeDefference = (int)(attendence.CheckTime.AddMinutes(TimeDurationLate).Subtract(IdealTime).TotalSeconds / 60);
                            TimeDefference = (TimeDefference >= TimeDurationLate && TimeDefference < 0) ? TimeDefference = 0 : TimeDefference;
                            attendence.MoveTime = TimeDefference;
                        }
                        itrat = 1;
                    }

                }
                if (itrat == 0)
                {
                    // 16:40 -->  18  = (16:40 + ((+20) + 60))
                    if (attHour >= FirstOutHour && FirstOutHour != DateTime.MinValue && attHour <= FirstOutHour.AddMinutes((LogoutDuration * -1) + AfterLogoutDuration))
                    {
                        flag = true;
                        attendence.Type = 2;
                        attendence.CheckOut = attendence.CheckTime ;
                        IdealTime = new DateTime(attendence.CheckTime.Year, attendence.CheckTime.Month, attendence.CheckTime.Day, AttTimeDetail._1StToHour.Value.Hour, AttTimeDetail._1StToHour.Value.Minute, 0);
                        TimeDefference = (int)(IdealTime.Subtract(attendence.CheckTime).TotalSeconds / 60);
                        attendence.MoveTime = TimeDefference;

                        Shift = 1;
                        itrat = 1;
                    }

                }
                if (itrat == 0)
                {
                    //att:   (19: 40) --> 22:40
                    if ((SecondOutHour.ToString("tt") == "ص" || SecondOutHour.ToString("tt") == "AM") && SecondOutHour < FirstInHour)
                    {
                        if (SecondInHour != DateTime.MinValue && attHour >= SecondInHour && SecondOutHour != DateTime.MinValue)
                        {
                            flag = true;
                            attendence.Type = 1;
                            attendence.CheckIn = attendence.CheckTime.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            IdealTime = new DateTime(attendence.CheckTime.Year, attendence.CheckTime.Month, attendence.CheckTime.Day, AttTimeDetail._2ndFromHour.Value.Hour, AttTimeDetail._2ndFromHour.Value.Minute, 0);
                            if (attendence.CheckTime < IdealTime)
                            {
                                //TimeDefference = (int)(attendence.CheckTime.AddMinutes(TimeDurationLate).Subtract(IdealTime).TotalSeconds / 60);
                                //TimeDefference = (TimeDefference >= TimeDurationLate && TimeDefference < 0) ? TimeDefference = 0 : TimeDefference;
                                //attendence.MoveTime = TimeDefference;
                                attendence.MoveTime = 0;

                            }
                            else
                            {
                                TimeDefference = (int)(attendence.CheckTime.AddMinutes(TimeDurationLate).Subtract(IdealTime).TotalSeconds / 60);
                                TimeDefference = (TimeDefference >= TimeDurationLate && TimeDefference < 0) ? TimeDefference = 0 : TimeDefference;
                                attendence.MoveTime = TimeDefference;
                            }
                            //TimeDefference = (int)(attendence.CheckTime.AddMinutes(TimeDurationLate).Subtract(IdealTime).TotalSeconds / 60);
                            //TimeDefference = (TimeDefference >= TimeDurationLate && TimeDefference < 0) ? TimeDefference = 0 : TimeDefference;
                            //attendence.MoveTime = TimeDefference;
                            Shift = 2;
                            itrat = 1;
                        }
                    }
                    if (SecondInHour != DateTime.MinValue && attHour >= SecondInHour && attHour < SecondOutHour && SecondOutHour != DateTime.MinValue)
                    {

                        flag = true;
                        attendence.Type = 1;
                        attendence.CheckIn = attendence.CheckTime.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        IdealTime = new DateTime(attendence.CheckTime.Year, attendence.CheckTime.Month, attendence.CheckTime.Day, AttTimeDetail._2ndFromHour.Value.Hour, AttTimeDetail._2ndFromHour.Value.Minute, 0);
                        //TimeDefference = (int)(attendence.CheckTime.AddMinutes(TimeDurationLate).Subtract(IdealTime).TotalSeconds / 60);
                        //TimeDefference = (TimeDefference >= TimeDurationLate && TimeDefference < 0) ? TimeDefference = 0 : TimeDefference;
                        //attendence.MoveTime = TimeDefference;
                        if (attendence.CheckTime < IdealTime)
                        {
                            //TimeDefference = (int)(attendence.CheckTime.AddMinutes(TimeDurationLate).Subtract(IdealTime).TotalSeconds / 60);
                            //TimeDefference = (TimeDefference >= TimeDurationLate && TimeDefference < 0) ? TimeDefference = 0 : TimeDefference;
                            //attendence.MoveTime = TimeDefference;
                            attendence.MoveTime = 0;

                        }
                        else
                        {
                            TimeDefference = (int)(attendence.CheckTime.AddMinutes(TimeDurationLate).Subtract(IdealTime).TotalSeconds / 60);
                            TimeDefference = (TimeDefference >= TimeDurationLate && TimeDefference < 0) ? TimeDefference = 0 : TimeDefference;
                            attendence.MoveTime = TimeDefference;
                        }
                        Shift = 2;
                        itrat = 1;
                    }

                }
                if (itrat == 0)
                {
                    //att 22:40 --> 22:40 + ( +20 + 60) -- > 24
                    if (attHour >= SecondOutHour && SecondOutHour != DateTime.MinValue && attHour <= SecondOutHour.AddMinutes((LogoutDuration * -1) + AfterLogoutDuration))
                    {
                        flag = true;
                        attendence.Type = 2;
                        attendence.CheckOut = attHour;

                        IdealTime = new DateTime(attendence.CheckTime.Year, attendence.CheckTime.Month, attendence.CheckTime.Day, AttTimeDetail._2ndToHour.Value.Hour, AttTimeDetail._2ndToHour.Value.Minute, 0);
                        TimeDefference = (int)(IdealTime.Subtract(attendence.CheckTime).TotalSeconds / 60);
                        attendence.MoveTime = TimeDefference;

                        Shift = 2;
                        itrat = 1;
                    }

                }
                if (flag)
                {
                    var Exits = _TaamerProContext.Attendence.Where(x=>x.RealEmpId==attendence.RealEmpId&&x.AttendenceDate== attendence.AttendenceDate&&x.BranchId== BranchId
                    && x.Type == attendence.Type && x.ShiftTime==Shift).ToList();
                    foreach (var item in Exits)
                    {
                        item.IsDeleted = true;
                    }

                    attendence.RealEmpId = Emp.EmployeeId;
                    attendence.EmpId = Emp.EmployeeId;
                    attendence.ShiftTime = Shift;
                    attendence.Source = attendence.Source == 0 ? 2 : attendence.Source;
                    attendence.CheckType = attendence.Type == 1 ? "دخول" : "خروج";
                    attendence.Hint = "تسجيل حركة من الساعة";

                    attendence.WorkCode = "0";
                    attendence.BranchId = BranchId;
                    attendence.AddUser = UserId;
                    attendence.AddDate = DateTime.Now;
                    attendence.FromApplication = 0;
                    _TaamerProContext.Attendence.Add(attendence);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة حضور جديد";
                   _SystemAction.SaveAction("SaveAttendence_N", "AttendenceService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ الحضور";
                    _SystemAction.SaveAction("SaveAttendence_N", "AttendenceService", 1, Resources.AttendanceDepartureRecordedSpecifiedTimes, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.AttendanceDepartureRecordedSpecifiedTimes };

                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الحضور";
                _SystemAction.SaveAction("SaveAttendence_N", "AttendenceService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage DeleteAttendencesByDate(string DateTime)
        {
            try
            {
                var atts = _TaamerProContext.Attendence.Where(x => x.AttendenceDate == DateTime && x.AddDate != System.DateTime.Now.Date).ToList();
                atts.RemoveRange(0, atts.Count());
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

            }
        }
        public GeneralMessage SaveAttendence_FromDevice(Attendence attendence, int UserId, int BranchId)
        {
            UserId = 1;
            BranchId = 1;
            try
            {
                Attendence attInsert = new Attendence();
                DateTime attenDate = DateTime.ParseExact(attendence.AttendenceDate, "yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

                var Exits = _AttendenceRepository.IsExist(attendence.RealEmpId, attendence.AttendenceDate, attendence.CheckTime.Hour, attendence.BranchId).Result;

                if (Exits)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ الحضور";
                    //SaveAction("SaveAttendence_FromDevice", "AttendenceService", 1, "تم إدخال هذه البيانات من قبل", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.DataHasBeenEnteredBefore };

                }

                var Emp = _TaamerProContext.Employees.Where(x=>x.EmployeeId==attendence.EmpId).FirstOrDefault();

                //if (!string.IsNullOrEmpty(Emp.EndWorkDate) || (string.IsNullOrEmpty(Emp.WorkStartDate) ||
                //    (!string.IsNullOrEmpty(Emp.WorkStartDate) && DateTime.ParseExact(Emp.WorkStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) > attenDate)))
                //{
                //    //-----------------------------------------------------------------------------------------------------------------
                //    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //    string ActionNote = "فشل في حفظ الحضور";
                //    //SaveAction("SaveAttendence_FromDevice", "AttendenceService", 1, "حساب الموظف موقوف", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //    //-----------------------------------------------------------------------------------------------------------------
                //    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.employeesAccountIsSuspended };
                //}

                int AttTimeId = Emp.DawamId ?? 0;

                int BeforeLogin = (Emp.EarlyLogin ?? 0) * -1;
                int TimeDurationLate = (Emp.TimeDurationLate ?? 0) * -1;
                int LogoutDuration = (Emp.LogoutDuration ?? 0) * -1;
                int AfterLogoutDuration = (Emp.AfterLogoutTime ?? 0);
                int earlylogin = (Emp.EarlyLogin ?? 0) * -1;


                if (AttTimeId == 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ الحضور";
                    //SaveAction("SaveAttendence_FromDevice", "AttendenceService", 1, string.Format("يرجى إدخال دوام الموظف {0}", Emp.EmployeeNameEn), "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase= string.Format("يرجى إدخال دوام الموظف {0}", Emp.EmployeeNameEn) };

                }

                AttTimeDetailsVM AttTimeDetail = _AttTimeDetailsRepository.GetAllAttTimeDetails("", Emp.DawamId.Value).Result.Where(x => x.Day == ((((int)attenDate.DayOfWeek + 1) % 7) + 1)).FirstOrDefault();

                //        DateTime FirstInHour, FirstOutHour, SecondInHour, SecondOutHour;
                //        int Shift = 0;
                //        FirstInHour = FirstOutHour = SecondInHour = SecondOutHour = DateTime.MinValue;
                //        DateTime IdealTime;
                //        int TimeDefference;

                //        DateTime attHour = attendence.CheckTime;

                //        DateTime Today = attenDate;
                //        if (AttTimeDetail._1StFromHour.HasValue)
                //        {
                //            FirstInHour = attenDate;
                //            FirstInHour = FirstInHour.AddHours(AttTimeDetail._1StFromHour.Value.Hour);
                //            FirstInHour = FirstInHour.AddMinutes(AttTimeDetail._1StFromHour.Value.Minute);

                //            FirstInHour = FirstInHour.AddMinutes(earlylogin);
                //        }
                //        if (AttTimeDetail._1StToHour.HasValue)
                //        {
                //            FirstOutHour = attenDate;
                //            FirstOutHour = FirstOutHour.AddHours(AttTimeDetail._1StToHour.Value.Hour);
                //            FirstOutHour = FirstOutHour.AddMinutes(AttTimeDetail._1StToHour.Value.Minute);

                //            FirstOutHour = FirstOutHour.AddMinutes(LogoutDuration);
                //        }

                //        if (AttTimeDetail._2ndFromHour.HasValue)
                //        {
                //            SecondInHour = attenDate;
                //            SecondInHour = SecondInHour.AddHours(AttTimeDetail._2ndFromHour.Value.Hour);
                //            SecondInHour = SecondInHour.AddMinutes(AttTimeDetail._2ndFromHour.Value.Minute);
                //            SecondInHour = SecondInHour.AddMinutes(earlylogin);
                //        }
                //        if (AttTimeDetail._2ndToHour.HasValue)
                //        {
                //            SecondOutHour = attenDate;
                //            SecondOutHour = SecondOutHour.AddHours(AttTimeDetail._2ndToHour.Value.Hour);
                //            SecondOutHour = SecondOutHour.AddMinutes(AttTimeDetail._2ndToHour.Value.Minute);

                //            SecondOutHour = SecondOutHour.AddMinutes(LogoutDuration);
                //        }

                //        bool flag = false;

                //        //FirstIn : 9:00 - :20 , FirstOut: (17:00 - :20) --> (17:00 + :60) , 
                //        //secondIn: (20:00 - :20), secondOut: (23:00 - :20) --> (23:00 + 60)

                //        //att  : 8:45 am --> 16:40
                //        if (FirstInHour != DateTime.MinValue && attHour >= FirstInHour && attHour < FirstOutHour && FirstOutHour != DateTime.MinValue)
                //        {
                //            flag = true;
                //            attendence.Type = 1;
                //            Shift = 1;

                //            attendence.CheckIn = attendence.CheckTime.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture);

                //            IdealTime = new DateTime(attendence.CheckTime.Year, attendence.CheckTime.Month, attendence.CheckTime.Day, AttTimeDetail._1StFromHour.Value.Hour, AttTimeDetail._1StFromHour.Value.Minute, 0);
                //            TimeDefference = (int)(attendence.CheckTime.AddMinutes(TimeDurationLate).Subtract(IdealTime).TotalSeconds / 60);
                //            TimeDefference = (TimeDefference >= TimeDurationLate && TimeDefference < 0) ? TimeDefference = 0 : TimeDefference;
                //            attendence.MoveTime = TimeDefference;

                //        }
                //        // 16:40 -->  18  = (16:40 + ((+20) + 60))
                //        if (attHour >= FirstOutHour && FirstOutHour != DateTime.MinValue && attHour < FirstOutHour.AddMinutes((LogoutDuration * -1) + AfterLogoutDuration))
                //        {
                //            flag = true;
                //            attendence.Type = 2;
                //            attendence.CheckOut = attenDate;
                //            IdealTime = new DateTime(attendence.CheckTime.Year, attendence.CheckTime.Month, attendence.CheckTime.Day, AttTimeDetail._1StToHour.Value.Hour, AttTimeDetail._1StToHour.Value.Minute, 0);
                //            TimeDefference = (int)(IdealTime.Subtract(attendence.CheckTime).TotalSeconds / 60);
                //            attendence.MoveTime = TimeDefference;

                //            Shift = 1;
                //        }


                //        //att:   (19: 40) --> 22:40
                //        if (SecondInHour != DateTime.MinValue && attHour >= SecondInHour && attHour < SecondOutHour && SecondOutHour != DateTime.MinValue)
                //        {
                //            flag = true;
                //            attendence.Type = 1;
                //            attendence.CheckIn = attendence.CheckTime.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture);
                //            IdealTime = new DateTime(attendence.CheckTime.Year, attendence.CheckTime.Month, attendence.CheckTime.Day, AttTimeDetail._2ndFromHour.Value.Hour, AttTimeDetail._2ndFromHour.Value.Minute, 0);
                //            TimeDefference = (int)(attendence.CheckTime.AddMinutes(TimeDurationLate).Subtract(IdealTime).TotalSeconds / 60);
                //            TimeDefference = (TimeDefference >= TimeDurationLate && TimeDefference < 0) ? TimeDefference = 0 : TimeDefference;
                //            attendence.MoveTime = TimeDefference;
                //            Shift = 2;
                //        }
                //        //att 22:40 --> 22:40 + ( +20 + 60) -- > 24
                //        if (attHour >= SecondOutHour && SecondOutHour != DateTime.MinValue && attHour <= SecondOutHour.AddMinutes((LogoutDuration * -1) + AfterLogoutDuration))
                //        {
                //            flag = true;
                //            attendence.Type = 2;
                //            attendence.CheckOut = attenDate;

                //            IdealTime = new DateTime(attendence.CheckTime.Year, attendence.CheckTime.Month, attendence.CheckTime.Day, AttTimeDetail._2ndToHour.Value.Hour, AttTimeDetail._2ndToHour.Value.Minute, 0);
                //            TimeDefference = (int)(IdealTime.Subtract(attendence.CheckTime).TotalSeconds / 60);
                //            attendence.MoveTime = TimeDefference;

                //            Shift = 2;
                //        }
                //        if (flag)
                //        {
                //            attendence.EmpId = Emp.EmployeeId;
                //            attendence.ShiftTime = Shift;
                //            attendence.Source = attendence.Source == 0 ? 2 : attendence.Source;
                //            attendence.CheckType = attendence.Type == 1 ? "دخول" : "خروج";
                //            attendence.Hint = "تسجيل حركة من الساعة";

                //            attendence.WorkCode = "0";
                //            attendence.BranchId = BranchId;
                //            attendence.AddUser = UserId;
                //            attendence.AddDate = DateTime.Now;


                //            attInsert.CheckType = attendence.CheckType;
                //            attInsert.CheckTime = attendence.CheckTime;
                //            attInsert.Day = attendence.Day;
                //            attInsert.CheckOut = attendence.CheckOut;

                //            attInsert.CheckIn = attendence.CheckIn;
                //            attInsert.AttendenceDate = attendence.AttendenceDate;
                //            attInsert.AttendenceHijriDate = attendence.AttendenceHijriDate;
                //            attInsert.BranchId = attendence.BranchId;
                //            attInsert.EmpId = attendence.EmpId;
                //            attInsert.Hint = attendence.Hint;
                //            attInsert.MoveTime = attendence.MoveTime;
                //            attInsert.RealEmpId = attendence.EmpId;
                //            attInsert.ShiftTime = attendence.ShiftTime;
                //            attInsert.Source = attendence.Source;
                //            attInsert.Type = attendence.Type;
                //            attInsert.AddUser = attendence.AddUser;
                //            attInsert.WorkCode = attendence.WorkCode;
                //            attInsert.AddDate = DateTime.Now;
                //            attInsert.AddUser = 1;

                //            _TaamerProContext.Attendence.Add(attInsert);
                //            _TaamerProContext.SaveChanges();


                //            //-----------------------------------------------------------------------------------------------------------------
                //            string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //            string ActionNote = "حفظ الحضور";
                //            //SaveAction("SaveAttendence_FromDevice", "AttendenceService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //            //-----------------------------------------------------------------------------------------------------------------
                //            return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                //        }
                //        else
                //        {
                //            //-----------------------------------------------------------------------------------------------------------------
                //            string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //            string ActionNote = "فشل في حفظ الحضور";
                //            //SaveAction("SaveAttendence_FromDevice", "AttendenceService", 1, "لا يمكن تسجيل الحضور أو الإنصراف في غير الأوقات المحددة", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //            //-----------------------------------------------------------------------------------------------------------------
                //            return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.AttendanceDepartureRecordedSpecifiedTimes };

                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //        //-----------------------------------------------------------------------------------------------------------------
                //        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //        string ActionNote = "فشل في حفظ الحضور";
                //        //SaveAction("SaveAttendence_FromDevice", "AttendenceService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //        //-----------------------------------------------------------------------------------------------------------------
                //        return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                //    }
                //}

                DateTime FirstInHour, FirstOutHour, SecondInHour, SecondOutHour;
                int Shift = 0;
                FirstInHour = FirstOutHour = SecondInHour = SecondOutHour = DateTime.MinValue;
                DateTime IdealTime;
                int TimeDefference;

                DateTime attHour = attenDate;
                if (attendence.Hour != null && attendence.Hour != 0)
                {
                    attHour = attHour.AddHours((int)attendence.Hour);

                }
                else
                {
                    attHour = attHour.AddHours(attendence.CheckTime.Hour);

                }
                if (attendence.Minute != null && attendence.Minute != 0)
                {
                    attHour = attHour.AddMinutes((int)attendence.Minute);
                }
                else
                {
                    attHour = attHour.AddMinutes((int)attendence.CheckTime.Minute);

                }


                attendence.CheckTime = attHour;


                DateTime Today = attenDate;
                if (AttTimeDetail._1StFromHour.HasValue)
                {
                    FirstInHour = attenDate;
                    FirstInHour = FirstInHour.AddHours(AttTimeDetail._1StFromHour.Value.Hour);
                    FirstInHour = FirstInHour.AddMinutes(AttTimeDetail._1StFromHour.Value.Minute);

                    FirstInHour = FirstInHour.AddMinutes(BeforeLogin);
                }
                if (AttTimeDetail._1StToHour.HasValue)
                {
                    FirstOutHour = attenDate;
                    FirstOutHour = FirstOutHour.AddHours(AttTimeDetail._1StToHour.Value.Hour);
                    FirstOutHour = FirstOutHour.AddMinutes(AttTimeDetail._1StToHour.Value.Minute);

                    FirstOutHour = FirstOutHour.AddMinutes(LogoutDuration);
                }
                if (AttTimeDetail._2ndFromHour.HasValue)
                {
                    SecondInHour = attenDate;
                    SecondInHour = SecondInHour.AddHours(AttTimeDetail._2ndFromHour.Value.Hour);
                    SecondInHour = SecondInHour.AddMinutes(AttTimeDetail._2ndFromHour.Value.Minute);
                    SecondInHour = SecondInHour.AddMinutes(BeforeLogin);
                }
                if (AttTimeDetail._2ndToHour.HasValue)
                {
                    SecondOutHour = attenDate;
                    SecondOutHour = SecondOutHour.AddHours(AttTimeDetail._2ndToHour.Value.Hour);
                    SecondOutHour = SecondOutHour.AddMinutes(AttTimeDetail._2ndToHour.Value.Minute);

                    SecondOutHour = SecondOutHour.AddMinutes(LogoutDuration);
                }

                bool flag = false;

                //FirstIn : 9:00 - :20 , FirstOut: (17:00 - :20) --> (17:00 + :60) , 
                //secondIn: (20:00 - :20), secondOut: (23:00 - :20) --> (23:00 + 60)


                //att  : 8:45 am --> 16:40 
                int itrat = 0;
                if (itrat == 0)
                {
                    if (FirstInHour != DateTime.MinValue && attHour >= FirstInHour && attHour < FirstOutHour && FirstOutHour != DateTime.MinValue)
                    {
                        flag = true;
                        attendence.Type = 1;
                        Shift = 1;

                        attendence.CheckIn = attendence.CheckTime.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

                        IdealTime = new DateTime(attendence.CheckTime.Year, attendence.CheckTime.Month, attendence.CheckTime.Day, AttTimeDetail._1StFromHour.Value.Hour, AttTimeDetail._1StFromHour.Value.Minute, 0);
                        //TimeDefference = (int)(attendence.CheckTime.AddMinutes(TimeDurationLate).Subtract(IdealTime).TotalSeconds / 60);
                        //TimeDefference = (TimeDefference >= TimeDurationLate && TimeDefference < 0) ? TimeDefference = 0 : TimeDefference;
                        //attendence.MoveTime = TimeDefference;
                        if (attendence.CheckTime < IdealTime)
                        {
                            //TimeDefference = (int)(attendence.CheckTime.AddMinutes(TimeDurationLate).Subtract(IdealTime).TotalSeconds / 60);
                            //TimeDefference = (TimeDefference >= TimeDurationLate && TimeDefference < 0) ? TimeDefference = 0 : TimeDefference;
                            //attendence.MoveTime = TimeDefference;
                            attendence.MoveTime = 0;

                        }
                        else
                        {
                            TimeDefference = (int)(attendence.CheckTime.AddMinutes(TimeDurationLate).Subtract(IdealTime).TotalSeconds / 60);
                            TimeDefference = (TimeDefference >= TimeDurationLate && TimeDefference < 0) ? TimeDefference = 0 : TimeDefference;
                            attendence.MoveTime = TimeDefference;
                        }
                        itrat = 1;
                    }

                }
                if (itrat == 0)
                {
                    // 16:40 -->  18  = (16:40 + ((+20) + 60))
                    if (attHour >= FirstOutHour && FirstOutHour != DateTime.MinValue && attHour <= FirstOutHour.AddMinutes((LogoutDuration * -1) + AfterLogoutDuration))
                    {
                        flag = true;
                        attendence.Type = 2;
                        attendence.CheckOut = attendence.CheckTime;
                        IdealTime = new DateTime(attendence.CheckTime.Year, attendence.CheckTime.Month, attendence.CheckTime.Day, AttTimeDetail._1StToHour.Value.Hour, AttTimeDetail._1StToHour.Value.Minute, 0);
                        TimeDefference = (int)(IdealTime.Subtract(attendence.CheckTime).TotalSeconds / 60);
                        attendence.MoveTime = TimeDefference;

                        Shift = 1;
                        itrat = 1;
                    }

                }
                if (itrat == 0)
                {
                    //att:   (19: 40) --> 22:40
                    if ((SecondOutHour.ToString("tt") == "ص" || SecondOutHour.ToString("tt") == "AM") && SecondOutHour < FirstInHour)
                    {
                        if (SecondInHour != DateTime.MinValue && attHour >= SecondInHour && SecondOutHour != DateTime.MinValue)
                        {
                            flag = true;
                            attendence.Type = 1;
                            attendence.CheckIn = attendence.CheckTime.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            IdealTime = new DateTime(attendence.CheckTime.Year, attendence.CheckTime.Month, attendence.CheckTime.Day, AttTimeDetail._2ndFromHour.Value.Hour, AttTimeDetail._2ndFromHour.Value.Minute, 0);
                            //TimeDefference = (int)(attendence.CheckTime.AddMinutes(TimeDurationLate).Subtract(IdealTime).TotalSeconds / 60);
                            //TimeDefference = (TimeDefference >= TimeDurationLate && TimeDefference < 0) ? TimeDefference = 0 : TimeDefference;
                            //attendence.MoveTime = TimeDefference;
                            if (attendence.CheckTime < IdealTime)
                            {
                                //TimeDefference = (int)(attendence.CheckTime.AddMinutes(TimeDurationLate).Subtract(IdealTime).TotalSeconds / 60);
                                //TimeDefference = (TimeDefference >= TimeDurationLate && TimeDefference < 0) ? TimeDefference = 0 : TimeDefference;
                                //attendence.MoveTime = TimeDefference;
                                attendence.MoveTime = 0;

                            }
                            else
                            {
                                TimeDefference = (int)(attendence.CheckTime.AddMinutes(TimeDurationLate).Subtract(IdealTime).TotalSeconds / 60);
                                TimeDefference = (TimeDefference >= TimeDurationLate && TimeDefference < 0) ? TimeDefference = 0 : TimeDefference;
                                attendence.MoveTime = TimeDefference;
                            }
                            Shift = 2;
                            itrat = 1;
                        }
                    }
                    if (SecondInHour != DateTime.MinValue && attHour >= SecondInHour && attHour < SecondOutHour && SecondOutHour != DateTime.MinValue)
                    {

                        flag = true;
                        attendence.Type = 1;
                        attendence.CheckIn = attendence.CheckTime.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        IdealTime = new DateTime(attendence.CheckTime.Year, attendence.CheckTime.Month, attendence.CheckTime.Day, AttTimeDetail._2ndFromHour.Value.Hour, AttTimeDetail._2ndFromHour.Value.Minute, 0);
                        //TimeDefference = (int)(attendence.CheckTime.AddMinutes(TimeDurationLate).Subtract(IdealTime).TotalSeconds / 60);
                        //TimeDefference = (TimeDefference >= TimeDurationLate && TimeDefference < 0) ? TimeDefference = 0 : TimeDefference;
                        //attendence.MoveTime = TimeDefference;
                        if (attendence.CheckTime < IdealTime)
                        {
                            //TimeDefference = (int)(attendence.CheckTime.AddMinutes(TimeDurationLate).Subtract(IdealTime).TotalSeconds / 60);
                            //TimeDefference = (TimeDefference >= TimeDurationLate && TimeDefference < 0) ? TimeDefference = 0 : TimeDefference;
                            //attendence.MoveTime = TimeDefference;
                            attendence.MoveTime = 0;

                        }
                        else
                        {
                            TimeDefference = (int)(attendence.CheckTime.AddMinutes(TimeDurationLate).Subtract(IdealTime).TotalSeconds / 60);
                            TimeDefference = (TimeDefference >= TimeDurationLate && TimeDefference < 0) ? TimeDefference = 0 : TimeDefference;
                            attendence.MoveTime = TimeDefference;
                        }
                        Shift = 2;
                        itrat = 1;
                    }

                }
                if (itrat == 0)
                {
                    //att 22:40 --> 22:40 + ( +20 + 60) -- > 24
                    if (attHour >= SecondOutHour && SecondOutHour != DateTime.MinValue && attHour <= SecondOutHour.AddMinutes((LogoutDuration * -1) + AfterLogoutDuration))
                    {
                        flag = true;
                        attendence.Type = 2;
                        attendence.CheckOut = attHour;

                        IdealTime = new DateTime(attendence.CheckTime.Year, attendence.CheckTime.Month, attendence.CheckTime.Day, AttTimeDetail._2ndToHour.Value.Hour, AttTimeDetail._2ndToHour.Value.Minute, 0);
                        TimeDefference = (int)(IdealTime.Subtract(attendence.CheckTime).TotalSeconds / 60);
                        attendence.MoveTime = TimeDefference;

                        Shift = 2;
                        itrat = 1;
                    }

                }
                if (flag)
                {
                    //var Exitss = _TaamerProContext.Attendence.Where(x => x.RealEmpId == attendence.RealEmpId && x.AttendenceDate == attendence.AttendenceDate && x.BranchId == BranchId
                    //&& x.Type == attendence.Type && x.ShiftTime == Shift).ToList();
                    ////foreach (var item in Exitss)
                    ////{
                    ////    item.IsDeleted = true;
                    ////}

                    attendence.RealEmpId = Emp.EmployeeId;
                    attendence.EmpId = Emp.EmployeeId;
                    attendence.ShiftTime = Shift;
                    attendence.Source = attendence.Source == 0 ? 2 : attendence.Source;
                    attendence.CheckType = attendence.Type == 1 ? "دخول" : "خروج";
                    attendence.Hint = "تسجيل حركة من الساعة";

                    attendence.WorkCode = "0";
                    attendence.BranchId = BranchId;
                    attendence.AddUser = UserId;
                    attendence.AddDate = DateTime.Now;
                    attendence.FromApplication = 0;
                    _TaamerProContext.Attendence.Add(attendence);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة حضور جديد";
                    _SystemAction.SaveAction("SaveAttendence_N", "AttendenceService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ الحضور";
                    _SystemAction.SaveAction("SaveAttendence_N", "AttendenceService", 1, Resources.AttendanceDepartureRecordedSpecifiedTimes, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.AttendanceDepartureRecordedSpecifiedTimes };

                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الحضور";
                _SystemAction.SaveAction("SaveAttendence_N", "AttendenceService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public GeneralMessage SaveListAttendence(List<Attendence> attendence, int UserId, int BranchId)
        {
            try
            {
                foreach (var row in attendence)
                {
                    if (row.AttendenceId == 0)
                    {
                        row.BranchId = BranchId;
                        row.AddUser = UserId;
                        row.AddDate = DateTime.Now;
                        _TaamerProContext.Attendence.Add(row);
                    }

                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة حضور";
               _SystemAction.SaveAction("SaveListAttendence", "AttendenceService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الحضور";
                _SystemAction.SaveAction("SaveListAttendence", "AttendenceService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage ConFirmMonthAttendence(Attendence attendence, int UserId, int BranchId, string Lang)
        {
            try
            {
                DateTime DateMonth = DateTime.ParseExact(attendence.Month, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var AllAttendess = _TaamerProContext.Attendees.Where(s => s.Date == attendence.Month && s.BranchId == BranchId);
                if (AllAttendess.Count() > 0)
                {
                    var messageBefore = "";
                    var messageBefore3 = "تم اعتماد شهر " + DateMonth.ToString("MMMM", CultureInfo.CreateSpecificCulture("en")) + " لسنة " + DateMonth.Year + " من قبل";

                    if (Lang == "rtl")
                    {
                        messageBefore = "تم اعتماد شهر " + DateMonth.ToString("MMMM", CultureInfo.CreateSpecificCulture("en")) + " لسنة " + DateMonth.Year + " من قبل";
                    }
                    else if (Lang == "ltr")
                    {
                        messageBefore = "A month was adopted " + DateMonth.ToString("MMMM", CultureInfo.CreateSpecificCulture("en")) + " For Year " + DateMonth.Year + " Before";
                    }
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "فشل في اعتماد الشهر";
                    _SystemAction.SaveAction("ConFirmMonthAttendence", "AttendenceService", 1, messageBefore3, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = messageBefore };
                }
                var MontDayesDate = GetDates(DateMonth.Year, DateMonth.Month);
                var AttTimeSett = _TaamerProContext.AttendaceTime.Where(s => s.IsDeleted == false && s.BranchId == BranchId).FirstOrDefault();
                var allemps = _TaamerProContext.Employees.Where(s => s.IsDeleted == false && s.BranchId == BranchId);
                foreach (var emp in allemps)
                {
                    var EmpAttendence = _TaamerProContext.Attendence.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.EmpId == emp.EmployeeId).ToList();
                    List<Attendees> attendees = new List<Attendees>();
                    foreach (var dayDate in MontDayesDate)
                    {
                        var checkinout = EmpAttendence.FirstOrDefault(a => DateTime.ParseExact(a.AttendenceDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) == dayDate);
                        var matchedDay = AttTimeSett.AttTimeDetails.FirstOrDefault(s => GetWeekDayName(s.Day) == dayDate.DayOfWeek.ToString());
                        if (checkinout != null)
                        {
                            attendees.Add(new Attendees
                            {
                                Date = dayDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")),
                                DayOfWeek = dayDate.DayOfWeek.ToString(),
                                Day = GetWeekDay(dayDate.DayOfWeek.ToString()),
                                EmpId = emp.EmployeeId,
                                Status = GetAttendenceStatus(dayDate, checkinout),
                                WorkMinutes = GetWorkMinutes(checkinout),
                                ActualWorkMinutes = GetActualWorkMinutes(matchedDay, AttTimeSett),
                                IsLate = (GetWorkMinutes(checkinout) < GetActualWorkMinutes(matchedDay, AttTimeSett) && GetWorkMinutes(checkinout) != 0) ? true : false,
                                Discount = GetEmpDiscount(emp.EmployeeId, dayDate, matchedDay, AttTimeSett, checkinout),
                                IsOverTime = (GetWorkMinutes(checkinout) > GetActualWorkMinutes(matchedDay, AttTimeSett) && GetWorkMinutes(checkinout) != 0) ? true : false,
                                IsLateCheckIn = false,
                                IsEarlyCheckOut = checkinout.CheckOut < matchedDay._1StToHour ? true : false,
                                IsEntry = checkinout.CheckIn == null ? true : false,
                                IsOut = checkinout.CheckOut == null ? true : false,
                                IsDone = true,
                                IsDeleted = false,
                                AddDate = DateTime.Now,
                                AddUser = UserId,
                                BranchId = BranchId,
                            });// checkinout.CheckIn > matchedDay._1StFromHour ? true :
                        }
                        else
                        {
                            attendees.Add(new Attendees
                            {
                                Date = dayDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")),
                                DayOfWeek = dayDate.DayOfWeek.ToString(),
                                Day = GetWeekDay(dayDate.DayOfWeek.ToString()),
                                EmpId = emp.EmployeeId,
                                Discount = GetEmpDaySal(emp.EmployeeId),
                                Status = 2,
                                ActualWorkMinutes = GetActualWorkMinutes(matchedDay, AttTimeSett),
                                IsDone = true,
                                IsDeleted = false,
                                AddDate = DateTime.Now,
                                AddUser = UserId,
                                BranchId = BranchId,
                            });
                        }
                    }
                    _TaamerProContext.Attendees.AddRange(attendees);
                }
                _TaamerProContext.SaveChanges();
                var message = "";
                var message3 = "تم اعتماد شهر " + DateMonth.ToString("MMMM", CultureInfo.CreateSpecificCulture("en")) + " لسنة " + DateMonth.Year + " بنجاح";

                if (Lang == "rtl")
                {
                    message = "تم اعتماد شهر " + DateMonth.ToString("MMMM", CultureInfo.CreateSpecificCulture("en")) + " لسنة " + DateMonth.Year + " بنجاح";
                }
                else if (Lang == "ltr")
                {
                    message = "A month was adopted " + DateMonth.ToString("MMMM", CultureInfo.CreateSpecificCulture("en")) + " For Year " + DateMonth.Year + " Succussfully";
                }
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تم اعتماد الشهر";
                _SystemAction.SaveAction("ConFirmMonthAttendence", "AttendenceService", 1, message3, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = message };
            }
            catch (Exception ex)
            {
                var message = "";
                var message3 = "فشل في اعتماد حضور وانصراف الشهر";
                if (Lang == "rtl")
                {
                    message = "فشل في اعتماد حضور وانصراف الشهر";
                }
                else if (Lang == "ltr")
                {
                    message = "Failed to approve the attendance and leave of the month";
                }
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في الاعتماد";
                _SystemAction.SaveAction("ConFirmMonthAttendence", "AttendenceService", 1, message3, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = message };
            }
        }
        public GeneralMessage DeleteAttendence(int AttendenceId, int UserId, int BranchId)
        {
            try
            {
                Attendence Atten = _AttendenceRepository.GetById(AttendenceId);
                Atten.IsDeleted = true;
                Atten.DeleteDate = DateTime.Now;
                Atten.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف حضور رقم " + AttendenceId;
                _SystemAction.SaveAction("DeleteAttendence", "AttendenceService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف حضور رقم " + AttendenceId; ;
                _SystemAction.SaveAction("DeleteAttendence", "AttendenceService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        private static List<DateTime> GetDates(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                             .Select(day => new DateTime(year, month, day)) // Map each day to a date
                             .ToList(); // Load dates into a list
        }
        private static int GetAttendenceStatus(DateTime dayDate, Attendence attendaceTime)
        {
            if (DateTime.Compare(dayDate, DateTime.ParseExact(attendaceTime.AttendenceDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) == 0 && attendaceTime.CheckOut != null)
            {
                return 1; // حضور
            }
            else
            {
                return 2; // غياب
            }
        }
        private static int GetWorkMinutes(Attendence attendace)
        {
            if (attendace.CheckIn != null && attendace.CheckOut != null)
            {
                TimeSpan span = (attendace.CheckOut ?? DateTime.Now).Subtract(DateTime.Now);
                return Convert.ToInt32(span.TotalMinutes);
            }
            return 0;
        }
        private static int GetActualWorkMinutes(AttTimeDetails matchedDay, AttendaceTime time)
        {
            if (matchedDay._2ndFromHour == null && matchedDay._2ndToHour == null)  // دوام واحد
            {
                TimeSpan span = (matchedDay._1StToHour ?? DateTime.Now).Subtract(matchedDay._1StFromHour ?? DateTime.Now);
                return Convert.ToInt32(span.TotalMinutes);
            }
            else // اتنين دوام في اليوم
            {
                TimeSpan span1 = (matchedDay._1StToHour ?? DateTime.Now).Subtract(matchedDay._1StFromHour ?? DateTime.Now);
                TimeSpan span2 = (matchedDay._2ndToHour ?? DateTime.Now).Subtract(matchedDay._2ndFromHour ?? DateTime.Now);
                return Convert.ToInt32(span1.TotalMinutes + span2.TotalMinutes);
            }
        }

        public static string GetWeekDayName(int Day)
        {
            switch (Day)
            {
                case 1:
                    return "Saturday";
                case 2:
                    return "Sunday";
                case 3:
                    return "Monday";
                case 4:
                    return "Tuesday";
                case 5:
                    return "Wednesday";
                case 6:
                    return "Thursday";
                case 7:
                    return "Friday";
                default:
                    return "Other";
            }
        }
        public static int GetWeekDay(string Day)
        {
            switch (Day)
            {
                case "Saturday":
                    return 1;
                case "Sunday":
                    return 2;
                case "Monday":
                    return 3;
                case "Tuesday":
                    return 4;
                case "Wednesday":
                    return 5;
                case "Thursday":
                    return 6;
                case "Friday":
                    return 7;
                default:
                    return 0;
            }
        }
        public decimal GetEmpDiscount(int EmpId, DateTime dayDate, AttTimeDetails matchedDay, AttendaceTime time, Attendence checkinout)
        {
            var Status = GetAttendenceStatus(dayDate, checkinout);
            if (Status == 1)
            {
                return (GetEmpDaySal(EmpId) / GetActualWorkMinutes(matchedDay, time)) * (GetActualWorkMinutes(matchedDay, time) - GetWorkMinutes(checkinout));
            }
            return 0;
        }
        public decimal GetEmpDaySal(int EmpId)
        {
            var employee = _TaamerProContext.Employees.Where(x=>x.EmployeeId==EmpId).FirstOrDefault();
            decimal monthSalary = employee.Salary ?? 0 + employee.Rewards ?? 0;
            var daySal = monthSalary / 30;
            return daySal;
        }
        public async Task<IEnumerable<AttendenceVM>> EmpAttendenceSearch(AttendenceVM AttendenceSearch, int BranchId)
        {
            //List<int> Emps = _EmployeesRepository.GetAllActiveEmpsByDate(AttendenceSearch.StartDate, AttendenceSearch.EndDate);
            var Attendence = await _AttendenceRepository.EmpAttendenceSearch(AttendenceSearch, BranchId);
            //Attendence = Attendence.Where(x => Emps.Contains(x.EmpId.Value)).ToList();
            return Attendence.ToList();
        }

        public IEnumerable<AbsenceVM> GetAbsenceData(string FromDate, string ToDate, int EmpId, int BranchId, string lang, string Con, int? yearid)
        {
            if (yearid != null)
            {
                var result = _AttendenceRepository.GetAbsenceData(FromDate, ToDate, EmpId, yearid, BranchId, lang, Con).Result;
                return result;
            }
            return new List<AbsenceVM>();
        }

        public IEnumerable<EmployeesVM> GetAbsentEmployeesNote(int UserId, int BranchId, string Lang, string Con)
        {
            var user = _TaamerProContext.Users.Where(x => x.UserId == UserId);
            //var priv = _userNotificationPrivilegesRepository.GetUsersByPrivilegesIds(UserId);// _NotifprivilegesService.GetPrivilegesIdsByUserId(UserId)
            //if (priv.Contains(172))
            DateTime Today = DateTime.Now;
            string From = new DateTime(Today.Year, Today.Month, 1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string To = new DateTime(Today.Year, Today.Month, DateTime.DaysInMonth(Today.Year, Today.Month)).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            var result = this.GetAbsenceData(From, To, 0, BranchId, Lang, Con, Today.Year).ToList();
            if (result.Count() > 0)
            {
                var EmpsInVaction = _employeesRepository.GetEmpNosInVacations().Result;
                result = result.Where(x => !EmpsInVaction.Contains(x.EmpNo)).ToList();
            }
            var moreThan10 = (from p in result
                              group p by new { p.EmpNo, p.E_FullName } into g
                              select new EmployeesVM
                              {
                                  EmployeeId = int.Parse(g.Key.EmpNo),
                                  EmployeeNameAr = g.Key.E_FullName,
                                  TotalDayAbs = g.Count(p => p.Mdate != null)
                              }).Where(x => x.TotalDayAbs >= 10).OrderBy(x => x.TotalDayAbs);


            return moreThan10;

        }

        public GeneralMessage InsertAbsentEmpSee(int UserId, int BranchId)
        {
            try
            {
                ProjectArchivesSee ProSee = new ProjectArchivesSee();
                ProSee.ProArchReID = 0;
                ProSee.UserId = UserId;
                ProSee.Status = true;
                ProSee.AddDate = DateTime.Now;
                _TaamerProContext.ProjectArchivesSee.Add(ProSee);
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة غياب ";
                _SystemAction.SaveAction("InsertAbsentEmpSee", "AttendenceService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = "Resources.General_SavedSuccessfully" };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الغياب";
                _SystemAction.SaveAction("InsertAbsentEmpSee", "AttendenceService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "Resources.General_SavedFailed" };
            }
        }

        public async Task<IEnumerable<AbsenceVM>> GetAbsenceDataToday(string TodayDate, int BranchId, string lang, string Con, int? yearid)
        {

            if (yearid != null)
            {
                return await _AttendenceRepository.GetAbsenceDataToday(TodayDate, yearid, BranchId, lang, Con);
            }
            return new List<AbsenceVM>();


        }


        public async Task<IEnumerable<LateVM>> GetLateData(string FromDate, string ToDate, int EmpId, int Shift, int BranchId, string lang, string Con, int? yearid)
        {

            if (yearid != null)
            {
                return await _AttendenceRepository.GetLateData(FromDate, ToDate, EmpId, yearid, Shift, BranchId, lang, Con);
            }
            return new List<LateVM>();


        }

        public string GetLateData_2(string FromDate, string ToDate, int EmpId, int Shift, int BranchId, string lang, string Con, int? yearid)
        {

            if (yearid != null)
            {
                var late = _AttendenceRepository.GetLateData(FromDate, ToDate, EmpId, yearid, Shift, BranchId, lang, Con).Result;
                TimeSpan t1 = TimeSpan.Parse("00:00");
                foreach (var item in late)
                {

                    t1 = t1.Add(TimeSpan.Parse(item.MoveTimeStringJoin1));

                }
                return t1.ToString();
            }
            return "00:00";


        }

        public async Task<IEnumerable<LateVM>> GetLateDataToday(string TodayDate, int Shift, int BranchId, string lang, string Con, int? yearid)
        {

            if (yearid != null)
            {
                return await _AttendenceRepository.GetLateDataToday(TodayDate, yearid, Shift, BranchId, lang, Con);
            }
            return new List<LateVM>();


        }



        public async Task<IEnumerable<LateVM>> GetEarlyDepartureData(string FromDate, string ToDate, int EmpId, int Shift, int BranchId, string lang, string Con, int? yearid)
        {

            if (yearid != null)
            {
                return await _AttendenceRepository.GetEarlyDepartureData(FromDate, ToDate, EmpId, yearid, Shift, BranchId, lang, Con);
            }
            return new List<LateVM>();


        }

        public async Task<IEnumerable<LateVM>> GetEarlyDepartureDataToday(string TodayDate, int Shift, int BranchId, string lang, string Con, int? yearid)
        {

            if (yearid != null)
            {
                return await _AttendenceRepository.GetEarlyDepartureDataToday(TodayDate, yearid, Shift, BranchId, lang, Con);
            }
            return new List<LateVM>();


        }

        public async Task<IEnumerable<NotLoggedOutVM>> GetNotLoggedOutData(string FromDate, string ToDate, int BranchId, string lang, string Con, int? yearid)
        {

            if (yearid != null)
            {
                return await _AttendenceRepository.GetNotLoggedOutData(FromDate, ToDate, yearid, BranchId, lang, Con);
            }
            return new List<NotLoggedOutVM>();


        }

        public async Task<IEnumerable<LateVM>> GetAttendanceData(string FromDate, string ToDate, int Shift, int BranchId, string lang, string Con, int? yearid)
        {

            if (yearid != null)
            {
                return await _AttendenceRepository.GetAttendanceData(FromDate, ToDate, yearid, Shift, BranchId, lang, Con);
            }
            return new List<LateVM>();


        }
        public async Task<IEnumerable<LateVM>> GetAttendanceData(string FromDate, string ToDate, int Shift, int BranchId, string lang, string Con, int? yearid, int pageNumber, int pageSize)
        {

            if (yearid != null)
            {
                return await _AttendenceRepository.GetAttendanceData(FromDate, ToDate, yearid, Shift, BranchId, lang, Con, pageNumber, pageSize);
            }
            return new List<LateVM>();


        }
        public async Task<IEnumerable<LateVM>> GetAttendanceData_Application(string FromDate, string ToDate, int Shift, int BranchId, string lang, string Con, int? yearid)
        {

            if (yearid != null)
            {
                if (Shift == 1)
                {
                    return  _AttendenceRepository.GetAttendanceData_Application(FromDate, ToDate, yearid, Shift, BranchId, lang, Con).Result.Where(s => (s.TimeJoin1 != "--" && s.TimeJoin1 != "") || (s.TimeLeave1 != "--" && s.TimeLeave1 != ""));

                }
                else if (Shift == 2)
                {
                    return  _AttendenceRepository.GetAttendanceData_Application(FromDate, ToDate, yearid, Shift, BranchId, lang, Con).Result.Where(s => (s.TimeJoin2 != "--" && s.TimeJoin2 != "") || (s.TimeLeave2 != "--" && s.TimeLeave2 != ""));

                }
                else
                {
                    return await _AttendenceRepository.GetAttendanceData_Application(FromDate, ToDate, yearid, Shift, BranchId, lang, Con);
                }
            }
            return new List<LateVM>();

        }

        public async Task<List<LateVM>> chckdata(List<LateVM> lmd,int swtype)
        {
            List <LateVM> newlmd =new List<LateVM>();
            var yesterday = DateTime.Now;
            yesterday = yesterday.AddDays(-1);
            var DayofWeek = GetWeekDay(yesterday.DayOfWeek.ToString());

            var tepyesterday = DateTime.Now;
            tepyesterday = tepyesterday.AddDays(-1);
            //var IsWorkDay="";
            //var IsHoliday="";
            foreach (var item in lmd)
            {
                if (swtype == 1)
                {

                    if (item.DawamId == "")
                    {
                        var msg = "يرجى ضبط الدوام للموظف: " + item.FullName;

                    }
                    else
                    {

                        var dawamint = Convert.ToInt16(item.DawamId);
                        var attdetails = await _AttTimeDetailsRepository.GetAllAttTimeDetailsByid(dawamint);

                        var StartWorkDate = DateTime.Parse(item.StartWorkDate);
                        var IsStarted = false;
                        if (yesterday > StartWorkDate || yesterday.Date == StartWorkDate.Date) //CurrentDate >= StartWorkDate
                            IsStarted = true;
                        var dd = ((DayofWeek + 9) % 7);
                        var IsWorkDay = attdetails.Where(x => x.Day == dd).ToList();
                        var officalhol = await _icalHolidayRepository.GetAllOfficalHoliday();

                        var IsHoliday = officalhol.Where(x => x.FromDate <= tepyesterday && x.ToDate >= tepyesterday).ToList();
                        var haspermission = _TaamerProContext.Permissions.Where(x => x.IsDeleted == false && x.EmpId.ToString() == item.EmpId && x.Date == tepyesterday.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"))).ToList();
                        item.Isholiday = IsHoliday;
                        item.Isworkday = IsWorkDay;

                        var status = "";

                        ////////////////////////////////////////regin attendence status/////////////////////////////////////////////
                        if (IsHoliday.Count() != 0)
                        {
                            status = "ع";
                        }
                        else if (IsWorkDay.Count() == 0)
                        {
                            status = "ن";
                        }
                        else
                            if (!IsStarted)
                        {
                            status = "x";
                        }
                        else if(haspermission !=null && haspermission.Count() > 0)
                        {
                            status = "ذ";
                        }
                        else
                         if (item.MAXSER == "")
                        {
                            status = "غ";
                        }

                        else
                        {
                            status = "ح";
                        }
                        item.status = status;

                    }
                }
                else if (swtype == 2)
                {
                    var status = "";

                    var ToDay = DateTime.Now;
                    var DayofWeek2 = GetWeekDay(ToDay.DayOfWeek.ToString());
                    var TempToDay = DateTime.Now;
                    //var IsWorkDay;
                    //var IsHoliday;

                    var row = "";
                    var IsEdit = "False";
                    var dawaamErrMsg = "يرجى ضبط الدوام للموظف: ";
                    var dawaamErrExists = false;
                    if (item.DawamId == "")
                    {
                        dawaamErrMsg = dawaamErrMsg + " ," + item.FullName;
                        dawaamErrExists = true;

                    }
                    else
                    {

                        var dawamint = Convert.ToInt16(item.DawamId);

                        var attdetails = await _AttTimeDetailsRepository.GetAllAttTimeDetailsByid(dawamint);

                        var StartWorkDate = DateTime.Parse(item.StartWorkDate);

                        var IsStarted = false;
                        if (yesterday > StartWorkDate || yesterday.Date == StartWorkDate.Date) //CurrentDate >= StartWorkDate
                            IsStarted = true;

                        //var dd = ((DayofWeek2 + 2) % 7);
                        var dd = ((DayofWeek2));

                        var IsWorkDay = attdetails.Where(x => x.Day == dd).ToList();

                        var officalhol = await _icalHolidayRepository.GetAllOfficalHoliday();

                        var IsHoliday = officalhol.Where(x => x.FromDate <= TempToDay && x.ToDate >= TempToDay).ToList();

                        item.Isholiday = IsHoliday;
                        item.Isworkday = IsWorkDay;
                        var haspermission = _TaamerProContext.Permissions.Where(x => x.IsDeleted == false && x.EmpId.ToString() == item.EmpId && x.Date == TempToDay.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"))).ToList();


                        if (IsHoliday.Count() != 0)
                        {
                            status = "ع";
                        }
                        else if (IsWorkDay.Count() == 0)
                        {
                            status = "ن";
                        }

                        else if (!IsStarted)
                        {
                            status = "x";
                        }
                        else if (haspermission != null && haspermission.Count() > 0)
                        {
                            status = "ذ";
                        }
                        else
                        if (item.MAXSER == "")
                        {

                            status = "غ";
                        }
                        else  //لسه مبصمش
                        if (item.MAXSER == "")
                        {

                            status = "?";
                        }

                        else
                        {
                            status = "ح";

                        }

                        item.status = status;
                    }
                }
                newlmd.Add(item);

                }
            return newlmd;

        }

      public async Task<string>  checkdataweek(int dayOfWeek, Attendance_W_VM item,DateTime CurrentDate,int M,string con)
        {
            var CheckValue ="";
            //var TodayofWeek = ((GetWeekDay(DateTime.Now.Day.ToString())+2) % 7);// (((new Date()).getDay() + 2) % 7);
            var TodayofWeek = ((GetWeekDay(DateTime.Now.DayOfWeek.ToString())) );// (((new Date()).getDay() + 2) % 7);

            var Isunkwon = dayOfWeek > TodayofWeek ? true : false;

            if (item.DawamId != null && item.DawamId != "")
            {
                var dawamint = Convert.ToInt16(item.DawamId);

                var attdetails = await _AttTimeDetailsRepository.GetAllAttTimeDetailsByid(dawamint);

                var vac = _vacationRepository.GetVacationApprovedDays(Convert.ToInt16(item.EmpId), con);

                var StartWorkDate = DateTime.Parse(item.StartWorkDate);
                var IsStarted = false;
                if (CurrentDate > StartWorkDate || CurrentDate.Date == StartWorkDate.Date) //CurrentDate >= StartWorkDate
                    IsStarted = true;

                //var dd = ((dayOfWeek + 2) % 7);
                var dd = ((dayOfWeek));

                var IsWorkDay = attdetails.Where(x => x.Day == dd).ToList();

                var officalhol = await _icalHolidayRepository.GetAllOfficalHoliday();

                var IsHoliday = officalhol.Where(x => x.FromDate <= CurrentDate && x.ToDate >= CurrentDate).ToList();
                var haspermission = _TaamerProContext.Permissions.Where(x => x.IsDeleted == false && x.EmpId.ToString() == item.EmpId && x.Date == CurrentDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"))).ToList();


                var isLate = false;
                if (IsWorkDay.Count() > 0)
                {
                    var timeNow = DateTime.Now.TimeOfDay;
                    var join1 = IsWorkDay[0]._1StFromHour.Value.TimeOfDay;


                    isLate = timeNow > join1 ? true : false;
                }


                if (IsHoliday.Count() != 0 && M == 0)
                {
                    CheckValue = "ع";
                }
                else if (IsWorkDay.Count() == 0 && M == 0)
                {
                    CheckValue = "ن";
                }
                else if (M == 0)
                {
                    if (Isunkwon)
                    { //اليوم لسه مجاش
                        CheckValue = "?";
                    }
                    else if (!isLate && dayOfWeek == TodayofWeek) //لسه مبصمش النهاردة
                    {
                        CheckValue = "?";
                    }
                    else if (!IsStarted)
                    {
                        CheckValue = "X";
                    }
                    else if (haspermission != null && haspermission.Count() > 0)
                    {
                        CheckValue = "ذ";
                    }
                    else
                    {
                        CheckValue = "غ";
                    }
                }
                else
                {
                    CheckValue = "ح";
                }

                return CheckValue;
            }
            else
            {
                return CheckValue;

            }
            
            
        }

        public async Task<string> checkdataMonth(int dayOfWeek, Attendance_M_VM item, DateTime CurrentDate, int M,string con)
        {
            var CheckValue = "";
            var TodayofWeek = GetWeekDay(DateTime.Now.DayOfWeek.ToString());// (((GetWeekDay(DateTime.Now.DayOfWeek.ToString())) +2)%7);// (((new Date()).getDay() + 2) % 7);
            var Isunkwon = CurrentDate.Date > DateTime.Now.Date ? true : false;
            if (item.DawamId != null && item.DawamId != "")
            {

                var dawamint = Convert.ToInt16(item.DawamId);

                var attdetails = await _AttTimeDetailsRepository.GetAllAttTimeDetailsByid(dawamint);


                var StartWorkDate = DateTime.Parse(item.StartWorkDate);
                var IsStarted = false;
                if (CurrentDate > StartWorkDate || CurrentDate.Date == StartWorkDate.Date) //CurrentDate >= StartWorkDate
                    IsStarted = true;
                var doweek = GetWeekDay(CurrentDate.DayOfWeek.ToString());
                var dd = ((dayOfWeek % 7) + 1);
                //var dd = (dayOfWeek );

                var IsWorkDay = attdetails.Where(x => x.Day == doweek).ToList();

                var officalhol = await _icalHolidayRepository.GetAllOfficalHoliday();

                var IsHoliday = officalhol.Where(x => x.FromDate <= CurrentDate && x.ToDate >= CurrentDate).ToList();
                var haspermission = _TaamerProContext.Permissions.Where(x => x.IsDeleted == false && x.EmpId.ToString() == item.EmpId && x.Date == CurrentDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"))).ToList();

                var vac = _vacationRepository.GetVacationApprovedDays(Convert.ToInt16(item.EmpId), con).Result;
                var invac = vac.ToList().Where(x => x.Contains(CurrentDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")))).ToList();
                var isLate = false;
                if (IsWorkDay.Count() > 0)
                {
                    var timeNow = DateTime.Now.TimeOfDay;
                    var join1 = IsWorkDay[0]._1StFromHour.Value.TimeOfDay;


                    isLate = (DateTime.Now.Date > CurrentDate.Date || timeNow > join1) ? true : false;
                }


                if (IsHoliday.Count() != 0 && M == 0)
                {
                    CheckValue = "ع";
                }
                if (invac.Count() > 0 && M == 0)
                {
                    CheckValue = "ج";

                }
                else if (IsWorkDay.Count() == 0 && M == 0)
                {
                    CheckValue = "ن";
                }
                else if (IsWorkDay.Count() == 0 && M > 0)
                {
                    CheckValue = "ح";
                }
                else if (haspermission != null && haspermission.Count() > 0)
                {
                    CheckValue = "ذ";
                }

                else if (M == 0)
                {
                    if (!IsStarted)
                    {
                        CheckValue = "X";
                    }
                    else if (Isunkwon)
                    {
                        CheckValue = "؟";
                    }
                    //else if (!isLate && (dayOfWeek == (GetWeekDay(DateTime.Now.DayOfWeek.ToString())) ))
                    else if (!isLate && (CurrentDate.Date == DateTime.Now.Date))

                    {
                        CheckValue = "؟";
                    }
                    //else if ((isLate && (dayOfWeek == (GetWeekDay(DateTime.Now.DayOfWeek.ToString())) )) || dayOfWeek < (GetWeekDay(DateTime.Now.DayOfWeek.ToString())))
                    else if ((isLate && (CurrentDate.Date == DateTime.Now.Date || CurrentDate.Date < DateTime.Now.Date)))

                    {
                        CheckValue = "غ";
                    }
                }
                else if (M > 0)
                {
                    CheckValue = "ح";
                }

                return CheckValue;
            }
            else
            {
                return CheckValue;
            }


        }
   
        public async Task<IEnumerable<LateVM>> GetAttendance_Screen(string FromDate, string ToDate, int Shift, int BranchId, int SwType, string lang, string Con, int? yearid, int UserIDF)
        {
            try
            {

                if (yearid != null)
                {
                    var Att = await _AttendenceRepository.GetAttendance_Screen(FromDate, ToDate, yearid, Shift, BranchId, SwType, lang, Con, UserIDF);
                    List<int> EmpsInVacations = await _employeesRepository.GetEmpsInVacations();
                    Att = Att.Where(x => !string.IsNullOrEmpty(x.EmpId) && !EmpsInVacations.Contains(int.Parse(x.EmpId))).ToList();
                    var attend = await chckdata(Att.ToList(), SwType);

                    return attend;
                }
                return new List<LateVM>();
            }
            catch (Exception ex)
            {
                return new List<LateVM>();
            }

        }
        public async Task<IEnumerable<Attendance_M_VM>> GetAttendance_Screen_M(int Year, int Month, int Shift, int BranchId, int SwType, string lang, string Con, int? yearid, int UserIDF)
        {
            List<Attendance_M_VM> attend_M = new List<Attendance_M_VM>();
            try
            {

                if (yearid != null)
                {

                    var Att = await _AttendenceRepository.GetAttendance_Screen_M(Year, Month, yearid, Shift, BranchId, SwType, lang, Con, UserIDF);
                    List<int> EmpsInVacations = await _employeesRepository.GetEmpsInVacations();
                    Att = Att.Where(x => !string.IsNullOrEmpty(x.EmpId) && !EmpsInVacations.Contains(int.Parse(x.EmpId))).ToList();
                    
                    foreach (var e in Att)
                    {
                        List<string> att_status = new List<string>();

                        for (int i = 1; i <= 31; i++)
                        {

                            var curr = DateTime.Now; // get current date
                            var dt1 = (((curr.Day + 1)));
                            var first = curr;
                            DateTime currentDate = DateTime.Today;

                            DateTime lastDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                            if (lastDayOfMonth.Day >= i)
                            {
                                if (i != 31)
                                {
                                    first = new DateTime(curr.Year, curr.Month, i);// curr.AddDays(GetWeekDay(curr.DayOfWeek.ToString()) + 1);// new Date(curr.setDate(curr.getDate() - (((curr.getDay() + 1) % 7)))); // First day is the day of the month - the day of the week
                                }


                                var M = 0;
                                switch (i)
                                {
                                    case 1:
                                        M = (int)e.M_1;
                                        break;
                                    case 2:
                                        M = (int)e.M_2;
                                        break;
                                    case 3:
                                        M = (int)e.M_3;
                                        break;
                                    case 4:
                                        M = (int)e.M_4;
                                        break;
                                    case 5:
                                        M = (int)e.M_5;
                                        break;
                                    case 6:
                                        M = (int)e.M_6;
                                        break;
                                    case 7:
                                        M = (int)e.M_7;
                                        break;
                                    case 8:
                                        M = (int)e.M_8;
                                        break;
                                    case 9:
                                        M = (int)e.M_9;
                                        break;
                                    case 10:
                                        M = (int)e.M_10;
                                        break;
                                    case 11:
                                        M = (int)e.M_11;
                                        break;
                                    case 12:
                                        M = (int)e.M_12;
                                        break;
                                    case 13:
                                        M = (int)e.M_13;
                                        break;
                                    case 14:
                                        M = (int)e.M_14;
                                        break;
                                    case 15:
                                        M = (int)e.M_15;
                                        break;
                                    case 16:
                                        M = (int)e.M_16;
                                        break;
                                    case 17:
                                        M = (int)e.M_17;
                                        break;
                                    case 18:
                                        M = (int)e.M_18;
                                        break;
                                    case 19:
                                        M = (int)e.M_19;
                                        break;
                                    case 20:
                                        M = (int)e.M_20;
                                        break;
                                    case 21:
                                        M = (int)e.M_21;
                                        break;
                                    case 22:
                                        M = (int)e.M_22;
                                        break;
                                    case 23:
                                        M = (int)e.M_23;
                                        break;
                                    case 24:
                                        M = (int)e.M_24;
                                        break;
                                    case 25:
                                        M = (int)e.M_25;
                                        break;
                                    case 26:
                                        M = (int)e.M_26;
                                        break;
                                    case 27:
                                        M = (int)e.M_27;
                                        break;
                                    case 28:
                                        M = (int)e.M_28;
                                        break;
                                    case 29:
                                        M = (int)e.M_29;
                                        break;
                                    case 30:
                                        M = (int)e.M_30;
                                        break;
                                    case 31:
                                        M = (int)e.M_31;
                                        break;


                                }
                                var status = await checkdataMonth(i, e, first, M, Con);
                                att_status.Add(status);
                            }
                        }
                        e.M_status = att_status;
                        attend_M.Add(e);
                    }
                    return attend_M;
                }
                return new List<Attendance_M_VM>();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);

            }

        }
        public async Task<IEnumerable<Attendance_W_VM>> GetAttendance_Screen_W(int Year, int Month, int Shift, int BranchId, int SwType, string lang, string Con, int? yearid, int UserIDF)
        {
            try
            {
                List<Attendance_W_VM> attend_w = new List<Attendance_W_VM>();
                if (yearid != null)
                {
                    var Att = await _AttendenceRepository.GetAttendance_Screen_W(Year, Month, yearid, Shift, BranchId, SwType, lang, Con, UserIDF);
                    List<int> EmpsInVacations = await _employeesRepository.GetEmpsInVacations();
                    Att = Att.Where(x => !string.IsNullOrEmpty(x.EmpId) && !EmpsInVacations.Contains(int.Parse(x.EmpId))).ToList();
                    
                    foreach (var e in Att)
                    {
                        List<string> att_status = new List<string>();
                        DateTime today = DateTime.Now;
                        var curr= GetFirstDayOfWook(today);
                        for (int i = 1; i <= 7; i++)
                        {

                            //var curr = DateTime.Now; // get current date
                            var first = curr;
                            if (i > 1)
                            {
                                first = curr.AddDays(i);
                            }

                            // var first = curr;// curr.AddDays(GetWeekDay(curr.DayOfWeek.ToString()) - dt1);// new Date(curr.setDate(curr.getDate() - (((curr.getDay() + 1) % 7)))); // First day is the day of the month - the day of the week
                            var M = 0;
                            switch (i)
                            {
                                case 1:
                                    M = (int)e.M_1;
                                    break;
                                case 2:
                                    M = (int)e.M_2;
                                    break;
                                case 3:
                                    M = (int)e.M_3;
                                    break;
                                case 4:
                                    M = (int)e.M_4;
                                    break;
                                case 5:
                                    M = (int)e.M_5;
                                    break;
                                case 6:
                                    M = (int)e.M_6;
                                    break;
                                case 7:
                                    M = (int)e.M_7;
                                    break;
                            }
                            var status = await checkdataweek(i, e, first, M,Con);
                            att_status.Add(status);

                        }
                        e.M_status = att_status;
                        attend_w.Add(e);
                    }
                    return attend_w;
                }
                return new List<Attendance_W_VM>();
            }catch(Exception ex)
            {
                throw new AbandonedMutexException();
            }

        }

        public DateTime GetFirstDayOfWook(DateTime Today)
        {
            DateTime today = DateTime.Now;

            // Calculate the difference between the current day and Saturday
            int daysUntilSaturday = GetWeekDay("Saturday") - GetWeekDay(today.DayOfWeek.ToString());

            //// If today is already Saturday or later, move to the next Saturday
            //if (daysUntilSaturday <= 0)
            //{
            //    daysUntilSaturday += 7;
            //}

            DateTime firstDayOfWeek = today.AddDays(daysUntilSaturday);
        return firstDayOfWeek;
        }

        //public GeneralMessage SENDWhatsap()
        //{
        //    try {

        //        const string accountSid = "ACa2eb8d5c59a2cc60a34bce09d07fef6d";
        //        const string authToken = "18bd89112be090950b7c3cda4ced041a";
        //        TwilioClient.Init(accountSid, authToken);
        //        //ServicePointManager.Expect100Continue = true;
        //        //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //        ServicePointManager.Expect100Continue = true;
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //        //var message = MessageResource.Create(to: new Twilio.Types.PhoneNumber("whatsapp:+20-11-4428-9894"), from: new Twilio.Types.PhoneNumber("whatsapp:+1-415-523-8886"), body: "Good morning Mr Shegaley");
        //        //Console.WriteLine(message.Sid);

        //        //string From = "8104032389";
        //        ////  var link = "https://web.whatsapp.com/send?phone=" + model.Mobile_NO + "&amp;forceIntent=true&amp;load=loadInIOSExternalSafari";
        //        //WhatsApp wa = new WhatsApp(From, "01144289894", "Ehab");
        //        System.Diagnostics.Process.Start("http://api.whatsapp.com/send?phone=+201144289894&text=anymsg");

        //        //wa.OnConnectSuccess += () =>
        //        //{
        //        //    wa.OnLoginSuccess += (mobileNo, Data) =>
        //        //    {
        //        //        wa.SendMessage("01151429120", "Hello");
        //        //    };
        //        //};
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return new GeneralMessage { Result = true, Message = Resources.request_sent_successfully };
        //}
    }
}

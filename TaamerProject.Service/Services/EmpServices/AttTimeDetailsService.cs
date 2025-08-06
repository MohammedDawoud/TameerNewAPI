using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
using Twilio.TwiML.Messaging;

namespace TaamerProject.Service.Services
{
    public class AttTimeDetailsService : IAttTimeDetailsService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IAttTimeDetailsRepository _AttTimeDetailsRepository;
        private readonly IUsersRepository _usersRepository;


        public AttTimeDetailsService(TaamerProjectContext dataContext, ISystemAction systemAction, IAttTimeDetailsRepository attTimeDetailsRepository, IUsersRepository usersRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _AttTimeDetailsRepository = attTimeDetailsRepository;
            this._usersRepository = usersRepository;
        }
        public async Task< IEnumerable<AttTimeDetailsVM>> GetAllAttTimeDetails(string SearchText, int AttTimeId)
        {
            var AttTimeDetails =await _AttTimeDetailsRepository.GetAllAttTimeDetails(SearchText, AttTimeId);
            return AttTimeDetails;
        }

        public async Task<IEnumerable<AttTimeDetailsVM>> GetAllAttTimeDetails2(int AttTimeId, int branchid)
        {
            List<AttTimeDetailsVM> attlist = new List<AttTimeDetailsVM>();
            var attid = _usersRepository.GetById(AttTimeId).TimeId;
            //IEnumerable< AttTimeDetailsVM> AttTimeDetails = new List<AttTimeDetailsVM>();
            //if (attid == null) {
            //    AttTimeDetails = _AttTimeDetailsRepository.GetAllAttTimeDetails2bybranchid((int)branchid).OrderBy(x => x.Day);//.GroupBy(x => x._1StFromHour);
            //}
            //else { 
            // AttTimeDetails = _AttTimeDetailsRepository.GetAllAttTimeDetails2((int)attid).OrderBy(x=>x.Day);//.GroupBy(x => x._1StFromHour);
            //}
            if (attid != null)
            {
                var emp = _TaamerProContext.Employees.Where(x => x.UserId == AttTimeId).FirstOrDefault();

                var AttTimeDetails = _AttTimeDetailsRepository.GetAllAttTimeDetails2((int)attid).Result.OrderBy(x => x.Day);//.GroupBy(x => x._1StFromHour);

                foreach (var item in AttTimeDetails)
                {
                    //DateTime from =(DateTime)item._1StFromHour;
                    var From_D = (item._1StFromHour??DateTime.Now).ToString("tt", CultureInfo.CreateSpecificCulture("en"));
                    item._1StFromHour_Time = item._1StFromHour == null ? "" : item._1StFromHour.ToString().Split(' ')[1] + " " + ((DateTime)item._1StFromHour).ToString("tt", CultureInfo.CreateSpecificCulture("en")).ToString();
                    item._1StToHour_Time = item._1StToHour == null ? "" : item._1StToHour.ToString().Split(' ')[1] + " " + ((DateTime)item._1StToHour).ToString("tt", CultureInfo.CreateSpecificCulture("en")).ToString();
                    item._2ndFromHour_Time = item._2ndFromHour == null ? "" : item._2ndFromHour.ToString().Split(' ')[1] + " " + ((DateTime)item._2ndFromHour).ToString("tt", CultureInfo.CreateSpecificCulture("en")).ToString();
                    item._2ndToHour_Time = item._2ndToHour == null ? "" : item._2ndToHour.ToString().Split(' ')[1] + " " + ((DateTime)item._2ndToHour).ToString("tt", CultureInfo.CreateSpecificCulture("en")).ToString();

                }
                var dawam =
                            from x in AttTimeDetails
                            group x by new
                            {
                                x._1StFromHour_Time,
                                x._1StToHour_Time,
                                x._2ndFromHour_Time,
                                x._2ndToHour_Time

                            }
                            into dawamgroups

                            select new
                            {
                                _1StFromHour = dawamgroups.Key._1StFromHour_Time,
                                _1StToHour = dawamgroups.Key._1StToHour_Time,
                                _2ndFromHour = dawamgroups.Key._2ndFromHour_Time,
                                _2ndToHour = dawamgroups.Key._2ndToHour_Time,
                                Dayname = dawamgroups.OrderBy(x => x.Day)


                            };

                foreach (var item in dawam)
                {
                    AttTimeDetailsVM attobj = new AttTimeDetailsVM();
                    if (item.Dayname.Count() > 1)
                    {
                        attobj.DayName = "من " + item.Dayname.First().DayName + " الي " + item.Dayname.Last().DayName;
                    }
                    else
                    {
                        attobj.DayName = item.Dayname.First().DayName;
                    }
                    if (item._1StFromHour != "")
                    {
                        try
                        {
                            attobj._1StFromHour = item._1StFromHour == "" ? DateTime.ParseExact(item._1StFromHour.ToString(), "yyyy-MM-dd hh:mm:ss tt", CultureInfo.CreateSpecificCulture("en")) : DateTime.ParseExact("2023-02-23" + " " + item._1StFromHour.ToString(), "yyyy-MM-dd hh:mm:ss tt", CultureInfo.CreateSpecificCulture("en"));// item._1StFromHour;

                        }
                        catch (Exception ex)
                        {

                            attobj._1StFromHour = item._1StFromHour == "" ? DateTime.ParseExact(item._1StFromHour.ToString(), "yyyy-MM-dd h:mm:ss tt", CultureInfo.CreateSpecificCulture("en")) : DateTime.ParseExact("2023-02-23" + " " + item._1StFromHour.ToString(), "yyyy-MM-dd h:mm:ss tt", CultureInfo.CreateSpecificCulture("en"));// item._1StFromHour;
                        }
                    }
                    if (item._1StToHour != "")
                    {
                        try
                        {
                            attobj._1StToHour = item._1StToHour == "" ? DateTime.ParseExact(item._1StToHour.ToString(), "yyyy-MM-dd hh:mm:ss tt", CultureInfo.CreateSpecificCulture("en")) : DateTime.ParseExact("2023-02-23" + " " + item._1StToHour.ToString(), "yyyy-MM-dd hh:mm:ss tt", CultureInfo.CreateSpecificCulture("en"));// item._1StToHour;
                        }
                        catch (Exception ex)
                        {
                            attobj._1StToHour = item._1StToHour == "" ? DateTime.ParseExact(item._1StToHour.ToString(), "yyyy-MM-dd h:mm:ss tt", CultureInfo.CreateSpecificCulture("en")) : DateTime.ParseExact("2023-02-23" + " " + item._1StToHour.ToString(), "yyyy-MM-dd h:mm:ss tt", CultureInfo.CreateSpecificCulture("en"));// item._1StToHour;
                        }
                    }
                    if (item._2ndFromHour != "")
                    {
                        try
                        {
                            attobj._2ndFromHour = item._2ndFromHour == "" ? DateTime.ParseExact(item._2ndFromHour.ToString(), "yyyy-MM-dd hh:mm:ss tt", CultureInfo.CreateSpecificCulture("en")) : DateTime.ParseExact("2023-02-23" + " " + item._2ndFromHour, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.CreateSpecificCulture("en"));// item._2ndFromHour;
                        }
                        catch (Exception ex)
                        {
                            attobj._2ndFromHour = item._2ndFromHour == "" ? DateTime.ParseExact(item._2ndFromHour.ToString(), "yyyy-MM-dd h:mm:ss tt", CultureInfo.CreateSpecificCulture("en")) : DateTime.ParseExact("2023-02-23" + " " + item._2ndFromHour, "yyyy-MM-dd h:mm:ss tt", CultureInfo.CreateSpecificCulture("en"));// item._2ndFromHour;
                        }
                    }
                    if (item._2ndToHour != "")
                    {
                        try
                        {
                            attobj._2ndToHour = item._2ndToHour == "" ? DateTime.ParseExact(item._2ndToHour.ToString(), "yyyy-MM-dd hh:mm:ss tt", CultureInfo.CreateSpecificCulture("en")) : DateTime.ParseExact("2023-02-23" + " " + item._2ndToHour, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.CreateSpecificCulture("en"));// item._2ndToHour;
                        }
                        catch (Exception ex)
                        {
                            attobj._2ndToHour = item._2ndToHour == "" ? DateTime.ParseExact(item._2ndToHour.ToString(), "yyyy-MM-dd h:mm:ss tt", CultureInfo.CreateSpecificCulture("en")) : DateTime.ParseExact("2023-02-23" + " " + item._2ndToHour, "yyyy-MM-dd h:mm:ss tt", CultureInfo.CreateSpecificCulture("en"));// item._2ndToHour;
                        }
                    }
                    if (emp != null)
                    {

                        attobj.EmpHourlyCost = emp.EmpHourlyCost;
                    }
                    attlist.Add(attobj);
                }
           
            }

            return attlist;


        }

        public async Task<bool> CheckUserPerDawamUserExist(int UserId, string TimeFrom, string TimeTo, int DayNo)
        {
            var attuserid = _usersRepository.GetById(UserId).TimeId;
            if (attuserid == null)
            {
                return true;
            }
            else
            {
                var AttTimeDetails = _AttTimeDetailsRepository.CheckUserPerDawamUserExist(UserId, TimeFrom, TimeTo, DayNo, attuserid ?? 0).Result;
                if (AttTimeDetails.Count() > 0)
                {
                    foreach (var att in AttTimeDetails)
                    {
                        if (att._1StFromHour != null && att._1StToHour != null)
                        {
                            var From_D = "0"; var To_D = "0";

                            try
                            {
                                From_D = (att._1StFromHour ?? DateTime.Now).ToString("HH", CultureInfo.CreateSpecificCulture("en"));
                            }
                            catch (Exception)
                            {
                                From_D = (att._1StFromHour ?? DateTime.Now).ToString("H", CultureInfo.CreateSpecificCulture("en"));
                            }
                            try
                            {
                                To_D = (att._1StToHour ?? DateTime.Now).ToString("HH", CultureInfo.CreateSpecificCulture("en"));
                            }
                            catch (Exception)
                            {
                                To_D = (att._1StToHour ?? DateTime.Now).ToString("H", CultureInfo.CreateSpecificCulture("en"));
                            }

                            if (Convert.ToInt32(From_D) <= Convert.ToInt32(TimeFrom) && Convert.ToInt32(To_D) >= Convert.ToInt32(TimeTo))
                            {
                                return true;
                            }
                            else
                            {
                                if (att._2ndFromHour != null && att._2ndToHour != null)
                                {

                                    var From_D_Sec = "0"; var To_D_Sec = "0";

                                    try
                                    {
                                        From_D_Sec = (att._2ndFromHour ?? DateTime.Now).ToString("HH", CultureInfo.CreateSpecificCulture("en"));
                                    }
                                    catch (Exception)
                                    {
                                        From_D_Sec = (att._2ndFromHour ?? DateTime.Now).ToString("H", CultureInfo.CreateSpecificCulture("en"));
                                    }
                                    try
                                    {
                                        To_D_Sec = (att._2ndToHour ?? DateTime.Now).ToString("HH", CultureInfo.CreateSpecificCulture("en"));
                                    }
                                    catch (Exception)
                                    {
                                        To_D_Sec = (att._2ndToHour ?? DateTime.Now).ToString("H", CultureInfo.CreateSpecificCulture("en"));
                                    }


                                    if (Convert.ToInt32(From_D_Sec) <= Convert.ToInt32(TimeFrom) && Convert.ToInt32(To_D_Sec) >= Convert.ToInt32(TimeTo))
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }

                            }
                        }
                        else
                        {
                            if (att._2ndFromHour != null && att._2ndToHour != null)
                            {

                                var From_D_Sec = "0"; var To_D_Sec = "0";

                                try
                                {
                                    From_D_Sec = (att._2ndFromHour ?? DateTime.Now).ToString("HH", CultureInfo.CreateSpecificCulture("en"));
                                }
                                catch (Exception)
                                {
                                    From_D_Sec = (att._2ndFromHour ?? DateTime.Now).ToString("H", CultureInfo.CreateSpecificCulture("en"));
                                }
                                try
                                {
                                    To_D_Sec = (att._2ndToHour ?? DateTime.Now).ToString("HH", CultureInfo.CreateSpecificCulture("en"));
                                }
                                catch (Exception)
                                {
                                    To_D_Sec = (att._2ndToHour ?? DateTime.Now).ToString("H", CultureInfo.CreateSpecificCulture("en"));
                                }



                                if (Convert.ToInt32(From_D_Sec) <= Convert.ToInt32(TimeFrom) && Convert.ToInt32(To_D_Sec) >= Convert.ToInt32(TimeTo))
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public async Task<IEnumerable<AttTimeDetailsVM>> GetAllAttTimeDetails()
        {
            var AttTimeDetails = await _AttTimeDetailsRepository.GetAllAttTimeDetails();
            return AttTimeDetails;
        }

        public async Task<IEnumerable<AttTimeDetailsVM>> GetAllAttTimeDetailsByid(int AttTimeId)
        {
            var AttTimeDetails = await _AttTimeDetailsRepository.GetAllAttTimeDetailsByid(AttTimeId);
            return AttTimeDetails;
        }
        public GeneralMessage SaveAttTimeDetails(AttTimeDetails attTimeDetails, int UserId, int BranchId, string Lang)
        {

            try
            {
                // save from saterday to thursday
                if (attTimeDetails.Day == 8)
                {
                    for (int i = 1; i <= 6; i++)
                    {
                        attTimeDetails.Day = i;
                        var DayExists = _TaamerProContext.AttTimeDetails.Where(s => s.IsDeleted == false && s.TimeDetailsId != attTimeDetails.TimeDetailsId && s.AttTimeId == attTimeDetails.AttTimeId && s.Day == i && s.BranchId == BranchId).FirstOrDefault();

                        if (DayExists == null)
                        {
                            attTimeDetails.TimeDetailsId = 0;

                            attTimeDetails.AddUser = UserId;
                            attTimeDetails.BranchId = BranchId;
                            attTimeDetails.AddDate = DateTime.Now;
                            _TaamerProContext.AttTimeDetails.Add(attTimeDetails);
                            _TaamerProContext.SaveChanges();
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote = "اضافة تفاصيل وقت الحضور ";
                           _SystemAction.SaveAction("SaveAttTimeDetails", "AttTimeDetailsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        }

                    }
                    //-----------------------------------------------------------------------------------------------------------------
                    var message = Resources.General_SavedSuccessfully;
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = message };

                }


                var DayExist = _TaamerProContext.AttTimeDetails.Where(s => s.IsDeleted == false && s.TimeDetailsId != attTimeDetails.TimeDetailsId && s.AttTimeId == attTimeDetails.AttTimeId && s.Day == attTimeDetails.Day && s.BranchId == BranchId).FirstOrDefault();
                if (DayExist != null)
                {
                    var messageBefore = "اليوم موجود من قبل";

                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ تفاصيل وقت الحضور";
                    _SystemAction.SaveAction("SaveAttTimeDetails", "AttTimeDetailsService", 1, "اليوم موجود من قبل", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = messageBefore };
                }
                if (attTimeDetails.TimeDetailsId == 0)
                {
                    attTimeDetails.AddUser = UserId;
                    attTimeDetails.BranchId = BranchId;
                    attTimeDetails.AddDate = DateTime.Now;
                    _TaamerProContext.AttTimeDetails.Add(attTimeDetails);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة تفاصيل وقت الحضور ";
                    _SystemAction.SaveAction("SaveAttTimeDetails", "AttTimeDetailsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    var message = Resources.General_SavedSuccessfully;

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = message };
                }
                else
                {
                    var AttTimeDetailsUpdated = _TaamerProContext.AttTimeDetails.Where(x=>x.TimeDetailsId==attTimeDetails.TimeDetailsId).FirstOrDefault();
                    if (AttTimeDetailsUpdated != null)
                    {
                        AttTimeDetailsUpdated.Day = attTimeDetails.Day;
                        AttTimeDetailsUpdated._1StFromHour = attTimeDetails._1StFromHour;
                        AttTimeDetailsUpdated._1StToHour = attTimeDetails._1StToHour;
                        AttTimeDetailsUpdated._2ndFromHour = attTimeDetails._2ndFromHour;
                        AttTimeDetailsUpdated._2ndToHour = attTimeDetails._2ndToHour;
                        AttTimeDetailsUpdated.UpdateUser = UserId;
                        AttTimeDetailsUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل تفاصيل وقت الحضور رقم " + attTimeDetails.TimeDetailsId;
                    _SystemAction.SaveAction("SaveAttTimeDetails", "AttTimeDetailsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    var message = Resources.General_SavedSuccessfully;
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = message };
                }

            }
            catch (Exception ex)
            {
                var message = Resources.General_SavedFailed;
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ تفاصيل وقت الحضور";
                _SystemAction.SaveAction("SaveAttTimeDetails", "AttTimeDetailsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في حفظ تفاصيل وقت الحضور" };
            }
        }
        public GeneralMessage DeleteAttTimeDetails(int TimeDetailsId, int UserId, int BranchId)
        {
            try
            {
                AttTimeDetails attTimeDetails = _TaamerProContext.AttTimeDetails.Where(x => x.TimeDetailsId == TimeDetailsId)!.FirstOrDefault()!;
                if(attTimeDetails != null) 
                {
                    attTimeDetails.IsDeleted = true;
                    attTimeDetails.DeleteDate = DateTime.Now;
                    attTimeDetails.DeleteUser = UserId;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف تفاصيل وقت الحضور رقم " + TimeDetailsId;
                _SystemAction.SaveAction("DeleteAttTimeDetails", "AttTimeDetailsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف تفاصيل وقت الحضور رقم " + TimeDetailsId; ;
                _SystemAction.SaveAction("DeleteAttTimeDetails", "AttTimeDetailsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }


        public decimal CalculateTaskHoursForEmployee(int userId, DateTime taskStart, DateTime taskEnd)
        {
            decimal totalTaskHours = 0;
            const decimal defaultWorkHoursPerDay = 8m; // Standard working hours if no shifts

            // Get Public Holidays
            var publicHolidays = _TaamerProContext.OfficalHoliday
                .Where(x => !x.IsDeleted)
                .ToList();

            // Get Employee
            var employee = _TaamerProContext.Users
                .FirstOrDefault(x => !x.IsDeleted && x.UserId == userId);

            if (employee == null)
            {
                // No employee found → Return default hours
                return GetDefaultWorkingHours(taskStart, taskEnd, publicHolidays, defaultWorkHoursPerDay);
            }

            // Get Dawam List (Work Schedule)
            var dawamList = _TaamerProContext.AttTimeDetails
                .Where(x => !x.IsDeleted && x.AttTimeId == employee.TimeId)
                .ToList();

            if (dawamList == null || !dawamList.Any())
            {
                // No Dawam (Work Schedule) → Return default hours
                return GetDefaultWorkingHours(taskStart, taskEnd, publicHolidays, defaultWorkHoursPerDay);
            }

            // Iterate through each day, ensuring full duration is counted
            DateTime currentDate = taskStart.Date;
            while (currentDate <= taskEnd.Date)
            {
                int currentDayNumber = ((int)currentDate.DayOfWeek + 1) % 7 + 1; // Convert Sunday=0 to 7

                // Skip public holidays
                if (publicHolidays.Any(h => currentDate.Date >= h.FromDate.Date && currentDate.Date <= h.ToDate.Date))
                {
                    currentDate = currentDate.AddDays(1);
                    continue;
                }

                // Get shift details for this day
                var dawam = dawamList.FirstOrDefault(d => d.Day == currentDayNumber);

                if (dawam == null)
                {
                    // No shift found → Weekend (skip)
                    currentDate = currentDate.AddDays(1);
                    continue;
                }

                // Define working time limits
                DateTime workStartTime = (currentDate == taskStart.Date) ? taskStart : currentDate.Date;
                DateTime workEndTime = (currentDate == taskEnd.Date) ? taskEnd : currentDate.Date.AddHours(23).AddMinutes(59);

                // Calculate work hours for both shifts (if available)
                if (dawam._1StFromHour.HasValue && dawam._1StToHour.HasValue)
                {
                    totalTaskHours += GetOverlappingHours(workStartTime, workEndTime, dawam._1StFromHour.Value, dawam._1StToHour.Value);
                }

                if (dawam._2ndFromHour.HasValue && dawam._2ndToHour.HasValue)
                {
                    totalTaskHours += GetOverlappingHours(workStartTime, workEndTime, dawam._2ndFromHour.Value, dawam._2ndToHour.Value);
                }

                currentDate = currentDate.AddDays(1); // Ensure all days are included
            }

            return totalTaskHours;
        }

        // Helper function to calculate overlapping hours
        private decimal GetOverlappingHours(DateTime taskStart, DateTime taskEnd, DateTime shiftStart, DateTime shiftEnd)
        {
            // Extract only the time part
            TimeSpan taskStartTime = taskStart.TimeOfDay;
            TimeSpan taskEndTime = taskEnd.TimeOfDay;
            TimeSpan shiftStartTime = shiftStart.TimeOfDay;
            TimeSpan shiftEndTime = shiftEnd.TimeOfDay;

            // Find the overlapping period
            TimeSpan maxStart = taskStartTime > shiftStartTime ? taskStartTime : shiftStartTime;
            TimeSpan minEnd = taskEndTime < shiftEndTime ? taskEndTime : shiftEndTime;

            return maxStart < minEnd ? (decimal)(minEnd - maxStart).TotalHours : 0;
        }


        //  Helper function to return default working hours (8 hours per working day)
        private decimal GetDefaultWorkingHours(DateTime taskStart, DateTime taskEnd, List<OfficalHoliday> publicHolidays, decimal workHoursPerDay)
        {
            decimal totalHours = 0;

            for (DateTime currentDate = taskStart.Date; currentDate <= taskEnd.Date; currentDate = currentDate.AddDays(1))
            {
                // Skip public holidays
                if (publicHolidays.Any(h => currentDate.Date >= h.FromDate.Date && currentDate.Date <= h.ToDate.Date))
                {
                    continue;
                }

                totalHours += workHoursPerDay;
            }

            return totalHours;
        }


    }
}

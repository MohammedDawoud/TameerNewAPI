using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using System.Globalization;
using System.Data.SqlClient;
using System.Data;

using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.Enums;

namespace TaamerProject.Repository.Repositories
{
    public class AttendenceRepository : IAttendenceRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public AttendenceRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public Attendence GetById(long AttId)
        {
            return _TaamerProContext.Attendence.Where(x => x.AttendenceId == AttId).FirstOrDefault();
        }

        public async Task<IEnumerable<AttendenceVM>> GetAllAttendence(int BranchId)
        {
            var Attendence = _TaamerProContext.Attendence.Where(s => s.IsDeleted == false && !string.IsNullOrEmpty(s.Employees.WorkStartDate)
            && string.IsNullOrEmpty(s.Employees.EndWorkDate) && s.BranchId == BranchId &&
            DateTime.ParseExact(s.Employees.WorkStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToLocalTime() <= s.CheckTime.ToLocalTime()
            ).Select(x => new AttendenceVM
            {
                AttendenceId = x.AttendenceId,
                EmpId = x.EmpId,
                Day = x.Day,
                CheckTime = x.CheckTime,
                CheckIn = x.CheckIn,
                CheckOut = x.CheckOut,
                IsLate = x.IsLate,
                LateDuration = x.LateDuration,
                IsOverTime = x.IsOverTime,
                SameDate = x.SameDate,
                IsDone = x.IsDone,
                BranchId = x.BranchId,
                EmployeeName = x.Employees.EmployeeNameAr,
                BranchName = x.Branch.NameAr,
                AttendenceHijriDate = x.AttendenceHijriDate,
                AttendenceDate = x.AttendenceDate,
                CheckType = x.CheckType,
                ShiftTime = x.ShiftTime
            }).OrderBy(x => x.CheckTime).ThenBy(x => x.AttendenceDate).ToList();

            List<int> PassedEmps = new List<int>();
            List<string> days = new List<string>();

            //Chickin
            List<AttendenceVM> result1 = Attendence.Where(c => c.CheckType == "دخول").GroupBy(item => new { item.EmpId, item.AttendenceDate, item.ShiftTime })
                                 .Select(grouping => grouping.FirstOrDefault())
                                 .OrderBy(x => x.AttendenceDate).ThenBy(x => x.CheckTime).ToList();

            List<AttendenceVM> result2 = Attendence.Where(c => c.CheckType == "خروج").GroupBy(item => new { item.EmpId, item.AttendenceDate, item.ShiftTime })
                                 .Select(grouping => grouping.LastOrDefault())
                                 .OrderBy(x => x.AttendenceDate).ThenBy(x => x.CheckTime).ToList();

            List<AttendenceVM> result = result1.Union(result2).OrderByDescending(x => x.AttendenceDate).ToList();

            return result;
        }
        public async Task<IEnumerable<AttendenceVM>> GetAllAttendence_Device(int BranchId)
        {
            var Attendence = _TaamerProContext.Attendence.Where(s => s.IsDeleted == false && !string.IsNullOrEmpty(s.Employees.WorkStartDate) && string.IsNullOrEmpty(s.Employees.EndWorkDate)
            && s.BranchId == BranchId && s.Source == 1).Select(x => new AttendenceVM
            {
                AttendenceId = x.AttendenceId,
                EmpId = x.EmpId,
                Day = x.Day,
                CheckTime = x.CheckTime,
                CheckIn = x.CheckIn,
                CheckOut = x.CheckOut,
                IsLate = x.IsLate,
                LateDuration = x.LateDuration,
                IsOverTime = x.IsOverTime,
                SameDate = x.SameDate,
                IsDone = x.IsDone,
                BranchId = x.BranchId,
                EmployeeName = x.Employees.EmployeeNameAr,
                BranchName = x.Branch.NameAr,
                AttendenceHijriDate = x.AttendenceHijriDate,
                AttendenceDate = x.AttendenceDate,
                CheckType = x.CheckType,
                ShiftTime = x.ShiftTime
            }).ToList();

            return Attendence;
        }
        public async Task<bool> IsExist(int EmpId, string Date, int Hour, int BranchId)
        {
            var Attendence = _TaamerProContext.Attendence.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.EmpId == EmpId
            && !string.IsNullOrEmpty(s.Employees.WorkStartDate) && string.IsNullOrEmpty(s.Employees.EndWorkDate) && s.RealEmpId == EmpId
            && s.AttendenceDate == Date && s.CheckTime.Hour == Hour).ToList();
            if (Attendence != null && Attendence.Count > 0)
                return true;
            else
                return false;
        }
        public async Task<IEnumerable<AttendenceVM>> EmpAttendenceSearch(AttendenceVM AttendenceSearch, int BranchId)
        {
            try
            {
                DateTime StartDate = DateTime.ParseExact(AttendenceSearch.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime EndDate = DateTime.ParseExact(AttendenceSearch.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                var Attendence = _TaamerProContext.Attendence.Where(s => s.IsDeleted == false
                && (s.EmpId == AttendenceSearch.EmpId || AttendenceSearch.EmpId == null || AttendenceSearch.EmpId==0) && (s.BranchId == AttendenceSearch.BranchId || AttendenceSearch.BranchId==0 || AttendenceSearch.BranchId==null))
                        .Select(x => new AttendenceVM
                        {
                            AttendenceId = x.AttendenceId,
                            EmpId = x.EmpId,
                            Day = x.Day,
                            CheckTime = x.CheckTime,
                            CheckOut = x.CheckOut,
                            IsLate = x.IsLate,
                            LateDuration = x.LateDuration,
                            IsOverTime = x.IsOverTime,
                            SameDate = x.SameDate,
                            IsDone = x.IsDone,
                            BranchId = x.BranchId,
                            EmployeeName = x.Employees.EmployeeNameAr,
                            BranchName = x.Branch.NameAr,
                            AttendenceHijriDate = x.AttendenceHijriDate,
                            AttendenceDate = x.AttendenceDate,
                            CheckType = x.CheckType,
                            ShiftTime = x.ShiftTime,
                            WorkStartDate = x.Employees.WorkStartDate,
                            EndWorkDate = x.Employees.EndWorkDate

                        }).ToList();

                Attendence = Attendence.Where(x => !string.IsNullOrEmpty(x.WorkStartDate) && DateTime.ParseExact(x.WorkStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(x.AttendenceDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                           && (string.IsNullOrEmpty(x.EndWorkDate) ||
                                               (!string.IsNullOrEmpty(x.EndWorkDate) && DateTime.ParseExact(x.EndWorkDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(x.AttendenceDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                                          ).ToList();

                Attendence = Attendence.Where(s => (StartDate <= DateTime.ParseExact(s.AttendenceDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) &&
                        DateTime.ParseExact(s.AttendenceDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= EndDate)).
                        OrderBy(x => x.CheckTime).ThenBy(x => x.AttendenceDate).ToList();

                //Chickin
                List<AttendenceVM> result1 = Attendence.Where(c => c.CheckType == "دخول").GroupBy(item => new { item.EmpId, item.AttendenceDate, item.ShiftTime})
                                     .Select(grouping => grouping.FirstOrDefault())
                                     .OrderBy(x => x.AttendenceDate).ThenBy(x => x.CheckTime).ToList();

                List<AttendenceVM> result2 = Attendence.Where(c => c.CheckType == "خروج").GroupBy(item => new { item.EmpId, item.AttendenceDate, item.ShiftTime })
                                     .Select(grouping => grouping.LastOrDefault())
                                     .OrderBy(x => x.AttendenceDate).ThenBy(x => x.CheckTime).ToList();

                List<AttendenceVM> result = result1.Union(result2).OrderByDescending(x => x.AttendenceDate).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new List<AttendenceVM>();
            }
        }
        public async Task<IEnumerable<AttendenceVM>> GetAllAttendenceSearch(int BranchId)
        {
            var Attendence = _TaamerProContext.Attendence.Where(s => s.IsDeleted == false && !string.IsNullOrEmpty(s.Employees.WorkStartDate) && string.IsNullOrEmpty(s.Employees.EndWorkDate))
                                               .Select(x => new AttendenceVM
                                               {
                                                   AttendenceId = x.AttendenceId,
                                                   EmpId = x.EmpId,
                                                   Day = x.Day,
                                                   CheckTime = x.CheckTime,
                                                   CheckOut = x.CheckOut,
                                                   IsLate = x.IsLate,
                                                   LateDuration = x.LateDuration,
                                                   IsOverTime = x.IsOverTime,
                                                   SameDate = x.SameDate,
                                                   IsDone = x.IsDone,
                                                   BranchId = x.BranchId,
                                                   EmployeeName = x.Employees.EmployeeNameAr,
                                                   BranchName = x.Branch.NameAr,
                                                   AttendenceHijriDate = x.AttendenceHijriDate,
                                                   AttendenceDate = x.AttendenceDate,
                                                   CheckType = x.CheckType,
                                                   ShiftTime = x.ShiftTime
                                               }).ToList();
            return Attendence;
        }

        public async Task<IEnumerable<AttendenceVM>> GetAllAttendenceBySearchObject(AttendenceVM Search, int BranchId)
        {
            try
            {
                var Attendence = _TaamerProContext.Attendence.Where(s => s.IsDeleted == false)
                      .Select(x => new AttendenceVM
                      {
                          AttendenceId = x.AttendenceId,
                          EmpId = x.EmpId,
                          Day = x.Day,
                          CheckTime = x.CheckTime,
                          CheckIn = x.CheckIn,
                          CheckOut = x.CheckOut,
                          IsLate = x.IsLate,
                          LateDuration = x.LateDuration,
                          IsOverTime = x.IsOverTime,
                          SameDate = x.SameDate,
                          IsDone = x.IsDone,
                          BranchId = x.BranchId,
                          EmployeeName = x.Employees.EmployeeNameAr,
                          BranchName = x.Branch.NameAr,
                          AttendenceHijriDate = x.AttendenceHijriDate,
                          AttendenceDate = x.AttendenceDate,
                          CheckType = x.CheckType,
                          ShiftTime = x.ShiftTime,
                          WorkStartDate = x.Employees.WorkStartDate,
                          EndWorkDate =x.Employees.EndWorkDate

                      }).ToList();

                 if (Search.EndDate == null || Search.StartDate == null)
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(Search.BranchId)))
                    {
                        Attendence = Attendence.Where(x => x.BranchId == Search.BranchId).ToList();
                    }

                    if (!String.IsNullOrEmpty(Convert.ToString(Search.RealEmpId)))
                    {
                        Attendence = Attendence.Where(x => x.EmpId == Search.RealEmpId).ToList();
                    }
                }
                else
                {

                    Attendence = Attendence.Where(x => !string.IsNullOrEmpty(x.WorkStartDate) && DateTime.ParseExact(x.WorkStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(x.AttendenceDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                && ((!string.IsNullOrEmpty(x.EndWorkDate) && DateTime.ParseExact(x.EndWorkDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(x.AttendenceDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) || string.IsNullOrEmpty(x.EndWorkDate)
                                )).ToList();

                    if (!String.IsNullOrEmpty(Convert.ToString(Search.BranchId)))
                    {
                        Attendence = Attendence.Where(x => x.BranchId == Search.BranchId).ToList();
                    }

                    if (!String.IsNullOrEmpty(Convert.ToString(Search.RealEmpId)))
                    {
                        Attendence = Attendence.Where(x => x.EmpId == Search.RealEmpId).ToList();
                    }


                    if (!String.IsNullOrEmpty(Search.StartDate))
                    {
                        Attendence = Attendence.Where(x => x.CheckTime.Value.ToLocalTime() >= DateTime.ParseExact(Search.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToLocalTime()).ToList();
                    }
                
                    if (!String.IsNullOrEmpty(Search.EndDate))
                    {
                        Attendence = Attendence.Where(x => x.CheckTime.Value.ToLocalTime() <= DateTime.ParseExact(Search.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToLocalTime()).ToList();
                    }
                }

                //Chickin
                List<AttendenceVM> result1 = Attendence.Where(c => c.CheckType == "دخول").GroupBy(item => new { item.EmpId, item.AttendenceDate, item.ShiftTime })
                                     .Select(grouping => grouping.FirstOrDefault())
                                     .OrderBy(x => x.AttendenceDate).ThenBy(x => x.CheckTime).ToList();

                List<AttendenceVM> result2 = Attendence.Where(c => c.CheckType == "خروج").GroupBy(item => new { item.EmpId, item.AttendenceDate, item.ShiftTime })
                                     .Select(grouping => grouping.LastOrDefault())
                                     .OrderBy(x => x.AttendenceDate).ThenBy(x => x.CheckTime).ToList();

                List<AttendenceVM> result = result1.Union(result2).OrderByDescending(x => x.AttendenceDate).ToList();


                return result;

            }
            catch(Exception ex)
            {
                var Attendence = _TaamerProContext.Attendence.Where(s => s.IsDeleted == false && !string.IsNullOrEmpty(s.Employees.WorkStartDate) && string.IsNullOrEmpty(s.Employees.EndWorkDate))
                                              .Select(x => new AttendenceVM
                                              {
                                                  AttendenceId = x.AttendenceId,
                                                  EmpId = x.EmpId,
                                                  Day = x.Day,
                                                  CheckTime = x.CheckTime,
                                                  CheckOut = x.CheckOut,
                                                  IsLate = x.IsLate,
                                                  LateDuration = x.LateDuration,
                                                  IsOverTime = x.IsOverTime,
                                                  SameDate = x.SameDate,
                                                  IsDone = x.IsDone,
                                                  BranchId = x.BranchId,
                                                  EmployeeName = x.Employees.EmployeeNameAr,
                                                  BranchName = x.Branch.NameAr,
                                                  AttendenceHijriDate = x.AttendenceHijriDate,
                                                  AttendenceDate = x.AttendenceDate,
                                                  CheckType = x.CheckType,
                                                  ShiftTime = x.ShiftTime,
                                                  

                                              }).ToList();
                return Attendence;
            }
        }

        public async Task<IEnumerable<AbsenceVM>> GetAbsenceData(string FromDate, string ToDate, int EmpId, int? YearId, int BranchId, string lang, string Con)
        {
            try
            {
                List<AbsenceVM> lmd = new List<AbsenceVM>();

                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_AttAbsenceEmployees";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        int EmpployeId = 0;
                        int BrId = 0;


                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", YearId + "-01-01"));
                        }
                        else
                        {
                            from = FromDate;
                            string fdate = String.Format("{0:yyyy-MM-dd}", from);
                            command.Parameters.Add(new SqlParameter("@From", fdate));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@To", (YearId + 1) + "-12-31"));

                        }
                        else
                        {
                            to = ToDate;

                            string tdate = String.Format("{0:yyyy-MM-dd}", to);
                            command.Parameters.Add(new SqlParameter("@To", tdate));
                        }

                        if (EmpId == 0)
                        {
                            EmpployeId = 0;
                            command.Parameters.Add(new SqlParameter("@EmpId", EmpployeId));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@EmpId", EmpId));
                        }

                        if (BranchId == 0)
                        {
                            BrId = 0;
                            command.Parameters.Add(new SqlParameter("@BranchId", BrId));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        }

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            lmd.Add(new AbsenceVM
                            {
                                EmpNo = (dr[0]).ToString(),

                                E_FullName = dr[2].ToString(),
                                DayNOfWeek = (dr[5]).ToString(),
                                Mdate = dr[4].ToString(),

                                E_BranchId = (dr[15]).ToString()




                            });

                        }
                    }
                }
                return lmd.ToList();
            }
            catch(Exception ex)
            {
                //string msg = ex.Message;
                List<AbsenceVM> lmd = new List<AbsenceVM>();
                return lmd.ToList();
            }

        }



        public async Task<IEnumerable<AbsenceVM>> GetAbsenceData_withWeekEnd(string FromDate, string ToDate, int EmpId, int? YearId, int BranchId, string lang, string Con)
        {
            try
            {
                List<AbsenceVM> lmd = new List<AbsenceVM>();

                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_AttAbsenceEmployees_withoutWeekEnd";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        int EmpployeId = 0;
                        int BrId = 0;


                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", YearId + "-01-01"));
                        }
                        else
                        {
                            from = FromDate;
                            string fdate = String.Format("{0:yyyy-MM-dd}", from);
                            command.Parameters.Add(new SqlParameter("@From", fdate));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@To", (YearId + 1) + "-12-31"));

                        }
                        else
                        {
                            to = ToDate;

                            string tdate = String.Format("{0:yyyy-MM-dd}", to);
                            command.Parameters.Add(new SqlParameter("@To", tdate));
                        }

                        if (EmpId == 0)
                        {
                            EmpployeId = 0;
                            command.Parameters.Add(new SqlParameter("@EmpId", EmpployeId));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@EmpId", EmpId));
                        }

                        if (BranchId == 0)
                        {
                            BrId = 0;
                            command.Parameters.Add(new SqlParameter("@BranchId", BrId));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        }

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            lmd.Add(new AbsenceVM
                            {
                                EmpNo = (dr[0]).ToString(),

                                E_FullName = dr[2].ToString(),
                                DayNOfWeek = (dr[5]).ToString(),
                                Mdate = dr[4].ToString(),

                                E_BranchId = (dr[15]).ToString()




                            });

                        }
                    }
                }
                return lmd.ToList();
            }
            catch (Exception ex)
            {
                //string msg = ex.Message;
                List<AbsenceVM> lmd = new List<AbsenceVM>();
                return lmd.ToList();
            }

        }

        public async Task<IEnumerable<AbsenceVM>> GetAbsenceDataToday(string TodayDate, int? YearId, int BranchId, string lang, string Con)
        {
            try
            {
                List<AbsenceVM> lmd = new List<AbsenceVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_TodayAttAbsenceEmployees";
                        command.Connection = con;
                        string from = null;
                        int BrId = 0;


                        if (TodayDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@ToDate", YearId + "-01-01"));
                        }
                        else
                        {
                            from = TodayDate;
                            string fdate = String.Format("{0:yyyy-MM-dd}", from);
                            command.Parameters.Add(new SqlParameter("@ToDate", fdate));
                        }



                        if (BranchId == 0)
                        {
                            BrId = 0;
                            command.Parameters.Add(new SqlParameter("@BranchId", BrId));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        }

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            lmd.Add(new AbsenceVM
                            {
                                EmpNo = (dr[0]).ToString(),

                                E_FullName = dr[2].ToString(),
                                DayNOfWeek = (dr[5]).ToString(),
                                Mdate = dr[4].ToString(),

                                E_BranchId = (dr[15]).ToString()




                            });

                        }
                    }
                }
                return lmd;
            }
            catch
            {
                //string msg = ex.Message;
                List<AbsenceVM> lmd = new List<AbsenceVM>();
                return lmd;
            }

        }
        public async Task<IEnumerable<LateVM>> GetLateData(string FromDate, string ToDate, int EmpId, int? YearId, int Shift, int BranchId, string lang, string Con)
        {
            try
            {
                List<LateVM> lmd = new List<LateVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_AttLateEmployees";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        int EmpployeId = 0;
                        int BrId = 0;
                        int shift = 0;

                        //discount variables 
                        int countLessThan15 = 0;
                        int countBetween15And30 = 0;
                        int countBetween30And60 = 0;
                        int countGreaterThan60 = 0;


                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", YearId + "-01-01"));
                        }
                        else
                        {
                            from = FromDate;
                            string fdate = String.Format("{0:yyyy-MM-dd}", from);
                            command.Parameters.Add(new SqlParameter("@From", fdate));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@To", (YearId + 1) + "-12-31"));

                        }
                        else
                        {
                            to = ToDate;

                            string tdate = String.Format("{0:yyyy-MM-dd}", to);
                            command.Parameters.Add(new SqlParameter("@To", tdate));
                        }

                        if (EmpId == 0)
                        {
                            EmpployeId = 0;
                            command.Parameters.Add(new SqlParameter("@EmpId", EmpployeId));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@EmpId", EmpId));
                        }

                        if (BranchId == 0)
                        {
                            BrId = 0;
                            command.Parameters.Add(new SqlParameter("@BranchId", BrId));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        }


                        if (Shift == 0)
                        {
                            shift = 0;
                            command.Parameters.Add(new SqlParameter("@ShiftTime", shift));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@ShiftTime", Shift));
                        }

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            lmd.Add(new LateVM
                            {

                                //  @Html.Raw(item.TimeJoin1.HasValue ? Convert.ToString(item.TimeJoin1.Value.ToString("HH:mm")) : item.EmpShift != 2 ? "----" : "")


                                EmpNo = (dr[16]).ToString(),
                                FullName = dr[12].ToString(),
                                DawamId = (dr[13]).ToString(),
                                TimeJoin1 = (!String.IsNullOrWhiteSpace(dr[4].ToString())) ? Convert.ToDateTime(dr[4]).ToString("hh:mm tt") : dr[13].ToString() != 2.ToString() ? "----" : "",
                                TimeJoin2 = (!String.IsNullOrWhiteSpace(dr[6].ToString())) ? Convert.ToDateTime(dr[6]).ToString("hh:mm tt") : dr[13].ToString() != 1.ToString() ? "----" : "",
                                
                                MoveTimeIntJoin1 = dr[8].ToString(),
                                MoveTimeStringJoin1 = (!String.IsNullOrWhiteSpace(dr[4].ToString())) ? (!String.IsNullOrWhiteSpace(dr[8].ToString()) || Convert.ToInt32(dr[8]) > 0 ? dr[18].ToString().Substring(1) : "00:00") : "",
                                MoveTimeIntJoin2 = dr[10].ToString(),
                                MoveTimeStringJoin2 = (!String.IsNullOrWhiteSpace(dr[6].ToString())) ? (!String.IsNullOrWhiteSpace(dr[10].ToString()) || Convert.ToInt32(dr[10]) > 0 ? dr[20].ToString().Substring(1) : "00:00") : "",


                                BranchId = (dr[2]).ToString(),
                                DateDay = ConvertDateCalendar((DateTime)dr[3], "Gregorian", "en-US"),

                                // Calculate the discount for the late times
                                Discount1 = (!String.IsNullOrWhiteSpace(dr[8].ToString())) ? CalculateDiscount(Convert.ToInt32(dr[8]), Convert.ToDecimal(dr[24]), ref countLessThan15, ref countBetween15And30, ref countBetween30And60, ref countGreaterThan60):0,
                                Discount2 = (!String.IsNullOrWhiteSpace(dr[10].ToString())) ? CalculateDiscount(Convert.ToInt32(dr[10]), Convert.ToDecimal(dr[24]), ref countLessThan15, ref countBetween15And30, ref countBetween30And60, ref countGreaterThan60):0


                            });

                        }
                    }
                }
                return lmd;
                    //.Where(x=>(x.MoveTimeStringJoin1 !=null && x.MoveTimeStringJoin1 !="00:00" && x.MoveTimeStringJoin1=="")
                //|| (x.MoveTimeStringJoin2 !=null && x.MoveTimeStringJoin2 != null && x.MoveTimeStringJoin2 != "")
                //);
            }
            catch (Exception ex)
            {
                //string msg = ex.Message;
                List<LateVM> lmd = new List<LateVM>();
                return lmd;
            }

        }



        public decimal CalculateDiscount(int lateMinutes,decimal salary, ref int countLessThan15, ref int countBetween15And30, ref int countBetween30And60, ref int countGreaterThan60)
        {
            if (lateMinutes < 15)
            {
                return IncrementAndCalculateDiscount(salary, (int)LatePeriod.LessThan15, ref countLessThan15);
            }
            else if (lateMinutes >= 15 && lateMinutes < 30)
            {
                return IncrementAndCalculateDiscount(salary,(int)LatePeriod.Between15And30, ref countBetween15And30);
            }
            else if (lateMinutes >= 30 && lateMinutes < 60)
            {
                return IncrementAndCalculateDiscount(salary, (int)LatePeriod.Between30And60,ref countBetween30And60);
            }
            else if (lateMinutes >= 60)
            {
                return IncrementAndCalculateDiscount(salary, (int)LatePeriod.GreaterThan60,ref countGreaterThan60);
            }

            return 0m;
        }

        private decimal IncrementAndCalculateDiscount(decimal salary,int type, ref int count)
        {
          

            count++;
            var id = count > 4 ? 4 : count;
            var laterules = _TaamerProContext.LateLists.FirstOrDefault(x=>x.ID== type);
            return id switch
            {
                (int)LatePeriod.LessThan15 => (laterules.First.Value/100) * salary,
                (int)LatePeriod.Between15And30 => (laterules.Second.Value / 100) * salary,
                (int)LatePeriod.Between30And60 => (laterules.Third.Value / 100) * salary,
                (int)LatePeriod.GreaterThan60 => (laterules.Fourth.Value / 100) * salary,
                _ => 0m
            };
        }

        public async Task<IEnumerable<LateVM>> GetLateDataToday(string TodayDate, int? YearId, int Shift, int BranchId, string lang, string Con)
        {
            try
            {
                List<LateVM> lmd = new List<LateVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_TodayAttLateEmployees";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        int BrId = 0;
                        int shift = 0;

                        if (TodayDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@ToDate", YearId + "-01-01"));
                        }
                        else
                        {
                            from = TodayDate;
                            string fdate = String.Format("{0:yyyy-MM-dd}", from);
                            command.Parameters.Add(new SqlParameter("@ToDate", fdate));
                        }



                        if (BranchId == 0)
                        {
                            BrId = 0;
                            command.Parameters.Add(new SqlParameter("@BranchId", BrId));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        }

                        if (Shift == 0)
                        {
                            shift = 0;
                            command.Parameters.Add(new SqlParameter("@ShiftTime", shift));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@ShiftTime", Shift));
                        }

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            lmd.Add(new LateVM
                            {
                                EmpNo = (dr[16]).ToString(),
                                FullName = dr[12].ToString(),
                                DawamId = (dr[13]).ToString(),
                                TimeJoin1 = (!String.IsNullOrWhiteSpace(dr[4].ToString())) ? Convert.ToDateTime(dr[4]).ToString("LT") : dr[13].ToString() != 2.ToString() ? "----" : "",
                                TimeJoin2 = (!String.IsNullOrWhiteSpace(dr[6].ToString())) ? Convert.ToDateTime(dr[6]).ToString("LT") : dr[13].ToString() != 1.ToString() ? "----" : "",

                                MoveTimeIntJoin1 = dr[8].ToString(),
                                MoveTimeStringJoin1 = (!String.IsNullOrWhiteSpace(dr[4].ToString())) ? (!String.IsNullOrWhiteSpace(dr[8].ToString()) || Convert.ToInt32(dr[8]) > 0 ? dr[18].ToString().Substring(1) : "00:00") : "",
                                MoveTimeIntJoin2 = dr[10].ToString(),
                                MoveTimeStringJoin2 = (!String.IsNullOrWhiteSpace(dr[6].ToString())) ? (!String.IsNullOrWhiteSpace(dr[10].ToString()) || Convert.ToInt32(dr[10]) > 0 ? dr[20].ToString().Substring(1) : "00:00") : "",

                                BranchId = (dr[2]).ToString(),
                                DateDay =  ConvertDateCalendar((DateTime)dr[3], "Gregorian", "en-US"),
                            });

                        }
                    }
                }
                return lmd;
            }
            catch
            {
                //string msg = ex.Message;
                List<LateVM> lmd = new List<LateVM>();
                return lmd;
            }

        }
        public async Task<IEnumerable<LateVM>> GetEarlyDepartureData(string FromDate, string ToDate, int EmpId, int? YearId, int Shift, int BranchId, string lang, string Con)
        {
            try
            {
                List<LateVM> lmd = new List<LateVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_AttEarlyDepartureEmployees";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        int EmpployeId = 0;
                        int BrId = 0;
                        int shift = 0;


                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", YearId + "-01-01"));
                        }
                        else
                        {
                            from = FromDate;
                            string fdate = String.Format("{0:yyyy-MM-dd}", from);
                            command.Parameters.Add(new SqlParameter("@From", fdate));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@To", (YearId + 1) + "-12-31"));

                        }
                        else
                        {
                            to = ToDate;

                            string tdate = String.Format("{0:yyyy-MM-dd}", to);
                            command.Parameters.Add(new SqlParameter("@To", tdate));
                        }

                        if (EmpId == 0)
                        {
                            EmpployeId = 0;
                            command.Parameters.Add(new SqlParameter("@EmpId", EmpployeId));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@EmpId", EmpId));
                        }

                        if (BranchId == 0)
                        {
                            BrId = 0;
                            command.Parameters.Add(new SqlParameter("@BranchId", BrId));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        }


                        if (Shift == 0)
                        {
                            shift = 0;
                            command.Parameters.Add(new SqlParameter("@ShiftTime", shift));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@ShiftTime", Shift));
                        }

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            lmd.Add(new LateVM
                            {


                                EmpNo = (dr[16]).ToString(),
                                FullName = dr[12].ToString(),
                                DawamId = (dr[13]).ToString(),
                                TimeLeave1 = (!String.IsNullOrWhiteSpace(dr[5].ToString())) ? Convert.ToDateTime(dr[5]).ToString("hh:mm tt") : dr[13].ToString() != 2.ToString() ? "----" : "",
                                TimeLeave2 = (!String.IsNullOrWhiteSpace(dr[7].ToString())) ? Convert.ToDateTime(dr[7]).ToString("hh:mm tt") : dr[13].ToString() != 1.ToString() ? "----" : "",



                                MoveTimeIntLeave1 = dr[9].ToString(),
                                MoveTimeStringLeave1 = (!String.IsNullOrWhiteSpace(dr[5].ToString())) ? (!String.IsNullOrWhiteSpace(dr[9].ToString()) || Convert.ToInt32(dr[9]) > 0 ? dr[19].ToString().Substring(1) : "00:00") : "",
                                MoveTimeIntLeave2 = dr[11].ToString(),
                                MoveTimeStringLeave2 = (!String.IsNullOrWhiteSpace(dr[7].ToString())) ? (!String.IsNullOrWhiteSpace(dr[11].ToString()) || Convert.ToInt32(dr[11]) > 0 ? dr[21].ToString().Substring(1) : "00:00") : "",

                                BranchId = (dr[2]).ToString(),
                                DateDay = ConvertDateCalendar((DateTime)dr[3], "Gregorian", "en-US"),






                            });

                        }
                    }
                }
                return lmd;
            }
            catch
            {
                //string msg = ex.Message;
                List<LateVM> lmd = new List<LateVM>();
                return lmd;
            }

        }
        public async Task<IEnumerable<LateVM>> GetEarlyDepartureDataToday(string TodayDate, int? YearId, int Shift, int BranchId, string lang, string Con)
        {
            try
            {
                List<LateVM> lmd = new List<LateVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_TodayAttEarlyDepartureEmployees";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        int EmpployeId = 0;
                        int BrId = 0;
                        int shift = 0;

                        if (TodayDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@ToDate", YearId + "-01-01"));
                        }
                        else
                        {
                            from = TodayDate;
                            string fdate = String.Format("{0:yyyy-MM-dd}", from);
                            command.Parameters.Add(new SqlParameter("@ToDate", fdate));
                        }



                        if (BranchId == 0)
                        {
                            BrId = 0;
                            command.Parameters.Add(new SqlParameter("@BranchId", BrId));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        }

                        if (Shift == 0)
                        {
                            shift = 0;
                            command.Parameters.Add(new SqlParameter("@ShiftTime", shift));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@ShiftTime", Shift));
                        }

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            lmd.Add(new LateVM
                            {
                                EmpNo = (dr[16]).ToString(),
                                FullName = dr[12].ToString(),
                                DawamId = (dr[13]).ToString(),
                                TimeLeave1 = (!String.IsNullOrWhiteSpace(dr[5].ToString())) ? Convert.ToDateTime(dr[5]).ToString("hh:mm tt") : dr[13].ToString() != 2.ToString() ? "----" : "",
                                TimeLeave2 = (!String.IsNullOrWhiteSpace(dr[7].ToString())) ? Convert.ToDateTime(dr[7]).ToString("hh:mm tt") : dr[13].ToString() != 1.ToString() ? "----" : "",



                                MoveTimeIntLeave1 = dr[9].ToString(),
                                MoveTimeStringLeave1 = (!String.IsNullOrWhiteSpace(dr[5].ToString())) ? (!String.IsNullOrWhiteSpace(dr[9].ToString()) || Convert.ToInt32(dr[9]) > 0 ? dr[19].ToString().Substring(1) : "00:00") : "",
                                MoveTimeIntLeave2 = dr[11].ToString(),
                                MoveTimeStringLeave2 = (!String.IsNullOrWhiteSpace(dr[7].ToString())) ? (!String.IsNullOrWhiteSpace(dr[11].ToString()) || Convert.ToInt32(dr[11]) > 0 ? dr[21].ToString().Substring(1) : "00:00") : "",

                                BranchId = (dr[2]).ToString(),
                                DateDay = Convert.ToDateTime(dr[3]).ToString("dd/MM/yyyy"),


                            });

                        }
                    }
                }
                return lmd;
            }
            catch
            {
                //string msg = ex.Message;
                List<LateVM> lmd = new List<LateVM>();
                return lmd;
            }

        }
        public async Task<IEnumerable<NotLoggedOutVM>> GetNotLoggedOutData(string FromDate, string ToDate, int? YearId, int BranchId, string lang, string Con)
        {
            try
            {
                List<NotLoggedOutVM> lmd = new List<NotLoggedOutVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_AttNotLoggedOutEmployees";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        int BrId = 0;


                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", YearId + "-01-01"));
                        }
                        else
                        {
                            from = FromDate;
                            string fdate = String.Format("{0:yyyy-MM-dd}", from);
                            command.Parameters.Add(new SqlParameter("@From", fdate));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@To", (YearId + 1) + "-12-31"));

                        }
                        else
                        {
                            to = ToDate;

                            string tdate = String.Format("{0:yyyy-MM-dd}", to);
                            command.Parameters.Add(new SqlParameter("@To", tdate));
                        }



                        if (BranchId == 0)
                        {
                            BrId = 0;
                            command.Parameters.Add(new SqlParameter("@BranchId", BrId));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        }




                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            lmd.Add(new NotLoggedOutVM
                            {

                                EmpNo = (dr[6]).ToString(),
                                FullName = dr[3].ToString(),
                                BranchName = (dr[4]).ToString(),
                                CheckTime = ConvertDateCalendar((DateTime)dr[2], "Gregorian", "en-US"),
                                Day = (int)(((DateTime)dr[2]).Date.DayOfWeek),
                                DayName= GetDayNameInArabic((DateTime)dr[2]),
                            });

                        }
                    }
                }
                return lmd;
            }
            catch
            {
                //string msg = ex.Message;
                List<NotLoggedOutVM> lmd = new List<NotLoggedOutVM>();
                return lmd;
            }

        }
        public static string GetDayNameInArabic(DateTime date)
        {
            CultureInfo arabicCulture = new CultureInfo("ar-SA");
            return date.ToString("dddd", arabicCulture);
        }
        public async Task<IEnumerable<LateVM>> GetAttendanceData(string FromDate, string ToDate, int? YearId, int Shift, int BranchId, string lang, string Con)
        {
            try
            {
                List<LateVM> lmd = new List<LateVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_AttEmployees";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        int EmpployeId = 0;
                        int BrId = 0;
                        int shift = 0;


                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", YearId + "-01-01"));
                        }
                        else
                        {
                            from = FromDate;
                            string fdate = String.Format("{0:yyyy-MM-dd}", from);
                            command.Parameters.Add(new SqlParameter("@From", fdate));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@To", (YearId + 1) + "-12-31"));

                        }
                        else
                        {
                            to = ToDate;

                            string tdate = String.Format("{0:yyyy-MM-dd}", to);
                            command.Parameters.Add(new SqlParameter("@To", tdate));
                        }



                        if (BranchId == 0)
                        {
                            BrId = 0;
                            command.Parameters.Add(new SqlParameter("@BranchId", BrId));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        }

                        //if (Shift == 0)
                        //{
                        //    shift = 0;
                        //    command.Parameters.Add(new SqlParameter("@ShiftTime", shift));

                        //}
                        //else
                        //{
                        //    command.Parameters.Add(new SqlParameter("@ShiftTime", Shift));
                        //}


                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {

                            //TimeSpan t1 = TimeSpan.Parse("00:00");
                            //var abs = 0;
                            //var att = 0;
                            //if (YearId != null)
                            //{
                            //    var late = GetLateData(FromDate, ConvertDateCalendar((DateTime)dr[3], "Gregorian", "en-US"), (int)dr[1], YearId, Shift, BranchId, lang, Con).Result.ToList();
                            //    if (late.Count() > 0)
                            //    {
                            //        foreach (var item in late)
                            //        {
                            //            if (item.MoveTimeStringJoin1 != null && item.MoveTimeStringJoin1 != "")
                            //            {
                            //                // t1. += int.Parse(item.MoveTimeStringJoin1.ToString().Substring(1));
                            //                var time = Convert.ToDateTime(item.MoveTimeStringJoin1).ToString("hh:mm");


                            //               t1 = t1.Add(TimeSpan.Parse(item.MoveTimeStringJoin1));
                            //            }
                            //        }
                            //    }
                            //    t1.ToString();
                            //}
                            //if (YearId != null)
                            //{
                            //    var result = GetAbsenceData(FromDate, ConvertDateCalendar((DateTime)dr[3], "Gregorian", "en-US"), (int)dr[1], YearId, BranchId, lang, Con).Result;
                            //    abs = result.Count();
                            //}
                            //AttendenceVM AttendenceSearch = new AttendenceVM();
                            //AttendenceSearch.StartDate = FromDate;
                            //AttendenceSearch.EndDate = ConvertDateCalendar((DateTime)dr[3], "Gregorian", "en-US");
                            //AttendenceSearch.EmpId = (int)dr[1];
                            //AttendenceSearch.BranchId = BranchId;
                            //att = EmpAttendenceSearch(AttendenceSearch, BranchId).Result.ToList().DistinctBy(x => x.AttendenceDate).ToList().Count();

                            lmd.Add(new LateVM
                            {


                                EmpNo = (dr[16]).ToString(),
                                FullName = dr[12].ToString(),
                                TimeJoin1 = (!String.IsNullOrWhiteSpace(dr[4].ToString())) ? Convert.ToDateTime(dr[4]).ToString("hh:mm tt") : dr[13].ToString() != 2.ToString() ? "----" : "",
                                TimeLeave1 = (!String.IsNullOrWhiteSpace(dr[5].ToString())) ? Convert.ToDateTime(dr[5]).ToString("hh:mm tt") : dr[13].ToString() != 2.ToString() ? "----" : "",
                                TimeJoin2 = (!String.IsNullOrWhiteSpace(dr[6].ToString())) ? Convert.ToDateTime(dr[6]).ToString("hh:mm tt") : dr[13].ToString() != 1.ToString() ? "----" : "",
                                TimeLeave2 = (!String.IsNullOrWhiteSpace(dr[7].ToString())) ? Convert.ToDateTime(dr[7]).ToString("hh:mm tt") : dr[13].ToString() != 1.ToString() ? "----" : "",
                                DateDay = ConvertDateCalendar((DateTime)dr[3], "Gregorian", "en-US"),
                                Late = (dr[28]).ToString(),
                                absence = (dr[29]).ToString(),
                                attend = (dr[30]).ToString(),

                            });


                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                List<LateVM> lmd = new List<LateVM>();
                return lmd;
            }

        }


        public async Task<IEnumerable<LateVM>> GetAttendanceData(
      string FromDate, string ToDate, int? YearId, int Shift, int BranchId,
      string lang, string Con, int pageNumber, int pageSize)
        {
            try
            {
                List<LateVM> lmd = new List<LateVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_AttEmployees";
                        command.Connection = con;

                        string from = string.IsNullOrEmpty(FromDate) ? $"{YearId}-01-01" : FromDate;
                        string to = string.IsNullOrEmpty(ToDate) ? $"{YearId + 1}-12-31" : ToDate;

                        command.Parameters.AddWithValue("@From", from);
                        command.Parameters.AddWithValue("@To", to);
                        command.Parameters.AddWithValue("@BranchId", BranchId);
                        command.Parameters.AddWithValue("@PageNumber", pageNumber);
                        command.Parameters.AddWithValue("@PageSize", pageSize);

                        con.Open();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                lmd.Add(new LateVM
                                {
                                    EmpNo = reader["EmpNo"].ToString(),
                                    FullName = reader["FullName"].ToString(),
                                    TimeJoin1 = reader["TimeJoin1"] != DBNull.Value ? Convert.ToDateTime(reader["TimeJoin1"]).ToString("hh:mm tt") : "----",
                                    TimeLeave1 = reader["TimeLeave1"] != DBNull.Value ? Convert.ToDateTime(reader["TimeLeave1"]).ToString("hh:mm tt") : "----",
                                    TimeJoin2 = reader["TimeJoin2"] != DBNull.Value ? Convert.ToDateTime(reader["TimeJoin2"]).ToString("hh:mm tt") : "----",
                                    TimeLeave2 = reader["TimeLeave2"] != DBNull.Value ? Convert.ToDateTime(reader["TimeLeave2"]).ToString("hh:mm tt") : "----",
                                    DateDay = Convert.ToDateTime(reader["DateDay"]).ToString("yyyy-MM-dd")
                                });
                            }
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<LateVM>();
            }
        }

        public async Task<IEnumerable<LateVM>> GetAttendanceData_Application(string FromDate, string ToDate, int? YearId, int Shift, int BranchId, string lang, string Con)
        {
            try
            {
                List<LateVM> lmd = new List<LateVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_AttEmployeesApp";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        int EmpployeId = 0;
                        int BrId = 0;
                        int shift = 0;


                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", YearId + "-01-01"));
                        }
                        else
                        {
                            from = FromDate;
                            string fdate = String.Format("{0:yyyy-MM-dd}", from);
                            command.Parameters.Add(new SqlParameter("@From", fdate));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@To", (YearId + 1) + "-12-31"));

                        }
                        else
                        {
                            to = ToDate;

                            string tdate = String.Format("{0:yyyy-MM-dd}", to);
                            command.Parameters.Add(new SqlParameter("@To", tdate));
                        }



                        if (BranchId == 0)
                        {
                            BrId = 0;
                            command.Parameters.Add(new SqlParameter("@BranchId", BrId));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        }

                        //if (Shift == 0)
                        //{
                        //    shift = 0;
                        //    command.Parameters.Add(new SqlParameter("@ShiftTime", shift));

                        //}
                        //else
                        //{
                        //    command.Parameters.Add(new SqlParameter("@ShiftTime", Shift));
                        //}


                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            lmd.Add(new LateVM
                            {
                                EmpId= (dr[1]).ToString(),

                                EmpNo = (dr[16]).ToString(),
                                FullName = dr[12].ToString(),
                                TimeJoin1 = (!String.IsNullOrWhiteSpace(dr[4].ToString())) ? Convert.ToDateTime(dr[4]).ToString("hh:mm tt") : dr[13].ToString() != 2.ToString() ? "--" : "",
                                TimeLeave1 = (!String.IsNullOrWhiteSpace(dr[5].ToString())) ? Convert.ToDateTime(dr[5]).ToString("hh:mm tt") : dr[13].ToString() != 2.ToString() ? "--" : "",
                                TimeJoin2 = (!String.IsNullOrWhiteSpace(dr[6].ToString())) ? Convert.ToDateTime(dr[6]).ToString("hh:mm tt") : dr[13].ToString() != 1.ToString() ? "--" : "",
                                TimeLeave2 = (!String.IsNullOrWhiteSpace(dr[7].ToString())) ? Convert.ToDateTime(dr[7]).ToString("hh:mm tt") : dr[13].ToString() != 1.ToString() ? "--" : "",

                                PhotoUrl = (!String.IsNullOrWhiteSpace(dr[24].ToString())) ? (dr[24]).ToString() : "/distnew/images/userprofile.png",
                                JobName = dr[25].ToString(),
                                Latitude= dr[26].ToString(),
                                Longitude = dr[27].ToString(),
                                FromApplication = dr[28].ToString(),
                                DateDay = ConvertDateCalendar((DateTime)dr[3], "Gregorian", "en-US"),
                                BranchName= lang=="en"? dr[32].ToString(): dr[31].ToString(),
                                MAXSER = (dr[0]).ToString(),
                                Type = Convert.ToInt32(dr[33]),
                                ShiftTime= Convert.ToInt32(dr[34]),
                                


                            });

                        }
                    }
                }
               // return lmd;

                var groupedData = lmd.GroupBy(
                                    x => new
                                    {
                                        x.DateDay,
                                        x.Type,
                                        x.ShiftTime,
                                        x.EmpId
                                    })
                                    .Select(g => new LateVM
                                    {
                                        DateDay = g.Key.DateDay,
                                        Type = g.Key.Type,
                                        EmpId = g.Key.EmpId,
                                        TimeJoin1 = g.Min(x => x.TimeJoin1),
                                        TimeLeave1 = g.Max(x => x.TimeLeave1),
                                        TimeJoin2 = g.Min(x => x.TimeJoin2),
                                        TimeLeave2 = g.Max(x => x.TimeLeave2),
                                       
                                        FullName = g.FirstOrDefault().FullName,

                                       
                                        PhotoUrl = g.FirstOrDefault().PhotoUrl,
                                        JobName = g.FirstOrDefault().JobName,
                                        Latitude = g.FirstOrDefault().Latitude,
                                        Longitude = g.FirstOrDefault().Longitude,
                                        FromApplication = g.FirstOrDefault().FromApplication,
                                        BranchName = g.FirstOrDefault().BranchName,
                                        MAXSER = g.FirstOrDefault().MAXSER,

                                    })
                                    .ToList();
                return groupedData;
                                        
            }
            catch (Exception ex)
            {
                //string msg = ex.Message;
                List<LateVM> lmd = new List<LateVM>();
                return lmd;
            }

        }
        public async Task<IEnumerable<Attendance_M_VM>> GetAttendance_Screen_M(int Year, int Month,int? YearId, int Shift, int BranchId, int SwType, string lang, string Con, int UserIDF)
        {
            try
            {
                List<Attendance_M_VM> lmd = new List<Attendance_M_VM>();

                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_Attendance_Screen";
                        command.Connection = con;
                        int BrId = 0;

                        command.Parameters.Add(new SqlParameter("@From", (YearId - 2) + "-01-01"));
                        command.Parameters.Add(new SqlParameter("@To", (YearId + 1) + "-12-31"));



                        if (BranchId == 0)
                        {
                            BrId = 0;
                            command.Parameters.Add(new SqlParameter("@BranchId", BrId));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        }
                        command.Parameters.Add(new SqlParameter("@EmpId", UserIDF));

                        command.Parameters.Add(new SqlParameter("@SwType", SwType));
                        //command.Parameters.Add(new SqlParameter("@Year", Year));
                        //command.Parameters.Add(new SqlParameter("@Month", Month));




                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            //string x = Convert.ToDateTime(dr[3]).ToString("yyyy-MM-dd");
                            //string x2 = dr[3].ToString();
                            string x3 = dr[3].ToString();
                            CultureInfo arCul = new CultureInfo("ar-SA");
                            HijriCalendar h = new HijriCalendar();
                            arCul.DateTimeFormat.Calendar = h;

                            //var EmpNO_N = dr[0] == null ? "" : dr[0].ToString();
                            //var FullName_N = dr[12].ToString();
                            //var TimeJoin1_N = (!String.IsNullOrWhiteSpace(dr[4].ToString())) ? Convert.ToDateTime(dr[4]).ToString("HH:mm") : dr[13].ToString() != 2.ToString() ? "----" : "";
                            //var TimeLeave1_N = (!String.IsNullOrWhiteSpace(dr[5].ToString())) ? Convert.ToDateTime(dr[5]).ToString("HH:mm") : dr[13].ToString() != 2.ToString() ? "----" : "";
                            //var TimeJoin2_N = (!String.IsNullOrWhiteSpace(dr[6].ToString())) ? Convert.ToDateTime(dr[6]).ToString("HH:mm") : dr[13].ToString() != 1.ToString() ? "----" : "";
                            //var TimeLeave2_N = (!String.IsNullOrWhiteSpace(dr[7].ToString())) ? Convert.ToDateTime(dr[7]).ToString("HH:mm") : dr[13].ToString() != 1.ToString() ? "----" : "";
                            //var DateDay_N = dr[3].ToString() == "" ? "" : DateTime.ParseExact(Convert.ToDateTime(dr[3]).ToString("yyyy-MM-dd"), "yyyy-MM-dd", arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces).ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            //var PhotoUrl = dr[24] == null ? "" : dr[24].ToString();
                            //var EmpId_N = dr[25] == null ? "" : dr[25].ToString();

                            //var xY = 1;
                            var b = dr[5];
                            var x = dr[5] == DBNull.Value ? 0 : (int)dr[5];



                            lmd.Add(new Attendance_M_VM
                            {
                                EmpId = dr[37] == null ? "" : dr[37].ToString(),
                                EmpNo = (dr[38]).ToString(),
                                FullName = dr[39].ToString(),
                                DawamId = dr["DawamId"].ToString() ?? "",
                                StartWorkDate = dr["WorkStartDate"].ToString() ?? "",
                                M_1 = dr[5] == DBNull.Value ? 0 : (int?)dr[5],
                                M_2 = dr[6] == DBNull.Value ? 0 : (int?)dr[6],
                                M_3 = dr[7] == DBNull.Value ? 0 : (int?)dr[7],
                                M_4 = dr[8] == DBNull.Value ? 0 : (int?)dr[8],
                                M_5 = dr[9] == DBNull.Value ? 0 : (int?)dr[9],
                                M_6 = dr[10] == DBNull.Value ? 0 : (int?)dr[10],
                                M_7 = dr[11] == DBNull.Value ? 0 : (int?)dr[11],
                                M_8 = dr[12] == DBNull.Value ? 0 : (int?)dr[12],
                                M_9 = dr[13] == DBNull.Value ? 0 : (int?)dr[13],
                                M_10 = dr[14] == DBNull.Value ? 0 : (int?)dr[14],
                                M_11 = dr[15] == DBNull.Value ? 0 : (int?)dr[15],
                                M_12 = dr[16] == DBNull.Value ? 0 : (int?)dr[16],
                                M_13 = dr[17] == DBNull.Value ? 0 : (int?)dr[17],
                                M_14 = dr[18] == DBNull.Value ? 0 : (int?)dr[18],
                                M_15 = dr[19] == DBNull.Value ? 0 : (int?)dr[19],
                                M_16 = dr[20] == DBNull.Value ? 0 : (int?)dr[20],
                                M_17 = dr[21] == DBNull.Value ? 0 : (int?)dr[21],
                                M_18 = dr[22] == DBNull.Value ? 0 : (int?)dr[22],
                                M_19 = dr[23] == DBNull.Value ? 0 : (int?)dr[23],
                                M_20 = dr[24] == DBNull.Value ? 0 : (int?)dr[24],
                                M_21 = dr[25] == DBNull.Value ? 0 : (int?)dr[25],
                                M_22 = dr[26] == DBNull.Value ? 0 : (int?)dr[26],
                                M_23 = dr[27] == DBNull.Value ? 0 : (int?)dr[27],
                                M_24 = dr[28] == DBNull.Value ? 0 : (int?)dr[28],
                                M_25 = dr[29] == DBNull.Value ? 0 : (int?)dr[29],
                                M_26 = dr[30] == DBNull.Value ? 0 : (int?)dr[30],
                                M_27 = dr[31] == DBNull.Value ? 0 : (int?)dr[31],
                                M_28 = dr[32] == DBNull.Value ? 0 : (int?)dr[32],
                                M_29 = dr[33] == DBNull.Value ? 0 : (int?)dr[33],
                                M_30 = dr[34] == DBNull.Value ? 0 : (int?)dr[34],
                                M_31 = dr[35] == DBNull.Value ? 0 : (int?)dr[35],
                                M_Total = dr[36] == DBNull.Value ? 0 : (int?)dr[36],

                            }) ;


                        }
                    }
                }
                return lmd;

            }
            catch(Exception ex)
            {
                //string msg = ex.Message;
                List<Attendance_M_VM> lmd = new List<Attendance_M_VM>();
                return lmd;
            }

        }

        public async Task<IEnumerable<Attendance_W_VM>> GetAttendance_Screen_W(int Year, int Month, int? YearId, int Shift, int BranchId, int SwType, string lang, string Con, int UserIDF)
        {
            try
            {
                List<Attendance_W_VM> lmd = new List<Attendance_W_VM>();

                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_Attendance_Screen";
                        command.Connection = con;

                        int BrId = 0;


                        command.Parameters.Add(new SqlParameter("@From", (YearId - 2) + "-01-01"));
                        command.Parameters.Add(new SqlParameter("@To", (YearId + 1) + "-12-31"));
                        //command.Parameters.Add(new SqlParameter("@From", (YearId - 2) ));
                        //command.Parameters.Add(new SqlParameter("@To", (YearId + 1) ));


                        if (BranchId == 0)
                        {
                            BrId = 0;
                            command.Parameters.Add(new SqlParameter("@BranchId", BrId));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        }
                        command.Parameters.Add(new SqlParameter("@EmpId", UserIDF));

                        command.Parameters.Add(new SqlParameter("@SwType", SwType));
                        //command.Parameters.Add(new SqlParameter("@Year", Year));
                        //command.Parameters.Add(new SqlParameter("@Month", Month));


                        var Date = FirstDayOfWeek(DateTime.Now);
                        int FirstDay = Date.Day;
                        //int FirstDay = Date.Day; //- 1;

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            //string x = Convert.ToDateTime(dr[3]).ToString("yyyy-MM-dd");
                            //string x2 = dr[3].ToString();
                            string x3 = dr[3].ToString();
                            CultureInfo arCul = new CultureInfo("ar-SA");
                            HijriCalendar h = new HijriCalendar();
                            arCul.DateTimeFormat.Calendar = h;

                            //var EmpNO_N = dr[0] == null ? "" : dr[0].ToString();
                            //var FullName_N = dr[12].ToString();
                            //var TimeJoin1_N = (!String.IsNullOrWhiteSpace(dr[4].ToString())) ? Convert.ToDateTime(dr[4]).ToString("HH:mm") : dr[13].ToString() != 2.ToString() ? "----" : "";
                            //var TimeLeave1_N = (!String.IsNullOrWhiteSpace(dr[5].ToString())) ? Convert.ToDateTime(dr[5]).ToString("HH:mm") : dr[13].ToString() != 2.ToString() ? "----" : "";
                            //var TimeJoin2_N = (!String.IsNullOrWhiteSpace(dr[6].ToString())) ? Convert.ToDateTime(dr[6]).ToString("HH:mm") : dr[13].ToString() != 1.ToString() ? "----" : "";
                            //var TimeLeave2_N = (!String.IsNullOrWhiteSpace(dr[7].ToString())) ? Convert.ToDateTime(dr[7]).ToString("HH:mm") : dr[13].ToString() != 1.ToString() ? "----" : "";
                            //var DateDay_N = dr[3].ToString() == "" ? "" : DateTime.ParseExact(Convert.ToDateTime(dr[3]).ToString("yyyy-MM-dd"), "yyyy-MM-dd", arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces).ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            //var PhotoUrl = dr[24] == null ? "" : dr[24].ToString();
                            //var EmpId_N = dr[25] == null ? "" : dr[25].ToString();

                            //var xY = 1;
                            if (FirstDay + 4 + 6 <= 35)
                            {
                                lmd.Add(new Attendance_W_VM
                                {

                                    //EmpId = dr[37] == null ? "" : dr[0].ToString(),
                                    EmpId = dr[37].ToString(),
                                    EmpNo = (dr[38]).ToString(),
                                    FullName = dr[39].ToString(),
                                    DawamId = (dr["DawamId"]).ToString() ?? "",
                                    StartWorkDate = (dr["WorkStartDate"]).ToString() ?? "",

                                    M_1 = dr[FirstDay + 4] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4],
                                    M_2 = dr[FirstDay + 4 + 1] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 + 1],
                                    M_3 = dr[FirstDay + 4 + 2] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 + 2],
                                    M_4 = dr[FirstDay + 4 + 3] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 + 3],
                                    M_5 = dr[FirstDay + 4 + 4] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 + 4],
                                    M_6 = dr[FirstDay + 4 + 5] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 + 5],
                                    M_7 = dr[FirstDay + 4 + 6] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 + 6],

                                });
                            }
                            else
                            {
                                if ( (FirstDay + 4 + 6)-35 ==6)
                                {
                                    lmd.Add(new Attendance_W_VM
                                    {

                                        //EmpId = dr[37] == null ? "" : dr[0].ToString(),
                                        EmpId = dr[37].ToString(),
                                        EmpNo = (dr[38]).ToString(),
                                        FullName = dr[39].ToString(),
                                        DawamId = (dr["DawamId"]).ToString() ?? "",
                                        StartWorkDate = (dr["WorkStartDate"]).ToString() ?? "",
                                        M_1 = dr[FirstDay + 4] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4],
                                        M_2 = dr[4+ 1] == DBNull.Value ? 0 : (int?)dr[4 + 1],
                                        M_3 = dr[4 + 2] == DBNull.Value ? 0 : (int?)dr[4 + 2],
                                        M_4 = dr[4 + 3] == DBNull.Value ? 0 : (int?)dr[4 + 3],
                                        M_5 = dr[4 + 4] == DBNull.Value ? 0 : (int?)dr[4 + 4],
                                        M_6 = dr[4 + 5] == DBNull.Value ? 0 : (int?)dr[4 + 5],
                                        M_7 = dr[4 + 5] == DBNull.Value ? 0 : (int?)dr[4 + 5],
                                    });
                                }
                                else if((FirstDay + 4 + 6) - 35 == 5)
                                {
                                    lmd.Add(new Attendance_W_VM
                                    {

                                        //EmpId = dr[37] == null ? "" : dr[0].ToString(),
                                        EmpId = dr[37].ToString(),
                                        EmpNo = (dr[38]).ToString(),
                                        FullName = dr[39].ToString(),
                                        DawamId = (dr["DawamId"]).ToString() ?? "",
                                        StartWorkDate = (dr["WorkStartDate"]).ToString() ?? "",
                                        M_1 = dr[FirstDay + 4] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 ],
                                        M_2 = dr[FirstDay + 4 + 1] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 + 1],
                                        //M_3 = 0,
                                        //M_4 = 0,
                                        //M_5 = 0,
                                        //M_6 = 0,
                                        //M_7 = 0,

                                        M_3 = dr[4 + 1] == DBNull.Value ? 0 : (int?)dr[4 + 1],
                                        M_4 = dr[4 + 2] == DBNull.Value ? 0 : (int?)dr[4 + 2],
                                        M_5 = dr[4 + 3] == DBNull.Value ? 0 : (int?)dr[4 + 3],
                                        M_6 = dr[4 + 4] == DBNull.Value ? 0 : (int?)dr[4 + 4],
                                        M_7 = dr[4 + 5] == DBNull.Value ? 0 : (int?)dr[4 + 5],

                                    });
                                }
                                else if ((FirstDay + 4 + 6) - 35 == 4)
                                {
                                    lmd.Add(new Attendance_W_VM
                                    {

                                        //EmpId = dr[37] == null ? "" : dr[0].ToString(),
                                        EmpId = dr[37].ToString(),
                                        EmpNo = (dr[38]).ToString(),
                                        FullName = dr[39].ToString(),
                                        DawamId = (dr["DawamId"]).ToString() ?? "",
                                        StartWorkDate = (dr["WorkStartDate"]).ToString() ?? "",
                                        M_1 = dr[FirstDay + 4] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 ],
                                        M_2 = dr[FirstDay + 4 + 1] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 + 1],
                                        M_3 = dr[FirstDay + 4 + 2] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 + 2],
                                        //M_4 = 0,
                                        //M_5 = 0,
                                        //M_6 = 0,
                                        //M_7 = 0,


                                        M_4 = dr[4 + 1] == DBNull.Value ? 0 : (int?)dr[4 + 1],
                                        M_5 = dr[4 + 2] == DBNull.Value ? 0 : (int?)dr[4 + 2],
                                        M_6 = dr[4 + 3] == DBNull.Value ? 0 : (int?)dr[4 + 3],
                                        M_7 = dr[4 + 4] == DBNull.Value ? 0 : (int?)dr[4 + 4],
                                    });
                                }
                                else if ((FirstDay + 4 + 6) - 35 == 3)
                                {
                                    lmd.Add(new Attendance_W_VM
                                    {

                                        //EmpId = dr[37] == null ? "" : dr[0].ToString(),
                                        EmpId = dr[37].ToString(),
                                        EmpNo = (dr[38]).ToString(),
                                        FullName = dr[39].ToString(),
                                        DawamId = (dr["DawamId"]).ToString() ?? "",
                                        StartWorkDate = (dr["WorkStartDate"]).ToString() ?? "",
                                        M_1 = dr[FirstDay + 4] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4],
                                        M_2 = dr[FirstDay + 4 + 1] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 + 1],
                                        M_3 = dr[FirstDay + 4 + 2] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 + 2],
                                        M_4 = dr[FirstDay + 4 + 3] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 + 3],
                                        //M_5 = 0,
                                        //M_6 = 0,
                                        //M_7 = 0,

                                        M_5 = dr[4 + 1] == DBNull.Value ? 0 : (int?)dr[4 + 1],
                                        M_6 = dr[4 + 2] == DBNull.Value ? 0 : (int?)dr[4 + 2],
                                        M_7 = dr[4 + 3] == DBNull.Value ? 0 : (int?)dr[4 + 3],
                                    });
                                }
                                else if ((FirstDay + 4 + 6) - 35 == 2)
                                {
                                    lmd.Add(new Attendance_W_VM
                                    {

                                        //EmpId = dr[37] == null ? "" : dr[0].ToString(),
                                        EmpId = dr[37].ToString(),
                                        EmpNo = (dr[38]).ToString(),
                                        FullName = dr[39].ToString(),
                                        DawamId = (dr["DawamId"]).ToString() ?? "",
                                        StartWorkDate = (dr["WorkStartDate"]).ToString() ?? "",
                                        M_1 = dr[FirstDay + 4] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4],
                                        M_2 = dr[FirstDay + 4 + 1] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 + 1],
                                        M_3 = dr[FirstDay + 4 + 2] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 + 2],
                                        M_4 = dr[FirstDay + 4 + 3] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 + 3],
                                        M_5 = dr[FirstDay + 4 + 4] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 + 4],
                                        //M_6 = 0,
                                        //M_7 = 0,

                                        M_6 = dr[4 + 1] == DBNull.Value ? 0 : (int?)dr[4 + 1],
                                        M_7 = dr[4 + 2] == DBNull.Value ? 0 : (int?)dr[4 + 2],

                                    });
                                }
                                else if ((FirstDay + 4 + 6) - 35 == 1)
                                {
                                    lmd.Add(new Attendance_W_VM
                                    {

                                        //EmpId = dr[37] == null ? "" : dr[0].ToString(),
                                        EmpId = dr[37].ToString(),
                                        EmpNo = (dr[38]).ToString(),
                                        FullName = dr[39].ToString(),
                                        DawamId = (dr["DawamId"]).ToString() ?? "",
                                        StartWorkDate = (dr["WorkStartDate"]).ToString() ?? "",
                                        M_1 = dr[FirstDay + 4] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 ],
                                        M_2 = dr[FirstDay + 4 + 1] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 + 1],
                                        M_3 = dr[FirstDay + 4 + 2] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 + 2],
                                        M_4 = dr[FirstDay + 4 + 3] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 + 3],
                                        M_5 = dr[FirstDay + 4 + 4] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 + 4],
                                        M_6 = dr[FirstDay + 4 + 5] == DBNull.Value ? 0 : (int?)dr[FirstDay + 4 + 5],
                                        //M_7 = 0,

                                        M_7 = dr[4 + 1] == DBNull.Value ? 0 : (int?)dr[4 + 1],
                                    });
                                }
                            }

                            //FirstDay اول يوم في الاسبوع 

                          
                        }
                    }
                }
                return lmd;
            }
            catch(Exception ex)
            {
                //string msg = ex.Message;
                List<Attendance_W_VM> lmd = new List<Attendance_W_VM>();
                return lmd;
            }

        }


        public static DateTime FirstDayOfWeek(DateTime dt)
        {
            //var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            //var diff = (dt.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek);
            //if (diff < 0)
            //    diff += 7;
            //return dt.AddDays(-diff).Date;
            DayOfWeek startOfWeek = DayOfWeek.Saturday;
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public string ConvertDateCalendar(DateTime DateConv, string Calendar, string DateLangCulture)
        {
            DateTimeFormatInfo DTFormat;
            DateLangCulture = DateLangCulture.ToLower();
            /// We can't have the hijri date writen in English. We will get a runtime error

            if (Calendar == "Hijri" && DateLangCulture.StartsWith("en-"))
            {

            }
            DateLangCulture = "ar-sa";
            /// Set the date time format to the given culture
            DTFormat = new System.Globalization.CultureInfo(DateLangCulture, false).DateTimeFormat;

            /// Set the calendar property of the date time format to the given calendar
            switch (Calendar)
            {
                case "Hijri":
                    DTFormat.Calendar = new System.Globalization.HijriCalendar();
                    break;
                case "Gregorian":
                    DTFormat.Calendar = new System.Globalization.GregorianCalendar();
                    break;
                default:
                    return "";
            }
            /// We format the date structure to whatever we want
            DTFormat.ShortDatePattern = @"yyyy/MM/dd";
            DTFormat.DateSeparator = "-";
            var ss = DateConv.Date.ToString("d", DTFormat);
            return (DateConv.Date.ToString("d", DTFormat));
        }
  

        public async Task<IEnumerable<LateVM>> GetAttendance_Screen(string FromDate, string ToDate,  int? YearId, int Shift, int BranchId,int SwType, string lang, string Con, int UserIDF)
        {
            try
            {
                List<LateVM> lmd = new List<LateVM>();

                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_Attendance_Screen";
                        command.Connection = con;
                        int BrId = 0;


                        command.Parameters.Add(new SqlParameter("@From", (YearId - 2) + "-01-01"));
                        command.Parameters.Add(new SqlParameter("@To", (YearId + 1) + "-12-31"));

                        //command.Parameters.Add(new SqlParameter("@From", "2021-02-27"));
                        //command.Parameters.Add(new SqlParameter("@To",  "2021-02-27"));



                        if (BranchId == 0)
                        {
                            BrId = 0;
                            command.Parameters.Add(new SqlParameter("@BranchId", BrId));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        }
                        command.Parameters.Add(new SqlParameter("@EmpId", UserIDF));

                        command.Parameters.Add(new SqlParameter("@SwType", SwType));
                        //command.Parameters.Add(new SqlParameter("@Year", 1));
                        //command.Parameters.Add(new SqlParameter("@Month", 1));

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            //string x = Convert.ToDateTime(dr[3]).ToString("yyyy-MM-dd");
                            //string x2 = dr[3].ToString();
                            string x3 = dr[3].ToString();
                            CultureInfo arCul = new CultureInfo("ar-SA");
                            HijriCalendar h = new HijriCalendar();
                            arCul.DateTimeFormat.Calendar = h;

                            //var EmpNO_N = dr[0] == null ? "" : dr[0].ToString();
                            //var FullName_N = dr[12].ToString();
                            //var TimeJoin1_N = (!String.IsNullOrWhiteSpace(dr[4].ToString())) ? Convert.ToDateTime(dr[4]).ToString("HH:mm") : dr[13].ToString() != 2.ToString() ? "----" : "";
                            //var TimeLeave1_N = (!String.IsNullOrWhiteSpace(dr[5].ToString())) ? Convert.ToDateTime(dr[5]).ToString("HH:mm") : dr[13].ToString() != 2.ToString() ? "----" : "";
                            //var TimeJoin2_N = (!String.IsNullOrWhiteSpace(dr[6].ToString())) ? Convert.ToDateTime(dr[6]).ToString("HH:mm") : dr[13].ToString() != 1.ToString() ? "----" : "";
                            //var TimeLeave2_N = (!String.IsNullOrWhiteSpace(dr[7].ToString())) ? Convert.ToDateTime(dr[7]).ToString("HH:mm") : dr[13].ToString() != 1.ToString() ? "----" : "";
                            //var DateDay_N = dr[3].ToString() == "" ? "" : DateTime.ParseExact(Convert.ToDateTime(dr[3]).ToString("yyyy-MM-dd"), "yyyy-MM-dd", arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces).ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            //var PhotoUrl = dr[24] == null ? "" : dr[24].ToString();
                            //var EmpId_N = dr[25] == null ? "" : dr[25].ToString();

                            //var xY = 1;
                            lmd.Add(new LateVM
                            {

                                MAXSER = dr[0] == null ? "" : dr[0].ToString(),
                                EmpNo = (dr[27]).ToString(),
                                FullName = dr[28].ToString(),
                                TimeJoin1 = (!String.IsNullOrWhiteSpace(dr[4].ToString())) ? Convert.ToDateTime(dr[4]).ToString("hh:mm tt") : dr[13].ToString() != 2.ToString() ? "----" : "",
                                TimeLeave1 = (!String.IsNullOrWhiteSpace(dr[5].ToString())) ? Convert.ToDateTime(dr[5]).ToString("hh:mm tt") : dr[13].ToString() != 2.ToString() ? "----" : "",
                                TimeJoin2 = (!String.IsNullOrWhiteSpace(dr[6].ToString())) ? Convert.ToDateTime(dr[6]).ToString("hh:mm tt") : dr[13].ToString() != 1.ToString() ? "----" : "",
                                TimeLeave2 = (!String.IsNullOrWhiteSpace(dr[7].ToString())) ? Convert.ToDateTime(dr[7]).ToString("hh:mm tt") : dr[13].ToString() != 1.ToString() ? "----" : "",
                                //DateDay = Convert.ToDateTime(dr[3]).ToString("dd/MM/yyyy"),
                                DawamId = (dr["DawamId1"]).ToString() ?? "",
                                DateDay = dr[3].ToString() == "" ? "" : Convert.ToDateTime(dr[3]).ToString("yyyy-MM-dd"),//dr[3].ToString() == "" ? "" : DateTime.ParseExact(Convert.ToDateTime(dr[3]).ToString("yyyy-MM-dd"), "yyyy-MM-dd", arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces).ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")),
                                PhotoUrl = dr[41] == null ? "" : dr[41].ToString(),
                                EmpId = dr[26] == null ? "" : dr[26].ToString(),
                                StartWorkDate = dr["WorkStartDate"].ToString() ?? "",

                            });


                        }
                    }
                }

                return lmd;
            }
            catch(Exception ex)
            {
                //string msg = ex.Message;
                List<LateVM> lmd = new List<LateVM>();
                return lmd;
            }

        }

    }
}

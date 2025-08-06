using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using System.Globalization;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class AttendeesRepository : IAttendeesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public AttendeesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        public async Task<IEnumerable<AttendeesVM>> GetAllAttendees(int BranchId)
        {
            var Attendees = _TaamerProContext.Attendees.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.EmpId==0).Select(x => new AttendeesVM
            {
               AttendeesId= x.AttendeesId,
                EmpId = x.EmpId,
                Date = x.Date,
                Day = x.Day,
                Status = x.Status,
                WorkMinutes = x.WorkMinutes,
                ActualWorkMinutes = x.ActualWorkMinutes,
                IsOverTime = x.IsOverTime,
                IsLate = x.IsLate,
                IsDone = x.IsDone,
                BranchId = x.BranchId,
                EmployeeName = x.Employees.EmployeeNameAr,
                LateMinutes = x.LateMinutes,
                IsEntry = x.IsEntry,
                LateCheckInMinutes = x.LateCheckInMinutes,
                EarlyCheckOutMin = x.EarlyCheckOutMin,
                IsRealVacancy = x.IsRealVacancy,
                IsOut = x.IsOut,
                Bonus = x.Bonus,
                AttTimeId = x.AttTimeId,
                Discount = x.Discount,
                IsEarlyCheckOut = x.IsEarlyCheckOut,
                OverTimeMinutes= x.OverTimeMinutes,
                EmployeeMobile = x.Employees.Mobile
            });
            return Attendees;
        }
        public async Task<IEnumerable<AttendeesVM>> GetAttendeesbyStatus(int Status, string Date, int BranchId)                
        {
            var Attendees = _TaamerProContext.Attendees.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Status == Status && s.Date == Date).Select(x => new AttendeesVM
            {
                AttendeesId = x.AttendeesId,
                EmpId = x.EmpId,
                Date = x.Date,
                Day = x.Day,
                Status = x.Status,
                WorkMinutes = x.WorkMinutes,
                ActualWorkMinutes = x.ActualWorkMinutes,
                IsOverTime = x.IsOverTime,
                IsLate = x.IsLate,
                IsDone = x.IsDone,
                BranchId = x.BranchId,
                EmployeeName = x.Employees.EmployeeNameAr,
                LateMinutes = x.LateMinutes,
                IsEntry = x.IsEntry,
                LateCheckInMinutes = x.LateCheckInMinutes,
                EarlyCheckOutMin = x.EarlyCheckOutMin,
                IsRealVacancy = x.IsRealVacancy,
                IsOut = x.IsOut,
                Bonus = x.Bonus,
                AttTimeId = x.AttTimeId,
                Discount = x.Discount,
                IsEarlyCheckOut = x.IsEarlyCheckOut,
                OverTimeMinutes = x.OverTimeMinutes,
                IsLateCheckIn = x.IsLateCheckIn,
                IsVacancy = x.IsVacancy,
                IsVacancyEmp = x.IsVacancyEmp,
                DayOfWeek = x.DayOfWeek,
                EmployeeMobile = x.Employees.Mobile

            });
            return Attendees;
        }
           
        public async Task<IEnumerable<AttendeesVM>> GetAttendeeslate(bool IsLate, string Date, int BranchId)                          
        {
            var Attendees = _TaamerProContext.Attendees.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.IsLate == IsLate && s.Date == Date)  .Select(x => new AttendeesVM      
            {
                 AttendeesId = x.AttendeesId,
                EmpId = x.EmpId,
                Date = x.Date,
                Day = x.Day,
                Status = x.Status,
                WorkMinutes = x.WorkMinutes,
                ActualWorkMinutes = x.ActualWorkMinutes,
                IsOverTime = x.IsOverTime,
                IsLate = x.IsLate,
                IsDone = x.IsDone,
                BranchId = x.BranchId,
                EmployeeName = x.Employees.EmployeeNameAr,
                LateMinutes = x.LateMinutes,
                IsEntry = x.IsEntry,
                LateCheckInMinutes = x.LateCheckInMinutes,
                EarlyCheckOutMin = x.EarlyCheckOutMin,
                IsRealVacancy = x.IsRealVacancy,
                IsOut = x.IsOut,
                Bonus = x.Bonus,
                AttTimeId = x.AttTimeId,
                Discount = x.Discount,
                IsEarlyCheckOut = x.IsEarlyCheckOut,
                OverTimeMinutes = x.OverTimeMinutes,
                IsLateCheckIn = x.IsLateCheckIn,
                IsVacancy = x.IsVacancy,
                IsVacancyEmp = x.IsVacancyEmp,
                DayOfWeek = x.DayOfWeek,
                EmployeeMobile = x.Employees.Mobile

            });
            return Attendees;
        }
           
        public async Task<IEnumerable<AttendeesVM>> GetAttendeesEarlyCheckOut(bool IsEarlyCheckOut, string Date, int BranchId)
        {
            var Attendees = _TaamerProContext.Attendees.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.IsEarlyCheckOut == IsEarlyCheckOut && s.Date ==Date)
             .Select(x => new AttendeesVM
             {
                 AttendeesId = x.AttendeesId,
                EmpId = x.EmpId,
                Date = x.Date,
                Day = x.Day,
                Status = x.Status,
                WorkMinutes = x.WorkMinutes,
                ActualWorkMinutes = x.ActualWorkMinutes,
                IsOverTime = x.IsOverTime,
                IsLate = x.IsLate,
                IsDone = x.IsDone,
                BranchId = x.BranchId,
                EmployeeName = x.Employees.EmployeeNameAr,
                LateMinutes = x.LateMinutes,
                IsEntry = x.IsEntry,
                LateCheckInMinutes = x.LateCheckInMinutes,
                EarlyCheckOutMin = x.EarlyCheckOutMin,
                IsRealVacancy = x.IsRealVacancy,
                IsOut = x.IsOut,
                Bonus = x.Bonus,
                AttTimeId = x.AttTimeId,
                Discount = x.Discount,
                IsEarlyCheckOut = x.IsEarlyCheckOut,
                OverTimeMinutes = x.OverTimeMinutes,
                IsLateCheckIn = x.IsLateCheckIn,
                IsVacancy = x.IsVacancy,
                IsVacancyEmp = x.IsVacancyEmp,
                DayOfWeek = x.DayOfWeek,
                EmployeeMobile = x.Employees.Mobile

            });
            return Attendees;
        }
             
        public async Task<IEnumerable<AttendeesVM>> GetAttendeesOut(bool IsOut, string Date, int BranchId)
        {
                                   var Attendees = _TaamerProContext.Attendees.Where(s => s.IsDeleted == false  && s.BranchId == BranchId && s.IsOut ==IsOut && s.Date== Date)
                                    .Select(x => new AttendeesVM { 
                                        AttendeesId = x.AttendeesId,
                                        EmpId = x.EmpId,
                                        Date = x.Date,
                                        Day = x.Day,
                                        Status = x.Status,
                                        WorkMinutes = x.WorkMinutes,
                                        ActualWorkMinutes = x.ActualWorkMinutes,
                                        IsOverTime = x.IsOverTime,
                                        IsLate = x.IsLate,
                                        IsDone = x.IsDone,
                                        BranchId = x.BranchId,
                                        EmployeeName = x.Employees.EmployeeNameAr,
                                        LateMinutes = x.LateMinutes,
                                        IsEntry = x.IsEntry,
                                        LateCheckInMinutes = x.LateCheckInMinutes,
                                        EarlyCheckOutMin = x.EarlyCheckOutMin,
                                        IsRealVacancy = x.IsRealVacancy,
                                        IsOut = x.IsOut,
                                        Bonus = x.Bonus,
                                        AttTimeId = x.AttTimeId,
                                        Discount = x.Discount,
                                        IsEarlyCheckOut = x.IsEarlyCheckOut,
                                        OverTimeMinutes = x.OverTimeMinutes,
                                        IsLateCheckIn = x.IsLateCheckIn,
                                        IsVacancy = x.IsVacancy,
                                        IsVacancyEmp = x.IsVacancyEmp,
                                        DayOfWeek = x.DayOfWeek,
                                        EmployeeMobile = x.Employees.Mobile

                                    });
                                    return Attendees;
                                }
        public async Task<IEnumerable<AttendeesVM>> GetAllEmpAbsence(int Status, string Date, int BranchId)
        {
   var Attendees = _TaamerProContext.Attendees.Where(s => s.IsDeleted == false)
    .Select(x => new AttendeesVM
    {
        AttendeesId = x.AttendeesId,
        EmpId = x.EmpId,
        Date = x.Date,
        Day = x.Day,
        Status = x.Status,
        WorkMinutes = x.WorkMinutes,
        ActualWorkMinutes = x.ActualWorkMinutes,
        IsOverTime = x.IsOverTime,
        IsLate = x.IsLate,
        IsDone = x.IsDone,
        BranchId = x.BranchId,
        EmployeeName = x.Employees.EmployeeNameAr,
        LateMinutes = x.LateMinutes,
        IsEntry = x.IsEntry,
        LateCheckInMinutes = x.LateCheckInMinutes,
        EarlyCheckOutMin = x.EarlyCheckOutMin,
        IsRealVacancy = x.IsRealVacancy,
        IsOut = x.IsOut,
        Bonus = x.Bonus,
        AttTimeId = x.AttTimeId,
        Discount = x.Discount,
        IsEarlyCheckOut = x.IsEarlyCheckOut,
        OverTimeMinutes = x.OverTimeMinutes,
        IsLateCheckIn = x.IsLateCheckIn,
        IsVacancy = x.IsVacancy,
        IsVacancyEmp = x.IsVacancyEmp,
        DayOfWeek = x.DayOfWeek,
        EmployeeMobile = x.Employees.Mobile
    }).ToList().Where(s => (s.BranchId == BranchId && s.Status == Status && DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture).Month == DateTime.ParseExact(Date, "yyyy-MM-dd", CultureInfo.InvariantCulture).Month));
            return Attendees;
        }
        public async Task<IEnumerable<AttendeesVM>> GetAllEmplate(bool IsLateCheckIn, string Date, int BranchId)
        {
            var Attendees = _TaamerProContext.Attendees.Where(s => s.IsDeleted == false)
            .Select(x => new AttendeesVM
            {
                AttendeesId = x.AttendeesId,
                EmpId = x.EmpId,
                Date = x.Date,
                Day = x.Day,
                Status = x.Status,
                WorkMinutes = x.WorkMinutes,
                ActualWorkMinutes = x.ActualWorkMinutes,
                IsOverTime = x.IsOverTime,
                IsLate = x.IsLate,
                IsDone = x.IsDone,
                BranchId = x.BranchId,
                EmployeeName = x.Employees.EmployeeNameAr,
                LateMinutes = x.LateMinutes,
                IsEntry = x.IsEntry,
                LateCheckInMinutes = x.LateCheckInMinutes,
                EarlyCheckOutMin = x.EarlyCheckOutMin,
                IsRealVacancy = x.IsRealVacancy,
                IsOut = x.IsOut,
                Bonus = x.Bonus,
                AttTimeId = x.AttTimeId,
                Discount = x.Discount,
                IsEarlyCheckOut = x.IsEarlyCheckOut,
                OverTimeMinutes = x.OverTimeMinutes,
                IsLateCheckIn = x.IsLateCheckIn,
                IsVacancy = x.IsVacancy,
                IsVacancyEmp = x.IsVacancyEmp,
                DayOfWeek = x.DayOfWeek,
                EmployeeMobile = x.Employees.Mobile,
                AttendenceDifference = x.ActualWorkMinutes- x.WorkMinutes,
            }).ToList().Where(s => (s.BranchId == BranchId && s.IsLateCheckIn == IsLateCheckIn && DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture).Month == DateTime.ParseExact(Date, "yyyy-MM-dd", CultureInfo.InvariantCulture).Month));
           return Attendees;
        }
        public async Task<IEnumerable<AttendeesVM>> GetAllEmpAbsencelate( string Date, int BranchId)
        {
            var Attendees = _TaamerProContext.Attendees.Where(s => s.IsDeleted == false)
            .Select(x => new AttendeesVM
            {
                AttendeesId = x.AttendeesId,
                EmpId = x.EmpId,
                Date = x.Date,
                Day = x.Day,
                Status = x.Status,
                WorkMinutes = x.WorkMinutes,
                ActualWorkMinutes = x.ActualWorkMinutes,
                IsOverTime = x.IsOverTime,
                IsLate = x.IsLate,
                IsDone = x.IsDone,
                BranchId = x.BranchId,
                EmployeeName = x.Employees.EmployeeNameAr,
                LateMinutes = x.LateMinutes,
                IsEntry = x.IsEntry,
                LateCheckInMinutes = x.LateCheckInMinutes,
                EarlyCheckOutMin = x.EarlyCheckOutMin,
                IsRealVacancy = x.IsRealVacancy,
                IsOut = x.IsOut,
                Bonus = x.Bonus,
                AttTimeId = x.AttTimeId,
                Discount = x.Discount,
                IsEarlyCheckOut = x.IsEarlyCheckOut,
                OverTimeMinutes = x.OverTimeMinutes,
                IsLateCheckIn = x.IsLateCheckIn,
                IsVacancy = x.IsVacancy,
                IsVacancyEmp = x.IsVacancyEmp,
                DayOfWeek = x.DayOfWeek,
                EmployeeMobile = x.Employees.Mobile,
                AttendenceDifference = x.ActualWorkMinutes - x.WorkMinutes,
            }).ToList().Where(s => (s.BranchId == BranchId &&  DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture).Month == DateTime.ParseExact(Date, "yyyy-MM-dd", CultureInfo.InvariantCulture).Month));
            return Attendees;
        }
    }
}

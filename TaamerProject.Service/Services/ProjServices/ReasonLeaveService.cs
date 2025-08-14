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
using TaamerProject.Service.Generic;
using TaamerProject.Repository.Repositories;
using TaamerP.Service.LocalResources;
using TaamerP.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace TaamerProject.Service.Services
{
    public class ReasonLeaveService :   IReasonLeaveService
    {
        private readonly IReasonLeaveRepository _reasonLeaveRepository;
 
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public ReasonLeaveService(IReasonLeaveRepository reasonLeaveRepository,
            TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _reasonLeaveRepository = reasonLeaveRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;

        }

        public Task<IEnumerable<ReasonLeaveVM>> GetAllreasons(string SearchText)
        {
            var reasons = _reasonLeaveRepository.GetAllreasons(SearchText);
            return reasons;
        }

        public Task<ReasonLeaveVM> Getreasonbyid(int ReasonId)
        {
            var reasons = _reasonLeaveRepository.Getreasonbyid(ReasonId);
            return reasons;
        }
        public GeneralMessage SaveReason(ReasonLeave reson, int UserId, int BranchId)
        {
            try
            {
     
                if (reson.ReasonId == 0)
                {
                    reson.AddUser = UserId;
                    reson.AddDate = DateTime.Now;
                    _TaamerProContext.ReasonLeave.Add(reson);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة سبب جديد";
                    _SystemAction.SaveAction("SaveJob", "JobService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    //var reasonUpdated = _reasonLeaveRepository.GetById(reson.ReasonId);
                    ReasonLeave? reasonUpdated = _TaamerProContext.ReasonLeave.Where(s => s.ReasonId == reson.ReasonId).FirstOrDefault();

                    if (reasonUpdated != null)
                    {
                        //JobUpdated.JobCode = job.JobCode;
                        reasonUpdated.ReasonTxt = reson.ReasonTxt;
                        reasonUpdated.DesecionTxt = reson.DesecionTxt;
              
                        reasonUpdated.UpdateUser = UserId;
                        reasonUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل سبب رقم " + reson.ReasonId;
                    _SystemAction.SaveAction("SaveJob", "JobService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_EditedSuccessfully };

                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ السبب";
                _SystemAction.SaveAction("SaveJob", "JobService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteReason(int ReasonId, int UserId, int BranchId)
        {
            try
            {
                //ReasonLeave reson = _reasonLeaveRepository.GetById(ReasonId);
                ReasonLeave? reson = _TaamerProContext.ReasonLeave.Where(s => s.ReasonId == ReasonId).FirstOrDefault();
                if (reson != null)
                {
                    if (ReasonId == 1 || ReasonId == 2 || ReasonId == 3 || ReasonId == 4)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = Resources.General_SavedFailed;
                        _SystemAction.SaveAction("DeleteReason", "ResonLeaveService", 1, Resources.CanNotDeleteMainReason, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.CanNotDeleteMainReason };
                    }
                    reson.IsDeleted = true;
                    reson.DeleteDate = DateTime.Now;
                    reson.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف سبب رقم " + ReasonId;
                    _SystemAction.SaveAction("DeleteClause", "Acc_ClausesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ السبب";
                _SystemAction.SaveAction("SaveJob", "JobService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public Task<IEnumerable<ReasonLeaveVM>> FillReasonSelect(string SearchText = "")
        {
            var resonLeave = _reasonLeaveRepository.GetAllreasons(SearchText);
            return resonLeave;
        }

        public EmployeeEndWork GetEmployeeWithWorkPeriods(int empId, int reasonLeave, string date)
        {
            var today = DateTime.Today;
            var enddate = today;
            if (!string.IsNullOrEmpty(date))
                enddate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var employee = _TaamerProContext.Employees
                .Include(x => x.Nationality)
                .FirstOrDefault(e => e.EmployeeId == empId);

            if (employee == null) return null;

            var firstContractDate = _TaamerProContext.EmpContract
                .Where(c => c.EmpId == empId && !c.IsDeleted)
                .AsEnumerable()
                .OrderBy(c => DateTime.Parse(c.StartWorkDate))
                .Select(c => DateTime.Parse(c.StartWorkDate))
                .FirstOrDefault();

            var latestContract = _TaamerProContext.EmpContract
                .Where(c => c.EmpId == empId && !c.IsDeleted)
                .AsEnumerable()
                .OrderByDescending(c => DateTime.Parse(c.EndWorkDate ?? today.ToString("yyyy-MM-dd")))
                .FirstOrDefault();

            var latestContractEndDate = enddate;

            var empDto = new EmployeeEndWork
            {
                EmpId = employee.EmployeeId,
                EmpName = employee.EmployeeNameAr,
                Nationality = employee.Nationality.NameAr,
                BirthDate = employee.BirthDate,
                Age = employee.Age,
                IdNumber = employee.NationalityId,
                IdExpiryDate = employee.NationalIdEndDate,
                HireDate = firstContractDate,
                CurrentContractEndDate = latestContractEndDate,
                LastBasicSalary = employee.Salary + employee.Allowances + employee.OtherAllownces,
            };

            empDto.WorkPeriods = _TaamerProContext.EmpContract
                .Where(c => c.EmpId == empId && !c.IsDeleted)
                .Select(c => new
                {
                    StartDate = c.StartWorkDate != null ? DateTime.Parse(c.StartWorkDate) : (DateTime?)null,
                    EndDate = c.EndWorkDate != null ? DateTime.Parse(c.EndWorkDate) : enddate,
                    Salary = c.FreelanceAmount,
                    Allowances = employee.Allowances + employee.OtherAllownces,
                    WorkingDaysPerWeek = 7,
                    EndType = reasonLeave
                })
                .AsEnumerable()
                .Select(c =>
                {
                    var start = c.StartDate ?? today;
                    var end = c.EndDate;

                    // Adjust for working days per week
                    var adjustedEnd = end;
                    if (c.WorkingDaysPerWeek < 7)
                    {
                        int totalDays = (end - start).Days + 1;
                        double ratio = c.WorkingDaysPerWeek / 7.0;
                        int adjustedTotalDays = (int)Math.Round(totalDays * ratio);
                        adjustedEnd = start.AddDays(adjustedTotalDays - 1);
                    }

                    // Accurate years, months, days
                    var (years, months, days) = CalculateServicePeriod(start, adjustedEnd);

                    decimal totalMonthly = (c.Salary ?? 0) + (c.Allowances ?? 0);
                    decimal dailyRate = totalMonthly / 30;

                    decimal firstFiveReward = 0;
                    decimal afterFiveReward = 0;

                    if (c.EndType == 1) // استقالة
                    {
                        if (years < 2)
                        {
                            firstFiveReward = 0;
                            afterFiveReward = 0;
                        }
                        else
                        {
                            // If less than or equal to 5 years
                            if (years < 5 || (years == 5 && (months > 0 || days > 0)))
                            {
                                firstFiveReward = ((years * (totalMonthly / 2)) +
                                                   (months * ((totalMonthly / 2 / 12) / 1)) +
                                                   (days * (totalMonthly / 2 / 12 / 30))) / 3;
                            }
                            else if ((years > 5 && years <= 10))
                            {
                                // First 5 years reward
                                firstFiveReward = (5 * (totalMonthly / 2)) * (2m / 3m);

                                // Remaining period after 5 years
                                int extraYears = years - 5;
                                decimal afterYears = extraYears * totalMonthly;
                                decimal afterMonths = months * (totalMonthly / 12);
                                decimal afterDays = days * (totalMonthly / 12 / 30);

                                afterFiveReward = (afterYears + afterMonths + afterDays) * (2m / 3m);
                            }
                            else
                            {
                                firstFiveReward = 5 * (totalMonthly / 2);

                                int extraYears = years - 5;
                                decimal afterYears = extraYears * totalMonthly;
                                decimal afterMonths = months * (totalMonthly / 12);
                                decimal afterDays = days * (totalMonthly / 12 / 30);
                                afterFiveReward = afterYears + afterMonths + afterDays;


                            }
                        }
                    }
                    else // نهاية خدمة عادية
                    {
                        if (years < 5 || (years == 5 && (months > 0 || days > 0)))
                        {
                            firstFiveReward = (years * (totalMonthly / 2)) +
                                              (months * (((totalMonthly) / 2 / 12) / 1)) +
                                              (days * (((totalMonthly) / 2) / 12 / 30));
                        }
                        else
                        {
                            firstFiveReward = 5 * (totalMonthly / 2);

                            int extraYears = years - 5;
                            decimal afterYears = extraYears * totalMonthly;
                            decimal afterMonths = months * (totalMonthly / 12);
                            decimal afterDays = days * (totalMonthly / 12 / 30);
                            afterFiveReward = afterYears + afterMonths + afterDays;
                        }
                    }

                    decimal totalReward = firstFiveReward + afterFiveReward;

                    return new WorkPeriodVM
                    {
                        StartDate = start,
                        EndDate = end,
                        Years = years,
                        Months = months,
                        Days = days,
                        Salary = c.Salary,
                        Allowances = c.Allowances,
                        WorkingDaysPerWeek = c.WorkingDaysPerWeek,
                        EndType = c.EndType,
                        FirstFiveYearsReward = Math.Round(firstFiveReward, 2),
                        AfterFiveYearsReward = Math.Round(afterFiveReward, 2),
                        EndOfServiceReward = Math.Round(totalReward, 2)
                    };
                })
                .ToList();

            empDto.TotalEndOfServiceReward = empDto.WorkPeriods.Sum(p => p.EndOfServiceReward);

            if (latestContractEndDate != null)
            {
                var monthDays = DateTime.DaysInMonth(latestContractEndDate.Year, latestContractEndDate.Month);
                var workedDaysInLastMonth = latestContractEndDate.Day;
                var dailyRate = (employee.Salary ?? 0) / monthDays;
                var dailyAllowance = ((employee.Allowances ?? 0) + (employee.OtherAllownces ?? 0)) / monthDays;
                empDto.LastMonthDueSalary = Math.Round((dailyRate + dailyAllowance) * workedDaysInLastMonth, 2);
            }

            empDto.TotalDue = (employee.VacationEndCount ?? 0) *
                              ((employee.Salary ?? 0) + (employee.Allowances ?? 0) + (employee.OtherAllownces ?? 0)) / 30;
            empDto.VacationEncashment = empDto.TotalDue;

            empDto.TotalDue = empDto.TotalEndOfServiceReward + empDto.TotalDue;
            // empDto.TotalDue = empDto.TotalEndOfServiceReward + empDto.LastMonthDueSalary + empDto.TotalDue;

            var monthLoans = latestContractEndDate.Month;
            var yearLoans = latestContractEndDate.Year;

            var loans = (from loan in _TaamerProContext.Loan
                         join details in _TaamerProContext.LoanDetails
                         on loan.LoanId equals details.LoanId
                         where loan.EmployeeId == empId
                               && !loan.IsDeleted
                               && !details.IsDeleted
                               && details.Date != null
                               && details.Date.Value.Month == monthLoans
                               && details.Date.Value.Year == yearLoans
                         select details.Amount ?? 0).Sum();

            empDto.LastMonthLoans = loans;
            empDto.NetPayable = empDto.TotalDue - loans;

            return empDto;
        }
        // Helper method for accurate period calculation
        private (int Years, int Months, int Days) CalculateServicePeriod(DateTime start, DateTime end)
        {
            int years = end.Year - start.Year;
            int months = end.Month - start.Month;
            int days = end.Day - start.Day + 1;

            if (days < 0)
            {
                months--;
                var prevMonth = end.AddMonths(-1);
                days += DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);
            }

            if (months < 0)
            {
                years--;
                months += 12;
            }

            return (years, months, days);
        }

    }
}

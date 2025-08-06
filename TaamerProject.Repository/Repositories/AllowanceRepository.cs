using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class AllowanceRepository : IAllowanceRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public AllowanceRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        public async Task<IEnumerable<AllowanceVM>> GetAllAllowances(int? EmpId, string SearchText, bool? IsSalaryPart = null)
        {
            var Allowances = _TaamerProContext.Allowance.Where(s => s.IsDeleted == false && s.EmployeeId == EmpId &&
            (IsSalaryPart.HasValue? s.AllowanceType.IsSalaryPart == IsSalaryPart.Value : true)).Select(x => new AllowanceVM
            {
                AllowanceId = x.AllowanceId,
                EmployeeId = x.EmployeeId,
                AllowanceTypeId = x.AllowanceTypeId,
                Date = x.Date,
                StartDate = x.StartDate,
                EndDate= x.EndDate,
                Month = x.Month != null ? x.Month.ToString() : "" ,
                IsFixed = x.IsFixed,
                AllowanceAmount = x.AllowanceAmount,
                UserId = x.UserId,
                AllowanceTypeName = x.AllowanceType.NameAr,
                AllowanceMonthNo=x.AllowanceMonthNo??0,
            });
            if (SearchText != "")
            {
                Allowances = Allowances.Where(s => s.AllowanceAmount.ToString().Contains(SearchText.Trim()) || s.AllowanceTypeName.Contains(SearchText.Trim()));
            }
            return Allowances;
        }
        public async Task<IEnumerable<AllowanceVM>> GetAllAllowancesSearch()
        {
            var Allowances = _TaamerProContext.Allowance.Where(s => s.IsDeleted == false && (s.AllowanceType.IsSalaryPart.Value == false || !s.AllowanceType.IsSalaryPart.HasValue)).Select(x => new AllowanceVM
            {
                AllowanceId = x.AllowanceId,
                EmployeeId = x.EmployeeId,
                AllowanceTypeId = x.AllowanceTypeId,
                Date = x.Date,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Month = x.Month != null ? x.Month.ToString() : "",
                IsFixed = x.IsFixed,
                AllowanceAmount = x.AllowanceAmount,
                UserId = x.UserId,
                AllowanceTypeName = x.AllowanceType.NameAr,
                EmployeeName = x.Employees.EmployeeNameAr,
                AllowanceMonthNo = x.AllowanceMonthNo ?? 0,
            });
            return Allowances;
        }
        public async Task<IEnumerable<AllowanceVM>> GetAllAllowancesBySearchObject(AllowanceVM AllowanceSearch)
        {
            bool? isfixed = null;
            if (AllowanceSearch.Status == 1) {
                isfixed = true;
            } else if (AllowanceSearch.Status == 2)
            {
                isfixed  = false;
            }
            else
            {
                isfixed = null;
            }
            var Allowances = _TaamerProContext.Allowance.Where(s => s.IsDeleted == false && (s.AllowanceType.IsSalaryPart.Value == false || !s.AllowanceType.IsSalaryPart.HasValue) &&
                                                                      (s.EmployeeId == AllowanceSearch.EmployeeId || AllowanceSearch.EmployeeId == null) &&
                                                                      (s.AllowanceTypeId == AllowanceSearch.AllowanceTypeId ||  AllowanceSearch.AllowanceTypeId == null) &&
                                                                      (s.IsFixed == isfixed || isfixed == null )).Select(x => new AllowanceVM
            {
                AllowanceId = x.AllowanceId,
                EmployeeId = x.EmployeeId,
                AllowanceTypeId = x.AllowanceTypeId,
                Date = x.Date,
                StartDate = x.StartDate,
                EndDate= x.EndDate,
                Month = x.Month != null ? x.Month.ToString() : "" ,
                IsFixed = x.IsFixed,
                AllowanceAmount = x.AllowanceAmount,
                UserId = x.UserId,
                AllowanceTypeName = x.AllowanceType.NameAr,
                EmployeeName = x.Employees.EmployeeNameAr,
                AllowanceMonthNo = x.AllowanceMonthNo ?? 0,
                                                                      });
            return Allowances;
        }


        /// <summary>
        /// Collect Allowances for payroll
        /// </summary>
        /// <param name="EmpId"></param>
        /// <param name="StartDate">Can be the start of Month or the Work start date for this employee (if it the first month of work)</param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public async Task<decimal> GetAllownacesSumForPayroll(int EmpId, DateTime StartDate, DateTime EndDate)
        {
            
            var Allowances = _TaamerProContext.Allowance.Where(s => s.IsDeleted == false && s.EmployeeId == EmpId &&
             (!s.AllowanceType.IsSalaryPart.HasValue || (s.AllowanceType.IsSalaryPart.HasValue && !s.AllowanceType.IsSalaryPart.Value) ) &&
             s.StartDate.Value.Month <= StartDate.Month && (!s.EndDate.HasValue || (s.EndDate.HasValue &&  s.EndDate.Value >= StartDate) )) 
                // تاريخ البداية أقل من بداية الشهر أو المباشرة و تاريخ النهاية لو موجود يبقى أكبر من بداية الشهر   
             .Select(x => new AllowanceVM
             {
                 AllowanceId = x.AllowanceId,
                 EmployeeId = x.EmployeeId,
                 AllowanceTypeId = x.AllowanceTypeId,
                 Date = x.Date,
                 StartDate = x.StartDate,
                 EndDate = x.EndDate,
                 Month = x.Month != null ? x.Month.ToString() : "",
                 IsFixed = x.IsFixed,
                 AllowanceAmount = x.AllowanceAmount ?? 0,
                 UserId = x.UserId,
                 AllowanceTypeName = x.AllowanceType.NameAr,
                 AllowanceMonthNo = x.AllowanceMonthNo ?? 0,
             }).ToList();

            decimal total = 0;
            foreach (var item in Allowances)
            {
                //تاريخ البداية و النهاية يبقى ضمن الفترة
                if (item.StartDate < StartDate)
                    item.StartDate = StartDate;

                if (!item.EndDate.HasValue)
                    item.EndDate = EndDate;

                else if (item.EndDate > EndDate)
                    item.EndDate = EndDate;

                var diff = item.EndDate.Value.Subtract(item.StartDate.Value).Days;
                var amountPerDiff= (item.AllowanceAmount.Value / 30) * diff;
                total += item.AllowanceAmount.Value;
            }
            return total;
        }



        public async Task<decimal> GetAllownacesSumForPayroll2(int EmpId, int MonthNo)
        {

            var Allowances = _TaamerProContext.Allowance.Where(s => s.IsDeleted == false && s.EmployeeId == EmpId &&
             (!s.AllowanceType.IsSalaryPart.HasValue || (s.AllowanceType.IsSalaryPart.HasValue && !s.AllowanceType.IsSalaryPart.Value)) && s.AllowanceMonthNo == MonthNo)
             // تاريخ البداية أقل من بداية الشهر أو المباشرة و تاريخ النهاية لو موجود يبقى أكبر من بداية الشهر   
             .Select(x => new AllowanceVM
             {
                 AllowanceId = x.AllowanceId,
                 EmployeeId = x.EmployeeId,
                 AllowanceTypeId = x.AllowanceTypeId,
                 Date = x.Date,
                 StartDate = x.StartDate,
                 EndDate = x.EndDate,
                 Month = x.Month != null ? x.Month.ToString() : "",
                 IsFixed = x.IsFixed,
                 AllowanceAmount = x.AllowanceAmount ?? 0,
                 UserId = x.UserId,
                 AllowanceTypeName = x.AllowanceType.NameAr,
                 AllowanceMonthNo = x.AllowanceMonthNo ?? 0,
             }).ToList();

            decimal total = 0;
            foreach (var item in Allowances)
            {
        
                total += item.AllowanceAmount.Value;
            }
            return total;
        }


    }
}



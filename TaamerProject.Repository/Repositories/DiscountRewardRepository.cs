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
    public class DiscountRewardRepository :IDiscountRewardRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public DiscountRewardRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<DiscountRewardVM>> GetAllDiscountRewards(int? EmpId, string SearchText)
        {
            var DiscountRewards = _TaamerProContext.DiscountReward.Where(s => s.IsDeleted == false && s.EmployeeId == EmpId).Select(x => new DiscountRewardVM
            {
                DiscountRewardId = x.DiscountRewardId,
                EmployeeId = x.EmployeeId,
                Amount = x.Amount,
                Date = x.Date,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                HijriDate = x.HijriDate,
                Type = x.Type,
                Notes = x.Notes,
                MonthNo=x.MonthNo,
                DiscountRewardName = x.Type == 1 ? "خصم" : "مكافأة"
            }).ToList();
            if (SearchText != "")
            {
                DiscountRewards = DiscountRewards.Where(s => s.DiscountRewardName.Contains(SearchText.Trim()) || s.Amount.ToString().Contains(SearchText.Trim())).ToList();
            }
            return DiscountRewards;
        }


        /// <summary>
        /// Collect Allowances for payroll
        /// </summary>
        /// <param name="EmpId"></param>
        /// <param name="StartDate">Can be the start of Month or the Work start date for this employee (if it the first month of work)</param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public async Task<decimal> GetDiscountRewordSumForPayroll(int EmpId, DateTime StartDate, DateTime EndDate, int Type)
        {
            try { 

            var items = _TaamerProContext.DiscountReward.Where(s => s.IsDeleted == false && s.EmployeeId == EmpId &&
             s.StartDate.Value.Month <= StartDate.Month && (!s.EndDate.HasValue || (s.EndDate.HasValue && s.EndDate.Value > StartDate)) && s.Type == Type)
             // تاريخ البداية أقل من بداية الشهر أو المباشرة و تاريخ النهاية لو موجود يبقى أكبر من بداية الشهر   
             .Select(x => new DiscountRewardVM
             {
                 DiscountRewardId = x.DiscountRewardId,
                 EmployeeId = x.EmployeeId,
                 Amount = x.Amount,
                 Date = x.Date,
                 StartDate = x.StartDate,
                 EndDate = x.EndDate,
                 HijriDate = x.HijriDate,
                 Type = x.Type,
                 Notes = x.Notes,
                 MonthNo=x.MonthNo,
                 DiscountRewardName = x.Type == 1 ? "خصم" : "مكافأة"
             }).ToList();

            decimal total = 0;
            foreach (var item in items)
            {
                //تاريخ البداية و النهاية يبقى ضمن الفترة
                if (item.StartDate < StartDate)
                    item.StartDate = StartDate;

                if (!item.EndDate.HasValue)
                    item.EndDate = EndDate;

                else if (item.EndDate > EndDate)
                    item.EndDate = EndDate;

                var diff = item.EndDate.Value.Subtract(item.StartDate.Value).Days;
                var amountPerDiff = (item.Amount.Value / 30) * diff;
                total += item.Amount.Value;
            }
            return total;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<decimal> GetDiscountRewordSumForPayroll2(int EmpId, int Monthno, int Type)
        {

            var items = _TaamerProContext.DiscountReward.Where(s => s.IsDeleted == false && s.EmployeeId == EmpId &&s.MonthNo==Monthno && s.Type == Type)
             // تاريخ البداية أقل من بداية الشهر أو المباشرة و تاريخ النهاية لو موجود يبقى أكبر من بداية الشهر   
             .Select(x => new DiscountRewardVM
             {
                 DiscountRewardId = x.DiscountRewardId,
                 EmployeeId = x.EmployeeId,
                 Amount = x.Amount,
                 Date = x.Date,
                 StartDate = x.StartDate,
                 EndDate = x.EndDate,
                 HijriDate = x.HijriDate,
                 Type = x.Type,
                 Notes = x.Notes,
                 MonthNo = x.MonthNo,
                 DiscountRewardName = x.Type == 1 ? "خصم" : "مكافأة"
             }).ToList();

            decimal total = 0;
            foreach (var item in items)
            {
                total += item.Amount.Value;
            }
            return total;
        }
    }
}



using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IDiscountRewardRepository
    {
        Task<IEnumerable<DiscountRewardVM>> GetAllDiscountRewards(int? EmpId, string SearchText);
        Task<decimal> GetDiscountRewordSumForPayroll(int EmpId, DateTime StartDate, DateTime EndDate, int Type);

        Task<decimal> GetDiscountRewordSumForPayroll2(int EmpId, int Monthno, int Type);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IDiscountRewardService 
    {
        Task<IEnumerable<DiscountRewardVM>> GetAllDiscountRewards(int? EmpId, string SearchText);
        GeneralMessage SaveDiscountReward(DiscountReward discountReward,int UserId, int BranchId, int? yearid);
        GeneralMessage DeleteDiscountReward(int DiscountRewardId,int UserId,int BranchId);
        Task<decimal> GetDiscountRewordSumForPayroll(int EmpId, DateTime StartDate, DateTime EndDate, int Type);
    }
}

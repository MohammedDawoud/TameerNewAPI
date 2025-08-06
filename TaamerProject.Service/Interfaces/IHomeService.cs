using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IHomeService
    {
        StatisticsVM GetAllStatistics(int BranchId, int? yearid);
        Task<StatisticsVM> GetAllUserStatistics(int UserId, int BranchId, string Lang);
        Task<StatisticsVM> GetUserStatisticsPercentData(int UserId, int BranchId, string Lang);

        Task<StatisticsVM> GetAllUserStatisticsFullReport(int UserId, int BranchId, string Lang);

        LayoutVM GetLayoutVm(int UserId, int BranchId, string Lang);
        Task<StatisticsVM> GetAllUserCustodiesStatistics(int UserId);
    }
}

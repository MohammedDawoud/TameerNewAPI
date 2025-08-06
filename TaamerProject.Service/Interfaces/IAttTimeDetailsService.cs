using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IAttTimeDetailsService
    {
        Task<IEnumerable<AttTimeDetailsVM>> GetAllAttTimeDetails(string SearchText, int AttTimeId);
        GeneralMessage SaveAttTimeDetails(AttTimeDetails attTimeDetailsint, int UserId, int BranchId, string Lang);
        GeneralMessage DeleteAttTimeDetails(int TimeDetailsId, int UserId, int BranchId);
        Task<IEnumerable<AttTimeDetailsVM>> GetAllAttTimeDetails();
        Task<IEnumerable<AttTimeDetailsVM>> GetAllAttTimeDetailsByid(int AttTimeId);
        // IEnumerable<AttTimeDetailsVM> GetAllAttTimeDetails2(int AttTimeId);
        Task<IEnumerable<AttTimeDetailsVM>> GetAllAttTimeDetails2(int AttTimeId, int branchid);
        Task<bool> CheckUserPerDawamUserExist(int UserId, string TimeFrom, string TimeTo, int DayNo);
        decimal CalculateTaskHoursForEmployee(int userId, DateTime taskStart, DateTime taskEnd);
    }
}

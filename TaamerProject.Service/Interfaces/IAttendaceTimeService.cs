using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IAttendaceTimeService
    {
        Task<IEnumerable<AttendaceTimeVM>> GetAllAttendaceTime();
        GeneralMessage SaveAttendaceTime(AttendaceTime attendaceTime, int UserId, int BranchId, string Lang);
        GeneralMessage DeleteAttendaceTime(int TimeId, int UserId, int BranchId);
        Task<int> GenerateNextAttendaceTimeNumber(int BranchId);
    }
}

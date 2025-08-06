using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ITimeOutRequestsService  
    {
        Task<IEnumerable<TimeOutRequestsVM>> GetTimeOutRequests(int BranchId);
        GeneralMessage SaveTimeOutRequests(TimeOutRequests TimeOutRequests, int UserId, int BranchId);
        GeneralMessage DeleteTimeOutRequests(int RequestId, int UserId, int BranchId);
        GeneralMessage ApproveRequest(int RequestId, int UserId, int BranchId, string Comment);
        GeneralMessage RejectRequest(int RequestId, int UserId, int BranchId, string Comment);
    }
}

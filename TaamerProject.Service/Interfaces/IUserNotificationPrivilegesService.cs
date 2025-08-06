using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IUserNotificationPrivilegesService  
    {
        Task<IEnumerable<UserNotificationPrivilegesVM>> GetUsersByPrivilegesIds(int? Priv);
        GeneralMessage SaveUserPrivilegesUsers(int AssignedUserId, List<int> Privs, int UserId, int BranchId, string Con);
        GeneralMessage SaveGroupPrivilegesUsers(int GroupId, List<int> Privs, int UserId, int BranchId,string Con);
        Task<List<int>> GetPrivilegesIdsByUserId(int UserId);
        bool SendSMS(string ReceiveNumber, string Message, int UserId, int BranchId);
    }
}

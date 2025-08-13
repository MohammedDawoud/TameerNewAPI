using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface ISys_UserLoginService
    {
        Task<IEnumerable<Sys_UserLoginVM>> GetAllUserLogin(int Type);
        Task<UsersLoginVM> GetUserLogin(string Username,string Password,int Type);

        GeneralMessage SaveUserLogin(Sys_UserLogin UserLogin, int UserId, int BranchId);
        GeneralMessage DeleteUserLogin(int UserLoginId, int UserId, int BranchId);
        GeneralMessage ConfirmUserLogin(List<int> UserLoginId,Int16 Status, int UserId, int BranchId);

    }
}

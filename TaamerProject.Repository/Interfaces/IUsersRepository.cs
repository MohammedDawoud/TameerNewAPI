using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common.FIlterModels;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Interfaces
{
    public interface IUsersRepository : IRepository<Users>
    {
        Task<IEnumerable<UsersVM>> GetAllUsersOnline2();

        Task<UsersVM> CheckISOnline(int userid);
        Task<IEnumerable<UsersVM>> GetAllUsers();
        Task<IEnumerable<UsersVM>> GetAllUsersNotOpenUser(int UserId);
        //
        Task<IEnumerable<UsersVM>> GetUserAndBranchByNameSearch(Users users);
        Task<IEnumerable<UsersVM>> GetAllOnlineUsers(int UserId);
        Task<int> GetOnlineUsers();
        Task<UsersVM> GetUser(string UserName);
        Task<UsersLoginVM> GetUserLogin(string username, string password);

        Task<UsersVM >GetUser_tadmin(string UserName);

        Task<int> GetAllUsersCount();
        Task<UsersVM> GetUserById(int UserId, string Lang);
        Task<IEnumerable<UsersVM>> GetUserByBranchId(int BranchId);
        Task<UsersVM> GetUserByEmailId(string EmailId);
        Task<UsersVM> GetUserByVeificationLinkId(string LinkId);
        Task<IEnumerable<UsersVM>> GetAllOtherUsers(int UserId);
        Task<IEnumerable<UsersVM>> GetSome(EmployeesVM Employees);
        Task<int> SearchUsersOfUserName(string UserSearchUserName);
        Task<int> SearchUsersOfEmail(string SearchUsersOfEmail);

        Task<UsersVM> GetUserByRecoverLink(String Link);
        Task<IEnumerable<UsersVM>> GetFullReport(ProjectPhasesTasksVM Search, string Lang,string Today, int BranchId);
        Task<List<RptAllEmpPerformance>> getempdataNew_Proc(PerformanceReportVM Search, string Lang, string Con, int BranchId);

        Task<PagedLists<UsersVM>> GetAllAsync(RequestParam<UsersFilterDTO> Param);
        Users GetMatchingUserWithPassword(string UserName, string activationCode = null);



    }
}

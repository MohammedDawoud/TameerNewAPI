using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IUsersService
    {
        Task<IEnumerable<UsersVM>> GetAllUsers();
        IEnumerable<Users> GetAllUsersCount();

        Task<IEnumerable<UsersVM>> GetAllUsersNotOpenUser(int UserId);
        Task<IEnumerable<UsersVM>> GetAllUsersOnline2();


        Task<UsersVM> CheckISOnline(int userid);
        //
        Task<IEnumerable<UsersVM>> GetUserAndBranchByNameSearch(Users users);
        Task<IEnumerable<UsersVM>> GetAllOnlineUsers(int UserId);
        int CheckEmail(int UserId, string email);
        int GetOnlineUsers();
        ////111
        IEnumerable<UsersVM> GetSomeusers();
        ////111
        //GeneralMessage SaveUsers(Users users, int UserId);
        GeneralMessage SaveUsers(Users users, int UserId, string link, string logo, string url, string resetCode, string Con, int BranchId);
        GeneralMessage SaveUsersProfile(Users users, int UserId, string link, string logo, string url, string resetCode, string Con, int BranchId);

        GeneralMessage ChangePassword(Users users, int UserId, int BranchId);
        GeneralMessage DeleteUsers(int UserId, int BranchId);
        GeneralMessage DeleteUsers2(int Users, int UserId, int BranchId);
        IEnumerable<object> FillUserSelect(int ExcludedUserId);
        IEnumerable<object> FillAllUserSelect(int UserId);
        IEnumerable<object> FillAllUsersSelectAll(int UserId);
        IEnumerable<object> FillAllUsersSelectAllGrantt(int UserId,string param);

        IEnumerable<object> FillAllUsersSelectAll(int UserId, int BranchId);
        GeneralMessage ChangeUserImage(Users users, int UserId, int BranchId);
        GeneralMessage ChangeUserStamp(Users users, int UserId, int BranchId);
        bool ValidateUserCofidential(string UserName, string Password, string activationCode);
        Task<UsersVM> GetUser(string username);
        Task<UsersVM> GetUser_tadmin(string username);

        GeneralMessage SaveUserPrivilegesUsers(int AssignedUserId, List<int> Privs, int UserId, int BranchId, string Con);
        GeneralMessage SaveGroupPrivilegesUsers(int GroupId, List<int> Privs, int UserId, int BranchId, string Con);

        List<int> GetPrivilegesIdsByUserId(int UserId);

        GeneralMessage CloseUser(Users users, int userid);
        List<int> GetBranchesByUserId(int UserId);
        Task<IEnumerable<BranchesVM>> GetAllBranchesByUserName(string Lang, string UseName);
        Task<UsersVM> GetUserById(int UserId, string Lang);
        Task<UsersVM> GetUserByEmailId(string EmailId);
        Task<UsersVM> GetUserByVeificationLinkId(string link);
        int UpdateOnlineStatus(bool IsOnline, int User, int UserId, int BranchId);
        int UpdateOnlineStatus2(bool IsOnlineNew, int User, int UserId, int BranchId);
        int LogoutUser(bool IsOnlineNew, int User, int UserId, int BranchId);
        int UpdateLastLoginDate(int UserId);
        int ClearExpireDate(int UserId);
        int UpdateActiveTime(int UserId, int BranchId);
        string DecryptValue1(string value);
        int UpdateLastLinkvalidDate(int UserId, string link);
        bool ProcessActivationCode(string UserName, string Password, int BranchId);
        bool ForgetPassword(string Email, string link, string emailFor, string Emailbody);
        string ForgetPasswordError(string Email, string link, string emailFor, string Emailbody);

        bool ChangePasswordLink(string link, string password, int UserId, int BranchId);
        bool RetrievePassword(string UserName, string Email, string emailFor, string Emailbody);
        Task<IEnumerable<UsersVM>> GetAllOtherUsers(int UserId);
        Task<UsersVM> GetUserById2(int UserId, string Lang);

        Task<int> GetMaxOrderNumber();
        Task<IEnumerable<UsersVM>> GetFullReport(ProjectPhasesTasksVM Search, string Lang, string Today, int BranchId);
        GeneralMessage UpdateAppActiveStatus(bool IsActivated, int User, int UserId, int BranchId);

        GeneralMessage Disappearewelcomeuser(int UserId, int BranchId);

        int UpdateQrCodeUser(int UserId, string Qrcodeurl);
        GeneralMessage DeleteDeviceId(int user, int UserId, int BranchId);
    }
}

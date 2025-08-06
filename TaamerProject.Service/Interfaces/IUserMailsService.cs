using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IUserMailsService   
    {
        Task<IEnumerable<UserMailsVM>> GetAllUserMails(int UserId, int BranchId);
        Task<IEnumerable<UserMailsVM>> GetAllUserMailsTrash(int UserId, int BranchId);
        Task<IEnumerable<UserMailsVM>> GetAllUserMailsSent(int UserId, int BranchId);
        Task<IEnumerable<UserMailsVM>> GetAllUnReadUserMails(int UserId, int BranchId);
        GeneralMessage SaveUserMails(UserMails userMails, int UserId, int BranchId);
        GeneralMessage DeleteUserMails(int MailId, int UserId, int BranchId);
        Task<int> GetAllUserMailsSentCount(int UserId, int BranchId);
        Task<int> GetAllUserMailsCount(int UserId, int BranchId);
        Task<int> GetAllUserMailsTrashCount(int UserId, int BranchId);
        bool ReadUserMails(int UserId, int BranchId);
    }
}

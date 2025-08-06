using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IUserMailsRepository
    {
        Task<IEnumerable<UserMailsVM>> GetAllUserMails(int UserId, int BranchId);
        Task<IEnumerable<UserMailsVM>> GetAllUserMailsTrash(int UserId, int BranchId);
        Task<IEnumerable<UserMailsVM>> GetAllUserMailsSent(int UserId, int BranchId);
        Task<IEnumerable<UserMailsVM>> GetAllUnReadUserMails(int UserId, int BranchId);
        Task<int> GetAllUserMailsSentCount(int UserId, int BranchId);
        Task<int> GetAllUserMailsTrashCount(int UserId, int BranchId);
        Task<int> GetAllUserMailsCount(int UserId, int BranchId);

    }
}

using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface ISupportResquestsRepository
    {
        Task<IEnumerable<SupportRequestVM>> GetAllSupportResquests(string lang, int BranchId,int UserId);
        Task<IEnumerable<SupportRequestVM>> GetAllOpenSupportResquests(string lang, int BranchId, int UserId);
        Task<IEnumerable<SupportRequestVM>> GetAllOpenSupportResquestsWithReplay(int UserId);
    }
}

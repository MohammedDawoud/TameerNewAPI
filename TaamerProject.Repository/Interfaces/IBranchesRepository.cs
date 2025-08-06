using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IBranchesRepository
    {
        Task<IEnumerable<BranchesVM>> GetAllBranches(string lang);
        Task<IEnumerable<BranchesVM>> FillBranchSelectNew(string lang);

        Task<IEnumerable<BranchesVM>> GetBranchByBranchId(string lang, int BranchId);
        Task<BranchesVM> GetBranchByBranchIdCheck(string lang, int BranchId);

        Task<IEnumerable<BranchesVM>> GetActiveBranch();
        Task<int> GenerateNextBranchNumber();
        Task<int> GetOrganizationId(int BranchId);
        Task<BranchesVM> GetCashBoxViaBranchID(int BranchId);
        Task<IEnumerable<BranchesVM>> GetBranchById(int branchId, string Lang);
        Branch GetById(int BranchId);

    }
}

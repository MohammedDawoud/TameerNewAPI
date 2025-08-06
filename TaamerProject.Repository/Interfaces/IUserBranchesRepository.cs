using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IUserBranchesRepository
    {
        
        Task<IEnumerable<BranchesVM>> GetAllBranchesByUserId(string Lang, int UserId);
        Task<IEnumerable<BranchesVM>> GetAllBranchesAndMainByUserId(string Lang, int UserId);

        Task<IEnumerable<BranchesVM>> GetBranchByBranchId(string Lang, int branchId);
    }
}

using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IChecksRepository
    {
        Task<IEnumerable<ChecksVM>> GetAllChecks(int? Type,int BranchId);
        Task<int> GetUnderCollectionCount(int BranchId);
        Task<int> GetExportsCount(int BranchId);
        Task<IEnumerable<ChecksVM>> GetAllCheckSearch(ChecksVM CheckSearch, int BranchId);
    }
}

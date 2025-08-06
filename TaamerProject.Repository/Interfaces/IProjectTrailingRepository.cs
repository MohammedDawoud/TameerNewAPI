using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IProjectTrailingRepository
    {
        Task<IEnumerable<ProjectTrailingVM>> GetProjectTrailingInOfficeArea(int BranchId);
        Task<IEnumerable<ProjectTrailingVM>> GetProjectTrailingInExternalSide(int BranchId);
        Task<int> GetMaxId();
    }
}

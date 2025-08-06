using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IProjectSubTypeRepository
    {
        Task<IEnumerable<ProjectSubTypeVM>> GetAllProjectSubType(int BranchId);
        Task<IEnumerable<ProjectSubTypeVM>> GetAllProjectSubsByProjectTypeId(int ProjectTypeId, string SearchText, int BranchId);
        Task<IEnumerable<ProjectSubTypeVM>> GetTimePeriordBySubTypeId(int SubTypeId);

    }
}

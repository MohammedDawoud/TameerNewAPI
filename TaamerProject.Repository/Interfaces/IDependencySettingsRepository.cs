using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IDependencySettingsRepository 
    {
        Task<IEnumerable<DependencySettingsVM>> GetAllDependencySettings(int? SuccessorId, int BranchId);
        Task<IEnumerable<DependencySettingsVM>> GetAllDependencyByProjSubTypeId(int? ProjSubTypeId, int BranchId);
        Task<IEnumerable<DependencySettingsNewVM>> GetAllDependencyByProjSubTypeIdNew(int? ProjSubTypeId, int BranchId);

    }
}

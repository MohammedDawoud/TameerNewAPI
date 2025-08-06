using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface ITasksDependencyRepository :IRepository<TasksDependency>
    {
        Task<IEnumerable<TasksDependencyVM>> GetAllTasksDependencies(int BranchId);
        Task<IEnumerable<TasksDependencyVM>> GetAllDependencyByProjectId(int ProjectId);
        Task<IEnumerable<TasksDependencyVM>> GetAllDependencyByProjectIdNew(int ProjectId);

        Task<TasksDependencyVM> GetDependencyByProjSubTypeId(int ProjectId);
    }
}

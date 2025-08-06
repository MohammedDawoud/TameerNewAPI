using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IProjectWorkersRepository
    {
        Task<IEnumerable<ProjectWorkersVM>> GetAllProjectWorkers(int? ProjectId, string SearchText);
        Task<int> GetUserProjectWorkerCount(int? UserId, int BranchId);
        Task<IEnumerable<ProjectWorkersVM>> GetUserProjectRpt(int? UserId);
    }
}

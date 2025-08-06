using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IProjectWorkersService  
    {
        Task<IEnumerable<ProjectWorkersVM>> GetAllProjectWorkers(int? ProjectId, string SearchText);
        Task<IEnumerable<ProjectWorkersVM>> GetUserProjectRpt(int? UserId);
        GeneralMessage SaveProjectWorker(ProjectWorkers ProjectWorkers, int UserId, int BranchId);
        GeneralMessage DeleteProjectWorker(int WorkerId, int UserId, int BranchId);
        Task<int> GetUserProjectWorkerCount(int? UserId, int BranchId);
    }
}

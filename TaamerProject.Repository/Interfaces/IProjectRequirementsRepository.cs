using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IProjectRequirementsRepository
    {
        Task<IEnumerable<ProjectRequirementsVM>> GetAllProjectRequirement(int BranchId);
        Task<IEnumerable<ProjectRequirementsVM>> GetAllProjectRequirementByTaskId(int BranchId, int PhasesTaskID);
        Task<IEnumerable<ProjectRequirementsVM>> GetAllProjectRequirementById(int BranchId, int RequirementId);

        Task<IEnumerable<ProjectRequirementsVM>> GetProjectRequirementByProjectSubTypeId(int ProjectSubTypeId,string SearchText, int BranchId);
        Task<IEnumerable<ProjectRequirementsVM>> GetProjectRequirementByTaskId(int TaskId, int BranchId);
        Task<IEnumerable<ProjectRequirementsVM>> GetProjectRequirementByOrderId(int orderid, int BranchId);
        Task<IEnumerable<ProjectRequirementsVM>> GetAllProjectRequirementOrderId(int BranchId, int Orderid);
    }
}

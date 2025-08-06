using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IProjectRequirementsService  
    {
        Task<IEnumerable<ProjectRequirementsVM>> GetAllProjectRequirement( int BranchId );
        Task<IEnumerable<ProjectRequirementsVM>> GetAllProjectRequirementByTaskId(int BranchId, int PhasesTaskID);
        Task<IEnumerable<ProjectRequirementsVM>> GetAllProjectRequirementById(int BranchId, int RequirementId);

        Task<IEnumerable<ProjectRequirementsVM>> GetProjectRequirementByProjectSubTypeId(int ProjectSubTypeId,string SearchText, int BranchId);
        GeneralMessage SaveProjectRequirement(ProjectRequirements projectRequirements, int UserId, int BranchId);
        GeneralMessage DeleteProjectRequirements(int RequirementId, int UserId, int BranchId);
        Task<IEnumerable<ProjectRequirementsVM>> GetProjectRequirementByTaskId(int TaskId, int BranchId);
        GeneralMessage SaveProjectRequirement2(List<ProjectRequirements> projectRequirements, int UserId, int BranchId);
        Task<IEnumerable<ProjectRequirementsVM>> GetProjectRequirementOrderId(int Orderid, int BranchId);
        Task<IEnumerable<ProjectRequirementsVM>> GetAllProjectRequirementByOrder(int BranchId, int Orderid);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IRequirementsService 
    {
        Task<IEnumerable<RequirementsVM>> GetAllRequirements(int BranchId);
        Task<IEnumerable<RequirementsVM>> GetAllRequirementsByProjectId(int ProjectId,int BranchId);
        GeneralMessage ConfirmRequirementStatus(int RequirementId, bool Status, int UserId, int BranchId);


        GeneralMessage SaveRequirements(Requirements requirements , int UserId, int BranchId);
        GeneralMessage SaveRequirementsbyProjectId(int ProjectId, int UserId, int BranchId);

        GeneralMessage DeleteRequirement(int RequirementId, int UserId, int BranchId);
        Task<IEnumerable<RequirementsVM>> FillRequirementsSelect(int BranchId);
    }
}

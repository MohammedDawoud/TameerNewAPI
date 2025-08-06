using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IModelRequirementsService
    {
       Task<IEnumerable<ModelRequirementsVM>> GetAllModelRequirements(int BranchId);
        GeneralMessage SaveModelRequirements(ModelRequirements modelRequirements, int UserId, int BranchId);
        GeneralMessage DeleteModelRequirement(int ModelReqId, int UserId, int BranchId);
        Task<IEnumerable<ModelRequirementsVM>> GetAllModelRequirementsByModelId(int ModelId);
    }
}

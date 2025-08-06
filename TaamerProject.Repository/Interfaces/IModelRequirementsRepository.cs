using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IModelRequirementsRepository
    {
        Task<IEnumerable<ModelRequirementsVM>> GetAllModelRequirements(int BranchId);
        Task<IEnumerable<ModelRequirementsVM>> GetAllModelRequirementsByModelId(int ModelId);
    }
}

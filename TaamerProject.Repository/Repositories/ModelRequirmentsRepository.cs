using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class ModelRequirementsRepository : IModelRequirementsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ModelRequirementsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        public async Task<IEnumerable<ModelRequirementsVM>> GetAllModelRequirements(int BranchId)
        {
            var modelRequirements = _TaamerProContext.ModelRequirements.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new ModelRequirementsVM
            {
                ModelReqId = x.ModelReqId,
                RequirementId = x.RequirementId,
                ModelId = x.ModelId,
                BranchId = x.BranchId
            }).ToList();
            return modelRequirements;
        }
        public async Task<IEnumerable<ModelRequirementsVM>> GetAllModelRequirementsByModelId(int ModelId)
        {
            var modelRequirements = _TaamerProContext.ModelRequirements.Where(s => s.IsDeleted == false && s.ModelId == ModelId).Select(x => new ModelRequirementsVM
            {
                RequirementName = x.Requirements.NameAr,
            }).ToList();
            return modelRequirements;
        }
    }
}

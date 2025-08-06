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
    public class ModelTypeRepository : IModelTypeRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ModelTypeRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        public async Task<IEnumerable<ModelTypeVM>> GetAllModelTypes(int BranchId)
        {
            var ModelTypes = _TaamerProContext.ModelType.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new ModelTypeVM
            {
                ModelTypeId = x.ModelTypeId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
            }).ToList();
                return ModelTypes;
        }
    }
}



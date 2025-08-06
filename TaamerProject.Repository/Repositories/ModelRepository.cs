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
    public class ModelRepository : IModelRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ModelRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        public async Task<IEnumerable<ModelVM>> GetAllModels(int BranchId)
        {
            var Models = _TaamerProContext.Model.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new ModelVM
            {
                ModelId = x.ModelId,
                ModelName = x.ModelName,
                Date = x.Date,
                HijriDate = x.HijriDate,
                Notes = x.Notes,
                UserId = x.UserId,
                FileUrl = x.FileUrl,
                TypeId = x.TypeId,
                Extension = x.Extension,
                UserName = x.Users.FullName,
                FileTypeName = x.FileType.NameAr,
            }).ToList();
                return Models;
        }
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class DependencySettingsRepository :  IDependencySettingsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public DependencySettingsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<DependencySettingsVM>> GetAllDependencySettings(int? SuccessorId, int BranchId)
        {
            var Dependencies = _TaamerProContext.DependencySettings.Where(s => s.IsDeleted == false && s.SuccessorId == SuccessorId && s.BranchId == BranchId).Select(x => new DependencySettingsVM
            {
                DependencyId = x.DependencyId,
                PredecessorId = x.PredecessorId,
                SuccessorId = x.SuccessorId,
                Type = x.Type,
                ProjSubTypeId = x.ProjSubTypeId,
                BranchId = x.BranchId
            }).ToList();
            return Dependencies;
        }

        public async Task<IEnumerable<DependencySettingsVM>> GetAllDependencyByProjSubTypeId(int? ProjSubTypeId, int BranchId)
        {
            var Dependencies = _TaamerProContext.DependencySettings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeId).Select(x => new DependencySettingsVM
            {
                DependencyId = x.DependencyId,
                PredecessorId = x.PredecessorId,
                SuccessorId = x.SuccessorId,
                Type = x.Type,
            }).ToList();
            return Dependencies;
        }
        public async Task<IEnumerable<DependencySettingsNewVM>> GetAllDependencyByProjSubTypeIdNew(int? ProjSubTypeId, int BranchId)
        {
            var Dependencies = _TaamerProContext.DependencySettingsNew.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeId).Select(x => new DependencySettingsNewVM
            {
                DependencyId = x.DependencyId,
                PredecessorId = x.PredecessorId,
                SuccessorId = x.SuccessorId,
                Type = x.Type,
                PredecessorIdindex=x.SettingsPredecessor!=null? x.SettingsPredecessor.taskindex:0,
                SuccessorIdindex = x.SettingsSuccessor != null ? x.SettingsSuccessor.taskindex : 0,

            }).ToList();
            return Dependencies;
        }

    }
}

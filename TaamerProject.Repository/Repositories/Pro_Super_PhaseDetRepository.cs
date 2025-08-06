using TaamerProject.Models;
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
    public class Pro_Super_PhaseDetRepository : IPro_Super_PhaseDetRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Pro_Super_PhaseDetRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task< IEnumerable<Pro_Super_PhaseDetVM>> GetAllSuper_PhaseDet(int? PhaseId)
        {
            var Super_PhaseDet = _TaamerProContext.Pro_Super_PhaseDet.Where(s => s.IsDeleted == false && s.PhaseId== PhaseId).Select(x => new Pro_Super_PhaseDetVM
            {
                PhaseDetailesId = x.PhaseDetailesId,
                PhaseId = x.PhaseId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Note = x.Note,
                IsRead = x.IsRead??false,
                BranchId = x.BranchId,
            }).ToList();
            return Super_PhaseDet;
        }
    }
}

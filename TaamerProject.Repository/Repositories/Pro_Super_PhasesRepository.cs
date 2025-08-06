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
    public class Pro_Super_PhasesRepository : IPro_Super_PhasesRepository
    {
    private readonly TaamerProjectContext _TaamerProContext;

    public Pro_Super_PhasesRepository(TaamerProjectContext dataContext)
    {
        _TaamerProContext = dataContext;

    }

    public async Task<IEnumerable<Pro_Super_PhasesVM>> GetAllSuperPhases(string SearchText)
        {
            if (SearchText == "")
            {
                var SuperPhases = _TaamerProContext.Pro_Super_Phases.Where(s => s.IsDeleted == false).Select(x => new Pro_Super_PhasesVM
                {
                    PhaseId = x.PhaseId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    IsRead = x.IsRead??false,
                    UserId = x.UserId,
                    Note = x.Note,
                    BranchId = x.BranchId,
                    SuperCode=x.SuperCode??"",
                }).ToList();
                return SuperPhases;
            }
            else

            {
                var SuperPhases = _TaamerProContext.Pro_Super_Phases.Where(s => s.IsDeleted == false && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText))).Select(x => new Pro_Super_PhasesVM
                {
                    PhaseId = x.PhaseId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    IsRead = x.IsRead ?? false,
                    UserId = x.UserId,
                    Note = x.Note,
                    BranchId = x.BranchId,
                    SuperCode = x.SuperCode ?? "",
                }).ToList();
                return SuperPhases;
            }
        }
        public async Task<Pro_Super_PhasesVM> GetSuper_PhasesById(int SuperId)
        {
            var SuperPhases = _TaamerProContext.Pro_Super_Phases.Where(s => s.IsDeleted == false && s.PhaseId== SuperId).Select(x => new Pro_Super_PhasesVM
            {
                PhaseId = x.PhaseId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                IsRead = x.IsRead ?? false,
                UserId = x.UserId,
                Note = x.Note,
                BranchId = x.BranchId,
                SuperCode = x.SuperCode ?? "",
            }).FirstOrDefault();
            return SuperPhases;
        }

    }
}

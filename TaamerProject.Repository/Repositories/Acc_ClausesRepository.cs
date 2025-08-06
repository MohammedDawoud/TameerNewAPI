using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class Acc_ClausesRepository : IAcc_ClausesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Acc_ClausesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<Acc_ClausesVM>> GetAllClauses(string SearchText)
        {
            if (SearchText == "")
            {
                var Clauses = _TaamerProContext.Acc_Clauses.Where(s => s.IsDeleted == false).Select(x => new Acc_ClausesVM
                {
                    ClauseId = x.ClauseId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                }).ToList();
                return Clauses;
            }
            else

            {
                var Clauses = _TaamerProContext.Acc_Clauses.Where(s => s.IsDeleted == false && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText))).Select(x => new Acc_ClausesVM
                {
                    ClauseId = x.ClauseId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                }).ToList();
                return Clauses;
            }
        }

    }
}

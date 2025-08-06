using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;

namespace TaamerProject.Repository.Repositories
{
    public class Pro_projectsReasonsRepository : IPro_projectsReasonsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;
        public Pro_projectsReasonsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<Pro_projectsReasonsVM>> GetAllprojectsReasons()
        {
            var Reasons = _TaamerProContext.Pro_projectsReasons.Where(s => s.IsDeleted == false).Select(x => new Pro_projectsReasonsVM
            {
                ReasonsId = x.ReasonsId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
            }).ToList();
            return Reasons;
        }
    }
}

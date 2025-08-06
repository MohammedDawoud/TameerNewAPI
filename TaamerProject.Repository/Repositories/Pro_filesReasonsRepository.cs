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
    public class Pro_filesReasonsRepository : IPro_filesReasonsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;
        public Pro_filesReasonsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<Pro_filesReasonsVM>> GetAllfilesReasons()
        {
            var Reasons = _TaamerProContext.Pro_filesReasons.Where(s => s.IsDeleted == false).Select(x => new Pro_filesReasonsVM
            {
                ReasonsId = x.ReasonsId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
            }).ToList();
            return Reasons;
        }
    }
}

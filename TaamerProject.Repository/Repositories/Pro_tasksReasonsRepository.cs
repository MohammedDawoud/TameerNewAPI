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
    public class Pro_tasksReasonsRepository : IPro_tasksReasonsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;
        public Pro_tasksReasonsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<Pro_tasksReasonsVM>> GetAlltasksReasons()
        {
            var Reasons = _TaamerProContext.Pro_tasksReasons.Where(s => s.IsDeleted == false).Select(x => new Pro_tasksReasonsVM
            {
                ReasonsId = x.ReasonsId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
            }).ToList();
            return Reasons;
        }
    }
}

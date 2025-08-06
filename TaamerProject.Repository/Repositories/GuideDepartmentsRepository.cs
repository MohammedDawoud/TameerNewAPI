
using TaamerProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using Bayanatech.TameerPro.Repository;

namespace TaamerProject.Repository.Repositories
{
    public class GuideDepartmentsRepository :  IGuideDepartmentsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public GuideDepartmentsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<GuideDepartmentsVM>> GetAllDeps(string lang, int? DepId = null)
        {
            var Deps = _TaamerProContext.GuideDepartments.Where(s => s.IsDeleted == false && (DepId.HasValue ? s.DepId == DepId.Value : true)).Select(x => new GuideDepartmentsVM
            {
                DepId = x.DepId,
                NameAr = x.DepNameAr,
                NameEn = x.DepNameEn

            }).ToList();

            return Deps;
        }
    }
}

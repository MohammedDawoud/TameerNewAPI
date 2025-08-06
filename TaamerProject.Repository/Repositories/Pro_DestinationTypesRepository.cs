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
    public class Pro_DestinationTypesRepository : IPro_DestinationTypesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;
        public Pro_DestinationTypesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<Pro_DestinationTypesVM>> GetAllDestinationTypes()
        {
            var Reasons = _TaamerProContext.Pro_DestinationTypes.Where(s => s.IsDeleted == false).Select(x => new Pro_DestinationTypesVM
            {
                DestinationTypeId = x.DestinationTypeId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                DepartmentName = x.DepartmentName,
            }).ToList();
            return Reasons;
        }
    }
}

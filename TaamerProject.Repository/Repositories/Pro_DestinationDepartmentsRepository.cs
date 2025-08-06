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
    public class Pro_DestinationDepartmentsRepository : IPro_DestinationDepartmentsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;
        public Pro_DestinationDepartmentsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<Pro_DestinationDepartmentsVM>> GetAllDestinationDepartments()
        {
            var Reasons = _TaamerProContext.Pro_DestinationDepartments.Where(s => s.IsDeleted == false).Select(x => new Pro_DestinationDepartmentsVM
            {
                DepartmentId = x.DepartmentId,
                DestinationTypeId = x.DestinationTypeId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
            }).ToList();
            return Reasons;
        }
        public async Task<IEnumerable<Pro_DestinationDepartmentsVM>> GetAllDestinationDepartmentsByTypeId(int TypeId)
        {
            var Reasons = _TaamerProContext.Pro_DestinationDepartments.Where(s => s.IsDeleted == false && s.DestinationTypeId== TypeId).Select(x => new Pro_DestinationDepartmentsVM
            {
                DepartmentId = x.DepartmentId,
                DestinationTypeId = x.DestinationTypeId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
            }).ToList();
            return Reasons;
        }

    }
}

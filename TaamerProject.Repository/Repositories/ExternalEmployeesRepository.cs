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
    public class ExternalEmployeesRepository : IExternalEmployeesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ExternalEmployeesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        public async Task<IEnumerable<ExternalEmployeesVM>> GetAllExternalEmployees(int? DepartmentId, string SearchText, int BranchId)
        {
            var externalEmployees = _TaamerProContext.ExternalEmployees.Where(s => s.IsDeleted == false && s.DepartmentId == DepartmentId && s.BranchId == BranchId).Select(x => new ExternalEmployeesVM
            {
                EmpId = x.EmpId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                DepartmentId = x.DepartmentId,
                Description = x.Description,
                DepartmentName = x.Department.DepartmentNameAr,
            }).ToList();
            if (SearchText != "")
            {
                externalEmployees = externalEmployees.Where(s => s.NameAr.Contains(SearchText.Trim()) || s.NameEn.Contains(SearchText.Trim())).ToList();
            }
            return externalEmployees;
        }

    }
}

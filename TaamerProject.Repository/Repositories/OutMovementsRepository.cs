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
    public class OutMovementsRepository : IOutMovementsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public OutMovementsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<OutMovementsVM>> GetAllOutMovements(int? TrailingId)
        {
            var outMovements = _TaamerProContext.OutMovements.Where(s => s.IsDeleted == false && s.TrailingId == TrailingId).Select(x => new OutMovementsVM
            {
                MoveId = x.MoveId,
                ConstraintNo = x.ConstraintNo,
                EmpId = x.EmpId,
                OrderNo = x.OrderNo,
                RequiredWork = x.RequiredWork,
                FinishedWork = x.FinishedWork,
                Date = x.Date,
                HijriDate = x.HijriDate,
                ExpeditorId = x.ExpeditorId,
                TrailingId = x.TrailingId,
                BranchId = x.BranchId,
                SideName = x.ProjectTrailing.Department.DepartmentNameAr,
                EmployeeName = x.Employees.EmployeeNameAr,
                ExternalEmployeeName = x.ExternalEmployees.NameAr,
            }).ToList();
            return outMovements;
        }
    }
}

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
    public class ExpensesGovernmentRepository : IExpensesGovernmentRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ExpensesGovernmentRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<ExpensesGovernmentVM>> GetAllExpensesGovernment(int? EmpId, string SearchText)
        {
            var ExpensesGovernment = _TaamerProContext.ExpensesGovernment.Where(s => s.IsDeleted == false && s.EmployeeId == EmpId).Select(x => new ExpensesGovernmentVM
            {
                ExpensesId = x.ExpensesId,
                EmployeeId = x.EmployeeId,
                TypeId = x.TypeId,
                StartDate = x.StartDate,
                StartHijriDate = x.StartHijriDate,
                EndDate = x.EndDate,
                EndHijriDate = x.EndHijriDate,
                Notes = x.Notes,
                Amount = x.Amount,
                UserId = x.UserId,
                ExpGovTypeName = x.ExpGovType.NameAr,
                Year = x.Year,
                HijriYear = x.HijriYear
            }).ToList();
            if (SearchText != "")
            {
                ExpensesGovernment = ExpensesGovernment.Where(s => s.ExpGovTypeName.Contains(SearchText.Trim()) || s.Amount.ToString().Contains(SearchText.Trim())).ToList();
            }
            return ExpensesGovernment;
        }
    }
}



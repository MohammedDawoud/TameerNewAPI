using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IPayrollMarchesRepository
    {
        Task<IEnumerable<PayrollMarchesVM>> GetPayrollMarches(int MonthId, int BranchSearch, string SearchText);
        Task<IEnumerable<PayrollMarchesVM>> GetPayrollMarches(int MonthId, int BranchSearch, string SearchText, int YearId);
        Task<PayrollMarches> GetPayrollMarches(int EmpId, int MonthId, int YearId);
        Task<PayrollMarches> GetPayrollMarches(int EmpId, int MonthId);
    }
}

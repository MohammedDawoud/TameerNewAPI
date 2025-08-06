using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IAllowanceRepository
    {
        Task<IEnumerable<AllowanceVM>> GetAllAllowances(int? EmpId, string SearchText, bool? IsSalaryPart = null);
        Task<IEnumerable<AllowanceVM>> GetAllAllowancesSearch();
        Task<IEnumerable<AllowanceVM>> GetAllAllowancesBySearchObject(AllowanceVM AllowanceSearch);
        Task<decimal> GetAllownacesSumForPayroll(int EmpId, DateTime StartDate, DateTime EndDate);
        Task<decimal> GetAllownacesSumForPayroll2(int EmpId, int MonthNo);
    }
}

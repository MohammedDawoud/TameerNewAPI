using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IAllowanceService
    {
        Task<IEnumerable<AllowanceVM>> GetAllAllowances(int? EmpId, string SearchText, bool? IsSalaryPart = null);
        Task<IEnumerable<AllowanceVM>> GetAllAllowancesSearch(AllowanceVM AllowanceSearch);
        GeneralMessage SaveAllowance(Allowance allowance, int UserId, int BranchId, string Lang);
        GeneralMessage SaveSalaryParts(EmpSalaryPartsVM salaryParts, int UserId, int BranchId, string Lang);
        GeneralMessage DeleteAllowance(int AllowanceId, int UserId, int BranchId);
        Task<decimal> GetAllownacesSumForPayroll(int EmpId, DateTime StartDate, DateTime EndDate);
    }
}

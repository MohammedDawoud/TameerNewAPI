using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IPayrollMarchesService
    {
        Task<PayrollMarches> GetPayrollMarches(int EmpId, int MonthId);
        Task<IEnumerable<PayrollMarchesVM>> GetPayrollMarches(int MonthId, int BranchSearch, string SearchText);
        Task<IEnumerable<PayrollMarchesVM>> GetPayrollMarches(int MonthId, int BranchSearch, string SearchText, int YearId);
        GeneralMessage SavePayrollMarches(PayrollMarches payroll, int UserId, int BranchId);
        GeneralMessage PostPayrollMarches(int PayrollId, int UserId, int BranchId);
        GeneralMessage PostEmpPayroll_Back(int PayrollId, int UserId, int BranchId);

        GeneralMessage PostEmpPayrollVoucher(int PayrollId, int UserId, int BranchId);
        GeneralMessage PostPayrollMarcheslist(List<Int32> payrollid, int UserId, int BranchId);
        GeneralMessage PostEmpPayrollPayVoucher(int PayrollId, int UserId, int BranchId);
        GeneralMessage PostAllEmpPayrollVoucher(List<int> PayrollId, int UserId, int BranchId);
        GeneralMessage PostALLEmpPayrollPayVoucher(List<int> PayrollId, int UserId, int BranchId);
    }
}

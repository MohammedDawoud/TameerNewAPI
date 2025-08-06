using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IExpensesGovernmentService  
    {
        Task<IEnumerable<ExpensesGovernmentVM>> GetAllExpensesGovernment(int? EmpId, string SearchText);
        GeneralMessage SaveExpensesGovernment(ExpensesGovernment expensesGovernment,int UserId, int BranchId);
        GeneralMessage DeleteExpensesGovernment(int ExpensesId,int UserId,int BranchId);
    }
}

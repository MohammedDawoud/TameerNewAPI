using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IExpensesGovernmentTypeService 
    {
        Task<IEnumerable<ExpensesGovernmentTypeVM>> GetAllExpensesGovernmentTypes(string SearchText, int BranchId);
        GeneralMessage SaveExpensesGovernmentType(ExpensesGovernmentType expensesGovernmentType ,int UserId, int BranchId);
        GeneralMessage DeleteExpensesGovernmentType(int ExpensesGovernmentTypeId,int UseId,int BranchId);
        IEnumerable<object> FillExpensesGovernmentTypeSelect(int BranchId);
    }
}

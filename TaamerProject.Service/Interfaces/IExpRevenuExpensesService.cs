using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IExpRevenuExpensesService  
    {
        Task<IEnumerable<ExpRevenuExpensesVM>> GetAllExpRevenuExpenses( int BranchId);
        Task<IEnumerable<ExpRevenuExpensesVM>> GetAllExpBysearchObject(ExpRevenuExpensesVM expsearch, int BranchId);
        GeneralMessage SaveExpRevenuExpenses(ExpRevenuExpenses revenuExpenses ,int UserId, int BranchId);
        GeneralMessage DeleteRevenuExpenses(int ExpectedId, int UserId, int BranchId);
        GeneralMessage FinishRestoreRevenuExpenses(int ExpectedId, int UserId, int BranchId);
        Task<IEnumerable< GetTotalExpRevByCCVM>> GetTotalExpRevByCC(string Con);
    }
}

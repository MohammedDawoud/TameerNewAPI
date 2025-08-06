using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IExpRevenuExpensesRepository
    {
        Task<IEnumerable<ExpRevenuExpensesVM>> GetAllExpRevenuExpenses(int BranchId);
        int GetExpensesCount(int BranchId);
        int GetRevenuesCount(int BranchId);
        decimal GetExpensesAmountAllBranches();
        decimal GetRevenuesAmountAllBranches();
        Task<IEnumerable<ExpRevenuExpensesVM>> GetAllExpBysearchObject(ExpRevenuExpensesVM expsearch, int BranchId);
        Task<IEnumerable<GetTotalExpRevByCCVM>> GetTotalExpRevByCC(string Con);
    }
}

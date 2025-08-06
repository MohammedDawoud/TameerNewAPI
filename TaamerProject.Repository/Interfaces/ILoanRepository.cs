using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface ILoanRepository : IRepository<Loan>
    {
        Task<IEnumerable<LoanVM>> GetAllLoans(int? EmpId, string SearchText);

        Task<IEnumerable<LoanVM>> GetAllLoansE(int? EmpId, string SearchText);

        Task<IEnumerable<LoanVM>> GetAllLoansAfterDecision(int? EmpId, string SearchText);

        Task<IEnumerable<LoanVM>> GetAllLoans2(int? UserId, string SearchText);

        Task<IEnumerable<LoanVM>> GetAllLoansW(string SearchText);

        Task<IEnumerable<LoanVM>> GetAllImprestBySearchObject(LoanVM VacationSearch, int BranchId);
        Task<IEnumerable<LoanVM>> GetAllImprestSearch(int BranchId);
        Task<IEnumerable<LoanVM>> GetAllLoansW2(string SearchText, int status);
    }
}

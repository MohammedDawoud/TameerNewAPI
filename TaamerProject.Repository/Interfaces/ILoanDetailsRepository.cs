using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface ILoanDetailsRepository : IRepository<LoanDetails>
    {
        Task<IEnumerable<LoanDetailsVM>> GetAllLoanDetails(int? loanId);
        Task<LoanDetailsVM> GetAmountPayedAndNotPayed(int? loanId);

        IEnumerable<LoanDetailsVM> GetAllLoanDetails2(int loanId);

    }
}

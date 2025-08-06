using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ILoanService
    {
        Task<IEnumerable<LoanVM>> GetAllLoans(int? EmpId, string SearchText);
        IEnumerable<Loan> GetAllLoansDataModel(int? EmpId);
        Task<IEnumerable<LoanVM>> GetAllLoansE(int? EmpId, string SearchText);

        Task<IEnumerable<LoanVM>> GetAllLoansAfterDecision(int? EmpId, string SearchText);

        Task<IEnumerable<LoanVM>> GetAllLoans2(int? UserId, string SearchText);

        Task<IEnumerable<LoanVM>> GetAllLoansW(string SearchText);

        Task<IEnumerable<LoanDetailsVM>> GetAllLoanDetails(int? LoanId);
        Task<LoanDetailsVM> GetAmountPayedAndNotPayed(int? LoanId);

        //GeneralMessage SaveLoan(Loan loan, int UserId, int BranchId, string Lang);
        GeneralMessage SaveLoan(Loan loan, int UserId, int BranchId, string Lang, string Url, string ImgUrl);
        GeneralMessage SaveLoan_Management(Loan loan, int UserId, int BranchId, string Lang, string Url, string ImgUrl);
        //GeneralMessage SaveLoan2(Loan loan, int UserId, int BranchId, string Lang);
        GeneralMessage SaveLoan2(Loan loan, int UserId, int BranchId, string Lang, string Url, string ImgUrl);

        // GeneralMessage SaveLoanWorkers(Loan loan, int UserId, int BranchId, string Lang);
        GeneralMessage SaveLoanWorkers(Loan loan, int UserId, int BranchId, string Lang, string Url, string ImgUrl);

        GeneralMessage PayLoan(int LoanDetailsId, int UserId, int BranchId);
        GeneralMessage DeleteLoan(int LoanId, int userId, int BranchId);
        Task<IEnumerable<LoanVM>> GetAllImprestSearch(LoanVM VacationSearch, int BranchId);

        //GeneralMessage UpdateStatus(int ImprestId, int UserId, int BranchId, string Lang, int Type,int? YearId);
        GeneralMessage UpdateStatus(int ImprestId, int UserId, int BranchId, string Lang, int Type, int? YearId, string Url, string ImgUrl, string? Reason);

        GeneralMessage UpdateDecisionType(int ImprestId, int UserId, int BranchId, string Lang, int DecisionType);
        GeneralMessage Updateconverttoaccounts(int ImprestId, int UserId, int BranchId, string Lang);
        Task<IEnumerable<LoanVM>> GetAllLoansW2(string SearchText, int status);


        IEnumerable<LoanDetailsVM> GetAllLoanDetails2(int LoanId);
    }
}

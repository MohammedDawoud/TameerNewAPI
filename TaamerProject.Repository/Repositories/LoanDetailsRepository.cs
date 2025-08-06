using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class LoanDetailsRepository :ILoanDetailsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public LoanDetailsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public IEnumerable<LoanDetails> GetAll()
        {
            var loandet= _TaamerProContext.LoanDetails.ToList<LoanDetails>();
            return loandet;

        }

        public async Task<IEnumerable<LoanDetailsVM>> GetAllLoanDetails(int? loanId)
        {
            try { 
            var LoanDetails = _TaamerProContext.LoanDetails.Where(s => s.IsDeleted == false && s.LoanId == loanId).Select(x => new LoanDetailsVM
            {
                LoanDetailsId = x.LoanDetailsId,
                LoanId = x.LoanId,
                Date = x.Date,
                Amount = x.Amount,
                Finished = x.Finished,
                SanadId = x.SanadId,
                UserId = x.UserId,
                AcceptedDate = x.Loan!.AcceptedDate==null?"":x.Loan!.AcceptedDate,
                MonthNo = x.Loan.MonthNo,
                EmployeeName = x.Loan!.Employees!.EmployeeNameAr == null ? "" : x.Loan!.Employees!.EmployeeNameAr,
                //AmountPayed = ,
                //AmountNotPayed = x.ProjectPhasesTasks.Sum(s => s.TimeMinutes),



            }).ToList();
            return LoanDetails;
                }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public  IEnumerable<LoanDetailsVM> GetAllLoanDetails2(int loanId)
        {
            try
            {
                var LoanDetails = _TaamerProContext.LoanDetails.Where(s => s.IsDeleted == false && s.LoanId == loanId).Select(x => new LoanDetailsVM
                {
                    LoanDetailsId = x.LoanDetailsId,
                    LoanId = x.LoanId,
                    Date = x.Date,
                    Amount = x.Amount,
                    Finished = x.Finished,
                    SanadId = x.SanadId,
                    UserId = x.UserId,
                    AcceptedDate = x.Loan!.AcceptedDate == null ? "" : x.Loan!.AcceptedDate,
                    MonthNo = x.Loan.MonthNo,
                    EmployeeName = x.Loan!.Employees!.EmployeeNameAr == null ? "" : x.Loan!.Employees!.EmployeeNameAr,
                    //AmountPayed = ,
                    //AmountNotPayed = x.ProjectPhasesTasks.Sum(s => s.TimeMinutes),
                    


                }).ToList();
                return LoanDetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task< LoanDetailsVM> GetAmountPayedAndNotPayed(int? loanId)
        {

            var SumAmountPayed = _TaamerProContext.LoanDetails.Where(s => s.IsDeleted == false  && s.LoanId == loanId && s.Date<DateTime.Now).Sum(x => x.Amount);
            var SumAmountNotPayed = _TaamerProContext.LoanDetails.Where(s => s.IsDeleted == false  && s.LoanId == loanId && s.Date >= DateTime.Now).Sum(x => x.Amount);
            var LoanDetails = new LoanDetailsVM();
            LoanDetails.AmountPayed = SumAmountPayed == null?0: Math.Ceiling(Convert.ToDecimal(SumAmountPayed));
            LoanDetails.AmountNotPayed = SumAmountNotPayed ==null?0: Math.Ceiling(Convert.ToDecimal(SumAmountNotPayed));
            return LoanDetails;
        }

        public LoanDetails GetById(int Id)
        {
            return _TaamerProContext.LoanDetails.Where(x => x.LoanId == Id).FirstOrDefault();
        }

        public IEnumerable<LoanDetails> GetMatching(Func<LoanDetails, bool> where)
        {
            return _TaamerProContext.LoanDetails.Where(where).ToList<LoanDetails>();
        }
    }
}



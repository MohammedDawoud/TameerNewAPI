using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace TaamerProject.Repository.Repositories
{

    public class LoanRepository : ILoanRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public LoanRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public  async Task<IEnumerable<LoanVM> >GetAllLoans(int? EmpId, string SearchText)
        {
            var Loans = _TaamerProContext.Loan.Where(s => s.IsDeleted == false && s.EmployeeId == EmpId && !s.Employees.IsDeleted && !string.IsNullOrEmpty(s.Employees.WorkStartDate) && string.IsNullOrEmpty(s.Employees.EndWorkDate)).Select(x => new LoanVM
            {
                LoanId = x.LoanId,
                EmployeeId = x.EmployeeId,
                Date = x.Date,
                Amount = x.Amount,
                MonthNo = x.MonthNo,
                Money = x.Money,
                Note = x.Note,
                UserId = x.UserId,
                Status = x.Status,
                LoanStatusName = x.Status == 1 ? "تقديم طلب" : x.Status == 2 ? "تم الموافقة على السلفة" :
                      x.Status == 3 ? "تم رفض السلفة " : x.Status == 4 ? "تم الان مراجعة السلفة" : x.Status == 5 ? "تم تأجيل السلفة " : "تم الانتهاء",
                AddDate = x.AddDate,
                EmployeeName = x.Employees.EmployeeNameAr,
                BranchName = x.Branch.NameAr,
                BranchId = x.BranchId,
                StartDate = x.StartDate==null ?"": x.StartDate,
                StartMonth = x.StartMonth,
                AcceptedDate = x.AcceptedDate,
                Isconverted=x.Isconverted??0,
                WorkStartDate = x.Employees.WorkStartDate
            }).ToList();
            if (SearchText != "")
            {
                Loans = Loans.Where(s => s.MonthNo.ToString().Contains(SearchText.Trim()) || s.Amount.ToString().Contains(SearchText.Trim())).ToList();
            }
            return Loans;
        }

        public  async Task<IEnumerable<LoanVM> >GetAllLoansE(int? UserID_E, string SearchText)
        {
            if(UserID_E==null)
            {
                UserID_E = 0;
            }

            var Loans = _TaamerProContext.Loan.Where(s => s.IsDeleted == false && s.EmployeeId == UserID_E && !s.Employees.IsDeleted && !string.IsNullOrEmpty(s.Employees.WorkStartDate) && string.IsNullOrEmpty(s.Employees.EndWorkDate)).Select(x => new LoanVM
            {
                LoanId = x.LoanId,
                EmployeeId = x.EmployeeId,
                Date = x.Date,
                Amount = x.Amount,
                MonthNo = x.MonthNo,
                Money = _TaamerProContext.LoanDetails.Where(s => s.IsDeleted == false && s.LoanId == x.LoanId && s.Date < DateTime.Now).Sum(x => x.Amount),
                Note = x.Note,
                UserId = x.UserId,
                Status = x.Status,
                LoanStatusName = x.Status == 1 ? "تقديم طلب" : x.Status == 2 ? "تم الموافقة على السلفة" :
                      x.Status == 3 ? "تم رفض السلفة " : x.Status == 4 ? "تم الان مراجعة السلفة" : x.Status == 5 ? "تم تأجيل السلفة " : "تم الانتهاء",
                AddDate = x.AddDate,
                EmployeeName = x.Employees.EmployeeNameAr,
                BranchName = x.Branch.NameAr,
                BranchId = x.BranchId,
                StartDate = x.StartDate == null ? "" : x.StartDate,
                StartMonth = x.StartMonth,
                AcceptedDate = x.AcceptedDate,
                Isconverted = x.Isconverted ?? 0,
                WorkStartDate = x.Employees.WorkStartDate,
                
               
            }).ToList();
            if (SearchText != "")
            {
                Loans = Loans.Where(s => s.MonthNo.ToString().Contains(SearchText.Trim()) || s.Amount.ToString().Contains(SearchText.Trim())).ToList();
            }
            return Loans;
        }


        public  async Task<IEnumerable<LoanVM> >GetAllLoansAfterDecision(int? EmpId, string SearchText)
        {
            var Loans = _TaamerProContext.Loan.Where(s => s.IsDeleted == false && s.DecisionType ==1 && !s.Employees.IsDeleted && !string.IsNullOrEmpty(s.Employees.WorkStartDate) && string.IsNullOrEmpty(s.Employees.EndWorkDate)).Select(x => new LoanVM
            {
                LoanId = x.LoanId,
                EmployeeId = x.EmployeeId,
                Date = x.Date,
                Amount = x.Amount,
                MonthNo = x.MonthNo,
                Money = x.Money,
                Note = x.Note,
                UserId = x.UserId,
                Status = x.Status,
                LoanStatusName = x.Status == 1 ? "تقديم طلب" : x.Status == 2 ? "تم الموافقة على السلفة" :
                      x.Status == 3 ? "تم رفض السلفة " : x.Status == 4 ? "تم الان مراجعة السلفة" : x.Status == 5 ? "تم تأجيل السلفة " : "تم الانتهاء",
                AddDate = x.AddDate,
                EmployeeName = x.Employees.EmployeeNameAr,
                BranchName = x.Branch.NameAr,
                BranchId = x.BranchId,
                StartDate = x.StartDate == null ? "" : x.StartDate,
                StartMonth = x.StartMonth,
                AcceptedDate = x.AcceptedDate,
                Isconverted = x.Isconverted ?? 0,
                WorkStartDate = x.Employees.WorkStartDate
            }).ToList();
            if (SearchText != "")
            {
                Loans = Loans.Where(s => s.MonthNo.ToString().Contains(SearchText.Trim()) || s.Amount.ToString().Contains(SearchText.Trim())).ToList();
            }
            return Loans;
        }

        public  async Task<IEnumerable<LoanVM> >GetAllLoans2(int? UserId, string SearchText)
        {
            var Loans = _TaamerProContext.Loan.Where(s => s.IsDeleted == false && s.UserId == UserId && !s.Employees.IsDeleted && !string.IsNullOrEmpty(s.Employees.WorkStartDate) && string.IsNullOrEmpty(s.Employees.EndWorkDate)).Select(x => new LoanVM
            {
                LoanId = x.LoanId,
                EmployeeId = x.EmployeeId,
                Date = x.Date,
                Amount = x.Amount,
                MonthNo = x.MonthNo,
                Money = x.Money,
                Note = x.Note,
                UserId = x.UserId,
                Status = x.Status,
                LoanStatusName = x.Status == 1 ? "تقديم طلب" : x.Status == 2 ? "تم الموافقة على السلفة" :
                      x.Status == 3 ? "تم رفض السلفة " : "تحت الطلب",
                AddDate = x.AddDate,
                EmployeeName = x.Employees.EmployeeNameAr,
                BranchName = x.Branch.NameAr,
                BranchId = x.BranchId,
                StartDate = x.StartDate == null ? "" : x.StartDate,
                StartMonth = x.StartMonth,
                AcceptedDate = x.AcceptedDate,
                Isconverted = x.Isconverted ?? 0,
                WorkStartDate = x.Employees.WorkStartDate,
                EmployeeNo = x.Employees.EmployeeNo,
                IdentityNo = x.Employees.NationalId.ToString(),
                EmployeeJob = x.Employees.Job.JobNameAr,
                DepartmentName = x.Employees.Department.DepartmentNameAr,
            }).ToList();
            if (SearchText != "")
            {
                Loans = Loans.Where(s => s.MonthNo.ToString().Contains(SearchText.Trim()) || s.Amount.ToString().Contains(SearchText.Trim())).ToList();
            }
            return Loans;
        }

        public  async Task<IEnumerable<LoanVM> >GetAllLoansW(string SearchText)
        {
            var Loans = _TaamerProContext.Loan.Where(s => s.IsDeleted == false && s.DecisionType == 1 && !s.Employees.IsDeleted && !string.IsNullOrEmpty(s.Employees.WorkStartDate) && string.IsNullOrEmpty(s.Employees.EndWorkDate)).Select(x => new LoanVM
            {
                LoanId = x.LoanId,
                EmployeeId = x.EmployeeId,
                Date = x.Date,
                Amount = x.Amount,
                MonthNo = x.MonthNo,
                Money = x.Money,
                Note = x.Note,
                UserId = x.UserId,
                Status = x.Status,
                LoanStatusName = x.Status == 1 ? "تقديم طلب" : x.Status == 2 ? "تم الموافقة على السلفة" :
                        x.Status == 3 ? "تم رفض السلفة " : x.Status == 4 ? "تم الان مراجعة السلفة" : x.Status == 5 ? "تم تأجيل السلفة " : "تم الانتهاء",
                AddDate = x.AddDate,
                EmployeeName = x.Employees.EmployeeNameAr == null ? "" : x.Employees.EmployeeNameAr,
                BranchName = x.Branch.NameAr == null ? "" : x.Branch.NameAr,
                BranchId = x.BranchId,
                StartDate = x.StartDate == null ? "" : x.StartDate,
                StartMonth = x.StartMonth,
                AcceptedDate = x.AcceptedDate,
                Isconverted = x.Isconverted ?? 0,
                WorkStartDate = x.Employees.WorkStartDate
            }).ToList();
            if (SearchText != "")
            {
                Loans = Loans.Where(s => s.MonthNo.ToString().Contains(SearchText.Trim()) || s.Amount.ToString().Contains(SearchText.Trim())).ToList();
            }
            return Loans;
        }


        public  async Task<IEnumerable<LoanVM> >GetAllLoansW2(string SearchText,int status)
        {
            var Loans = _TaamerProContext.Loan.Where(s => s.IsDeleted == false && s.DecisionType == 1 && !s.Employees.IsDeleted && !string.IsNullOrEmpty(s.Employees.WorkStartDate) && string.IsNullOrEmpty(s.Employees.EndWorkDate)).Select(x => new LoanVM
            {
                LoanId = x.LoanId,
                EmployeeId = x.EmployeeId,
                Date = x.Date,
                Amount = x.Amount,
                MonthNo = x.MonthNo,
                Money = x.Money,
                Note = x.Note,
                UserId = x.UserId,
                Status = x.Status,
                LoanStatusName = x.Status == 1 ? "تقديم طلب" : x.Status == 2 ? "تم الموافقة على السلفة" :
                        x.Status == 3 ? "تم رفض السلفة " : x.Status == 4 ? "تم الان مراجعة السلفة" : x.Status == 5 ? "تم تأجيل السلفة " : "تم الانتهاء",
                AddDate = x.AddDate,
                EmployeeName = x.Employees.EmployeeNameAr == null ? "" : x.Employees.EmployeeNameAr,
                BranchName = x.Branch.NameAr == null ? "" : x.Branch.NameAr,
                BranchId = x.BranchId,
                StartDate = x.StartDate == null ? "" : x.StartDate,
                StartMonth = x.StartMonth,
                AcceptedDate = x.AcceptedDate,
                Isconverted = x.Isconverted ?? 0,
                WorkStartDate = x.Employees.WorkStartDate,
                EmployeeNo = x.Employees.EmployeeNo,
                IdentityNo = x.Employees.NationalId.ToString(),
                EmployeeJob = x.Employees.Job.JobNameAr,
                DepartmentName = x.Employees.Department.DepartmentNameAr,
            }).ToList();
            if (SearchText != "")
            {
                Loans = Loans.Where(s => s.MonthNo.ToString().Contains(SearchText.Trim()) || s.Amount.ToString().Contains(SearchText.Trim())).ToList();
            }
            if(status > 0 && status != null)
            {
                Loans = Loans.Where(s => s.Status== status).ToList();

            }
            return Loans;
        }

        public  async Task<IEnumerable<LoanVM> >GetAllImprestSearch(int BranchId)
        {
            try { 
            var Loans = _TaamerProContext.Loan.Where(s => s.IsDeleted == false && !s.Employees!.IsDeleted && !string.IsNullOrEmpty(s.Employees!.WorkStartDate) && string.IsNullOrEmpty(s.Employees!.EndWorkDate)).Select(x => new LoanVM
            {
                LoanId = x.LoanId,
                EmployeeId = x.EmployeeId,
                Date = x.Date,
                Amount = x.Amount,
                MonthNo = x.MonthNo,
                Money = x.Money,
                Note = x.Note,
                UserId = x.UserId,
                Status=x.Status,
                LoanStatusName = x.Status == 1 ? "تقديم طلب" : x.Status == 2 ? "تم الموافقة على السلفة" :
                      x.Status == 3 ? "تم رفض السلفة " : x.Status == 4 ? "يتم الان مراجعة السلفة" : x.Status == 5 ? "تم تأجيل السلفة " : "تم الانتهاء",
                EmployeeName = x.Employees!.EmployeeNameAr,
                BranchName = x.Branch!.NameAr,
                BranchId = x.BranchId,
                StartDate = x.StartDate,
                StartMonth = x.StartMonth,
                AcceptedDate = x.AcceptedDate,
                DecisionType=x.DecisionType,
                Isconverted = x.Isconverted ?? 0,
                WorkStartDate = x.Employees!.WorkStartDate,
                AcceptUser = x.UserAcccept!.FullNameAr??"",
                EmployeeNo = x.Employees.EmployeeNo,
                IdentityNo = x.Employees.NationalId.ToString(),
                EmployeeJob = x.Employees.Job.JobNameAr,
                DepartmentName = x.Employees.Department.DepartmentNameAr,

            }).ToList();
            return Loans;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public  async Task<IEnumerable<LoanVM> >GetAllImprestBySearchObject(LoanVM ImprestSearch, int BranchId)
        {
            try
            {
                if (ImprestSearch.EndDate == null || ImprestSearch.StartDate == null)
                {
                    var loan1 = _TaamerProContext.Loan.Where(s => s.IsDeleted == false && (s.EmployeeId == ImprestSearch.EmployeeId || ImprestSearch.EmployeeId == null) &&
                   (s.Status == ImprestSearch.Status || ImprestSearch.Status == null) && !s.Employees.IsDeleted && !string.IsNullOrEmpty(s.Employees.WorkStartDate) && string.IsNullOrEmpty(s.Employees.EndWorkDate)).Select(x => new LoanVM
                   {

                      LoanId = x.LoanId,
                      EmployeeId = x.EmployeeId,
                      Date = x.Date,
                      Amount = x.Amount,
                      MonthNo = x.MonthNo,
                      Money = x.Money,
                      Note = x.Note,
                      UserId = x.UserId,
                       Status = x.Status,

                       LoanStatusName = x.Status == 1 ? "تقديم طلب" : x.Status == 2 ? "تم الموافقة على السلفة" :
                      x.Status == 3 ? "تم رفض السلفة " : x.Status == 4 ? "يتم الان مراجعة السلفة" : x.Status == 5 ? "تم تأجيل السلفة " : "تم الانتهاء",
                       EmployeeName = x.Employees.EmployeeNameAr,
                       BranchName = x.Branch.NameAr,
                       BranchId = x.BranchId,
                       StartDate = x.StartDate,
                       StartMonth = x.StartMonth,
                       AcceptedDate = x.AcceptedDate,
                       DecisionType = x.DecisionType,
                       WorkStartDate = x.Employees.WorkStartDate,
                       Isconverted = x.Isconverted ?? 0,
                       AcceptUser = x.UserAcccept.FullNameAr??"",
                       EmployeeNo = x.Employees.EmployeeNo,
                       IdentityNo = x.Employees.NationalId.ToString(),
                       EmployeeJob = x.Employees.Job.JobNameAr,
                       DepartmentName=x.Employees.Department.DepartmentNameAr,


                   }).Select(s => new LoanVM
                  {

                      LoanId = s.LoanId,
                      EmployeeId = s.EmployeeId,
                      Date = s.Date,
                      Amount = s.Amount,
                      MonthNo = s.MonthNo,
                      Money = s.Money,
                      Note = s.Note,
                      UserId = s.UserId,
                       Status = s.Status,

                       LoanStatusName = s.LoanStatusName,
                       EmployeeName = s.EmployeeName,
                       BranchName = s.BranchName,
                       BranchId = s.BranchId,
                       StartDate = s.StartDate,
                       StartMonth = s.StartMonth,
                       AcceptedDate = s.AcceptedDate,
                       DecisionType = s.DecisionType,
                       WorkStartDate = s.WorkStartDate,
                       Isconverted = s.Isconverted ?? 0,
                       AcceptUser = s.AcceptUser??"",
                       DepartmentName=s.DepartmentName,
                       EmployeeJob = s.EmployeeJob,
                       EmployeeNo = s.EmployeeNo,
                       IdentityNo = s.IdentityNo,
                       
                   }).ToList();
                    return loan1;
                }
                else
                {
                    var loan = _TaamerProContext.Loan.Where(s => s.IsDeleted == false && (s.EmployeeId == ImprestSearch.EmployeeId || ImprestSearch.EmployeeId == null) &&
                    (s.Status == ImprestSearch.Status || ImprestSearch.Status == null)).Select(x => new LoanVM
                    {
                        LoanId = x.LoanId,
                        EmployeeId = x.EmployeeId,
                        Date = x.Date,
                        Amount = x.Amount,
                        MonthNo = x.MonthNo,
                        Money = x.Money,
                        Note = x.Note,
                        UserId = x.UserId,
                        Status = x.Status,
                        LoanStatusName = x.Status == 1 ? "تقديم طلب" : x.Status == 2 ? "تم الموافقة على السلفة" :
       x.Status == 3 ? "تم رفض السلفة " : x.Status == 4 ? "تم الان مراجعة السلفة" : x.Status == 5 ? "تم تأجيل السلفة " : "تم الانتهاء",
                        EmployeeName = x.Employees.EmployeeNameAr,
                        BranchName = x.Branch.NameAr,
                        BranchId = x.BranchId,
                        StartDate = x.StartDate,
                        StartMonth = x.StartMonth,
                        AcceptedDate = x.AcceptedDate,
                        DecisionType = x.DecisionType,
                        Isconverted = x.Isconverted ?? 0,
                        WorkStartDate = x.Employees.WorkStartDate,
                        AcceptUser = x.UserAcccept.FullName ?? "",
                           EmployeeNo = x.Employees.EmployeeNo,
                        IdentityNo = x.Employees.NationalId.ToString(),
                        EmployeeJob = x.Employees.Job.JobNameAr,
                        DepartmentName = x.Employees.Department.DepartmentNameAr,
                    }).ToList();


                    //loan = loan.Where(s => DateTime.ParseExact(s.WorkStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ImprestSearch.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).ToList();

                    //}).ToList().Where(s => DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(ImprestSearch.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture));

                    return loan.Where(s => DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(ImprestSearch.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) &&
                    DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ImprestSearch.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)); ;
                }
            }
            catch (Exception ex)
            {
                var loans = _TaamerProContext.Loan.Where(s => s.IsDeleted == false).Select(x => new LoanVM
                {
                    LoanId = x.LoanId,
                    EmployeeId = x.EmployeeId,
                    Date = x.Date,
                    Amount = x.Amount,
                    MonthNo = x.MonthNo,
                    Money = x.Money,
                    Note = x.Note,
                    UserId = x.UserId,
                    Status = x.Status,

                    LoanStatusName = x.Status == 1 ? "تقديم طلب" : x.Status == 2 ? "تم الموافقة على السلفة" :
                      x.Status == 3 ? "تم رفض السلفة " : x.Status == 4 ? "تم الان مراجعة السلفة" : x.Status == 5 ? "تم تأجيل السلفة " : "تم الانتهاء",
                    EmployeeName = x.Employees.EmployeeNameAr,
                    BranchName = x.Branch.NameAr,
                    BranchId = x.BranchId,
                    StartDate = x.StartDate,
                    StartMonth = x.StartMonth,
                    AcceptedDate = x.AcceptedDate,
                    DecisionType = x.DecisionType,
                    Isconverted = x.Isconverted ?? 0,
                    WorkStartDate = x.Employees.WorkStartDate,
                    AcceptUser = x.UserAcccept.FullName??"",
                    EmployeeNo = x.Employees.EmployeeNo,
                    IdentityNo = x.Employees.NationalId.ToString(),
                    EmployeeJob = x.Employees.Job.JobNameAr,
                    DepartmentName = x.Employees.Department.DepartmentNameAr,
                });
                return loans;
            }

        }

        public IEnumerable<Loan> GetAll()
        {
            return _TaamerProContext.Loan.ToList<Loan>();
        }

        public Loan GetById(int Id)
        {
          return  _TaamerProContext.Loan.Where(x => x.LoanId == Id).FirstOrDefault();
        }

        public IEnumerable<Loan> GetMatching(Func<Loan, bool> where)
        {
            return _TaamerProContext.Loan.Where(where).ToList<Loan>();
        }
    }
}



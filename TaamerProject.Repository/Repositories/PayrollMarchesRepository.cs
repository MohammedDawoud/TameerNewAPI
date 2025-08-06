using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Interfaces
{
    public class PayrollMarchesRepository : IPayrollMarchesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public PayrollMarchesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<PayrollMarches> GetPayrollMarches(int EmpId, int MonthId)
        {
            var payroll = _TaamerProContext.PayrollMarches.Where(x => !x.IsDeleted && x.MonthNo == MonthId && x.EmpId == EmpId).OrderByDescending(x => x.PayrollId).FirstOrDefault();
            
            return payroll;
        }
        public async Task<PayrollMarches> GetPayrollMarches(int EmpId, int MonthId,int YearId)
        {
            var payroll = _TaamerProContext.PayrollMarches.Where(x => !x.IsDeleted && x.MonthNo == MonthId && x.EmpId == EmpId && x.YearId== YearId).OrderByDescending(x=>x.PayrollId).FirstOrDefault();

            return payroll;
        }

        public async Task<IEnumerable<PayrollMarchesVM>> GetPayrollMarches(int MonthId, int BranchSearch, string SearchText)
        {
            var payrolls = _TaamerProContext.PayrollMarches.Where(x => !x.IsDeleted && x.MonthNo == MonthId && 
            
            ((BranchSearch > 0 && x.Employee.BranchId == BranchSearch) ||(BranchSearch == 0 && true)) ).Select(x => new PayrollMarchesVM
            {
                PayrollId = x.PayrollId,
                MonthNo = x.MonthNo,
                EmpId = x.EmpId.Value,
                EmpName = x.Employee.EmployeeNameAr,
                PostDate = x.PostDate,
                MainSalary = x.MainSalary,
                SalaryOfThisMonth = x.SalaryOfThisMonth,
                Bonus = x.Bonus,
                CommunicationAllawance = x.CommunicationAllawance,
                ProfessionAllawance =x.ProfessionAllawance,
                TransportationAllawance = x.TransportationAllawance,
                HousingAllowance = x.HousingAllowance,
                MonthlyAllowances = x.MonthlyAllowances,
                ExtraAllowances = x.ExtraAllowances,
                TotalRewards = x.TotalRewards,
                TotalDiscounts = x.TotalDiscounts,  
                TotalLoans= x.TotalLoans,
                TotalSalaryOfThisMonth = x.TotalSalaryOfThisMonth,
                TotalAbsDays= x.TotalAbsDays,
                TotalVacations = x.TotalVacations,
                AddUser = x.AddUser,
                AddDate = x.AddDate,
                IsPostVoucher=x.IsPostVoucher??false,
                IsPostPayVoucher=x.IsPostPayVoucher??false,
                Taamen=x.Taamen??"0",
            }).ToList();
            return payrolls;
        }

        public async Task<IEnumerable<PayrollMarchesVM>> GetPayrollMarches(int MonthId, int BranchSearch, string SearchText,int YearId)
        {
            var payrolls = _TaamerProContext.PayrollMarches.Where(x => !x.IsDeleted && x.MonthNo == MonthId && x.YearId == YearId &&

            ((BranchSearch > 0 && x.Employee.BranchId == BranchSearch) || (BranchSearch == 0 && true))).Select(x => new PayrollMarchesVM
            {
                PayrollId = x.PayrollId,
                MonthNo = x.MonthNo,
                EmpId = x.EmpId.Value,
                EmpName = x.Employee.EmployeeNameAr,
                PostDate = x.PostDate,
                MainSalary = x.MainSalary,
                SalaryOfThisMonth = x.SalaryOfThisMonth,
                Bonus = x.Bonus,
                CommunicationAllawance = x.CommunicationAllawance,
                ProfessionAllawance = x.ProfessionAllawance,
                TransportationAllawance = x.TransportationAllawance,
                HousingAllowance = x.HousingAllowance,
                MonthlyAllowances = x.MonthlyAllowances,
                ExtraAllowances = x.ExtraAllowances,
                TotalRewards = x.TotalRewards,
                TotalDiscounts = x.TotalDiscounts,
                TotalLoans = x.TotalLoans,
                TotalSalaryOfThisMonth = x.TotalSalaryOfThisMonth,
                TotalAbsDays = x.TotalAbsDays,
                TotalVacations = x.TotalVacations,
                AddUser = x.AddUser,
                AddDate = x.AddDate,
                IsPostVoucher = x.IsPostVoucher ?? false,
                IsPostPayVoucher = x.IsPostPayVoucher ?? false,
                Taamen = x.Taamen ?? "0",
                YearId = x.YearId,
                TotalLateDiscount = x.TotalLateDiscount,
                TotalAbsenceDiscount =x.TotalAbsenceDiscount,

                
            }).ToList().OrderByDescending(x=>x.PayrollId).ToList();
            return payrolls.DistinctBy(x => x.EmpId).ToList();
        }


    }
}

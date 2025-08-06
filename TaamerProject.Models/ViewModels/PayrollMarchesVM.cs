using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class PayrollMarchesVM : Auditable
    {
        public int PayrollId { get; set; }
        public int EmpId { get; set; }
        public string? EmpName { get; set; }
        public int MonthNo { get; set; }
        public string? MonthName { get; set; }
        public DateTime? PostDate { get; set; }
        public decimal? MainSalary { get; set; }
        public decimal? SalaryOfThisMonth { get; set; }
        public decimal? Bonus { get; set; }
        public decimal? CommunicationAllawance { get; set; }
        public decimal? ProfessionAllawance { get; set; }
        public decimal? TransportationAllawance { get; set; }
        public decimal? HousingAllowance { get; set; }
        public decimal? MonthlyAllowances { get; set; }
        public decimal? ExtraAllowances { get; set; }
        public decimal? TotalRewards { get; set; }
        public decimal? TotalDiscounts { get; set; }
        public decimal? TotalLoans { get; set; }
        public decimal? TotalSalaryOfThisMonth { get; set; }
        public decimal? TotalAbsDays { get; set; }
        public decimal? TotalVacations { get; set; }
        public decimal? TotalLateDiscount { get; set; }
        public decimal? TotalAbsenceDiscount { get; set; }
        public bool? IsPostVoucher { get; set; }

        public bool? IsPostPayVoucher { get; set; }
        public string? Taamen { get; set; }
        public int? YearId { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaamerProject.Models
{
    public class EmployeesSalaryRptVM
    {
        public string? EmployeeNameAr { get; set; }

        public string? Salary { get; set; }

        public string? CommunicationAllawance { get; set; }
        public string? ProfessionAllawance { get; set; }
        public string? TransportationAllawance { get; set; }
        public string? HousingAllowance { get; set; }

        public string? MonthlyAllowances { get; set; }
        public string? AddAllowances { get; set; }
        public string? TotalLoans { get; set; }
        public string? Bonus { get; set; }
        public string? TotalDiscounts { get; set; }
        public string? TotalRewards { get; set; }
        public string? TotalViolations { get; set; }
        public string? TotalySalaries { get; set; }
        public string? TotalyDays { get; set; }
        public string? TotalLateDiscount { get; set; }
        public string? TotalAbsenceDiscount { get; set; }
    }
    public class EmployeesSalaryRptVM_New
    {
        public List<EmployeesSalaryRptVM>? employeesSalaries { get; set; }
        public OrganizationsVM? OrgData{ get; set; }
        public TotalEmployeesSalaryRptVM Total{ get; set; }
        public string? BranchName { get; set; }
        public string? MonthName { get; set; }
    }

    public class TotalEmployeesSalaryRptVM
    {

        public string? TSalary { get; set; }

        public string? TCommunicationAllawance { get; set; }
        public string? TProfessionAllawance { get; set; }
        public string? TTransportationAllawance { get; set; }
        public string? THousingAllowance { get; set; }

        public string? TMonthlyAllowances { get; set; }
        public string? TAddAllowances { get; set; }
        public string? TTotalLoans { get; set; }
        public string? TBonus { get; set; }
        public string? TTotalDiscounts { get; set; }
        public string? TTotalRewards { get; set; }
        public string? TTotalViolations { get; set; }
        public string? TTotalySalaries { get; set; }
        public string? TTotalyDays { get; set; }
        public string? TTotalLateDiscount { get; set; }
        public string? TTotalAbsenceDiscount { get; set; }

    }


}
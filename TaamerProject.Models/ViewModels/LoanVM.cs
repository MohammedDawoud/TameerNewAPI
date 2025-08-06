using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class LoanVM
    {
        public int LoanId { get; set; }
        public int? EmployeeId { get; set; }
        public string?  Date { get; set; }
        public string? HijriDate { get; set; }
        public decimal? Amount { get; set; }
        public int? MonthNo { get; set; }
        public decimal? Money { get; set; }
        public string? Note { get; set; }
        public int? UserId { get; set; }
        public int? Status { get; set; }
        public string? LoanStatusName { get; set; }
        public bool IsSearch { get; set; }
        public string? EmployeeName { get; set; }

        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public int? StartMonth { get; set; }
        public string? AcceptedDate { get; set; }
        public int? DecisionType { get; set; }
        public string? WorkStartDate { get; set; }
        public string? AcceptUser { get; set; }
        public int? Isconverted { get; set; }

        public DateTime? AddDate { get; set; }

        public string? EmployeeNo { get; set; }
        public string? EmployeeJob { get; set; }
        public string? IdentityNo { get; set; }
        public string? DepartmentName { get; set; }
    }
}

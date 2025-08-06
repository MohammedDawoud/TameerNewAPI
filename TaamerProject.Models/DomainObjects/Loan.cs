using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Loan : Auditable
    {
        public int LoanId { get; set; }
        public int? EmployeeId { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public decimal? Amount { get; set; }
        public int? MonthNo { get; set; }
        public decimal? Money { get; set; }
        public string? Note { get; set; }
        public int? UserId { get; set; }
        public int? Status { get; set; }
        public int? BranchId { get; set; }
        public string? StartDate { get; set; }
        public int? StartMonth { get; set; }
        public string? AcceptedDate { get; set; }
        public int? DecisionType { get; set; }
      public int? AcceptedUser { get; set; }
        public int? Isconverted { get; set; }

        public virtual Branch? Branch { get; set; }
        public virtual Employees? Employees { get; set; }
        public virtual List<LoanDetails>? LoanDetails { get; set; }
        public virtual Users? UserAcccept { get; set; }

    }
}

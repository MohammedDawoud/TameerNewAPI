using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ExpensesGovernment : Auditable
    {
        public int ExpensesId { get; set; }
        public int? EmployeeId { get; set; }
        public int? TypeId { get; set; }
        public string? StartDate { get; set; }
        public string? StartHijriDate { get; set; }
        public string? EndDate { get; set; }
        public string? EndHijriDate { get; set; }
        public string? Notes { get; set; }
        public decimal? Amount { get; set; }
        public int? UserId { get; set; }
        public int? Year { get; set; }
        public int? HijriYear { get; set; }
        public ExpensesGovernmentType? ExpGovType { get; set; }
       // public Employees Employees { get; set; }
    }
}

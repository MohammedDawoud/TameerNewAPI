using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Allowance : Auditable
    {
        public int AllowanceId { get; set; }
        public int? EmployeeId { get; set; }
        public int? AllowanceTypeId { get; set; }
        public string? Date { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Month { get; set; }
        public bool IsFixed { get; set; }
        public decimal? AllowanceAmount { get; set; }
        public int? UserId { get; set; }
        public int? AllowanceMonthNo { get; set; }
        public virtual AllowanceType? AllowanceType { get; set; }
        public virtual Employees? Employees { get; set; }
    }
}

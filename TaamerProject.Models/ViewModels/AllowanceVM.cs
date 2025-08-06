using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class AllowanceVM
    {
        public int AllowanceId { get; set; }
        public int? EmployeeId { get; set; }
        public int? AllowanceTypeId { get; set; }
        public  string?   Date { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Month { get; set; }
        public bool? IsFixed { get; set; }
        public decimal? AllowanceAmount { get; set; }
        public int? UserId { get; set; }
        public string? AllowanceTypeName { get; set; }
        public bool? IsSearch { get; set; }
        public string? EmployeeName { get; set; }
        public int? Status { get; set; }

        public int? AllowanceMonthNo { get; set; }
    }
}

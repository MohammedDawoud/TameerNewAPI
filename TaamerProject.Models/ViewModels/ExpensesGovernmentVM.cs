using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class ExpensesGovernmentVM
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
        public string? ExpGovTypeName { get; set; }
    }
}

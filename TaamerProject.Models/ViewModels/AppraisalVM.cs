using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class AppraisalVM
    {
        public int AppraisalId { get; set; }
        public int? EmpId { get; set; }
        public decimal? Degree { get; set; }
        public int? ManagerId { get; set; }
        public DateTime? MonthDate { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public string? EmployeeName { get; set; }
    }
}

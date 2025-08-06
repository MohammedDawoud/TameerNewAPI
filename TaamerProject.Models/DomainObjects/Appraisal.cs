using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Appraisal : Auditable
    {
        public int AppraisalId { get; set; }
        public int? EmpId { get; set; }
        public decimal Degree { get; set; }
        public int? ManagerId { get; set; }
        public DateTime? MonthDate { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public  Employees? Employees { get; set; }

    }
}
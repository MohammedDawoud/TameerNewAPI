using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class AttAbsentDayVM
    {
        public int Id { get; set; }
        public int? EmpId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? AbsDays { get; set; }
        public DateTime? SDate { get; set; }
        public DateTime? EDate { get; set; }
    }
}

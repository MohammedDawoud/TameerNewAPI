using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class FiscalYears : Auditable
    {
        public int FiscalId { get; set; }
        public int? YearId { get; set; }
        public string? YearName { get; set; }
        public int? BranchId { get; set; }
        public int? UserId { get; set; }
        public bool? IsActive { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class OutMovements : Auditable
    {
        public long MoveId { get; set; }
        public int? ConstraintNo { get; set; }
        public int? EmpId { get; set; }
        public int? OrderNo { get; set; }
        public string? RequiredWork { get; set; }
        public string? FinishedWork { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public int? ExpeditorId { get; set; }
        public int? TrailingId { get; set; }
        public int? BranchId { get; set; }
        public virtual ProjectTrailing? ProjectTrailing { get; set; }
        public virtual Employees? Employees { get; set; }
        public virtual ExternalEmployees? ExternalEmployees { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class OutMovementsVM
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
        public string? SideName { get; set; }
        public string? EmployeeName { get; set; }
        public string? ExternalEmployeeName { get; set; }
    }
}

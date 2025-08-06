
using System.ComponentModel.DataAnnotations.Schema;

namespace TaamerProject.Models
{
    public class ProjectTrailing : Auditable
    {
        public int TrailingId { get; set; }
        public int? ProjectId { get; set; }
        public int? DepartmentId { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public bool Active { get; set; }
        public int? Status { get; set; }
        public int? TypeId { get; set; }
        public int? UserId { get; set; }
        public string? ReceiveDate { get; set; }
        public string? ReceiveHijriDate { get; set; }
        public int? ReceiveUserId { get; set; }
        public string? Notes { get; set; }
        public int? TaskId { get; set; }
        public int? BranchId { get; set; }
        public virtual Project? Project { get; set; }
        public virtual Department? Department { get; set; }
        [NotMapped]
        public List<TrailingFiles>? TrailingFiles { get; set; }
    }
}

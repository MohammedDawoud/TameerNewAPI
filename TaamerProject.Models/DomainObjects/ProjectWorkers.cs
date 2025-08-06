
namespace TaamerProject.Models
{
    public class ProjectWorkers : Auditable
    {
        public long WorkerId { get; set; }
        public int? ProjectId { get; set; }
        public int? UserId { get; set; }
        public int? BranchId { get; set; }
        public int? WorkerType { get; set; }
        public virtual Users? Users { get; set; }
        public virtual Project? Project { get; set; }
    }
}

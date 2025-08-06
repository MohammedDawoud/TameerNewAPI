

namespace TaamerProject.Models
{
    public class ProUserPrivileges : Auditable
    {
        public long UserPrivId { get; set; }
        public int? PrivilegeId { get; set; }
        public int? ProjectID { get; set; }
        public string? Projectno { get; set; }
        public int? UserId { get; set; }
        public bool? Select { get; set; }
        public bool? Insert { get; set; }
        public bool? Update { get; set; }
        public bool? Delete { get; set; }
        public int? CustomerID { get; set; }
        public virtual Users? Users { get; set; }
        public virtual Project? Projects { get; set; }
    }
}

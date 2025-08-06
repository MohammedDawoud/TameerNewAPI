

namespace TaamerProject.Models
{
    public class ProUserPrivilegesVM
    {
        public long UserPrivId { get; set; }
        public int? PrivilegeId { get; set; }
        public int? ProjectID { get; set; }
        public string? Projectno { get; set; }
        public int? UserId { get; set; }
        public string? FullName { get; set; }
        public bool? Select { get; set; }
        public bool? Insert { get; set; }
        public bool? Update { get; set; }
        public bool? Delete { get; set; }
        public int? CustomerID { get; set; }
        public int? MangerId { get; set; }
        public virtual Users Users { get; set; }
        public virtual Project Project { get; set; }
    }
}

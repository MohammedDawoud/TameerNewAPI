using System;

namespace TaamerProject.Models
{
    public class GroupPrivilegesVM : Auditable
    {
        public long GroupPrivId { get; set; }
        public int? GroupId { get; set; }
        public int? PrivilegeId { get; set; }
        public int? BranchId { get; set; }
    }
}

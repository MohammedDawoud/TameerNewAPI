using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class UserBranches : Auditable
    {
        public long UserBranchId { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public Branch? Branches { get; set; }
    }
}

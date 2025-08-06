using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.DomainObjects
{
    public class Permissions :Auditable
    {
        public int PermissionId { get; set; }
        public int? EmpId { get; set; }
        public int? TypeId { get; set; }
        public string? Date { get;set;}
	    public string? Reason { get; set; }
        public int? Status { get; set; }
        public string? AcceptedDate { get; set; }
        public int? PermissionHours { get; set; }
        public int? DecisionType { get; set; }
        public int? AcceptedUser { get; set; }
        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
        public int? UserId { get; set; }
        public int? BranchId { get; set; }

        public virtual PermissionType? PermissionType { get; set; }
        public virtual Employees? Employee { get; set; }
        public virtual Users? UserAcccept { get; set; }

    }
}

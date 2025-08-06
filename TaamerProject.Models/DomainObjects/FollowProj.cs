using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class FollowProj : Auditable
    {
        public int FollowProjId { get; set; }
        public int? ProjectId { get; set; }
        public int? EmpId { get; set; }
        public string? TimeNo { get; set; }
        public string? TimeType { get; set; }
        public string? EmpRate { get; set; }
        public decimal? Amount { get; set; }
        public decimal? ExpectedCost { get; set; }
        public bool? ConfirmRate { get; set; }
        public virtual Project? project { get; set; }
        public virtual Employees? employees { get; set; }


    }
}

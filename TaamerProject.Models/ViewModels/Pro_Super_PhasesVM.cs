using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Pro_Super_PhasesVM
    {
        public int PhaseId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public bool? IsRead { get; set; }
        public int? UserId { get; set; }
        public string? Note { get; set; }
        public int? BranchId { get; set; }
        public string? SuperCode { get; set; }

        public virtual List<Pro_Super_PhaseDetVM>? SuperPhaseDet { get; set; }

    }
}

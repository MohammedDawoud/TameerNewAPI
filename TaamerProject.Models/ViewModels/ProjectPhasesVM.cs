using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class ProjectPhasesVM
    {
        public int PhaseId { get; set; }
        public string? PhaseCode { get; set; }
        public string? PhaseNameAr { get; set; }
        public string? PhaseNameEn { get; set; }
        public int? ProjectId { get; set; }
        public string? ProjectName { get; set; }
    }
}

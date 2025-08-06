using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class FullProjectsReport : Auditable
    {

        public int ReportId { get; set; }
        public int? Type { get; set; }
        public int? ProjectId { get; set; }
        public int? PhaseTaskId { get; set; }

        public decimal? Revenue { get; set; }
        public decimal? Expenses { get; set; }
        public decimal? Projectpercentage { get; set; }
        public decimal? Taskpercentage { get; set; }
        public string? date { get; set; }
        public int? Time { get; set; }

        public virtual Project? Project { get; set; }
        public virtual ProjectPhasesTasks? ProjectPhasesTasks { get; set; }

    }
}

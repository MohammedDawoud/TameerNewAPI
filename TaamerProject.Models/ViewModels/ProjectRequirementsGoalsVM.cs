using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{

    public class ProjectRequirementsGoalsVM
    {
        public int RequirementGoalId { get; set; }
        public int? ProjectId { get; set; }
        public int? RequirementId { get; set; }
        public string? RequirmentName { get; set; }
        public string? timestr { get; set; }

        public int? LineNo { get; set; }

    }
}

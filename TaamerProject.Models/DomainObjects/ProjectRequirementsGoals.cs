

namespace TaamerProject.Models
{
    public class ProjectRequirementsGoals : Auditable
    {

        public int RequirementGoalId { get; set; }
        public int? ProjectId { get; set; }
        public int? RequirementId { get; set; }
        public virtual Project? Project { get; set; }
        public virtual RequirementsandGoals? RequirementsandGoals { get; set; }


    }
}

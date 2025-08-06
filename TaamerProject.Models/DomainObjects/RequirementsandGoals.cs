

namespace TaamerProject.Models
{
    public class RequirementsandGoals : Auditable
    {

        public int RequirementId { get; set; }

        public string? RequirmentName { get; set; }
        public int? EmployeeId { get; set; }
        public int? DepartmentId { get; set; }
        public int? ProjectTypeId { get; set; }
        public int? ProjectId { get; set; }
        public int? LineNumber { get; set; }

        public string? TimeNo { get; set; }
        public string? TimeType { get; set; }
        public virtual ProjectType? ProjectType { get; set; }
        public virtual Project? Project { get; set; }
        public virtual Employees? Employees { get; set; }
        public virtual Department? Department { get; set; }




    }
}

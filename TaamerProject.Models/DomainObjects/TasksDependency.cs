using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class TasksDependency : Auditable
    {
        public int DependencyId { get; set; }
        public int? PredecessorId { get; set; }
        public int? SuccessorId { get; set; }
        public int? ProjSubTypeId { get; set; }
        public int? Type { get; set; }
        public int? ProjectId { get; set; }
        public int? BranchId { get; set; }
        public virtual ProjectPhasesTasks? SettingsPredecessor { get; set; }
        public virtual ProjectPhasesTasks? SettingsSuccessor { get; set; }
    }
}

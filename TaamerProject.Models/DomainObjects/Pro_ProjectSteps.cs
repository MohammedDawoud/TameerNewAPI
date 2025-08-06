using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Pro_ProjectSteps : Auditable
    {
        public int ProjectStepId { get; set; }
        public int? ProjectId { get; set; }
        public int? StepId { get; set; }
        public int? StepDetailId { get; set; }
        public bool? Status { get; set; }
        public int? UserId { get; set; }
        public int? BranchId { get; set; }
        public string? Date { get; set; }
        public string? Notes { get; set; }
        public virtual Users? Users { get; set; }
        public virtual Pro_StepDetails? StepDetails { get; set; }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Pro_ProjectStepsVM
    {
        public int ProjectStepId { get; set; }
        public int? ProjectId { get; set; }
        public int? StepId { get; set; }
        public string? StepName { get; set; }
        public int? StepDetailId { get; set; }
        public string? StepDetailNameAr { get; set; }
        public string? StepDetailNameEn { get; set; }
        public bool? Status { get; set; }
        public string? StatusName { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public int? BranchId { get; set; }
        public string? Date { get; set; }
        public string? Notes { get; set; }
    }
}

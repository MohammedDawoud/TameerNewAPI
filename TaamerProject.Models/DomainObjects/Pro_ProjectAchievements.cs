using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Pro_ProjectAchievements : Auditable
    {
        public int ProjectAchievementId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int? ProjectId { get; set; }
        public int? StepId { get; set; }
        public int? LineNumber { get; set; }
        public int? UserId { get; set; }
        public int? BranchId { get; set; }

    }
}

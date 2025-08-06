using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class TemplatesTasks : Auditable
    {
        public int TemplateTaskId { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public int? ParentId { get; set; }
        public int? TemplateId { get; set; }
        public int? ProjSubTypeId { get; set; }
        public int? Type { get; set; }
        public int? UserId { get; set; }
        public int? ProjectId { get; set; }
        public int? TimeMinutes { get; set; }
        public int? TimeType { get; set; }
        public int? Remaining { get; set; }
        public bool? IsUrgent { get; set; }
        public bool? IsTemp { get; set; }
        public int? TaskType { get; set; }
        public int? Status { get; set; }
        public int? OldStatus { get; set; }
        public bool? Active { get; set; }
        public int? StopCount { get; set; }
        public int? OrderNo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? PercentComplete { get; set; }
        public decimal? Cost { get; set; }
        public int? ToUserId { get; set; }
        public string? Notes { get; set; }
        public int? BranchId { get; set; }
        public long? LocationId { get; set; }
        public virtual Project? Project { get; set; }
        public int? SettingId { get; set; }
        public int? ParentSettingId { get; set; }
        public ProjectSubTypes? ProjectSubTypes { get; set; }
        //public ProjectPhasesTasks? MainPhase { get; set; }
        public ProjectPhasesTasks? SubPhase { get; set; }
        public Users? Users { get; set; }
        public NodeLocations? NodeLocations { get; set; }
        public Settings? Settings { get; set; }
    }
}

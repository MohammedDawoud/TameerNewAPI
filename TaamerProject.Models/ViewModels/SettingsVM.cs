using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class SettingsVM
    {
        public int SettingId { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public int? ParentId { get; set; }
        public int? ProjSubTypeId { get; set; }
        public int? Type { get; set; }
        public int? TimeMinutes { get; set; }
        public bool? IsUrgent { get; set; }
        public bool? IsTemp { get; set; }
        public int? TaskType { get; set; }
        public int? OrderNo { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public decimal? PercentComplete { get; set; }
        public decimal? Cost { get; set; }
        public int? BranchId { get; set; }
        public int? UserId { get; set; }
        public int? TimeType { get; set; }
        public string? TaskTypeName { get; set; }
        public string? TimeTypeName { get; set; }
        public string? UserName { get; set; }
        public string? TimeStr { get; set; }
        public string? NodeLocation { get; set; }
        public string? Notes { get; set; }
        public string?  TaskFullDescription { get; set; }
        public int? Priority { get; set; }
        public int? ExecPercentage { get; set; }
        public int? TaskOn { get; set; }
        public int? MainPhaseId { get; set; }
        public int? SubPhaseId { get; set; }
        public int? IsMerig { get; set; }
        public string? EndTime { get; set; }
        public bool? IsUserDeleted { get; set; }
        public int? VacationCount { get; set; }

        public int? RequirmentId { get; set; }
    }
}

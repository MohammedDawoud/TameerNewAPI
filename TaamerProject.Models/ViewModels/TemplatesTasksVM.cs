using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class TemplatesTasksVM
    {
        public int TemplateTaskId { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public int? ParentId { get; set; }
        public int? TemplateId { get; set; }
        public int? ProjSubTypeId { get; set; }
        public int? Type { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
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
        public string? MainPhaseName { get; set; }
        public string? SubPhaseName { get; set; }
        public string? ProjectTypeName { get; set; }
        public string? ProjectSubTypeName { get; set; }
        public string? ClientName { get; set; }
        public string? ProjectNumber { get; set; }
        public string? Project { get; set; }
        public string? ProjectMangerName { get; set; }
        public string? TaskTypeName { get; set; }
        public string? StatusName { get; set; }
        public string? TimeStr { get; set; }
        public string? TaskStart { get; set; }
        public string? TaskEnd { get; set; }
        public string? TimeTypeName { get; set; }
        public string? ProTypeName { get; set; }
        public string? NodeLocation { get; set; }
        public int? PlayingTime { get; set; }
    }
}

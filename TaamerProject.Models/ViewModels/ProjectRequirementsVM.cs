using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ProjectRequirementsVM
    {
        public int RequirementId { get; set; }
        public int? ProjectTypeId { get; set; }
        public int? ProjectSubTypeId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int? BranchId { get; set; }
        public int? UserId { get; set; }
        public decimal? Cost { get; set; }
        public string? AttachmentUrl { get; set; }
        public int? PhasesTaskID { get; set; }
        public int? OrderId { get; set; }
        public int? NotifactionId { get; set; }
        public string? ProjectSubTypeName { get; set; }
        public string? ProjectTypesName { get; set; }
        public int? PageInsert { get; set; }
        public string? PageInsertName { get; set; }
        public string? UserFullName { get; set; }
        public string? AddDate { get; set; }


    }
}

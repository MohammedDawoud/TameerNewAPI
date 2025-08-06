using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class RequirementsVM
    {
        public int RequirementId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int? BranchId { get; set; }
        public string? AttachmentUrl { get; set; }
        public int? ProjectId { get; set; }
        public bool? ConfirmStatus { get; set; }

        public string? ConfirmStatusDate { get; set; }
        public decimal? Cost { get; set; }
        public string? ConfirmStatustxt { get; set; }
        public string? AddUserName { get; set; }


    }
}

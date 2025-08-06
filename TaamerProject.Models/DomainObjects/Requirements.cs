using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Requirements : Auditable
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


        public virtual Project? Project { get; set; }
        public virtual Users? UpdateUsers { get; set; }


    }
}
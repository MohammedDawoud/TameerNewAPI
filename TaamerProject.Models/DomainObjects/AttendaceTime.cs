using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class AttendaceTime : Auditable
    {
        public int TimeId { get; set; }
        public string? Code { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string?  Notes { get; set; }
        public int? UserId { get; set; }
        public int? BranchId { get; set; }
        public virtual List <AttTimeDetails>? AttTimeDetails { get; set; }
    }
}
   


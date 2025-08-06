using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Groups : Auditable
    {
        public int GroupId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Notes { get; set; }
        public int? BranchId { get; set; }
      
    }
}

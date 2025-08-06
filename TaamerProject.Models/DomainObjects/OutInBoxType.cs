using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class OutInBoxType : Auditable
    {
        public int TypeId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int BranchId { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ModelType : Auditable
    {
        public int ModelTypeId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int BranchId { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Department : Auditable
    {
        public int DepartmentId { get; set; }
        public string? DepartmentNameAr { get; set; }
        public string? DepartmentNameEn { get; set; }
        public int? Type { get; set; }
        public int BranchId { get; set; }
    }
}

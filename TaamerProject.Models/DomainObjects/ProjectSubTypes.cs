using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ProjectSubTypes : Auditable
    {
        public int SubTypeId { get; set; }
        public int ProjectTypeId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int? BranchId { get; set; }
        public string? TimePeriod { get; set; }
        public ProjectType? ProjectType { get; set; }
    }
}

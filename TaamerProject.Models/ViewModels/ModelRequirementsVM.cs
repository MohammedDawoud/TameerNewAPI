using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class     ModelRequirementsVM
    {
        public int ModelReqId { get; set; }
        public int? RequirementId { get; set; }
        public int? ModelId { get; set; }
        public int? BranchId { get; set; }
        public string? RequirementName { get; set; }
    }
}

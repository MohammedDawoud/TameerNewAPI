using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ModelRequirements : Auditable
    {
        public int ModelReqId { get; set; }
        public int? RequirementId { get; set; }
        public int? ModelId { get; set; }
        public int? BranchId { get; set; }

        public virtual Requirements? Requirements  { get; set; }
    }
}
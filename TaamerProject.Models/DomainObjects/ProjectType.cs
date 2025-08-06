using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TaamerProject.Models
{
    public class ProjectType : Auditable
    {
        public int TypeId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int? Typeum { get; set; }
        public int? TypeCode { get; set; }

        public virtual List <RequirementsandGoals>? RequirementsandGoals { get; set; }


    }
}

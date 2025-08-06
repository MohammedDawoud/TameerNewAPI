using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Pro_StepDetails : Auditable
    {
        public int StepDetailId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int? StepId { get; set; }
        public string? StepName { get; set; }
    }
}

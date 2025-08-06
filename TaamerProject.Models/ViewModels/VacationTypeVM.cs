using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class VacationTypeVM
    {
        public int VacationTypeId { get; set; }
        public string? Code { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Notes { get; set; }
    }
}

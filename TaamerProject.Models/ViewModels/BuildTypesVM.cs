using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class BuildTypesVM
    {
        public int BuildTypeId { get; set; }
        public string NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Description { get; set; }
    }
}

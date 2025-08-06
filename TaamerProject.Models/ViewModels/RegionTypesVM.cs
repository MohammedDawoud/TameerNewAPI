using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class RegionTypesVM
    {
        public int RegionTypeId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
    }
}

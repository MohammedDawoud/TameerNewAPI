using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class CityPassVM
    {
        public int CityId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Notes { get; set; }
    }
}

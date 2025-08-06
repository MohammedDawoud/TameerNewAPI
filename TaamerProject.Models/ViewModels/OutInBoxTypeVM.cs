using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class OutInBoxTypeVM
    {
        public int TypeId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
    }
}

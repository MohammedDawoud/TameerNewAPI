using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Nationality : Auditable
    {
        public int NationalityId { get; set; }
        public string? NationalityCode { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Notes { get; set; }
      
    }
}

using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class AttDeviceSitting : Auditable
    {
        public int AttDeviceSittingId { get; set; }
        public string? ArgCompanyCode { get; set; }
        public string? ArgEmpUsername { get; set; }
        
        public string? ArgEmpPassowrd { get; set; }
        public string? ArgDeviceName { get; set; }



    }
}

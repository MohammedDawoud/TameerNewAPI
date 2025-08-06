using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Job : Auditable
    {
        public int JobId { get; set; }
        public string? JobCode { get; set; }
        public string? JobNameAr { get; set; }
        public string? JobNameEn { get; set; }
        public string? Notes { get; set; }
    }
}

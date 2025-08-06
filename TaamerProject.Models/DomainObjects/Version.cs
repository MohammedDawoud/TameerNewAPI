using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Versions : Auditable
    {
        public int VersionId { get; set; }
        public string? VersionCode { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
    }
}

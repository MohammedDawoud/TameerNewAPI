using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class DraftVM
    {
        public int DraftId { get; set; }
        public string? DraftName { get; set; }
        public string? DraftUrl { get; set; }
        public int? ProjectTypeId { get; set; }
        public string? ProjectTypeName { get; set; }
    }
}

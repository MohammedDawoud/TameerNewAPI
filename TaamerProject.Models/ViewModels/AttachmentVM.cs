using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class AttachmentVM
    {
        public int AttachmentId { get; set; }
        public string? AttachmentName { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public string? AttachmentUrl { get; set; }
        public string? Notes { get; set; }
        public int? EmployeeId { get; set; }
    }
}

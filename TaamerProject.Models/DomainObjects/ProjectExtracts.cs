using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ProjectExtracts : Auditable
    {
        public int ExtractId { get; set; }
        public string? ExtractNo { get; set; }
        public string? Type { get; set; }
        public decimal? Value { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? Status { get; set; }
        public int? ProjectId { get; set; }
        public string? ValueText { get; set; }
        public bool IsDoneBefore { get; set; }
        public bool IsDoneAfter { get; set; }
        public string? AttachmentUrl { get; set; }
        public string? SignatureUrl { get; set; }
    }
}

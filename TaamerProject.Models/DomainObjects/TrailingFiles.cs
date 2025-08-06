using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class TrailingFiles : Auditable
    {
        public int FileId { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public int? TypeId { get; set; }
        public int? ProjectId { get; set; }
        public int? TrailingId { get; set; }
        public string? Notes { get; set; }
        public int? BranchId { get; set; }
    }
}

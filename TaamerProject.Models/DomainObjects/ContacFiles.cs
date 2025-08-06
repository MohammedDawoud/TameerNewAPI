using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ContacFiles : Auditable
    {
        public int FileId { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public decimal FileSize { get; set; }
        public string? Extension { get; set; }
        public int? OutInBoxId { get; set; }
        public string? Notes { get; set; }
        public bool IsCertified { get; set; }
        public int? UserId { get; set; }
        public DateTime? UploadDate { get; set; }
        public string? DeleteUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? DeleteType { get; set; }
        //public FileType FileType { get; set; }
        public int? BranchId { get; set; }
        public OutInBox? OutInBox { get; set; }
        public virtual Users? Users { get; set; }

    }
}

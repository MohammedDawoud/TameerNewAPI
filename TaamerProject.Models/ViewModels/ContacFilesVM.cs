using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ContacFilesVM
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
        public int? BranchId { get; set; }
        public string? OutInBoxNumber { get; set; }
        public string? OutInBoxDate { get; set; }
        public string? ContacFiles { get; set; }
        public string? UserName { get; set; }
    }
}

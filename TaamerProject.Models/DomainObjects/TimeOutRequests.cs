using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class TimeOutRequests : Auditable
    {
        public int RequestId { get; set; }
        public string? Address { get; set; }
        public int? Duration { get; set; }
        public string? Reason { get; set; }
        public string? AttachmentUrl { get; set; }
        public int? UserId { get; set; }
        public int? ActionUserId { get; set; }
        public int? Status { get; set; }
        public int? TaskId { get; set; }
        public string? Comment { get; set; }
        public int BranchId { get; set; }
        public virtual Users? Users { get; set; }
    }
}

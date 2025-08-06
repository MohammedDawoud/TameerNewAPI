using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class NotificationVM
    {
        public long NotificationId { get; set; }
        public string? Name { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public int? SendUserId { get; set; }
        public int? ReceiveUserId { get; set; }
        public string? Description { get; set; }
        public bool? Done { get; set; }
        public bool? AllUsers { get; set; }
        public int? ActionUser { get; set; }
        public DateTime? ActionDate { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? SendDate { get; set; }
        public DateTime? ReadingDate { get; set; }
        public int? ProjectId { get; set; }
        public string? ProjectNo { get; set; }
        public string? AttachmentUrl { get; set; }
        public int? Type { get; set; }
        public List<Employees> Employees { get; set; }
        public string? EmpName { get; set; }
        public string? UserName { get; set; }
        public string? ReceivedUserName { get; set; }
        public string? DateDifference { get; set; }
        public string? SendUserName { get; set; }
        public string? SendUserImgUrl { get; set; }
        public string? ReceivedUserImgUrl { get; set; }
        public string? Title { get; set; }
        public int? taskId { get; set; }
        public bool? IsHidden { get; set; }
        public DateTime? NextTime { get; set; }
        public string? dayes { get; set; }
        public string? time { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaamerProject.Models
{
    public class Notification : Auditable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NotificationId { get; set; }
        public string? Name { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public int? SendUserId { get; set; }
        public int? DepartmentId { get; set; }
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
        public string? AttachmentUrl { get; set; }
        public int? Type { get; set; }
        public int? BranchId { get; set; }
        public int? TaskId { get; set; }
        [NotMapped]
        public List<int>? AssignedUsersIds { get; set; }
        public Employees? Employees { get; set; }
        public Users? Users { get; set; }
        public Users? ReceiveUsers { get; set; }
        public string? Title { get; set; }
        public bool? IsHidden { get; set; }
        public Project? Project { get; set; }

        public DateTime? NextTime { get; set; }
        //public string? dayes { get; set; }
        //public string? time { get; set; }
    }

    public class Notification2 : Auditable
    {
        public long NotificationId { get; set; }
        public string? Name { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public int? SendUserId { get; set; }
        public int? DepartmentId { get; set; }
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
        public string? AttachmentUrl { get; set; }
        public int? Type { get; set; }
        public int? BranchId { get; set; }
        public int? TaskId { get; set; }
        [NotMapped]
        public List<int>? AssignedUsersIds { get; set; }
        public string? Title { get; set; }
        public bool? IsHidden { get; set; }

        public DateTime? NextTime { get; set; }
        //public string? dayes { get; set; }
        //public string? time { get; set; }
    }
}

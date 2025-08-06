using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class UserMails : Auditable
    {
        public long MailId { get; set; }
        public int UserId { get; set; }
        public int SenderUserId { get; set; }
        public string? MailText { get; set; }
        public string? MailSubject { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public bool AllUsers { get; set; }
        public int BranchId { get; set; }
        public bool IsRead { get; set; }
        public Users? SendUsers { get; set; }
        public Users? Users { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class UserMailsVM
    {
        public long MailId { get; set; }
        public int? UserId { get; set; }
        public int? SenderUserId { get; set; }
        public string? MailText { get; set; }
        public string? MailSubject { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public bool AllUsers { get; set; }
        public int? BranchId { get; set; }
        public string? SendUserName { get; set; }
        public string? UserName { get; set; }
        public string? SendUserImgUrl { get; set; }
    }
}

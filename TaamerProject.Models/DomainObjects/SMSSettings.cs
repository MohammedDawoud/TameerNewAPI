using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class SMSSettings : Auditable
    {
        public int SettingId { get; set; }
        public string? MobileNo { get; set; }
        public string? Password { get; set; }
        public string? SenderName { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public string? ApiUrl { get; set; }
        public string? UserName { get; set; }
        public bool? SendCustomerSMS { get; set; }
    }
}

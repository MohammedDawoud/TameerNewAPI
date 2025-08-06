using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class EmailSettingVM
    {
        public int SettingId { get; set; }
        public string? SenderEmail { get; set; }
        public string? Password { get; set; }
        public string? SenderName { get; set; }
        public string? Host { get; set; }
        public string? Port { get; set; }
        public bool SSL { get; set; }
        public int? UserId { get; set; }
        public int? BranchId { get; set; }
        public string? DisplayName { get; set; }
    }
}

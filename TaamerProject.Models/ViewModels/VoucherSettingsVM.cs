using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class VoucherSettingsVM
    {
        public int SettingId { get; set; }
        public int? Type { get; set; }
        public int? AccountId { get; set; }
        public int? UserId { get; set; }
        public string? AccountName { get; set; }
        public string? TypeName { get; set; }
    }
}

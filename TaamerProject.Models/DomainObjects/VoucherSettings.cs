using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class VoucherSettings : Auditable
    {
        public int SettingId { get; set; }
        public int Type { get; set; }
        public int AccountId { get; set; }
        public int? UserId { get; set; }
        public int BranchId { get; set; }
        public Accounts? Accounts { get; set; }
    }
}

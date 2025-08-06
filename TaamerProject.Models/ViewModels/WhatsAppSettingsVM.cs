using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class WhatsAppSettingsVM
    {
        public int SettingId { get; set; }
        public string? MobileNo { get; set; }
        public string? Password { get; set; }
        public string? SenderName { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public string? ApiUrl { get; set; }
        public string? UserName { get; set; }
        public string? InstanceId { get; set; }
        public string? Token { get; set; }
        public string? TypeName { get; set; }
        public bool? Sendactivation { get; set; }
        public bool? SendactivationOffer { get; set; }
        public bool? SendactivationProject { get; set; }
        public bool? SendactivationSupervision { get; set; }


    }
}

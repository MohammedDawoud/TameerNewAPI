using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class ProSettingDetailsVM
    {
        public int ProSettingId { get; set; }
        public string? ProSettingNo { get; set; }
        public string? ProSettingNote { get; set; }
        public int? ProjectTypeId { get; set; }
        public int? ProjectSubtypeId { get; set; }
        public string? ProjectTypeName { get; set; }
        public string? ProjectSubTypeName { get; set; }
        public string?  UserName { get; set; }
        public DateTime? AddDate { get; set; }
        public string? ExpectedTime { get; set; }

        
       public List<SettingsVM> settingslst { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class NotificationSettingsVM
    {
       
            public int SettingId { get; set; }
            public int? IDEndCount { get; set; }
            public int? PassportCount { get; set; }
            public int? LicesnseCount { get; set; }
            public int? ContractCount { get; set; }
            public int? MedicalCount { get; set; }
            public int? VacancyCount { get; set; }
            public int? LoanCount { get; set; }
            public int? BranchId { get; set; }
     }
}

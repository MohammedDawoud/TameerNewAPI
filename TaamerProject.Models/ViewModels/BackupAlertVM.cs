using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
   public class BackupAlertVM
    {

       
        public int AlertId { get; set; }
        public int? UserId { get; set; }
        public int? AlertSms { get; set; }
        public int? AlertTimeType { get; set; }
        public int? Alert_IsSent { get; set; }
        public string? AlertNextTime { get; set; }
        public DateTime? AlertTime { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
    }
}

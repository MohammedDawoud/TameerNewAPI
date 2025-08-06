using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
   public class BackupAlert : Auditable
    {
        public int AlertId { get; set; }
        public int? UserId { get; set; }
        public int? AlertSms { get; set; }
        public int? AlertTimeType { get; set; }
        public int? Alert_IsSent { get; set; }
        public string? AlertNextTime { get; set; }
        public DateTime? AlertTime { get; set; }
        public Users? Users { get; set; }

    }
}

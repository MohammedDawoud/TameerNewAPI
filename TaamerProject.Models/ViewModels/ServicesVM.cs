using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class ServicesVM
    {
        public int ServiceId { get; set; }
        public string? Number { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public string? ExpireDate { get; set; }
        public string? ExpireHijriDate { get; set; }
        public int? UserId { get; set; }
        public string? Notes { get; set; }
        public int? DepartmentId { get; set; }
        public int? NotifyCount { get; set; }
        public int? AccountId { get; set; }

        public int? BankId { get; set; }
        public string? DepartmentName { get; set; }

        public string? BanksName { get; set; }
        public string? AccountName { get; set; }
        public bool RepeatAlarm { get; set; }
        public int? RecurrenceRateId { get; set; }
        public string? AttachmentUrl { get; set; }


    }
}

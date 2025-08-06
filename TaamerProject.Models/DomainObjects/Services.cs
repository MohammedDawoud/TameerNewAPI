using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaamerProject.Models
{
    public class Service : Auditable
    {
        public readonly string? NameAr;

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
        public int BranchId { get; set; }
        public bool RepeatAlarm { get; set; }
        public int? RecurrenceRateId { get; set; }
        public string? AttachmentUrl { get; set; }



        public Department? Department { get; set; }
        public Banks? Banks { get; set; }
        public Accounts? Account { get; set; }
        public virtual List<Contracts>? Contracts { get; set; }

    }
}

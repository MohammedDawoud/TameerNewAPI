using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class OfficialDocumentsVM
    {
        public int DocumentId { get; set; }
        public string? Number { get; set; }
        public string? OfficialDocumentsName { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public string? ExpiredDate { get; set; }
        public string? ExpiredHijriDate { get; set; }
        public int? UserId { get; set; }
        public string? Notes { get; set; }
        public string? AttachmentUrl { get; set; }
        public int? DepartmentId { get; set; }
        public int? NotifyCount { get; set; }
        public int? BranchId { get; set; }
        public string? DepartmentName { get; set; }
        public bool RepeatAlarm { get; set; }
        public int? RecurrenceRateId { get; set; }

    }
}

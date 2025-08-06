using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class OfficialDocuments : Auditable
    {
        public int DocumentId { get; set; }
        public string? Number { get; set; }
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
        public int ? BranchId { get; set; }
        public bool RepeatAlarm { get; set; }
        public int? RecurrenceRateId { get; set; }
        public Department? Department { get; set; }
    }
}

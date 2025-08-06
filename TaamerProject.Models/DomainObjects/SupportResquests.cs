using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class SupportResquests : Auditable
    {
        public int RequestId { get; set; }
        public int Type { get; set; }
        public string? Address { get; set; }
        public string? Topic { get; set; }
        public DateTime? Date { get; set; }
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
        public string? AttachmentUrl { get; set; }
        public int BranchId { get; set; }
        public string? Department { get; set; }
        public string? priority { get; set; }
        public int? Status { get; set; }
        public string? Repaly { get; set; }
        public string? CustomerULR { get; set; }
        public string? LastReplayDate { get; set; }
        public string? TicketNo { get; set; }
        public string? LastReplayFrom { get; set; }
    }
}

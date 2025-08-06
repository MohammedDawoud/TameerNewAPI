using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ChattingLog : Auditable
    {
        public int LogId { get; set; }
        public int? UserId { get; set; }
        public string? Body { get; set; }
        public int? ReceivedUserId { get; set; }
        public string? Status { get; set; }
        public DateTime? Date { get; set; }
        public string? HijriDate { get; set; }
        public virtual Users? SenderUser { get; set; }
        public virtual Users? ReceiveUsers { get; set; }
    }
}

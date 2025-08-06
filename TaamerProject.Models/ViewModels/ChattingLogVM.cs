using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class ChattingLogVM
    {
        public int LogId { get; set; }
        public int? UserId { get; set; }
        public string? Body { get; set; }
        public int? ReceivedUserId { get; set; }
        public string? Status { get; set; }
        public DateTime? Date { get; set; }
        public string? HijriDate { get; set; }
        public string? SenderUserName { get; set; }
        public string? ReceiveUserName { get; set; }
    }
}

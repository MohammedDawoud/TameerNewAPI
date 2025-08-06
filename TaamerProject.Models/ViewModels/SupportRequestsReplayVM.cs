using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.ViewModels
{
    public class SupportRequestsReplayVM
    {
        public int SupportRequestsReplayId { get; set; }
        public int? UserId { get; set; }
        public int? CusomerId { get; set; }
        public string? ContactDate { get; set; }
        public string? Contacttxt { get; set; }
        public string? SenderPhoto { get; set; }
        public int? ServiceRequestId { get; set; }
        public string? SenderName { get; set; }
        public bool? IsRead { get; set; }
        public string? ReplayFrom { get; set; }
        public string? AttachmentUrl { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class CustomerSMSVM
    {
        public int SMSId { get; set; }
        public int? CustomerId { get; set; }
        public int? SenderUser { get; set; }
        public string? SMSText { get; set; }
        public string? SMSSubject { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public bool? AllCustomers { get; set; }
        public int? UserId { get; set; }
        public int? BranchId { get; set; }
        public string? SenderUserName { get; set; }


    }
}

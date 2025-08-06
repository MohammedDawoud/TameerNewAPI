

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaamerProject.Models
{
    public class CustomerSMS : Auditable
    {
        public int SMSId { get; set; }
        public int? CustomerId { get; set; }
        public int? SenderUser { get; set; }
        public string? SMSText { get; set; }
        public string? SMSSubject { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public bool AllCustomers { get; set; }
        public int? BranchId { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual Users? Users { get; set; }
        [NotMapped]
        public List<int>? AssignedCustomersSMSIds { get; set; }
    }
}

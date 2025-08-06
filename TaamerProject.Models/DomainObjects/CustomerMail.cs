using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaamerProject.Models
{
    public class CustomerMail : Auditable
    {
        public int MailId { get; set; }
        public int? CustomerId { get; set; }
        public int? SenderUser { get; set; }
        public string? MailText { get; set; }
        public string? MailSubject { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public string? FileUrl { get; set; }
        public bool AllCustomers { get; set; }
        public int? BranchId { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Users? Users { get; set; }
        [NotMapped]
        public List<int>? AssignedCustomersIds { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class CustomerMailVM
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
        public string? SenderUserName { get; set; }
        public string? CustomerEmail { get; set; }
        public List<int> AssignedCustomersIds { get; set; }


    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaamerProject.Models
{
    public class Guarantees : Auditable
    {
        public int GuaranteeId { get; set; }
        public string? Number { get; set; }
        public string? BankName { get; set; }
        public decimal Value { get; set; }
        public string? ProjectName { get; set; }
        public int Type { get; set; }
        public decimal Percentage { get; set; }
        public string? CustomerName { get; set; }
        public int GuarantorAccId { get; set; }
        public int? ProjectId { get; set; }
        public int? CustomerId { get; set; }
        public int Period { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsReturned { get; set; }
        public string? ReturnReason { get; set; }
        public int? InvoiceId { get; set; }
        public int BranchId { get; set; }
        public virtual Accounts? Accounts { get; set; }
        [NotMapped]
        public string? StartDateStr { get; set; }
    }
}

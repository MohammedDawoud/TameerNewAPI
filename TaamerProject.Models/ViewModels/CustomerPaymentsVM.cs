using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class CustomerPaymentsVM
    {
        public int PaymentId { get; set; }
        public int? PaymentNo { get; set; }
        public int? ContractId { get; set; }
        public string? PaymentDate { get; set; }
        public string? PaymentDateHijri { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int? TransactionId { get; set; }
        public int? InvoiceId { get; set; }
        public int? ToAccountId { get; set; }
        public bool IsPaid { get; set; }
        public int? BranchId { get; set; }
        public string? AccountName { get; set; }
        public string? PaymentDatestring { get; set; }
        public string? AmountValueText { get; set; }
        public int? OfferId { get; set; }
        public int? Isconst { get; set; }
        public int? ServiceId { get; set; }
        public string? AmountValueText_EN { get; set; }
        public decimal? AmountPercentage { get; set; }
        public bool? IsCanceled { get; set; }
        public string? InvoiceNumber { get; set; }




    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaamerProject.Models
{
    public class CustomerPayments : Auditable
    {
        public int PaymentId { get; set; }
        public int? PaymentNo { get; set; }
        public int? ContractId { get; set; }
        public string? PaymentDate { get; set; }
        public string? PaymentDateHijri { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? AmountValueText { get; set; }
        public int? TransactionId { get; set; }
        public int? InvoiceId { get; set; }
        public int? ToAccountId { get; set; }
        public bool IsPaid { get; set; }
        public int? BranchId { get; set; }
        public int? OfferId { get; set; }
        public int? Isconst { get; set; }
        public int? ServiceId { get; set; }
        public string? AmountValueText_EN { get; set; }
        public decimal? AmountPercentage { get; set; }
        public bool? IsCanceled { get; set; }

        public virtual OffersPrices? OffersPrices { get; set; }
        public virtual Contracts? Contracts { get; set; }
        public virtual Accounts? Accounts { get; set; }
        public virtual Invoices? Invoices { get; set; }


        [NotMapped]
        public string? PaymentDatestring { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Transactions : Auditable
    {
        public long TransactionId { get; set; }
        public int? LineNumber { get; set; }
        public int? AccountId { get; set; }
        public int? InvoiceId { get; set; }
        public int? Type { get; set; }
        public string? TransactionDate { get; set; }
        public string? TransactionHijriDate { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Depit { get; set; }
        public int? OrderNumber { get; set; }
        public string? Details { get; set; }
        public string? Notes { get; set; }
        public int? BranchId { get; set; }
        public bool? IsPost { get; set; }
        public int? YearId { get; set; }
        public string? InvoiceReference { get; set; }
        public double? CurrentBalance { get; set; }
        public int? CostCenterId { get; set; }
        public int? ContractId { get; set; }
        public int? PaymentId { get; set; }
        public int? CustomerId { get; set; }
        public int? AccountType { get; set; }
        public int? JournalNo { get; set; }
        public int? DiscountRewardId { get; set; }
        public string? RecycleYearTo { get; set; }
        public bool? RecycleStatus { get; set; }
        public bool? AccCalcExpen { get; set; }
        public bool? AccCalcIncome { get; set; }

        public string? AccTransactionDate { get; set; }
        public string? AccTransactionHijriDate { get; set; }
        public int? VoucherDetailsId { get; set; }
        public decimal? Amounttax { get; set; }

        public virtual Invoices? Invoices { get; set; }
        public virtual Accounts? Accounts { get; set; }
        public virtual CostCenters? CostCenters { get; set; }
        public virtual AccTransactionTypes? AccTransactionTypes { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual Contracts? Contracts { get; set; }
        public virtual DiscountReward? DiscountReward { get; set; }




    }
}

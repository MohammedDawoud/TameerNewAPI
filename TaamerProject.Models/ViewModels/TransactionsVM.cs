using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class TransactionsVM
    {
        public long TransactionId { get; set; }
        public int? LineNumber { get; set; }
        public int? AccountId { get; set; }
        public int? InvoiceId { get; set; }
        public int? AccountType { get; set; }
        public int? Type { get; set; }
        public string? TransactionDate { get; set; }
        public string? TransactionHijriDate { get; set; }
        //[DisplayFormat(DataFormatstring? = "{0:###.####}")]
        public decimal? Credit { get; set; }
        //[DisplayFormat(DataFormatstring? = "{0:###.####}")]
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
        public string? AccountName { get; set; }
        public string? AccountCode { get; set; }
        public string? CostCenterName { get; set; }
        public string? DepitOrCreditName { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Balance { get; set; }
        public string? TypeName { get; set; }
        public string? DateFrom { get; set; }
        public string? DateTo { get; set; }
        public decimal? DebitBalance { get; set; }
        public decimal? CreditBalance { get; set; }
        public string? CustomerName { get; set; }
        public int? JournalNo { get; set; }
        public string? InvoiceNumber { get; set; }
        public int? DiscountRewardId { get; set; }
        public string? RecycleYearTo { get; set; }
        public bool? RecycleStatus { get; set; }
        public string? InvoiceDate { get; set; }
        public bool? AccCalcExpen { get; set; }
        public bool? AccCalcIncome { get; set; }

        public string? AccTransactionDate { get; set; }
        public string? AccTransactionHijriDate { get; set; }
        public int? VoucherDetailsId { get; set; }
        public short? PayType { get; set; }
        public bool? Rad { get; set; }
        public Project? project { get; set; }
        public string? pieceNo { get; set; }

        public string? TotalRes { get; set; }
        public decimal? Amounttax { get; set; }
        public int? Classification { get; set; }
        public string? AccCalcAll { get; set; }

    }
}

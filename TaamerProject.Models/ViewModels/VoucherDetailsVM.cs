using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class VoucherDetailsVM
    {
        public int VoucherDetailsId { get; set; }
        public int? InvoiceId { get; set; }
        public int? LineNumber { get; set; }
        public int? AccountId { get; set; }
        public int? TaxType { get; set; }
        public decimal? Amount { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? ToAccountId { get; set; }
        public string? ReferenceNumber { get; set; }
        public int? CostCenterId { get; set; }
        public int? PayType { get; set; }
        public string? Description { get; set; }
        public string? AccountName { get; set; }
        public string? ToAccountName { get; set; }
        public string? CostCenterName { get; set; }
        public string? PayTypeName { get; set; }
        public string? TaxTypeName { get; set; }
        public string? ToAccountCode { get; set; }
        public string? CheckNo { get; set; }
        public string? CheckDate { get; set; }
        public int? MoneyOrderNo { get; set; }
        public string? MoneyOrderDate { get; set; }
        public string? BankName { get; set; }
        public string? TFK { get; set; }
        public decimal? Qty { get; set; }
        public int? BankId { get; set; }
        public int? ServicesPriceId { get; set; }
        public int? CategoryId { get; set; }

        public string? ServicesPriceName { get; set; }
        public string? ServiceTypeName { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryTypeName { get; set; }

        public int? IsRetrieve { get; set; }

        public string? InvoiceNumber { get; set; }
        public string? QRCode { get; set; }
        public decimal? DiscountPercentage_Det { get; set; }
        public decimal? DiscountValue_Det { get; set; }
        public List<AccServicesPricesOfferVM>? ServicesPricesOffer { get; set; }

        //public string? Reference { get; set; }
        //public decimal? PaidValue { get; set; }

    }
}

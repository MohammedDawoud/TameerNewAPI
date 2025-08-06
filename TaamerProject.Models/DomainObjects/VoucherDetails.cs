
namespace TaamerProject.Models
{
    public class VoucherDetails : Auditable
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
        public string? TFK { get; set; }
        public decimal? Qty { get; set; }
        public string? CheckNo { get; set; }
        public string? CheckDate { get; set; }
        public int? MoneyOrderNo { get; set; }
        public string? MoneyOrderDate { get; set; }
      //  public string? BankName { get; set; }
        public int? BankId { get; set; }
        public int? ServicesPriceId { get; set; }
        public int? CategoryId { get; set; }
        public int? IsRetrieve { get; set; }

        public decimal? DiscountPercentage_Det { get; set; }
        public decimal? DiscountValue_Det { get; set; }


        public Banks? Banks { get; set; }
        public virtual Acc_Services_Price? ServicesPrice { get; set; }

        public virtual Acc_Categories? Categories { get; set; }

        public virtual Invoices? Invoices { get; set; }
        public virtual Accounts? Accounts { get; set; }
        public virtual Accounts? ToAccounts { get; set; }
        public virtual CostCenters? CostCenters { get; set; }

    }
}

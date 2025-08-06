using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaamerProject.Models
{
    public class Invoices : Auditable
    {
        public int InvoiceId { get; set; }
        public string? InvoiceNumber { get; set; }
        public int Type { get; set; }
        public int? CustomerId { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? ExchangeRate { get; set; }
        public short? OrderId { get; set; }
        public bool? Rad { get; set; }
        public int? DelegateId { get; set; }
        public string? Notes { get; set; }
        public short? StoreId { get; set; }
        public int? JournalNumber { get; set; }
        public bool? IsPurchaseReturn { get; set; }
        public decimal? InvoiceValue { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? TotalValue { get; set; }
        public string? InvoiceValueText { get; set; }
        public decimal? Paid { get; set; }
        public decimal? PaidRequired { get; set; }
        public decimal? TotalExpenses { get; set; }
        public int? BranchId { get; set; }
        public bool? IsPost { get; set; }
        public string? PostDate { get; set; }
        public string? PostHijriDate { get; set; }
        public int? YearId { get; set; }
        public short? CurrentOp { get; set; }
        public int? UserId { get; set; }
        public string? ReceiverName { get; set; }
        public string? InvoiceReference { get; set; }

        public int? ProjectId { get; set; }
        public int? ToAccountId { get; set; }
        public short? VoucherType { get; set; }
        public string? ToInvoiceId { get; set; }
        public bool? IsTax { get; set; }
        public decimal? TaxAmount { get; set; }
        public short? PayType { get; set; }
        public short? NumberOfDepit { get; set; }
        public short? NumberOfCredit { get; set; }
        public bool? printBankAccount { get; set; }
        public decimal? PaidValue { get; set; }
        public string? SupplierInvoiceNo { get; set; }
        public string? RecevierTxt { get; set; }

        public int? ClauseId { get; set; }
        public int? SupplierId { get; set; }

        public int? CostCenterId { get; set; }
        public short? PageInsert { get; set; }

        public string? QRCodeNum { get; set; }
        public string? InvoiceNotes { get; set; }
        public string? RecycleYearTo { get; set; }
        public bool? RecycleStatus { get; set; }
        public string? InvoiceRetId { get; set; }
        public bool? DunCalc { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }


        public string? VoucherAlarmDate { get; set; }
        public bool? VoucherAlarmCheck { get; set; }
        public short? IsSendAlarm { get; set; }
        public int? MovementId { get; set; }

        public int? CreditNotiId { get; set; }
        public int? DepitNotiId { get; set; }
        public string? InvUUID { get; set; }
        public bool? VoucherAdjustment { get; set; }
        public string? PurchaseOrderNo { get; set; }


        //public string? InvoiceHash { get; set; }
        //public string? SingedXML { get; set; }
        //public string? EncodedInvoice { get; set; }
        //public string? ZatcaUUID { get; set; }
        //public string? QRCode { get; set; }
        //public string? PIH { get; set; }
        //public string? SingedXMLFileName { get; set; }
        //public long? InvoiceNoRequest { get; set; }

        public virtual Accounts? Accounts { get; set; }


        public virtual Users? AddUsers { get; set; }
        public virtual Employees? Delegate { get; set; }

        public virtual List<VoucherDetails>? VoucherDetails { get; set; }
        public List<Acc_InvoicesRequests>? InvoicesRequests { get; set; }

        public virtual Invoices? Invoices_Credit { get; set; }
        public virtual Invoices? Invoices_Depit { get; set; }
        public virtual List<Transactions>? TransactionDetails { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual Acc_Suppliers? Suppliers { get; set; }

        public virtual Project? Project { get; set; }
        public virtual Branch? Branch { get; set; } 
        public virtual AccTransactionTypes? AccTransactionTypes { get; set; }
        //public virtual List<Invoices> InvoicesNoti { get; set; }
        //public virtual Invoices AccInvoice { get; set; }

        [NotMapped]
        public int? FromAccountId { get; set; }
        [NotMapped]
        public string? SupplierInvoiceNotxt { get; set; }

        [NotMapped]
        public virtual List<Acc_Services_PriceOffer>? ServicesPriceOffer { get; set; }
    }
}

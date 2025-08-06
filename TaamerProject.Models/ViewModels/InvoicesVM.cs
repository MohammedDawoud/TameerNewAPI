using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class InvoicesVM
    {
        public int InvoiceId { get; set; }
        public string? InvoiceNumber { get; set; }
        public int Type { get; set; }
        public int? CustomerId { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? ExchangeRate { get; set; }
        public short? OederId { get; set; }
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
        public decimal? Paid { get; set; }
        public decimal? PaidRequired { get; set; }
        public decimal? TotalExpenses { get; set; }
        public string? InvoiceValueText { get; set; }
        public decimal? TotalRevenue { get; set; }
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
        public string? ToAccountName { get; set; }
        public string? ToAccountCode { get; set; }
        public short? VoucherType { get; set; }
        public string? ToInvoiceId { get; set; }
        public bool? IsTax { get; set; }
        public decimal? TaxAmount { get; set; }
        public short? PayType { get; set; }
        public string? CustomerCode { get; set; }
        public string? StatusName { get; set; }
        public string? StatusNameNew { get; set; }

        public short? NumberOfDepit { get; set; }
        public short? NumberOfCredit { get; set; } 
        public string? TransactionTypeName { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerName_W { get; set; }

        public string? AccountCode { get; set; }
        public string? ProjectNo { get; set; }
        public decimal? RequiredAmount { get; set; }
        public string? ProjectName { get; set; }
        public string? projecttypeName { get; set; }
        public string? AddUser { get; set; }
        public string? CustomerType { get; set; }

        public bool? printBankAccount { get; set; }
        public DateTime? AddDate { get; set; }
        public string? Address{ get; set; }
        public decimal? PaidValue { get; set; }
        public string? PayTypeName { get; set; }

        public string? CustomerPhone { get; set; }

        public string? CustomerMobile { get; set; }


        public int? CustomerAccountId { get; set; }


        public string? SupplierInvoiceNo { get; set; }
        public string? RecevierTxt { get; set; }

        public int? ClauseId { get; set; }
        public int? SupplierId { get; set; }
        public int? CostCenterId { get; set; }
        public short? PageInsert { get; set; }

        public string? QRCodeNum { get; set; }
        public string? InvoiceNotes { get; set; }

        public string? DetailsDesc { get; set; }


        public string? RadName { get; set; }

        public string? AccountNameRet { get; set; }
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
        public decimal? CreditNotiTotal { get; set; }
        public decimal? DepitNotiTotal { get; set; }
        public string? InvUUID { get; set; }
        public bool? VoucherAdjustment { get; set; }
        public string? ContractNo { get; set; }
        public string? AddInvoiceImg { get; set; }


        //public string? InvoiceHash { get; set; }
        //public string? SingedXML { get; set; }
        //public string? EncodedInvoice { get; set; }
        //public string? ZatcaUUID { get; set; }
        //public string? QRCode { get; set; }
        //public string? PIH { get; set; }
        //public string? SingedXMLFileName { get; set; }

        public int? AppearUser { get; set; }


        public string? SupplierName { get; set; }
        public string? ClauseName { get; set; }
        //public long? InvoiceNoRequest { get; set; }
        public string? DelegateName { get; set; }
        public string? PurchaseOrderNo { get; set; }
        public string? PurchaseOrderStatus { get; set; }

        public List<VoucherDetailsVM> VoucherDetails { get; set; }
        public List<TransactionsVM> Transactions { get; set; }
        public int? InvoicesRequestsCount { get; set; }
        public List<Acc_InvoicesRequestsVM>? InvoicesRequests { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class BranchesVM
    {
        public int BranchId { get; set; }
        public string? Code { get; set; }
        public string? BranchName { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? BranchManager { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public int? WarehouseId { get; set; }
        public int? CurrencyId { get; set; }
        public int? BoxAccId { get; set; }
        public int? StockAccd { get; set; }
        public int? SaleCostAccId { get; set; }
        public int? SaleCashAccId { get; set; }
        public int? SaleDelayAccId { get; set; }
        public int? SaleDiscountAccId { get; set; }
        public int? SaleReturnCashAccId { get; set; }
        public int? SaleReturnDelayAccId { get; set; }
        public int? SaleReturnDiscountAccId { get; set; }
        public int? PurchaseCashAccId { get; set; }
        public int? PurchaseDelayAccId { get; set; }
        public int? PurchaseApprovalAccId { get; set; }
        public int? PurchaseOutCashAccId { get; set; }
        public int? PurchaseOutDelayAccId { get; set; }
        public int? PurchaseDiscAccId { get; set; }
        public int? PurchaseReturnCashAccId { get; set; }
        public int? PurchaseReturnDelayAccId { get; set; }
        public int? PurchaseReturnApprovAccId { get; set; }
        public int? PurchaseReturnDiscAccId { get; set; }
        public int? RevenuesAccountId { get; set; }
        public int? SuspendedFundAccId { get; set; }
        public int? CashInvoicesAccId { get; set; }
        public int? DelayInvoicesAccId { get; set; }
        public int? DiscountInvoicesAccId { get; set; }
        public int? CashReturnInvoicesAccId { get; set; }
        public int? DelayReturnInvoicesAccId { get; set; }
        public int? DiscountReturnInvoiceAccId { get; set; }
        public int? CheckInvoicesAccId { get; set; }
        public int? VisaInvoicesAccId { get; set; }
        public int? TeleInvoicesAccId { get; set; }
        public int? AmericanAccId { get; set; }
        public int? CustomersAccId { get; set; }
        public int? SuppliersAccId { get; set; }
        public int? EmployeesAccId { get; set; }
        public int? GuaranteeAccId { get; set; }
        public int? ContractsAccId { get; set; }
        public int? TaxsAccId { get; set; }
        public string? EngineeringLicense { get; set; }
        public string? LabLicense { get; set; }
        public string? Mailbox { get; set; }
        public int? CityId { get; set; }
        public int? LastExport { get; set; }
        public int? LastExportInner { get; set; }
        public string? CityName { get; set; }
        public bool? IsActive { get; set; }
        public string? CurrencyName { get; set; }
        public int? OrganizationId { get; set; }
        public int? LoanAccId { get; set; }
        public int? BoxAccId2 { get; set; }
        public string? AccountBank { get; set; }
        public string? PostalCodeFinal { get; set; }
        public string? ExternalPhone { get; set; }
        public string? Country { get; set; }
        public string? Neighborhood { get; set; }
        public string? StreetName { get; set; }
        public string? BuildingNumber { get; set; }
        public string? AccountBank2 { get; set; }
        public string? Address { get; set; }
        public string? TaxCode { get; set; }
        public string? PostalCode { get; set; }
        public string? ProjectStartCode { get; set; }
        public string? OfferStartCode { get; set; }
        public string? TaskStartCode { get; set; }
        public string? OrderStartCode { get; set; }
        public string? InvoiceStartCode { get; set; }
        public bool? InvoiceBranchSeparated { get; set; }
        public string? Engineering_License { get; set; }
        public string? Engineering_LicenseDate { get; set; }
        public int? BankId { get; set; }
        public int? BankId2 { get; set; }
        public string? BankIdImgURL { get; set; }
        public string? BankId2ImgURL { get; set; }
        public bool? IsPrintInvoice { get; set; }
        public string? BranchLogoUrl { get; set; }

        public string? HeaderLogoUrl { get; set; }
        public string? FooterLogoUrl { get; set; }

        public bool? headerPrintInvoice { get; set; }
        public bool? headerPrintrevoucher { get; set; }
        public bool? headerprintdarvoucher { get; set; }
        public bool? headerPrintpayvoucher { get; set; }
        public bool? headerPrintcontract { get; set; }
        public int? BublicRevenue { get; set; }
        public int? OtherRevenue { get; set; }
        public string? CSR { get; set; }
        public string? PrivateKey { get; set; }
        public string? PublicKey { get; set; }
        public string? SecreteKey { get; set; }
        public int? ModeType { get; set; }

    }
}

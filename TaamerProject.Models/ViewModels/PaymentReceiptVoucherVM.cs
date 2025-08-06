using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class PaymentReceiptVoucherVM
    {
        public PaymentReceiptVoucherVM() {
            PaidCustomerPayments = new List<CustomerPaymentsVM>();
        }
        public int PaymentNo { get; set; }
        public string? PaymentDate { get; set; }
        public string? PaymentDateHijri { get; set; }
        public string? VoucherDate { get; set; }
        public string? VoucherDateHijri { get; set; }
        public decimal? VoucherAmount { get; set; }
        public string? VoucherAmountValText { get; set; }
        public decimal? VoucherTaxAmount { get; set; }
        public decimal? TotalVoucherAmount { get; set; }
        public decimal? RequiredAmount { get; set; }
        public string? AccountName { get; set; }
        public string? BranchName { get; set; }
        public string? VoucherNumber { get; set; }
        public string? ProjectNumber { get; set; }
        public string? CustomerName { get; set; }
        public string? ContractNumber { get; set; }
        public decimal TotalPiad { get; set; }
        public decimal TotalRemaining { get; set; }
        public string? TaxCode { get; set; }
        public string? VoucherDescription { get; set; }
        public string? OrganizationLogoUrl { get; set; }
        public string? OrganizationAddress { get; set; }
        public string? OrganizationPhone { get; set; }
        public string? OrganizationName { get; set; }
        public string? OrganizationWebSite { get; set; }
        public string? OrganizationMail { get; set; }
        public string? OrganizationCityName { get; set; }
        public List<CustomerPaymentsVM> PaidCustomerPayments { get; set; }
    }
}

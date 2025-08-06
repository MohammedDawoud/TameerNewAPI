using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class VoucherFilterVM
    {
        public int Type { get; set; }
        public string? dateFrom { get; set; }
        public string? dateTo { get; set; }
        public int? VoucherNo { get; set; }
        public bool? IsSearch { get; set; }
        public int? IsPost { get; set; }
        public int? ProjectId { get; set; }
        public bool? IsChecked { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CustomerId { get; set; }
        public int? Status { get; set; }

        public int? SupplierId { get; set; }
        public int? AccountId { get; set; }
        public string? QRCode { get; set; }
        public string? InvoiceNote { get; set; }

        public int? PageInsert { get; set; }
        public string? BranchName { get; set; }
        public bool? PrevInvoices { get; set; }




    }
}

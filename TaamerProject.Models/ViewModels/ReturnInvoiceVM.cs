using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class ReturnInvoiceVM
    {
        public int AccountId { get; set; }
        public decimal? Total { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Depit { get; set; }
    }

    public class GenerateNextVoucherNumberVM
    {
        public string? InvoiceNumber { get; set; }
        public int? BranchId { get; set; }
        public int? Type { get; set; }
        public int? YearId { get; set; }
        public string? NameAr { get; set; }
        public string? InvoiceStartCode { get; set; }
        public bool? InvoiceBranchSeparated { get; set; }
        public int? Newinvoicenumber { get; set; }

    }
}

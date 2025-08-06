using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class JournalsVM
    {
        public int JournalId { get; set; }
        public int? JournalNo { get; set; }
        public int? VoucherId { get; set; }
        public int? VoucherType { get; set; }
        public int? BranchId { get; set; }
      
        public int? UserId { get; set; }
        public string? Date { get; set; }
        public int? VoucherNo { get; set; }
        public string? VoucherTypeName { get; set; }
        public string? AccountCode { get; set; }
        public string? AccountName { get; set; }
        public string? Notes { get; set; }
        public decimal? Depit { get; set; }
        public decimal? Credit { get; set; }
        public string? ReferenceNo { get; set; }
        public List<TransactionsVM>? Transactions { get; set; }
    }
}

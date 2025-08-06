using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Journals : Auditable
    {
        public int JournalId { get; set; }
        public int JournalNo { get; set; }
        public int VoucherId { get; set; }
        public int VoucherType { get; set; }
        public int BranchId { get; set; }
        public int YearMalia { get; set; }
       
        public int UserId { get; set; }
        public Invoices? Invoice { get; set; }
    }
}

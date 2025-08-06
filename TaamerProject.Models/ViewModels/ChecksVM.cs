using System;
namespace TaamerProject.Models
{
    public class ChecksVM
    {
        public int CheckId { get; set; }
        public string? CheckNumber { get; set; }
        public decimal? Amount { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public int? BankId { get; set; }
        public string? BeneficiaryName { get; set; }
        public int? UserId { get; set; }
        public int? BranchId { get; set; }
        public DateTime? ActionDate { get; set; }
        public int? InvoiceId { get; set; }
        public decimal? TotalAmont { get; set; }
        public int Type { get; set; }
        public bool IsFinished { get; set; }
        public string? ReceivedName { get; set; }
        public string? Notes { get; set; }
        public string? TypeName { get; set; }
         public string? BanksName { get; set; }
    }
}

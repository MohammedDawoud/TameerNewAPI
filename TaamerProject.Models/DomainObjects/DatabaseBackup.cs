using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class DatabaseBackup : Auditable
    {
        public long BackupId { get; set; }
        public int? UserId { get; set; }
        public string? SavedName { get; set; }
        public string? LocalSavedPath { get; set; }       
        public string? Date { get; set; }
        public string? FileSize { get; set; } 
        public int? TotalProject { get; set; }
        public int? TotalClient { get; set; }
        public string? TotalExp { get; set; }
        public string? TotalReve { get; set; }
        public int? TotalBranches { get; set; }
        public int? TotalUsers { get; set; }
        public int? LastPro { get; set; }
        public int? Lastinvoice { get; set; }
        public int? LastVoucherRet { get; set; }
        public int? LastreVoucher { get; set; }
        public int? LastpayVoucher { get; set; }
        public int? LastEntyvoucher { get; set; }
        public int? LasEmpContract { get; set; }
        public int? LasCustomer { get; set; }
        public int? TotalarchiveProject { get; set; }

        public Users? Users { get; set; }
      

    }
}

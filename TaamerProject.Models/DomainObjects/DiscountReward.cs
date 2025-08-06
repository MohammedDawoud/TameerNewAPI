using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaamerProject.Models
{
    public class DiscountReward : Auditable
    {
        [Column("DisRewardId")]
        public int DiscountRewardId { get; set; }
        public int? EmployeeId { get; set; }
        public decimal? Amount { get; set; }
        public string? Date { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? HijriDate { get; set; }
        public int? Type { get; set; }
        public string? Notes { get; set; }

        public int? MonthNo { get; set; }
        public Employees? Employees { get; set; }
        public virtual List<Transactions>? TransactionDetails { get; set; }

    }
}

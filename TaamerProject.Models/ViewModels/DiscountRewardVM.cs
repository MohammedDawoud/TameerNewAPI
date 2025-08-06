using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class DiscountRewardVM
    {
        public int DiscountRewardId { get; set; }
        public int? EmployeeId { get; set; }
        public decimal? Amount { get; set; }
        public string? Date { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? HijriDate { get; set; }
        public int? Type { get; set; }
        public string? Notes { get; set; }
        public string? DiscountRewardName { get; set; }
        public int? MonthNo { get; set; }

    }
}

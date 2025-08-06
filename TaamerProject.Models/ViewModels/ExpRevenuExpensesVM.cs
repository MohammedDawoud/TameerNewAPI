using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ExpRevenuExpensesVM
    {
        public int ExpecteId { get; set; }
        public int? AccountId { get; set; }
        public int ToAccountId { get; set; }
        public int? CostCenterId { get; set; }
        public decimal? Amount { get; set; }
        public string? Notes { get; set; }
        public int ? Type { get; set; }
        public bool IsDone { get; set; }
        public string? CollectionDate { get; set; }
        public int? UserId { get; set; }
        public int? BranchId { get; set; }
        public string? TypeName { get; set; }
        public string? AccountName { get; set; }
        public string? ToAccountName { get; set; }
        public string? CostCenterName { get; set; }
        public string? StatusName { get; set; }
        public string? StartDate { get; set; }
       public string? EndDate { get; set; }
        public bool? IsChecked { get; set; }
    }
}

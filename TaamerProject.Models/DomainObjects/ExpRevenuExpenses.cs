
namespace TaamerProject.Models
{
    public class ExpRevenuExpenses : Auditable
    {
        public int ExpecteId { get; set; }
        public int AccountId { get; set; }
        public int ToAccountId { get; set; }
        public int CostCenterId { get; set; }
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
        public int Type { get; set; }
        public bool IsDone { get; set; }
        public string? CollectionDate { get; set; }
        public int? UserId { get; set; }
        public int? BranchId { get; set; }

        public virtual Accounts? Accounts { get; set; }
        public virtual Accounts? ToAccounts { get; set; }
        public virtual CostCenters? CostCenters { get; set; }
    }
}



namespace TaamerProject.Models
{
    public class CarMovements : Auditable

    {
        public int MovementId { get; set; }
        public int? ItemId { get; set; }
        public int? Type { get; set; }
        public int? EmpId { get; set; }
        public int? BranchId { get; set; }
        public string? Date { get; set; }
        //public string? EndDate { get; set; }
        public string? HijriDate { get; set; }
        public decimal? EmpAmount { get; set; }
        public decimal? OwnerAmount { get; set; }
        public string? Notes { get; set; }
        public int? AccountId { get; set; }

        public virtual Employees? Employees { get; set; }
        public virtual Item? Item { get; set; }
        public virtual CarMovementsType? Types { get; set; }

    }


     public class Carmovement2   {
        public int MovementId { get; set; }
    public int? ItemId { get; set; }
    public int? Type { get; set; }
    public int? EmpId { get; set; }
    public int? BranchId { get; set; }
    public string? Date { get; set; }
    //public string? EndDate { get; set; }
    public string? HijriDate { get; set; }
    public decimal? EmpAmount { get; set; }
    public decimal? OwnerAmount { get; set; }
    public string? Notes { get; set; }
    public int? AccountId { get; set; }

    public virtual Employees? Employees { get; set; }
    public virtual Item? Item { get; set; }
    public virtual CarMovementsType? Types { get; set; }

}
}

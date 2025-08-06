
namespace TaamerProject.Models
{
    public class ExpensesGovernmentType : Auditable
    {
        public int ExpensesGovernmentTypeId { get; set; }
        public string? Code { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Notes { get; set; }
        public int? UserId { get; set; }
        public int BranchId { get; set; }
    }
}


namespace TaamerProject.Models
{
    public class ExternalEmployees : Auditable
    {
        public int EmpId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int? DepartmentId { get; set; }
        public string? Description { get; set; }
        public int BranchId { get; set; }
        public Department? Department { get; set; }
    }
}

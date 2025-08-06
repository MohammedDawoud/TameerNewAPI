
namespace TaamerProject.Models
{
    public class BuildTypes : Auditable
    {
        public int BuildTypeId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Description { get; set; }
    }
}

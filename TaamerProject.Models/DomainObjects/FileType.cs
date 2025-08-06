
namespace TaamerProject.Models
{
    public class FileType : Auditable
    {
        public int FileTypeId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int BranchId { get; set; }
    }
}

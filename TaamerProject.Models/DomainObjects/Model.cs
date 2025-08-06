
using System.ComponentModel.DataAnnotations.Schema;

namespace TaamerProject.Models
{
    public class Model : Auditable
    {
        public static string? AttachmentUrl;

        public int ModelId { get; set; }
        public string? ModelName { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public string? Notes { get; set; }
        public int? UserId { get; set; }
        public string? FileUrl { get; set; }
        public int? TypeId { get; set; }
        public string? Extension { get; set; }
        public int BranchId { get; set; }
        public Users? Users { get; set; }
        public FileType? FileType { get; set; }
        [NotMapped]
        public List<int>? ModelRequirementsIds { get; set; }
    }
}

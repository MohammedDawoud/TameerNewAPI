
namespace TaamerProject.Models
{
    public class ProjectFiles : Auditable
    {
        public int FileId { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public decimal FileSize { get; set; }
        public string? Extension { get; set; }
        public int? TypeId { get; set; }
        public string? Notes { get; set; }
        public bool? IsCertified { get; set; }
        public int? ProjectId { get; set; }
        public int? TaskId { get; set; }
        public string? Brand { get; set; }
        public int? NotificationId { get; set; }
        public int? UserId { get; set; }
        public int? BranchId { get; set; }
        public string? Type { get; set; }
        public string? DeleteUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? DeleteType { get; set; }
        public string? UploadDate { get; set; }
        public string? BarcodeFileNum { get; set; }
        public string? CompanyTaxNo { get; set; }


        public bool? IsShare { get; set; }
        public bool? ViewShare { get; set; }
        public bool? DonwloadShare { get; set; }
        public int? TimeShare { get; set; }
        public int? TimeTypeShare { get; set; }
        public string? TimeShareDate { get; set; }
        public string? FileUrlW { get; set; }
        public string? CustomeComment { get; set; }
        public int? PageInsert { get; set; }
        public int? UploadType { get; set; }
        public string? UploadName { get; set; }
        public string? UploadFileId { get; set; }
        public string? UploadFileIdB { get; set; }

        public FileType? FileType { get; set; }
        public Project? Project { get; set; }
        public virtual Users? Users { get; set; }
        public virtual ProjectPhasesTasks? ProjectPhasesTasks { get; set; }
    }
}


namespace TaamerProject.Models
{
    public class Settings : Auditable
    {
        public int SettingId { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public int? ParentId { get; set; }
        public int? ProjSubTypeId { get; set; }
        public int? Type { get; set; }
        public int? TimeMinutes { get; set; }
        public bool? IsUrgent { get; set; }
        public bool? IsTemp { get; set; }
        public int? TaskType { get; set; }
        public int? OrderNo { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public decimal? PercentComplete { get; set; }
        public decimal? Cost { get; set; }
        public int? BranchId { get; set; }
        public string? Notes { get; set; }
        public int? UserId { get; set; }
        public int? TimeType { get; set; }
        public long? LocationId { get; set; }
        public int? CopySettingId { get; set; }
        public int? Priority { get; set; }
        public int? ExecPercentage { get; set; }
        public int? IsMerig { get; set; }
        public string? EndTime { get; set; }

        public int? RequirmentId { get; set; }

        public decimal? Totaltaskcost { get; set; }
        public decimal? Totalhourstask { get; set; }


        public Employees? Employees { get; set; }
        public NodeLocations? NodeLocations { get; set; }
        public Users? Users { get; set; }
        public virtual TaskType? TaskTypeModel { get; set; }
    }
}

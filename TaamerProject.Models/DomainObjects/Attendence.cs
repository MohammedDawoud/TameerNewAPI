using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaamerProject.Models
{
    public class Attendence : Auditable
    {
        public long AttendenceId { get; set; }
        public int EmpId { get; set; }
        public int RealEmpId { get; set; }
        public string? Day { get; set; }
        public string? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public bool? IsLate { get; set; }
        public int? LateDuration { get; set; }
        public bool? IsOverTime { get; set; }
        public string? SameDate { get; set; }
        public bool? IsDone { get; set; }
        public int BranchId { get; set; }
        public int? Type { get; set; }
        public int? Source { get; set; }
        public string? Hint { get; set; }
        public int? ShiftTime { get; set; }
        public string? CheckType { get; set; }
        public DateTime CheckTime { get; set; }
        public string? WorkCode { get; set; }
        public bool? Done { get; set; }
        public int? MoveTime { get; set; }

        public string? AttendenceDate { get; set; }
        public string? AttendenceHijriDate { get; set; }
        public virtual Employees? Employees { get; set; }
        public virtual Branch? Branch { get; set; }

        [NotMapped]
        public string? Month { get; set; }

        public string? Location { get; set; }

        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public int? FromApplication { get; set; }
        public string? Comment { get; set; }
        [NotMapped]
        public int? Hour { get; set; }

        [NotMapped]
        public int? Minute { get; set; }

    }
}

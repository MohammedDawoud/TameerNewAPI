using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class AttTimeDetails : Auditable
    {
       
        public int TimeDetailsId { get; set; }
        public int? AttTimeId { get; set; }
        public int Day { get; set; }
        public DateTime? DayDate { get; set; }
        public DateTime? _1StFromHour { get; set; }
        public DateTime? _1StToHour { get; set; }
        public DateTime? _2ndFromHour { get; set; }
        public DateTime? _2ndToHour { get; set; }
        public bool? IsWeekDay { get; set; }
        public int BranchId { get; set; }
        public virtual AttendaceTime? AttendaceTime { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class AttTimeDetailsVM
    {
        public int TimeDetailsId { get; set; }
        public int? AttTimeId { get; set; }
        public int? Day { get; set; }
        public string? DayName { get; set; }
        public DateTime? DayDate { get; set; }
        public DateTime? _1StFromHour { get; set; }
        public DateTime? _1StToHour { get; set; }
        public DateTime? _2ndFromHour { get; set; }
        public DateTime? _2ndToHour { get; set; }

        public string? _1StFromHour_Time { get; set; }
        public  string? _1StToHour_Time { get; set; }
        public string? _2ndFromHour_Time { get; set; }
        public string? _2ndToHour_Time { get; set; }
        public bool? IsWeekDay { get; set; }
        public int? BranchId { get; set; }

        public decimal? EmpHourlyCost { get; set; }

    }
}

using System;

namespace TaamerProject.Models
{
    public class OfficalHolidayVM : Auditable
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string? Description { get; set; }

    }
}

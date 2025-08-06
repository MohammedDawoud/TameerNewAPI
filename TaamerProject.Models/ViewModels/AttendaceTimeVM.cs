using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class AttendaceTimeVM
    {
        public int TimeId { get; set; }
        public string? Code { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Notes { get; set; }
        public int? UserId { get; set; }
        public int? BranchId { get; set; }

    }
}

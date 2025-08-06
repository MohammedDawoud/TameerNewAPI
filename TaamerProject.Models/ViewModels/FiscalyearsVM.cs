using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class FiscalyearsVM
    {
        public int FiscalId { get; set; }
        public int? YearId { get; set; }
        public string? YearName { get; set; }
        public int? BranchId { get; set; }
        public int? UserId { get; set; }
        public bool? IsActive { get; set; }
    }
}

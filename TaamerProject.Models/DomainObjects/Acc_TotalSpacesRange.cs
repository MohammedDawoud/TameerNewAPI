using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Acc_TotalSpacesRange : Auditable
    {
        public int TotalSpacesRangeId { get; set; }
        public string? TotalSpacesRengeName { get; set; }
        public int? RangeValue { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Acc_Floors : Auditable
    {
        public int FloorId { get; set; }
        public string? FloorName { get; set; }
        public decimal? FloorRatio { get; set; }
    }
}

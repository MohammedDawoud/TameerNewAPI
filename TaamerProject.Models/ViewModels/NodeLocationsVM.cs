using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class NodeLocationsVM
    {
        public int LocationId { get; set; }
        public int? SettingId { get; set; }
        public int? TaskId { get; set; }
        public string? Location { get; set; }
    }
}

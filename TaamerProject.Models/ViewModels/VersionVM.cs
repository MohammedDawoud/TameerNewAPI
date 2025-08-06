using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class VersionVM
    {
        public int VersionId { get; set; }
        public string? VersionCode { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class JobVM
    {
        public int JobId { get; set; }
        public string? JobCode { get; set; }
        public string? JobNameAr { get; set; }
        public string? JobNameEn { get; set; }
        public string? Notes { get; set; }
    }
}

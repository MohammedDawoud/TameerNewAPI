using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class ProjectSubTypeVM
    {
        public int SubTypeId { get; set; }
        public int ProjectTypeId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? TimePeriod { get; set; }
        public string? TimePeriodStr { get; set; }


    }
}

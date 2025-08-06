using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class GuideDepartmentDetails : Auditable
    {
        public int DepDetailsId { get; set; }
        public int DepId { get; set; }
        public int Type { get; set; }
        public string? Header { get; set; }
        public string? Link { get; set; }
        public string? Text { get; set; }
        public string? NameAR { get; set; }
        public string? NameEn { get; set; }

        public virtual GuideDepartments? GuideDepartments { get; set; }
    }
}

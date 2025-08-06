using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class GuideDepartments : Auditable
    {
        public int DepId { get; set; }
        public string? DepNameAr { get; set; }
        public string? DepNameEn { get; set; }
    }
}

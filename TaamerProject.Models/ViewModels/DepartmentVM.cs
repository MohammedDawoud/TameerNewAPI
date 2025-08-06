using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class DepartmentVM
    {
        public int DepartmentId { get; set; }
        public string? DepartmentNameAr { get; set; }
        public string? DepartmentNameEn { get; set; }
        public int? Type { get; set; }
        public string? TypeName { get; set; }
        public int BranchId { get; set; }

    }
}

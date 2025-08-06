using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class ExternalEmployeesVM
    {
        public int EmpId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int? DepartmentId { get; set; }
        public string? Description { get; set; }
        public string? DepartmentName { get; set; }
    }
}

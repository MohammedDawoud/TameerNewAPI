using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class EmpStructureVM
    {
        public long StructureId { get; set; }
        public int? EmpId { get; set; }
        public int? ManagerId { get; set; }
        public int? BranchId { get; set; }
    }
}

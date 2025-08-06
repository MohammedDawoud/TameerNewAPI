using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class DependencySettingsVM
    {
        public int DependencyId { get; set; }
        public int? PredecessorId { get; set; }
        public int? SuccessorId { get; set; }
        public int? Type { get; set; }
        public int? ProjSubTypeId { get; set; }
        public int? BranchId { get; set; }
        //public Tasks Tasks { get; set; }
    }
}

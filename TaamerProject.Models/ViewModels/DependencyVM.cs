using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class DependencyVM
    {
        public int DependencyId { get; set; }
        public int PredecessorId { get; set; }
        public int SuccessorId { get; set; }
        public int Type { get; set; }
        public string? TaskCode{ get; set; }
        public string? TaskName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}

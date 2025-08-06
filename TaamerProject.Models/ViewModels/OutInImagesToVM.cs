using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class OutInImagesToVM
    {
        public int ImageToId { get; set; }
        public int? OutInboxId { get; set; }
        public int? DepartmentId { get; set; }
        public int? BranchId { get; set; }

    }
}

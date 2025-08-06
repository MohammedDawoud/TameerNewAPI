using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
   public class rptGetEmpDoneTasksVM
    {
        public string? DescriptionAr { get; set; }
        public string? Duration { get; set; }
        public string? ProjectNo { get; set; }
        public string? ClientName { get; set; }
        public string? EndDate { get; set; }
        public string? Cost { get; set; }
        public string? EmpName { get; set; }
        public string? JobName { get; set; }
        public int? Remaining { get; set; }
        public int? Status { get; set; }

    }
}

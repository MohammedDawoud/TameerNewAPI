using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class rptGetOnGoingWorkOrdersByExecEmp
    {
        public string? WorkOrderId { get; set; }
        public string? RequiredOrder { get; set; }
        public string? ByUser { get; set; }
        public string? CustomerName { get; set; }
        public string? Duration { get; set; }
        public string? LeftDuration { get; set; }
        public string? EmpName { get; set; }
        public string? JobName { get; set; }
    }
}

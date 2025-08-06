using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Pro_TaskOperationsVM
    {
        public int TaskOperationId { get; set; }
        public int? PhaseTaskId { get; set; }
        public int? WorkOrderId { get; set; }
        public int? Type { get; set; }
        public string? OperationName { get; set; }
        public string? Date { get; set; }
        public int? UserId { get; set; }
        public int? BranchId { get; set; }
        public string? Note { get; set; }
        public string? TaskNo { get; set; }
        public string? DescriptionAr { get; set; }
        public string? ExtraNote { get; set; }
        public string? AddUserName { get; set; }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
   public class Pro_TaskOperations : Auditable
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
        public virtual ProjectPhasesTasks? ProjectPhasesTasks { get; set; }
        public virtual WorkOrders? WorkOrders { get; set; }
        public virtual Users? AddUsers { get; set; }
        public virtual Users? Users { get; set; }

    }
}

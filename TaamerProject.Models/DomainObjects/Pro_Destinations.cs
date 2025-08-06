using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Pro_Destinations : Auditable
    {
        public int DestinationId { get; set; }
        public int? ProjectId { get; set; }
        public string? TransactionNumber { get; set; }
        public int? DestinationTypeId { get; set; }
        public int? DepartmentId { get; set; }
        public string? FileName { get; set; }
        public int? UserId { get; set; }
        public int? UserIdRec { get; set; }
        public int? Counter { get; set; }
        public int? CounterRec { get; set; }
        public int? Status { get; set; }
        public DateTime? AddFileDate { get; set; }
        public DateTime? AddFileDateRec { get; set; }
        public string? Notes { get; set; }
        public string? NotesRec { get; set; }
        public int? FileReasonId { get; set; }
        public int? BranchId { get; set; }
        [NotMapped]
        public string? Checkcode { get; set; }
        [NotMapped]
        public string? AddFileDateTime { get; set; }
        [NotMapped]
        public string? AddFileDateRecTime { get; set; }
        public virtual Users? User { get; set; }
        public virtual Users? UserRec { get; set; }
        public virtual Project? Project { get; set; }
        public virtual Pro_DestinationTypes? DestinationType { get; set; }
        public virtual Pro_DestinationDepartments? Department { get; set; }





    }
}

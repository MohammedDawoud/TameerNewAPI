using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Pro_DestinationsVM
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
        public string? CustomerName { get; set; }
        public string? ProjectNo { get; set; }
        public string? DestinationTypeName { get; set; }
        public string? UserName { get; set; }
        public string? UserRecName { get; set; }
        public string? StatusName { get; set; }
        public string? FirstProjectDate { get; set; }
        public string? FirstProjectExpireDate { get; set; }
        public string? DepartmentName { get; set; }



    }
}

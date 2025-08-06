using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ProjectTrailingVM
    {
        public int TrailingId { get; set; }
        public int? ProjectId { get; set; }
        public int? DepartmentId { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public bool? Active { get; set; }
        public int? Status { get; set; }
        public int? TypeId { get; set; }
        public int? UserId { get; set; }
        public string? ReceiveDate { get; set; }
        public string? ReceiveHijriDate { get; set; }
        public int? ReceiveUserId { get; set; }
        public string? Notes { get; set; }
        public int? TaskId { get; set; }
        public int? BranchId { get; set; }
        public string? ProjectNumber { get; set; }
        public string? SideName { get; set; }
        public string? CustomerName { get; set; }
        public string? ProjectTypeName { get; set; }
        public string? SketchNumber { get; set; }
        public string? SiteName { get; set; }
        public string? MobileNo { get; set; }
    }
}

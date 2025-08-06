using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ProjectWorkersVM
    {
        public long WorkerId { get; set; }
        public int? ProjectId { get; set; }
        public int? UserId { get; set; }
        public int? BranchId { get; set; }
        public int? WorkerType { get; set; }
        public string? UserFullName { get; set; }
        public string? WorkerTypeName { get; set; }

        public string? ProjectNo { get; set; }
        public string? ProjectTypeName { get; set; }
        public string? ProjectSubTypeName { get; set; }
        public string? ProjectNumber { get; set; }
        public string? ProjectMangerName { get; set; }
        public string? ProjectExpectedTime { get; set; }
        public string? ProjectDescription { get; set; }
        public decimal Cost { get; set; }
        public string? CustomerName { get; set; }
        public string? ProjectTypesName { get; set; }
        public int? ExpectedTime { get; set; }

        public string? UserImg { get; set; }



    }
}

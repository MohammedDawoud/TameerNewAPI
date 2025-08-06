using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class NodeLocations : Auditable
    {
        public long LocationId { get; set; }
        public int? SettingId { get; set; }
        public int? TaskId { get; set; }
        public string? Location { get; set; }
        public int? ProjectId { get; set; }
        public int? ProSubTypeId { get; set; }
        public int? EmpId { get; set; }
    }
}

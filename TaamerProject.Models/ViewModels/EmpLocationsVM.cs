using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class EmpLocationsVM
    {
        public int EmpLocationId { get; set; }
        public int EmpId { get; set; }
        public int LocationId { get; set; }
        public string? EmployeeName { get; set; }
        public string? JobName { get; set; }
        public string? AttendenceLocationName { get; set; }
        public string? DepartmentName { get; set; }
        public bool? allowoutsidesite { get; set; }
        public bool? allowallsite { get; set; }

    }
}

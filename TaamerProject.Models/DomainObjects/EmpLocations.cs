using System;
using System.ComponentModel.DataAnnotations;
using TaamerProject.Models.DomainObjects;
namespace TaamerProject.Models
{
    public class EmpLocations : Auditable
    {
        public int EmpLocationId { get; set; }
        public int EmpId { get; set; }
        public int LocationId { get; set; }
        public Employees? Employee { get; set; }
        public AttendenceLocationSettings? AttendenceLocation { get; set; }
    }
}

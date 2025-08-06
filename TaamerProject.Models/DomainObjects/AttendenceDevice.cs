using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class AttendenceDevice : Auditable
    {
        public int AttendenceDeviceId { get; set; }
        public string? DeviceIP { get; set; }
        public string? Port { get; set; }
        public string? MachineNumber { get; set; }
        public int BranchId { get; set; }
        public Branch? BranchName { get; set; }
        public DateTime LastUpdate { get; set; }
       
    }
}

using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class AttendenceDeviceVM
    {
        public int AttendenceDeviceId { get; set; }
        public string? DeviceIP { get; set; }
        public string? Port { get; set; }
        public string? MachineNumber { get; set; }
        public int? BranchId { get; set; }
        public bool? IsSearch { get; set; }
        public string? BranchName { get; set; }
        public DateTime? LastUpdate { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class DeviceAtt : Auditable
    {
        public int Id { get; set; }
        public string? DeviceId { get; set; }
        public DateTime LastUpdate { get; set; }
      
    }
}

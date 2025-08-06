using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace TaamerProject.Models
{
    public class OutInBoxSerial : Auditable
    {
        public int OutInSerialId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int? LastNumber { get; set; }
        public int? Type { get; set; }
        public int? BranchId { get; set; }
    }
}

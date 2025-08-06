using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class OutInBoxSerialVM
    {
        public int OutInSerialId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int? LastNumber { get; set; }
        public int? Type { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class InstrumentSourcesVM
    {
        public int SourceId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class InstrumentSources : Auditable
    {
        public int SourceId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
    }
}

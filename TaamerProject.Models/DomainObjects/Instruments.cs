using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Instruments : Auditable
    {
        public int InstrumentId { get; set; }
        public string? InstrumentNo { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public int? ProjectId { get; set; }
        public int? SourceId { get; set; }
        public Project? project { get; set; }
        public InstrumentSources? instrumentSources { get; set; }
    }
}

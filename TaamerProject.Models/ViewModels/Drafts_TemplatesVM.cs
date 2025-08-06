using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Drafts_TemplatesVM
    {
        public int DraftTempleteId { get; set; }
        public int? ProjectTypeId { get; set; }
        public string? Name { get; set; }
        public string? DraftUrl { get; set; }

        public string? ProjectTypeName { get; set; }
    }
}

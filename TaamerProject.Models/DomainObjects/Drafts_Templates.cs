using System;
using System.Collections.Generic;

namespace TaamerProject.Models
{
    public class Drafts_Templates : Auditable
    {
        public int DraftTempleteId { get; set; }
        public int? ProjectTypeId { get; set; }
        public string? Name { get; set; }
        public string? DraftUrl { get; set; }
        public virtual ProjectType? ProjectType { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace TaamerProject.Models
{
    public class Draft : Auditable
    {
        public int DraftId { get; set; }
        public int? ProjectTypeId { get; set; }
        public string? Name { get; set; }
        public string? DraftUrl { get; set; }
        public virtual ProjectType? ProjectType { get; set; }
        public virtual List<DraftDetails>? DraftDetails { get; set; }
    }
}

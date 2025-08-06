using System;
using System.Collections.Generic;

namespace TaamerProject.Models
{
    public class DraftDetails : Auditable
    {
        public int DraftDetailId { get; set; }
        public int DraftId { get; set; }
        public int? ProjectId { get; set; }
        public virtual Draft? Draft { get; set; }
        public virtual Project? Project { get; set; }
    }
}

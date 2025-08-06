using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ProSettingDetailsNew : Auditable
    {
        public int ProSettingId { get; set; }
        public string? ProSettingNo { get; set; }
        public string? ProSettingNote { get; set; }
        public int? ProjectTypeId { get; set; }
        public int? ProjectSubtypeId { get; set; }
        public  Users? Users { get; set; }
        public ProjectType? ProjectType { get; set; }
        public ProjectSubTypes? ProjectSubTypes { get; set; }

    }
}

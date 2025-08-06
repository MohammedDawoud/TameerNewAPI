using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.DomainObjects
{
    public class NotificationConfiguration :Auditable
    {
        public int ConfigurationId { get; set; }

        public string? Code { get; set; }

        public string? Description { get; set; }

        public string? Title { get; set; }

        public int? To { get; set; }

        public int? BranchId { get; set; }
        public virtual List<NotificationConfigurationsAssines>? NotificationConfigurationsAssines { get; set; }
        [NotMapped]
        public List<int>? Assignees { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.ViewModels
{
    public class NotificationConfigurationVM
    {
        public int ConfigurationId { get; set; }

        public string? Code { get; set; }

        public string? Description { get; set; }

        public string? Title { get; set; }

        public int? To { get; set; }

        public int? BranchId { get; set; }

        public List<int?>? Assignees { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class LayoutVM
    {
        public int NotificationsCount { get; set; }
        public int? AllertCount { get; set; }
        public int? TasksByUserCount { get; set; }
        public int? MyInboxCount { get; set; }
        public int? ProjectCount { get; set; }
        public SelectVM? Currency { get; set; }
        public SelectVM? Year { get; set; }
        public int? WorkOrderCount { get; set; }
        public int? TaskCount { get; set; }
        public int? OnlineUser { get; set; }
    }

    public class SelectVM
    {
        public int? Id { get; set; }
        public int? Name { get; set; }
    }
}

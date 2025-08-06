using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.DomainObjects
{
    public class NotificationConfigurationsAssines :Auditable
    {
        public int ConfigurationAssinesId { get; set; }
        public int? ConfigurationId { get; set; }
        public int? UserId { get; set; }
        public NotificationConfiguration NotificationConfiguration { get; set; }
    }
}

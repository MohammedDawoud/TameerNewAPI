using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.DomainObjects
{
    public class CommercialActivity :Auditable
    {
        public int CommercialActivityId { get; set; }
        public int? Type { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.DomainObjects
{
    public class Customer_Branches :Auditable
    {
        public int Customer_BranchesId { get; set; }
        public int? CustomerId { get; set; }
        public int? BranchId { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Branch? Branch { get; set; }
    }
}

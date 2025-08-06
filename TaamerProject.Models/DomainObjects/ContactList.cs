using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.DomainObjects
{
    public class ContactList : Auditable
    {
        public int ContactListId { get; set; }
        public int? UserId { get; set; }
        public string? ContactDate { get; set; }
        public string? Contacttxt { get; set; }
        public int? ProjectId { get; set; }
        public int? TaskId { get; set; }
        public int? OrderId { get; set; }
        public virtual Users? Users { get; set; }
        public virtual WorkOrders? WorkOrders { get; set; }
        public virtual ProjectPhasesTasks? ProjectPhasesTasks { get; set; }

    }
}

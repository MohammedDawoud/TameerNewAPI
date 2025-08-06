using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Acc_Services_PriceOffer : Auditable
    {
        public int ServicesIdVou { get; set; }
        public int? OfferId { get; set; }
        public int? ServicesId { get; set; }
        public int? ParentId { get; set; }
        public int? SureService { get; set; }
        public int? ContractId { get; set; }
        public int? LineNumber { get; set; }
        public int? InvoiceId { get; set; }


        public virtual Acc_Services_Price? ServiceDetails { get; set; }

    }
}

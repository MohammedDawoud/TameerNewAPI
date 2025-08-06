using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class AccServicesPricesOfferVM
    {
        public int  Id{ get; set; }
        public string? Name{ get; set; }
        public int ServicesIdVou { get; set; }
        public int? OfferId { get; set; }
        public int? ServicesId { get; set; }
        public int? ParentId { get; set; }
        public int? SureService { get; set; }
        public string? ServicesName { get; set; }
        public string? ServicesNameEn { get; set; }

        public int? ContractId { get; set; }
        public int? LineNumber { get; set; }
        public int? InvoiceId { get; set; }



    }
}

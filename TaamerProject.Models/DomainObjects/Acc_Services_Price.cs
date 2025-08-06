using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Acc_Services_Price : Auditable
    {
        public int ServicesId { get; set; }
        public string? ServicesName { get; set; }
        public decimal? Amount { get; set; }
        public string? AccountName { get; set; }
        public int? AccountId { get; set; }
        public int? ProjectId { get; set; }
        public int? ProjectSubTypeID { get; set; }

        public int? ParentId { get; set; }
        public int? CostCenterId { get; set; }
        public int? PackageId { get; set; }
        public string? ServiceName_EN { get; set; }
        public int? ServiceType { get; set; }
        public virtual Acc_Packages? Package { get; set; }

        public virtual Accounts? AccountParentId { get; set; }
        public virtual ProjectType? ProjectParentId { get; set; }
        public virtual ProjectSubTypes? ProjectSubTypes { get; set; }
        public virtual Acc_Services_Price? Parent { get; set; }

        public virtual List<OfferService>? OfferService { get; set; }
        public virtual List<ContractServices>? ContractServices { get; set; }
        public virtual Acc_Services_PriceOffer? servicepriceoffer { get; set; }


    }
}

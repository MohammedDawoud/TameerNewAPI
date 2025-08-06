using System;
using System.Collections.Generic;

namespace TaamerProject.Models
{
    public class OffersConditions : Auditable
    {

        public int OffersConditionsId { get; set; }
        public string? OfferConditiontxt { get; set; }
        public int? OfferId { get; set; }
        public int BranchId { get; set; }
        public int? Isconst { get; set; }

        public string? OfferConditiontxt_EN { get; set; }

        public virtual OffersPrices? OffersPrices { get; set; }
        
    }
}

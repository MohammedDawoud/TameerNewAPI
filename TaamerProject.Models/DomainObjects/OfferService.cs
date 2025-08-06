
namespace TaamerProject.Models
{
    public class OfferService : Auditable
    {
        public int OffersServicesId { get; set; }
        public int? OfferId { get; set; }

        public int? ServiceId { get; set; }
        public int? ServiceQty { get; set; }
        public string? serviceoffertxt { get; set; }
        public int? BranchId { get; set; }
        public int? TaxType { get; set; }

        public decimal? Serviceamountval { get; set; }
        public int? LineNumber { get; set; }
        public decimal? Amount { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? DiscountPercentage_Det { get; set; }
        public decimal? DiscountValue_Det { get; set; }
        public virtual OffersPrices? OffersPrices { get; set; }
        public virtual Acc_Services_Price?  serviceprice  { get; set; }



    }
}

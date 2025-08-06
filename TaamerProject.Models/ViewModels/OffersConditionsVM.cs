using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class OffersConditionsVM
    {

        public int OffersConditionsId { get; set; }
        public string? OfferConditiontxt { get; set; }
        public int? OfferId { get; set; }
        public int BranchId { get; set; }
        public int? Isconst { get; set; }
        public string? OfferConditiontxt_EN { get; set; }
    }
}

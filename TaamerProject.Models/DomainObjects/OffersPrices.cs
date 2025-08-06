using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaamerProject.Models
{
    public class OffersPrices : Auditable
    {

        public int OffersPricesId { get; set; }
        public string? OfferNo { get; set; }
        public string? OfferDate { get; set; }
        public string? OfferHijriDate { get; set; }
        public int? UserId { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? Department { get; set; }
        public decimal OfferValue { get; set; }
        public string? OfferValueTxt { get; set; }

        public int? OfferStatus { get; set; }
        public int? CustomerStatus { get; set; }
        public int? ServiceId { get; set; }
        public int? BranchId { get; set; }

        public string? RememberDate { get; set; }
        public int? ISsent { get; set; }
        public bool? OfferAlarmCheck { get; set; }
        public int? ServQty { get; set; }
        public int? OfferNoType { get; set; }
        public int? ProjectId { get; set; }


        public bool? IsContainLogo { get; set; }

        public bool? IsContainSign { get; set; }
        public bool? printBankAccount { get; set; }

        public string? CustomerEmail { get; set; }
        public string? Customerphone { get; set; }

        public string? CUstomerName_EN { get; set; }
        public int? IsEnglish { get; set; }

        public string? NickName { get; set; }
        public string? Description { get; set; }

        public string? Introduction { get; set; }
        public int? setIntroduction { get; set; }

        public int? NotDisCustPrint { get; set; }
        public int? CustomerMailCode { get; set; }
        public bool? IsCertified { get; set; }
        public string? CertifiedCode { get; set; }
        public string? ProjectName { get; set; }
        public int? ImplementationDuration { get; set; }
        public int? OfferValidity { get; set; }

        public virtual List<CustomerPayments>? CustomerPayments { get; set; }
        public virtual List<OffersConditions>? OffersConditions { get; set; }
        public virtual List<OfferService>? OfferService { get; set; }

        [NotMapped]
        public virtual List<Acc_Services_PriceOffer>? ServicesPriceOffer { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Users? Users { get; set; }
        public virtual Service? Service { get; set; }
        public virtual Project? Project { get; set; }


    }
}

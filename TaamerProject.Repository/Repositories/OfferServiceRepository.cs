using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class OfferServiceRepository : IOfferServiceRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public OfferServiceRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }


        public async Task< IEnumerable<OfferServiceVM>> GetOfferservicenByid(int offerid)
        {

            var Offers = _TaamerProContext.OfferService.Where(s => s.IsDeleted == false && s.OfferId == offerid).Select(x => new OfferServiceVM
            {
                OffersServicesId=x.OffersServicesId,
                OfferId=x.OfferId,
                ServiceId=x.ServiceId,
                ServiceQty=x.ServiceQty,
                BranchId=x.BranchId,
                serviceoffertxt=x.serviceoffertxt,
                TaxType=x.TaxType,
                ServiceName=x.serviceprice.ServicesName,
                ServiceAmount=x.serviceprice.Amount,
                Serviceamountval=x.Serviceamountval==null? x.serviceprice.Amount: x.Serviceamountval,
                LineNumber=x.LineNumber??0,
                ServicesName = x.serviceprice.ServicesName,
                ServicesId = x.ServiceId,
                ServicesNameEN=x.serviceprice.ServiceName_EN==null? x.serviceprice.ServicesName : x.serviceprice.ServiceName_EN,
                Amount = x.Amount ?? 0,
                TaxAmount = x.TaxAmount ?? 0,
                TotalAmount = x.TotalAmount ?? 0,
                DiscountPercentage_Det = x.DiscountPercentage_Det ?? 0,
                DiscountValue_Det = x.DiscountValue_Det ?? 0,

            }).ToList().OrderBy(x => x.LineNumber).ThenBy(x => x.OffersServicesId).ToList();
            return Offers;


        }

    }
}

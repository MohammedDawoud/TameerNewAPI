using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class OfferpricesconditionRepository : IOfferpriceconditionRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;


        public OfferpricesconditionRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }


        public async Task<IEnumerable<OffersConditionsVM>> GetOfferconditionByid(int offerid)
        {
            var Offers = _TaamerProContext.OffersConditions.Where(s => s.IsDeleted == false && s.OfferId == offerid).Select(x => new OffersConditionsVM
            {
                OffersConditionsId=x.OffersConditionsId,
                BranchId=x.BranchId,
                OfferConditiontxt=x.OfferConditiontxt,
                OfferId=x.OfferId,
                Isconst=x.Isconst,
                OfferConditiontxt_EN=x.OfferConditiontxt_EN==null?x.OfferConditiontxt : x.OfferConditiontxt_EN ??"",

            }).ToList();
            return Offers;


        }

        public async Task<IEnumerable<OffersConditionsVM>> GetOfferconditionBytxt(string OfferConditiontxt)
        {
            var Offers = _TaamerProContext.OffersConditions.Where(s => s.IsDeleted == false && s.OfferConditiontxt == OfferConditiontxt).Select(x => new OffersConditionsVM
            {
                OffersConditionsId = x.OffersConditionsId,
                BranchId = x.BranchId,
                OfferConditiontxt = x.OfferConditiontxt,
                OfferId = x.OfferId,
                Isconst = x.Isconst,
                OfferConditiontxt_EN = x.OfferConditiontxt_EN == null ? x.OfferConditiontxt : x.OfferConditiontxt_EN ?? "",
            }).ToList();
            return Offers;


        }

        public async Task<IEnumerable<OffersConditionsVM>> GetOfferconditionconst(int BranchId)
        {
            var Offers = _TaamerProContext.OffersConditions.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Isconst == 1).Select(x => new OffersConditionsVM
            {
                OffersConditionsId = x.OffersConditionsId,
                BranchId = x.BranchId,
                OfferConditiontxt = x.OfferConditiontxt,
                OfferId = x.OfferId,
                Isconst = x.Isconst,
                OfferConditiontxt_EN = x.OfferConditiontxt_EN == null ? x.OfferConditiontxt : x.OfferConditiontxt_EN ?? "",
            }).ToList().GroupBy(s=>s.OfferConditiontxt).Select(x=>x.First());
            return Offers;


        }

    }
}

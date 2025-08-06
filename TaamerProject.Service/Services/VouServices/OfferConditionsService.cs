using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.Service.Services
{
    public class OfferConditionsService : IOfferpriceconditionService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IOfferpriceconditionRepository _offerpriceconditionRepository;



        public OfferConditionsService(TaamerProjectContext dataContext, ISystemAction systemAction, IOfferpriceconditionRepository offerpriceconditionRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _offerpriceconditionRepository = offerpriceconditionRepository;
        }

        public async Task<IEnumerable<OffersConditionsVM>> GetOfferconditionByid(int offerid)
        {
            var offers =await _offerpriceconditionRepository.GetOfferconditionByid(offerid);
            return offers;
        }

        public async Task<IEnumerable<OffersConditionsVM>> GetOfferconditionByid(string offercontxt)
        {
            var offers = await _offerpriceconditionRepository.GetOfferconditionBytxt(offercontxt);
            return offers;
        }


        public async Task<IEnumerable<OffersConditionsVM>> GetOfferconditionconst(int BranchId)
        {
            var offers = await _offerpriceconditionRepository.GetOfferconditionconst(BranchId);
            return offers;
        }
    }
}

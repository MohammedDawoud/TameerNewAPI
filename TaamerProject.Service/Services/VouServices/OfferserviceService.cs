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
    public class OfferserviceService : IOfferserviceService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IOfferServiceRepository _offerServiceRepository;



        public OfferserviceService(TaamerProjectContext dataContext, ISystemAction systemAction, IOfferServiceRepository offerServiceRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _offerServiceRepository = offerServiceRepository;
        }
        public Task<IEnumerable<OfferServiceVM>> GetOfferservicenByid(int offerid)
        {
            var offers =  _offerServiceRepository.GetOfferservicenByid(offerid);
            return offers;
        }

    }
}

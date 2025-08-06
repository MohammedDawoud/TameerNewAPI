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
    public class ContractServicesService : IContractServicesService
    {

        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IContractServicesRepository _contractServicesRepository;

        public ContractServicesService(TaamerProjectContext dataContext, ISystemAction systemAction, IContractServicesRepository contractServicesRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _contractServicesRepository = contractServicesRepository;
        }
        public async Task<IEnumerable<ContractServicesVM>> GetContractservicenByid(int contractid)
        {
            var contracts =await _contractServicesRepository.GetContractservicenByid(contractid);
            return contracts;
        }
    }
}

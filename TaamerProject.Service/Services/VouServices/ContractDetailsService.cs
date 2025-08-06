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
    public class ContractDetailsService : IContractDetailsService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IContractDetailsRepository _contractDetailsRepository;



        public ContractDetailsService(TaamerProjectContext dataContext, ISystemAction systemAction, IContractDetailsRepository contractDetailsRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _contractDetailsRepository = contractDetailsRepository;
        }
        public async Task<IEnumerable<ContractDetailsVM>> GetAllEmpConDetailsByContractId(int? ContractId)
        {
            var details =  await _contractDetailsRepository.GetAllConDetailsByContractId(ContractId);
            return details;
        }
    }
}

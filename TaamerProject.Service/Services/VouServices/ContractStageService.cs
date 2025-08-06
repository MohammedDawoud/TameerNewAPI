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
    public class ContractStageService : IContractStageService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IContractStageRepository _contractStageRepository;



        public ContractStageService(TaamerProjectContext dataContext, ISystemAction systemAction,IContractStageRepository contractStageRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _contractStageRepository = contractStageRepository;
        }
        public async Task<IEnumerable<ContractStageVM>> GetAllphasesByContractId(int? ContractId)
        {
            var stage = await _contractStageRepository.GetAllphasesByContractId(ContractId);
            return stage;
        }

    }
}

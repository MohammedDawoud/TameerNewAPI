using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.Service.Services
{
    public class EmpContractDetailsService :   IEmpContractDetailsService
    {
        private readonly IEmpContractDetailRepository _EmpContractDetailRepository;
        private readonly IAccountsRepository _AccountsRepository;
        private readonly IFiscalyearsRepository _fiscalyearsRepository;
        private readonly ICostCenterRepository _CostCenterRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public EmpContractDetailsService(TaamerProjectContext dataContext
            , ISystemAction systemAction, IEmpContractDetailRepository EmpContractDetailRepository,
            IAccountsRepository AccountsRepository, IFiscalyearsRepository fiscalyearsRepository,
            ICostCenterRepository CostCenterRepository
            )
        {
            _EmpContractDetailRepository = EmpContractDetailRepository;
            _AccountsRepository = AccountsRepository;
            _fiscalyearsRepository = fiscalyearsRepository;
            _CostCenterRepository = CostCenterRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task<IEnumerable<EmpContractDetailVM>> GetAllEmpConDetailsByContractId(int? ContractId)
        {
            var details = _EmpContractDetailRepository.GetAllEmpConDetailsByContractId(ContractId);
            return details; 
        }
       
    }
}

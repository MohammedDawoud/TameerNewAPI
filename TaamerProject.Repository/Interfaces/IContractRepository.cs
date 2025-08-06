using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IContractRepository 
    {
        Contracts GetById(int contractid);
        Task<IEnumerable<ContractsVM>> GetAllContracts();
        Task<IEnumerable<ContractsVM>> GetAllContracts_B(int BranchId,int yearid);
        Task<IEnumerable<ContractsVM>> GetAllContractsBySearch(ContractsVM contractsVM, int BranchId, int YearId);
        Task<IEnumerable<ContractsVM>> GetAllContractsBySearchCustomer(ContractsVM contractsVM, int BranchId, int YearId);

        Task<IEnumerable<ContractsVM>> GetAllCustHaveRemainingMoney(CustomerVM CustomersSearch, string lang, int BranchId);
        Task<int> GetMaxId();
       
        Task<int> GenerateNextContractNumber(int BranchId, string codePrefix);
        Task<int> GenerateNextContractNumber2(int BranchId, string codePrefix);

        Task<int> GenerateNextContractNumber3(int BranchId, string codePrefix);

        Task<int> GenerateNextContractNumber4(int BranchId, string codePrefix);
        Task<int> GenerateNextContractNumber5(int BranchId, string codePrefix);

        Task<IEnumerable<ContractsVM>> GetContractById(int contractid);
        Task<IEnumerable<ContractsVM>> GetAllContracts_BSearch(ContractsVM contractsVM, int BranchId, int yearid);
        Task<IEnumerable<ContractsVM>> GetAllContracts_BSearchCustomer(ContractsVM contractsVM, int BranchId, int yearid);

    }
}

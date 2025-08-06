using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Service.Services;

namespace TaamerProject.Service.Interfaces
{
    public interface IContractService  
    {
        Task<IEnumerable<ContractsVM>> GetAllContracts();
        Task<IEnumerable<ContractsVM>> GetAllContracts_B(int BranchId, int? yearid);
        Task<IEnumerable<ContractsVM>> GetAllContractsBySearch(ContractsVM contractsVM, int BranchId, int? yearid);
        Task<IEnumerable<ContractsVM>> GetAllContractsBySearchCustomer(ContractsVM contractsVM, int BranchId, int? yearid);

        GeneralMessage SaveContract(Contracts contract, int UserId, int BranchId, int? yearid);
        GeneralMessage EditContract(Contracts contract, int UserId, int BranchId, int? yearid);

        GeneralMessage SaveContract2(Contracts contract, int UserId, int BranchId, int? yearid);

        GeneralMessage SaveContractFile(int contractid, int UserId, int BranchId, string Url);
        GeneralMessage SaveContractFileExtra(int contractid, int UserId, int BranchId, string Url);

        GeneralMessage CancelContract(int ContractId, int UserId, int BranchId);
        string ConvertDateCalendar(DateTime DateConv, string Calendar, string DateLangCulture);
        List<CustomerPaymentsVM> GenerateCustomerPayments(ContractsVM contractsVM, int OrgId);

        string GenerateContractNumber(int BranchId);
        string GenerateContractNumber2(int BranchId);

        string GenerateContractNumber3(int BranchId);
        string GenerateContractNumber4(int BranchId);
        string GenerateContractNumber5(int BranchId);

        Task<IEnumerable<ContractsVM>> GetContractbyid(int contractid);

        GeneralMessage EditContractService(Contracts contract, int UserId, int BranchId, int? yearid);
        HijriDateFormat ConvertDateCalendar3(DateTime DateConv, string Calendar, string DateLangCulture);
    }
}

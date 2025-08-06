using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using System.Data;

namespace TaamerProject.Service.Interfaces
{
    public interface IEmpContractService  
    {
        Task<IEnumerable<EmpContractVM>> GetAllEmpContract( string lang, int BranchId);


        Task<IEnumerable<EmpContractVM>> GetAllEmpContract(string lang,int SearchAll, int BranchId);
        Task<IEnumerable<EmpContractVM>> GetAllEmpContractSearch(EmpContractVM SalarySearch, string lang, int BranchId);
        GeneralMessage SaveEmpContract(EmpContract data, int UserId, int BranchId, int? Year, string lang);
        GeneralMessage BeginNewEmployeeWork(EmpContract data, int User,int BranchId);
        GeneralMessage EndWorkforAnEmployee(EmpContract data, string Reason, string Duration, int User, string Lang, int BranchId);
        GeneralMessage DeleteEmpContract(int EmpId, int UserId,int BranchId);

        Task<int> GenerateNextEmpContractNumber(int BranchId);

        Task<IEnumerable<EmpContractDetailVM>> GetAllDetailsByContractId(int? ContractId);
        //heba
        Task<DataTable> GetAllContractDetailsByContractId(int? ContractId, string Con);
        Task<IEnumerable<EmpContractVM>> GetLastEmpContractSearch(int contractid, string lang);
        rptEmpEndWorkVM GetEmpdatatoendwork(EmpContract data, int User, string Lang, int BranchId);

        GeneralMessage EndWorkforAnEmployeeQuaContract(int EmpId, string Reason, string Duration, int User, string Lang, int BranchId);
        Task<EmpContractVM> GetEMployeeContractByEmp(int EmpId);
    }
}

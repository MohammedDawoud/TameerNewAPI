using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IEmpContractRepository 
    {
        Task<IEnumerable<EmpContractVM>> GetAllEmpContract(string lang,int BranchId);
        //IEnumerable<EmployeesVM> FillAllEmployees(string lang, int BranchId);
        Task<IEnumerable<EmpContractVM>> GetAllBranchEmpContract(string lang);
        Task<IEnumerable<EmpContractVM>> GetAllEmpContract(string lang, int SearchAll, int BranchId);
        Task<IEnumerable<EmpContractVM>> GetAllEmpContractSearch(string lang, int BranchId);
        Task<IEnumerable<EmpContractVM>> GetAllEmpContractBySearchObject(EmpContractVM SalarySearch, string lang, int BranchId);
        //IEnumerable<EmpContractVM> GetAllUsersEmpContract();

        Task<EmpContractVM> GetEmpContractById(int EmpId,string lang);
        Task<IEnumerable<EmpContractVM>> SearchEmpContract(EmpContractVM EmployeesSearch,string lang, int BranchId);

        Task<int> GenerateNextEmpContractNumber(int BranchId);
        Task<int> GenerateNextQuaEmpContractNumber(int BranchId);
        Task<IEnumerable<EmpContractVM>> GetEmpcoById(int contractid, string lang);
        Task<EmpContractVM> GetEMployeeContractByEmp(int EmpId);




    }
}

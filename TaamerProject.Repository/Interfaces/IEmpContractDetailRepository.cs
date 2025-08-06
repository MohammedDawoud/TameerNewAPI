using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IEmpContractDetailRepository
    {

        Task<IEnumerable<EmpContractDetailVM>> GetAllEmpConDetailsByContractId(int? ContractId);

        Task<IEnumerable<EmpContractDetailVM>> GetAllDetailsByContractId(int? ContractId);
        //heba
        Task<DataTable> GetAllContractDetailsByContractId(int? ContractId, string Con);
    }
}

using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface ICustodyRepository : IRepository<Custody>
    {
        Task<IEnumerable<CustodyVM>> GetAllCustody(int BranchId);
        Task<IEnumerable<CustodyVM>> GetDistinctCustody(int BranchId);
        

        Task<IEnumerable<CustodyVM>> GetSomeCustody(int BranchId,bool Status);
        Task<IEnumerable<CustodyVM>> GetSomeCustodyVoucher(int BranchId, bool Status);


        Task<EmployeesVM> GetEmployeeByItemId(int Item, int BranchId);
        Task<IEnumerable<CustodyVM>> SearchCustody(CustodyVM CustodySearch, string lang, int BranchId);
        Task<IEnumerable<CustodyVM>> SearchCustodyVoucher(CustodyVM CustodySearch, string lang, int BranchId);
        Task<IEnumerable<CustodyVM>> GetSomeCustodyByEmployeeId(int EmployeeId, bool Status);
        Task<IEnumerable<CustodyVM>> GetCustodiesByEmployeeId(int EmployeeId);
    }
}

using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface ICustomerPaymentsRepository : IRepository<CustomerPayments>
    {
        Task<IEnumerable<CustomerPaymentsVM>> GetAllCustomerPayments(int ContractId);
        Task<IEnumerable<CustomerPaymentsVM>> GetAllCustomerPaymentsIsNotCanceled(int ContractId);

        Task<IEnumerable<CustomerPaymentsVM>> GetAllCustomerPaymentsbyofferis(int offerid);
        Task<IEnumerable<CustomerPaymentsVM>> GetAllCustomerPaymentsbyamounttxt(decimal amount, string amounttxt);
        Task<IEnumerable<CustomerPaymentsVM>> GetAllCustomerPaymentsconst(int BranchId);
    }
}

using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IServicesRepository
    {
        Task<IEnumerable<ServicesVM>> GetAllServices(int BranchId);
        Task<int> GetInvoicesAndServicesCount(int BranchId);
        Task<IEnumerable<ServicesVM>> GetServicesSearch(ServicesVM ServiceSearch, int BranchId);
        Task<int?> GenerateNextServicesNumber(int BranchId);
    }
}

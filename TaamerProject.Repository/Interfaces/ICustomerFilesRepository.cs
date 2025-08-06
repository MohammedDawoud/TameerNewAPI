using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface ICustomerFilesRepository : IRepository<CustomerFiles>
    {
        Task<IEnumerable<CustomerFilesVM>> GetAllCustomerFilesUploaded(int CustomerId,string SearchText);
        Task<IEnumerable<CustomerFilesVM>> GetAllCustomerFiles(int? CustomerId);
    }
}

using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IAcc_SuppliersRepository : IRepository<Acc_Suppliers>
    {
        Task<IEnumerable<Acc_SuppliersVM>> GetAllSuppliers(string SearchText, int BranchId, int? yearid);
        Task<Acc_SuppliersVM> GetSupplierByID(int SupplierId);


    }
}

using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface ICurrencyRepository 
    {
        Task<IEnumerable<CurrencyVM>> GetAllCurrency(int BranchId);
        Task<IEnumerable<CurrencyVM>> GetAllCurrency2(string SearchText);

        Task<int> GenerateCurrencyNumber(int BranchId);
    }
}

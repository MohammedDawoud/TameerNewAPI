using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ICurrencyService
    {
        Task<IEnumerable<CurrencyVM>> GetAllCurrency(int BranchId);
        Task<IEnumerable<CurrencyVM>> GetAllCurrency2(string SearchText);

        GeneralMessage SaveCurrency(Currency currency, int UserId, int BranchId);
        GeneralMessage DeleteCurrency(int CurrencyId, int UserId, int BranchId);
        Task<int> GenerateCurrencyNumber(int BranchId);
    }
}

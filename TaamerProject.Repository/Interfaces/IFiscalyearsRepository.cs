using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IFiscalyearsRepository : IRepository<FiscalYears>
    {
        Task<IEnumerable<FiscalyearsVM>> GetAllFiscalyears();
        int CheckYearExist(int? yearId);
        int GetYearID(int FiscalId);
        Task<FiscalYears> GetCurrentYear();
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IFiscalyearsService
    {
        Task<IEnumerable<FiscalyearsVM>> GetAllFiscalyears();
        int CheckYearExist(int? yearId);
        int GetYearID(int FiscalId);
        GeneralMessage SaveFiscalyears(FiscalYears fiscalyears, int UserId, int BranchId);

        GeneralMessage ActivateFiscalYear(int FiscalId, int SystemSettingId, int UserId, int BranchId);

        GeneralMessage DeleteFiscalyears(int FiscalId, int UserId, int BranchId);
        FiscalyearsVM GetActiveYear();
        FiscalyearsVM GetActiveYearID(int FiscalId);
    }
}

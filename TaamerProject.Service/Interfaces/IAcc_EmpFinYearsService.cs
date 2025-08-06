using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IAcc_EmpFinYearsService
    {
        Task<GeneralMessage> SaveFiscalyearsPriv(Acc_EmpFinYears fiscalyearsPriv, int UserId, int BranchId);
        Task<int> CheckPriv(int? EmpID_P, int? BranchID_P, int? YearID_P);
        Task<IEnumerable<Acc_EmpFinYearsVM>> GetAllFiscalyearsPriv();
        GeneralMessage DeleteFiscalyearsPriv(int ID, int UserId, int BranchId);

        Task<IEnumerable<Acc_EmpFinYearsVM>> GetAllBranchesByUserId(int UserId);
        GeneralMessage AccountRecycle(int YearID, string Con, int UserId, int BranchId);
        GeneralMessage AccountRecycleDeleteYear(int YearID, string Con, int UserId, int BranchId);
        int AccountRecycleCheckYear(int YearID, string Con, int UserId, int BranchId);

        Task<IEnumerable<Acc_EmpFinYearsVM>> FillYearByUserIdandBranchSelect(int UserId, int? BranchID);
    }
}

using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IAcc_EmpFinYearsRepository
    {
        Task<IEnumerable<Acc_EmpFinYearsVM>> GetAllFiscalyearsPriv();
        Task<int> CheckPriv(int? EmpID_P, int? BranchID_P, int? YearID_P);

        Task<IEnumerable<Acc_EmpFinYearsVM>> GetAllBranchesByUserId(int UserId);
        Task<IEnumerable<Acc_EmpFinYearsVM>> FillYearByUserIdandBranchSelect(int UserId,int? BranchID);
        Task<IEnumerable<Acc_EmpFinYearsVM>> FillYearByUserIdandBranchSelect_W_Branch(int UserId);


    }
}
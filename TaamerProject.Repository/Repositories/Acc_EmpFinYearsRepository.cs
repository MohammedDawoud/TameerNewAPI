
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using System.Data;
using System.Data.SqlClient;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class Acc_EmpFinYearsRepository : IAcc_EmpFinYearsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Acc_EmpFinYearsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        public async Task<IEnumerable<Acc_EmpFinYearsVM>> GetAllFiscalyearsPriv()
        {
            var fiscalyears = _TaamerProContext.Acc_EmpFinYears.Where(s => s.IsDeleted == false).Select(x => new Acc_EmpFinYearsVM
            {
                Acc_EmpFinYearID = x.Acc_EmpFinYearID,
                EmpID = x.EmpID,
                BranchID = x.BranchID,
                YearID = x.YearID,
                Username = x.user.FullName,
                Userjob = x.user.Jobs.JobNameAr,
                Branchname = x.branch.NameAr,
                YearValue = x.fiscalYears.YearId,

            }).ToList();
            return fiscalyears;
        }


        public async Task<int> CheckPriv(int? EmpID_P, int? BranchID_P, int? YearID_P)
        {
            var fiscalyears = _TaamerProContext.Acc_EmpFinYears.Where(s => s.IsDeleted == false && (BranchID_P == s.BranchID) && (EmpID_P == s.EmpID) && (YearID_P == s.YearID)).Count();
            return fiscalyears;
        }


        public async Task<IEnumerable<Acc_EmpFinYearsVM>> GetAllBranchesByUserId(int UserId)
        {
            var branches = _TaamerProContext.Acc_EmpFinYears.Where(s => s.EmpID == UserId && s.IsDeleted == false).Select(x => new Acc_EmpFinYearsVM
            {

                Branchname = x.branch.NameAr,
                BranchID = x.branch.BranchId,
            }).ToList();
            return branches;
        }
        public async Task<IEnumerable<Acc_EmpFinYearsVM>> FillYearByUserIdandBranchSelect(int UserId,int? BranchID)
        {
            var Years = _TaamerProContext.Acc_EmpFinYears.Where(s => s.EmpID == UserId && s.BranchID == BranchID && s.IsDeleted == false).Select(x => new Acc_EmpFinYearsVM
            {

                YearID = x.fiscalYears.YearId,
                YearValue = x.fiscalYears.FiscalId,
            }).ToList();
            return Years;
        }
        public async Task<IEnumerable<Acc_EmpFinYearsVM>> FillYearByUserIdandBranchSelect_W_Branch(int UserId)
        {
            var Years = _TaamerProContext.Acc_EmpFinYears.Where(s => s.EmpID == UserId && s.IsDeleted == false).Select(x => new Acc_EmpFinYearsVM
            {

                YearID = x.fiscalYears.YearId,
                YearValue = x.fiscalYears.FiscalId,
            }).ToList();
            return Years;
        }

    }
}
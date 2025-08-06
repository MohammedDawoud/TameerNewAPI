using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace TaamerProject.Repository.Repositories
{
    public class FiscalyearsRepository : IFiscalyearsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public FiscalyearsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        
        public async Task< IEnumerable<FiscalyearsVM>> GetAllFiscalyears()
        {
            var fiscalyears = _TaamerProContext.FiscalYears.Where(s => s.IsDeleted == false).Select(x => new FiscalyearsVM
            {
                FiscalId = x.FiscalId,
                YearId = x.YearId,
                YearName = x.YearName,
                BranchId = x.BranchId,
                UserId = x.UserId,
                IsActive = x.IsActive,
            }).ToList();
            return fiscalyears;
        }
        public async Task< FiscalYears> GetCurrentYear()
        {
            var year = _TaamerProContext.FiscalYears.Where(s => s.IsDeleted == false && s.IsActive == true).FirstOrDefault();
            return year;
        }
        public int CheckYearExist(int? yearId)
        {
            var year = _TaamerProContext.FiscalYears.Where(s => s.IsDeleted == false && s.YearId==yearId).Count();
            return year;
        }
        public int GetYearID(int FiscalId)
        {
            var year = _TaamerProContext.FiscalYears.Where(s => s.IsDeleted == false && s.FiscalId == FiscalId).FirstOrDefault();
            return year.YearId ?? 0;
        }

        public IEnumerable<FiscalYears> GetAll()
        {
            throw new NotImplementedException();
        }

        public FiscalYears GetById(int Id)
        {
            return _TaamerProContext.FiscalYears.Where(x => x.FiscalId == Id).FirstOrDefault();
        }

        public IEnumerable<FiscalYears> GetMatching(Func<FiscalYears, bool> where)
        {
            return _TaamerProContext.FiscalYears.Where(where).ToList<FiscalYears>();
        }
    }
}

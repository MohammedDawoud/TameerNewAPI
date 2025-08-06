using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class ChecksRepository : IChecksRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ChecksRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        public async Task<IEnumerable<ChecksVM>> GetAllChecks(int? Type,int BranchId)
        {
            var checks = _TaamerProContext.Checks.Where(s => s.IsDeleted == false && s.Type == Type && s.BranchId == BranchId).Select(x => new ChecksVM
            {
                CheckId = x.CheckId,
                CheckNumber = x.CheckNumber,
                Type  = x.Type,
                Date = x.Date,
                HijriDate = x.HijriDate,
                ActionDate = x.ActionDate,
                Amount = x.Amount,
                TotalAmont = x.TotalAmont,
                BanksName = x.Banks.NameAr,

                BankId=x.BankId,
                UserId = x.UserId,
                BranchId = x.BranchId,
                BeneficiaryName = x.BeneficiaryName,
                TypeName = x.Type == 1 ? "  تحت التحصيل " : x.Type == 2 ? " صادرة " : "" ,
                ReceivedName = x.ReceivedName,
                IsFinished = x.IsFinished,
                Notes = x.Notes,
            }).ToList();
            return checks;
        }

        public async Task<IEnumerable<ChecksVM>> GetAllCheckSearch(ChecksVM CheckSearch,  int BranchId)
        {
            var checks = _TaamerProContext.Checks.Where(s => s.IsDeleted == false &&  s.BranchId == BranchId
              && (s.Type == CheckSearch.Type )           
              && (s.CheckNumber == CheckSearch.CheckNumber || CheckSearch.CheckNumber == null)
                                                                 && (s.BeneficiaryName == CheckSearch.BeneficiaryName || CheckSearch.BeneficiaryName == null)
                                                                    && (s.Amount == CheckSearch.Amount || CheckSearch.Amount == null)
                                                                 && (s.Date == CheckSearch.Date || CheckSearch.Date == null)
                                                              )
                
                .Select(x => new ChecksVM
            {
                CheckId = x.CheckId,
                CheckNumber = x.CheckNumber,
                Type = x.Type,
                Date = x.Date,
                HijriDate = x.HijriDate,
                ActionDate = x.ActionDate,
                Amount = x.Amount,
                TotalAmont = x.TotalAmont,
                    BanksName = x.Banks.NameAr,
                    BankId = x.BankId,
                    UserId = x.UserId,
                BranchId = x.BranchId,
                BeneficiaryName = x.BeneficiaryName,
                TypeName = x.Type == 1 ? "  تحت التحصيل " : x.Type == 2 ? " صادرة " : "",
                ReceivedName = x.ReceivedName,
                IsFinished = x.IsFinished,
                Notes = x.Notes,
            }).ToList();
            return checks;
        }


        public async Task<int> GetUnderCollectionCount(int BranchId)
        {
            return _TaamerProContext.Checks.Where(s => s.IsDeleted == false && s.Type == 1 && s.BranchId == BranchId).Count();
        }
        public async Task<int> GetExportsCount(int BranchId)
        {
            return _TaamerProContext.Checks.Where(s => s.IsDeleted == false && s.Type == 2 && s.BranchId == BranchId).Count();
        }
    }
}



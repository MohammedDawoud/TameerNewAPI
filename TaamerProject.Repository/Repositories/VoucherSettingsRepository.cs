using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class VoucherSettingsRepository : IVoucherSettingsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public VoucherSettingsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<VoucherSettingsVM>> GetAllVoucherSettings(int BranchId)
        {
            var jobs = _TaamerProContext.VoucherSettings.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new VoucherSettingsVM
            {
                SettingId = x.SettingId,
                AccountId = x.AccountId,
                Type = x.Type,
                AccountName = x.Accounts.NameEn,
                TypeName = x.Type == 5 ? "" : "",
                }).ToList();
                return jobs;
        }
        public async Task<List<int>> GetAccountIdsByType (int Type, int BranchId)
        {
            var AccountIds = new List<int>();
            var settings = _TaamerProContext.VoucherSettings.Where(s => s.Type == Type && s.IsDeleted == false && s.BranchId == BranchId).ToList();
            if (settings != null && settings.Count > 0)
            {
                AccountIds = settings.Select(s => s.AccountId).ToList();
            }
            return AccountIds;
        }
    }
}



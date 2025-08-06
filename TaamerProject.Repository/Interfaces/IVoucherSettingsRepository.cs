using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IVoucherSettingsRepository
    {
        Task<IEnumerable<VoucherSettingsVM>> GetAllVoucherSettings(int BranchId);
        Task<List<int>> GetAccountIdsByType(int Type, int BranchId);
    }
}

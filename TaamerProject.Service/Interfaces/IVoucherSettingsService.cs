using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IVoucherSettingsService 
    {
        Task<IEnumerable<VoucherSettingsVM>> GetAllVoucherSettings( int BranchId);
        GeneralMessage SaveVoucherSettings(int Type, List<int> AccountIds, int UserId, int BranchId);
        GeneralMessage DeleteVoucherSettings(int settingId, int UserId, int BranchId);
        Task<List<int>> GetAccountIdsByType(int Type, int BranchId);
    }
}

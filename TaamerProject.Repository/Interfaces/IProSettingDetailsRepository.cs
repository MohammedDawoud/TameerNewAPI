
using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IProSettingDetailsRepository
    {
        Task<ProSettingDetailsVM> CheckProSettingData(int ProjectTypeId, int ProjectSubTypeId, int BranchId);
        Task<ProSettingDetailsVM> CheckProSettingData2( int? ProjectSubTypeId, int BranchId);

        Task<int> CheckProSettingNo(ProSettingDetails proSettingDetails);
        Task<IEnumerable<ProSettingDetailsVM>> FillProSettingNo();
        Task<int> GenerateNextProSettingNumber();
        Task<ProSettingDetailsVM> GetProSettingById(int ProSettingId);
    }
}

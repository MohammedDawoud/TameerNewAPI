
using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IProSettingDetailsNewRepository
    {
        Task<ProSettingDetailsNewVM> CheckProSettingData(int ProjectTypeId, int ProjectSubTypeId, int BranchId); 
        Task<ProSettingDetailsNewVM> CheckProSettingData2( int? ProjectSubTypeId, int BranchId);

        Task<int> CheckProSettingNo(ProSettingDetailsNew proSettingDetails);
        Task<IEnumerable<ProSettingDetailsNewVM>> FillProSettingNo();
        Task<int> GenerateNextProSettingNumber();
        Task<ProSettingDetailsNewVM> GetProSettingById(int ProSettingId);
    }
}

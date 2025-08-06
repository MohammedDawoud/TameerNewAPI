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
    public class AttDeviceSettingRepository :IAttDevicesettingRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public AttDeviceSettingRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<AttDeviceSittingVM> GetAttDevicesetting(int BranchId)
        {
            var AttDevicesetting = _TaamerProContext.AttDeviceSitting.Where(s => s.IsDeleted == false).Select(x => new AttDeviceSittingVM
            {
                AttDeviceSittingId = x.AttDeviceSittingId,
                ArgCompanyCode = x.ArgCompanyCode,
                ArgDeviceName  = x.ArgDeviceName,
                ArgEmpPassowrd = x.ArgEmpPassowrd,
                ArgEmpUsername = x.ArgEmpUsername,
            }).FirstOrDefault();
            return AttDevicesetting;
        }
    }
}

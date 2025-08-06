using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IAttDeviceService
    {
        Task<AttDeviceSittingVM> GetDevicesetting(int BranchId);
        GeneralMessage SaveAttdeviceSetting(AttDeviceSitting attdeviceseting, int UserId, int BranchId);
    }
}

using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IDeviceAttRepository 
    {

        Task<IEnumerable<DeviceAttVM>> GetAllDeviceAttByID(string lang, int BranchId,int Id);
        
      
    }
}

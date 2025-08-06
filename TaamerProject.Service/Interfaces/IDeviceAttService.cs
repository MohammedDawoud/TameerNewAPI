using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IDeviceAttService  
    {
       
        Task<IEnumerable<DeviceAttVM>> GetAllDeviceAttByID(string lang, int BranchId,int Id);
        
    
        GeneralMessage SaveDeviceAtt(DeviceAtt attendencedevice, int UserId, int BranchId);
      
    }
}

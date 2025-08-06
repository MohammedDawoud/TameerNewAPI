using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class DeviceAttRepository : IDeviceAttRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public DeviceAttRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<DeviceAttVM>> GetAllDeviceAttByID(string lang, int BranchId,int Id)
        {
            var attendencedevice = _TaamerProContext.DeviceAtt.Where(s => s.IsDeleted == false &&  s.Id==Id).Select(x => new DeviceAttVM
            {
                Id = x.Id,
                DeviceId = x.DeviceId,
                LastUpdate = x.LastUpdate,
               

            });
            return attendencedevice;
        }

      
      
    }
}

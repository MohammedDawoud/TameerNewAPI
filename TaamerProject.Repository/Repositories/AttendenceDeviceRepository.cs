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
    public class AttendenceDeviceRepository : IAttendenceDeviceRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public AttendenceDeviceRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IQueryable<AttendenceDeviceVM>> GetAllAttendenceDevices(string lang,int BranchId)
        {
            var attendencedevice = _TaamerProContext.AttendenceDevice.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new AttendenceDeviceVM
            {
                AttendenceDeviceId = x.AttendenceDeviceId,
                DeviceIP = x.DeviceIP,
                Port = x.Port,
                MachineNumber = x.MachineNumber,
                BranchId = x.BranchId,
                BranchName = x.BranchName.NameAr,
                LastUpdate = x.LastUpdate

            });
           return attendencedevice;
        }

        public async Task<IEnumerable<AttendenceDeviceVM>> GetAllAttendenceDevicesByID(string lang, int BranchId,int Id)
        {
            var attendencedevice = _TaamerProContext.AttendenceDevice.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AttendenceDeviceId==Id).Select(x => new AttendenceDeviceVM
            {
                AttendenceDeviceId = x.AttendenceDeviceId,
                DeviceIP = x.DeviceIP,
                Port = x.Port,
                MachineNumber = x.MachineNumber,
                BranchName = x.BranchName.NameAr,
                LastUpdate = x.LastUpdate

            });
            return attendencedevice;
        }
        public async Task<IEnumerable<AttendenceDeviceVM>> GetAllDevicesByID(string lang,  int Id)
        {
            var attendencedevice = _TaamerProContext.AttendenceDevice.Where(s => s.IsDeleted == false  && s.AttendenceDeviceId == Id).Select(x => new AttendenceDeviceVM
            {
                AttendenceDeviceId = x.AttendenceDeviceId,
                DeviceIP = x.DeviceIP,
                Port = x.Port,
                MachineNumber = x.MachineNumber,
                BranchName = x.BranchName.NameAr,
                LastUpdate=x.LastUpdate

            });
            return attendencedevice;
        }

        public async Task<IEnumerable<AttendenceDeviceVM>> GetAllAttendenceDevicesSearchObject(AttendenceDeviceVM Search, int BranchId)
        {

            try
            {
               

        var Devices = _TaamerProContext.AttendenceDevice.Where(s => s.IsDeleted == false     ).Select(x => new AttendenceDeviceVM
                   {
                       AttendenceDeviceId = x.AttendenceDeviceId,
                       DeviceIP = x.DeviceIP,
                       Port = x.Port,
                       MachineNumber = x.MachineNumber,
                       BranchId=x.BranchId,
                       BranchName = x.BranchName.NameAr,
                   }).Select(s => new AttendenceDeviceVM
                   {
                       AttendenceDeviceId = s.AttendenceDeviceId,
                       DeviceIP = s.DeviceIP,
                       Port = s.Port,
                       MachineNumber = s.MachineNumber,
                       BranchId = s.BranchId,
                       BranchName = s.BranchName,
                       
                   }).ToList();
                if (!String.IsNullOrWhiteSpace(Convert.ToString(Search.BranchId)) && Search.BranchId>0)
                {
                    Devices = Devices.Where(w=>w.BranchId == Search.BranchId).ToList();
            
                }
                if (!String.IsNullOrWhiteSpace(Convert.ToString(Search.DeviceIP)))
                {
                    Devices = Devices.Where(w => w.DeviceIP == Search.DeviceIP).ToList();

                }
                if (!String.IsNullOrWhiteSpace(Convert.ToString(Search.MachineNumber)))
                {
                    Devices = Devices.Where(w => w.MachineNumber == Search.MachineNumber).ToList();

                }




//                 && (s.DeviceIP == Search.DeviceIP || Search.DeviceIP == null) &&
//               (s.MachineNumber == Search.MachineNumber || Search.MachineNumber == null)
//              ).Select(x => new AttendenceDeviceVM
//{
//    AttendenceDeviceId = x.AttendenceDeviceId,
//    DeviceIP = x.DeviceIP,
//    Port = x.Port,
//    MachineNumber = x.MachineNumber,
//    BranchName = x.BranchName.NameAr,
//}).Select(s => new AttendenceDeviceVM
//{
//    AttendenceDeviceId = s.AttendenceDeviceId,
//    DeviceIP = s.DeviceIP,
//    Port = s.Port,
//    MachineNumber = s.MachineNumber,
//    BranchName = s.BranchName,

//}).ToList();


                return Devices;
               
            }
            catch 
            {
                var Device = _TaamerProContext.AttendenceDevice.Where(s => s.IsDeleted == false && s.BranchId == BranchId ).Select(x => new AttendenceDeviceVM
               {
                   AttendenceDeviceId = x.AttendenceDeviceId,
                   DeviceIP = x.DeviceIP,
                   Port = x.Port,
                   MachineNumber = x.MachineNumber,
                   BranchId = x.BranchId,
                    BranchName = x.BranchName.NameAr,
                });
                return Device;
            }

        }
        public async Task<IQueryable<AttendenceDeviceVM>> SearchAttendenceDevices(AttendenceDeviceVM AttendenceDevicesSearch, string lang, int BranchId)
        {
            var AttendenceDevices = _TaamerProContext.AttendenceDevice.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.DeviceIP == AttendenceDevicesSearch.DeviceIP || s.DeviceIP.Contains(AttendenceDevicesSearch.DeviceIP) || AttendenceDevicesSearch.DeviceIP == null) &&
                                                (s.Port == AttendenceDevicesSearch.Port || s.Port.Contains(AttendenceDevicesSearch.Port) || AttendenceDevicesSearch.Port == null) &&
                                                (s.MachineNumber == AttendenceDevicesSearch.MachineNumber || s.MachineNumber.Contains(AttendenceDevicesSearch.MachineNumber) || AttendenceDevicesSearch.MachineNumber == null))
                                                .Select(x => new AttendenceDeviceVM
                                                {
                                                    DeviceIP = x.DeviceIP,
                                                    Port = x.Port,
                                                    MachineNumber = x.MachineNumber,
                                                });
            return AttendenceDevices; ;
        }
        public async Task<AttendenceDeviceVM> SearchAttendenceDevicesById(string attendencedeviceId, string lang)
        {
            return _TaamerProContext.AttendenceDevice.Where(s => s.IsDeleted == false && s.DeviceIP == attendencedeviceId)
                                                .Select(x => new AttendenceDeviceVM
                                                {
                                                    DeviceIP = x.DeviceIP,
                                                    Port = x.Port,
                                                    MachineNumber = x.MachineNumber,
                                                }).FirstOrDefault();
            //return AttendenceDevices;
        }
    }
}

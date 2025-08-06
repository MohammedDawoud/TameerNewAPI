using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IAttendenceDeviceService
    {
        Task<IQueryable<AttendenceDeviceVM>> GetAllAttendenceDevices(string lang, int BranchId);
        Task<IEnumerable<AttendenceDeviceVM>> GetAllAttendenceDevicesByID(string lang, int BranchId, int Id);
        Task<IEnumerable<AttendenceDeviceVM>> GetAllDevicesByID(string lang, int Id);

        Task<IEnumerable<AttendenceDeviceVM>> GetAllAttendenceDevicesSearch(AttendenceDeviceVM Search, string lang, int BranchId);
        GeneralMessage SaveAttendenceDevice(AttendenceDevice attendencedevice, int UserId, int BranchId);
        GeneralMessage SaveDevice(List<AttendenceDevice> attendencedevice, int UserId, int BranchId);
        GeneralMessage DeleteAttendenceDevice(int attendencedeviceId, int UserId, int BranchId);
        Task<IEnumerable<AttendenceDeviceVM>> SearchAttendenceDevices(AttendenceDeviceVM AttendenceDevicesSearch, string lang, int BranchId);
        Task<AttendenceDeviceVM> SearchAttendenceDevicesById(string attendencedeviceId, string lang);
    }
}

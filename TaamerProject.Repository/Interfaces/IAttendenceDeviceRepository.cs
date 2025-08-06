using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IAttendenceDeviceRepository
    {
        Task<IQueryable<AttendenceDeviceVM>> GetAllAttendenceDevices(string lang, int BranchId);
        Task<IEnumerable<AttendenceDeviceVM>> GetAllAttendenceDevicesByID(string lang, int BranchId,int Id);
        Task<IEnumerable<AttendenceDeviceVM>> GetAllDevicesByID(string lang, int Id);

        Task<IQueryable<AttendenceDeviceVM>> SearchAttendenceDevices(AttendenceDeviceVM AttendenceDevicesSearch, string lang, int BranchId);
        Task<AttendenceDeviceVM> SearchAttendenceDevicesById(string attendencedeviceId, string lang);
        Task<IEnumerable<AttendenceDeviceVM>> GetAllAttendenceDevicesSearchObject(AttendenceDeviceVM Search, int BranchId);
    }
}

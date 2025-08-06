using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Repository.Repositories;
using System.Net;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class AttendenceDeviceService : IAttendenceDeviceService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IAttendenceDeviceRepository _AttendenceDeviceRepository;

        public AttendenceDeviceService(TaamerProjectContext dataContext, ISystemAction systemAction, IAttendenceDeviceRepository attendenceDevice)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _AttendenceDeviceRepository = attendenceDevice;

        }
        public async Task<IQueryable<AttendenceDeviceVM>> GetAllAttendenceDevices(string lang, int BranchId)
        {
            var attendencedevices =await _AttendenceDeviceRepository.GetAllAttendenceDevices(lang, BranchId);
            return attendencedevices;
        }

        public async Task<IEnumerable<AttendenceDeviceVM>> GetAllAttendenceDevicesByID(string lang, int BranchId, int Id)
        {
            var attendencedevices = await _AttendenceDeviceRepository.GetAllAttendenceDevicesByID(lang, BranchId, Id);
            return attendencedevices;
        }

        public async Task<IEnumerable<AttendenceDeviceVM>> GetAllDevicesByID(string lang, int Id)
        {
            var attendencedevices = await _AttendenceDeviceRepository.GetAllDevicesByID(lang, Id);
            return attendencedevices;
        }

        public async Task<IEnumerable<AttendenceDeviceVM>> GetAllAttendenceDevicesSearch(AttendenceDeviceVM Search, string lang, int BranchId)
        {
            if ((bool)Search.IsSearch)
            {
                return await _AttendenceDeviceRepository.GetAllAttendenceDevicesSearchObject(Search, BranchId);
            }
            else
            {
                return await _AttendenceDeviceRepository.GetAllAttendenceDevices(lang, BranchId);
            }
        }


        public GeneralMessage SaveAttendenceDevice(AttendenceDevice attendencedevice, int UserId, int BranchId)
        {
            try
            {
                if (attendencedevice.AttendenceDeviceId == 0)
                {
                    attendencedevice.BranchId = BranchId;
                    attendencedevice.AddUser = UserId;
                    attendencedevice.AddDate = DateTime.Now;
                    attendencedevice.LastUpdate = DateTime.Now.AddYears(-1); ;
                    // attendencedevice.UpdatedDate = DateTime.Now.AddYears(-1);
                    _TaamerProContext.AttendenceDevice.Add(attendencedevice);
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة جهاز بصمة جديد";
                   _SystemAction.SaveAction("SaveAttendenceDevice", "AttendenceDeviceService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var attendencedeviceUpdated = _TaamerProContext.AttendenceDevice.Where(x=>x.AttendenceDeviceId==attendencedevice.AttendenceDeviceId).FirstOrDefault();
                    if (attendencedeviceUpdated != null)
                    {

                        attendencedeviceUpdated.DeviceIP = attendencedevice.DeviceIP;
                        attendencedeviceUpdated.Port = attendencedevice.Port;
                        attendencedeviceUpdated.MachineNumber = attendencedevice.MachineNumber;
                        attendencedeviceUpdated.BranchId = attendencedevice.BranchId;
                        attendencedeviceUpdated.UpdateUser = UserId;
                        attendencedeviceUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل جهاز بصمة رقم " + attendencedevice.AttendenceDeviceId;
                    _SystemAction.SaveAction("SaveAttendenceDevice", "AttendenceDeviceService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ جهاز البصمة";
                _SystemAction.SaveAction("SaveAttendenceDevice", "AttendenceDeviceService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }



        public GeneralMessage SaveDevice(List<AttendenceDevice> attendencedevice, int UserId, int BranchId)
        {
            try
            {
                foreach (var row in attendencedevice)
                {
                    var attendencedeviceUpdated = _TaamerProContext.AttendenceDevice.Where(x => x.AttendenceDeviceId == row.AttendenceDeviceId).FirstOrDefault();
                    if (attendencedeviceUpdated != null)
                    {

                        attendencedeviceUpdated.DeviceIP = row.DeviceIP;
                        attendencedeviceUpdated.Port = row.Port;
                        attendencedeviceUpdated.MachineNumber = row.MachineNumber;
                        attendencedeviceUpdated.BranchId = BranchId;
                        attendencedeviceUpdated.UpdateUser = UserId;
                        attendencedeviceUpdated.UpdateDate = row.UpdateDate;
                        attendencedeviceUpdated.LastUpdate = DateTime.Now;
                    }

                }
                _TaamerProContext.SaveChanges();
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة جهاز بصمة جديد";
                _SystemAction.SaveAction("SaveDevice", "AttendenceDeviceService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ جهاز البصمة";
                _SystemAction.SaveAction("SaveDevice", "AttendenceDeviceService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        //public GeneralMessage SaveDevice(AttendenceDevice attendencedevice, int UserId, int BranchId)
        //{
        //    try
        //    {
        //        if (attendencedevice.AttendenceDeviceId == 0)
        //        {
        //            attendencedevice.BranchId = BranchId;
        //            attendencedevice.AddUser = UserId;
        //            attendencedevice.AddDate = DateTime.Now;
        //            _AttendenceDeviceRepository.Add(attendencedevice);
        //        }
        //        else
        //        {
        //            var attendencedeviceUpdated = _AttendenceDeviceRepository.GetById(attendencedevice.AttendenceDeviceId);
        //            if (attendencedeviceUpdated != null)
        //            {

        //                attendencedeviceUpdated.DeviceIP = attendencedevice.DeviceIP;
        //                attendencedeviceUpdated.Port = attendencedevice.Port;
        //                attendencedeviceUpdated.MachineNumber = attendencedevice.MachineNumber;
        //                attendencedeviceUpdated.BranchId = attendencedevice.BranchId;
        //                attendencedeviceUpdated.UpdateUser = UserId;
        //                attendencedeviceUpdated.UpdatedDate = DateTime.Now;
        //            }
        //        }
        //        _uow.SaveChanges();
        //        return new GeneralMessage { Result = true, Message = Resources.General_SavedSuccessfully };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new GeneralMessage { Result = false, Message = Resources.General_SavedFailed };
        //    }
        //}
        public GeneralMessage DeleteAttendenceDevice(int attendencedeviceId, int UserId, int BranchId)
        {
            try
            {
                AttendenceDevice attendencedevice = _TaamerProContext.AttendenceDevice.Where(x => x.AttendenceDeviceId == attendencedeviceId).FirstOrDefault();
                attendencedevice.IsDeleted = true;
                attendencedevice.DeleteDate = DateTime.Now;
                attendencedevice.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف جهاز بصمة رقم " + attendencedeviceId;
                _SystemAction.SaveAction("DeleteAttendenceDevice", "AttendenceDeviceService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف جهاز بصمة رقم " + attendencedeviceId; ;
                _SystemAction.SaveAction("DeleteAttendenceDevice", "AttendenceDeviceService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public async Task<IEnumerable<AttendenceDeviceVM>> SearchAttendenceDevices(AttendenceDeviceVM AttendenceDevicesSearch, string lang, int BranchId)
        {
            var attendencedevices = await _AttendenceDeviceRepository.SearchAttendenceDevices(AttendenceDevicesSearch, lang, BranchId);
            return attendencedevices;
        }
        public async Task<AttendenceDeviceVM> SearchAttendenceDevicesById(string attendencedeviceId, string lang)
        {
            var attendencedevices =await _AttendenceDeviceRepository.SearchAttendenceDevicesById(attendencedeviceId, lang);
            return attendencedevices;
        }
    }
}

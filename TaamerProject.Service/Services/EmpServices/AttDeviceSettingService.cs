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
using System.Net;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class AttDeviceSettingService : IAttDeviceService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IAttDevicesettingRepository _attDevicesetting;

        public AttDeviceSettingService(TaamerProjectContext dataContext, ISystemAction systemAction, IAttDevicesettingRepository attDevicesetting)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _attDevicesetting = attDevicesetting;

        }
        public async Task<AttDeviceSittingVM> GetDevicesetting(int BranchId)
        {
            return await _attDevicesetting.GetAttDevicesetting(BranchId);
        }




        public GeneralMessage SaveAttdeviceSetting(AttDeviceSitting attdeviceseting, int UserId, int BranchId)
        {
            try
            {

                var attdevice = _TaamerProContext.AttDeviceSitting.Where(s => s.IsDeleted == false);
                if (attdevice == null || attdevice.Count() == 0)
                {
                    attdeviceseting.AddUser = UserId;
                    attdeviceseting.AddDate = DateTime.Now;
                    _TaamerProContext.AttDeviceSitting.Add(attdeviceseting);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "حفظ اعدادات جهاز البصمة";
                    _SystemAction.SaveAction("SaveClause", "EmailSettingService", 1, Resources.General_SavedSuccessfully , "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var DeviceSettingUpdated = _TaamerProContext.AttDeviceSitting.Where(x=>x.AttDeviceSittingId==attdeviceseting.AttDeviceSittingId).FirstOrDefault();
                    if (DeviceSettingUpdated != null)
                    {
                        DeviceSettingUpdated.ArgCompanyCode = attdeviceseting.ArgCompanyCode;
                        DeviceSettingUpdated.ArgEmpUsername = attdeviceseting.ArgEmpUsername;
                        DeviceSettingUpdated.ArgEmpPassowrd = attdeviceseting.ArgEmpPassowrd;
                        DeviceSettingUpdated.ArgDeviceName = attdeviceseting.ArgDeviceName;

                        DeviceSettingUpdated.UpdateUser = UserId;
                        DeviceSettingUpdated.UpdateDate = DateTime.Now;

                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل الاعدادات رقم " + attdeviceseting.AttDeviceSittingId;
                   _SystemAction.SaveAction("SaveClause", "EmailSettingService", 2, Resources.General_SavedSuccessfully , "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ اعدادات جهاز البصمة";
                _SystemAction.SaveAction("SaveClause", "EmailSettingService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
    }
}

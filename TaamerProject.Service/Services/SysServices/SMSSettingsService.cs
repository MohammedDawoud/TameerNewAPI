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
using TaamerProject.Repository.Repositories;
using TaamerProject.Service.Generic;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class SMSSettingsService :   ISMSSettingsService
    {
        private readonly ISMSSettingsRepository _SMSSettingsRepository;
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly ICustomerSMSService _sMSService;
        public SMSSettingsService(ISMSSettingsRepository SMSSettingsRepository,
              ISys_SystemActionsRepository sys_SystemActionsRepository,
            TaamerProjectContext dataContext, ISystemAction systemAction, ICustomerSMSService sMSService)
        {
            _SMSSettingsRepository = SMSSettingsRepository;
            _Sys_SystemActionsRepository = sys_SystemActionsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _sMSService = sMSService;
        }
 
    public GeneralMessage SavesmsSetting(SMSSettings sMSSettings, int UserId, int BranchId)
               
        {
            try
                {
               // var settings = _SMSSettingsRepository.GetMatching(s => s.IsDeleted == false && s.BranchId == BranchId);
                var settings = _TaamerProContext.SMSSettings.Where(s => s.IsDeleted == false && s.BranchId == BranchId);
                if (settings == null || settings.Count() == 0)
                {
                        sMSSettings.BranchId = BranchId;
                        sMSSettings.AddUser = UserId;
                        sMSSettings.AddDate = DateTime.Now;
                        _TaamerProContext.SMSSettings.Add(sMSSettings);

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة إعدادات الرسائل جديدة";
                    _SystemAction.SaveAction("SavesmsSetting", "SMSSettingsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
               else
               {
                    //var SMSSettingsUpdated = _SMSSettingsRepository.GetById(sMSSettings.SettingId);
                    SMSSettings? SMSSettingsUpdated =   _TaamerProContext.SMSSettings.Where(s => s.SettingId == sMSSettings.SettingId).FirstOrDefault();
                    if (SMSSettingsUpdated != null)
                      {
                           SMSSettingsUpdated.ApiUrl = sMSSettings.ApiUrl;
                           SMSSettingsUpdated.MobileNo = sMSSettings.MobileNo;
                           SMSSettingsUpdated.Password = sMSSettings.Password;
                           SMSSettingsUpdated.SenderName = sMSSettings.SenderName;
                           SMSSettingsUpdated.UpdateUser = UserId;
                           SMSSettingsUpdated.UpdateDate = DateTime.Now;
                           SMSSettingsUpdated.UserName = sMSSettings.UserName;
                        SMSSettingsUpdated.SendCustomerSMS= sMSSettings.SendCustomerSMS;

                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل إعدادات الرسائل, خيار رقم " + sMSSettings.SettingId;
                    _SystemAction.SaveAction("SavesmsSetting", "SMSSettingsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ إعدادات الرسائل";
                _SystemAction.SaveAction("SavesmsSetting", "SMSSettingsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public Task<SMSSettingsVM> GetsmsSetting(int BranchId)
        {
            return _SMSSettingsRepository.GetsmsSetting(BranchId);
        }


        public GeneralMessage SendSMS_Test(int UserId,int BranchId, string Mobile, string Message)
        {
            return _sMSService.SaveCustomerSMS_Notification(Mobile,Message,UserId,BranchId);
        }




    }
}

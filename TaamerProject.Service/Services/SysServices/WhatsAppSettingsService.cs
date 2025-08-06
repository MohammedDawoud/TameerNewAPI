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
    public class WhatsAppSettingsService : IWhatsAppSettingsService
    {
        private readonly IWhatsAppSettingsRepository _WhatsAppSettingsRepository;
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly ICustomerSMSService _sMSService;
        public WhatsAppSettingsService(IWhatsAppSettingsRepository WhatsAppSettingsRepository,
              ISys_SystemActionsRepository sys_SystemActionsRepository,
            TaamerProjectContext dataContext, ISystemAction systemAction, ICustomerSMSService sMSService)
        {
            _WhatsAppSettingsRepository = WhatsAppSettingsRepository;
            _Sys_SystemActionsRepository = sys_SystemActionsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _sMSService = sMSService;
        }
 
    public GeneralMessage SaveWhatsAppSetting(WhatsAppSettings WhatsAppSettings, int UserId, int BranchId)
               
        {
            try
                {
                var settings = _TaamerProContext.WhatsAppSettings.Where(s => s.IsDeleted == false && s.BranchId == BranchId);
                if (settings == null || settings.Count() == 0)
                {
                    WhatsAppSettings.BranchId = BranchId;
                    WhatsAppSettings.UserId = UserId;
                    WhatsAppSettings.AddUser = UserId;
                    WhatsAppSettings.AddDate = DateTime.Now;
                    WhatsAppSettings.Sendactivation = WhatsAppSettings.Sendactivation ?? false;
                    WhatsAppSettings.SendactivationOffer = WhatsAppSettings.SendactivationOffer ?? false;
                    WhatsAppSettings.SendactivationProject = WhatsAppSettings.SendactivationProject ?? false;
                    WhatsAppSettings.SendactivationSupervision = WhatsAppSettings.SendactivationSupervision ?? false;


                    _TaamerProContext.WhatsAppSettings.Add(WhatsAppSettings);

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة إعدادات الرسائل جديدة";
                    _SystemAction.SaveAction("SaveWhatsAppSetting", "WhatsAppSettingsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
               else
               {
                    WhatsAppSettings? WhatsAppSettingsUpdated =   _TaamerProContext.WhatsAppSettings.Where(s => s.SettingId == WhatsAppSettings.SettingId).FirstOrDefault();
                    if (WhatsAppSettingsUpdated != null)
                      {
                           WhatsAppSettingsUpdated.ApiUrl = WhatsAppSettings.ApiUrl;
                           WhatsAppSettingsUpdated.MobileNo = WhatsAppSettings.MobileNo;
                           WhatsAppSettingsUpdated.Password = WhatsAppSettings.Password;
                           WhatsAppSettingsUpdated.SenderName = WhatsAppSettings.SenderName;
                           WhatsAppSettingsUpdated.UpdateUser = UserId;
                        WhatsAppSettingsUpdated.UserId = UserId;

                        WhatsAppSettingsUpdated.UpdateDate = DateTime.Now;
                           WhatsAppSettingsUpdated.UserName = WhatsAppSettings.UserName;
                            WhatsAppSettingsUpdated.InstanceId = WhatsAppSettings.InstanceId;
                            WhatsAppSettingsUpdated.Token = WhatsAppSettings.Token;
                        WhatsAppSettingsUpdated.TypeName = WhatsAppSettings.TypeName;
                        WhatsAppSettingsUpdated.Sendactivation= WhatsAppSettings.Sendactivation??false;
                        WhatsAppSettingsUpdated.SendactivationOffer = WhatsAppSettings.SendactivationOffer ?? false;
                        WhatsAppSettingsUpdated.SendactivationProject = WhatsAppSettings.SendactivationProject ?? false;
                        WhatsAppSettingsUpdated.SendactivationSupervision = WhatsAppSettings.SendactivationSupervision ?? false;


                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل إعدادات الرسائل, خيار رقم " + WhatsAppSettings.SettingId;
                    _SystemAction.SaveAction("SaveWhatsAppSetting", "WhatsAppSettingsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ إعدادات الرسائل";
                _SystemAction.SaveAction("SaveWhatsAppSetting", "WhatsAppSettingsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public Task<WhatsAppSettingsVM> GetWhatsAppSetting(int BranchId)
        {
            return _WhatsAppSettingsRepository.GetWhatsAppSetting(BranchId);
        }


        public GeneralMessage SendWhatsApp_Test(int UserId,int BranchId, string Mobile, string Message, string environmentURL, string PDFURL)
        {
            return _sMSService.SendWhatsApp_Notification(Mobile,Message,UserId,BranchId, environmentURL, PDFURL);
        }




    }
}

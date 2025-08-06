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
    public class NotificationSettingsService : INotificationSettingsService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly INotificationSettingsRepository _NotificationSettingsRepository;



        public NotificationSettingsService(TaamerProjectContext dataContext, ISystemAction systemAction, INotificationSettingsRepository notificationSettingsRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _NotificationSettingsRepository = notificationSettingsRepository;
        }

        public async Task<IEnumerable<NotificationSettingsVM>> GetAllNotificationSettings()
        {
            var NotificationSettings =await _NotificationSettingsRepository.GetAllNotificationSettings();
            return NotificationSettings;
        }
        public GeneralMessage UpdateNotificationSettings(NotificationSettings notificationSettings, int UserId, int BranchId)
        {
            try
            {

                if (notificationSettings.SettingId == 0)
                {
                    notificationSettings.AddUser = UserId;
                    notificationSettings.BranchId = BranchId;
                    notificationSettings.AddDate = DateTime.Now;
                    _TaamerProContext.NotificationSettings.Add(notificationSettings);
                }
                else
                {
                    var NotificationSettingsUpdated = _TaamerProContext.NotificationSettings.Where(x=>x.SettingId==notificationSettings.SettingId).FirstOrDefault();
                    if (NotificationSettingsUpdated != null)
                    {
                        NotificationSettingsUpdated.IDEndCount = notificationSettings.IDEndCount;
                        NotificationSettingsUpdated.PassportCount = notificationSettings.PassportCount;
                        NotificationSettingsUpdated.LicesnseCount = notificationSettings.LicesnseCount;
                        NotificationSettingsUpdated.ContractCount = notificationSettings.ContractCount;
                        NotificationSettingsUpdated.MedicalCount = notificationSettings.MedicalCount;
                        NotificationSettingsUpdated.VacancyCount = notificationSettings.VacancyCount;
                        NotificationSettingsUpdated.LoanCount = notificationSettings.LoanCount;
                        //NotificationSettingsUpdated.BranchId = notificationSettings.BranchId;
                        NotificationSettingsUpdated.UpdateUser = UserId;
                        NotificationSettingsUpdated.UpdateDate = DateTime.Now;
                    }
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تعديل إعدادات الاشعارات " + notificationSettings.SettingId;
               _SystemAction.SaveAction("UpdateNotificationSettings", "NotificationSettingsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ التعديل";
                _SystemAction.SaveAction("UpdateNotificationSettings", "NotificationSettingsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        //public GeneralMessage DeleteNotificationSettings( int SettingId, int UserId)
        //{
        //    try
        //    {
        //        NotificationSettings notification = _NotificationSettingsRepository.GetById(SettingId);
        //        notification.IsDeleted = true;
        //        notification.DeleteDate = DateTime.Now;
        //        notification.DeleteUser = UserId;
        //        _uow.SaveChanges();
        //        return new GeneralMessage { Result = true, Message = "تم الحذف بنجاح" };
        //    }
        //    catch (Exception)
        //    {
        //        return new GeneralMessage { Result = false, Message = Resources.General_DeletedFailed };
        //    }
        //}
    }
}

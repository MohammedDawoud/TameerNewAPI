using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class EmailSettingService :   IEmailSettingService
    {
        private readonly IEmailSettingRepository _EmailSettingRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public EmailSettingService(TaamerProjectContext dataContext
            , ISystemAction systemAction, IEmailSettingRepository EmailSettingRepository)
        {
            _EmailSettingRepository = EmailSettingRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;

        }
        public GeneralMessage SaveEmailSetting(EmailSetting EmailSetting, int UserId, int BranchId)
        {
            try
            {
                EmailSetting.SenderEmail.Trim();
                EmailSetting.Password.Trim();
                EmailSetting.Port.Trim();
                //var settings = _EmailSettingRepository.GetMatching(s => s.IsDeleted == false && s.BranchId == BranchId);
                var settings = _TaamerProContext.EmailSetting.Where(s => s.IsDeleted == false && s.BranchId == BranchId).ToList();

                if (settings == null || settings.Count() == 0)
                {
                    EmailSetting.UserId = UserId;
                    EmailSetting.AddUser = UserId;
                    EmailSetting.BranchId = BranchId;
                    EmailSetting.AddDate = DateTime.Now;
                    _TaamerProContext.EmailSetting.Add(EmailSetting);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "حفظ اعدادات الميل";
                     _SystemAction.SaveAction("SaveClause", "EmailSettingService", 1, Resources.General_SavedSuccessfully , "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully  };
                }
                else
                {
                  //  var EmailSettingUpdated = _EmailSettingRepository.GetById(EmailSetting.SettingId);
                    EmailSetting? EmailSettingUpdated = _TaamerProContext.EmailSetting.Where(s => s.SettingId == EmailSetting.SettingId).FirstOrDefault();

                    if (EmailSettingUpdated != null)
                    {
                        EmailSettingUpdated.SenderEmail = EmailSetting.SenderEmail;
                        EmailSettingUpdated.Password = EmailSetting.Password;
                        EmailSettingUpdated.SenderName = EmailSetting.SenderName;
                        EmailSettingUpdated.Host = EmailSetting.Host;
                        EmailSettingUpdated.Port = EmailSetting.Port;
                        EmailSettingUpdated.SSL = EmailSetting.SSL;
                        EmailSettingUpdated.UserId = UserId;
                        EmailSettingUpdated.UpdateUser = UserId;
                        EmailSettingUpdated.UpdateDate = DateTime.Now;
                        EmailSettingUpdated.DisplayName = EmailSetting.DisplayName;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل الاعدادات رقم " + EmailSetting.SettingId;
                     _SystemAction.SaveAction("SaveClause", "EmailSettingService", 2, Resources.General_SavedSuccessfully , "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully  };
                }

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ اعدادات الميل";
                 _SystemAction.SaveAction("SaveClause", "EmailSettingService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public Task<EmailSettingVM> GetEmailSetting(int BranchId)
        {
            return _EmailSettingRepository.GetEmailSetting(BranchId);
        }
   

    }
}

using System.Collections.Generic;
using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using System.Net.Mail;
using Google.Apis.Drive.v3.Data;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class UserMailsService :   IUserMailsService
    {
        private readonly IUserMailsRepository _UserMailsRepository;
        private readonly IEmailSettingRepository _EmailSettingRepository;
        private readonly IUsersRepository _UsersRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public UserMailsService(IUserMailsRepository UserMailsRepository, IEmailSettingRepository EmailSettingRepository
            ,IUsersRepository UsersRepository, TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _UserMailsRepository = UserMailsRepository;
            _EmailSettingRepository = EmailSettingRepository;
            _UsersRepository = UsersRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task<IEnumerable<UserMailsVM>> GetAllUserMails(int UserId, int BranchId)
        {
            var UserMails = _UserMailsRepository.GetAllUserMails(UserId, BranchId);
            return UserMails ;
        }
        public Task<IEnumerable<UserMailsVM>> GetAllUnReadUserMails(int UserId, int BranchId)
        {
            var UserMails = _UserMailsRepository.GetAllUnReadUserMails(UserId, BranchId);
            return UserMails ;
        }
        public Task<int> GetAllUserMailsCount(int UserId, int BranchId)
        {
            var UserMails = _UserMailsRepository.GetAllUserMailsCount(UserId, BranchId);
            return UserMails;
        }
        public Task<IEnumerable<UserMailsVM>> GetAllUserMailsSent(int UserId, int BranchId)
        {
            var UserMails = _UserMailsRepository.GetAllUserMailsSent(UserId, BranchId);
            return UserMails ;
        }
        public Task<int> GetAllUserMailsSentCount(int UserId, int BranchId)
        {
            var UserMails = _UserMailsRepository.GetAllUserMailsSentCount(UserId, BranchId);
            return UserMails;
        }
        public Task<IEnumerable<UserMailsVM>> GetAllUserMailsTrash(int UserId, int BranchId)
        {
            var UserMails = _UserMailsRepository.GetAllUserMailsTrash(UserId, BranchId);
            return UserMails ;
        }
        public Task<int> GetAllUserMailsTrashCount(int UserId, int BranchId)
        {
            var UserMails = _UserMailsRepository.GetAllUserMailsTrashCount(UserId, BranchId);
            return UserMails;
        }
        public GeneralMessage SaveUserMails(UserMails userMails, int UserId, int BranchId)
        {
            try
            {
                var UserMailsObj = new UserMails();
                UserMailsObj.UserId = userMails.UserId;
                UserMailsObj.SenderUserId = UserId;
                UserMailsObj.MailText = userMails.MailText;
                UserMailsObj.MailSubject = userMails.MailSubject;
                UserMailsObj.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                UserMailsObj.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar"));
                UserMailsObj.IsRead = false;
                UserMailsObj.AddUser = UserId;
                UserMailsObj.BranchId = BranchId;
                UserMailsObj.AddDate = DateTime.Now;
                _TaamerProContext.UserMails.Add(UserMailsObj);
                
                var mail = new MailMessage();
                var emailSettings = _EmailSettingRepository.GetEmailSetting(BranchId);
                var loginInfo = new NetworkCredential(emailSettings.Result.SenderEmail, emailSettings.Result.Password);

                if (_EmailSettingRepository.GetEmailSetting(BranchId).Result.DisplayName != null)
                {
                    mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(BranchId).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(BranchId).Result.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(BranchId).Result.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                }

                // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(BranchId).SenderEmail);
                //mail.To.Add(new MailAddress(_UsersRepository.GetById(userMails.UserId).Email));
                var receivedUser = _TaamerProContext.Users.Where(s => s.UserId == userMails.UserId).FirstOrDefault();
                mail.To.Add(new MailAddress(receivedUser?.Email ?? ""));

                mail.Subject = userMails.MailSubject;
                mail.Body = userMails.MailText;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(emailSettings.Result.Host, Convert.ToInt16(emailSettings.Result.Port));///new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(emailSettings.Result.Port);

                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
                
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "إرسال ميل لمستخدم";
                _SystemAction.SaveAction("SaveUserMails", "UserMailsService", 1, Resources.sent_succesfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.sent_succesfully };
            }
            catch (Exception ex)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في إرسال ميل لمستخدم";
                _SystemAction.SaveAction("SaveClause", "UserMailsService", 1, "فشل في الارسال , تأكد من اعدادات البريد الالكتروني", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.Not_sent };
            }
        }
        public GeneralMessage DeleteUserMails(int MailId, int UserId, int BranchId)
        {
            try
            {
                //UserMails UserMails = _UserMailsRepository.GetById(MailId);
                UserMails? UserMails = _TaamerProContext.UserMails.Where(s => s.MailId == MailId).FirstOrDefault();
                if (UserMails != null)
                {
                    UserMails.IsDeleted = true;
                    UserMails.DeleteDate = DateTime.Now;
                    UserMails.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف بريد إلكتروني رقم " + MailId;
                    _SystemAction.SaveAction("DeleteUserMails", "UserMailsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف بريد إلكتروني رقم " + MailId; ;
                _SystemAction.SaveAction("DeleteUserMails", "UserMailsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public bool ReadUserMails(int UserId, int BranchId)
        {
            try
            {
                // var unReadNotify = _UserMailsRepository.GetMatching(s => s.IsDeleted == false && (s.UserId == UserId || s.AllUsers == true) && s.IsRead == false);
                var unReadNotify = _TaamerProContext.UserMails.Where(s => s.IsDeleted == false && (s.UserId == UserId || s.AllUsers == true) && s.IsRead == false);


                if (unReadNotify != null)
                {
                    foreach (var item in unReadNotify)
                    {
                        item.IsRead = true;
                    }
                }
                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تم قراءة البريد الإليكتروني للمستخدم رقم " + UserId;
                _SystemAction.SaveAction("ReadUserMails", "UserMailsService", 2, "تم قراءة البريد الإليكتروني", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return true;
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في  قراءة البريد الإليكتروني للمستخدم رقم " + UserId; ;
                _SystemAction.SaveAction("DeleteClause", "UserMailsService", 3, "فشل في  قراءة البريد الإليكتروني", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return false;
            }
        }

     
    }
}

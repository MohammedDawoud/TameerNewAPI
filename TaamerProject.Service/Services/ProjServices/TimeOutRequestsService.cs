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
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class TimeOutRequestsService :  ITimeOutRequestsService
    {
        private readonly ITimeOutRequestsRepository _TimeOutRequestsRepository;
        private readonly IEmailSettingRepository _EmailSettingRepository;
        private readonly IUsersRepository _UsersRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public TimeOutRequestsService(TaamerProjectContext dataContext, ISystemAction systemAction, IEmailSettingRepository emailSettingRepository,
            IUsersRepository usersRepository, ITimeOutRequestsRepository timeOutRequestsRepository)
        {
            _TimeOutRequestsRepository = timeOutRequestsRepository;
            _EmailSettingRepository = emailSettingRepository;
            _UsersRepository = usersRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task<IEnumerable<TimeOutRequestsVM>> GetTimeOutRequests(int BranchId)
        {
            var TimeOutRequests = _TimeOutRequestsRepository.GetTimeOutRequests(BranchId);
            return TimeOutRequests;
        }
        public GeneralMessage SaveTimeOutRequests(TimeOutRequests TimeOutRequests, int UserId, int BranchId)
        {
            try
            {
                if (TimeOutRequests.RequestId == 0)
                {
                    TimeOutRequests.Status = 0;
                    TimeOutRequests.UserId = UserId;
                    TimeOutRequests.BranchId = BranchId;
                    //TimeOutRequests.Duration = TimeOutRequests.Duration * 60;
                    TimeOutRequests.AddUser = UserId;
                    TimeOutRequests.AddDate = DateTime.Now;
                    _TaamerProContext.TimeOutRequests.Add(TimeOutRequests);

                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة طلب تمديد جديد";
                _SystemAction.SaveAction("SaveTimeOutRequests", "TimeOutRequestsService", 1, "تم ارسال الطلب بنجاح", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ طلب التمديد";
                _SystemAction.SaveAction("SaveTimeOutRequests", "TimeOutRequestsService", 1, "فشل في ارسال الطلب", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public   GeneralMessage DeleteTimeOutRequests(int RequestId, int UserId, int BranchId)
        {
            try
            {
                //TimeOutRequests TimeOutRequests = _TimeOutRequestsRepository.GetById(RequestId);
                TimeOutRequests? TimeOutRequests =   _TaamerProContext.TimeOutRequests.Where(s => s.RequestId == RequestId).FirstOrDefault();
                if (TimeOutRequests != null)
                {
                    TimeOutRequests.IsDeleted = true;
                    TimeOutRequests.DeleteDate = DateTime.Now;
                    TimeOutRequests.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف طلب تمديد رقم " + RequestId;
                    _SystemAction.SaveAction("DeleteTimeOutRequests", "TimeOutRequestsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------


                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف طلب تمديد رقم " + RequestId; ;
                _SystemAction.SaveAction("DeleteTimeOutRequests", "TimeOutRequestsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }

        public GeneralMessage ApproveRequest(int RequestId, int UserId,int BranchId , string Comment)
        {
            try
            {

                //var request = _TimeOutRequestsRepository.GetById(RequestId);
                TimeOutRequests? request =   _TaamerProContext.TimeOutRequests.Where(s => s.RequestId == RequestId).FirstOrDefault();
                if (request != null)
                {
                    request.Status = 1;
                    request.ActionUserId = UserId;
                    request.Comment = Comment;
                    _TaamerProContext.SaveChanges();
                    try
                    {
                        var mail = new MailMessage();
                        var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(BranchId).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(BranchId).Result.Password);

                        if (_EmailSettingRepository.GetEmailSetting(BranchId).Result.DisplayName != null)
                        {
                            mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(BranchId).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(BranchId).Result.DisplayName);
                        }
                        else
                        {
                            mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(BranchId).Result.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                        }
                        //mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(BranchId).Result.SenderEmail);

                        var emailSettings =  _TaamerProContext.Users.Where(s => s.UserId == UserId).FirstOrDefault();
                        mail.To.Add(new MailAddress(emailSettings?.Email ?? ""));

                        //mail.To.Add(new MailAddress(_UsersRepository.GetById(UserId).Email));
                        mail.Subject = "قبول طلب تمديد مهمة";
                        var userName =  _TaamerProContext.Users.Where(s => s.UserId == UserId).FirstOrDefault();
                        mail.Body = "تم قبول طلب تمديد مهمه الخاص بكم من قبل " + userName?.FullName + "";
                        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(BranchId).Result.Host);
                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = loginInfo;
                        smtpClient.Send(mail);
                    }
                    catch (Exception)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "قبول طلب تمديد مهمة و فشل إرسال بريد إلكتروني";
                        _SystemAction.SaveAction("ApproveRequest", "TimeOutRequestsService", 2, "فشل في ارسال بريد الكتروني", "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في ارسال بريد الكتروني " };
                    }

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "قبول طلب تمديد مهمة و إرسال بريد إلكتروني";
                    _SystemAction.SaveAction("ApproveRequest", "TimeOutRequestsService", 2, "تم قبول الطلب وارسال بريد الكتروني ", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم قبول الطلب وارسال بريد الكتروني " };
            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في قبول الطلب";
                _SystemAction.SaveAction("ApproveRequest", "TimeOutRequestsService", 2, "فشل في قبول الطلب", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في قبول الطلب" };
            }
        }
        public GeneralMessage RejectRequest(int RequestId, int UserId, int BranchId ,string Comment)
        {
            try
            {
               // var request = _TimeOutRequestsRepository.GetById(RequestId);
                TimeOutRequests? request =  _TaamerProContext.TimeOutRequests.Where(s => s.RequestId == RequestId).FirstOrDefault();
                if (request != null)
                {
                    request.Status = 2;
                    request.Comment = Comment;
                    request.ActionUserId = UserId;
                    _TaamerProContext.SaveChanges();
                    try
                    {
                        var mail = new MailMessage();
                        var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(BranchId).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(BranchId).Result.Password);
                        if (_EmailSettingRepository.GetEmailSetting(BranchId).Result.DisplayName != null)
                        {
                            mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(BranchId).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(BranchId).Result.DisplayName);
                        }
                        else
                        {
                            mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(BranchId).Result.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                        }
                        // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(BranchId).SenderEmail);

                        var emailSettings =  _TaamerProContext.Users.Where(s => s.UserId == UserId).FirstOrDefault();
                        mail.To.Add(new MailAddress(emailSettings?.Email ?? ""));

                        // mail.To.Add(new MailAddress(_UsersRepository.GetById(UserId).Email));
                        mail.Subject = "رفض طلب تمديد مهمة";
                        var userName =  _TaamerProContext.Users.Where(s => s.UserId == UserId).FirstOrDefault();
                        mail.Body = "تم رفض طلب تمديد مهمه الخاص بكم من قبل " + userName?.FullName + "";
                        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(BranchId).Result.Host);
                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = loginInfo;
                        smtpClient.Send(mail);
                    }
                    catch (Exception)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "رفض طلب تمديد مهمة و فشل إرسال بريد إلكتروني";
                        _SystemAction.SaveAction("RejectRequest", "TimeOutRequestsService", 2, "فشل في ارسال بريد الكتروني", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في ارسال بريد الكتروني " };
                    }

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "رفض طلب تمديد مهمة و إرسال بريد إلكتروني";
                    _SystemAction.SaveAction("RejectRequest", "TimeOutRequestsService", 2, " تم رفض الطلب وارسال بريد الكتروني ", "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = " تم رفض الطلب وارسال بريد الكتروني " };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في رفض الطلب";
                _SystemAction.SaveAction("RejectRequest", "TimeOutRequestsService", 2, "فشل في رفض الطلب", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = " فشل في رفض الطلب " };
            }
        }


    }

}

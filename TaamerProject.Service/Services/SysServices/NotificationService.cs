using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using System.Net;
using System.Data.SqlClient;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class NotificationService : INotificationService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly INotificationRepository _NotificationRepository;
        private readonly IEmployeesRepository _EmployeesRepository;
        private readonly IUsersRepository _UserRepository;
        private readonly IOrganizationsRepository _organizationsRepository;


        public NotificationService(TaamerProjectContext dataContext, ISystemAction systemAction, INotificationRepository notificationRepository, IEmployeesRepository employeesRepository
            , IUsersRepository usersRepository, IOrganizationsRepository organizationsRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _NotificationRepository = notificationRepository;
            _EmployeesRepository = employeesRepository;
            _UserRepository = usersRepository;
            _organizationsRepository = organizationsRepository;
        }

        public async Task<IEnumerable<NotificationVM>> GetAllNotifications(int ProjectId)
        {
            var Notifications =await _NotificationRepository.GetAllNotifications(ProjectId);
            return Notifications.ToList();
        }
        public async Task< IEnumerable<NotificationVM>> GetAllNotificationsBackup()
        {
            var Notifications =await _NotificationRepository.GetAllNotificationsBackup();
            return Notifications.ToList();
        }
        public async Task<IEnumerable<NotificationVM>> GetAllAlerts(int BranchId)
        {
            var Notifications =await _NotificationRepository.GetAllAlerts(BranchId);
            return Notifications;
        }
        public async Task<IEnumerable<NotificationVM>> GetUserlAlerts_Dashboard(int BranchId, int UserId)
        {
            var user = _UserRepository.GetById(UserId);
            var Notifications = _NotificationRepository.GetMatching(s => s.IsDeleted == false && s.Type == 2 && (s.Done.HasValue && !s.Done.Value) &&
            (s.ReceiveUserId == UserId || s.DepartmentId == user.DepartmentId || (s.AllUsers.HasValue && s.AllUsers.Value))).Select(x => new NotificationVM
            {
                NotificationId = x.NotificationId,
                Name = x.Name,
                Date = x.Date,
                HijriDate = x.HijriDate,
                SendUserId = x.SendUserId,
                ReceiveUserId = x.ReceiveUserId,
                Description = x.Description,
                Done = x.Done,
                AllUsers = x.AllUsers,
                ActionUser = x.ActionUser,
                ActionDate = x.ActionDate,
                IsRead = x.IsRead,
                Type = x.Type,
                SendDate = x.SendDate,
                ReadingDate = x.ReadingDate,
                ProjectId = x.ProjectId,
                //UserName = x.Users.FullName,
                //ReceivedUserName = x.ReceiveUsers.FullName ?? "كل المستخدمين",
                //SendUserName = x.Users.FullName,
                AttachmentUrl = x.AttachmentUrl,

            }).ToList().OrderByDescending(a => a.SendDate);
            return Notifications;
        }
        public async Task<IEnumerable<NotificationVM>> GetUserlAlerts(int BranchId, int UserId)
        {

            var Notifications =await _NotificationRepository.GetUserlAlerts(BranchId, UserId);
            return Notifications;
        }
        public GeneralMessage SaveNotification(Notification notification, int UserId, int BranchId)
        {
            try
            {
                Notification UserNotification = null;
                if (notification.NotificationId == 0)
                {
                    if (notification.AllUsers == false || notification.AssignedUsersIds != null)
                    {
                        foreach (var NotificationUserId in notification.AssignedUsersIds)
                        {
                            UserNotification = new Notification();
                            UserNotification.ReceiveUserId = NotificationUserId;
                            UserNotification.Name = notification.Name;
                            UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                            UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                            UserNotification.SendUserId = UserId;
                            UserNotification.Type = 1; // notification
                            UserNotification.Description = notification.Description;
                            UserNotification.Done = notification.Done;
                            UserNotification.AllUsers = notification.AllUsers;
                            UserNotification.ActionUser = notification.ActionUser;
                            UserNotification.ActionDate = notification.ActionDate;
                            UserNotification.SendDate = DateTime.Now;
                            UserNotification.ProjectId = notification.ProjectId;
                            UserNotification.AttachmentUrl = notification.AttachmentUrl;
                            UserNotification.AddUser = UserId;
                            UserNotification.BranchId = BranchId;
                            UserNotification.AddDate = DateTime.Now;
                            UserNotification.Title = notification.Title;
                            UserNotification.IsHidden = false;
                            UserNotification.NextTime = null;
                            _TaamerProContext.Notification.Add(UserNotification);
                            sendmobilenotification(UserId, notification.Name, notification.Description);

                        }
                    }
                    else
                    {
                        UserNotification = new Notification();
                        //var UserNotification = new Notification();
                        UserNotification.ReceiveUserId = null;
                        UserNotification.Name = notification.Name;
                        UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                        UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                        UserNotification.SendUserId = UserId;
                        UserNotification.Type = 1; // notification
                        UserNotification.Description = notification.Description;
                        UserNotification.Done = notification.Done;
                        UserNotification.AllUsers = notification.AllUsers;
                        UserNotification.ActionUser = notification.ActionUser;
                        UserNotification.ActionDate = notification.ActionDate;
                        UserNotification.SendDate = DateTime.Now;
                        UserNotification.ProjectId = notification.ProjectId;
                        UserNotification.AttachmentUrl = notification.AttachmentUrl;
                        UserNotification.AddUser = UserId;
                        UserNotification.AddDate = DateTime.Now;
                        UserNotification.Title = notification.Title;
                        UserNotification.IsHidden = false;
                        UserNotification.NextTime = null;
                        _TaamerProContext.Notification.Add(UserNotification);

                        sendmobilenotification(UserId, notification.Title, notification.Description);
                    }
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة اشعار جديد";
               _SystemAction.SaveAction("SaveNotification", "NotificationService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedParm = Convert.ToInt32(UserNotification.NotificationId) };  //Convert.ToInt32(_NotificationRepository.GetMaxId()) };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ االاشعار";
                _SystemAction.SaveAction("SaveNotification", "NotificationService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public GeneralMessage SaveNotification2(Notification2 notification, int UserId, int BranchId)
        {
            try
            {
                Notification UserNotification = null;
                if (notification.NotificationId == 0)
                {
                    if (notification.AllUsers == false || notification.AssignedUsersIds != null)
                    {
                        foreach (var NotificationUserId in notification.AssignedUsersIds)
                        {
                            UserNotification = new Notification();
                            UserNotification.ReceiveUserId = NotificationUserId;
                            UserNotification.Name = notification.Name;
                            UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                            UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                            UserNotification.SendUserId = UserId;
                            UserNotification.Type = 1; // notification
                            UserNotification.Description = notification.Description;
                            UserNotification.Done = notification.Done;
                            UserNotification.AllUsers = notification.AllUsers;
                            UserNotification.ActionUser = notification.ActionUser;
                            UserNotification.ActionDate = notification.ActionDate;
                            UserNotification.SendDate = DateTime.Now;
                            UserNotification.ProjectId = notification.ProjectId;
                            UserNotification.AttachmentUrl = notification.AttachmentUrl;
                            UserNotification.AddUser = UserId;
                            UserNotification.BranchId = BranchId;
                            UserNotification.AddDate = DateTime.Now;
                            UserNotification.Title = notification.Title;
                            UserNotification.IsHidden = false;
                            UserNotification.NextTime = null;
                            _TaamerProContext.Notification.Add(UserNotification);
                            sendmobilenotification(NotificationUserId, notification.Name, notification.Description);

                        }
                    }
                    else
                    {
                        UserNotification = new Notification();
                        //var UserNotification = new Notification();
                        UserNotification.ReceiveUserId = null;
                        UserNotification.Name = notification.Name;
                        UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                        UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                        UserNotification.SendUserId = UserId;
                        UserNotification.Type = 1; // notification
                        UserNotification.Description = notification.Description;
                        UserNotification.Done = notification.Done;
                        UserNotification.AllUsers = notification.AllUsers;
                        UserNotification.ActionUser = notification.ActionUser;
                        UserNotification.ActionDate = notification.ActionDate;
                        UserNotification.SendDate = DateTime.Now;
                        UserNotification.ProjectId = notification.ProjectId;
                        UserNotification.AttachmentUrl = notification.AttachmentUrl;
                        UserNotification.AddUser = UserId;
                        UserNotification.AddDate = DateTime.Now;
                        UserNotification.Title = notification.Title;
                        UserNotification.IsHidden = false;
                        UserNotification.NextTime = null;
                        _TaamerProContext.Notification.Add(UserNotification);

                        sendmobilenotification(UserId, notification.Title, notification.Description);
                    }
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة اشعار جديد";
                _SystemAction.SaveAction("SaveNotification", "NotificationService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedParm = Convert.ToInt32(UserNotification.NotificationId) };  //Convert.ToInt32(_NotificationRepository.GetMaxId()) };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ االاشعار";
                _SystemAction.SaveAction("SaveNotification", "NotificationService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public bool ReadUserNotification(int UserId)
        {
            try
            {
                var unReadNotify = _NotificationRepository.GetMatching(s => s.IsDeleted == false && (s.ReceiveUserId == UserId || s.AllUsers == true) && (s.IsRead == false || s.IsRead == null));
                if (unReadNotify != null)
                {
                    foreach (var item in unReadNotify)
                    {
                        item.IsRead = true;
                        item.ReadingDate = DateTime.Now;
                    }
                }
                _TaamerProContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public GeneralMessage ReadNotification(int NotiID)
        {
            try
            {
                var unReadNotify = _TaamerProContext.Notification.Where(s => s.IsDeleted == false && s.NotificationId == NotiID).FirstOrDefault();
                if (unReadNotify != null)
                {
                    unReadNotify.IsRead = true;
                    unReadNotify.ReadingDate = DateTime.Now;
                }
                _TaamerProContext.SaveChanges();
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedStr = "" };
            }
        }

        public GeneralMessage ReadNotificationList(List<int> NotiID)
        {
            try
            {
                if (NotiID != null && NotiID.Count()>0)
                {
                    foreach (var item in NotiID)
                    {
                        var unReadNotify = _TaamerProContext.Notification.Where(s =>  s.NotificationId == item).FirstOrDefault();
                        if (unReadNotify != null)
                        {
                            unReadNotify.IsRead = true;
                            unReadNotify.ReadingDate = DateTime.Now;
                        }
                    }
                }
                _TaamerProContext.SaveChanges();
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "ReadAllNotification" };
            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "ReadAllNotification", ReturnedStr = "" };
            }
        }
        public GeneralMessage SaveAlert(Notification alert, int UserId, int BranchId)
        {
            try
            {
                if (alert.NotificationId == 0)
                {
                    alert.BranchId = BranchId;
                    alert.AddUser = alert.SendUserId = UserId;
                    alert.AddDate = DateTime.Now;
                    alert.SendDate = DateTime.Now;
                    alert.Type = 2; // alert
                    alert.IsHidden = false;
                    alert.IsRead = false;
                    alert.DepartmentId = alert.DepartmentId;
                    alert.ReceiveUserId = alert.ReceiveUserId == 0 ? null : alert.ReceiveUserId;
                    alert.AllUsers = alert.AllUsers;
                    _TaamerProContext.Notification.Add(alert);
                }
                else
                {
                    var NotificationUpdated = _NotificationRepository.GetById((int)alert.NotificationId);
                    if (NotificationUpdated != null)
                    {
                        NotificationUpdated.Name = alert.Name;
                        NotificationUpdated.Date = alert.Date;
                        NotificationUpdated.HijriDate = alert.HijriDate;
                        NotificationUpdated.SendUserId = UserId;
                        NotificationUpdated.Description = alert.Description;
                        NotificationUpdated.Done = alert.Done;
                        NotificationUpdated.AllUsers = alert.AllUsers;
                        NotificationUpdated.UpdateUser = UserId;
                        NotificationUpdated.UpdateDate = DateTime.Now;
                    }
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة تنبيه جديد";
                _SystemAction.SaveAction("SaveAlert", "NotificationService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ تنبيه";
                _SystemAction.SaveAction("SaveAlert", "NotificationService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage EndAlert(int AlertId, int UserId, int BranchId)
        {
            try
            {
                var NotificationUpdated = _NotificationRepository.GetById(AlertId);
                if (NotificationUpdated != null)
                {
                    NotificationUpdated.Done = true;
                    NotificationUpdated.UpdateUser = UserId;
                    NotificationUpdated.UpdateDate = DateTime.Now;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "إنهاء تنبيه جديد";
                    _SystemAction.SaveAction("EndAlert", "NotificationService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في إنهاء تنبيه";
                    _SystemAction.SaveAction("EndAlert", "NotificationService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في إنهاء تنبيه";
                _SystemAction.SaveAction("EndAlert", "NotificationService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteNotification(int NotificationId, int UserId, int BranchId)
        {
            try
            {
                Notification notification = _NotificationRepository.GetById(NotificationId);
                notification.IsDeleted = true;
                notification.DeleteDate = DateTime.Now;
                notification.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف الإشعار رقم " + NotificationId;
                _SystemAction.SaveAction("DeleteNotification", "NotificationService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في الإشعار رقم " + NotificationId; ;
                _SystemAction.SaveAction("DeleteNotification", "NotificationService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public GeneralMessage DeleteAlert(int NotificationId, int UserId, int BranchId)
        {
            try
            {
                Notification alert = _NotificationRepository.GetById(NotificationId);
                alert.IsDeleted = true;
                alert.DeleteDate = DateTime.Now;
                alert.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف التنبيه رقم " + NotificationId;
                _SystemAction.SaveAction("DeleteAlert", "NotificationService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف التنبيه رقم " + NotificationId; ;
               _SystemAction.SaveAction("DeleteAlert", "NotificationService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
         public async Task<IEnumerable<NotificationVM>> GetUserNotification(int UserId)
        {
            var Notifications =await _NotificationRepository.GetUserNotification(UserId);
            return Notifications;
        }
         public async Task<IEnumerable<NotificationVM>> GetUnReadUserNotification(int UserId)
        {
            return await _NotificationRepository.GetUnReadUserNotification(UserId);
        }

        public async Task<int> GetUnReadUserNotificationcount(int UserId)
        {
            return await _NotificationRepository.GetUnReadUserNotificationcount(UserId);
        }
        public async Task<IEnumerable<NotificationVM>> GetNotificationReceived(int UserId)
        {
            var Notifications = await _NotificationRepository.GetNotificationReceived(UserId);
            return Notifications;
        }
         public async Task<IEnumerable<NotificationVM>> GetNotificationTasksStart(int UserId)
        {
            var Notifications = await _NotificationRepository.GetNotificationTasksStart(UserId);
            return Notifications;
        }
         public async Task<IEnumerable<NotificationVM>> GetUserAlert(int UserId)
        {
            var User = _UserRepository.GetById(UserId);
            var Alerts = await _NotificationRepository.GetUserAlert(User);
            return Alerts;
        }
         public async Task<IEnumerable<NotificationVM>> NotificationsSent(int UserId)
        {
            var Notifications = await _NotificationRepository.NotificationsSent(UserId);
            return Notifications;
        }


         public async Task<IEnumerable<NotificationVM>> NotificationsSent2(int UserId)
        {
            var Notifications = await _NotificationRepository.NotificationsSent2(UserId);
            return Notifications;
        }
        public async Task<IEnumerable<GetOfficialPapersStatitecsVM>> GetOfficialDocsStatsecs(string Con)
        {
            return await _NotificationRepository.GetOfficialDocsStatsecs(Con);
        }

        public GeneralMessage SetNotificationStatus(int NotificationId, string Con, int UserId, int BranchId)
        {
            try
            {
                SqlConnection con = new SqlConnection(Con);
                con.Open();
                SqlCommand _cmd = new SqlCommand("exec NotificationStatusAssign " + NotificationId.ToString());
                _cmd.Connection = con;
                _cmd.ExecuteNonQuery();
                con.Close();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تحديث حالة الاشعار رقم " + NotificationId;
               _SystemAction.SaveAction("SetNotificationStatus", "NotificationService", 3, "تم التحديث بنجاح", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في تحديث الاشعار رقم " + NotificationId; ;
               _SystemAction.SaveAction("SetNotificationStatus", "NotificationService", 3, "فشل في التحديث", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage SetNotificationStatus2(List<int> NotificationId, string Con, int UserId, int BranchId)
        {
            try
            {
                SqlConnection con = new SqlConnection(Con);
                con.Open();
                foreach (int i in NotificationId)
                {
                    SqlCommand _cmd = new SqlCommand("exec NotificationStatusAssign " + i.ToString());
                    _cmd.Connection = con;
                    _cmd.ExecuteNonQuery();

                }


                con.Close();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تحديث حالة الاشعار رقم " + NotificationId;
               _SystemAction.SaveAction("SetNotificationStatus", "NotificationService", 3, "تم التحديث بنجاح", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully};
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في تحديث الاشعار رقم " + NotificationId; ;
               _SystemAction.SaveAction("SetNotificationStatus", "NotificationService", 3, "فشل في التحديث", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage HideAlert (int NotificationId, int UserId, int BranchId)
        {
            try
            {
                Notification notification = _NotificationRepository.GetById(NotificationId);
                notification.IsHidden = true;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف الإشعار رقم " + NotificationId;
                _SystemAction.SaveAction("DeleteNotification", "NotificationService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في الإشعار رقم " + NotificationId; ;
                _SystemAction.SaveAction("DeleteNotification", "NotificationService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }


        public GeneralMessage DeleteNotificationById(int notifcationID, int UserId, int BranchId)
        {
            Notification notification = _NotificationRepository.GetById(notifcationID);
            if (notification.ProjectId != null)
            {
                notification.IsHidden = true;
                notification.IsDeleted = false;
                notification.UpdateDate = DateTime.Now;
                notification.UpdateUser = UserId;
            }
            else
            {
                notification.IsHidden = true;
                notification.IsDeleted = true;
                notification.DeleteDate = DateTime.Now;
                notification.DeleteUser = UserId;
            }

            _TaamerProContext.SaveChanges();
            //-----------------------------------------------------------------------------------------------------------------
            string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            string ActionNote = " حذف الاشعار رقم " + notifcationID;
           _SystemAction.SaveAction("DeleteNotificationById", "NotificationService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
            //-----------------------------------------------------------------------------------------------------------------
            return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
        }

        /// <summary>
        /// Get Vacation, Loan acceptance and Cutody Free Notifications
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="NoteType">1: Vacation, 2: Loan, 3: Custody</param>
        /// <returns></returns>
        /// 
         public async Task<IEnumerable<NotificationVM>> GetUserBackupNotes(int UserId, int NoteType)
        {
            var notes = await _NotificationRepository.GetUnReadUserNotification(UserId);
            var backnote = notes.Where(s => s.Type == NoteType);

            return backnote;
        }
        public async Task<NotificationVM> GetUserBackupNotesAlert(int UserId)
        {
            var notes = await _NotificationRepository.GetUnReadUserNotificationbacupAlert(UserId);

            return notes.FirstOrDefault();
        }
         public async Task<IEnumerable<NotificationVM>> GetUserHomeNotes(int UserId, int NoteType)
        {
            var notes = await _NotificationRepository.GetUnReadUserNotification(UserId);
            switch (NoteType)
            {
                case (1):
                    {
                        notes = notes.Where(x => x.Name == "Resources.ResourceManager.GetString(General_Vacation, CultureInfo.CreateSpecificCulture(en))" ||
                                                 x.Name == "Resources.ResourceManager.GetString(General_Vacation, CultureInfo.CreateSpecificCulture(ar))").ToList();
                        break;
                    }
                case (2):
                    {
                        notes = notes.Where(x => x.Name == "Resources.ResourceManager.GetString(Emp_Loans, CultureInfo.CreateSpecificCulture(en))" ||
                                            x.Name == "Resources.ResourceManager.GetString(Emp_Loans, CultureInfo.CreateSpecificCulture(ar))").ToList();
                        break;
                    }
                case (3):
                    {
                        notes = notes.Where(x => x.Name == "Resources.ResourceManager.GetString(Notice_CustodyFinish, CultureInfo.CreateSpecificCulture(en))" ||
                                            x.Name == "Resources.ResourceManager.GetString(Notice_CustodyFinish, CultureInfo.CreateSpecificCulture(ar))").ToList();
                        break;
                    }
                default:
                    break;
            }

            return notes;
        }


        public GeneralMessage sendmobilenotification(int UserId, string Title, string Notody)
        {
            try
            {
                var user = _UserRepository.GetById(UserId);
                if (user != null)
                {
                    if (user.DeviceTokenId != null && user.DeviceTokenId != "")
                    {
                        var uri = _organizationsRepository.GetMatching(x => x.IsDeleted == false).FirstOrDefault().ApiBaseUri;
                        if (uri != null && uri != "")
                        {
                            //Generate Token
                            var token = getapitoken(UserId, uri);
                            if (user.DeviceType == 1)
                            {
                                using (var client = new HttpClient())
                                {
                                    //Base API URI
                                    client.BaseAddress = new Uri(uri);
                                    //JWT TOKEN
                                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                    client.DefaultRequestHeaders
                                    .Accept
                                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                    //HTTP POST API
                                    var responseTask = client.PostAsync("/api/Notifications/sendmobile?Deviceid=" + user.DeviceTokenId + "&Devicetype=1&Title=" + Title + "&Body=" + Notody + "", null);
                                    responseTask.Wait();


                                }




                            }
                            else if (user.DeviceType == 2)
                            {
                                using (var client = new HttpClient())
                                {
                                    //Base API URI
                                    client.BaseAddress = new Uri(uri);
                                    //JWT TOKEN
                                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                    client.DefaultRequestHeaders
                                    .Accept
                                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                    //HTTP POST API
                                    var responseTask = client.PostAsync("/api/Notifications/sendmobile?Deviceid=" + user.DeviceTokenId + "&Devicetype=2&Title=" + Title + "&Body=" + Notody + "", null);
                                    responseTask.Wait();


                                }

                            }
                            else { }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.sent_succesfully };
        }



        public string getapitoken(int UserId, string URI)
        {
            var result = "";
            try
            {
                var user = _UserRepository.GetById(UserId);
                if (user.DeviceTokenId != null && user.DeviceTokenId != "")
                {
                    if (user.DeviceType == 1)
                    {
                        using (var client = new HttpClient())
                        {
                            //Base URI
                            client.BaseAddress = new Uri(URI);
                            //HTTP GET


                            //Http GET 
                            //Get Token API
                            var responseTask = client.GetAsync("api/UserProfile/gettoken?userid=" + UserId + "");
                            responseTask.Wait();

                            var reslt = responseTask.Result;

                            result = reslt.Content.ReadAsStringAsync().Result;

                        }

                    }
                    else { }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }


    }
}

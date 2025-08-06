using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationVM>> GetAllNotifications(int ProjectId);
        Task<IEnumerable<NotificationVM>> GetAllAlerts(int BranchId);
        Task<IEnumerable<NotificationVM>> GetUserlAlerts(int BranchId, int UserId);
        Task<IEnumerable<NotificationVM>> GetUserlAlerts_Dashboard(int BranchId, int UserId);
        GeneralMessage SaveNotification(Notification notification, int UserId, int BranchId);
        Task<IEnumerable<NotificationVM>> GetUserNotification(int UserId);
        GeneralMessage DeleteNotification(int NotificationId, int UserId, int BranchId);
        GeneralMessage SaveAlert(Notification alert, int UserId, int BranchId);
        GeneralMessage EndAlert(int AlertId, int UserId, int BranchId);
        GeneralMessage DeleteAlert(int NotificationId, int UserId, int BranchId);
        Task<IEnumerable<NotificationVM>> GetUserAlert(int UserId);
        Task<IEnumerable<NotificationVM>> NotificationsSent(int UserId);
        Task<IEnumerable<NotificationVM>> NotificationsSent2(int UserId);
        Task<IEnumerable<NotificationVM>> GetNotificationReceived(int UserId);
        Task<IEnumerable<NotificationVM>> GetNotificationTasksStart(int UserId);

        Task<IEnumerable<NotificationVM>> GetUnReadUserNotification(int UserId);
        bool ReadUserNotification(int UserId);
        GeneralMessage ReadNotification(int NotiID);
        GeneralMessage ReadNotificationList(List<int> NotiID);
        Task<IEnumerable<GetOfficialPapersStatitecsVM>> GetOfficialDocsStatsecs(string Con);

        GeneralMessage SetNotificationStatus(int NotificationId, string Con, int UserId, int BranchId);
        GeneralMessage DeleteNotificationById(int notifcationID, int UserId, int BranchId);
        Task<IEnumerable<NotificationVM>> GetUserHomeNotes(int UserId, int NoteType);
        Task<IEnumerable<NotificationVM>> GetUserBackupNotes(int UserId, int NoteType);
        Task<IEnumerable<NotificationVM>> GetAllNotificationsBackup();
        GeneralMessage SetNotificationStatus2(List<int> NotificationId, string Con, int UserId, int BranchId);
        Task<NotificationVM> GetUserBackupNotesAlert(int UserId);
        GeneralMessage sendmobilenotification(int UserId, string Title, string Notody);

        GeneralMessage SaveNotification2(Notification2 notification, int UserId, int BranchId);
        GeneralMessage HideAlert(int NotificationId, int UserId, int BranchId);

        Task<int> GetUnReadUserNotificationcount(int UserId);
    }
}

using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface INotificationRepository :IRepository<Notification>
    {
        Task<IEnumerable<NotificationVM>> GetAllNotifications(int ProjectId);
        Task<IEnumerable<NotificationVM>> GetAllAlerts(int BranchId);
        Task<IEnumerable<NotificationVM>> GetUserlAlerts(int BranchId, int UserId);
        Task<IEnumerable<NotificationVM>> GetUserNotification(int UserId);
        Task<IEnumerable<NotificationVM>> GetUserAlert(Users User);
        Task<IEnumerable<NotificationVM>> NotificationsSent(int UserId);

        Task<IEnumerable<NotificationVM>> NotificationsSent2(int UserId);
        Task<IEnumerable<NotificationVM>> GetNotificationReceived(int UserId);
        Task<IEnumerable<NotificationVM>> GetNotificationTasksStart(int UserId);

        Task<IEnumerable<NotificationVM>> GetUnReadUserNotification(int UserId);
        Task<IEnumerable<NotificationVM>> GetUnReadUserNotificationbacup(int Type);
        Task<IEnumerable<NotificationVM>> GetAllNotificationsBackup();
        Task<long> GetMaxId();
        Task<int> GetNotificationCount();
        Task<int> GetAlertCount();
        Task<IEnumerable<GetOfficialPapersStatitecsVM>> GetOfficialDocsStatsecs(string Con);
        Task<IEnumerable<NotificationVM>> GetUnReadUserNotificationbacupAlert(int userid);
        Task<int> GetUnReadUserNotificationcount(int UserId);
    }
}

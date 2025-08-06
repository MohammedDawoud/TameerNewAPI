using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface INotificationSettingsService
    {
        Task<IEnumerable<NotificationSettingsVM>> GetAllNotificationSettings();
        GeneralMessage UpdateNotificationSettings(NotificationSettings notificationSettings, int UserId, int BranchId);
        //GeneralMessage DeleteNotificationSettings(int SettingId, int UserId);
    }
}

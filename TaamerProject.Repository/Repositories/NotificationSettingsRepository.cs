using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class NotificationSettingsRepository : INotificationSettingsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public NotificationSettingsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task< IEnumerable<NotificationSettingsVM>> GetAllNotificationSettings()
        {
            var notificationSettings = _TaamerProContext.NotificationSettings.Where(s => s.IsDeleted == false).Select(x => new NotificationSettingsVM
            {
                SettingId = x.SettingId,
                IDEndCount = x.IDEndCount,
                PassportCount = x.PassportCount,
                LicesnseCount = x.LicesnseCount,
                ContractCount = x.ContractCount,
                MedicalCount = x.MedicalCount,
                VacancyCount = x.VacancyCount,
                LoanCount = x.LoanCount,
                BranchId = x.BranchId,
            }).ToList();

       
            return notificationSettings;
        }


    }
}

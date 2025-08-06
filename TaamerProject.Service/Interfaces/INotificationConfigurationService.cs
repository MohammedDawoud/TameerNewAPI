using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Models.ViewModels;

namespace TaamerProject.Service.Interfaces
{
   public interface INotificationConfigurationService
    {
        Task<IEnumerable<NotificationConfigurationVM>> GetAll(string searchText);
        Task<NotificationConfigurationVM> GetById(int configurationId);
        GeneralMessage Save(NotificationConfiguration config, int userId, int branchId);
        GeneralMessage Delete(int configurationId, int userId, int branchId);
    }
}

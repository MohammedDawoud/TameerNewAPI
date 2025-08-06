using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Models.ViewModels;

namespace TaamerProject.Repository.Interfaces
{
    public interface INotificationConfigurationRepository
    {
        Task<IEnumerable<NotificationConfigurationVM>> GetAll(string searchText);
        Task<NotificationConfigurationVM> GetById(int configurationId);
    }
}

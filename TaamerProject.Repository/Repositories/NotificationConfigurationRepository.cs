using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Models.ViewModels;
using TaamerProject.Repository.Interfaces;

namespace TaamerProject.Repository.Repositories
{
    public class NotificationConfigurationRepository : INotificationConfigurationRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public NotificationConfigurationRepository(TaamerProjectContext projectContext)
        {
            _TaamerProContext = projectContext;
        }

        public TaamerProjectContext ProjectContext { get; }

        public Task<IEnumerable<NotificationConfigurationVM>> GetAll(string searchText)
        {
            var query = _TaamerProContext.NotificationConfigurations
                .Where(x => !x.IsDeleted && (string.IsNullOrEmpty(searchText) || x.Title.Contains(searchText)))
                .Select(x => new NotificationConfigurationVM
                {
                    ConfigurationId = x.ConfigurationId,
                    Code = x.Code,
                    Description = x.Description,
                    Title = x.Title,
                    To = x.To,
                    BranchId = x.BranchId,
                    Assignees = x.NotificationConfigurationsAssines != null
    ? x.NotificationConfigurationsAssines
        .Where(a => !a.IsDeleted)
        .Select(a => a.UserId)
        .ToList()
    : new List<int?>()

                }).AsEnumerable();

            return Task.FromResult(query);
        }

        public Task<NotificationConfigurationVM> GetById(int configurationId)
        {
            var result = _TaamerProContext.NotificationConfigurations
                .Where(x => x.ConfigurationId == configurationId && !x.IsDeleted)
                .Select(x => new NotificationConfigurationVM
                {
                    ConfigurationId = x.ConfigurationId,
                    Code = x.Code,
                    Description = x.Description,
                    Title = x.Title,
                    To = x.To,
                    BranchId = x.BranchId,
                    Assignees = x.NotificationConfigurationsAssines != null
    ? x.NotificationConfigurationsAssines
        .Where(a => !a.IsDeleted)
        .Select(a => a.UserId)
        .ToList()
    : new List<int?>()
                })
                .FirstOrDefault();

            return Task.FromResult(result);
        }
    }
}

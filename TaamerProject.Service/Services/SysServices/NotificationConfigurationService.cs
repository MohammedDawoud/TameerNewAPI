using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Models.ViewModels;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class NotificationConfigurationService : INotificationConfigurationService
    {
        private readonly TaamerProjectContext _context;
        private readonly ISystemAction _systemAction;
        private readonly IUsersRepository _usersRepository;
        private readonly INotificationConfigurationRepository configurationRepository;

        public NotificationConfigurationService(TaamerProjectContext context, 
            ISystemAction systemAction, IUsersRepository usersRepository,
            INotificationConfigurationRepository configurationRepository)
        {
            _context = context;
            _systemAction = systemAction;
            _usersRepository = usersRepository;
            this.configurationRepository = configurationRepository;
        }

        public async Task<IEnumerable<NotificationConfigurationVM>> GetAll(string searchText)
        {
            return await configurationRepository.GetAll(searchText);
        }

        public async Task<NotificationConfigurationVM> GetById(int configurationId)
        {
            return await configurationRepository.GetById(configurationId);
        }

        public GeneralMessage Save(NotificationConfiguration config, int userId, int branchId)
        {
            try
            {
                if (config.ConfigurationId == 0)
                {
                    
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكن اضافة اشعار جديد" };
                }
                else
                {
                    var existing = _context.NotificationConfigurations.Include(x => x.NotificationConfigurationsAssines)
.FirstOrDefault(x => x.ConfigurationId == config.ConfigurationId);
                    if (existing != null)
                    {
                        existing.Code = config.Code;
                        existing.Description = config.Description;
                        existing.Title = config.Title;
                        existing.To = config.To;
                        existing.BranchId = config.BranchId;
                        existing.UpdateUser = userId;
                        existing.UpdateDate = DateTime.Now;
                        // إزالة المعينين القدامى
                        if(existing.NotificationConfigurationsAssines !=null && existing.NotificationConfigurationsAssines.Count()>0)
                        _context.NotificationConfigurationsAssines
                            .RemoveRange(existing.NotificationConfigurationsAssines);

                        // إضافة المعينين الجدد
                        if (config.Assignees != null && config.Assignees.Any() && config.To==1)
                        {
                            existing.NotificationConfigurationsAssines = config.Assignees
                                .Select(a => new NotificationConfigurationsAssines
                                {
                                    ConfigurationId = config.ConfigurationId,
                                    UserId = a
                                }).ToList();
                        }


                        _context.SaveChanges();

                        string actionNote = $"تعديل إعداد تنبيه رقم {config.ConfigurationId}";
                        _systemAction.SaveAction("Save", nameof(NotificationConfigurationService), 2, Resources.General_EditedSuccessfully, "", "", DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), userId, branchId, actionNote, 1);

                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };
                    }
                }
            }
            catch (Exception)
            {
                string actionNote = "فشل في حفظ إعداد التنبيه";
                _systemAction.SaveAction("Save", nameof(NotificationConfigurationService), 1, Resources.General_SavedFailed, "", "", DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), userId, branchId, actionNote, 0);
            }

            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
        }

        public GeneralMessage Delete(int configurationId, int userId, int branchId)
        {
            try
            {
                var config = _context.NotificationConfigurations.FirstOrDefault(x => x.ConfigurationId == configurationId);
                if (config != null)
                {
                    config.IsDeleted = true;
                    config.DeleteUser = userId;
                    config.DeleteDate = DateTime.Now;

                    _context.SaveChanges();

                    string actionNote = $"تم حذف إعداد تنبيه رقم {configurationId}";
                    _systemAction.SaveAction("Delete", nameof(NotificationConfigurationService), 3, Resources.General_DeletedSuccessfully, "", "", DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), userId, branchId, actionNote, 1);

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
                }
            }
            catch (Exception)
            {
                string actionNote = $"فشل في حذف إعداد تنبيه رقم {configurationId}";
                _systemAction.SaveAction("Delete", nameof(NotificationConfigurationService), 3, Resources.General_DeletedFailed, "", "", DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), userId, branchId, actionNote, 0);
            }

            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
        }
    }
}

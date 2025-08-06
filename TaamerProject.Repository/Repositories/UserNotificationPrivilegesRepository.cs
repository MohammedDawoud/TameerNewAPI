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
    public class UserNotificationPrivilegesRepository : IUserNotificationPrivilegesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public UserNotificationPrivilegesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<UserNotificationPrivilegesVM>> GetUsersByPrivilegesIds(int? PrivId)
        {
            var users = _TaamerProContext.UserNotificationPrivileges.Where(s => s.IsDeleted == false && s.PrivilegeId == PrivId && s.Users.IsDeleted == false).Select(x => new
            {
                x.PrivilegeId,
                x.UserId,
                FullName = x.Users.FullName,
                x.Select,
                x.Update,
                x.Insert,
                x.Delete
            }).Select(m => new UserNotificationPrivilegesVM
            {
                PrivilegeId = m.PrivilegeId,
                UserId = m.UserId,
                FullName = m.FullName,
                Select = m.Select,
                Update = m.Update,
                Insert = m.Insert,
                Delete = m.Delete
            }).ToList();
            return users;
        }
    }
}

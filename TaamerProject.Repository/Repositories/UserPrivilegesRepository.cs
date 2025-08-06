using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace TaamerProject.Repository.Repositories
{
    public class UserPrivilegesRepository :  IUserPrivilegesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public UserPrivilegesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }


        public List<int> GetPrivilegesIdsByUserId(int UserId)
        {
            var PrivIds = new List<int>();
            var Privs = _TaamerProContext.UserPrivileges.Where(s => s.UserId == UserId && s.IsDeleted == false).OrderBy(o => o.UserPrivId).ToList();
            if (Privs != null && Privs.Count > 0)
            {
                PrivIds = Privs.Select(s => s.PrivilegeId ?? 0).ToList();
            }
            return PrivIds;
        }
    }
}

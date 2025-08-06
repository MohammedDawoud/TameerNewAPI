using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Repository.Interfaces;

namespace TaamerProject.Repository.Repositories
{
    public class ProUserPrivilegesRepository : IProUserPrivilegesRepository
    {

        private readonly TaamerProjectContext _TaamerProContext;

        public ProUserPrivilegesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<ProUserPrivilegesVM>> GetAllPriv(string SearchText, string Projectno, int BranchId)
        {
            var Privs = _TaamerProContext.ProUserPrivileges.Where(s => s.IsDeleted == false && s.Projectno == Projectno && s.Projects!.IsDeleted == false).Select(x => new ProUserPrivilegesVM
            {
                UserPrivId = x.UserPrivId,
                PrivilegeId = x.PrivilegeId,
                ProjectID = x.ProjectID,
                Projectno = x.Projectno,
                UserId = x.UserId,
                Select = x.Select,
                Insert = x.Insert,
                Update = x.Update,
                Delete = x.Delete,
                FullName = x.Users!.FullName,
                MangerId = x.Projects != null ? x.Projects.MangerId : 0,

            }).ToList().Select(s => new ProUserPrivilegesVM()
            {
                UserPrivId = s.UserPrivId,
                PrivilegeId = s.PrivilegeId,
                ProjectID = s.ProjectID,
                Projectno = s.Projectno,
                UserId = s.UserId,
                Select = s.Select,
                Insert = s.Insert,
                Update = s.Update,
                Delete = s.Delete,
                FullName = s.FullName,
                MangerId = s.MangerId,

            });
            if (SearchText != "")
            {
                Privs = Privs.Where(s => s.FullName!.Contains(SearchText.Trim())).ToList();
            }
            return Privs;
        }
        public async Task<IEnumerable<ProUserPrivilegesVM>> GetAllPrivUser(int UserId,int projectId)
        {
            var Privs = _TaamerProContext.ProUserPrivileges.Where(s => s.IsDeleted == false && s.ProjectID == projectId && s.UserId==UserId).Select(x => new ProUserPrivilegesVM
            {
                UserPrivId = x.UserPrivId,
                PrivilegeId = x.PrivilegeId,
                ProjectID = x.ProjectID,
                Projectno = x.Projectno,
                UserId = x.UserId,
                Select = x.Select,
                Insert = x.Insert,
                Update = x.Update,
                Delete = x.Delete,
                FullName = x.Users!.FullName,
                MangerId = x.Projects != null ? x.Projects.MangerId : 0,

            }).ToList().Select(s => new ProUserPrivilegesVM()
            {
                UserPrivId = s.UserPrivId,
                PrivilegeId = s.PrivilegeId,
                ProjectID = s.ProjectID,
                Projectno = s.Projectno,
                UserId = s.UserId,
                Select = s.Select,
                Insert = s.Insert,
                Update = s.Update,
                Delete = s.Delete,
                FullName = s.FullName,
                MangerId = s.MangerId,
            });
       
            return Privs;
        }

    }
}

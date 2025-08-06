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
    public class ProjectArchivesSeeRepository : IProjectArchivesSeeRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ProjectArchivesSeeRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<ProjectArchivesSeeVM>> GetProjectArchSee()
        {
            var ProjectArchSee = _TaamerProContext.ProjectArchivesSee.Where(s => s.IsDeleted == false && s.See_TypeID==1).Select(x => new ProjectArchivesSeeVM
            {
                ProArchSeeID = x.ProArchSeeID,
                ProArchReID = x.ProArchReID,
                UserId = x.UserId,
                Status = x.Status,
            }).ToList();
            return ProjectArchSee;
        }
        public async Task<IEnumerable<ProjectArchivesSeeVM>> GetProjectArchSee_Phases()
        {
            var ProjectArchSee = _TaamerProContext.ProjectArchivesSee.Where(s => s.IsDeleted == false && s.See_TypeID == 2).Select(x => new ProjectArchivesSeeVM
            {
                ProArchSeeID = x.ProArchSeeID,
                ProArchReID = x.ProArchReID,
                UserId = x.UserId,
                Status = x.Status,
            }).ToList();
            return ProjectArchSee;
        }
        public async Task<IEnumerable<ProjectArchivesSeeVM>> GetProjectArchSeeParm(int ProArchReID, int UserId)
        {
            var ProjectArchSee = _TaamerProContext.ProjectArchivesSee.Where(s => s.IsDeleted == false && s.ProArchReID == ProArchReID && s.UserId == UserId && s.See_TypeID == 1).Select(x => new ProjectArchivesSeeVM
            {
                ProArchSeeID = x.ProArchSeeID,
                ProArchReID = x.ProArchReID,
                UserId = x.UserId,
                Status = x.Status,
            }).ToList();
            return ProjectArchSee;
        }
        public async Task<IEnumerable<ProjectArchivesSeeVM>> GetProjectArchSeeParm_Phases(int ProArchReID, int UserId)
        {
            var ProjectArchSee = _TaamerProContext.ProjectArchivesSee.Where(s => s.IsDeleted == false && s.ProArchReID == ProArchReID && s.UserId == UserId && s.See_TypeID == 2).Select(x => new ProjectArchivesSeeVM
            {
                ProArchSeeID = x.ProArchSeeID,
                ProArchReID = x.ProArchReID,
                UserId = x.UserId,
                Status = x.Status,
            }).ToList();
            return ProjectArchSee;
        }

    }
}

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
    public class ProjectArchivesReRepository : IProjectArchivesReRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ProjectArchivesReRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<ProjectArchivesReVM>> GetProjectArchRe()
        {
            var ProjectArchRe = _TaamerProContext.ProjectArchivesRe.Where(s => s.IsDeleted == false && s.Re_TypeID == 1).Select(x => new ProjectArchivesReVM
            {
                ProArchReID = x.ProArchReID,
                ProjectId = x.ProjectId,
                ReDate = x.ReDate,
                ProjectNo = x.Project.ProjectNo,
                

            }).ToList();
            return ProjectArchRe;
        }
        public async Task<IEnumerable<ProjectArchivesReVM>> GetProjectArchRe_Phases()
        {
            var ProjectArchRe = _TaamerProContext.ProjectArchivesRe.Where(s => s.IsDeleted == false && s.Re_TypeID==2).Select(x => new ProjectArchivesReVM
            {
                ProArchReID = x.ProArchReID,
                ProjectId = x.ProjectId,
                ReDate = x.ReDate,
                ProjectNo = x.Project.ProjectNo,


            }).ToList();
            return ProjectArchRe;
        }

        public async Task<IEnumerable<ProjectArchivesReVM>> GetProjectArchReParm(int ProArchReID)
        {
            var ProjectArchRe = _TaamerProContext.ProjectArchivesRe.Where(s => s.IsDeleted == false && s.Re_TypeID == 1 && s.ProArchReID== ProArchReID).Select(x => new ProjectArchivesReVM
            {
                ProArchReID = x.ProArchReID,
                ProjectId = x.ProjectId,
                ReDate = x.ReDate,
                ProjectNo = x.Project.ProjectNo,
                CustomerName=x.Project.customer.CustomerNameAr,

            }).ToList();
            return ProjectArchRe;
        }
        public async Task<IEnumerable<ProjectArchivesReVM>> GetProjectArchReParm_Phases(int ProArchReID)
        {
            var ProjectArchRe = _TaamerProContext.ProjectArchivesRe.Where(s => s.IsDeleted == false && s.Re_TypeID == 2  &&  s.ProArchReID == ProArchReID).Select(x => new ProjectArchivesReVM
            {
                ProArchReID = x.ProArchReID,
                ProjectId = x.ProjectId,
                ReDate = x.ReDate,
                ProjectNo = x.Project.ProjectNo,
                CustomerName = x.Project.customer.CustomerNameAr,
                PhasesName = x.Phases.DescriptionAr,
                ProjectTypeName = x.Project.projecttype.NameAr,
                ProjectSubTypeName = x.Project.projectsubtype.NameAr,

            }).ToList();
            return ProjectArchRe;
        }


    }
}

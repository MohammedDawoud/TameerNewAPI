using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using System.Net;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Generic;

namespace TaamerProject.Service.Services
{
    public class ProjectArchivesReService :   IProjectArchivesReService
    {
        private readonly IProjectArchivesReRepository _ProjectArchivesReRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public ProjectArchivesReService(IProjectArchivesReRepository ProjectArchivesReRepository,TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _ProjectArchivesReRepository = ProjectArchivesReRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task<IEnumerable<ProjectArchivesReVM>> GetProjectArchRe()
        {
            var projects = _ProjectArchivesReRepository.GetProjectArchRe();
            return projects;
        }
        public Task<IEnumerable<ProjectArchivesReVM>> GetProjectArchRe_Phases()
        {
            var projects = _ProjectArchivesReRepository.GetProjectArchRe_Phases();
            return projects;
        }
        public Task<IEnumerable<ProjectArchivesReVM>> GetProjectArchReParm(int ProArchReID)
        {
            var projects = _ProjectArchivesReRepository.GetProjectArchReParm(ProArchReID);
            return projects;
        }
        public Task<IEnumerable<ProjectArchivesReVM>> GetProjectArchReParm_Phases(int ProArchReID)
        {
            var projects = _ProjectArchivesReRepository.GetProjectArchReParm_Phases(ProArchReID);
            return projects;
        }
    }
}

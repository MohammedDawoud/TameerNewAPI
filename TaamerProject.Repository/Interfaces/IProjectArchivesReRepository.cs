using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IProjectArchivesReRepository
    {
        Task<IEnumerable<ProjectArchivesReVM>> GetProjectArchRe();
        Task<IEnumerable<ProjectArchivesReVM>> GetProjectArchRe_Phases();

        Task<IEnumerable<ProjectArchivesReVM>> GetProjectArchReParm(int ProArchReID);
        Task<IEnumerable<ProjectArchivesReVM>> GetProjectArchReParm_Phases(int ProArchReID);



    }
}

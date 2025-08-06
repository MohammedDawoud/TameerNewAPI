using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IProjectArchivesReService  
    {
        Task<IEnumerable<ProjectArchivesReVM>> GetProjectArchRe();
        Task<IEnumerable<ProjectArchivesReVM>> GetProjectArchRe_Phases();

        Task<IEnumerable<ProjectArchivesReVM>> GetProjectArchReParm(int ProArchReID);
        Task<IEnumerable<ProjectArchivesReVM>> GetProjectArchReParm_Phases(int ProArchReID);



    }
}

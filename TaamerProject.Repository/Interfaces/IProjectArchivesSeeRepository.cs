using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IProjectArchivesSeeRepository
    {
        Task<IEnumerable<ProjectArchivesSeeVM>> GetProjectArchSee();
        Task<IEnumerable<ProjectArchivesSeeVM>> GetProjectArchSee_Phases();

        Task<IEnumerable<ProjectArchivesSeeVM>> GetProjectArchSeeParm(int ProArchReID, int UserId);
        Task<IEnumerable<ProjectArchivesSeeVM>> GetProjectArchSeeParm_Phases(int ProArchReID, int UserId);

    }
}

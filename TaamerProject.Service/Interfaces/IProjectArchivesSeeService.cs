using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IProjectArchivesSeeService  
    {
        Task<IEnumerable<ProjectArchivesSeeVM>> GetProjectArchSee();
        Task<IEnumerable<ProjectArchivesSeeVM>> GetProjectArchSee_Phases();

        Task<IEnumerable<ProjectArchivesSeeVM>> GetProjectArchSeeParm(int ProArchReID, int UserId);
        Task<IEnumerable<ProjectArchivesSeeVM>> GetProjectArchSeeParm_Phases(int ProArchReID, int UserId);

        GeneralMessage InsertProjectArchSee(int ProArchReID, int UserId, int BranchId);
        GeneralMessage InsertProjectArchSee_Phases(int ProArchReID, int UserId, int BranchId);




    }
}

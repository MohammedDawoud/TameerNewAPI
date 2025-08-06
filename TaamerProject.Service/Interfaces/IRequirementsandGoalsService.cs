using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IRequirementsandGoalsService  
    {
        Task<IEnumerable<RequirementsandGoalsVM>> GetAllrequirmentbyProjecttype(string Lang, int projecttypeid);
        GeneralMessage deleteprojectrequirment(int requirmentid, int UserId, int BranchId);
        Task<IEnumerable<RequirementsandGoalsVM>> GetAllrequirmentbyProjecttype2(string Lang, int projecttypeid, int projectsubtype);
    }
}

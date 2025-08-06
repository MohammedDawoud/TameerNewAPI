using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IProjectRequirementsGoalsService  
    {
        GeneralMessage saveprojectrequirment(List<ProjectRequirementsGoals> requirementsGoals, int projectid, int UserId, int BranchId);

        Task<IEnumerable<ProjectRequirementsGoalsVM>> GetAllrequirmentbyProjectid(string Lang, int projectid);
        Task<ProjectRequirementsGoalsVM> GetFilterdrequirmentbyProjectid(string Lang, int projectid);
       Task<ProjectRequirementsGoalsVM> GetAllrequirmentbyrequireid(string Lang, int projectid);
    }
}

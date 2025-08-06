using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IProjectRequirementsGoalsRepository
    {
        Task<IEnumerable<ProjectRequirementsGoalsVM>> GetAllrequirmentbyProjectid(string Lang, int projectid);
        Task<ProjectRequirementsGoalsVM> GetAllrequirmentbyrequireid(string Lang, int requireid);
    }
}

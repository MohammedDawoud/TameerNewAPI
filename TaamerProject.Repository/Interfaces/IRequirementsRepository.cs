using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IRequirementsRepository
    {
        Task<IEnumerable<RequirementsVM>> GetAllRequirements(int BranchId);
        Task<IEnumerable<RequirementsVM>> GetAllRequirementsByProjectId(int ProjectId, int BranchId);

    }
}

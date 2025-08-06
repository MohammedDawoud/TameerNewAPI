using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IProjectSubTypesService  
    {
        Task<IEnumerable<ProjectSubTypeVM>> GetAllProjectSubType(int BranchId);
        Task<IEnumerable<ProjectSubTypeVM>> GetAllProjectSubsByProjectTypeId(int ProjectTypeId, string SearchText, int BranchId);
        ProjectSubTypeVM GetTimePeriordBySubTypeId(int SubTypeId);

        GeneralMessage SaveProjectSubTypes(ProjectSubTypes subTypes ,int UserId, int BranchId);
        GeneralMessage DeleteSubTypes(int SubTypeId, int UserId, int BranchId);

    }
}

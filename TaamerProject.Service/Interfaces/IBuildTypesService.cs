using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IBuildTypesService
    {
        Task<IEnumerable<BuildTypesVM>> GetAllBuildTypes(string SearchText);
        GeneralMessage SaveBuildType(BuildTypes buildTypes, int UserId, int BranchId);
        GeneralMessage DeleteBuildTypes(int BuildTypeId, int UserId, int BranchId);
    }
}

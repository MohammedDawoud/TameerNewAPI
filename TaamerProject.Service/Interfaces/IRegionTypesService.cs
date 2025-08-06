using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IRegionTypesService  
    {
        Task<IEnumerable<RegionTypesVM>> GetAllRegionTypes(string SearchTex);
        GeneralMessage SaveRegionTypes(RegionTypes regionTypes,int UserId, int BranchId);
        GeneralMessage DeleteRegionTypes(int RegionTypeId ,int UserId, int BranchId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IGuideDepartmentDetailsService  
    {
        Task<IEnumerable<GuideDepartmentDetailsVM>> GetAllDepDetails(int DepId, string searchStr, int? DepDetailId = null);
        GeneralMessage SaveDetails(GuideDepartmentDetails DepDetails, int UserId,int BranchId);
        GeneralMessage DeleteDetails(int DepDetails, int UserId, int BranchId);
         Task<List<GuideDepartmentDetailsVM_Grouped>> GetAllDepDetails2(string searchStr);
    }
}

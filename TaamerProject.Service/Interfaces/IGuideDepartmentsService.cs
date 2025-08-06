using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IGuideDepartmentsService 
    {
        Task<IEnumerable<GuideDepartmentsVM>> GetAllDeps(string lang, int? DepId = null);
        GeneralMessage SaveDepartment(GuideDepartments Dep, int UserId,int BranchId);
         GeneralMessage DeleteDepartment(int DepId, int UserId, int BranchId);
    }
}

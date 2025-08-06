using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IExternalEmployeesService  
    {
        Task<IEnumerable<ExternalEmployeesVM>> GetAllExternalEmployees(int? DepartmentId, string SearchText, int BranchId);
        GeneralMessage SaveExternalEmployees(ExternalEmployees externalEmployees, int UserId, int BranchId);
        GeneralMessage DeleteExternalEmployees(int EmpId, int UserId,int BranchId);
        IEnumerable<object> FillExternalEmployeeSelect(int? DepartmentId, int BranchId);
    }
}

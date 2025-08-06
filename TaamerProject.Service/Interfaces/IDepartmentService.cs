using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IDepartmentService  
    {
        Task<IEnumerable<DepartmentVM>> GetAllDepartment(string SearchText, int BranchId);
        Task<IEnumerable<DepartmentVM>> GetAllDepartmentbyType(int? Type, int BranchId, string SearchText);
        Task<IEnumerable<DepartmentVM>> GetAllDepartmentbyTypeUser(int? Type, int BranchId, string SearchText);
        GeneralMessage SaveDepartment(Department department, int UserId, int BranchId);
        GeneralMessage DeleteDepartment(int DepartmentId, int UserId,int BranchId);
        Task<IEnumerable<DepartmentVM>> GetExternalDepartment();

        Task<IEnumerable<DepartmentVM>> GetDepartmentbyid(int DeptId);
    }
}

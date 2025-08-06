using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IDepartmentRepository 
    {
        Task<IEnumerable<DepartmentVM>> GetAllDepartment(string SearchText,int BranchId);
        Task<IEnumerable<DepartmentVM>> GetAllDepartmentbyTypeUser(int? Type, int BranchId, string SearchText);
        Task<IEnumerable<DepartmentVM>> GetAllDepartmentbyType(int? Type, int BranchId, string SearchText);
        Task<IEnumerable<DepartmentVM>> GetExternalDepartment();

        Task<IEnumerable<DepartmentVM>> GetDepartmentbyid(int DeptId);
    }
}

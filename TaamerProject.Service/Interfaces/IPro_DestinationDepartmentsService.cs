using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IPro_DestinationDepartmentsService
    {
        Task<IEnumerable<Pro_DestinationDepartmentsVM>> GetAllDestinationDepartments();
        Task<IEnumerable<Pro_DestinationDepartmentsVM>> GetAllDestinationDepartmentsByTypeId(int TypeId);

        GeneralMessage SaveDestinationDepartment(Pro_DestinationDepartments DestinationDepartment, int UserId, int BranchId);
        GeneralMessage DeleteDestinationDepartment(int DepartmentId, int UserId, int BranchId);
    }
}

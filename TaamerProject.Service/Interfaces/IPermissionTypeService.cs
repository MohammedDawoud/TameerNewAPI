using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Models.ViewModels;

namespace TaamerProject.Service.Interfaces
{
    public interface IPermissionTypeService
    {
        Task<IEnumerable<PermissionTypeVM>> GetAllPermissionTypes(string SearchText);
        GeneralMessage SavePermissionType(PermissionType permissionType, int UserId, int BranchId);
        GeneralMessage DeletePermissionType(int TypeId, int UserId, int BranchId);
        Task<IEnumerable<PermissionTypeVM>> FillPermissionTypeSelect(string SearchText = "");

    }
}

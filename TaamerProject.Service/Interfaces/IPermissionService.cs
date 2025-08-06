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
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionsVM>> GetAllPermissions(int? EmpId, int? Type, int? Status, string? FromDate, string? ToDate, string SearchText);
        GeneralMessage SavePermission(Permissions permission, int UserId, int BranchId, string Lang, string Url, string ImgUrl);
        GeneralMessage Updatepermission(int PermissionId, int UserId, int BranchId, string Lang, int Type, string Con, string Url, string ImgUrl, string? reason);
        GeneralMessage DeletePermissions(int PermissionId, int UserId, int BranchId);
        Task<IEnumerable<PermissionsVM>> GetAllPermissions(int? EmpId);
    }
}

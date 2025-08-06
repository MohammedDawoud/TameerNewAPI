using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.ViewModels;

namespace TaamerProject.Repository.Interfaces
{
    public interface IPermissionsRepository
    {
        Task<IEnumerable<PermissionsVM>> GetAllPermissions(int? EmpId, int? Type, int? Status, string? FromDate, string? ToDate, string SearchText);
        Task<IEnumerable<PermissionsVM>> GetAllPermissions(int? EmpId);
    }
}

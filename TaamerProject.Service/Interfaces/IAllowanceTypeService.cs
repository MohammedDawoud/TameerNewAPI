using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IAllowanceTypeService
    {
        Task<IEnumerable<AllowanceTypeVM>> GetAllAllowancesTypes(string SearchText, bool? IsSalaryPart = false);
        GeneralMessage SaveAllowanceType(AllowanceType allowanceType, int UserId, int BranchId);
        GeneralMessage DeleteAllowanceType(int AllowanceTypeId, int UserId, int BranchId);
        IEnumerable<object> FillAllowanceTypeSelect(string SearchText = "");
    }
}

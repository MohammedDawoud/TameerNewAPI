using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IOutInBoxTypeService
    {
        Task<IEnumerable<OutInBoxTypeVM>> GetAllOutInBoxTypes(string SearchText, int BranchId);
        GeneralMessage SaveOutInBoxType(OutInBoxType OutInBoxType, int userId, int BranchId);
        GeneralMessage DeleteOutInBoxType(int TypeId, int userId, int BranchId);
        IEnumerable<object> FillOutInBoxTypeSelect(int BranchId);
    }
}

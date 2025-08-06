using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ICarMovementsTypeService
    {
        Task<IEnumerable<CarMovementsTypeVM>> GetAllTypes(string SearchText);
        GeneralMessage SaveItemType(CarMovementsType carMovementType, int UserId, int BranchId);
        GeneralMessage DeleteType(int TypeId, int UserId, int BranchId);
        IEnumerable<object> FillCarMovmentsTypeSelect(string SearchText = "");
    }
}

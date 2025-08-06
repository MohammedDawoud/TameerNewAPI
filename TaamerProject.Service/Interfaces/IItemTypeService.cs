using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IItemTypeService
    {
        Task<IEnumerable<ItemTypeVM>> GetAllItemTypes(string SearchText);
        GeneralMessage SaveItemType(ItemType itemType, int UserId, int BranchId);
        GeneralMessage DeleteItemType(int ItemTypeId, int UserId, int BranchId);
        IEnumerable<object> FillItemTypeSelect(string SearchText = "");
    }
}

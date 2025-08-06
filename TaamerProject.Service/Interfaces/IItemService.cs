using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<ItemVM>> GetAllItems(string lang, int? typeId = null);
        GeneralMessage SaveItem(Item item, int UserId, int BranchId);
        GeneralMessage DeleteItem(int ItemId, int UserId, int BranchId);
        IEnumerable<object> FillItemSelect();

        IEnumerable<object> FillItemSelectSQL(string Con);
        IEnumerable<object> FillItemSelectSQL(string Con, int ItemId);


        //IEnumerable<object> FillSelectItem(string lang, int BranchId, int ItemId);
        IEnumerable<object> FillItemCarSelect();
        IEnumerable<ItemVM> SearchItems(ItemVM ItemsSearch, string lang);
        bool IsCar(int ItemId);
    }
}

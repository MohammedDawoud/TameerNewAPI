using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IItemRepository : IRepository<Item>    
    {
        Task<IEnumerable<ItemVM>> GetAllItems(string lang, int? typeId = null);
        Task<IEnumerable<ItemVM>> SearchItems(ItemVM ItemsSearch , string lang);
        //IEnumerable<object> FillSelectItem(string lang, int BranchId, int ItemId);
    }
}
 
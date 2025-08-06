
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using Bayanatech.TameerPro.Repository;

namespace TaamerProject.Repository.Repositories
{
    public class ItemTypeRepository : IItemTypeRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ItemTypeRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<ItemTypeVM>> GetAllItemTypes(string SearchText)
        {
            if (SearchText == "")
            {
                var ItemTypes = _TaamerProContext.ItemType.Where(s => s.IsDeleted == false).Select(x => new ItemTypeVM
                {
                    ItemTypeId = x.ItemTypeId,
                    Code = x.Code,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Notes = x.Notes,
                    UserId = x.UserId
                });
                return ItemTypes;
            }
            else
            {
                var ItemTypes = _TaamerProContext.ItemType.Where(s => s.IsDeleted == false  && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText))).Select(x => new ItemTypeVM
                {
                    ItemTypeId = x.ItemTypeId,
                    Code = x.Code,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Notes = x.Notes,
                    UserId = x.UserId
                });
                return ItemTypes;
            }
        }
    }
}

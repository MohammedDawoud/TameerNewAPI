using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ItemRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public Item Add(Item entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Item> AddRange(IEnumerable<Item> entities)
        {
            throw new NotImplementedException();
        }

        public bool Exists(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ItemVM>> FillSelectItem(string lang, int BranchId, int ItemId)
        {
            var Item = _TaamerProContext.Item.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.ItemId == ItemId).Select(x => new ItemVM
            {
                ItemId = x.ItemId,
                ItemName = x.NameAr
            }).ToList();
            return Item;
        }

        public IEnumerable<Item> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ItemVM>> GetAllItems(string lang, int? typeId = null)
        {
            var items = _TaamerProContext.Item.Where(s => s.IsDeleted == false && (typeId.HasValue? s.TypeId == typeId.Value : true)).Select(x => new ItemVM
            {
                ItemId = x.ItemId,
                ItemName = lang == "ltr" ? x.NameEn : x.NameAr,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                TypeId = x.TypeId,
                Quantity = x.Quantity,
                Price = x.Price,
                SachetNo = x.SachetNo,
                FormNo = x.FormNo,
                Color = x.Color,
                IssuancePlace = x.IssuancePlace,
                IssuanceDate = x.IssuanceDate,
                IssuanceHijriDate = x.IssuanceHijriDate,
                IssuanceEndDate = x.IssuanceEndDate,
                IssuanceEndHijriDate = x.IssuanceEndHijriDate,
                SupplyDate = x.SupplyDate,
                SupplyHijriDate = x.SupplyHijriDate,
                PlateNo = x.PlateNo,
                InsuranceNo = x.InsuranceNo,
                InsuranceEndDate = x.InsuranceEndDate,
                InsuranceEndHijriDate = x.InsuranceEndHijriDate,
                LiscenceFileUrl = x.LiscenceFileUrl,
                InsuranceFileUrl = x.InsuranceFileUrl,
                BranchId = x.BranchId,
                Status = x.Status,
            }).ToList();
          
            return items;
        }

        public Item GetById(int Id)
        {
           return _TaamerProContext.Item.Where(x => x.ItemId == Id).FirstOrDefault();
        }

        public IEnumerable<Item> GetMatching(Func<Item, bool> where)
        {
            return _TaamerProContext.Item.Where(where).ToList<Item>();
        }

        public IQueryable<Item> Queryable()
        {
            throw new NotImplementedException();
        }

        public void Remove(Item entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void RemoveMatching(Func<Item, bool> where)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Item> entities)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ItemVM>> SearchItems(ItemVM ItemsSearch,string lang)
        {
            var Items = _TaamerProContext.Item.Where(s => s.IsDeleted == false && (s.NameAr == ItemsSearch.NameAr || ItemsSearch.NameAr == null) &&
                                                (s.TypeId == ItemsSearch.TypeId || ItemsSearch.TypeId == null) &&
                                                (s.Color == ItemsSearch.Color || ItemsSearch.Color == null))
                                                .Select(x => new ItemVM
                                                {
                                                    ItemId = x.ItemId,
                                                    ItemName = lang == "ltr" ? x.NameEn : x.NameAr,
                                                    NameAr = x.NameAr,
                                                    NameEn = x.NameEn,
                                                    TypeId = x.TypeId,
                                                    Quantity = x.Quantity,
                                                    Price = x.Price,
                                                    SachetNo = x.SachetNo,
                                                    FormNo = x.FormNo,
                                                    Color = x.Color,
                                                    IssuancePlace = x.IssuancePlace,
                                                    IssuanceDate = x.IssuanceDate,
                                                    IssuanceHijriDate = x.IssuanceHijriDate,
                                                    IssuanceEndDate = x.IssuanceEndDate,
                                                    IssuanceEndHijriDate = x.IssuanceEndHijriDate,
                                                    SupplyDate = x.SupplyDate,
                                                    SupplyHijriDate = x.SupplyHijriDate,
                                                    PlateNo = x.PlateNo,
                                                    InsuranceNo = x.InsuranceNo,
                                                    InsuranceEndDate = x.InsuranceEndDate,
                                                    InsuranceEndHijriDate = x.InsuranceEndHijriDate,
                                                    LiscenceFileUrl = x.LiscenceFileUrl,
                                                    InsuranceFileUrl = x.InsuranceFileUrl,
                                                    BranchId = x.BranchId,
                                                    Status = x.Status,
                                                }).ToList();
            return Items;
        }

        public void Update(Item entityToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}



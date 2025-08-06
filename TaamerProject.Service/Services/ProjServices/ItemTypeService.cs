using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using Bayanatech.TameerPro.Repository;
using System.Net;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class ItemTypeService : IItemTypeService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IItemTypeRepository _ItemTypeRepository;



        public ItemTypeService(TaamerProjectContext dataContext, ISystemAction systemAction , IItemTypeRepository itemTypeRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _ItemTypeRepository = itemTypeRepository;
        }

        public async Task<IEnumerable<ItemTypeVM>> GetAllItemTypes(string SearchText)
        {
            var ItemTypes =await _ItemTypeRepository.GetAllItemTypes(SearchText??"");
            return ItemTypes;
        }
        public GeneralMessage SaveItemType(ItemType itemType, int UserId, int BranchId)
        {
            try
            {
                var codeExist = _TaamerProContext.ItemType.Where(s => s.IsDeleted == false && s.ItemTypeId != itemType.ItemTypeId && s.Code == itemType.Code).FirstOrDefault();
                if (codeExist != null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ نوع عهدة";
                   _SystemAction.SaveAction("SaveItemType", "ItemTypeService", 1, Resources.TheCodeAlreadyExists, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.TheCodeAlreadyExists };
                }

                if (itemType.ItemTypeId == 0)
                {
                    itemType.AddUser = UserId;
                    itemType.AddDate = DateTime.Now;
                    itemType.Code = (_TaamerProContext.ItemType.AsQueryable().Count() + 1).ToString();
                    _TaamerProContext.ItemType.Add(itemType);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة نوع عهدة جديدة";
                    _SystemAction.SaveAction("SaveItemType", "ItemTypeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var ItemTypeUpdated = _TaamerProContext.ItemType.Where(x=>x.ItemTypeId==itemType.ItemTypeId).FirstOrDefault();
                    if (ItemTypeUpdated != null)
                    {
                        ItemTypeUpdated.Code = itemType.Code;
                        ItemTypeUpdated.NameAr = itemType.NameAr;
                        ItemTypeUpdated.NameEn = itemType.NameEn;
                        ItemTypeUpdated.Notes = itemType.Notes;
                        ItemTypeUpdated.UserId = itemType.UserId;
                        ItemTypeUpdated.UpdateUser = UserId;
                        ItemTypeUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل نوع عهدة رقم " + itemType.ItemTypeId;
                    _SystemAction.SaveAction("SaveItemType", "ItemTypeService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ نوع عهدة";
                _SystemAction.SaveAction("SaveItemType", "ItemTypeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteItemType(int ItemTypeId, int UserId, int BranchId)
        {
            try
            {
                ItemType itemType = _TaamerProContext.ItemType.Where(x => x.ItemTypeId == ItemTypeId).FirstOrDefault();
                itemType.IsDeleted = true;
                itemType.DeleteDate = DateTime.Now;
                itemType.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote4 = " حذف نوع عهدة رقم " + ItemTypeId;
                _SystemAction.SaveAction("DeleteItemType", "ItemTypeService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate4, UserId, BranchId, ActionNote4, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حذف نوع عهدة";
                _SystemAction.SaveAction("DeleteItemType", "ItemTypeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public IEnumerable<object> FillItemTypeSelect(string SearchText = "")
        {
            return _ItemTypeRepository.GetAllItemTypes(SearchText).Result.Select(s => new
            {
                Id = s.ItemTypeId,
                Name = s.NameAr
            });
        }
    }
}

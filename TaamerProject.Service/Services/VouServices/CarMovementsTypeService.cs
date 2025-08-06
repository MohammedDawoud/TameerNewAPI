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
using System.Net;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class CarMovementsTypeService : ICarMovementsTypeService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly ICarMovementsTypeRepository _CarMovementsTypeRepository;
        private readonly IUsersRepository _usersRepository;


        public CarMovementsTypeService(TaamerProjectContext dataContext, ISystemAction systemAction, ICarMovementsTypeRepository carMovementsRepository, IUsersRepository usersRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            this._usersRepository = usersRepository;
            _CarMovementsTypeRepository = carMovementsRepository;
        }

        public async Task<IEnumerable<CarMovementsTypeVM>> GetAllTypes(string SearchText)
        {
            var Types = await _CarMovementsTypeRepository.GetAllCarMovmentsTypes(SearchText);
            return Types;
        }
        public GeneralMessage SaveItemType(CarMovementsType carMovementType, int UserId, int BranchId)
        {
            try
            {
                var codeExist = _TaamerProContext.CarMovementsType.Where(s => s.IsDeleted == false && s.TypeId != carMovementType.TypeId && s.Code == carMovementType.Code).FirstOrDefault();
                if (codeExist != null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ نوع حركة سيارة";
                   _SystemAction.SaveAction("SaveItemType", "CarMovementsTypeService", 1, Resources.TheCodeAlreadyExists, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.TheCodeAlreadyExists };
                }

                if (carMovementType.TypeId == 0)
                {
                    carMovementType.AddUser = UserId;
                    carMovementType.AddDate = DateTime.Now;
                    carMovementType.Code = (_TaamerProContext.CarMovementsType.AsQueryable().Count() + 1).ToString();
                    _TaamerProContext.CarMovementsType.Add(carMovementType);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة نوع حركة سيارة جديدة";
                    _SystemAction.SaveAction("SaveItemType", "CarMovementsTypeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else if (carMovementType.TypeId == 1 || carMovementType.TypeId == 2 || carMovementType.TypeId == 3 || carMovementType.TypeId == 4 || carMovementType.TypeId == 5 || carMovementType.TypeId == 6)
                {
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في تعديل نوع حركة سيارة";
                    _SystemAction.SaveAction("SaveItemType", "CarMovementsTypeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedFailed };
                }
                else
                {
                    var ItemTypeUpdated = _TaamerProContext.CarMovementsType.Where(x=>x.TypeId==carMovementType.TypeId).FirstOrDefault();
                    if (ItemTypeUpdated != null)
                    {
                        ItemTypeUpdated.Code = carMovementType.Code;
                        ItemTypeUpdated.NameAr = carMovementType.NameAr;
                        ItemTypeUpdated.NameEn = carMovementType.NameEn;
                        ItemTypeUpdated.Notes = carMovementType.Notes;
                        ItemTypeUpdated.UpdateUser = UserId;
                        ItemTypeUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل نوع حركة سيارة رقم " + carMovementType.TypeId;
                    _SystemAction.SaveAction("SaveItemType", "CarMovementsTypeService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ نوع حركة سيارة";
                _SystemAction.SaveAction("SaveItemType", "CarMovementsTypeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteType(int TypeId, int UserId, int BranchId)
        {
            try
            {
                if (TypeId == 1 || TypeId == 2 || TypeId == 3 || TypeId == 4 || TypeId == 5 || TypeId == 6)
                {
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حذف نوع حركة سيارة";
                    _SystemAction.SaveAction("SaveItemType", "CarMovementsTypeService", 1, "لا يمكنك حذف علي هذا النوع", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.CanNotDeleteThisItem };
                }
                CarMovementsType itemType = _TaamerProContext.CarMovementsType.Where(x=>x.TypeId==TypeId).FirstOrDefault();
                itemType.IsDeleted = true;
                itemType.DeleteDate = DateTime.Now;
                itemType.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote4 = " حذف نوع حركة سيارة رقم " + TypeId;
                _SystemAction.SaveAction("DeleteType", "CarMovementsTypeService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate4, UserId, BranchId, ActionNote4, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حذف حركة سيارة عهدة";
                _SystemAction.SaveAction("DeleteType", "CarMovementsTypeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public IEnumerable<object> FillCarMovmentsTypeSelect(string SearchText = "")
        {
            return _CarMovementsTypeRepository.GetAllCarMovmentsTypes(SearchText).Result.Select(s => new
            {
                Id = s.TypeId,
                Name = s.NameAr
            });
        }
    }
}

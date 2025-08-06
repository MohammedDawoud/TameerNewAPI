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
    public class ModelTypeService : IModelTypeService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IModelTypeRepository _ModelTypeRepository;



        public ModelTypeService(TaamerProjectContext dataContext, ISystemAction systemAction, IModelTypeRepository modelTypeRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _ModelTypeRepository = modelTypeRepository;
        }
        public async Task<IEnumerable<ModelTypeVM>> GetAllModelTypes(int BranchId)
        {
            var ModelTypes =await _ModelTypeRepository.GetAllModelTypes(BranchId);
            return ModelTypes;
        }
        public GeneralMessage SaveModelType(ModelType modelType, int UserId, int BranchId)
        {
            try
            {
                if (modelType.ModelTypeId == 0)
                {
                    modelType.BranchId = BranchId;
                    modelType.AddUser = UserId;
                    modelType.AddDate = DateTime.Now;
                    _TaamerProContext.ModelType.Add(modelType);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة خدمة جديدة";
                   _SystemAction.SaveAction("SaveModelType", "ModelTypeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var ModelTypeUpdated = _TaamerProContext.ModelType.Where(x=>x.ModelTypeId==modelType.ModelTypeId).FirstOrDefault();
                    if (ModelTypeUpdated != null)
                    {
                        ModelTypeUpdated.NameAr = modelType.NameAr;
                        ModelTypeUpdated.NameEn = modelType.NameEn;
                        ModelTypeUpdated.UpdateUser = UserId;
                        ModelTypeUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل خدمة رقم " + modelType.ModelTypeId;
                    _SystemAction.SaveAction("SaveModelType", "ModelTypeService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };


                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الخدمة";
                _SystemAction.SaveAction("SaveModelType", "ModelTypeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteModelType(int ModelTypeId, int UserId, int BranchId)
        {
            try
            {
                ModelType modelType = _TaamerProContext.ModelType.Where(x=>x.ModelTypeId==ModelTypeId).FirstOrDefault();
                modelType.IsDeleted = true;
                modelType.DeleteDate = DateTime.Now;
                modelType.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف الخدمة رقم " + ModelTypeId;
                _SystemAction.SaveAction("DeleteModelType", "ModelTypeService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف الخدمة رقم " + ModelTypeId; ;
                _SystemAction.SaveAction("DeleteModelType", "ModelTypeService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public IEnumerable<object> FillModelTypeSelect(int BranchId)
        {
            return _ModelTypeRepository.GetAllModelTypes(BranchId).Result.Select(s => new
            {
                Id = s.ModelTypeId,
                Name = s.NameAr
            });
        }
    }
}

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
    public class ModelService : IModelService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IModelRepository _ModelRepository;



        public ModelService(TaamerProjectContext dataContext, ISystemAction systemAction,IModelRepository modelRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _ModelRepository = modelRepository;
        }
        public async Task<IEnumerable<ModelVM>> GetAllModels(int BranchId)
        {
            var Models = await _ModelRepository.GetAllModels(BranchId);
            return Models;
        }
        public GeneralMessage SaveModel(Model model, int UserId, int BranchId)
        {
            try
            {
                if (model.ModelId == 0)
                {
                    model.AddUser = UserId;
                    model.BranchId = BranchId;
                    model.AddDate = DateTime.Now;
                    _TaamerProContext.Model.Add(model);
                    _TaamerProContext.SaveChanges();
                    foreach (var item in model.ModelRequirementsIds.ToList())
                    {
                        var modelReq = new ModelRequirements();
                        modelReq.ModelId = model.ModelId;
                        modelReq.RequirementId = item;
                        modelReq.BranchId = BranchId;
                        modelReq.AddUser = UserId;
                        modelReq.AddDate = DateTime.Now;
                        _TaamerProContext.ModelRequirements.Add(modelReq);
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة خدمة جديدة";
                   _SystemAction.SaveAction("SaveModel", "ModelService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var ModelReqUpdated = _TaamerProContext.Model.Where(x=>x.ModelId==model.ModelId).FirstOrDefault();
                    if (ModelReqUpdated != null)
                    {
                        ModelReqUpdated.ModelName = model.ModelName;
                        ModelReqUpdated.AddUser = UserId;
                        ModelReqUpdated.AddDate = DateTime.Now;

                        var ModelRequiremnts = _TaamerProContext.ModelRequirements.Where(s => s.ModelId == model.ModelId).ToList();
                        _TaamerProContext.ModelRequirements.RemoveRange(ModelRequiremnts);
                        foreach (var item in model.ModelRequirementsIds.ToList())
                        {
                            var modelReq = new ModelRequirements();
                            modelReq.ModelId = model.ModelId;
                            modelReq.RequirementId = item;
                            modelReq.BranchId = BranchId;
                            modelReq.AddUser = UserId;
                            modelReq.AddDate = DateTime.Now;
                            _TaamerProContext.ModelRequirements.Add(modelReq);
                        }
                        _TaamerProContext.Model.Add(ModelReqUpdated);
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل خدمة رقم " + model.ModelId;
                   _SystemAction.SaveAction("SaveModel", "ModelService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };

                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الخدمة";
               _SystemAction.SaveAction("SaveClause", "ModelService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteModel(int ModelId, int UserId, int BranchId)
        {
            try
            {
                Model model = _TaamerProContext.Model.Where(x=>x.ModelId==ModelId).FirstOrDefault();
                model.IsDeleted = true;
                model.DeleteDate = DateTime.Now;
                model.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف خدمة رقم " + ModelId;
               _SystemAction.SaveAction("DeleteClause", "ModelService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف الخدمة رقم " + ModelId; ;
               _SystemAction.SaveAction("DeleteModel", "ModelService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
    }
}

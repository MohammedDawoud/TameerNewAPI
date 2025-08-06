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
    public class ModelRequirementsService : IModelRequirementsService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IModelRequirementsRepository _ModelRequirementsRepository;



        public ModelRequirementsService(TaamerProjectContext dataContext, ISystemAction systemAction, IModelRequirementsRepository modelRequirementsRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _ModelRequirementsRepository = modelRequirementsRepository;
        }
        public async Task<IEnumerable<ModelRequirementsVM>> GetAllModelRequirements(int BranchId)
        {
            var modelRequirements =await _ModelRequirementsRepository.GetAllModelRequirements(BranchId);
            return modelRequirements;
        }
        public GeneralMessage SaveModelRequirements(ModelRequirements modelRequirements, int UserId, int BranchId)
        {
            try
            {
                if (modelRequirements.ModelReqId == 0)
                {
                    modelRequirements.BranchId = BranchId;
                    modelRequirements.AddUser = UserId;
                    modelRequirements.AddDate = DateTime.Now;
                    _TaamerProContext.ModelRequirements.Add(modelRequirements);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة  جديد";
                   _SystemAction.SaveAction("SaveModelRequirements", "ModelRequirementsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    var RequirementsUpdated = _TaamerProContext.ModelRequirements.Where(x=>x.ModelReqId==modelRequirements.RequirementId).FirstOrDefault();
                    if (RequirementsUpdated != null)
                    {
                        RequirementsUpdated.RequirementId = modelRequirements.RequirementId;
                        RequirementsUpdated.ModelId = modelRequirements.ModelId;
                        //RequirementsUpdated.BranchId = modelRequirements.BranchId;
                        RequirementsUpdated.UpdateUser = UserId;
                        RequirementsUpdated.UpdateDate = DateTime.Now;

                    }

                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل المتطلب رقم " + modelRequirements.RequirementId;
                    _SystemAction.SaveAction("SaveModelRequirements", "ModelRequirementsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };
                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ المتطلب";
                _SystemAction.SaveAction("SaveModelRequirements", "ModelRequirementsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteModelRequirement(int ModelReqId, int UserId, int BranchId)
        {
            try
            {
                ModelRequirements modelRequirements = _TaamerProContext.ModelRequirements.Where(x => x.ModelReqId == ModelReqId).FirstOrDefault();
                modelRequirements.IsDeleted = true;
                modelRequirements.DeleteDate = DateTime.Now;
                modelRequirements.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف المتطلب رقم " + ModelReqId;
                _SystemAction.SaveAction("DeleteModelRequirement", "ModelRequirementsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف المتطلب رقم " + ModelReqId; ;
                _SystemAction.SaveAction("DeleteModelRequirement", "ModelRequirementsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        //public IEnumerable<object> FillModelRequirementsSelect()
        //{
        //    return _ModelRequirementsRepository.GetAllModelRequirements().Select(s => new
        //    {
        //        Id = s.ModelReqId,
        //        Name = s.NameAr
        //    });
        //}
        public async Task< IEnumerable<ModelRequirementsVM>> GetAllModelRequirementsByModelId(int ModelId)
        {
            return await _ModelRequirementsRepository.GetAllModelRequirementsByModelId(ModelId);
        }
    }
}

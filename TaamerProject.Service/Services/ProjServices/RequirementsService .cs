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
using TaamerProject.Repository.Repositories;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class RequirementsService :   IRequirementsService
    {
        private readonly IRequirementsRepository _RequirementsRepository;
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public RequirementsService(IRequirementsRepository requirementsRepository,
            ISys_SystemActionsRepository sys_SystemActionsRepository,
             TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _RequirementsRepository = requirementsRepository;
            _Sys_SystemActionsRepository = sys_SystemActionsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task<IEnumerable<RequirementsVM>> GetAllRequirements(int BranchId)
        {
            var requirements = _RequirementsRepository.GetAllRequirements(BranchId);
            return requirements;
        }
        public Task<IEnumerable<RequirementsVM>> GetAllRequirementsByProjectId(int ProjectId, int BranchId)
        {
            var requirements = _RequirementsRepository.GetAllRequirementsByProjectId(ProjectId,BranchId);
            return requirements;
        }
        public GeneralMessage ConfirmRequirementStatus(int RequirementId,bool Status, int UserId, int BranchId)
        {
            try
            {
                Requirements? requirements = _TaamerProContext.Requirements.Where(s => s.RequirementId == RequirementId).FirstOrDefault();
                if (requirements != null)
                {
                    requirements.ConfirmStatus = Status;
                    if (Status == true)
                    {
                        requirements.ConfirmStatusDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        requirements.UpdateUser = UserId;
                    }
                    else
                    {
                        requirements.ConfirmStatusDate = null;
                        requirements.UpdateUser = null;
                    }
                    _TaamerProContext.SaveChanges();
                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage SaveRequirements(Requirements requirements, int UserId, int BranchId)
        {
            try
            {
                if (requirements.RequirementId == 0)
                {
                    requirements.AddUser = UserId;
                    requirements.BranchId = BranchId;
                    requirements.AddDate = DateTime.Now;
                    _TaamerProContext.Requirements.Add(requirements);

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة متطلب جديد";
                    _SystemAction.SaveAction("SaveRequirements", "RequirementsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                   // var RequirementsUpdated = _RequirementsRepository.GetById(requirements.RequirementId);
                    Requirements? RequirementsUpdated = _TaamerProContext.Requirements.Where(s => s.RequirementId == requirements.RequirementId).FirstOrDefault();

                    if (RequirementsUpdated != null)
                    {
                        RequirementsUpdated.NameAr = requirements.NameAr;
                        RequirementsUpdated.NameEn = requirements.NameEn;
                        RequirementsUpdated.UpdateUser = UserId;
                        RequirementsUpdated.UpdateDate = DateTime.Now;

                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل متطلب رقم " + requirements.RequirementId;
                    _SystemAction.SaveAction("SaveRequirements", "RequirementsService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception)
            { //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ المتطلب";
                _SystemAction.SaveAction("SaveRequirements", "RequirementsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage SaveRequirementsbyProjectId(int ProjectId, int UserId, int BranchId)
        {
            try
            {
                var requirements = new Requirements();
                var project = _TaamerProContext.Project.Where(s => s.ProjectId == ProjectId).FirstOrDefault();
                if(project != null)
                {
                    var Requ = _TaamerProContext.ProjectRequirements.Where(s => s.IsDeleted==false && s.ProjectTypeId == project.ProjectTypeId && s.ProjectSubTypeId == project.SubProjectTypeId).ToList();
                    foreach( var req in Requ )
                    {
                        requirements.RequirementId = 0;
                        requirements.NameAr = req.NameAr;
                        requirements.NameEn = req.NameEn;
                        requirements.AttachmentUrl = req.AttachmentUrl;
                        requirements.ProjectId = ProjectId;
                        requirements.ConfirmStatus = false;
                        requirements.ConfirmStatusDate = null;
                        requirements.Cost = req.Cost??0;
                        requirements.AddUser = UserId;
                        requirements.BranchId = BranchId;
                        requirements.AddDate = DateTime.Now;
                        _TaamerProContext.Requirements.Add(requirements);
                        _TaamerProContext.SaveChanges();
                    }
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة متطلب جديد";
                    _SystemAction.SaveAction("SaveRequirementsbyProjectId", "RequirementsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }
            }
            catch (Exception)
            { //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ المتطلب";
                _SystemAction.SaveAction("SaveRequirementsbyProjectId", "RequirementsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage DeleteRequirement(int RequirementId, int UserId, int BranchId)
        {
            try
            {
               // Requirements requirements = _RequirementsRepository.GetById(RequirementId);
                Requirements? requirements =   _TaamerProContext.Requirements.Where(s => s.RequirementId == RequirementId).FirstOrDefault();
                if (requirements != null)
                {
                    requirements.IsDeleted = true;
                    requirements.DeleteDate = DateTime.Now;
                    requirements.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف متطلب رقم " + RequirementId;
                    _SystemAction.SaveAction("SaveRequirements", "RequirementsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف متطلب رقم " + RequirementId; ;
                _SystemAction.SaveAction("SaveRequirements", "RequirementsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public Task<IEnumerable<RequirementsVM>> FillRequirementsSelect(int BranchId)
        {
            var taskType = _RequirementsRepository.GetAllRequirements(BranchId);
            return taskType;
            //return _RequirementsRepository.GetAllRequirements(BranchId).Select(s => new
            //{
            //    Id = s.RequirementId,
            //    Name = s.NameAr
            //});
        }

       

    }
}

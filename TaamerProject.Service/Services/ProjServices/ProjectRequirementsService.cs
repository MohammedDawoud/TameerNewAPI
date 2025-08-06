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
using TaamerProject.Service.Generic;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class ProjectRequirementsService :   IProjectRequirementsService
    {
        private readonly IProjectRequirementsRepository _ProjectRequirementsRepository;
         private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public ProjectRequirementsService(IProjectRequirementsRepository ProjectRequirementsRepository,TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _ProjectRequirementsRepository = ProjectRequirementsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }


        public Task<IEnumerable<ProjectRequirementsVM>> GetAllProjectRequirement( int BranchId)
        {
            var ProjectRequirements = _ProjectRequirementsRepository.GetAllProjectRequirement(BranchId);
            return ProjectRequirements;
        }

        public Task<IEnumerable<ProjectRequirementsVM>> GetAllProjectRequirementByTaskId(int BranchId, int PhasesTaskID)
        {
            var ProjectRequirements = _ProjectRequirementsRepository.GetAllProjectRequirementByTaskId(BranchId, PhasesTaskID);
            return ProjectRequirements;
        }
        public Task<IEnumerable<ProjectRequirementsVM>> GetAllProjectRequirementById(int BranchId, int RequirementId)
        {
            var ProjectRequirements = _ProjectRequirementsRepository.GetAllProjectRequirementById(BranchId, RequirementId);
            return ProjectRequirements;
        }

        public Task<IEnumerable<ProjectRequirementsVM>> GetAllProjectRequirementByOrder(int BranchId, int Orderid)
        {
            var ProjectRequirements = _ProjectRequirementsRepository.GetAllProjectRequirementOrderId(BranchId, Orderid);
            return ProjectRequirements;
        }
        public Task<IEnumerable<ProjectRequirementsVM>> GetProjectRequirementByProjectSubTypeId(int ProjectSubTypeId, string SearchText, int BranchId)
        {
            var ProjectRequirements = _ProjectRequirementsRepository.GetProjectRequirementByProjectSubTypeId(ProjectSubTypeId, SearchText, BranchId);
            return ProjectRequirements;
        }
        public Task<IEnumerable<ProjectRequirementsVM>> GetProjectRequirementByTaskId(int TaskId, int BranchId)
        {
            var ProjectRequirements = _ProjectRequirementsRepository.GetProjectRequirementByTaskId(TaskId, BranchId);
            return ProjectRequirements;
        }
        public Task<IEnumerable<ProjectRequirementsVM>> GetProjectRequirementOrderId(int Orderid, int BranchId)
        {
            var ProjectRequirements = _ProjectRequirementsRepository.GetProjectRequirementByOrderId(Orderid, BranchId);
            return ProjectRequirements;
        }

        public GeneralMessage SaveProjectRequirement2(List<ProjectRequirements> projectRequirements, int UserId, int BranchId)
        {
          var  typname = "";
            try
            {

                //if (projectRequirements.RequirementId == 0)
                //{
                foreach(var Item in projectRequirements) {
                    Item.BranchId = BranchId;
                    Item.AddUser = UserId;
                    Item.AddDate = DateTime.Now;
                    _TaamerProContext.ProjectRequirements.Add(Item);
                }
                _TaamerProContext.SaveChanges();
                try
                {
                     typname = _TaamerProContext.ProjectType.FirstOrDefault(x => x.TypeId == projectRequirements.FirstOrDefault().ProjectTypeId).NameAr;
                }
                catch (Exception ex)
                {

                }
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة متطلبات مشروع " + typname;
                    _SystemAction.SaveAction("SaveProjectRequirement", "ProjectRequirementsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                //}
                //else
                //{
                //    var ProjectRequirementsUpdated = _ProjectRequirementsRepository.GetById(projectRequirements.RequirementId);
                //    if (ProjectRequirementsUpdated != null)
                //    {
                //        ProjectRequirementsUpdated.ProjectTypeId = projectRequirements.ProjectTypeId;
                //        ProjectRequirementsUpdated.ProjectSubTypeId = projectRequirements.ProjectSubTypeId;
                //        ProjectRequirementsUpdated.NameAr = projectRequirements.NameAr;
                //        ProjectRequirementsUpdated.NameEn = projectRequirements.NameEn;
                //        ProjectRequirementsUpdated.Cost = projectRequirements.Cost;
                //        if (projectRequirements.AttachmentUrl != null)
                //        {
                //            ProjectRequirementsUpdated.AttachmentUrl = projectRequirements.AttachmentUrl;
                //        }
                //        ProjectRequirementsUpdated.UpdateUser = UserId;
                //        ProjectRequirementsUpdated.UpdatedDate = DateTime.Now;
                //    }
                //    _TaamerProContext.SaveChanges();
                //    //-----------------------------------------------------------------------------------------------------------------
                //    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //    string ActionNote2 = " تعديل متطلبات مشروع رقم " + projectRequirements;
                //    _SystemAction.SaveAction("SaveProjectRequirement", "ProjectRequirementsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                //    //-----------------------------------------------------------------------------------------------------------------
                //    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };

                //}
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                _SystemAction.SaveAction("SaveProjectRequirement", "ProjectRequirementsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }




        public GeneralMessage SaveProjectRequirement(ProjectRequirements projectRequirements, int UserId, int BranchId)
        {
            var typname = "";
            try
            {

                if (projectRequirements.RequirementId == 0)
                {
                    projectRequirements.BranchId = BranchId;
                    projectRequirements.AddUser = UserId;
                    projectRequirements.AddDate = DateTime.Now;
                    _TaamerProContext.ProjectRequirements.Add(projectRequirements);
                    _TaamerProContext.SaveChanges();
                    try
                    {
                        if(projectRequirements.ProjectTypeId!=null)
                        typname = _TaamerProContext.ProjectType.FirstOrDefault(x => x.TypeId == projectRequirements.ProjectTypeId)!.NameAr??"";
                    }
                    catch (Exception ex)
                    {

                    }
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة متطلبات مشروع "+ typname;
                    _SystemAction.SaveAction("SaveProjectRequirement", "ProjectRequirementsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                   // var ProjectRequirementsUpdated = _ProjectRequirementsRepository.GetById(projectRequirements.RequirementId);
                    ProjectRequirements? ProjectRequirementsUpdated = _TaamerProContext.ProjectRequirements.Where(s => s.RequirementId == projectRequirements.RequirementId).FirstOrDefault();

                    if (ProjectRequirementsUpdated != null)
                    {
                        ProjectRequirementsUpdated.ProjectTypeId = projectRequirements.ProjectTypeId;
                        ProjectRequirementsUpdated.ProjectSubTypeId = projectRequirements.ProjectSubTypeId;
                        ProjectRequirementsUpdated.NameAr = projectRequirements.NameAr;
                        ProjectRequirementsUpdated.NameEn = projectRequirements.NameEn;
                        ProjectRequirementsUpdated.Cost = projectRequirements.Cost;
                        if (projectRequirements.AttachmentUrl != null)
                        {
                            ProjectRequirementsUpdated.AttachmentUrl = projectRequirements.AttachmentUrl;
                        }
                        ProjectRequirementsUpdated.UpdateUser = UserId;
                        ProjectRequirementsUpdated.UpdateDate = DateTime.Now;
                   }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " تعديل متطلبات مشروع رقم " + projectRequirements;
                    _SystemAction.SaveAction("SaveProjectRequirement", "ProjectRequirementsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };

                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                _SystemAction.SaveAction("SaveProjectRequirement", "ProjectRequirementsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteProjectRequirements(int RequirementId, int UserId, int BranchId)
        {
            try
            {
               // ProjectRequirements Requir = _ProjectRequirementsRepository.GetById(RequirementId);
                ProjectRequirements? Requir = _TaamerProContext.ProjectRequirements.Where(s => s.RequirementId ==  RequirementId).FirstOrDefault();
                if (Requir != null)
                {
                    Requir.IsDeleted = true;
                    Requir.DeleteDate = DateTime.Now;
                    Requir.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف متطلبات مشروع رقم " + RequirementId;
                    _SystemAction.SaveAction("DeleteProjectRequirements", "ProjectRequirementsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }


                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف متطلبات مشروع رقم " + RequirementId; ;
                _SystemAction.SaveAction("DeleteProjectRequirements", "ProjectRequirementsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }

      

    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;
using Twilio.TwiML.Voice;

namespace TaamerProject.Service.Services
{
    public class DraftDetailsService :   IDraftDetailsService
    {
        private readonly IDraftRepository _draftRepository;
        private readonly IDraftDetailsRepository _DraftDetailsRepository;
        private readonly IProjectRepository _ProjectRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public DraftDetailsService(TaamerProjectContext dataContext
            , ISystemAction systemAction, IDraftRepository draftRepository, IDraftDetailsRepository DraftDetailsRepository,
            IProjectRepository ProjectRepository
            )
        {
            _draftRepository = draftRepository;
            _DraftDetailsRepository = DraftDetailsRepository;
            _ProjectRepository = ProjectRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;

        }
        public Task<IEnumerable<DraftDetailsVM>> GetAllDraftsDetailsbyProjectId(int? ProjectId)
        {
            var draftDetails = _DraftDetailsRepository.GetAllDraftsDetailsbyProjectId(ProjectId);
            return draftDetails;
        }

        public Task<IEnumerable<DraftDetailsVM>> GetAllDraftDetailsByDraftId(int? DraftId)
        {
            var draftDetails = _DraftDetailsRepository.GetAllDraftDetailsByDraftId(DraftId);
            return draftDetails;
        }

        public IEnumerable<DraftDetailsVM> GetAllDraftDetailsByDraftId_Union(int draftId)
        {
            //var draft = _draftRepository.GetById(draftId);
            Draft? draft = _TaamerProContext.Draft.Where(s => s.DraftId == draftId).FirstOrDefault();
            if (draft != null)
            {
                var draftDetails = _DraftDetailsRepository.GetAllDraftDetailsByDraftId(draft.DraftId).Result.ToList();
                var projIds = draftDetails.Select(x => x.ProjectId).ToList();

                var projects = _TaamerProContext.Project.Where(x => x.IsDeleted == false && x.Status == 0 &&
                x.ProjectTypeId == draft.ProjectTypeId && x.ContractId != null &&
                !projIds.Contains(x.ProjectId)).Select(x => new DraftDetailsVM
                {
                    ProjectNo = x.ProjectNo,
                    DraftId = draft.DraftId,
                    DraftName = draft.Name,
                    ProjectId = x.ProjectId,
                    DraftDetailId = 0
                }).ToList();
                var final = draftDetails.Union(projects);
                return final;
            }
            else
            {
                return new List<DraftDetailsVM>();
            }
        }
 
        public GeneralMessage SaveDraftDetails(DraftDetails draftDetails, int UserId, int BranchId)
        {
            try
            {
               // var project = _ProjectRepository.GetById(draftDetails.ProjectId);

                //Project? project = _TaamerProContext.Project.Where(s => s.ProjectId == draftDetails.ProjectId).FirstOrDefault();
                var proDraftDetails = _TaamerProContext.DraftDetails.Where(s => s.ProjectId == draftDetails.ProjectId).ToList();

                if (proDraftDetails.Count()>0)
                {
                    _TaamerProContext.DraftDetails.RemoveRange(proDraftDetails.ToList());
                    _TaamerProContext.DraftDetails.Add(draftDetails);
                    ////project.DraftDetails.Add(draftDetails);
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة رابط بمسودة جديد";
                     _SystemAction.SaveAction("SaveDraftDetails", "DraftDetailsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {

                    _TaamerProContext.DraftDetails.Add(draftDetails);
                    //project.DraftDetails.Add(draftDetails);
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة رابط بمسودة جديد";
                    _SystemAction.SaveAction("SaveDraftDetails", "DraftDetailsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }

                //if (draftDetails.DraftDetailId == 0)
                //{
                //    draftDetails.AddUser = UserId;
                //    draftDetails.AddDate = DateTime.Now;
                //    _DraftDetailsRepository.Add(draftDetails);
                //    _TaamerProContext.SaveChanges();
                //    //-----------------------------------------------------------------------------------------------------------------
                //    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //    string ActionNote = "اضافة رابط بمسودة جديد";
                //     _SystemAction.SaveAction("SaveDraftDetails", "DraftDetailsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //    //-----------------------------------------------------------------------------------------------------------------
                //    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                //}
                //else
                //{
                //    var DraftDetailUpdated = _DraftDetailsRepository.GetById(draftDetails.DraftDetailId);
                //    if (DraftDetailUpdated != null)
                //    {
                //        DraftDetailUpdated.ProjectId = draftDetails.ProjectId;
                //        DraftDetailUpdated.ProjectId= draftDetails.ProjectId;

                //        DraftDetailUpdated.UpdateUser = UserId;
                //        DraftDetailUpdated.UpdatedDate = DateTime.Now;

                //        DraftDetailUpdated.IsDeleted = draftDetails.IsDeleted;
                //        if(draftDetails.IsDeleted)
                //            DraftDetailUpdated.DeleteDate = DateTime.Now;
                //        else
                //            DraftDetailUpdated.DeleteDate = null;

                //    }
                //    _TaamerProContext.SaveChanges();

                //    //-----------------------------------------------------------------------------------------------------------------
                //    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //    string ActionNote = " تعديل  رابط بمسودة رقم " + draftDetails.DraftDetailId;
                //     _SystemAction.SaveAction("SaveDraftDetails", "DraftDetailsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //    //-----------------------------------------------------------------------------------------------------------------

                //    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_EditedSuccessfully };
                //}
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ رابط للمسودة";
                 _SystemAction.SaveAction("SaveDraftDetails", "DraftDetailsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteDraftDetails(int DraftDetailsId, int UserId, int BranchId)
        {
            try
            {
                //DraftDetails draftDetails = _DraftDetailsRepository.GetById(DraftDetailsId);
                DraftDetails? draftDetails = _TaamerProContext.DraftDetails.Where(s => s.DraftDetailId == DraftDetailsId).FirstOrDefault();
                if (draftDetails != null)
                {
                    draftDetails.IsDeleted = true;
                    draftDetails.DeleteDate = DateTime.Now;
                    draftDetails.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف رابط مسودة رقم " + DraftDetailsId;
                    _SystemAction.SaveAction("DeleteDraftDetails", "DraftDetailsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
               
                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف رابط مسودة رقم " + DraftDetailsId; ;
                 _SystemAction.SaveAction("DeleteDraftDetails", "DraftDetailsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }

    
    }
}

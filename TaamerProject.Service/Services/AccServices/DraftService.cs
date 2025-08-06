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

namespace TaamerProject.Service.Services
{
    public class DraftService :   IDraftService
    {
        private readonly IDraftRepository _DraftRepository;
        private readonly IDraftDetailsRepository _DraftDetailsRepository;
        private readonly IProjectTypeRepository _projectTypeRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public DraftService(TaamerProjectContext dataContext
            , ISystemAction systemAction, IDraftRepository DraftRepository, IDraftDetailsRepository DraftDetailsRepository,
            IProjectTypeRepository projectTypeRepository
            )
        {
            _DraftRepository = DraftRepository;
            _DraftDetailsRepository = DraftDetailsRepository;
            _projectTypeRepository = projectTypeRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;

        }
        public Task<IEnumerable<DraftVM>> GetAllDrafts()
        {
            var drafts = _DraftRepository.GetAllDrafts();
            return drafts;
        }

        public Task<DraftVM> GetDraftById(int DraftId)
        {
            var drafts = _DraftRepository.GetDraftById(DraftId);
            return drafts;
        }
         
        public Task<IEnumerable<DraftVM>> GetAllDraftsbyProjectsType(int? projectTypeId)
        {
            var drafts = _DraftRepository.GetAllDraftsbyProjectsType(projectTypeId);
            return drafts;
        }
        public Task<IEnumerable<DraftVM>> GetAllDraftsbyProjectsType_2(int? projectTypeId)
        {
            var drafts = _DraftRepository.GetAllDraftsbyProjectsType_2(projectTypeId);
            return drafts;
        }
        public GeneralMessage SaveDraft(Draft draft, int UserId, int BranchId)
        {
            try
            {
                //var IsExist = _DraftRepository.GetMatching(x => x.Name == draft.Name && x.ProjectTypeId == draft.ProjectTypeId && x.IsDeleted == false).FirstOrDefault();
                var IsExist = _TaamerProContext.Draft.Where(x => x.Name == draft.Name && x.ProjectTypeId == draft.ProjectTypeId && x.IsDeleted == false).FirstOrDefault();

                if (draft.DraftId == 0 && IsExist == null)
                {
                    draft.AddUser = UserId;
                    draft.AddDate = DateTime.Now;
                    _TaamerProContext.Draft.Add(draft);

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة مسودة جديد";
                     _SystemAction.SaveAction("SaveDraft", "SaveDraft", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    //var DraftUpdated = _DraftRepository.GetById(draft.DraftId);
                    Draft? DraftUpdated = _TaamerProContext.Draft.Where(s => s.DraftId == draft.DraftId).FirstOrDefault();
                    
                    if (DraftUpdated != null)
                    {
                        DraftUpdated.Name = draft.Name;
                        DraftUpdated.DraftUrl = draft.DraftUrl;
                        DraftUpdated.ProjectTypeId = draft.ProjectTypeId;

                        if (draft.IsDeleted)
                        {
                            DraftUpdated.IsDeleted = draft.IsDeleted;
                            DraftUpdated.DeleteUser = UserId;
                            DraftUpdated.DeleteDate = DateTime.Now;
                        }
                        else
                        {
                            DraftUpdated.UpdateUser = UserId;
                            DraftUpdated.UpdateDate = DateTime.Now;
                        }
                        //if (draft.DraftDetails != null || draft.DraftDetails.Count() > 0)
                        //{
                        //    if (DraftUpdated.DraftDetails != null || DraftUpdated.DraftDetails.Count() > 0)
                        //        _DraftDetailsRepository.RemoveRange(DraftUpdated.DraftDetails.ToList());

                        //    foreach (var item in draft.DraftDetails)
                        //    {
                        //        _DraftDetailsRepository.Add(item);
                        //    }
                        //}
                    }

                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل مسودة رقم " + draft.DraftId;
                     _SystemAction.SaveAction("SaveDraft", "SaveDraft", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_EditedSuccessfully };
                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ المسودة";
                 _SystemAction.SaveAction("SaveDraft", "SaveDraft", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public GeneralMessage DeleteDraft(int DraftId, int UserId,int BranchId)
        {
            try
            {
                //Draft draft = _DraftRepository.GetById(DraftId);
                Draft? draft = _TaamerProContext.Draft.Where(s => s.DraftId == DraftId).FirstOrDefault();
                if (draft != null)
                {
                    draft.IsDeleted = true;
                    draft.DeleteDate = DateTime.Now;
                    draft.DeleteUser = UserId;
                    var draftdet = _TaamerProContext.DraftDetails.Where(s => s.DraftId == DraftId).ToList();
                    if(draftdet.Count()>0)
                    {
                        _TaamerProContext.DraftDetails.RemoveRange(draftdet);
                    }
                    

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف مسودة رقم " + DraftId;
                    _SystemAction.SaveAction("DeleteDraft", "DraftService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }


                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف مسودة رقم " + DraftId; ;
                 _SystemAction.SaveAction("DeleteDraft", "DraftService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
         
        public  IEnumerable<DraftVM> GetAllDraftsByDraftName_Union(string DraftName)
        {
            var drafts = GetAllDrafts().Result.Where(x => x.DraftName == DraftName).ToList();
            var draftProTyp = drafts.Select(x => x.ProjectTypeId);

            //Draft? draft = _TaamerProContext.Draft.Where(s => s.DraftId == draftId).FirstOrDefault();
            //if (draft != null)
            //{
            //    var draftDetails = _DraftDetailsRepository.GetAllDraftDetailsByDraftId(draft.DraftId).Result.ToList();
            //    var projIds = draftDetails.Select(x => x.ProjectId).ToList();

            //    var projects = _TaamerProContext.Project.Where(x => x.IsDeleted == false && x.Status == 0 &&
            //    x.ProjectTypeId == draft.ProjectTypeId && x.ContractId != null &&
            //    !projIds.Contains(x.ProjectId)).Select(x => new DraftDetailsVM
            //    {
            //        ProjectNo = x.ProjectNo,
            //        DraftId = draft.DraftId,
            //        DraftName = draft.Name,
            //        ProjectId = x.ProjectId,
            //        DraftDetailId = 0
            //    }).ToList();
            //    var final = draftDetails.Union(projects);
            //    return final;
            //}


            var projTypes = _TaamerProContext.ProjectType.Where(x => !x.IsDeleted && !draftProTyp.Contains(x.TypeId))
                .Select(s =>
               new DraftVM
               {
                   ProjectTypeId = s.TypeId,
                   ProjectTypeName = s.NameAr,
                   DraftName = drafts[0].DraftName,
                   DraftUrl = drafts[0].DraftUrl,
                   DraftId = 0
               }).ToList();

            var final = drafts.Union(projTypes);
            return final.ToList();
        }
        //public IEnumerable<DepartmentVM> GetAllDeptsByBranchId(int branchId)
        //{
        //    var departments = _DepartmentRepository.GetAllDeptsByBranchId(branchId).ToList();
        //    return departments;
        //}
      

    }
}

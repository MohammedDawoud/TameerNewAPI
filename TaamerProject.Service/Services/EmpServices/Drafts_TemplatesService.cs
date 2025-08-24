using System.Globalization;
using System.Net;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class Drafts_TemplatesService :  IDrafts_TemplatesService
    {
        private readonly IDrafts_TemplatesRepository _drafts_TemplatesRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        
        public Drafts_TemplatesService(TaamerProjectContext dataContext
            , ISystemAction systemAction, IDrafts_TemplatesRepository drafts_TemplatesRepository)
        {
            _drafts_TemplatesRepository = drafts_TemplatesRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }


        public Task<IEnumerable<Drafts_TemplatesVM>> GetAllDrafts_templates()
        {
            var drafts = _drafts_TemplatesRepository.GetAllDrafts_templates();
            return drafts;
        }


        public Task<Drafts_TemplatesVM> GetDraft_templateByProjectId(int projecttypeid)
        {
            var drafts = _drafts_TemplatesRepository.GetDraft_templateByProjectId(projecttypeid);
            return drafts;
        }
        public GeneralMessage SaveDraft_Templates(Drafts_Templates drafts, int UserId, int BranchId)
        {
            try
            {
                //var IsExist = _drafts_TemplatesRepository.GetMatching(x => x.Name == drafts.Name && x.ProjectTypeId == drafts.ProjectTypeId && x.IsDeleted == false).FirstOrDefault();
                var IsExist = _TaamerProContext.Drafts_Templates.Where(x => x.Name == drafts.Name && x.ProjectTypeId == drafts.ProjectTypeId && x.IsDeleted == false).FirstOrDefault();

                if (drafts.DraftTempleteId == 0 && IsExist == null)
                {
                    drafts.AddUser = UserId;
                    drafts.AddDate = DateTime.Now;
                    _TaamerProContext.Drafts_Templates.Add(drafts);

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
                    // var DraftUpdated = _drafts_TemplatesRepository.GetById(drafts.DraftTempleteId);
                    Drafts_Templates? DraftUpdated = _TaamerProContext.Drafts_Templates.Where(s => s.DraftTempleteId == drafts.DraftTempleteId).FirstOrDefault();

                    if (DraftUpdated != null)
                    {
                        DraftUpdated.Name = drafts.Name;
                        DraftUpdated.DraftUrl = drafts.DraftUrl;
                        DraftUpdated.ProjectTypeId = drafts.ProjectTypeId;
                       
                        if (drafts.IsDeleted)
                        {
                            DraftUpdated.IsDeleted = drafts.IsDeleted;
                            DraftUpdated.DeleteUser = UserId;
                            DraftUpdated.DeleteDate = DateTime.Now;
                        }
                        else
                        {
                            DraftUpdated.UpdateUser = UserId;
                            DraftUpdated.UpdateDate = DateTime.Now;
                        }
                    }

                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل مسودة رقم " + drafts.DraftTempleteId;
                     _SystemAction.SaveAction("SaveDraft", "SaveDraft", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_EditedSuccessfully };
                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ المسودة";
                 _SystemAction.SaveAction("SaveDraft", "SaveDraft", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }





        public GeneralMessage ConnectDraft_Templates_WithProject(int DraftId,int ProjectTypeId, int UserId, int BranchId)
        {
            try
            {
                //var IsExist = _drafts_TemplatesRepository.GetMatching( x=> x.IsDeleted == false && x.ProjectTypeId == ProjectTypeId ).FirstOrDefault();
                var IsExist = _TaamerProContext.Drafts_Templates.Where(x => x.IsDeleted == false && x.ProjectTypeId == ProjectTypeId).FirstOrDefault();

                if ( IsExist == null)
                {
                   // var olddraft = _drafts_TemplatesRepository.GetMatching(x => x.IsDeleted == false && x.DraftTempleteId == DraftId).FirstOrDefault();
                    var olddraft = _TaamerProContext.Drafts_Templates.Where(x => x.IsDeleted == false && x.DraftTempleteId == DraftId).FirstOrDefault();


                    if (olddraft != null)
                    {
                        Drafts_Templates newdraft = new Drafts_Templates();

                        newdraft.ProjectTypeId = ProjectTypeId;
                        newdraft.Name = olddraft.Name;
                        newdraft.DraftUrl = olddraft.DraftUrl;
                        newdraft.AddUser = UserId;
                        newdraft.AddDate = DateTime.Now;
                        _TaamerProContext.Drafts_Templates.Add(newdraft);
                    }
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
                    _TaamerProContext.Drafts_Templates.Remove(IsExist);
                   // var olddraft = _drafts_TemplatesRepository.GetMatching(x => x.IsDeleted == false && x.DraftTempleteId == DraftId).FirstOrDefault();
                    var olddraft = _TaamerProContext.Drafts_Templates.Where(x => x.IsDeleted == false && x.DraftTempleteId == DraftId).FirstOrDefault();


                    if (olddraft != null) {
                    Drafts_Templates newdraft = new Drafts_Templates();

                    newdraft.ProjectTypeId = ProjectTypeId;
                    newdraft.Name = olddraft.Name;
                    newdraft.DraftUrl = olddraft.DraftUrl;
                    newdraft.AddUser = UserId;
                    newdraft.AddDate = DateTime.Now;
                    _TaamerProContext.Drafts_Templates.Add(newdraft);
                    }
                    _TaamerProContext.SaveChanges();
                 
                

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل مسودة رقم " ;
                     _SystemAction.SaveAction("SaveDraft", "SaveDraft", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_EditedSuccessfully };
                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ المسودة";
                 _SystemAction.SaveAction("SaveDraft", "SaveDraft", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
       

    }
}

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
    public class ProjectExtractsService :   IProjectExtractsService
    {
        private readonly IProjectExtractsRepository _ProjectExtractsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public ProjectExtractsService(IProjectExtractsRepository ProjectExtractsRepository,TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _ProjectExtractsRepository = ProjectExtractsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task<IEnumerable<ProjectExtractsVM>> GetAllProjectExtracts(int? ProjectId)
        {
            var ProjectExtracts = _ProjectExtractsRepository.GetAllProjectExtracts(ProjectId);
            return ProjectExtracts;
        }
        public GeneralMessage SaveProjectExtracts(ProjectExtracts projectExtracts,int UserId, int BranchId)
        {
            try
            {
                if (projectExtracts.ExtractId == 0)
                {
                    projectExtracts.AddUser = UserId;
                    projectExtracts.AddDate = DateTime.Now;
                    _TaamerProContext.ProjectExtracts.Add(projectExtracts);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة التعليق على المشروع ";
                    _SystemAction.SaveAction("SaveProjectExtracts", "ProjectExtractsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_SavedSuccessfully };
                }
                else
                {
                    //var ProjectExtractsUpdated = _ProjectExtractsRepository.GetById(projectExtracts.ExtractId);
                    ProjectExtracts? ProjectExtractsUpdated = _TaamerProContext.ProjectExtracts.Where(s => s.ExtractId == projectExtracts.ExtractId).FirstOrDefault();

                    if (ProjectExtractsUpdated != null && projectExtracts.IsDoneBefore == false)
                    {
                        ProjectExtractsUpdated.ExtractNo = projectExtracts.ExtractNo;
                        ProjectExtractsUpdated.Type = projectExtracts.Type;
                        ProjectExtractsUpdated.Value = projectExtracts.Value;
                        ProjectExtractsUpdated.DateFrom = projectExtracts.DateFrom;
                        ProjectExtractsUpdated.DateTo = projectExtracts.DateTo;
                        ProjectExtractsUpdated.Status = projectExtracts.Status;
                        ProjectExtractsUpdated.ValueText = projectExtracts.ValueText;
                        ProjectExtractsUpdated.UpdateUser = UserId;
                        ProjectExtractsUpdated.UpdateDate = DateTime.Now;
                    }                      
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " تعديل التعليق رقم " + projectExtracts.ExtractId;
                    _SystemAction.SaveAction("SaveProjectExtracts", "ProjectExtractsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  "تم التعديل" };

                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                _SystemAction.SaveAction("SaveProjectExtracts", "ProjectExtractsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteProjectExtracts(int ExtractId,int UserId, int BranchId)
        {
            try
            {
                //ProjectExtracts projectExtracts = _ProjectExtractsRepository.GetById(ExtractId);
                ProjectExtracts? projectExtracts = _TaamerProContext.ProjectExtracts.Where(s => s.ExtractId ==  ExtractId).FirstOrDefault();

                if (projectExtracts != null)
                {
                    projectExtracts.IsDeleted = true;
                    projectExtracts.DeleteDate = DateTime.Now;
                    projectExtracts.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف التعليق رقم " + ExtractId;
                    _SystemAction.SaveAction("DeleteProjectExtracts", "ProjectExtractsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف التعليق رقم " + ExtractId; ;
                _SystemAction.SaveAction("DeleteProjectExtracts", "ProjectExtractsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  Resources.General_DeletedFailed };
            }
        }

        public GeneralMessage UpdateExtractAttachment(ProjectExtracts projectExtracts,int UserId, int BranchId)
        {
            try
            {
                   // var ProjectExtractsUpdated = _ProjectExtractsRepository.GetById(projectExtracts.ExtractId);
                ProjectExtracts? ProjectExtractsUpdated = _TaamerProContext.ProjectExtracts.Where(s => s.ExtractId == projectExtracts.ExtractId).FirstOrDefault();

                if (ProjectExtractsUpdated != null)
                    {
                        ProjectExtractsUpdated.IsDoneBefore = true;
                        ProjectExtractsUpdated.AttachmentUrl = projectExtracts.AttachmentUrl;
                        ProjectExtractsUpdated.UpdateUser = UserId;
                        ProjectExtractsUpdated.UpdateDate = DateTime.Now;
                    }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = " تعديل مرفقات المشروع رقم " + projectExtracts.ExtractId;
                _SystemAction.SaveAction("UpdateExtractAttachment", "ProjectExtractsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ المرفقات";
                _SystemAction.SaveAction("UpdateExtractAttachment", "ProjectExtractsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  Resources.General_SavedFailed };
            }
        }
        public GeneralMessage UpdateExtractSignature(ProjectExtracts projectExtracts,int UserId, int BranchId)
        {
            try
            {
                //var ProjectExtractsUpdated = _ProjectExtractsRepository.GetById(projectExtracts.ExtractId);
                ProjectExtracts? ProjectExtractsUpdated = _TaamerProContext.ProjectExtracts.Where(s => s.ExtractId == projectExtracts.ExtractId).FirstOrDefault();
                if (ProjectExtractsUpdated != null)
                {
                    ProjectExtractsUpdated.IsDoneAfter = true;
                    ProjectExtractsUpdated.SignatureUrl = projectExtracts.SignatureUrl;
                    ProjectExtractsUpdated.UpdateUser = UserId;
                    ProjectExtractsUpdated.UpdateDate = DateTime.Now;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = " تعديل توقيع المشروع رقم " + projectExtracts.ExtractId;
                _SystemAction.SaveAction("UpdateExtractSignature", "ProjectExtractsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  "تم التعديل" };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في الحفظ ";
                _SystemAction.SaveAction("UpdateExtractAttachment", "ProjectExtractsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  Resources.General_SavedFailed };
            }
        }
        

    }
}

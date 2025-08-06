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
using System.Xml.Linq;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class ProjectCommentsService :   IProjectCommentsService
    {
        private readonly IProjectCommentsRepository _commentsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public ProjectCommentsService(IProjectCommentsRepository commentsRepository,TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _commentsRepository = commentsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task<IEnumerable<ProjectCommentsVM>> GetAllProjectComments(int ProjectId)
        {
            var comments = _commentsRepository.GetAllProjectComments(ProjectId);
            return comments;
        }
        public GeneralMessage SaveComment(ProjectComments comments,int UserId, int BranchId)
        {
            try
            {
                if (comments.CommentId == 0)
                {
                    comments.UserId = UserId;
                    comments.Date = DateTime.Now;
                    comments.AddUser = UserId;
                    comments.AddDate = DateTime.Now;
                    _TaamerProContext.ProjectComments.Add(comments);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة تعليق ";
                    _SystemAction.SaveAction("SaveComment", "ProjectCommentsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_SavedSuccessfully };
                }
                else
                {
                    //var ProjectCommentsUpdated = _commentsRepository.GetById(comments.CommentId);
                    ProjectComments? ProjectCommentsUpdated = _TaamerProContext.ProjectComments.Where(s => s.CommentId == comments.CommentId).FirstOrDefault();

                    if (ProjectCommentsUpdated != null)
                    {
                        ProjectCommentsUpdated.Comment = comments.Comment;
                        ProjectCommentsUpdated.UpdateUser = UserId;
                        ProjectCommentsUpdated.UpdateDate = DateTime.Now;

                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " تعديل التعليق رقم " + comments.CommentId;
                    _SystemAction.SaveAction("SaveComment", "ProjectCommentsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_EditedSuccessfully };

                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ التعليق";
                _SystemAction.SaveAction("SaveComment", "ProjectCommentsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteComment(int CommentId,int  UserId, int BranchId)
        {
            try
            {
                // ProjectComments comment = _commentsRepository.GetById(CommentId);
                ProjectComments? comment = _TaamerProContext.ProjectComments.Where(s => s.CommentId == CommentId).FirstOrDefault();
                if (comment != null)
                {
                    comment.IsDeleted = true;
                    comment.DeleteDate = DateTime.Now;
                    comment.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف التعليق رقم " + CommentId;
                    _SystemAction.SaveAction("DeleteComment", "ProjectCommentsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }


                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف التعليق رقم " + CommentId; ;
                _SystemAction.SaveAction("DeleteComment", "ProjectCommentsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  Resources.General_DeletedFailed };
            }
        }
       

    }
}

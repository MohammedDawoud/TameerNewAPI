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
    public class ProjectPiecesService :   IProjectPiecesService
    {
        private readonly IProjectPiecesRepository _ProjectPiecesRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public ProjectPiecesService(IProjectPiecesRepository ProjectPiecesRepository, TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _ProjectPiecesRepository = ProjectPiecesRepository;
             _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task<IEnumerable<ProjectPiecesVM>> GetAllProjectPieces(int ProjectId,string SearchText)
        {
            var projectPieces = _ProjectPiecesRepository.GetAllProjectPieces(ProjectId, SearchText);
            return projectPieces;
        }
        public GeneralMessage SaveProjectPieces(ProjectPieces projectPieces, int UserId, int BranchId)
        {
            try
            {
                if (projectPieces.PieceId == 0)
                {
                    projectPieces.AddUser = UserId;
                    projectPieces.AddDate = DateTime.Now;
                    _TaamerProContext.ProjectPieces.Add(projectPieces);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة متطلبات المشروع ";
                    _SystemAction.SaveAction("SaveProjectPieces", "ProjectPiecesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    //  var ProjectPiecesUpdated = _ProjectPiecesRepository.GetById(projectPieces.PieceId);
                    ProjectPieces? ProjectPiecesUpdated = _TaamerProContext.ProjectPieces.Where(s => s.PieceId == projectPieces.PieceId).FirstOrDefault();

                    if (ProjectPiecesUpdated != null)
                    {
                        ProjectPiecesUpdated.PieceNo = projectPieces.PieceNo;
                        ProjectPiecesUpdated.Notes = projectPieces.Notes;
                        ProjectPiecesUpdated.UpdateUser = UserId;
                        ProjectPiecesUpdated.UpdateDate = DateTime.Now;

                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل متطلبات المشروع رقم " + projectPieces.PieceId;
                    _SystemAction.SaveAction("SaveProjectPieces", "ProjectPiecesService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };


                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                _SystemAction.SaveAction("SaveProjectPieces", "ProjectPiecesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
           
        }
        public GeneralMessage DeleteProjectPieces(int PieceId, int UserId, int BranchId)
        {
            try
            {
                //ProjectPieces projectPieces = _ProjectPiecesRepository.GetById(PieceId);
                ProjectPieces? projectPieces = _TaamerProContext.ProjectPieces.Where(s => s.PieceId ==  PieceId).FirstOrDefault();
                if (projectPieces != null)
                {
                    projectPieces.IsDeleted = true;
                    projectPieces.DeleteDate = DateTime.Now;
                    projectPieces.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف متطلبات المشروع رقم " + PieceId;
                    _SystemAction.SaveAction("DeleteProjectPieces", "ProjectPiecesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف التعليق رقم " + PieceId; ;
                _SystemAction.SaveAction("DeleteProjectPieces", "ProjectCommentsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
       

    }
}

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
    public class ProjectWorkersService :   IProjectWorkersService
    {
        private readonly IProjectWorkersRepository _ProjectWorkersRepository;
        private readonly IUserPrivilegesRepository _UserPrivilegesRepository;
 
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public ProjectWorkersService(IProjectWorkersRepository ProjectWorkersRepository, IUserPrivilegesRepository UserPrivilegesRepository,
            TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _ProjectWorkersRepository = ProjectWorkersRepository;
            _UserPrivilegesRepository = UserPrivilegesRepository;
 
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task<IEnumerable<ProjectWorkersVM>> GetAllProjectWorkers(int? ProjectId, string SearchText)
        {
            var ProjectWorkers = _ProjectWorkersRepository.GetAllProjectWorkers(ProjectId, SearchText);
            return ProjectWorkers;
        }
        public Task<IEnumerable<ProjectWorkersVM>> GetUserProjectRpt(int? UserId)
        {
            var ProjectWorkers = _ProjectWorkersRepository.GetUserProjectRpt( UserId);
            return ProjectWorkers;
        }
        public GeneralMessage SaveProjectWorker(ProjectWorkers projectWorkers, int UserId, int BranchId)
        {
            try
            {
               // var exitsworker = _ProjectWorkersRepository.GetMatching(s => s.IsDeleted == false &&  s.ProjectId == projectWorkers.ProjectId && s.WorkerId != projectWorkers.WorkerId && s.UserId == projectWorkers.UserId).FirstOrDefault();
                var exitsworker = _TaamerProContext.ProjectWorkers.Where(s => s.IsDeleted == false && s.ProjectId == projectWorkers.ProjectId && s.WorkerId != projectWorkers.WorkerId && s.UserId == projectWorkers.UserId).FirstOrDefault();

                if (exitsworker != null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = Resources.General_SavedFailed;
                    _SystemAction.SaveAction("SaveProjectWorker", "ProjectCommentsService", 1, Resources.user_already_exist, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.user_already_exist };
                }
                if (projectWorkers.WorkerId == 0)
                {
                    projectWorkers.AddUser = UserId;
                    projectWorkers.BranchId = BranchId;
                    projectWorkers.AddDate = DateTime.Now;
                    _TaamerProContext.ProjectWorkers.Add(projectWorkers);

                    var userpriv = new UserPrivileges();
                    userpriv.UserId = projectWorkers.UserId;
                    userpriv.PrivilegeId = 111026;
                    userpriv.IsDeleted = false;
                    userpriv.AddDate = DateTime.Now;
                    _TaamerProContext.UserPrivileges.Add(userpriv);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة مستخدمين للمشروع ";
                    _SystemAction.SaveAction("SaveProjectWorker", "ProjectCommentsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                   // var projectWorkersUpdated = _ProjectWorkersRepository.GetById(projectWorkers.WorkerId);
                    ProjectWorkers? projectWorkersUpdated = _TaamerProContext.ProjectWorkers.Where(s => s.WorkerId == projectWorkers.WorkerId).FirstOrDefault();

                    if (projectWorkersUpdated != null)
                    {
                        projectWorkersUpdated.UserId = projectWorkers.UserId;
                        projectWorkersUpdated.WorkerType = projectWorkers.WorkerType;
                        projectWorkersUpdated.UpdateUser = UserId;
                        projectWorkersUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " تعديل مستخدمين للمشروع رقم " + projectWorkers;
                    _SystemAction.SaveAction("SaveProjectWorker", "ProjectCommentsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                _SystemAction.SaveAction("SaveProjectWorker", "ProjectCommentsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteProjectWorker(int WorkerId, int UserId, int BranchId)
        {
            try
            {
                //  ProjectWorkers projectWorkers = _ProjectWorkersRepository.GetById(WorkerId);
                ProjectWorkers? projectWorkers = _TaamerProContext.ProjectWorkers.Where(s => s.WorkerId == WorkerId).FirstOrDefault();
                if (projectWorkers != null)
                {
                    projectWorkers.IsDeleted = true;
                    projectWorkers.DeleteDate = DateTime.Now;
                    projectWorkers.DeleteUser = UserId;

                    //var userpriv = _UserPrivilegesRepository.GetMatching(s => s.IsDeleted == false && s.UserId == projectWorkers.UserId && s.PrivilegeId == 111026).FirstOrDefault();
                    var userpriv = _TaamerProContext.UserPrivileges.Where(s => s.IsDeleted == false && s.UserId == projectWorkers.UserId && s.PrivilegeId == 111026).FirstOrDefault();


                    if (userpriv != null)
                    {
                        _TaamerProContext.UserPrivileges.Remove(userpriv);
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف مستخدم من مشروع رقم " + WorkerId;
                    _SystemAction.SaveAction("DeleteProjectWorker", "ProjectCommentsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف مستخدم من مشروع رقم " + WorkerId; ;
                _SystemAction.SaveAction("DeleteProjectWorker", "ProjectCommentsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public async Task<int> GetUserProjectWorkerCount(int? UserId,int BranchId)
        {
            var UserTaskCount =await _ProjectWorkersRepository.GetUserProjectWorkerCount(UserId, BranchId);
            return UserTaskCount;
        }
       

    }
}

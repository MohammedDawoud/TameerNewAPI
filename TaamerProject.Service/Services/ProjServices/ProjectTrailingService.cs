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
    public class ProjectTrailingService :   IProjectTrailingService
    {
        private readonly IProjectTrailingRepository _ProjectTrailingRepository;
        private readonly ITrailingFilesRepository _TrailingFilesRepository;
        private readonly INotificationRepository _NotificationRepository;
        private readonly IProjectRepository _ProjectRepository;
        private readonly IUsersRepository _UsersRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public ProjectTrailingService(IProjectTrailingRepository ProjectTrailingRepository, ITrailingFilesRepository TrailingFilesRepository,
             INotificationRepository NotificationRepository, IProjectRepository ProjectRepository, IUsersRepository UsersRepository,
            TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _ProjectTrailingRepository = ProjectTrailingRepository;
            _TrailingFilesRepository = TrailingFilesRepository;
            _NotificationRepository = NotificationRepository;
            _ProjectRepository = ProjectRepository;
            _UsersRepository = UsersRepository;
             _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task<IEnumerable<ProjectTrailingVM>> GetProjectTrailingInOfficeArea(int BranchId)
        {
            var ProjectTrailing = _ProjectTrailingRepository.GetProjectTrailingInOfficeArea(BranchId);
            return ProjectTrailing;
        }
        public Task<IEnumerable<ProjectTrailingVM>> GetProjectTrailingInExternalSide(int BranchId)
        {
            var ProjectTrailing = _ProjectTrailingRepository.GetProjectTrailingInExternalSide(BranchId);
            return ProjectTrailing;
        }
        public GeneralMessage SaveProjectTrailing(ProjectTrailing ProjectTrailing, int UserId, int BranchId, string Lang)
        {
            try
            {
               // var tarilingpro = _ProjectTrailingRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == ProjectTrailing.ProjectId && s.DepartmentId == ProjectTrailing.DepartmentId).FirstOrDefault();
                var tarilingpro = _TaamerProContext.ProjectTrailing.Where(s => s.IsDeleted == false && s.ProjectId == ProjectTrailing.ProjectId && s.DepartmentId == ProjectTrailing.DepartmentId).FirstOrDefault();


                if (tarilingpro != null)
                {
                    var massagebe = "";
                    if (Lang == "rtl")
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = Resources.General_SavedFailed;
                        _SystemAction.SaveAction("SaveProjectTrailing", "ProjectTrailingService", 1, "المشروع موجود بالفعل في المتابعة لنفس هذه الجهة", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        massagebe = "المشروع موجود بالفعل في المتابعة لنفس هذه الجهة منذ تاريخ " + tarilingpro.Date + "";
                    }
                    else
                    {
                        massagebe = "The project has already been in follow-up to the same authority since the date of " + tarilingpro.Date + "";
                    }
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = massagebe };
                }
                //var project = _ProjectRepository.GetById(ProjectTrailing.ProjectId);
                Project? project = _TaamerProContext.Project.Where(s => s.ProjectId == ProjectTrailing.ProjectId).FirstOrDefault();
                if (project != null)
                {
                    ProjectTrailing.TypeId = 0;
                    ProjectTrailing.Status = 1;
                    ProjectTrailing.Active = true;
                    ProjectTrailing.UserId = UserId;
                    ProjectTrailing.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                    ProjectTrailing.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar"));
                    ProjectTrailing.BranchId = BranchId;
                    ProjectTrailing.AddUser = UserId;
                    ProjectTrailing.AddDate = DateTime.Now;
                    _TaamerProContext.ProjectTrailing.Add(ProjectTrailing);
                    if (ProjectTrailing.TrailingFiles != null)
                    {
                        foreach (var item in (ProjectTrailing.TrailingFiles.ToList()))
                        {
                            var TrailingFiles = new TrailingFiles();
                            TrailingFiles.ProjectId = item.ProjectId;
                            TrailingFiles.TrailingId = _ProjectTrailingRepository.GetMaxId().Result + 1;
                            TrailingFiles.FileName = item.FileName;
                            TrailingFiles.FileUrl = item.FileUrl;
                            TrailingFiles.TypeId = item.TypeId;
                            TrailingFiles.BranchId = BranchId;
                            TrailingFiles.AddUser = UserId;
                            TrailingFiles.AddDate = DateTime.Now;
                            _TaamerProContext.TrailingFiles.Add(TrailingFiles);
                        }
                    }
                    try
                    {
                        //var user = _UsersRepository.GetById(UserId);
                        Users? user = _TaamerProContext.Users.Where(s => s.UserId == UserId).FirstOrDefault();
                       
                        var ProNotification = new Notification();
                        ProNotification.Name = "اشعار علي مشروع رقم :" + project.ProjectNo;
                        ProNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                        ProNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                        ProNotification.SendUserId = UserId;
                        ProNotification.Type = 1; // notification
                        ProNotification.Description = "تم ارسال المشروع رقم " + project.ProjectNo + "للمتابعة بواسطة" + user.FullName + "";
                        ProNotification.AllUsers = true;
                        ProNotification.SendDate = DateTime.Now;
                        ProNotification.ProjectId = ProjectTrailing.ProjectId;
                        ProNotification.AddUser = UserId;
                        ProNotification.AddDate = DateTime.Now;
                        _TaamerProContext.Notification.Add(ProNotification);
                        _TaamerProContext.SaveChanges();
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "اضافة اشعار علي مشروع ";
                        _SystemAction.SaveAction("SaveProjectTrailing", "ProjectTrailingService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                    }
                    catch (Exception ex)
                    {
                    }
                    var massage = "";
                    if (Lang == "rtl")
                    {
                        massage = Resources.sent_succesfully;
                    }
                    else
                    {
                        massage = "Send Successfully";
                    }
                }
              
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = " massage", ReturnedStr = project.ProjectNo };
             }
            catch (Exception)
            {
                var massage = "";
                if (Lang == "rtl")
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في الارسال";
                    _SystemAction.SaveAction("SaveProjectTrailing", "ProjectTrailingService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    massage = "فشل في الارسال";
                }
                else
                {
                    massage = "Failed send";
                }
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = massage };
            }
        }
        public GeneralMessage ReceiveProjectTrailing(ProjectTrailing ProjectTrailing, int UserId, int BranchId)
        {
            try
            {
                //var ProjectTrailingUpdated = _ProjectTrailingRepository.GetById(ProjectTrailing.TrailingId);
                ProjectTrailing? ProjectTrailingUpdated = _TaamerProContext.ProjectTrailing.Where(s => s.TrailingId == ProjectTrailing.TrailingId).FirstOrDefault();

                //var project = _ProjectRepository.GetById(ProjectTrailingUpdated.ProjectId);
                Project? project = _TaamerProContext.Project.Where(s => s.ProjectId == ProjectTrailingUpdated.ProjectId).FirstOrDefault();

                if (ProjectTrailingUpdated != null)
                {
                    ProjectTrailingUpdated.Active = true;
                    ProjectTrailingUpdated.Status = 1;
                    ProjectTrailingUpdated.TypeId = 1;
                    ProjectTrailingUpdated.Notes = ProjectTrailing.Notes;
                    ProjectTrailingUpdated.ReceiveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                    ProjectTrailingUpdated.ReceiveHijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar"));
                    ProjectTrailingUpdated.ReceiveUserId = UserId;
                    ProjectTrailingUpdated.UpdateUser = UserId;
                    ProjectTrailingUpdated.UpdateDate = DateTime.Now;
                    try
                    {
                        //  var user = _UsersRepository.GetById(UserId);
                        Users? user = _TaamerProContext.Users.Where(s => s.UserId == UserId).FirstOrDefault();

                        var ProNotification = new Notification();
                        ProNotification.Name = "اشعار علي مشروع رقم " + project.ProjectNo + "";
                        ProNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                        ProNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                        ProNotification.SendUserId = UserId;
                        ProNotification.Type = 1; // notification
                        ProNotification.Description = "تم استلام المشروع رقم " + project.ProjectNo + " بواسطة " + user.FullName + "";
                        ProNotification.AllUsers = true;
                        ProNotification.SendDate = DateTime.Now;
                        ProNotification.ProjectId = ProjectTrailing.ProjectId;
                        ProNotification.AddUser = UserId;
                        ProNotification.AddDate = DateTime.Now;
                        _TaamerProContext.Notification.Add(ProNotification);
                        _TaamerProContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة اشعار علي مشروع ";
                _SystemAction.SaveAction("ReceiveProjectTrailing", "ProjectTrailingService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedStr = project.ProjectNo };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                _SystemAction.SaveAction("ReceiveProjectTrailing", "ProjectTrailingService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage RejectProjectTrailing(ProjectTrailing ProjectTrailing, int UserId, int BranchId)
        {
            try
            {
                //var ProjectTrailingUpdated = _ProjectTrailingRepository.GetById(ProjectTrailing.TrailingId);
              //  var project = _ProjectRepository.GetById(ProjectTrailingUpdated.ProjectId);

                ProjectTrailing? ProjectTrailingUpdated = _TaamerProContext.ProjectTrailing.Where(s => s.TrailingId == ProjectTrailing.TrailingId).FirstOrDefault();

                 Project? project = _TaamerProContext.Project.Where(s => s.ProjectId == ProjectTrailingUpdated.ProjectId).FirstOrDefault();


                if (ProjectTrailingUpdated != null)
                {
                    ProjectTrailingUpdated.Active = false;
                    ProjectTrailingUpdated.Status = 1;
                    ProjectTrailingUpdated.TypeId = 0;
                    ProjectTrailingUpdated.Notes = ProjectTrailing.Notes;

                    var ProTrailing = new ProjectTrailing();
                    ProTrailing.ProjectId = ProjectTrailingUpdated.ProjectId;
                    ProTrailing.DepartmentId = ProjectTrailingUpdated.DepartmentId;
                    ProTrailing.Active = false;
                    ProTrailing.Status = 0;
                    ProTrailing.TypeId = 2;
                    ProTrailing.Date = ProjectTrailingUpdated.Date;
                    ProTrailing.BranchId = BranchId;
                    ProTrailing.AddUser = UserId;
                    ProTrailing.AddDate = DateTime.Now;
                    _TaamerProContext.ProjectTrailing.Add(ProTrailing);
                    try
                    {
                        //var user = _UsersRepository.GetById(UserId);
                        Users? user = _TaamerProContext.Users.Where(s => s.UserId == UserId).FirstOrDefault();

                        var ProNotification = new Notification();
                        ProNotification.Name = "اشعار علي مشروع رقم " + project.ProjectNo + "";
                        ProNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                        ProNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                        ProNotification.SendUserId = UserId;
                        ProNotification.Type = 1; // notification
                        ProNotification.Description = "تم رفض المشروع رقم " + project.ProjectNo + " بواسطة " + user.FullName + "";
                        ProNotification.AllUsers = true;
                        ProNotification.SendDate = DateTime.Now;
                        ProNotification.ProjectId = ProjectTrailing.ProjectId;
                        ProNotification.AddUser = UserId;
                        ProNotification.AddDate = DateTime.Now;
                        _TaamerProContext.Notification.Add(ProNotification);
                        _TaamerProContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "رفض المشروع ";
                _SystemAction.SaveAction("RejectProjectTrailing", "ProjectTrailingService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedFailed, ReturnedStr = project.ProjectNo };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في رفض الملف";
                _SystemAction.SaveAction("RejectProjectTrailing", "ProjectTrailingService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage AcceptProjectTrailing(int? TrailingId, int UserId, int BranchId)
        {
            try
            {
                //var ProjectTrailingUpdated = _ProjectTrailingRepository.GetById(TrailingId);
                ProjectTrailing? ProjectTrailingUpdated = _TaamerProContext.ProjectTrailing.Where(s => s.TrailingId == TrailingId).FirstOrDefault();

                //var project = _ProjectRepository.GetById(ProjectTrailingUpdated.ProjectId);
                Project? project = _TaamerProContext.Project.Where(s => s.ProjectId == ProjectTrailingUpdated.ProjectId).FirstOrDefault();


                if (ProjectTrailingUpdated != null)
                {
                    ProjectTrailingUpdated.Active = false;

                    var ProTrailing = new ProjectTrailing();
                    ProTrailing.ProjectId = ProjectTrailingUpdated.ProjectId;
                    ProTrailing.DepartmentId = ProjectTrailingUpdated.DepartmentId;
                    ProTrailing.Active = false;
                    ProTrailing.Status = 0;
                    ProTrailing.TypeId = 3;
                    ProTrailing.Date = ProjectTrailingUpdated.Date;
                    ProTrailing.BranchId = BranchId;
                    ProTrailing.AddUser = UserId;
                    ProTrailing.AddDate = DateTime.Now;
                    _TaamerProContext.ProjectTrailing.Add(ProTrailing);
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "الموافقة على المشروع ";
                _SystemAction.SaveAction("AcceptProjectTrailing", "ProjectTrailingService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedStr = project.ProjectNo };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "لم يتم الموافقة على المشروع";
                _SystemAction.SaveAction("AcceptProjectTrailing", "ProjectTrailingService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.projectHasNotApproved };
            }
        }
        public GeneralMessage DeleteProjectTrailing(int TrailingId, int UserId, int BranchId)
        {
            try
            {
              //  ProjectTrailing ProjectTrailing = _ProjectTrailingRepository.GetById(TrailingId);
                ProjectTrailing? ProjectTrailing = _TaamerProContext.ProjectTrailing.Where(s => s.TrailingId == TrailingId).FirstOrDefault();
                if (ProjectTrailing != null)
                {
                    ProjectTrailing.IsDeleted = true;
                    ProjectTrailing.DeleteDate = DateTime.Now;
                    ProjectTrailing.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف مهام رقم " + TrailingId;
                    _SystemAction.SaveAction("DeleteProjectTrailing", "ProjectTrailingService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في الحذف " ;
                _SystemAction.SaveAction("DeleteProjectTrailing", "ProjectTrailingService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public  Task<IEnumerable<TrailingFilesVM>> GetTrailingFilesByTrailingId(int? TrailingId, string SearchText)
        {
            var TrailingFiles = _TrailingFilesRepository.GetTrailingFilesByTrailingId(TrailingId, SearchText);
            return TrailingFiles;
        }
        

    }
}

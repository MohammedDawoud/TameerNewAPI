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
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using TaamerProject.Service.Generic;
using TaamerP.Service.LocalResources;
using TaamerProject.Repository.Repositories;
using static Dropbox.Api.UsersCommon.AccountType;

namespace TaamerProject.Service.Services
{
    public class TasksDependencyService :  ITasksDependencyService
    {
        private readonly ITasksDependencyRepository _TasksDependencyRepository;
        private readonly IProjectPhasesTasksRepository _ProjectPhasesTasksRepository;
        private readonly IProjectRepository _ProjectRepository;
        private readonly INodeLocationsRepository _NodeLocationsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IUsersRepository _UsersRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IBranchesRepository _BranchesRepository;

        public TasksDependencyService(TaamerProjectContext dataContext, ISystemAction systemAction, IDepartmentRepository departmentRepository,
            IUsersRepository usersRepository,ITasksDependencyRepository tasksDependencyRepository, IProjectPhasesTasksRepository projectPhasesTasksRepository,
            IProjectRepository projectRepository, IBranchesRepository branchesRepository, INodeLocationsRepository  nodeLocationsRepository)
        {
            _TasksDependencyRepository = tasksDependencyRepository;
            _ProjectPhasesTasksRepository = projectPhasesTasksRepository;
            _ProjectRepository = projectRepository;
            _NodeLocationsRepository = nodeLocationsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _UsersRepository = usersRepository;
            _departmentRepository = departmentRepository;
            _BranchesRepository = branchesRepository;

        }
        public Task<IEnumerable<TasksDependencyVM>> GetAllTasksDependencies(int BranchId)
        {
            var Dependencies = _TasksDependencyRepository.GetAllTasksDependencies(BranchId);
            return Dependencies;
        }
        public GeneralMessage SaveTasksDependency(TasksDependency TasksDependency, int UserId, int BranchId)
        {
            try
            {
                if (TasksDependency.DependencyId == 0)
                {
                    TasksDependency.AddUser = UserId;
                    TasksDependency.BranchId = BranchId;
                    TasksDependency.AddDate = DateTime.Now;
                    _TaamerProContext.TasksDependency.Add(TasksDependency);
                     _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة سير مهام جديد";
                    _SystemAction.SaveAction("SaveTasksDependency", "TasksDependencyService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                   // var TasksDependencyUpdated = _TasksDependencyRepository.GetById(TasksDependency.DependencyId);
                    TasksDependency? TasksDependencyUpdated =  _TaamerProContext.TasksDependency.Where(s => s.DependencyId == TasksDependency.DependencyId).FirstOrDefault();

                    if (TasksDependencyUpdated != null)
                    {
                        TasksDependencyUpdated.PredecessorId = TasksDependency.PredecessorId;
                        TasksDependencyUpdated.SuccessorId = TasksDependency.SuccessorId;
                        TasksDependencyUpdated.ProjSubTypeId = TasksDependency.ProjSubTypeId;
                        TasksDependencyUpdated.Type = TasksDependency.Type;
                        TasksDependencyUpdated.BranchId = TasksDependency.BranchId;
                        TasksDependencyUpdated.UpdateUser = UserId;
                        TasksDependencyUpdated.UpdateDate = DateTime.Now;
                    }
                     _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل سير مهام رقم " + TasksDependency.DependencyId;
                    _SystemAction.SaveAction("SaveTasksDependency", "TasksDependencyService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception)
            { //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ سير المهام";
                _SystemAction.SaveAction("SaveTasksDependency", "TasksDependencyService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage SaveDependencyPhasesTask(int ProjectId, List<TasksDependency> TaskLink,List<NodeLocations> NodeLocList, int UserId, int BranchId)
        {
            try
            {
               // var existingDependencySettings = _TasksDependencyRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == ProjectId);
                var existingDependencySettings = _TaamerProContext.TasksDependency.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId).ToList();
                if (existingDependencySettings.Count() > 0)
                {
                    _TaamerProContext.TasksDependency.RemoveRange(existingDependencySettings);

                }

                if (TaskLink != null)
                {
                    foreach (var item in TaskLink)
                    {
                        if (item.PredecessorId != null || item.SuccessorId != null)
                        {
                            var dependncy = new TasksDependency();
                            dependncy.PredecessorId = item.PredecessorId;
                            dependncy.SuccessorId = item.SuccessorId;
                            dependncy.Type = 0;
                            dependncy.ProjectId = ProjectId;


                            // dependncy.ProjSubTypeId = _ProjectRepository.GetById(ProjectId).SubProjectTypeId;
                            var ProjSubTypeId =  _TaamerProContext.Project.Where(s => s.ProjectId == ProjectId).FirstOrDefault()!.SubProjectTypeId??0;
                            dependncy.ProjSubTypeId = ProjSubTypeId;

                            dependncy.IsDeleted = false;
                            dependncy.AddDate = DateTime.Now;
                            dependncy.BranchId = BranchId;
                            dependncy.AddUser = UserId;
                            _TaamerProContext.TasksDependency.Add(dependncy);
                        }
                    }

                }
                //var existingNodeLocation = _NodeLocationsRepository.GetMatching(s => s.ProjectId == ProjectId);
                var existingNodeLocation = _TaamerProContext.NodeLocations.Where(s => s.ProjectId == ProjectId).ToList();

                if (existingNodeLocation.Count()>0)
                {
                    _TaamerProContext.NodeLocations.RemoveRange(existingNodeLocation);
                }

                if (NodeLocList != null)
                {
                    foreach (var item in NodeLocList)
                    {

                       // var SettingPhase = _ProjectPhasesTasksRepository.GetMatching(s => s.PhaseTaskId == item.TaskId && s.Type != 3 && s.ProjectId == ProjectId && s.IsDeleted==false).Select(x => x.PhaseTaskId);
                        var SettingPhase = _TaamerProContext.ProjectPhasesTasks.Where(s => s.PhaseTaskId == item.TaskId && s.Type != 3 && s.ProjectId == ProjectId && s.IsDeleted == false).Select(x => x.PhaseTaskId);

                        foreach (var i in SettingPhase)
                        {
                            //var count = _ProjectPhasesTasksRepository.GetMatching(s => s.Type == 3 && s.ProjectId == ProjectId && s.ParentId == i && s.IsDeleted==false).Count();
                            var count = _TaamerProContext.ProjectPhasesTasks.Where(s=>s.Type ==3 && s.ProjectId == ProjectId && s.ParentId == i && s.IsDeleted == false).Count();
                            if (count == 0)
                            { 
                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote = "فشل في حفظ سير مهام";
                                _SystemAction.SaveAction("SaveDependencyPhasesTask", "TasksDependencyService", 1, Resources.CanNotSaveTheStepsWithOutTasks, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                                //-----------------------------------------------------------------------------------------------------------------

                                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.CanNotSaveTheStepsWithOutTasks };
                            }
                        }


                        var Loc = new NodeLocations();
                        Loc.ProjectId = ProjectId;
                        Loc.TaskId = item.TaskId;
                        Loc.Location = item.Location;
                        Loc.AddDate = DateTime.Now;
                        Loc.AddUser = UserId;
                        _TaamerProContext.NodeLocations.Add(item);
                         _TaamerProContext.SaveChanges();

                       // var relatedTaks = _ProjectPhasesTasksRepository.GetById(item.TaskId);
                        var relatedTaks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.PhaseTaskId == item.TaskId).FirstOrDefault();
                        if (relatedTaks != null)
                        {
                            relatedTaks.LocationId = item.LocationId;
                        }
                       
                    }
                }
                else
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ سير مهام";
                    _SystemAction.SaveAction("SaveDependencyPhasesTask", "TasksDependencyService", 1, Resources.CanNotSaveTheStepsWithOutTasks, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.CanNotSaveTheStepsWithOutTasks };
                }

                 _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = "اضافة سير مهام جديد";
                _SystemAction.SaveAction("SaveDependencyPhasesTask", "TasksDependencyService", 1, Resources.TasksSavedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.TasksSavedSuccessfully};
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ سير مهام";
                _SystemAction.SaveAction("SaveDependencyPhasesTask", "TasksDependencyService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage SaveDependencyPhasesTaskNew(int ProjectId, List<TasksDependency> TaskLinkList, List<ProjectPhasesTasks> TasksList, int UserId, int BranchId)
        {
            try
            {
                var project = _ProjectRepository.GetById(ProjectId);
                project.UpdateUser = UserId;
                project.UpdateDate = DateTime.Now;

                var codePrefix = "";
                var prostartcode = _BranchesRepository.GetById(BranchId).TaskStartCode;
                if (prostartcode != null && prostartcode != "")
                {
                    codePrefix = prostartcode;
                }
                var Value = _ProjectPhasesTasksRepository.GenerateNextTaskNumber(BranchId, codePrefix, 0).Result;
                Value = Value - 1;

                var existingphases = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId).ToList(); ;

                if (existingphases != null && existingphases.Count() > 0)
                {
                    _TaamerProContext.ProjectPhasesTasks.RemoveRange(existingphases);
                }
                if (TasksList != null)
                {

                    foreach (var phases in TasksList)
                    {
                        phases.IsConverted = phases.IsConverted ?? 0;
                        phases.IsMerig = phases.IsMerig ?? -1;
                        string? NewValue = null;

                        if (phases.Type == 3)
                        {
                            Value = Value + 1;
                            NewValue = string.Format("{0:000000}", Value);
                            if (codePrefix != "")
                            {
                                NewValue = codePrefix + NewValue;
                            }
                            phases.Status = phases.Status ?? 1;

                            Users? BranchIdOfUser = _TaamerProContext.Users.Where(s => s.UserId == phases.UserId).FirstOrDefault();
                            var StartDateV = (phases.StartDateNew ?? DateTime.Now).ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            var EndDateV = (phases.EndDateNew ?? DateTime.Now).ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

                            var UserVacation = _TaamerProContext.Vacation.AsEnumerable().Where(s => s.IsDeleted == false && s.UserId == phases.UserId && s.VacationStatus == 2 && s.DecisionType == 1 && (s.BackToWorkDate == null || (s.BackToWorkDate ?? "") == "")).ToList();
                            UserVacation = UserVacation.Where(s =>

                            ((!(s.StartDate == null || s.StartDate.Equals("")) && !(phases.StartDateNew == null || phases.StartDateNew.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(StartDateV, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                                (!(s.StartDate == null || s.StartDate.Equals("")) && !(phases.EndDateNew == null || phases.EndDateNew.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(EndDateV, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                                ||
                                ((!(s.EndDate == null || s.EndDate.Equals("")) && !(phases.StartDateNew == null || phases.StartDateNew.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(StartDateV, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                                (!(s.EndDate == null || s.EndDate.Equals("")) && !(phases.EndDateNew == null || phases.EndDateNew.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(EndDateV, "yyyy-MM-dd", CultureInfo.InvariantCulture)))

                                ||
                                ((!(s.StartDate == null || s.StartDate.Equals("")) && !(phases.StartDateNew == null || phases.StartDateNew.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(StartDateV, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                                (!(s.EndDate == null || s.EndDate.Equals("")) && !(phases.StartDateNew == null || phases.StartDateNew.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(StartDateV, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                                ||
                                ((!(s.StartDate == null || s.StartDate.Equals("")) && !(phases.EndDateNew == null || phases.EndDateNew.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(EndDateV, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                                (!(s.EndDate == null || s.EndDate.Equals("")) && !(phases.EndDateNew == null || phases.EndDateNew.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(EndDateV, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                            ).ToList();
                            if (UserVacation.Count() != 0)
                            {
                                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Proj_SaveFailedUserVacationProSetting };
                            }


                            var BranchIdOfUserOrDepartment = 0;

                            if (phases.UserId != null)
                            {
                                BranchIdOfUserOrDepartment = _UsersRepository.GetById(phases.UserId ?? 0).BranchId ?? 0;
                            }
                            else
                            {
                                BranchIdOfUserOrDepartment = _departmentRepository.GetDepartmentbyid(phases.DepartmentId ?? 0).Result?.FirstOrDefault()?.BranchId ?? 0;
                            }
                            if (project != null)
                            {
                                BranchIdOfUserOrDepartment = project.BranchId;
                            }

                            phases.TaskNo = NewValue;
                            phases.TaskNoType = 1;
                            phases.TaskTimeFrom = (phases.StartDateNew ?? DateTime.Now).ToString("hh:mm tt", CultureInfo.InvariantCulture);
                            phases.TaskTimeTo = (phases.EndDateNew ?? DateTime.Now).ToString("hh:mm tt", CultureInfo.InvariantCulture);

                            phases.ProjSubTypeId = project?.SubProjectTypeId;
                            phases.EndTime = DateTime.Now.ToString("h:mm", CultureInfo.InvariantCulture);
                            var dateonly = phases.ExcpectedEndDate;
                            var timeonly = DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                            DateTime dt1 = DateTime.ParseExact(dateonly + " " + timeonly, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            phases.TaskFullTime = dt1.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);


                            phases.StartDate = (phases.StartDateNew ?? DateTime.Now).ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            phases.EndDate = (phases.EndDateNew ?? DateTime.Now).ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));



                            phases.BranchId = BranchIdOfUserOrDepartment;
                            phases.AddUser = UserId;
                            phases.AddDate = DateTime.Now;
                            phases.AddTaskUserId = UserId;

                            phases.PlusTime = phases.PlusTime ?? false;
                            phases.Managerapproval = phases.Managerapproval ?? null;
                            phases.ReasonsId = phases.ReasonsId ?? null;
                            phases.IsTemp = phases.IsTemp ?? null;

                            var DateDiff = ((phases.EndDateNew ?? new DateTime()) - (phases.StartDateNew ?? new DateTime()));
                            if(phases.Status==1)
                            {
                                phases.Remaining = Convert.ToInt32(DateDiff.TotalMinutes);
                            }
                            else
                            {
                                phases.Remaining = phases.Remaining;
                            }

                            var TimeType = 1;
                            var TimeMinutes = 0;

                            if (DateDiff.TotalHours < 24)
                            {
                                TimeType = 1;
                                TimeMinutes = Convert.ToInt32(DateDiff.TotalHours);
                            }
                            else
                            {
                                TimeType = 2;
                                TimeMinutes = Convert.ToInt32(DateDiff.TotalDays) + 1;
                            }
                            phases.TimeType = TimeType;
                            phases.TimeMinutes = TimeMinutes;

                            //phases.BranchId = BranchIdOfUser.BranchId;
                            phases.EndTime = DateTime.Now.ToString("h:mm");
                        }
                        else
                        {
                            phases.BranchId = project.BranchId;
                            NewValue = null;

                        }

                        if (phases.IsTemp==true)
                        {
                            phases.Status = 3;
                        }

                        if (phases.PhaseTaskId == 0)
                        {
                            phases.AddUser = UserId;
                            phases.AddDate = DateTime.Now;
                            _TaamerProContext.ProjectPhasesTasks.Add(phases);
                        }
                    }
                    _TaamerProContext.SaveChanges();

                    var tasksEdit = _TaamerProContext.ProjectPhasesTasks.Where(s => (s.Type == 2 || s.Type == 3) && s.ProjectId == ProjectId && s.IsDeleted == false).ToList();

                    foreach (var taskk in tasksEdit)
                    {
                        var ParentIdV = _TaamerProContext.ProjectPhasesTasks.Where(s => s.taskindex == taskk.ParentId && s.ProjectId == ProjectId && s.IsDeleted == false).Select(x => x.PhaseTaskId).FirstOrDefault();
                        taskk.ParentId = ParentIdV;
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------
                    var existingDependencySettings = _TaamerProContext.TasksDependency.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId).ToList();
                    if (existingDependencySettings.Count() > 0)
                    {
                        _TaamerProContext.TasksDependency.RemoveRange(existingDependencySettings);
                    }


                    if (TaskLinkList != null)
                    {
                        foreach (var item in TaskLinkList)
                        {
                            var PredecessorIdV = _TaamerProContext.ProjectPhasesTasks.Where(s => s.taskindex == item.PredecessorId && s.Type == 3 && s.ProjectId == ProjectId && s.IsDeleted == false).Select(x => x.PhaseTaskId).FirstOrDefault();
                            var SuccessorIdV = _TaamerProContext.ProjectPhasesTasks.Where(s => s.taskindex == item.SuccessorId && s.Type == 3 && s.ProjectId == ProjectId && s.IsDeleted == false).Select(x => x.PhaseTaskId).FirstOrDefault();

                            if (PredecessorIdV == 0 || SuccessorIdV == 0)
                            {

                            }
                            else
                            {
                                var dependncy = new TasksDependency();
                                dependncy.PredecessorId = PredecessorIdV;
                                dependncy.SuccessorId = SuccessorIdV;
                                dependncy.Type = 0;
                                dependncy.ProjectId = ProjectId;
                                dependncy.ProjSubTypeId = project?.SubProjectTypeId ?? 0;
                                dependncy.IsDeleted = false;
                                dependncy.AddDate = DateTime.Now;
                                dependncy.BranchId = BranchId;
                                dependncy.AddUser = UserId;
                                _TaamerProContext.TasksDependency.Add(dependncy);
                            }
                        }

                    }
                    _TaamerProContext.SaveChanges();
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.TasksSavedSuccessfully };
                }
                else
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }



            }
            catch (Exception ex)
            {
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ حفظ سير المهام";
                _SystemAction.SaveAction("SaveDependencySettings", "DependencySettingsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }



        public GeneralMessage DeleteTasksDependency(int DependencyId, int UserId, int BranchId)
        {
            try
            {
               // TasksDependency TasksDependency = _TasksDependencyRepository.GetById(DependencyId);
                TasksDependency? TasksDependency =   _TaamerProContext.TasksDependency.Where(s => s.DependencyId == DependencyId).FirstOrDefault();
                if (TasksDependency != null)
                {
                    TasksDependency.IsDeleted = true;
                    TasksDependency.DeleteDate = DateTime.Now;
                    TasksDependency.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                }
               
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف سير مهام رقم " + DependencyId;
                _SystemAction.SaveAction("DeleteTasksDependency", "TasksDependencyService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            { 
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف سير مهام رقم " + DependencyId; ;
                _SystemAction.SaveAction("DeleteTasksDependency", "TasksDependencyService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------


                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public ProjectTasksNodeVM GetTasksNodeByProjectId(int ProjectId)
        {
            var NodeTasks = new ProjectTasksNodeVM();
            NodeTasks.nodeDataArray = _ProjectPhasesTasksRepository.GetAllPhasesTasksByProjectId(ProjectId).Result;
            NodeTasks.linkDataArray = _TasksDependencyRepository.GetAllDependencyByProjectId(ProjectId).Result;
            //var succesorIds = _TasksDependencyRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == ProjectId && (s.PredecessorId !=0 && s.SuccessorId!=0)).Select(s => s.SuccessorId);
            var succesorIds = _TaamerProContext.TasksDependency.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId && (s.PredecessorId != 0 && s.SuccessorId != 0)).Select(s => s.SuccessorId);
           // var predecessorIds = _TasksDependencyRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == ProjectId && (s.PredecessorId != 0 && s.SuccessorId != 0)).Select(s => s.PredecessorId);
            var predecessorIds = _TaamerProContext.TasksDependency.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId && (s.PredecessorId != 0 && s.SuccessorId != 0)).Select(s => s.PredecessorId);

            NodeTasks.firstLevelNode = predecessorIds.Except(succesorIds).Select(s => new ProjectPhasesTasksVM
            {
                PhaseTaskId = s.Value,
            });
            return NodeTasks;
        }
        public ProjectTasksNodeVM GetTasksNodeByProjectIdNew(int ProjectId)
        {
            var NodeTasks = new ProjectTasksNodeVM();
            NodeTasks.nodeDataArray = _ProjectPhasesTasksRepository.GetAllPhasesTasksByProjectId(ProjectId).Result;
            NodeTasks.linkDataArray = _TasksDependencyRepository.GetAllDependencyByProjectIdNew(ProjectId).Result;
            return NodeTasks;
        }

        public TasksDependency GetTasksDependency(int ProjectId)
        {

            var ProjectIds = _TaamerProContext.TasksDependency.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId).FirstOrDefault();

            return ProjectIds??new TasksDependency();
        }

        public List<AccountTreeVM> GetProjectPhasesTaskTree(int ProjectId)
        {
            var ProPha = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId).OrderBy(s => s.PhaseTaskId).ToList();

            if (ProPha != null && ProPha.Count() > 0)
            {
                List<AccountTreeVM> treeItems = new List<AccountTreeVM>();
                foreach (var item in ProPha)
                {
                    treeItems.Add(new AccountTreeVM(item.PhaseTaskId.ToString(), ((item.ParentId == 0 || item.ParentId == null) ? "#" : item.ParentId.ToString()), item.DescriptionAr = item.DescriptionAr));
                }
                return treeItems;
            }
            else
            {
                return new List<AccountTreeVM>();
            }
        }


       
    }
}

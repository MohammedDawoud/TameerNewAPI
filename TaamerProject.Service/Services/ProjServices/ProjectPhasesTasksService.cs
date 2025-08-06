using Microsoft.Graph.Models;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.Enums;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Repository.Repositories;
using TaamerProject.Service.IGeneric;   //dd
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;
using Twilio.TwiML.Voice;

//using static iTextSharp.text.pdf.AcroFields;
using static System.Runtime.InteropServices.JavaScript.JSType;  

namespace TaamerProject.Service.Services
{
    public class ProjectPhasesTasksService : IProjectPhasesTasksService
    {

        private readonly IProjectPhasesTasksRepository _ProjectPhasesTasksRepository;
        private readonly IProUserPrivilegesRepository _ProUserPrivilegesRepository;
        private readonly IFileRepository _FileRepository;
        private readonly IProjectRequirementsRepository _ProjectRequirementsRepository;

        private readonly IProjectRepository _ProjectRepository;
        private readonly ISettingsRepository _SettingsRepository;
        private readonly IDependencySettingsRepository _DependencySettingsRepository;
        private readonly ITasksDependencyRepository _TasksDependencyRepository;
        private readonly IProjectWorkersRepository _ProjectWorkersRepository;
        private readonly INotificationRepository _NotificationRepository;
        private readonly IEmailSettingRepository _EmailSettingRepository;
        private readonly IUsersRepository _UsersRepository;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly ICustomerRepository _CustomerRepository;
        private readonly IUserPrivilegesRepository _UserPrivilegesRepository;
        private readonly ICostCenterRepository _CostCenterRepository;
        private readonly IVacationRepository _VacationRepository;
        private readonly IWorkOrdersRepository _workordersRepository;
        private readonly IProjectArchivesReRepository _ProjectArchivesReRepository;
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;
        private readonly IUserNotificationPrivilegesService _userNotificationPrivilegesService;
        private readonly IHomeService _homerservice;
        private readonly IProjectService _projectService;
        private readonly IOffersPricesRepository _offersPricesRepository;

        private readonly IProjectRequirementsGoalsRepository _projectRequirementsGoalsRepository;
        private readonly IOrganizationsRepository _OrganizationsRepository;
        private readonly INotificationService _notificationService;
        private readonly IDepartmentRepository _departmentRepository;

        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;


        public ProjectPhasesTasksService(IProjectRequirementsRepository ProjectRequirementsRepository, IFileRepository fileRepository, IProjectPhasesTasksRepository projectPhasesTasksRepository, IProUserPrivilegesRepository proUserPrivilegesRepository, IProjectRepository projectRepository,
            ISettingsRepository settingsRepository, IDependencySettingsRepository dependencySettingsRepository, ITasksDependencyRepository tasksDependencyRepository,
            IProjectWorkersRepository projectWorkersRepository, INotificationRepository notificationRepository, IEmailSettingRepository emailSettingRepository,
            IUsersRepository usersRepository, IBranchesRepository branchesRepository, ICustomerRepository customerRepository, IUserPrivilegesRepository userPrivilegesRepository,
           ICostCenterRepository costCenterRepository, IVacationRepository vacationRepository, IWorkOrdersRepository workOrdersRepository, IProjectArchivesReRepository projectArchivesReRepository,
           ISys_SystemActionsRepository sys_SystemActionsRepository, IUserNotificationPrivilegesService userNotificationPrivilegesService, IHomeService homeService,
           IProjectService projectService, IOffersPricesRepository offersPricesRepository, IProjectRequirementsGoalsRepository projectRequirementsGoalsRepository,
           IOrganizationsRepository organizationsRepository, INotificationService notificationService, TaamerProjectContext dataContext, ISystemAction systemAction, IDepartmentRepository departmentRepository)
        {
            _ProjectPhasesTasksRepository = projectPhasesTasksRepository;
            _ProUserPrivilegesRepository = proUserPrivilegesRepository;
            _FileRepository = fileRepository;
            _ProjectRequirementsRepository = ProjectRequirementsRepository;

            _ProjectRepository = projectRepository;
            _SettingsRepository = settingsRepository;
            _DependencySettingsRepository = dependencySettingsRepository;
            _TasksDependencyRepository = tasksDependencyRepository;
            _ProjectWorkersRepository = projectWorkersRepository;
            _NotificationRepository = notificationRepository;
            _EmailSettingRepository = emailSettingRepository;
            _UsersRepository = usersRepository;
            _BranchesRepository = branchesRepository;
            _CustomerRepository = customerRepository;
            _UserPrivilegesRepository = userPrivilegesRepository;
            _CostCenterRepository = costCenterRepository;
            _VacationRepository = vacationRepository;
            _workordersRepository = workOrdersRepository;
            _ProjectArchivesReRepository = projectArchivesReRepository;
            _Sys_SystemActionsRepository = sys_SystemActionsRepository;
            _userNotificationPrivilegesService = userNotificationPrivilegesService;
            _homerservice = homeService;
            _projectService = projectService;
            _offersPricesRepository = offersPricesRepository;
            _projectRequirementsGoalsRepository = projectRequirementsGoalsRepository;
            _OrganizationsRepository = organizationsRepository;
            _notificationService = notificationService;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesAndTasks(int projectId, string lang)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetAllProjectPhasesAndTasks(projectId, lang);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasks(string SearchText, int BranchId, string lang)
        {
            var ProjectPhasesTasks =await _ProjectPhasesTasksRepository.GetAllProjectPhasesTasks(SearchText, BranchId, lang);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasks_WithB(string SearchText, int BranchId, string lang)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetAllProjectPhasesTasks_WithB(SearchText, BranchId, lang);
            return ProjectPhasesTasks;
        }
        public async Task<ProjectPhasesTasksVM> GetProjectPhasesTasksbygoalandproject(int projectid, int progectgoal, int BranchId, string lang)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetProjectPhasesTasksbygoalandproject(projectid, progectgoal, BranchId, lang);
            return ProjectPhasesTasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksU(int UserId, int BranchId, string lang)
        {
            var Tasks = await _ProjectPhasesTasksRepository.GetAllProjectPhasesTasksU(UserId, BranchId, lang);
            return Tasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksU2(int UserId, int BranchId, string lang, string DateFrom, string DateTo)
        {
            var Tasks = await _ProjectPhasesTasksRepository.GetAllProjectPhasesTasksU2(UserId, BranchId, lang, DateFrom, DateTo);
            return Tasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksUPage(int UserId, int BranchId, string lang)
        {
            var Tasks = await _ProjectPhasesTasksRepository.GetAllProjectPhasesTasksUPage(UserId, BranchId, lang);
            return Tasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksS(int? UserId, int BranchId, int? status, string Lang, string DateFrom, string DateTo, List<int> BranchesList)
        {

            var Tasks = await _ProjectPhasesTasksRepository.GetAllProjectPhasesTasksS(UserId, BranchId, status, Lang, DateFrom, DateTo,BranchesList);
            return Tasks;
        }


        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksS(int? UserId, int BranchId, int? status, string Lang, string DateFrom, string DateTo,string? Searchtext)
        {

            var Tasks = await _ProjectPhasesTasksRepository.GetAllProjectPhasesTasksS(UserId, BranchId, status, Lang, DateFrom, DateTo, Searchtext);
            return Tasks;
        }


        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdrptsearch(int? UserId, int? status, string Lang, string DateFrom, string DateTo, int BranchId, List<int> BranchesList)
        {
            var Tasks = await _ProjectPhasesTasksRepository.GetLateTasksByUserIdrptsearch(UserId, status, Lang, DateFrom, DateTo, BranchId,BranchesList);

            return Tasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdrptsearch(int? UserId, int? status, string Lang, string DateFrom, string DateTo, int BranchId,string? SearchText)
        {
            var Tasks = await _ProjectPhasesTasksRepository.GetLateTasksByUserIdrptsearch(UserId, status, Lang, DateFrom, DateTo, BranchId, SearchText);

            return Tasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksbystatus(int? UserId, int BranchId, int? status, string Lang, string DateFrom, string DateTo, List<int> BranchesList)
        {
            var Tasks = await _ProjectPhasesTasksRepository.GetAllProjectPhasesTasksbystatus(UserId, BranchId, status, Lang, DateFrom, DateTo,BranchesList);
            return Tasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksbystatus(int? UserId, int BranchId, int? status, string Lang, string DateFrom, string DateTo,string? Searchtext)
        {
            var Tasks = await _ProjectPhasesTasksRepository.GetAllProjectPhasesTasksbystatus(UserId, BranchId, status, Lang, DateFrom, DateTo, Searchtext);
            return Tasks;
        }


        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasks_Costs(int? UserId, int BranchId, string Lang, string DateFrom, string DateTo, List<int> BranchesList)
        {
            var Tasks = await _ProjectPhasesTasksRepository.GetAllProjectPhasesTasks_Costs(UserId, BranchId, Lang, DateFrom, DateTo, BranchesList);
            return Tasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksW(int BranchId, string lang)
        {
            var Tasks = await _ProjectPhasesTasksRepository.GetAllProjectPhasesTasksW(BranchId, lang);
            return Tasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksPhasesByProjectId(int ProjectId, int BranchId)
        {
            var Tasks = await _ProjectPhasesTasksRepository.GetAllTasksPhasesByProjectId(ProjectId, BranchId);
            return Tasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllNewProjectPhasesTasks(string EndDateP, int BranchId)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetAllNewProjectPhasesTasks(EndDateP, BranchId);
            return ProjectPhasesTasks;
        }
        public  Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksWithoutUser(int? DepartmentId,int BranchId, string Lang)
        {
            var ProjectPhasesTasks =  _ProjectPhasesTasksRepository.GetTasksWithoutUser(DepartmentId, BranchId,Lang);
            return ProjectPhasesTasks;
        }
        public IEnumerable<TasksLoadVM> GetAllNewProjectPhasesTasksd(string EndDateP, int BranchId, string Lang)
        {
            var ProjectPhasesTasks = _ProjectPhasesTasksRepository.GetAllNewProjectPhasesTaskswithtime(EndDateP, BranchId, Lang).Result;
            var Projects = ProjectPhasesTasks?.GroupBy(x => x.ProjectId)?.Select(g => g.First())?.ToDictionary(x => x.ProjectId);

            if (ProjectPhasesTasks != null && ProjectPhasesTasks.Count() > 0)
            {
                List<TasksVM> treeItems = new List<TasksVM>();

                foreach (var item in ProjectPhasesTasks)
                {
                    var tamded = "";
                    var ta7weel = "";
                    if (item.PlusTime == true)
                    {
                        //tamded = "<i class='ri-time-line'></i>";
                        //tamded = "<span> <i class='fa-solid fa-folder-plus tree-icon'></i></span>";
                        //tamded = "(مطلوب تمديدها)";
                    }
                    if (item.IsConverted == 1)
                    {
                        //ta7weel = "<i class='fa fa-arrow-right'></i>";
                        //ta7weel = "<span> <i class='fa-solid fa-folder-plus tree-icon'></i></span>";
                        //ta7weel = "(مطلوب تحويلها)";
                    }
                    string TimeSTR = "";
                    if (!(item.EndDateNew==null || item.StartDateNew==null))
                    {
                        TimeSpan span = ((item.EndDateNew ?? new DateTime()) - (item.StartDateNew ?? new DateTime()));
                        if (span.Days > 0)
                        {
                            TimeSTR = TimeSTR + span.Days + " يوم ";
                        }
                        if (span.Hours > 0)
                        {
                            TimeSTR = TimeSTR + span.Hours + " ساعة ";
                        }
                        if (span.Minutes > 0)
                        {
                            TimeSTR = TimeSTR + span.Minutes + " دقيقة ";
                        }
                    }
                    else
                    {
                        TimeSTR = item.TimeStr;
                    }

                    treeItems.Add(new TasksVM(item.PhaseTaskId.ToString(), ((item.ProjectId == 0 || item.ProjectId == null) ? "#" : item.ProjectId.ToString() + "pr"),item.TaskNo+"-"+ item.DescriptionAr + "-" + TimeSTR + " " + tamded + " " + ta7weel, item.PhaseTaskId.ToString(), item.PlusTime, item.IsConverted));
                }
                List<TasksVM> treepro = new List<TasksVM>();
                foreach (var item in Projects)
                {
                    var Pro = (item.Key.ToString() + "pr");
                    var ListChild = treeItems.Where(s => s.parent == Pro).ToList();
                    treepro.Add(new TasksVM(item.Key.ToString() + "pr", "#", item.Value?.ProjectNumber?.ToString() + " - " + item.Value?.ClientName?.ToString() , ListChild, item.Key.ToString(), null, null));
                }
                var IteUnion = treeItems.Union(treepro);

                var objList = new List<TasksLoadVM>();
                var obj = new TasksLoadVM();
                var objCh = new ChildrenVM();
                var objChitem = new itemVM();

                foreach (var item in treepro)
                {
                    obj = new TasksLoadVM();
                    obj.id = item.id;
                    obj.name = item.name;
                    obj.children = new List<ChildrenVM>();
                    foreach (var item2 in item.children)
                    {
                        objCh = new ChildrenVM();
                        objCh.id = item2.id;
                        objCh.name = item2.name;
                        objCh.plusTime = item2.plusTime;
                        objCh.isConverted = item2.isConverted;
                        objChitem = new itemVM();
                        objChitem.phaseid= item2.id;
                        objChitem.phrase = item2.name;
                        objCh.item = objChitem;
                        obj.children!.Add(objCh);
                    }
                    objList.Add(obj);
                }
                return objList;
            }
            else
            {
                return new List<TasksLoadVM>();
            }
        }
        public IEnumerable<AccountTreeVM> GetAllNewProjectPhasesTasksTree(string EndDateP, int BranchId)
        {
            var ProjectPhasesTasks1 = _ProjectPhasesTasksRepository.GetAllNewProjectPhasesTasks(EndDateP, BranchId).Result.GroupBy(x => x.ProjectId).ToList();

            if (ProjectPhasesTasks1 != null && ProjectPhasesTasks1.Count() > 0)
            {
                List<AccountTreeVM> treeItems = new List<AccountTreeVM>();
                return treeItems;
            }
            else
            {
                return new List<AccountTreeVM>();
            }
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllNewProjectPhasesTasksByUserId(string EndDateP, int BranchId, int USERID)
        {
            var ProjectPhasesTasks =await _ProjectPhasesTasksRepository.GetAllNewProjectPhasesTasksByUserId(EndDateP, BranchId, USERID);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllLateProjectPhasesTasks(string EndDateP, int BranchId)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetAllLateProjectPhasesTasks(EndDateP, BranchId);
            return ProjectPhasesTasks;
        }
        public IEnumerable<TasksLoadVM> GetAllLateProjectPhasesTasksd(string EndDateP, int BranchId, string Lang)
        {
            var ProjectPhasesTasks = _ProjectPhasesTasksRepository.GetAllLateProjectPhasesTaskswithtime(EndDateP, BranchId, Lang).Result;
            var Projects = ProjectPhasesTasks?.GroupBy(x => x.ProjectId)?.Select(g => g.First())?.ToDictionary(x => x.ProjectId);

            if (ProjectPhasesTasks != null && ProjectPhasesTasks.Count() > 0)
            {
                List<TasksVM> treeItems = new List<TasksVM>();

                foreach (var item in ProjectPhasesTasks)
                {
                    var tamded = "";
                    var ta7weel = "";
                    if (item.PlusTime == true)
                    {
                        //tamded = "<i class='ri-time-line'></i>";
                        //tamded = "<span> <i class='fa-solid fa-folder-plus tree-icon'></i></span>";
                        //tamded = "(مطلوب تمديدها)";
                    }
                    if (item.IsConverted == 1)
                    {
                        //ta7weel = "<i class='fa fa-arrow-right'></i>";
                        //ta7weel = "<span> <i class='fa-solid fa-folder-plus tree-icon'></i></span>";
                        //ta7weel = "(مطلوب تحويلها)";
                    }
                    string TimeSTR = "";
                    if (!(item.EndDateNew == null || item.StartDateNew == null))
                    {
                        TimeSpan span = ((item.EndDateNew ?? new DateTime()) - (item.StartDateNew ?? new DateTime()));
                        if (span.Days > 0)
                        {
                            TimeSTR = TimeSTR + span.Days + " يوم ";
                        }
                        if (span.Hours > 0)
                        {
                            TimeSTR = TimeSTR + span.Hours + " ساعة ";
                        }
                        if (span.Minutes > 0)
                        {
                            TimeSTR = TimeSTR + span.Minutes + " دقيقة ";
                        }
                    }
                    else
                    {
                        TimeSTR = item.TimeStr;
                    }

                    treeItems.Add(new TasksVM(item.PhaseTaskId.ToString(), ((item.ProjectId == 0 || item.ProjectId == null) ? "#" : item.ProjectId.ToString() + "pr"),item.TaskNo+"-"+ item.DescriptionAr + "-" + item.TimeStr + " " + tamded + " " + ta7weel, item.PhaseTaskId.ToString(), item.PlusTime, item.IsConverted));
                }
                List<TasksVM> treepro = new List<TasksVM>();
                foreach (var item in Projects)
                {
                    var Pro = (item.Key.ToString() + "pr");
                    var ListChild = treeItems.Where(s => s.parent == Pro).ToList();
                    treepro.Add(new TasksVM(item.Key.ToString() + "pr", "#", item.Value?.ProjectNumber?.ToString() + " - " + item.Value?.ClientName?.ToString(), ListChild, item.Key.ToString(), null, null));
                }
                var IteUnion = treeItems.Union(treepro);

                var objList = new List<TasksLoadVM>();
                var obj = new TasksLoadVM();
                var objCh = new ChildrenVM();
                var objChitem = new itemVM();

                foreach (var item in treepro)
                {
                    obj = new TasksLoadVM();
                    obj.id = item.id;
                    obj.name = item.name;
                    obj.children = new List<ChildrenVM>();
                    foreach (var item2 in item.children)
                    {
                        objCh = new ChildrenVM();
                        objCh.id = item2.id;
                        objCh.name = item2.name;
                        objCh.plusTime = item2.plusTime;
                        objCh.isConverted = item2.isConverted;
                        objChitem = new itemVM();
                        objChitem.phaseid = item2.id;
                        objChitem.phrase = item2.name;
                        objCh.item = objChitem;
                        obj.children!.Add(objCh);
                    }
                    objList.Add(obj);
                }
                return objList;
            }
            else
            {
                return new List<TasksLoadVM>();
            }

        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllLateProjectPhasesTasksbyUserId(string EndDateP, int BranchId, int UserId)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetAllLateProjectPhasesTasksbyUserId(EndDateP, BranchId, UserId);
            return ProjectPhasesTasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllLateProjectPhasesTasksbyUserId2(string EndDateP, int BranchId, int? UserId, string Lang, List<int> BranchesList)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetAllLateProjectPhasesTasksbyUserId2(EndDateP, BranchId, UserId, Lang,BranchesList);
            return ProjectPhasesTasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllLateProjectPhasesTasksbyUserId2(string EndDateP, int BranchId, int? UserId, string Lang,string? SearchText)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetAllLateProjectPhasesTasksbyUserId2(EndDateP, BranchId, UserId, Lang, SearchText);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksByUserId(string SearchText, int? UserId, int BranchId)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetAllProjectPhasesTasksByUserId(SearchText, UserId, BranchId);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasks2(string SearchText, int BranchId, string Lang)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetAllProjectPhasesTasks2(SearchText, BranchId, Lang);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetProjectPhasesTasks2(int ProjectId, int BranchId, string Lang)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetProjectPhasesTasks2(ProjectId, BranchId, Lang);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<TasksLoadVM>> GetProjectPhasesTasks2Tree(int ProjectId, string SearchText, int BranchId, string Lang)
        {
            var ProjectPhasesTasks =  _ProjectPhasesTasksRepository.GetProjectPhasesTasks2(ProjectId, BranchId, Lang).Result.ToList();
            var Tasks = ProjectPhasesTasks.GroupBy(x => x.PhaseTaskId).Select(g => g.First()).ToDictionary(x => x.PhaseTaskId);

            if (SearchText != "" && SearchText != null)
            {
                ProjectPhasesTasks = ProjectPhasesTasks.Where(s => s.DescriptionAr.Contains(SearchText.Trim())).ToList();
            }
            List<ProjectRequirementsVM> ProjectRequirementsVMList = new List<ProjectRequirementsVM>();


            foreach (var phase in ProjectPhasesTasks)
            {
                var ProjectRequirements = _ProjectRequirementsRepository.GetAllProjectRequirementByTaskId(BranchId, phase.PhaseTaskId).Result.ToList();

                foreach (var item2 in ProjectRequirements)
                {
                    ProjectRequirementsVMList.Add(item2);
                }
            }

            if (Tasks != null && Tasks.Count() > 0)
            {
                List<TasksVM> treeItems = new List<TasksVM>();
                foreach (var item in ProjectRequirementsVMList)
                {
                    treeItems.Add(new TasksVM(item.RequirementId.ToString(), ((item.PhasesTaskID == 0 || item.PhasesTaskID == null) ? "#" : item.PhasesTaskID.ToString() + "pr"), item.NameAr + "-" + item.UserFullName, item.PhasesTaskID.ToString(),null,null));
                }

                List<TasksVM> treepro = new List<TasksVM>();
                foreach (var item in Tasks)
                {
                    var Pro = (item.Key.ToString() + "pr");
                    var ListChild = treeItems.Where(s => s.parent == Pro).ToList();
                    treepro.Add(new TasksVM(item.Key.ToString() + "pr", "#",item.Value?.TaskNo+" - "+ item.Value?.DescriptionAr?.ToString() + " - " + item.Value?.TimeStr?.ToString(), ListChild, item.Key.ToString(),null,null));
                }
                var IteUnion = treeItems.Union(treepro);

                var objList = new List<TasksLoadVM>();
                var obj = new TasksLoadVM();
                var objCh = new ChildrenVM();
                var objChitem = new itemVM();

                foreach (var item in treepro)
                {
                    obj = new TasksLoadVM();
                    obj.id = item.id;
                    obj.phaseTaskId = item.phaseTaskId;
                    obj.name = item.name;
                    obj.children = new List<ChildrenVM>();
                    if(item.children.Count()>0)
                    {
                        foreach (var item2 in item.children)
                        {
                            objCh = new ChildrenVM();
                            objCh.id = item2.id;
                            objCh.name = item2.name;
                            objCh.plusTime = item2.plusTime;
                            objCh.isConverted = item2.isConverted;
                            objChitem = new itemVM();
                            objChitem.phaseid = item2.id;
                            objChitem.phrase = item2.name;
                            objChitem.phaseTaskId = item.phaseTaskId;
                            objCh.item = objChitem;
                            obj.children!.Add(objCh);
                        }
                    }
                    else
                    {
                        objCh = new ChildrenVM();
                        objCh.id = "0";
                        objCh.name = "لا يوجد ملفات";
                        objChitem = new itemVM();
                        objChitem.phaseid = "0";
                        objChitem.phrase = "لا يوجد ";
                        objChitem.phaseTaskId = item.phaseTaskId;
                        objCh.item = objChitem;
                        obj.children!.Add(objCh);
                    }

                    objList.Add(obj);
                }
                return objList;
            }
            else
            {
                return new List<TasksLoadVM>();
            }
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasks(string SearchText, string Lang)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetInProgressProjectPhasesTasks(SearchText, Lang);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasks_Branches(int BranchId, string Lang)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetInProgressProjectPhasesTasks_Branches(BranchId, Lang);
            return ProjectPhasesTasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasks_Branches(int BranchId, string Lang,int? CustomerId)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetInProgressProjectPhasesTasks_Branchesfilterd(BranchId, Lang, CustomerId);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasks_Branches(int BranchId, string Lang, int? CustomerId,string? SearchText)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetInProgressProjectPhasesTasks_Branchesfilterd(BranchId, Lang, CustomerId, SearchText);
            return ProjectPhasesTasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasksHome(string SearchText, int BranchId, string Lang)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetInProgressProjectPhasesTasksHome(SearchText, BranchId, Lang);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasksHome_Search(ProjectPhasesTasksVM Search, string Lang)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetInProgressProjectPhasesTasksHome_Search(Search, Lang);
            return ProjectPhasesTasks;
        }
        public async Task<decimal?> GetProjectPhasesTasksCountByStatus(int? UserId, int Status, int BranchId)
        {
            decimal? ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetProjectPhasesTasksCountByStatus(UserId, Status, BranchId);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByUserId(int? UserId, int Status, int BranchId)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetTasksByUserId(UserId, Status, BranchId);
            return ProjectPhasesTasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByUserIdCustomerIdProjectId(int? UserId, int? CustomerId, int? ProjectId)
        {
            var ProjectPhaseTasks = await _ProjectPhasesTasksRepository.GetTasksByUserIdCustomerIdProjectId(UserId, CustomerId, ProjectId);
            return ProjectPhaseTasks;
        }




        public IEnumerable<TasksLoadVM> GetTasksByUserIdCustomerIdProjectIdTree(int? UserId, int? CustomerId, int? ProjectId,int? BrancId, string Lang)
        {
            var ProjectPhasesTasks = _ProjectPhasesTasksRepository.GetTasksByUserIdCustomerIdProjectIdwithtime(UserId, CustomerId, ProjectId, BrancId, Lang).Result;
            var Projects = ProjectPhasesTasks.GroupBy(x => x.ProjectId).Select(g => g.First()).ToDictionary(x => x.ProjectId);

            if (ProjectPhasesTasks != null && ProjectPhasesTasks.Count() > 0)
            {
                List<TasksVM> treeItems = new List<TasksVM>();


                foreach (var item in ProjectPhasesTasks)
                {
                    var tamded = "";
                    var ta7weel = "";
                    if (item.PlusTime == true)
                    {
                        //tamded = "<i class='ri-time-line'></i>";
                        //tamded = "<span> <i class='fa-solid fa-folder-plus tree-icon'></i></span>";
                        //tamded = "(مطلوب تمديدها)";
                    }
                    if (item.IsConverted == 1)
                    {
                        //ta7weel = "<i class='fa fa-arrow-right'></i>";
                        //ta7weel = "<span> <i class='fa-solid fa-folder-plus tree-icon'></i></span>";
                        //ta7weel = "(مطلوب تحويلها)";
                    }

                    string TimeSTR = "";
                    if (!(item.EndDateNew == null || item.StartDateNew == null))
                    {
                        TimeSpan span = ((item.EndDateNew ?? new DateTime()) - (item.StartDateNew ?? new DateTime()));
                        if (span.Days > 0)
                        {
                            TimeSTR = TimeSTR + span.Days + " يوم ";
                        }
                        if (span.Hours > 0)
                        {
                            TimeSTR = TimeSTR + span.Hours + " ساعة ";
                        }
                        if (span.Minutes > 0)
                        {
                            TimeSTR = TimeSTR + span.Minutes + " دقيقة ";
                        }
                    }
                    else
                    {
                        TimeSTR = item.TimeStr;
                    }

                    treeItems.Add(new TasksVM(item.PhaseTaskId.ToString(), ((item.ProjectId == 0 || item.ProjectId == null) ? "#" : item.ProjectId.ToString() + "pr"),item.TaskNo+"-"+ item.DescriptionAr + "-" + TimeSTR + " " + tamded + " " + ta7weel, item.PhaseTaskId.ToString(), item.PlusTime, item.IsConverted));
                }

                List<TasksVM> treepro = new List<TasksVM>();
                foreach (var item in Projects)
                {
                    var Pro = (item.Key.ToString() + "pr");
                    var ListChild = treeItems.Where(s => s.parent == Pro).ToList();
                    treepro.Add(new TasksVM(item.Key.ToString() + "pr", "#", item.Value?.ProjectNumber?.ToString() + " - " + item.Value?.ClientName?.ToString(), ListChild, item.Key.ToString(), null, null));
                }
                var IteUnion = treeItems.Union(treepro);

                var objList = new List<TasksLoadVM>();
                var obj = new TasksLoadVM();
                var objCh = new ChildrenVM();
                var objChitem = new itemVM();

                foreach (var item in treepro)
                {
                    obj = new TasksLoadVM();
                    obj.id = item.id;
                    obj.name = item.name;
                    obj.children = new List<ChildrenVM>();
                    foreach (var item2 in item.children)
                    {
                        objCh = new ChildrenVM();
                        objCh.id = item2.id;
                        objCh.name = item2.name;
                        objCh.plusTime = item2.plusTime;
                        objCh.isConverted = item2.isConverted;
                        objChitem = new itemVM();
                        objChitem.phaseid = item2.id;
                        objChitem.phrase = item2.name;
                        objCh.item = objChitem;
                        obj.children!.Add(objCh);
                    }
                    objList.Add(obj);
                }
                return objList;

            }
            else
            {
                return new List<TasksLoadVM>();
            }

        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByUserIdHome(int? UserId, int Status, int BranchId)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetTasksByUserIdHome(UserId, Status, BranchId);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByUserIdUser(int? UserId, string lang, int Status, int BranchId)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetTasksByUserIdUser(UserId, lang, Status, BranchId);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserId(int? UserId, int Status, int BranchId)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetLateTasksByUserId(UserId, Status, BranchId);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdHome(string EndDateP, int? UserId, int BranchId, string Lang)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetLateTasksByUserIdHome(EndDateP, UserId, BranchId, Lang);
            return ProjectPhasesTasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdHomefilterd(int? UserId, string Lang,int? ProjectId,int? CustomerId)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetLateTasksByUserIdHomefilterd(UserId, Lang, ProjectId, CustomerId);
            return ProjectPhasesTasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdHomefilterd(int? UserId, string Lang, int? ProjectId, int? CustomerId,string? Searchtext)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetLateTasksByUserIdHomefilterd(UserId, Lang, ProjectId, CustomerId, Searchtext);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdCustomerIdProjectId(int? UserId, int? CustomerId, int? ProjectId)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetLateTasksByUserIdCustomerIdProjectId(UserId, CustomerId, ProjectId);
            return ProjectPhasesTasks;
        }


        public IEnumerable<TasksLoadVM> GetLateTasksByUserIdCustomerIdProjectIdTree(int? UserId, int? CustomerId, int? ProjectId, int? BrancId, string Lang)
        {
            var ProjectPhasesTasks = _ProjectPhasesTasksRepository.GetLateTasksByUserIdCustomerIdProjectIdwithtime(UserId, CustomerId, ProjectId, BrancId, Lang).Result;
            var Projects = ProjectPhasesTasks.GroupBy(x => x.ProjectId).Select(g => g.First()).ToDictionary(x => x.ProjectId);

            if (ProjectPhasesTasks != null && ProjectPhasesTasks.Count() > 0)
            {
                List<TasksVM> treeItems = new List<TasksVM>();


                foreach (var item in ProjectPhasesTasks)
                {
                    var tamded = "";
                    var ta7weel = "";
                    if (item.PlusTime == true)
                    {
                        //tamded = "<i class='ri-time-line'></i>";
                        //tamded = "<span> <i class='fa-solid fa-folder-plus tree-icon'></i></span>";
                        //tamded = "(مطلوب تمديدها)";
                    }
                    if (item.IsConverted == 1)
                    {
                        //ta7weel = "<i class='fa fa-arrow-right'></i>";
                        //ta7weel = "<span> <i class='fa-solid fa-folder-plus tree-icon'></i></span>";
                        //ta7weel = "(مطلوب تحويلها)";
                    }

                    string TimeSTR = "";
                    if (!(item.EndDateNew == null || item.StartDateNew == null))
                    {
                        TimeSpan span = ((item.EndDateNew ?? new DateTime()) - (item.StartDateNew ?? new DateTime()));
                        if (span.Days > 0)
                        {
                            TimeSTR = TimeSTR + span.Days + " يوم ";
                        }
                        if (span.Hours > 0)
                        {
                            TimeSTR = TimeSTR + span.Hours + " ساعة ";
                        }
                        if (span.Minutes > 0)
                        {
                            TimeSTR = TimeSTR + span.Minutes + " دقيقة ";
                        }
                    }
                    else
                    {
                        TimeSTR = item.TimeStr;
                    }
                    treeItems.Add(new TasksVM(item.PhaseTaskId.ToString(), ((item.ProjectId == 0 || item.ProjectId == null) ? "#" : item.ProjectId.ToString() + "pr"),item.TaskNo+"-"+ item.DescriptionAr + "-" + TimeSTR + " " + tamded + " " + ta7weel, item.PhaseTaskId.ToString(), item.PlusTime, item.IsConverted));
                }
                List<TasksVM> treepro = new List<TasksVM>();
                foreach (var item in Projects)
                {
                    var Pro = (item.Key.ToString() + "pr");
                    var ListChild = treeItems.Where(s => s.parent == Pro).ToList();
                    treepro.Add(new TasksVM(item.Key.ToString() + "pr", "#", item.Value?.ProjectNumber?.ToString() + " - " + item.Value?.ClientName?.ToString(), ListChild, item.Key.ToString(), null, null));
                }
                var IteUnion = treeItems.Union(treepro);

                var objList = new List<TasksLoadVM>();
                var obj = new TasksLoadVM();
                var objCh = new ChildrenVM();
                var objChitem = new itemVM();

                foreach (var item in treepro)
                {
                    obj = new TasksLoadVM();
                    obj.id = item.id;
                    obj.name = item.name;
                    obj.children = new List<ChildrenVM>();
                    foreach (var item2 in item.children)
                    {
                        objCh = new ChildrenVM();
                        objCh.id = item2.id;
                        objCh.name = item2.name;
                        objCh.plusTime = item2.plusTime;
                        objCh.isConverted = item2.isConverted;
                        objChitem = new itemVM();
                        objChitem.phaseid = item2.id;
                        objChitem.phrase = item2.name;
                        objCh.item = objChitem;
                        obj.children!.Add(objCh);
                    }
                    objList.Add(obj);
                }
                return objList;
            }
            else
            {
                return new List<TasksLoadVM>();
            }

        }


        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdHome_Search(ProjectPhasesTasksVM Search, string Lang)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetLateTasksByUserIdHome_Search(Search, Lang);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetDayWeekMonth_Tasks(int? UserId, int Status, int BranchId, int Flag, string StartDate, string EndDate)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetDayWeekMonth_Tasks(UserId, Status, BranchId, Flag, StartDate, EndDate);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByDate(string StartDate, string EndDate, int BranchId)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetTasksByDate(StartDate, EndDate, BranchId);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByDateByUserId(string StartDate, string EndDate, int? UserId, int BranchId)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetTasksByDateByUserId(StartDate, EndDate, UserId, BranchId);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetNewTasksByUserId(string EndDateP, int? UserId, int BranchId, string Lang, bool? AllStatusExptEnd = null)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetNewTasksByUserId(EndDateP, UserId, BranchId, Lang, AllStatusExptEnd);
            return ProjectPhasesTasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetNewTasksByUserId2(int? UserId, string Lang,int? ProjectId,int? CustomerId)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetNewTasksByUserId2(UserId, Lang, ProjectId, CustomerId);
            return ProjectPhasesTasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetNewTasksByUserId2(int? UserId, string Lang, int? ProjectId, int? CustomerId,string? SearchText)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetNewTasksByUserId2(UserId, Lang, ProjectId, CustomerId, SearchText);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetNewTasksByUserIdSearchProj(string EndDateP, int? ProjectId, int? UserId, int BranchId, string Lang, bool? AllStatusExptEnd = null)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetNewTasksByUserIdSearchProj(EndDateP, ProjectId, UserId, BranchId, Lang, AllStatusExptEnd);
            return ProjectPhasesTasks;
        }
        public async Task<int> GetNewTasksCountByUserId(string EndDateP, int? UserId, int BranchId)
        {
            int ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetNewTasksCountByUserId(EndDateP, UserId, BranchId);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserId(string EndDateP, int? UserId, int BranchId)
        {
            var ProjectPhasesTasks =await _ProjectPhasesTasksRepository.GetLateTasksByUserId(EndDateP, UserId, BranchId);
            return ProjectPhasesTasks;
        }
        public async Task<int> GetLateTasksCountByUserId(string EndDateP, int? UserId, int BranchId, string Lang)
        {
            int ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetLateTasksCountByUserId(EndDateP, UserId, BranchId);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByUserIdAndProjectId(string EndDate, int? UserId, int BranchId)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetTasksByUserIdAndProjectId(EndDate, UserId, BranchId);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByStatus(int? StatusId, int BranchId)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetTasksByStatus(StatusId, BranchId);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByProjectNo(string ProjectNo, int BranchId)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetTasksByProjectNo(ProjectNo, BranchId);
            return ProjectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksSearch(ProjectPhasesTasksVM TasksSearch, int BranchId)
        {
            var ProjectPhasesTasks = await _ProjectPhasesTasksRepository.GetAllTasksSearch(TasksSearch, BranchId);
            return ProjectPhasesTasks;
        }
        //EditD1
        public GeneralMessage SaveProjectPhasesTasks(Project Project, int UserId, int BranchId, string Url, string ImgUrl)
        {
            var WhichPart = "Part Phase(1)";

            var projSubTypeSett2 = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId).ToList();
            if (projSubTypeSett2.Count() != 0)
            {
                foreach (var item in projSubTypeSett2)
                {
                    if (item.UserId != null)
                    {
                        var UserCheck = _TaamerProContext.Users.Where(s => s.UserId == item.UserId).FirstOrDefault();
                        if (UserCheck.IsDeleted == true)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote = Resources.General_SavedFailed + Project.ProjectNo;
                            _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.Save_Faild_Check_users, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Save_Faild_Check_users };
                        }
                        //var UserVacation = _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && s.UserId == item.UserId && s.VacationStatus != 4 && (s.BackToWorkDate == null || s.BackToWorkDate.Equals(""))).Count();
                        //if (UserVacation != 0)
                        //{
                        //    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Proj_SaveFailedUserVacationProSetting };
                        //}
                    }
                }
            }
            WhichPart = "Part Phase(2)";
            var Privs = Project.ProUserPrivileges;
            Project.ProUserPrivileges = new List<ProUserPrivileges>();
            try
            {
                if (Project.ProjectId == 0)
                {
                    var totaldays = 0.0;
                    DateTime resultEnd = DateTime.ParseExact(Project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime resultStart = DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    totaldays = (resultEnd - resultStart).TotalDays + 1;

                    ////////////////////////////// project ////////////////////////////////////////////////////////////////////////////////////////////
                    var codeExist = _ProjectRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId != Project.ProjectId && s.ProjectNo == Project.ProjectNo).FirstOrDefault();
                    if (codeExist != null)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حفظ المشروع" + Project.ProjectNo;
                       _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.ProjectNumberAlready };
                    }
                    DateTime VProjectDate = DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime VProjectExpireDate = DateTime.ParseExact(Project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (VProjectExpireDate.Date <= VProjectDate.Date)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حفظ المشروع" + Project.ProjectNo;
                        _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.EndProjectDate};
                    }
                    WhichPart = "Part Phase(3)";
                    Project.NoOfDays = Convert.ToInt32(totaldays);
                    Project.FirstProjectDate = Project.ProjectDate;
                    Project.FirstProjectExpireDate = Project.ProjectExpireDate;

                    Project.Status = 0;
                    Project.BranchId = BranchId;
                    Project.AddUser = UserId;
                    Project.AddDate = DateTime.Now;
                    _TaamerProContext.Project.Add(Project);
                    WhichPart = "Part Phase(4)";
                    _TaamerProContext.SaveChanges();
                    WhichPart = "Part Phase(5)";
                    Project.ProUserPrivileges = Privs;


                    try
                    {
                        if (Project.ProjectRequirementsGoals.Count() > 0)
                        {

                            foreach (var item in Project.ProjectRequirementsGoals.ToList())
                            {
                                item.RequirementGoalId = 0;
                                item.AddDate = DateTime.Now;
                                item.AddUser = UserId;
                                item.ProjectId = Project.ProjectId;
                                _TaamerProContext.ProjectRequirementsGoals.Add(item);
                                _TaamerProContext.SaveChanges();

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate7 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote7 = "فشل في حفظ أهداف المشروع" + Project.ProjectNo; ;
                        _SystemAction.SaveAction("SaveProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate7, UserId, BranchId, ActionNote7, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                    }


                    WhichPart = "Part Phase(6)";
                    /////add projectno to offerprice
                    if (Project.OffersPricesId != null)
                    {
                        try
                        {
                            var offer = _offersPricesRepository.GetById(Project.OffersPricesId??0);
                            offer.ProjectId = Project.ProjectId;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    WhichPart = "Part Phase(7)";
                    //var Customer = _CustomerRepository.GetById(Project.CustomerId ?? 0);
                    //// add cost center
                    ///
                    try
                    {

                        var newcostCenter = new CostCenters();
                        var CostCenterByid = _CostCenterRepository.GetById(Project.CostCenterId??0);
                        newcostCenter.ParentId = Project.CostCenterId;
                        newcostCenter.BranchId = CostCenterByid.BranchId;
                        newcostCenter.Code = Project.ProjectNo;
                        newcostCenter.NameAr = Project.CustomerName;
                        newcostCenter.NameEn = Project.CustomerName;
                        newcostCenter.AddDate = DateTime.Now;
                        newcostCenter.AddUser = UserId;
                        newcostCenter.CustomerId = Project.CustomerId;

                        newcostCenter.ProjId = Project.ProjectId;
                        _TaamerProContext.CostCenters.Add(newcostCenter);
                    }
                    catch (Exception ex)
                    {

                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate8 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote8 = "فشل في حفظ مركز تكلفة للمشروع" + Project.ProjectNo;
                       _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate8, UserId, BranchId, ActionNote8, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                    }
                    WhichPart = "Part Phase(8)";
                    _TaamerProContext.SaveChanges();
                    WhichPart = "Part Phase(9)";
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //////////////////////////////////////////// project tasks and phases ///////////////////////////////////////////////////////////////////////
                    //var projSubTypeSett = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId).ToList();
                    var projSubTypeSett = projSubTypeSett2;

                    var projectPhasesTaskList = new List<ProjectPhasesTasks>();
                    var projectPhasesTaskObj = new ProjectPhasesTasks();
                    var projectWorkers = new List<ProjectWorkers>();
                    var projectWorkersPriv = new List<UserPrivileges>();
                    var ListOfTaskNotify = new List<Notification>();

                    var TempTime = DateTime.Now.ToString("h:mm", CultureInfo.InvariantCulture);
                    var TempDate = Project.ProjectDate;
                    DateTime d = new DateTime();
                    DateTime oDate = DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    int? tempRemaining = 0;
                    if (projSubTypeSett.Count() != 0)
                    {
                        foreach (var item in projSubTypeSett)
                        {
                            if (item.TimeType == 1)       //hour
                            {
                                TempDate = Project.ProjectDate;
                                TempTime = DateTime.Now.AddHours(Convert.ToDouble(item.TimeMinutes)).ToString("h:mm");
                                tempRemaining = item.TimeMinutes * 60;
                            }
                            else        //day
                            {
                                d = oDate.AddDays(Convert.ToDouble(item.TimeMinutes));
                                TempDate = d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                                TempTime = DateTime.Now.ToString("h:mm", CultureInfo.InvariantCulture);
                                tempRemaining = item.TimeMinutes * 60 * 24;
                            }
                            var dateonly_A = TempDate;//mfroood hna l end
                            var timeonly_A = DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                            DateTime dt_A = DateTime.ParseExact(dateonly_A + " " + timeonly_A, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);



                            int? ParentV = null;
                            if (item.ParentId == null)
                            {
                                ParentV = null;
                            }
                            else
                            {
                                //ParentV = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId && s.ProjectId == Project.ProjectId && s.SettingId == item.ParentId).FirstOrDefault()?.PhaseTaskId ?? null;
                                ParentV = projectPhasesTaskList.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId && s.ProjectId == Project.ProjectId && s.SettingId == item.ParentId).FirstOrDefault()?.PhaseTaskId ?? null;                
                            }

                            projectPhasesTaskObj=new ProjectPhasesTasks
                            {
                                DescriptionAr = item.DescriptionAr,
                                DescriptionEn = item.DescriptionEn,
                                ProjSubTypeId = item.ProjSubTypeId,
                                Type = item.Type,
                                TimeMinutes = item.TimeMinutes,
                                Cost = item.Cost,
                                //Remaining = item.TimeMinutes,
                                Remaining = tempRemaining,
                                Status = (item.Type == 3) ? 1 : 0,
                                StopCount = 0,
                                TimeType = item.TimeType,
                                IsUrgent = item.IsUrgent,
                                TaskType = item.TaskType,
                                StartDate = null,
                                EndDate = null,
                                //ExcpectedStartDate = Project.ProjectDate,
                                //ExcpectedEndDate = TempDate,
                                
                                ExcpectedStartDate = null,
                                ExcpectedEndDate = null,
                                ParentId = ParentV,
                                UserId = item.UserId,
                                SettingId = item.SettingId,
                                PhasePriority = item.Priority,
                                ParentSettingId = ParentV,
                                Notes = item.Notes,
                                ProjectId = Project.ProjectId,//_ProjectRepository.GetMaxId() + 1;
                                BranchId = Project.BranchId,   //edit to set task with projectbranch
                                AddUser = UserId,
                                AddDate = DateTime.Now,
                                PlusTime = false,
                                IsConverted = 0,
                                IsMerig = item.IsMerig,
                                EndTime = TempTime,
                                TaskFullTime = dt_A.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture),
                                Totaltaskcost=item.Totaltaskcost,
                                Totalhourstask=item.Totalhourstask,
                                
                            };
                            _TaamerProContext.ProjectPhasesTasks.Add(projectPhasesTaskObj);
                            _TaamerProContext.SaveChanges();  // commit 
                            projectPhasesTaskList.Add(projectPhasesTaskObj);

                        }
                        //_TaamerProContext.ProjectPhasesTasks.AddRange(projectPhasesTaskList); ///////// project tasks pahses
                        //_TaamerProContext.SaveChanges();  // commit 

                    }
                    //update parentId
                    WhichPart = "Part Phase(10)";
                    //var LastAddedPhasesTask = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId && s.ProjectId == Project.ProjectId).ToList();
                    //foreach (var item in LastAddedPhasesTask)
                    //{
                    //    if (item.ParentSettingId == null)
                    //    {
                    //        item.ParentId = null;
                    //    }
                    //    else
                    //    {
                    //        var parent = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId && s.ProjectId == Project.ProjectId && s.SettingId == item.ParentSettingId).FirstOrDefault();
                    //        if (parent != null)
                    //        {
                    //            item.ParentId = parent.PhaseTaskId;
                    //        }

                    //    }
                    //}
                    WhichPart = "Part Phase(11)";
                    // save pro dependency  from setting dependency
                    var projSubTypeDependencySett = _TaamerProContext.DependencySettings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId).ToList();
                    
                    var projectPhasesCount = _ProjectPhasesTasksRepository.GetAllTasksByProjectIdWithoutBranch(Project.ProjectId, BranchId).Result.Count();
                    if (projectPhasesCount != 0)
                    {
                        foreach (var item in projSubTypeDependencySett)
                        {
                            var ProDependency = new TasksDependency();
                            ProDependency.ProjSubTypeId = item.ProjSubTypeId;
                            var Pre = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == item.ProjSubTypeId && s.SettingId == item.PredecessorId && s.ProjectId == Project.ProjectId && s.Type == 3).FirstOrDefault();
                            var PreSetting = 0;
                            if (Pre != null)
                            {
                                PreSetting = Pre.PhaseTaskId;
                            }
                            ProDependency.PredecessorId = PreSetting;
                            var Succ = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == item.ProjSubTypeId && s.SettingId == item.SuccessorId && s.ProjectId == Project.ProjectId && s.Type == 3).FirstOrDefault();
                            var SuccSetting = 0;
                            if (Succ != null)
                            {
                                SuccSetting = Succ.PhaseTaskId;
                            }
                            ProDependency.SuccessorId = SuccSetting;
                            ProDependency.Type = item.Type;
                            ProDependency.BranchId = BranchId;
                            ProDependency.AddUser = UserId;
                            ProDependency.ProjectId = Project.ProjectId;
                            ProDependency.AddDate = DateTime.Now;
                            _TaamerProContext.TasksDependency.Add(ProDependency);
                        }
                    }
                    WhichPart = "Part Phase(12)";
                    //////////////////////////////////////// tasks notifications //////////////////////////////////////////////////////////////////////////
                    ///
                    var branch = _BranchesRepository.GetById(BranchId);
                    //var customer = _CustomerRepository.GetById((int)Project.CustomerId);
                    foreach (var task in projectPhasesTaskList.Where(s => s.Type == 3))  //add tasks notifications
                    {

                        var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(task.UserId ?? 0).Result;
                        if (UserNotifPriv.Count() != 0)
                        {
                            if (UserNotifPriv.Contains(352))
                            {
                                try
                                {
                                    ListOfTaskNotify.Add(new Notification
                                    {
                                        ReceiveUserId = task.UserId,
                                        Name = Resources.General_Newtasks,
                                        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                        HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                        SendUserId = 1,
                                        Type = 1, // notification
                                        Description = " لديك مهمه جديدة : " + task.DescriptionAr + " علي مشروع رقم " + Project.ProjectNo + " للعميل " + Project.CustomerName + " " + "  فرع  " + branch.NameAr + "",
                                        AllUsers = false,
                                        SendDate = DateTime.Now,
                                        ProjectId = task.ProjectId,
                                        TaskId = task.PhaseTaskId,
                                        AddUser = UserId,
                                        AddDate = DateTime.Now,
                                        IsHidden = false
                                    });
                                    _notificationService.sendmobilenotification(task.UserId??0, Resources.General_Newtasks, "");// " لديك مهمه جديدة : " + task.DescriptionAr + " علي مشروع رقم " + Project.ProjectNo + " للعميل " + customer.CustomerNameAr + " " + "  فرع  " + branch.NameAr + "");

                                }
                                catch (Exception ex)
                                {
                                    //-----------------------------------------------------------------------------------------------------------------
                                    string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    string ActionNote4 = "فشل في ارسال اشعار مهمة";
                                   _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                                    //-----------------------------------------------------------------------------------------------------------------
                                }


                            }

                            if (UserNotifPriv.Contains(353))
                            {
                                try
                                {
                                    var userObj = _UsersRepository.GetById(task.UserId??0);

                                    var NotStr = Project.CustomerName + " للعميل  " + Project.ProjectNo + " علي مشروع رقم " + task.DescriptionAr + " لديك مهمه جديدة  ";
                                    if (userObj.Mobile != null && userObj.Mobile != "")
                                    {
                                        var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //-----------------------------------------------------------------------------------------------------------------
                                    string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    string ActionNote4 = "فشل في ارسال SMS مهمة";
                                   _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                                    //-----------------------------------------------------------------------------------------------------------------

                                }

                            }
                        }

                    }

                    _TaamerProContext.Notification.AddRange(ListOfTaskNotify);   /// add notifications
                    WhichPart = "Part Phase(13)";
                    if (Project.TransactionTypeId == 1)
                    {
                        Project.MotionProject = 1;
                        Project.MotionProjectDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        Project.MotionProjectNote = "أضافة فاتورة علي مشروع";

                        var ListOfPrivNotify = new List<Notification>();
                        var UserNotifPriv = _userNotificationPrivilegesService.GetUsersByPrivilegesIds(3252).Result;
                        if (UserNotifPriv.Count() != 0)
                        {
                            //_userPrivilegesRepository.GetMatching(s => s.IsDeleted == false && s.PrivilegeId == 131001).Where(w => w.Users.IsDeleted == false)
                            foreach (var userCounter in UserNotifPriv)
                            {
                                try
                                {
                                    ListOfPrivNotify.Add(new Notification
                                    {
                                        ReceiveUserId = userCounter.UserId,
                                        Name = Resources.MNAcc_Invoice,
                                        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                        HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                        SendUserId = UserId,
                                        Type = 1, // notification
                                        Description = " يوجد فاتورة جديدة علي مشروع رقم  : " + Project.ProjectNo + " للعميل " + Project.CustomerName + " " + " فرع  " + branch.NameAr + "",
                                        AllUsers = false,
                                        SendDate = DateTime.Now,
                                        ProjectId = 0,
                                        TaskId = 0,
                                        AddUser = UserId,
                                        AddDate = DateTime.Now,
                                        IsHidden = false
                                    });
                                    _notificationService.sendmobilenotification(userCounter.UserId??0, Resources.MNAcc_Invoice, " يوجد فاتورة جديدة علي مشروع رقم  : " + Project.ProjectNo + " للعميل " + Project.CustomerName + " " + " فرع  " + branch.NameAr + "");
                                }
                                catch (Exception ex)
                                {

                                    //-----------------------------------------------------------------------------------------------------------------
                                    string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    string ActionNote4 = "فشل في ارسال اشعار لمن لدية صلاحية فاتورة";
                                   _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                                    //-----------------------------------------------------------------------------------------------------------------
                                }

                            }

                            _TaamerProContext.Notification.AddRange(ListOfPrivNotify);

                        }

                        var UserNotifPriv_email = _userNotificationPrivilegesService.GetUsersByPrivilegesIds(3251).Result;
                        if (UserNotifPriv_email.Count() != 0)
                        {
                            foreach (var userCounter in UserNotifPriv_email)
                            {
                                try
                                {
                                    var Desc = " المستخدم " + userCounter.FullName + " تم اصدار فاتورة لمشروع رقم " + Project.ProjectNo + " للعميل " + Project.CustomerName + " فرع " + branch.NameAr;

                                    SendMailNoti(0, Desc, "اصدار فاتورة علي مشروع", BranchId, UserId, userCounter.UserId ?? 0);


                                    var htmlBody = "";


                                    htmlBody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                             
                                                <table style=' border: 1px solid black; border-collapse: collapse;' align='center'>
                                                  <tr>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>رقم المشروع</th>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>اسم العميل</th>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>الفرع </th>
                                                  </tr>
                                                    <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + Project.ProjectNo + @"</td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + Project.CustomerName + @"</td>
                                              <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + branch.NameAr + @"</td>
                                                    </tr>
                                                </table>
                                            </body>
                                            </html>";
                                    //SendMailNoti(projectId, Desc, "ايقاف مشروع", BranchId, UserId, proj.MangerId ?? 0);
                                    SendMail_ProjectStamp(BranchId, UserId, userCounter.UserId ?? 0, "اصدار فاتورة علي مشروع", htmlBody, Url, ImgUrl, 6, true);
                                }
                                catch (Exception ex)
                                {

                                    //-----------------------------------------------------------------------------------------------------------------
                                    string ActionDate5 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    string ActionNote5 = "فشل في ارسال ميل لمن لدية صلاحية فاتورة";
                                   _SystemAction.SaveAction("SaveProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate5, UserId, BranchId, ActionNote5, 0);
                                    //-----------------------------------------------------------------------------------------------------------------
                                }


                            }

                        }

                        var UserNotifPriv_Mobile = _userNotificationPrivilegesService.GetUsersByPrivilegesIds(3253).Result;
                        if (UserNotifPriv_Mobile.Count() != 0)
                        {
                            foreach (var userCounter in UserNotifPriv_Mobile)
                            {
                                try
                                {
                                    var userObj = _UsersRepository.GetById(userCounter.UserId??0);
                                    var NotStr = " المستخدم " + userCounter.FullName + " تم اصدار فاتورة لمشروع رقم " + Project.ProjectNo + " للعميل " + Project.CustomerName + " فرع " + branch.NameAr;
                                    if (userObj.Mobile != null && userObj.Mobile != "")
                                    {
                                        var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //-----------------------------------------------------------------------------------------------------------------
                                    string ActionDate6 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    string ActionNote6 = "فشل في ارسال SMS لمن لدية صلاحية فاتورة";
                                   _SystemAction.SaveAction("SaveProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate6, UserId, BranchId, ActionNote6, 0);
                                    //-----------------------------------------------------------------------------------------------------------------
                                }

                            }
                        }



                    }

                    WhichPart = "Part Phase(14)";
                    //////////////////////////////////////// project workers ///////////////////////////////////////////////////////////////////////////////
                    var projectusers = projSubTypeSett.Where(s => s.UserId != UserId && s.Type == 3).Select(s => s.UserId).Distinct();
                    projectWorkers.Add(new ProjectWorkers //// add current user as asenior project
                    {
                        ProjectId = Project.ProjectId,
                        //UserId = UserId,
                        UserId = Project.MangerId,
                        BranchId = BranchId,
                        WorkerType = 1,
                        IsDeleted = false,
                        AddDate = DateTime.Now,
                    });
                    WhichPart = "Part Phase(15)";
                    /////////////// priv
                    projectWorkersPriv.Add(new UserPrivileges
                    {
                        UserId = UserId,
                        PrivilegeId = 111026, // // finish proj
                        AddUser = UserId,
                        AddDate = DateTime.Now,
                    });
                    WhichPart = "Part Phase(16)";
                    if (Project.ProUserPrivileges != null && Project.ProUserPrivileges.Count > 0)
                    {
                        try
                        {
                            foreach (ProUserPrivileges priv in Project.ProUserPrivileges)
                            {
                                if (priv.UserPrivId == 0)
                                {

                                    projectWorkers.Add(new ProjectWorkers
                                    {
                                        UserId = priv.UserId,
                                        ProjectId = Project.ProjectId,
                                        BranchId = BranchId,
                                        WorkerType = 2,
                                        AddUser = UserId,
                                        AddDate = DateTime.Now,
                                    });
                                    //////////////// priv
                                    projectWorkersPriv.Add(new UserPrivileges
                                    {
                                        UserId = priv.UserId,
                                        PrivilegeId = 111026, // finish proj
                                        AddUser = UserId,
                                        AddDate = DateTime.Now,
                                    });


                                    priv.AddUser = UserId;
                                    priv.AddDate = DateTime.Now;
                                    _TaamerProContext.ProUserPrivileges.Add(priv);
                                    var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(priv.UserId ?? 0).Result;
                                    if (UserNotifPriv.Count() != 0)
                                    {
                                        if (UserNotifPriv.Contains(392))
                                        {
                                            try
                                            {
                                                ListOfTaskNotify.Add(new Notification
                                                {
                                                    ReceiveUserId = priv.UserId,
                                                    Name = "صلاحيات مشروع",
                                                    Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                                    HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                                    SendUserId = UserId,
                                                    Type = 1, // notification
                                                    Description = "  تم اضافتك علي مشروع رقم " + priv.Projectno + " للعميل " + Project.CustomerName,
                                                    AllUsers = false,
                                                    SendDate = DateTime.Now,
                                                    ProjectId = priv.ProjectID,
                                                    TaskId = 0,
                                                    AddUser = UserId,
                                                    AddDate = DateTime.Now,
                                                    IsHidden = false
                                                });
                                                _notificationService.sendmobilenotification(priv.UserId??0, "صلاحيات مشروع", "  تم اضافتك علي مشروع رقم " + priv.Projectno + " للعميل " + Project.CustomerName);
                                            }
                                            catch (Exception)
                                            {

                                            }


                                        }

                                        if (UserNotifPriv.Contains(391))
                                        {
                                            try
                                            {
                                                var Desc = Project.CustomerName + " للعميل " + priv.Projectno + "  تم اضافتك علي مشروع رقم ";

                                                SendMailNoti(Project.ProjectId, Desc, "اضافة علي مشروع", BranchId, UserId, priv.UserId ?? 0);

                                            }
                                            catch (Exception)
                                            {

                                            }

                                        }

                                        if (UserNotifPriv.Contains(393))
                                        {
                                            try
                                            {
                                                var userObj = _UsersRepository.GetById(priv.UserId??0);

                                                var NotStr = Project.CustomerName + " للعميل " + priv.Projectno + "  تم اضافتك علي مشروع رقم ";
                                                if (userObj.Mobile != null && userObj.Mobile != "")
                                                {
                                                    var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                                }
                                            }
                                            catch (Exception)
                                            {

                                            }

                                        }

                                    }
                                }
                            }
                            _TaamerProContext.Notification.AddRange(ListOfTaskNotify);
                        }
                        catch (Exception ex)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = "فشل في حفظ التعليق";
                           _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                        }
                    }
                    _TaamerProContext.ProjectWorkers.AddRange(projectWorkers); // add project users
                    _TaamerProContext.UserPrivileges.AddRange(projectWorkersPriv);
                    WhichPart = "Part Phase(17)";
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                WhichPart = "Part Phase(18)";
                _TaamerProContext.SaveChanges();
                WhichPart = "Part Phase(19)";

                SetExpectedDate(Project.ProjectId, Project.ProjectDate,false);
                WhichPart = "Part Phase(20)";
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة مشروع جديد" + "برقم" + Project.ProjectNo;
               _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.ProjectSaved, ReturnedStr = Project.ProjectId.ToString() };
            }
            catch (Exception ex)
            {
                SendMail_ProjectSavedWrong(BranchId, WhichPart + " " + ex.Message + ">>>>" + ex.InnerException, false);
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ المشروع";
               _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.ProjectFailed, ReturnedStr = WhichPart + " " + ex.Message + ">>>>" + ex.InnerException };
            }
        }
        public GeneralMessage SaveProjectPhasesTasksPart1(Project Project, int UserId, int BranchId, string Url, string ImgUrl)
        {
            var WhichPart = "Part Phase(1)";
            string VacMsg = "";
            var projSubTypeSett2 = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId).ToList();
            if (projSubTypeSett2.Count() != 0)
            {
                foreach (var item in projSubTypeSett2)
                {
                    if (item.UserId != null)
                    {
                        var UserCheck = _TaamerProContext.Users.Where(s => s.UserId == item.UserId).FirstOrDefault();
                        if (UserCheck.IsDeleted == true)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote = Resources.General_SavedFailed + Project.ProjectNo;
                            _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.Save_Faild_Check_users, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Save_Faild_Check_users };
                        }

                        var UserVacation = _TaamerProContext.Vacation.AsEnumerable().Where(s => s.IsDeleted == false && s.UserId == item.UserId && s.VacationStatus == 2 && s.DecisionType == 1 && (s.BackToWorkDate == null || (s.BackToWorkDate ?? "") == "")).ToList();
                        UserVacation = UserVacation.Where(s =>
                        // أو عنده إجازة في نفس وقت المهمة
                        ((!(s.StartDate == null || s.StartDate.Equals("")) && !(Project.ProjectDate == null || Project.ProjectDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                            (!(s.StartDate == null || s.StartDate.Equals("")) && !(Project.ProjectExpireDate == null || Project.ProjectExpireDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                            ||
                            ((!(s.EndDate == null || s.EndDate.Equals("")) && !(Project.ProjectDate == null || Project.ProjectDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                            (!(s.EndDate == null || s.EndDate.Equals("")) && !(Project.ProjectExpireDate == null || Project.ProjectExpireDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                        ||
                        ((!(s.StartDate == null || s.StartDate.Equals("")) && !(Project.ProjectDate == null || Project.ProjectDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                        (!(s.EndDate == null || s.EndDate.Equals("")) && !(Project.ProjectDate == null || Project.ProjectDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                        ||
                        ((!(s.StartDate == null || s.StartDate.Equals("")) && !(Project.ProjectExpireDate == null || Project.ProjectExpireDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                        (!(s.EndDate == null || s.EndDate.Equals("")) && !(Project.ProjectExpireDate == null || Project.ProjectExpireDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                        ).ToList();

                        if (UserVacation.Count() != 0)
                        {
                            VacMsg = " ولكن يوجد متستخدمين في اجازة في نفس فترة المشروع موجودين علي السير ";
                        }

                    }
                }
            }
            WhichPart = "Part Phase(2)";
            var Privs = Project.ProUserPrivileges;
            Project.ProUserPrivileges = new List<ProUserPrivileges>();
            try
            {
                if (Project.ProjectId == 0)
                {
                    var totaldays = 0.0;
                    DateTime resultEnd = DateTime.ParseExact(Project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime resultStart = DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    totaldays = (resultEnd - resultStart).TotalDays + 1;

                    ////////////////////////////// project ////////////////////////////////////////////////////////////////////////////////////////////
                    var codeExist = _ProjectRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId != Project.ProjectId && s.ProjectNo == Project.ProjectNo).FirstOrDefault();
                    if (codeExist != null)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حفظ المشروع" + Project.ProjectNo;
                        _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.ProjectNumberAlready };
                    }
                    DateTime VProjectDate = DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime VProjectExpireDate = DateTime.ParseExact(Project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (VProjectExpireDate.Date <= VProjectDate.Date)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حفظ المشروع" + Project.ProjectNo;
                        _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.EndProjectDate };
                    }
                    WhichPart = "Part Phase(3)";
                    Project.NoOfDays = Convert.ToInt32(totaldays);
                    Project.FirstProjectDate = Project.ProjectDate;
                    Project.FirstProjectExpireDate = Project.ProjectExpireDate;

                    Project.Status = 0;
                    Project.BranchId = BranchId;
                    Project.AddUser = UserId;
                    Project.AddDate = DateTime.Now;
                    _TaamerProContext.Project.Add(Project);
                    WhichPart = "Part Phase(4)";
                    _TaamerProContext.SaveChanges();
                    WhichPart = "Part Phase(5)";
                    Project.ProUserPrivileges = Privs;


                    _TaamerProContext.SaveChanges();
                    var projectWorkers = new List<ProjectWorkers>();
                    var projectWorkersPriv = new List<UserPrivileges>();
                    var ListOfTaskNotify = new List<Notification>();
                    //////////////////////////////////////// project workers ///////////////////////////////////////////////////////////////////////////////
                    projectWorkers.Add(new ProjectWorkers //// add current user as asenior project
                    {
                        ProjectId = Project.ProjectId,
                        //UserId = UserId,
                        UserId = Project.MangerId,
                        BranchId = BranchId,
                        WorkerType = 1,
                        IsDeleted = false,
                        AddDate = DateTime.Now,
                    });
                    WhichPart = "Part Phase(15)";
                    /////////////// priv
                    projectWorkersPriv.Add(new UserPrivileges
                    {
                        UserId = UserId,
                        PrivilegeId = 111026, // // finish proj
                        AddUser = UserId,
                        AddDate = DateTime.Now,
                    });
                    WhichPart = "Part Phase(16)";
                    if (Project.ProUserPrivileges != null && Project.ProUserPrivileges.Count > 0)
                    {
                        try
                        {
                            foreach (ProUserPrivileges priv in Project.ProUserPrivileges)
                            {
                                if (priv.UserPrivId == 0)
                                {

                                    projectWorkers.Add(new ProjectWorkers
                                    {
                                        UserId = priv.UserId,
                                        ProjectId = Project.ProjectId,
                                        BranchId = BranchId,
                                        WorkerType = 2,
                                        AddUser = UserId,
                                        AddDate = DateTime.Now,
                                    });
                                    //////////////// priv
                                    projectWorkersPriv.Add(new UserPrivileges
                                    {
                                        UserId = priv.UserId,
                                        PrivilegeId = 111026, // finish proj
                                        AddUser = UserId,
                                        AddDate = DateTime.Now,
                                    });


                                    priv.AddUser = UserId;
                                    priv.AddDate = DateTime.Now;
                                    _TaamerProContext.ProUserPrivileges.Add(priv);
                                    var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(priv.UserId ?? 0).Result;
                                    if (UserNotifPriv.Count() != 0)
                                    {
                                        if (UserNotifPriv.Contains(392))
                                        {
                                            try
                                            {
                                                ListOfTaskNotify.Add(new Notification
                                                {
                                                    ReceiveUserId = priv.UserId,
                                                    Name = "صلاحيات مشروع",
                                                    Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                                    HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                                    SendUserId = UserId,
                                                    Type = 1, // notification
                                                    Description = "  تم اضافتك علي مشروع رقم " + priv.Projectno + " للعميل " + Project.CustomerName,
                                                    AllUsers = false,
                                                    SendDate = DateTime.Now,
                                                    ProjectId = priv.ProjectID,
                                                    TaskId = 0,
                                                    AddUser = UserId,
                                                    AddDate = DateTime.Now,
                                                    IsHidden = false
                                                });
                                                _notificationService.sendmobilenotification(priv.UserId ?? 0, "صلاحيات مشروع", "  تم اضافتك علي مشروع رقم " + priv.Projectno + " للعميل " + Project.CustomerName);
                                            }
                                            catch (Exception)
                                            {

                                            }


                                        }

                                        if (UserNotifPriv.Contains(391))
                                        {
                                            try
                                            {
                                                var Desc = Project.CustomerName + " للعميل " + priv.Projectno + "  تم اضافتك علي مشروع رقم ";

                                                SendMailNoti(Project.ProjectId, Desc, "اضافة علي مشروع", BranchId, UserId, priv.UserId ?? 0);

                                            }
                                            catch (Exception)
                                            {

                                            }

                                        }

                                        if (UserNotifPriv.Contains(393))
                                        {
                                            try
                                            {
                                                var userObj = _UsersRepository.GetById(priv.UserId ?? 0);

                                                var NotStr = Project.CustomerName + " للعميل " + priv.Projectno + "  تم اضافتك علي مشروع رقم ";
                                                if (userObj.Mobile != null && userObj.Mobile != "")
                                                {
                                                    var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                                }
                                            }
                                            catch (Exception)
                                            {

                                            }

                                        }

                                    }
                                }
                            }
                            _TaamerProContext.Notification.AddRange(ListOfTaskNotify);
                        }
                        catch (Exception ex)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = "فشل في حفظ التعليق";
                            _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                        }
                    }
                    _TaamerProContext.ProjectWorkers.AddRange(projectWorkers); // add project users
                    _TaamerProContext.UserPrivileges.AddRange(projectWorkersPriv);
                    WhichPart = "Part Phase(17)";
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                WhichPart = "Part Phase(18)";
                _TaamerProContext.SaveChanges();
                WhichPart = "Part Phase(19)";

                WhichPart = "Part Phase(20)";
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة مشروع جديد" + "برقم" + Project.ProjectNo;
                _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.ProjectSaved, ReturnedStrExtra = VacMsg, ReturnedStr = Project.ProjectId.ToString() };
            }
            catch (Exception ex)
            {
                SendMail_ProjectSavedWrong(BranchId, WhichPart + " " + ex.Message + ">>>>" + ex.InnerException, false);
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ المشروع";
                _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.ProjectFailed, ReturnedStr = WhichPart + " " + ex.Message + ">>>>" + ex.InnerException };
            }
        }
        public GeneralMessage SaveProjectPhasesTasksPart2(Project Project, int UserId, int BranchId, string Url, string ImgUrl)
        {
            var projSubTypeSett = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId).ToList();
            //var projSubTypeSett = projSubTypeSett2;

            var projectPhasesTaskList = new List<ProjectPhasesTasks>();
            var projectPhasesTaskObj = new ProjectPhasesTasks();
            var ListOfTaskNotify = new List<Notification>();

            var codePrefix = "TSK#";
            //var prostartcode = _BranchesRepository.GetById(BranchId).TaskStartCode;
            //if (prostartcode != null && prostartcode != "")
            //{
            //    codePrefix = prostartcode;
            //}
            var Value = _ProjectPhasesTasksRepository.GenerateNextTaskNumber(BranchId, codePrefix, 0).Result;
            Value = Value - 1;


            var TempTime = DateTime.Now.ToString("h:mm", CultureInfo.InvariantCulture);
            var TempDate = Project.ProjectDate;
            DateTime d = new DateTime();
            DateTime oDate = DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            int? tempRemaining = 0;

            if (projSubTypeSett.Count() != 0)
            {
                foreach (var item in projSubTypeSett)
                {
                    string? NewValue = null;
                    if (item.Type == 3)
                    {
                        Value = Value + 1;
                        NewValue = string.Format("{0:000000}", Value);
                        if (codePrefix != "")
                        {
                            NewValue = codePrefix + NewValue;
                        }
                    }
                    else
                    {
                        NewValue = null;
                    }



                    if (item.TimeType == 1)       //hour
                    {
                        TempDate = Project.ProjectDate;
                        TempTime = DateTime.Now.AddHours(Convert.ToDouble(item.TimeMinutes)).ToString("h:mm");
                        tempRemaining = item.TimeMinutes * 60;
                    }
                    else        //day
                    {
                        d = oDate.AddDays(Convert.ToDouble(item.TimeMinutes));
                        TempDate = d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                        TempTime = DateTime.Now.ToString("h:mm", CultureInfo.InvariantCulture);
                        tempRemaining = item.TimeMinutes * 60 * 24;
                    }
                    var dateonly_A = TempDate;//mfroood hna l end
                    var timeonly_A = DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                    DateTime dt_A = DateTime.ParseExact(dateonly_A + " " + timeonly_A, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);



                    int? ParentV = null;
                    if (item.ParentId == null)
                    {
                        ParentV = null;
                    }
                    else
                    {
                        //ParentV = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId && s.ProjectId == Project.ProjectId && s.SettingId == item.ParentId).FirstOrDefault()?.PhaseTaskId ?? null;
                        ParentV = projectPhasesTaskList.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId && s.ProjectId == Project.ProjectId && s.SettingId == item.ParentId).FirstOrDefault()?.PhaseTaskId ?? null;
                    }

                    projectPhasesTaskObj = new ProjectPhasesTasks
                    {
                        DescriptionAr = item.DescriptionAr,
                        DescriptionEn = item.DescriptionEn,
                        ProjSubTypeId = item.ProjSubTypeId,
                        TaskNo = NewValue,
                        TaskNoType = 1,
                        Type = item.Type,
                        TimeMinutes = item.TimeMinutes,
                        Cost = item.Cost,
                        //Remaining = item.TimeMinutes,
                        Remaining = tempRemaining,
                        Status = (item.Type == 3) ? 1 : 0,
                        StopCount = 0,
                        TimeType = item.TimeType,
                        IsUrgent = item.IsUrgent,
                        TaskType = item.TaskType,
                        StartDate = null,
                        EndDate = null,
                        //ExcpectedStartDate = Project.ProjectDate,
                        //ExcpectedEndDate = TempDate,

                        ExcpectedStartDate = null,
                        ExcpectedEndDate = null,
                        ParentId = ParentV,
                        UserId = item.UserId,
                        SettingId = item.SettingId,
                        PhasePriority = item.Priority,
                        ParentSettingId = ParentV,
                        Notes = item.Notes,
                        ProjectId = Project.ProjectId,//_ProjectRepository.GetMaxId() + 1;
                        BranchId = Project.BranchId,   //edit to set task with projectbranch
                        AddUser = UserId,
                        AddDate = DateTime.Now,
                        PlusTime = false,
                        IsConverted = 0,
                        IsMerig = item.IsMerig,
                        EndTime = TempTime,
                        TaskFullTime = dt_A.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture),
                        Totaltaskcost = item.Totaltaskcost,
                        Totalhourstask = item.Totalhourstask,

                    };
                    _TaamerProContext.ProjectPhasesTasks.Add(projectPhasesTaskObj);
                    _TaamerProContext.SaveChanges();  // commit 
                    projectPhasesTaskList.Add(projectPhasesTaskObj);

                }
                //_TaamerProContext.ProjectPhasesTasks.AddRange(projectPhasesTaskList); ///////// project tasks pahses
                //_TaamerProContext.SaveChanges();  // commit 

            }
            // save pro dependency  from setting dependency
            var projSubTypeDependencySett = _TaamerProContext.DependencySettings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId).ToList();

            var projectPhasesCount = _ProjectPhasesTasksRepository.GetAllTasksByProjectIdWithoutBranch(Project.ProjectId, BranchId).Result.Count();
            if (projectPhasesCount != 0)
            {
                foreach (var item in projSubTypeDependencySett)
                {
                    var ProDependency = new TasksDependency();
                    ProDependency.ProjSubTypeId = item.ProjSubTypeId;
                    var Pre = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == item.ProjSubTypeId && s.SettingId == item.PredecessorId && s.ProjectId == Project.ProjectId && s.Type == 3).FirstOrDefault();
                    var PreSetting = 0;
                    if (Pre != null)
                    {
                        PreSetting = Pre.PhaseTaskId;
                    }
                    ProDependency.PredecessorId = PreSetting;
                    var Succ = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == item.ProjSubTypeId && s.SettingId == item.SuccessorId && s.ProjectId == Project.ProjectId && s.Type == 3).FirstOrDefault();
                    var SuccSetting = 0;
                    if (Succ != null)
                    {
                        SuccSetting = Succ.PhaseTaskId;
                    }
                    ProDependency.SuccessorId = SuccSetting;
                    ProDependency.Type = item.Type;
                    ProDependency.BranchId = BranchId;
                    ProDependency.AddUser = UserId;
                    ProDependency.ProjectId = Project.ProjectId;
                    ProDependency.AddDate = DateTime.Now;
                    _TaamerProContext.TasksDependency.Add(ProDependency);
                }
            }
            //////////////////////////////////////// tasks notifications //////////////////////////////////////////////////////////////////////////
            ///
            var branch = _BranchesRepository.GetById(BranchId);
            //var customer = _CustomerRepository.GetById((int)Project.CustomerId);
            foreach (var task in projectPhasesTaskList.Where(s => s.Type == 3))  //add tasks notifications
            {

                var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(task.UserId ?? 0).Result;
                if (UserNotifPriv.Count() != 0)
                {
                    if (UserNotifPriv.Contains(352))
                    {
                        try
                        {
                            ListOfTaskNotify.Add(new Notification
                            {
                                ReceiveUserId = task.UserId,
                                Name = Resources.General_Newtasks,
                                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                SendUserId = 1,
                                Type = 1, // notification
                                Description = " لديك مهمه جديدة : " + task.DescriptionAr + " رقم" + task.TaskNo + " علي مشروع رقم " + Project.ProjectNo + " للعميل " + Project.CustomerName + " " + "  فرع  " + branch.NameAr + "",
                                AllUsers = false,
                                SendDate = DateTime.Now,
                                ProjectId = task.ProjectId,
                                TaskId = task.PhaseTaskId,
                                AddUser = UserId,
                                AddDate = DateTime.Now,
                                IsHidden = false
                            });
                            _notificationService.sendmobilenotification(task.UserId ?? 0, Resources.General_Newtasks, "");// " لديك مهمه جديدة : " + task.DescriptionAr + " علي مشروع رقم " + Project.ProjectNo + " للعميل " + customer.CustomerNameAr + " " + "  فرع  " + branch.NameAr + "");

                        }
                        catch (Exception ex)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote4 = "فشل في ارسال اشعار مهمة";
                            _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                        }


                    }

                    if (UserNotifPriv.Contains(353))
                    {
                        try
                        {
                            var userObj = _UsersRepository.GetById(task.UserId ?? 0);

                            var NotStr = Project.CustomerName + " للعميل  " + Project.ProjectNo + " علي مشروع رقم "+ task.TaskNo +" رقم "+ task.DescriptionAr + " لديك مهمه جديدة  ";
                            if (userObj.Mobile != null && userObj.Mobile != "")
                            {
                                var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                            }
                        }
                        catch (Exception ex)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote4 = "فشل في ارسال SMS مهمة";
                            _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                            //-----------------------------------------------------------------------------------------------------------------

                        }

                    }
                }

            }

            _TaamerProContext.Notification.AddRange(ListOfTaskNotify);   /// add notifications

            _TaamerProContext.SaveChanges();
            SetExpectedDate(Project.ProjectId, Project.ProjectDate, false);

            return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.ProjectSaved, ReturnedStr = Project.ProjectId.ToString() };
        }
        public GeneralMessage SaveProjectPhasesTasksPart3(Project Project, int UserId, int BranchId, string Url, string ImgUrl)
        {
            try
            {
                if (Project.ProjectRequirementsGoals.Count() > 0)
                {

                    foreach (var item in Project.ProjectRequirementsGoals.ToList())
                    {
                        item.RequirementGoalId = 0;
                        item.AddDate = DateTime.Now;
                        item.AddUser = UserId;
                        item.ProjectId = Project.ProjectId;
                        _TaamerProContext.ProjectRequirementsGoals.Add(item);
                        _TaamerProContext.SaveChanges();

                    }
                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate7 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote7 = "فشل في حفظ أهداف المشروع" + Project.ProjectNo; ;
                _SystemAction.SaveAction("SaveProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate7, UserId, BranchId, ActionNote7, 0);
                //-----------------------------------------------------------------------------------------------------------------
            }


            if (Project.OffersPricesId != null)
            {
                try
                {
                    var offer = _offersPricesRepository.GetById(Project.OffersPricesId ?? 0);
                    offer.ProjectId = Project.ProjectId;
                }
                catch (Exception ex)
                {

                }
            }

            try
            {

                var newcostCenter = new CostCenters();
                var CostCenterByid = _CostCenterRepository.GetById(Project.CostCenterId ?? 0);
                newcostCenter.ParentId = Project.CostCenterId;
                newcostCenter.BranchId = CostCenterByid.BranchId;
                newcostCenter.Code = Project.ProjectNo;
                newcostCenter.NameAr = Project.CustomerName;
                newcostCenter.NameEn = Project.CustomerName;
                newcostCenter.AddDate = DateTime.Now;
                newcostCenter.AddUser = UserId;
                newcostCenter.CustomerId = Project.CustomerId;

                newcostCenter.ProjId = Project.ProjectId;
                _TaamerProContext.CostCenters.Add(newcostCenter);
            }
            catch (Exception ex)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate8 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote8 = "فشل في حفظ مركز تكلفة للمشروع" + Project.ProjectNo;
                _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate8, UserId, BranchId, ActionNote8, 0);
                //-----------------------------------------------------------------------------------------------------------------
            }

            if (Project.TransactionTypeId == 1)
            {
                var branch = _BranchesRepository.GetById(BranchId);

                Project.MotionProject = 1;
                Project.MotionProjectDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                Project.MotionProjectNote = "أضافة فاتورة علي مشروع";

                var ListOfPrivNotify = new List<Notification>();
                var UserNotifPriv = _userNotificationPrivilegesService.GetUsersByPrivilegesIds(3252).Result;
                if (UserNotifPriv.Count() != 0)
                {
                    //_userPrivilegesRepository.GetMatching(s => s.IsDeleted == false && s.PrivilegeId == 131001).Where(w => w.Users.IsDeleted == false)
                    foreach (var userCounter in UserNotifPriv)
                    {
                        try
                        {
                            ListOfPrivNotify.Add(new Notification
                            {
                                ReceiveUserId = userCounter.UserId,
                                Name = Resources.MNAcc_Invoice,
                                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                SendUserId = UserId,
                                Type = 1, // notification
                                Description = " يوجد فاتورة جديدة علي مشروع رقم  : " + Project.ProjectNo + " للعميل " + Project.CustomerName + " " + " فرع  " + branch.NameAr + "",
                                AllUsers = false,
                                SendDate = DateTime.Now,
                                ProjectId = 0,
                                TaskId = 0,
                                AddUser = UserId,
                                AddDate = DateTime.Now,
                                IsHidden = false
                            });
                            _notificationService.sendmobilenotification(userCounter.UserId ?? 0, Resources.MNAcc_Invoice, " يوجد فاتورة جديدة علي مشروع رقم  : " + Project.ProjectNo + " للعميل " + Project.CustomerName + " " + " فرع  " + branch.NameAr + "");
                        }
                        catch (Exception ex)
                        {

                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote4 = "فشل في ارسال اشعار لمن لدية صلاحية فاتورة";
                            _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                        }

                    }

                    _TaamerProContext.Notification.AddRange(ListOfPrivNotify);

                }

                var UserNotifPriv_email = _userNotificationPrivilegesService.GetUsersByPrivilegesIds(3251).Result;
                if (UserNotifPriv_email.Count() != 0)
                {
                    foreach (var userCounter in UserNotifPriv_email)
                    {
                        try
                        {
                            var Desc = " المستخدم " + userCounter.FullName + " تم اصدار فاتورة لمشروع رقم " + Project.ProjectNo + " للعميل " + Project.CustomerName + " فرع " + branch.NameAr;

                            SendMailNoti(0, Desc, "اصدار فاتورة علي مشروع", BranchId, UserId, userCounter.UserId ?? 0);


                            var htmlBody = "";


                            htmlBody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                             
                                                <table style=' border: 1px solid black; border-collapse: collapse;' align='center'>
                                                  <tr>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>رقم المشروع</th>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>اسم العميل</th>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>الفرع </th>
                                                  </tr>
                                                    <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + Project.ProjectNo + @"</td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + Project.CustomerName + @"</td>
                                              <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + branch.NameAr + @"</td>
                                                    </tr>
                                                </table>
                                            </body>
                                            </html>";
                            //SendMailNoti(projectId, Desc, "ايقاف مشروع", BranchId, UserId, proj.MangerId ?? 0);
                            SendMail_ProjectStamp(BranchId, UserId, userCounter.UserId ?? 0, "اصدار فاتورة علي مشروع", htmlBody, Url, ImgUrl, 6, true);
                        }
                        catch (Exception ex)
                        {

                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate5 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote5 = "فشل في ارسال ميل لمن لدية صلاحية فاتورة";
                            _SystemAction.SaveAction("SaveProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate5, UserId, BranchId, ActionNote5, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                        }


                    }

                }

                var UserNotifPriv_Mobile = _userNotificationPrivilegesService.GetUsersByPrivilegesIds(3253).Result;
                if (UserNotifPriv_Mobile.Count() != 0)
                {
                    foreach (var userCounter in UserNotifPriv_Mobile)
                    {
                        try
                        {
                            var userObj = _UsersRepository.GetById(userCounter.UserId ?? 0);
                            var NotStr = " المستخدم " + userCounter.FullName + " تم اصدار فاتورة لمشروع رقم " + Project.ProjectNo + " للعميل " + Project.CustomerName + " فرع " + branch.NameAr;
                            if (userObj.Mobile != null && userObj.Mobile != "")
                            {
                                var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                            }
                        }
                        catch (Exception ex)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate6 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote6 = "فشل في ارسال SMS لمن لدية صلاحية فاتورة";
                            _SystemAction.SaveAction("SaveProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate6, UserId, BranchId, ActionNote6, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                        }

                    }
                }



            }


            _TaamerProContext.SaveChanges();
            return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.ProjectSaved, ReturnedStr = Project.ProjectId.ToString() };
        }
        public GeneralMessage SaveProjectPhasesTasksNew(Project Project, int UserId, int BranchId, string Url, string ImgUrl)
        {
            Stopwatch sw;
            sw = Stopwatch.StartNew();

            var WhichPart = "Part Phase(1)";
            Console.WriteLine("Part Phase(1)   --  " +sw.ElapsedMilliseconds);

            var projSubTypeSett2 = _TaamerProContext.SettingsNew.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId).ToList();
            if (projSubTypeSett2.Count() != 0)
            {
                foreach (var item in projSubTypeSett2)
                {
                    if (item.UserId != null)
                    {
                        var UserCheck = _TaamerProContext.Users.Where(s => s.UserId == item.UserId).FirstOrDefault();
                        if (UserCheck.IsDeleted == true)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote = Resources.General_SavedFailed + Project.ProjectNo;
                            _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.Save_Faild_Check_users, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Save_Faild_Check_users };
                        }
                        //var UserVacation = _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && s.UserId == item.UserId && s.VacationStatus != 4 && (s.BackToWorkDate == null || s.BackToWorkDate.Equals(""))).Count();
                        //if (UserVacation != 0)
                        //{
                        //    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Proj_SaveFailedUserVacationProSetting };
                        //}
                    }
                }
            }
            WhichPart = "Part Phase(2)";
            Console.WriteLine("Part Phase(2)   --  " + sw.ElapsedMilliseconds);
            var Privs = Project.ProUserPrivileges;
            Project.ProUserPrivileges = new List<ProUserPrivileges>();
            try
            {
                if (Project.ProjectId == 0)
                {
                    var totaldays = 0.0;
                    DateTime resultEnd = DateTime.ParseExact(Project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime resultStart = DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    totaldays = (resultEnd - resultStart).TotalDays + 1;

                    ////////////////////////////// project ////////////////////////////////////////////////////////////////////////////////////////////
                    var codeExist = _ProjectRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId != Project.ProjectId && s.ProjectNo == Project.ProjectNo).FirstOrDefault();
                    if (codeExist != null)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حفظ المشروع" + Project.ProjectNo;
                        _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.ProjectNumberAlready };
                    }
                    DateTime VProjectDate = DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime VProjectExpireDate = DateTime.ParseExact(Project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (VProjectExpireDate.Date <= VProjectDate.Date)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حفظ المشروع" + Project.ProjectNo;
                        _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.EndProjectDate };
                    }
                    WhichPart = "Part Phase(3)";
                    Console.WriteLine("Part Phase(3)   --  " + sw.ElapsedMilliseconds);

                    Project.NoOfDays = Convert.ToInt32(totaldays);
                    Project.FirstProjectDate = Project.ProjectDate;
                    Project.FirstProjectExpireDate = Project.ProjectExpireDate;

                    Project.Status = 0;
                    Project.BranchId = BranchId;
                    Project.AddUser = UserId;
                    Project.AddDate = DateTime.Now;
                    _TaamerProContext.Project.Add(Project);
                    WhichPart = "Part Phase(4)";
                    Console.WriteLine("Part Phase(4)   --  " + sw.ElapsedMilliseconds);

                    _TaamerProContext.SaveChanges();
                    WhichPart = "Part Phase(5)";
                    Console.WriteLine("Part Phase(5)   --  " + sw.ElapsedMilliseconds);

                    Project.ProUserPrivileges = Privs;


                    try
                    {
                        if (Project.ProjectRequirementsGoals.Count() > 0)
                        {

                            foreach (var item in Project.ProjectRequirementsGoals.ToList())
                            {
                                item.RequirementGoalId = 0;
                                item.AddDate = DateTime.Now;
                                item.AddUser = UserId;
                                item.ProjectId = Project.ProjectId;
                                _TaamerProContext.ProjectRequirementsGoals.Add(item);
                                _TaamerProContext.SaveChanges();

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate7 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote7 = "فشل في حفظ أهداف المشروع" + Project.ProjectNo; ;
                        _SystemAction.SaveAction("SaveProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate7, UserId, BranchId, ActionNote7, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                    }


                    WhichPart = "Part Phase(6)";
                    Console.WriteLine("Part Phase(6)   --  " + sw.ElapsedMilliseconds);

                    /////add projectno to offerprice
                    if (Project.OffersPricesId != null)
                    {
                        try
                        {
                            var offer = _offersPricesRepository.GetById(Project.OffersPricesId ?? 0);
                            offer.ProjectId = Project.ProjectId;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    WhichPart = "Part Phase(7)";
                    Console.WriteLine("Part Phase(7)   --  " + sw.ElapsedMilliseconds);

                    //var Customer = _CustomerRepository.GetById(Project.CustomerId ?? 0);
                    //// add cost center
                    ///
                    try
                    {

                        var newcostCenter = new CostCenters();
                        var CostCenterByid = _CostCenterRepository.GetById(Project.CostCenterId ?? 0);
                        newcostCenter.ParentId = Project.CostCenterId;
                        newcostCenter.BranchId = CostCenterByid.BranchId;
                        newcostCenter.Code = Project.ProjectNo;
                        newcostCenter.NameAr = Project.CustomerName;
                        newcostCenter.NameEn = Project.CustomerName;
                        newcostCenter.AddDate = DateTime.Now;
                        newcostCenter.AddUser = UserId;
                        newcostCenter.CustomerId = Project.CustomerId;

                        newcostCenter.ProjId = Project.ProjectId;
                        _TaamerProContext.CostCenters.Add(newcostCenter);
                    }
                    catch (Exception ex)
                    {

                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate8 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote8 = "فشل في حفظ مركز تكلفة للمشروع" + Project.ProjectNo;
                        _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate8, UserId, BranchId, ActionNote8, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                    }
                    WhichPart = "Part Phase(8)";
                    Console.WriteLine("Part Phase(8)   --  " + sw.ElapsedMilliseconds);

                    _TaamerProContext.SaveChanges();
                    WhichPart = "Part Phase(9)";
                    Console.WriteLine("Part Phase(9)   --  " + sw.ElapsedMilliseconds);

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //////////////////////////////////////////// project tasks and phases ///////////////////////////////////////////////////////////////////////
                    //var projSubTypeSett = _TaamerProContext.SettingsNew.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId).ToList();
                    var projSubTypeSett = projSubTypeSett2;

                    var projectPhasesTaskList = new List<ProjectPhasesTasks>();
                    var projectPhasesTaskObj = new ProjectPhasesTasks();

                    var projectWorkers = new List<ProjectWorkers>();
                    var projectWorkersPriv = new List<UserPrivileges>();
                    var ListOfTaskNotify = new List<Notification>();

                    var TempTime = DateTime.Now.ToString("h:mm", CultureInfo.InvariantCulture);
                    var TempDate = Project.ProjectDate;
                    DateTime d = new DateTime();
                    DateTime oDate = DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    int? tempRemaining = 0;
                    if (projSubTypeSett.Count() != 0)
                    {
                        foreach (var item in projSubTypeSett)
                        {
                            var DateDiff = ((item.EndDate ?? new DateTime()) - (item.StartDate ?? new DateTime()));
                            tempRemaining = Convert.ToInt32(DateDiff.TotalMinutes);

                            if (item.TimeType == 1)       //hour
                            {
                                TempDate = Project.ProjectDate;
                                TempTime = DateTime.Now.AddHours(Convert.ToDouble(item.TimeMinutes)).ToString("h:mm");
                                //tempRemaining = item.TimeMinutes * 60;
                            }
                            else        //day
                            {
                                d = oDate.AddDays(Convert.ToDouble(item.TimeMinutes));
                                TempDate = d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                                TempTime = DateTime.Now.ToString("h:mm", CultureInfo.InvariantCulture);
                                //tempRemaining = item.TimeMinutes * 60 * 24;
                            }
                            var dateonly_A = TempDate;//mfroood hna l end
                            var timeonly_A = DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                            DateTime dt_A = DateTime.ParseExact(dateonly_A + " " + timeonly_A, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);



                            int? ParentV = null;
                            if (item.ParentId == null)
                            {
                                ParentV = null;
                            }
                            else
                            {
                                //ParentV = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId && s.ProjectId == Project.ProjectId && s.SettingId == item.ParentId).FirstOrDefault()?.PhaseTaskId ?? null;
                                ParentV = projectPhasesTaskList.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId && s.ProjectId == Project.ProjectId && s.SettingId == item.ParentId).FirstOrDefault()?.PhaseTaskId ?? null;

                            }

                            projectPhasesTaskObj =new ProjectPhasesTasks
                            {
                                DescriptionAr = item.DescriptionAr,
                                DescriptionEn = item.DescriptionEn,
                                ProjSubTypeId = item.ProjSubTypeId,
                                Type = item.Type,
                                TimeMinutes = item.TimeMinutes,
                                Cost = item.Cost,
                                //Remaining = item.TimeMinutes,
                                Remaining = tempRemaining,
                                Status =(item.Type == 3) ? item.IsTemp == true ? 3 : 1 : 0,
                                StopCount = 0,
                                TimeType = item.TimeType,
                                IsUrgent = item.IsUrgent,
                                TaskType = item.TaskType,
                                StartDate = null,
                                EndDate = null,
                                //ExcpectedStartDate = Project.ProjectDate,
                                //ExcpectedEndDate = TempDate,
                                ExcpectedStartDate = null,
                                ExcpectedEndDate = null,
                                ParentId = ParentV,
                                UserId = item.UserId,
                                SettingId = item.SettingId,
                                PhasePriority = item.Priority,
                                ParentSettingId = ParentV,
                                Notes = item.Notes,
                                ProjectId = Project.ProjectId,//_ProjectRepository.GetMaxId() + 1;
                                BranchId = Project.BranchId,   //edit to set task with projectbranch
                                AddUser = UserId,
                                AddDate = DateTime.Now,
                                PlusTime = false,
                                IsConverted = 0,
                                IsMerig = item.IsMerig,
                                EndTime = TempTime,
                                TaskFullTime = dt_A.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture),
                                indentation = item.indentation,
                                taskindex = item.taskindex,
                                ReasonsId = item.ReasonsId,
                                IsTemp=item.IsTemp,
                                Managerapproval = item.Managerapproval,
                                StartDateNew = item.StartDate,
                                EndDateNew = item.EndDate,
                                Totalhourstask =item.Totalhourstask,
                                Totaltaskcost=item.Totaltaskcost,

                             };
                            _TaamerProContext.ProjectPhasesTasks.Add(projectPhasesTaskObj);
                            _TaamerProContext.SaveChanges();  // commit 
                            projectPhasesTaskList.Add(projectPhasesTaskObj);

                        }
                        //_TaamerProContext.ProjectPhasesTasks.AddRange(projectPhasesTaskList); ///////// project tasks pahses
                        //_TaamerProContext.SaveChanges();  // commit 

                    }
                    //update parentId
                    WhichPart = "Part Phase(10)";
                    Console.WriteLine("Part Phase(10)   --  " + sw.ElapsedMilliseconds);

                    //var LastAddedPhasesTask = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId && s.ProjectId == Project.ProjectId).ToList();
                    //foreach (var item in LastAddedPhasesTask)
                    //{
                    //    if (item.ParentSettingId == null)
                    //    {
                    //        item.ParentId = null;
                    //    }
                    //    else
                    //    {
                    //        var parent = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId && s.ProjectId == Project.ProjectId && s.SettingId == item.ParentSettingId).FirstOrDefault();
                    //        if (parent != null)
                    //        {
                    //            item.ParentId = parent.PhaseTaskId;
                    //        }

                    //    }
                    //}
                    WhichPart = "Part Phase(11)";
                    Console.WriteLine("Part Phase(11)   --  " + sw.ElapsedMilliseconds);

                    // save pro dependency  from setting dependency
                    var projSubTypeDependencySett = _TaamerProContext.DependencySettingsNew.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId).ToList();

                    var projectPhasesCount = _ProjectPhasesTasksRepository.GetAllTasksByProjectIdWithoutBranch(Project.ProjectId, BranchId).Result.Count();
                    if (projectPhasesCount != 0)
                    {
                        foreach (var item in projSubTypeDependencySett)
                        {
                            var ProDependency = new TasksDependency();
                            ProDependency.ProjSubTypeId = item.ProjSubTypeId;
                            var Pre = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == item.ProjSubTypeId && s.SettingId == item.PredecessorId && s.ProjectId == Project.ProjectId && s.Type == 3).FirstOrDefault();
                            var PreSetting = 0;
                            if (Pre != null)
                            {
                                PreSetting = Pre.PhaseTaskId;
                            }
                            ProDependency.PredecessorId = PreSetting;
                            var Succ = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == item.ProjSubTypeId && s.SettingId == item.SuccessorId && s.ProjectId == Project.ProjectId && s.Type == 3).FirstOrDefault();
                            var SuccSetting = 0;
                            if (Succ != null)
                            {
                                SuccSetting = Succ.PhaseTaskId;
                            }
                            ProDependency.SuccessorId = SuccSetting;
                            ProDependency.Type = item.Type;
                            ProDependency.BranchId = BranchId;
                            ProDependency.AddUser = UserId;
                            ProDependency.ProjectId = Project.ProjectId;
                            ProDependency.AddDate = DateTime.Now;
                            _TaamerProContext.TasksDependency.Add(ProDependency);
                        }
                    }
                    WhichPart = "Part Phase(12)";
                    Console.WriteLine("Part Phase(12)   --  " + sw.ElapsedMilliseconds);

                    //////////////////////////////////////// tasks notifications //////////////////////////////////////////////////////////////////////////
                    ///
                    var branch = _BranchesRepository.GetById(BranchId);
                    //var customer = _CustomerRepository.GetById((int)Project.CustomerId);
                    foreach (var task in projectPhasesTaskList.Where(s => s.Type == 3))  //add tasks notifications
                    {

                        var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(task.UserId ?? 0).Result;
                        if (UserNotifPriv.Count() != 0)
                        {
                            if (UserNotifPriv.Contains(352))
                            {
                                try
                                {
                                    ListOfTaskNotify.Add(new Notification
                                    {
                                        ReceiveUserId = task.UserId,
                                        Name = Resources.General_Newtasks,
                                        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                        HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                        SendUserId = 1,
                                        Type = 1, // notification
                                        Description = " لديك مهمه جديدة : " + task.DescriptionAr + " علي مشروع رقم " + Project.ProjectNo + " للعميل " + Project.CustomerName + " " + "  فرع  " + branch.NameAr + "",
                                        AllUsers = false,
                                        SendDate = DateTime.Now,
                                        ProjectId = task.ProjectId,
                                        TaskId = task.PhaseTaskId,
                                        AddUser = UserId,
                                        AddDate = DateTime.Now,
                                        IsHidden = false
                                    });
                                    _notificationService.sendmobilenotification(task.UserId ?? 0, Resources.General_Newtasks, "");// " لديك مهمه جديدة : " + task.DescriptionAr + " علي مشروع رقم " + Project.ProjectNo + " للعميل " + customer.CustomerNameAr + " " + "  فرع  " + branch.NameAr + "");

                                }
                                catch (Exception ex)
                                {
                                    //-----------------------------------------------------------------------------------------------------------------
                                    string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    string ActionNote4 = "فشل في ارسال اشعار مهمة";
                                    _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                                    //-----------------------------------------------------------------------------------------------------------------
                                }


                            }

                            if (UserNotifPriv.Contains(353))
                            {
                                try
                                {
                                    var userObj = _UsersRepository.GetById(task.UserId ?? 0);

                                    var NotStr = Project.CustomerName + " للعميل  " + Project.ProjectNo + " علي مشروع رقم " + task.DescriptionAr + " لديك مهمه جديدة  ";
                                    if (userObj.Mobile != null && userObj.Mobile != "")
                                    {
                                        var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //-----------------------------------------------------------------------------------------------------------------
                                    string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    string ActionNote4 = "فشل في ارسال SMS مهمة";
                                    _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                                    //-----------------------------------------------------------------------------------------------------------------

                                }

                            }
                        }

                    }

                    _TaamerProContext.Notification.AddRange(ListOfTaskNotify);   /// add notifications
                    WhichPart = "Part Phase(13)";
                    Console.WriteLine("Part Phase(13)   --  " + sw.ElapsedMilliseconds);

                    if (Project.TransactionTypeId == 1)
                    {
                        Project.MotionProject = 1;
                        Project.MotionProjectDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        Project.MotionProjectNote = "أضافة فاتورة علي مشروع";

                        var ListOfPrivNotify = new List<Notification>();
                        var UserNotifPriv = _userNotificationPrivilegesService.GetUsersByPrivilegesIds(3252).Result;
                        if (UserNotifPriv.Count() != 0)
                        {
                            //_userPrivilegesRepository.GetMatching(s => s.IsDeleted == false && s.PrivilegeId == 131001).Where(w => w.Users.IsDeleted == false)
                            foreach (var userCounter in UserNotifPriv)
                            {
                                try
                                {
                                    ListOfPrivNotify.Add(new Notification
                                    {
                                        ReceiveUserId = userCounter.UserId,
                                        Name = Resources.MNAcc_Invoice,
                                        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                        HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                        SendUserId = UserId,
                                        Type = 1, // notification
                                        Description = " يوجد فاتورة جديدة علي مشروع رقم  : " + Project.ProjectNo + " للعميل " + Project.CustomerName + " " + " فرع  " + branch.NameAr + "",
                                        AllUsers = false,
                                        SendDate = DateTime.Now,
                                        ProjectId = 0,
                                        TaskId = 0,
                                        AddUser = UserId,
                                        AddDate = DateTime.Now,
                                        IsHidden = false
                                    });
                                    _notificationService.sendmobilenotification(userCounter.UserId ?? 0, Resources.MNAcc_Invoice, " يوجد فاتورة جديدة علي مشروع رقم  : " + Project.ProjectNo + " للعميل " + Project.CustomerName + " " + " فرع  " + branch.NameAr + "");
                                }
                                catch (Exception ex)
                                {

                                    //-----------------------------------------------------------------------------------------------------------------
                                    string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    string ActionNote4 = "فشل في ارسال اشعار لمن لدية صلاحية فاتورة";
                                    _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                                    //-----------------------------------------------------------------------------------------------------------------
                                }

                            }

                            _TaamerProContext.Notification.AddRange(ListOfPrivNotify);

                        }

                        var UserNotifPriv_email = _userNotificationPrivilegesService.GetUsersByPrivilegesIds(3251).Result;
                        if (UserNotifPriv_email.Count() != 0)
                        {
                            foreach (var userCounter in UserNotifPriv_email)
                            {
                                try
                                {
                                    var Desc = " المستخدم " + userCounter.FullName + " تم اصدار فاتورة لمشروع رقم " + Project.ProjectNo + " للعميل " + Project.CustomerName + " فرع " + branch.NameAr;

                                    SendMailNoti(0, Desc, "اصدار فاتورة علي مشروع", BranchId, UserId, userCounter.UserId ?? 0);


                                    var htmlBody = "";


                                    htmlBody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                             
                                                <table style=' border: 1px solid black; border-collapse: collapse;' align='center'>
                                                  <tr>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>رقم المشروع</th>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>اسم العميل</th>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>الفرع </th>
                                                  </tr>
                                                    <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + Project.ProjectNo + @"</td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + Project.CustomerName + @"</td>
                                              <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + branch.NameAr + @"</td>
                                                    </tr>
                                                </table>
                                            </body>
                                            </html>";
                                    //SendMailNoti(projectId, Desc, "ايقاف مشروع", BranchId, UserId, proj.MangerId ?? 0);
                                    SendMail_ProjectStamp(BranchId, UserId, userCounter.UserId ?? 0, "اصدار فاتورة علي مشروع", htmlBody, Url, ImgUrl, 6, true);
                                }
                                catch (Exception ex)
                                {

                                    //-----------------------------------------------------------------------------------------------------------------
                                    string ActionDate5 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    string ActionNote5 = "فشل في ارسال ميل لمن لدية صلاحية فاتورة";
                                    _SystemAction.SaveAction("SaveProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate5, UserId, BranchId, ActionNote5, 0);
                                    //-----------------------------------------------------------------------------------------------------------------
                                }


                            }

                        }

                        var UserNotifPriv_Mobile = _userNotificationPrivilegesService.GetUsersByPrivilegesIds(3253).Result;
                        if (UserNotifPriv_Mobile.Count() != 0)
                        {
                            foreach (var userCounter in UserNotifPriv_Mobile)
                            {
                                try
                                {
                                    var userObj = _UsersRepository.GetById(userCounter.UserId ?? 0);
                                    var NotStr = " المستخدم " + userCounter.FullName + " تم اصدار فاتورة لمشروع رقم " + Project.ProjectNo + " للعميل " + Project.CustomerName + " فرع " + branch.NameAr;
                                    if (userObj.Mobile != null && userObj.Mobile != "")
                                    {
                                        var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //-----------------------------------------------------------------------------------------------------------------
                                    string ActionDate6 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    string ActionNote6 = "فشل في ارسال SMS لمن لدية صلاحية فاتورة";
                                    _SystemAction.SaveAction("SaveProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate6, UserId, BranchId, ActionNote6, 0);
                                    //-----------------------------------------------------------------------------------------------------------------
                                }

                            }
                        }



                    }
                    Console.WriteLine("Part Phase(14)   --  " + sw.ElapsedMilliseconds);
                    WhichPart = "Part Phase(14)";
                    //////////////////////////////////////// project workers ///////////////////////////////////////////////////////////////////////////////
                    var projectusers = projSubTypeSett.Where(s => s.UserId != UserId && s.Type == 3).Select(s => s.UserId).Distinct();
                    projectWorkers.Add(new ProjectWorkers //// add current user as asenior project
                    {
                        ProjectId = Project.ProjectId,
                        //UserId = UserId,
                        UserId = Project.MangerId,
                        BranchId = BranchId,
                        WorkerType = 1,
                        IsDeleted = false,
                        AddDate = DateTime.Now,
                    });
                    WhichPart = "Part Phase(15)";
                    Console.WriteLine("Part Phase(15)   --  " + sw.ElapsedMilliseconds);

                    /////////////// priv
                    projectWorkersPriv.Add(new UserPrivileges
                    {
                        UserId = UserId,
                        PrivilegeId = 111026, // // finish proj
                        AddUser = UserId,
                        AddDate = DateTime.Now,
                    });
                    WhichPart = "Part Phase(16)";
                    Console.WriteLine("Part Phase(16)   --  " + sw.ElapsedMilliseconds);

                    if (Project.ProUserPrivileges != null && Project.ProUserPrivileges.Count > 0)
                    {
                        try
                        {
                            foreach (ProUserPrivileges priv in Project.ProUserPrivileges)
                            {
                                if (priv.UserPrivId == 0)
                                {

                                    projectWorkers.Add(new ProjectWorkers
                                    {
                                        UserId = priv.UserId,
                                        ProjectId = Project.ProjectId,
                                        BranchId = BranchId,
                                        WorkerType = 2,
                                        AddUser = UserId,
                                        AddDate = DateTime.Now,
                                    });
                                    //////////////// priv
                                    projectWorkersPriv.Add(new UserPrivileges
                                    {
                                        UserId = priv.UserId,
                                        PrivilegeId = 111026, // finish proj
                                        AddUser = UserId,
                                        AddDate = DateTime.Now,
                                    });


                                    priv.AddUser = UserId;
                                    priv.AddDate = DateTime.Now;
                                    _TaamerProContext.ProUserPrivileges.Add(priv);
                                    var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(priv.UserId ?? 0).Result;
                                    if (UserNotifPriv.Count() != 0)
                                    {
                                        if (UserNotifPriv.Contains(392))
                                        {
                                            try
                                            {
                                                ListOfTaskNotify.Add(new Notification
                                                {
                                                    ReceiveUserId = priv.UserId,
                                                    Name = "صلاحيات مشروع",
                                                    Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                                    HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                                    SendUserId = UserId,
                                                    Type = 1, // notification
                                                    Description = "  تم اضافتك علي مشروع رقم " + priv.Projectno + " للعميل " + Project.CustomerName,
                                                    AllUsers = false,
                                                    SendDate = DateTime.Now,
                                                    ProjectId = priv.ProjectID,
                                                    TaskId = 0,
                                                    AddUser = UserId,
                                                    AddDate = DateTime.Now,
                                                    IsHidden = false
                                                });
                                                _notificationService.sendmobilenotification(priv.UserId ?? 0, "صلاحيات مشروع", "  تم اضافتك علي مشروع رقم " + priv.Projectno + " للعميل " + Project.CustomerName);
                                            }
                                            catch (Exception)
                                            {

                                            }


                                        }

                                        if (UserNotifPriv.Contains(391))
                                        {
                                            try
                                            {
                                                var Desc = Project.CustomerName + " للعميل " + priv.Projectno + "  تم اضافتك علي مشروع رقم ";

                                                SendMailNoti(Project.ProjectId, Desc, "اضافة علي مشروع", BranchId, UserId, priv.UserId ?? 0);

                                            }
                                            catch (Exception)
                                            {

                                            }

                                        }

                                        if (UserNotifPriv.Contains(393))
                                        {
                                            try
                                            {
                                                var userObj = _UsersRepository.GetById(priv.UserId ?? 0);

                                                var NotStr = Project.CustomerName + " للعميل " + priv.Projectno + "  تم اضافتك علي مشروع رقم ";
                                                if (userObj.Mobile != null && userObj.Mobile != "")
                                                {
                                                    var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                                }
                                            }
                                            catch (Exception)
                                            {

                                            }

                                        }

                                    }
                                }
                            }
                            _TaamerProContext.Notification.AddRange(ListOfTaskNotify);
                        }
                        catch (Exception ex)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = "فشل في حفظ التعليق";
                            _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                        }
                    }
                    _TaamerProContext.ProjectWorkers.AddRange(projectWorkers); // add project users
                    _TaamerProContext.UserPrivileges.AddRange(projectWorkersPriv);
                    Console.WriteLine("Part Phase(17)   --  " + sw.ElapsedMilliseconds);

                    WhichPart = "Part Phase(17)";
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                WhichPart = "Part Phase(18)";
                Console.WriteLine("Part Phase(18)   --  " + sw.ElapsedMilliseconds);

                _TaamerProContext.SaveChanges();
                WhichPart = "Part Phase(19)";
                Console.WriteLine("Part Phase(19)   --  " + sw.ElapsedMilliseconds);


                SetExpectedDateNew(Project.ProjectId, Project.ProjectDate,true);
                WhichPart = "Part Phase(20)";
                Console.WriteLine("Part Phase(20)   --  " + sw.ElapsedMilliseconds);

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة مشروع جديد" + "برقم" + Project.ProjectNo;
                _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.ProjectSaved, ReturnedStr = Project.ProjectId.ToString() };
            }
            catch (Exception ex)
            {
                SendMail_ProjectSavedWrong(BranchId, WhichPart + " " + ex.Message + ">>>>" + ex.InnerException, false);
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ المشروع";
                _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.ProjectFailed, ReturnedStr = WhichPart + " " + ex.Message + ">>>>" + ex.InnerException };
            }
        }
        public GeneralMessage SaveProjectPhasesTasksNewPart1(Project Project, int UserId, int BranchId, string Url, string ImgUrl)
        {
            var WhichPart = "Part Phase(1)";
            string VacMsg = "";
            var projSubTypeSett2 = _TaamerProContext.SettingsNew.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId).ToList();
            if (projSubTypeSett2.Count() != 0)
            {
                foreach (var item in projSubTypeSett2)
                {
                    if (item.UserId != null)
                    {
                        var UserCheck = _TaamerProContext.Users.Where(s => s.UserId == item.UserId).FirstOrDefault();
                        if (UserCheck.IsDeleted == true)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote = Resources.General_SavedFailed + Project.ProjectNo;
                            _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.Save_Faild_Check_users, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Save_Faild_Check_users };
                        }

                        var UserVacation = _TaamerProContext.Vacation.AsEnumerable().Where(s => s.IsDeleted == false && s.UserId == item.UserId && s.VacationStatus == 2 && s.DecisionType == 1 && (s.BackToWorkDate == null || (s.BackToWorkDate ?? "") == "")).ToList();
                        UserVacation = UserVacation.Where(s =>
                        // أو عنده إجازة في نفس وقت المهمة
                        ((!(s.StartDate == null || s.StartDate.Equals("")) && !(Project.ProjectDate == null || Project.ProjectDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                            (!(s.StartDate == null || s.StartDate.Equals("")) && !(Project.ProjectExpireDate == null || Project.ProjectExpireDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                            ||
                            ((!(s.EndDate == null || s.EndDate.Equals("")) && !(Project.ProjectDate == null || Project.ProjectDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                            (!(s.EndDate == null || s.EndDate.Equals("")) && !(Project.ProjectExpireDate == null || Project.ProjectExpireDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                        ||
                        ((!(s.StartDate == null || s.StartDate.Equals("")) && !(Project.ProjectDate == null || Project.ProjectDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                        (!(s.EndDate == null || s.EndDate.Equals("")) && !(Project.ProjectDate == null || Project.ProjectDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                        ||
                        ((!(s.StartDate == null || s.StartDate.Equals("")) && !(Project.ProjectExpireDate == null || Project.ProjectExpireDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                        (!(s.EndDate == null || s.EndDate.Equals("")) && !(Project.ProjectExpireDate == null || Project.ProjectExpireDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                            ).ToList();

                        if (UserVacation.Count() != 0)
                        {
                            VacMsg = " ولكن يوجد متستخدمين في اجازة في نفس فترة المشروع موجودين علي السير ";
                        }

                    }
                }
            }
            WhichPart = "Part Phase(2)";
            var Privs = Project.ProUserPrivileges;
            Project.ProUserPrivileges = new List<ProUserPrivileges>();
            try
            {
                if (Project.ProjectId == 0)
                {
                    var totaldays = 0.0;
                    DateTime resultEnd = DateTime.ParseExact(Project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime resultStart = DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    totaldays = (resultEnd - resultStart).TotalDays + 1;

                    ////////////////////////////// project ////////////////////////////////////////////////////////////////////////////////////////////
                    var codeExist = _ProjectRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId != Project.ProjectId && s.ProjectNo == Project.ProjectNo).FirstOrDefault();
                    if (codeExist != null)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حفظ المشروع" + Project.ProjectNo;
                        _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.ProjectNumberAlready };
                    }
                    DateTime VProjectDate = DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime VProjectExpireDate = DateTime.ParseExact(Project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (VProjectExpireDate.Date <= VProjectDate.Date)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حفظ المشروع" + Project.ProjectNo;
                        _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.EndProjectDate };
                    }
                    Project.NoOfDays = Convert.ToInt32(totaldays);
                    Project.FirstProjectDate = Project.ProjectDate;
                    Project.FirstProjectExpireDate = Project.ProjectExpireDate;

                    Project.Status = 0;
                    Project.BranchId = BranchId;
                    Project.AddUser = UserId;
                    Project.AddDate = DateTime.Now;
                    _TaamerProContext.Project.Add(Project);

                    _TaamerProContext.SaveChanges();

                    Project.ProUserPrivileges = Privs;

                    _TaamerProContext.SaveChanges();
                    var projectWorkers = new List<ProjectWorkers>();
                    var projectWorkersPriv = new List<UserPrivileges>();
                    var ListOfTaskNotify = new List<Notification>();

                    //////////////////////////////////////// project workers ////////////////////////////////////////
                    projectWorkers.Add(new ProjectWorkers //// add current user as asenior project
                    {
                        ProjectId = Project.ProjectId,
                        //UserId = UserId,
                        UserId = Project.MangerId,
                        BranchId = BranchId,
                        WorkerType = 1,
                        IsDeleted = false,
                        AddDate = DateTime.Now,
                    });
                    WhichPart = "Part Phase(15)";

                    /////////////// priv
                    projectWorkersPriv.Add(new UserPrivileges
                    {
                        UserId = UserId,
                        PrivilegeId = 111026, // // finish proj
                        AddUser = UserId,
                        AddDate = DateTime.Now,
                    });
                    WhichPart = "Part Phase(16)";

                    if (Project.ProUserPrivileges != null && Project.ProUserPrivileges.Count > 0)
                    {
                        try
                        {
                            foreach (ProUserPrivileges priv in Project.ProUserPrivileges)
                            {
                                if (priv.UserPrivId == 0)
                                {

                                    projectWorkers.Add(new ProjectWorkers
                                    {
                                        UserId = priv.UserId,
                                        ProjectId = Project.ProjectId,
                                        BranchId = BranchId,
                                        WorkerType = 2,
                                        AddUser = UserId,
                                        AddDate = DateTime.Now,
                                    });
                                    //////////////// priv
                                    projectWorkersPriv.Add(new UserPrivileges
                                    {
                                        UserId = priv.UserId,
                                        PrivilegeId = 111026, // finish proj
                                        AddUser = UserId,
                                        AddDate = DateTime.Now,
                                    });


                                    priv.AddUser = UserId;
                                    priv.AddDate = DateTime.Now;
                                    _TaamerProContext.ProUserPrivileges.Add(priv);
                                    var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(priv.UserId ?? 0).Result;
                                    if (UserNotifPriv.Count() != 0)
                                    {
                                        if (UserNotifPriv.Contains(392))
                                        {
                                            try
                                            {
                                                ListOfTaskNotify.Add(new Notification
                                                {
                                                    ReceiveUserId = priv.UserId,
                                                    Name = "صلاحيات مشروع",
                                                    Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                                    HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                                    SendUserId = UserId,
                                                    Type = 1, // notification
                                                    Description = "  تم اضافتك علي مشروع رقم " + priv.Projectno + " للعميل " + Project.CustomerName,
                                                    AllUsers = false,
                                                    SendDate = DateTime.Now,
                                                    ProjectId = priv.ProjectID,
                                                    TaskId = 0,
                                                    AddUser = UserId,
                                                    AddDate = DateTime.Now,
                                                    IsHidden = false
                                                });
                                                _notificationService.sendmobilenotification(priv.UserId ?? 0, "صلاحيات مشروع", "  تم اضافتك علي مشروع رقم " + priv.Projectno + " للعميل " + Project.CustomerName);
                                            }
                                            catch (Exception)
                                            {

                                            }


                                        }

                                        if (UserNotifPriv.Contains(391))
                                        {
                                            try
                                            {
                                                var Desc = Project.CustomerName + " للعميل " + priv.Projectno + "  تم اضافتك علي مشروع رقم ";

                                                SendMailNoti(Project.ProjectId, Desc, "اضافة علي مشروع", BranchId, UserId, priv.UserId ?? 0);

                                            }
                                            catch (Exception)
                                            {

                                            }

                                        }

                                        if (UserNotifPriv.Contains(393))
                                        {
                                            try
                                            {
                                                var userObj = _UsersRepository.GetById(priv.UserId ?? 0);

                                                var NotStr = Project.CustomerName + " للعميل " + priv.Projectno + "  تم اضافتك علي مشروع رقم ";
                                                if (userObj.Mobile != null && userObj.Mobile != "")
                                                {
                                                    var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                                }
                                            }
                                            catch (Exception)
                                            {

                                            }

                                        }

                                    }
                                }
                            }
                            _TaamerProContext.Notification.AddRange(ListOfTaskNotify);
                        }
                        catch (Exception ex)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = "فشل في حفظ التعليق";
                            _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                        }
                    }
                    _TaamerProContext.ProjectWorkers.AddRange(projectWorkers); // add project users
                    _TaamerProContext.UserPrivileges.AddRange(projectWorkersPriv);
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }

                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة مشروع جديد" + "برقم" + Project.ProjectNo;
                _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.ProjectSaved, ReturnedStrExtra = VacMsg , ReturnedStr = Project.ProjectId.ToString() };
            }
            catch (Exception ex)
            {
                SendMail_ProjectSavedWrong(BranchId, WhichPart + " " + ex.Message + ">>>>" + ex.InnerException, false);
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ المشروع";
                _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.ProjectFailed, ReturnedStr = WhichPart + " " + ex.Message + ">>>>" + ex.InnerException };
            }
        }
        public GeneralMessage SaveProjectPhasesTasksNewPart2(Project Project, int UserId, int BranchId, string Url, string ImgUrl)
        {
            var projSubTypeSett = _TaamerProContext.SettingsNew.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId).ToList();

            //var projSubTypeSett = projSubTypeSett2;

            var projectPhasesTaskList = new List<ProjectPhasesTasks>();
            var projectPhasesTaskObj = new ProjectPhasesTasks();
            var ListOfTaskNotify = new List<Notification>();


            var codePrefix = "TSK#";
            //var prostartcode = _BranchesRepository.GetById(BranchId).TaskStartCode;
            //if (prostartcode != null && prostartcode != "")
            //{
            //    codePrefix = prostartcode;
            //}
            var Value = _ProjectPhasesTasksRepository.GenerateNextTaskNumber(BranchId, codePrefix, 0).Result;
            Value = Value - 1;

            var TempTime = DateTime.Now.ToString("h:mm", CultureInfo.InvariantCulture);
            var TempDate = Project.ProjectDate;
            DateTime d = new DateTime();
            DateTime oDate = DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            int? tempRemaining = 0;
            if (projSubTypeSett.Count() != 0)
            {
                foreach (var item in projSubTypeSett)
                {
                    string? NewValue = null;
                    if (item.Type==3)
                    {
                        Value = Value + 1;
                        NewValue = string.Format("{0:000000}", Value);
                        if (codePrefix != "")
                        {
                            NewValue = codePrefix + NewValue;
                        }
                    }
                    else
                    {
                        NewValue = null;
                    }


                    var DateDiff = ((item.EndDate ?? new DateTime()) - (item.StartDate ?? new DateTime()));
                    tempRemaining = Convert.ToInt32(DateDiff.TotalMinutes);

                    if (item.TimeType == 1)       //hour
                    {
                        TempDate = Project.ProjectDate;
                        TempTime = DateTime.Now.AddHours(Convert.ToDouble(item.TimeMinutes)).ToString("h:mm");
                        //tempRemaining = item.TimeMinutes * 60;
                    }
                    else        //day
                    {
                        d = oDate.AddDays(Convert.ToDouble(item.TimeMinutes));
                        TempDate = d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                        TempTime = DateTime.Now.ToString("h:mm", CultureInfo.InvariantCulture);
                        //tempRemaining = item.TimeMinutes * 60 * 24;
                    }
                    var dateonly_A = TempDate;//mfroood hna l end
                    var timeonly_A = DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                    DateTime dt_A = DateTime.ParseExact(dateonly_A + " " + timeonly_A, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);



                    int? ParentV = null;
                    if (item.ParentId == null)
                    {
                        ParentV = null;
                    }
                    else
                    {
                        //ParentV = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId && s.ProjectId == Project.ProjectId && s.SettingId == item.ParentId).FirstOrDefault()?.PhaseTaskId ?? null;
                        ParentV = projectPhasesTaskList.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId && s.ProjectId == Project.ProjectId && s.SettingId == item.ParentId).FirstOrDefault()?.PhaseTaskId ?? null;

                    }

                    projectPhasesTaskObj = new ProjectPhasesTasks
                    {
                        DescriptionAr = item.DescriptionAr,
                        DescriptionEn = item.DescriptionEn,
                        ProjSubTypeId = item.ProjSubTypeId,
                        TaskNo= NewValue,
                        TaskNoType= 1,
                        Type = item.Type,
                        TimeMinutes = item.TimeMinutes,
                        Cost = item.Cost,
                        //Remaining = item.TimeMinutes,
                        Remaining = tempRemaining,
                        Status = (item.Type == 3) ? item.IsTemp == true ? 3 : 1 : 0,
                        StopCount = 0,
                        TimeType = item.TimeType,
                        IsUrgent = item.IsUrgent,
                        TaskType = item.TaskType,
                        StartDate = null,
                        EndDate = null,
                        //ExcpectedStartDate = Project.ProjectDate,
                        //ExcpectedEndDate = TempDate,
                        ExcpectedStartDate = null,
                        ExcpectedEndDate = null,
                        ParentId = ParentV,
                        UserId = item.UserId,
                        SettingId = item.SettingId,
                        PhasePriority = item.Priority,
                        ParentSettingId = ParentV,
                        Notes = item.Notes,
                        ProjectId = Project.ProjectId,
                        BranchId = Project.BranchId,   //edit to set task with projectbranch
                        AddUser = UserId,
                        AddDate = DateTime.Now,
                        PlusTime = false,
                        IsConverted = 0,
                        IsMerig = item.IsMerig,
                        EndTime = TempTime,
                        TaskFullTime = dt_A.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture),
                        indentation = item.indentation,
                        taskindex = item.taskindex,
                        ReasonsId = item.ReasonsId,
                        IsTemp = item.IsTemp,
                        Managerapproval = item.Managerapproval,
                        StartDateNew = item.StartDate,
                        EndDateNew = item.EndDate,
                        Totalhourstask = item.Totalhourstask,
                        Totaltaskcost = item.Totaltaskcost,

                    };
                    _TaamerProContext.ProjectPhasesTasks.Add(projectPhasesTaskObj);
                    _TaamerProContext.SaveChanges();  // commit 
                    projectPhasesTaskList.Add(projectPhasesTaskObj);

                }
                //_TaamerProContext.ProjectPhasesTasks.AddRange(projectPhasesTaskList); ///////// project tasks pahses
                //_TaamerProContext.SaveChanges();  // commit 

            }

            // save pro dependency  from setting dependency
            var projSubTypeDependencySett = _TaamerProContext.DependencySettingsNew.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId).ToList();

            var projectPhasesCount = _ProjectPhasesTasksRepository.GetAllTasksByProjectIdWithoutBranch(Project.ProjectId, BranchId).Result.Count();
            if (projectPhasesCount != 0)
            {
                foreach (var item in projSubTypeDependencySett)
                {
                    var ProDependency = new TasksDependency();
                    ProDependency.ProjSubTypeId = item.ProjSubTypeId;
                    var Pre = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == item.ProjSubTypeId && s.SettingId == item.PredecessorId && s.ProjectId == Project.ProjectId && s.Type == 3).FirstOrDefault();
                    var PreSetting = 0;
                    if (Pre != null)
                    {
                        PreSetting = Pre.PhaseTaskId;
                    }
                    ProDependency.PredecessorId = PreSetting;
                    var Succ = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == item.ProjSubTypeId && s.SettingId == item.SuccessorId && s.ProjectId == Project.ProjectId && s.Type == 3).FirstOrDefault();
                    var SuccSetting = 0;
                    if (Succ != null)
                    {
                        SuccSetting = Succ.PhaseTaskId;
                    }
                    ProDependency.SuccessorId = SuccSetting;
                    ProDependency.Type = item.Type;
                    ProDependency.BranchId = BranchId;
                    ProDependency.AddUser = UserId;
                    ProDependency.ProjectId = Project.ProjectId;
                    ProDependency.AddDate = DateTime.Now;
                    _TaamerProContext.TasksDependency.Add(ProDependency);
                }
            }

            foreach (var task in projectPhasesTaskList.Where(s => s.Type == 3))  //add tasks notifications
            {

                var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(task.UserId ?? 0).Result;
                if (UserNotifPriv.Count() != 0)
                {
                    if (UserNotifPriv.Contains(352))
                    {
                        var branch = _BranchesRepository.GetById(BranchId);
                        try
                        {
                            ListOfTaskNotify.Add(new Notification
                            {
                                ReceiveUserId = task.UserId,
                                Name = Resources.General_Newtasks,
                                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                SendUserId = 1,
                                Type = 1, // notification
                                Description = " لديك مهمه جديدة : " + task.DescriptionAr + " رقم" + task.TaskNo + " علي مشروع رقم " + Project.ProjectNo + " للعميل " + Project.CustomerName + " " + "  فرع  " + branch.NameAr + "",
                                AllUsers = false,
                                SendDate = DateTime.Now,
                                ProjectId = task.ProjectId,
                                TaskId = task.PhaseTaskId,
                                AddUser = UserId,
                                AddDate = DateTime.Now,
                                IsHidden = false
                            });
                            _notificationService.sendmobilenotification(task.UserId ?? 0, Resources.General_Newtasks, "");// " لديك مهمه جديدة : " + task.DescriptionAr + " علي مشروع رقم " + Project.ProjectNo + " للعميل " + customer.CustomerNameAr + " " + "  فرع  " + branch.NameAr + "");

                        }
                        catch (Exception ex)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote4 = "فشل في ارسال اشعار مهمة";
                            _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                        }


                    }

                    if (UserNotifPriv.Contains(353))
                    {
                        try
                        {
                            var userObj = _UsersRepository.GetById(task.UserId ?? 0);

                            var NotStr = Project.CustomerName + " للعميل  " + Project.ProjectNo + " علي مشروع رقم " + task.TaskNo +" رقم "+ task.DescriptionAr + " لديك مهمه جديدة  ";
                            if (userObj.Mobile != null && userObj.Mobile != "")
                            {
                                var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                            }
                        }
                        catch (Exception ex)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote4 = "فشل في ارسال SMS مهمة";
                            _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                            //-----------------------------------------------------------------------------------------------------------------

                        }

                    }
                }

            }

            _TaamerProContext.Notification.AddRange(ListOfTaskNotify);   /// add notifications

            _TaamerProContext.SaveChanges();
            SetExpectedDateNew(Project.ProjectId, Project.ProjectDate, true);

            return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.ProjectSaved, ReturnedStr = Project.ProjectId.ToString() };
        }
        public GeneralMessage SaveProjectPhasesTasksNewPart3(Project Project, int UserId, int BranchId, string Url, string ImgUrl)
        {
            try
            {
                if (Project.ProjectRequirementsGoals.Count() > 0)
                {

                    foreach (var item in Project.ProjectRequirementsGoals.ToList())
                    {
                        item.RequirementGoalId = 0;
                        item.AddDate = DateTime.Now;
                        item.AddUser = UserId;
                        item.ProjectId = Project.ProjectId;
                        _TaamerProContext.ProjectRequirementsGoals.Add(item);
                        _TaamerProContext.SaveChanges();

                    }
                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate7 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote7 = "فشل في حفظ أهداف المشروع" + Project.ProjectNo; ;
                _SystemAction.SaveAction("SaveProject", "SaveProjectPhasesTasksNewPart3", 1, Resources.General_SavedFailed, "", "", ActionDate7, UserId, BranchId, ActionNote7, 0);
                //-----------------------------------------------------------------------------------------------------------------
            }

            if (Project.OffersPricesId != null)
            {
                try
                {
                    var offer = _offersPricesRepository.GetById(Project.OffersPricesId ?? 0);
                    offer.ProjectId = Project.ProjectId;
                }
                catch (Exception ex)
                {

                }
            }

            try
            {

                var newcostCenter = new CostCenters();
                var CostCenterByid = _CostCenterRepository.GetById(Project.CostCenterId ?? 0);
                newcostCenter.ParentId = Project.CostCenterId;
                newcostCenter.BranchId = CostCenterByid.BranchId;
                newcostCenter.Code = Project.ProjectNo;
                newcostCenter.NameAr = Project.CustomerName;
                newcostCenter.NameEn = Project.CustomerName;
                newcostCenter.AddDate = DateTime.Now;
                newcostCenter.AddUser = UserId;
                newcostCenter.CustomerId = Project.CustomerId;

                newcostCenter.ProjId = Project.ProjectId;
                _TaamerProContext.CostCenters.Add(newcostCenter);
            }
            catch (Exception ex)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate8 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote8 = "فشل في حفظ مركز تكلفة للمشروع" + Project.ProjectNo;
                _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate8, UserId, BranchId, ActionNote8, 0);
                //-----------------------------------------------------------------------------------------------------------------
            }

            if (Project.TransactionTypeId == 1)
            {
                var branch = _BranchesRepository.GetById(BranchId);

                Project.MotionProject = 1;
                Project.MotionProjectDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                Project.MotionProjectNote = "أضافة فاتورة علي مشروع";

                var ListOfPrivNotify = new List<Notification>();
                var UserNotifPriv = _userNotificationPrivilegesService.GetUsersByPrivilegesIds(3252).Result;
                if (UserNotifPriv.Count() != 0)
                {
                    //_userPrivilegesRepository.GetMatching(s => s.IsDeleted == false && s.PrivilegeId == 131001).Where(w => w.Users.IsDeleted == false)
                    foreach (var userCounter in UserNotifPriv)
                    {
                        try
                        {
                            ListOfPrivNotify.Add(new Notification
                            {
                                ReceiveUserId = userCounter.UserId,
                                Name = Resources.MNAcc_Invoice,
                                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                SendUserId = UserId,
                                Type = 1, // notification
                                Description = " يوجد فاتورة جديدة علي مشروع رقم  : " + Project.ProjectNo + " للعميل " + Project.CustomerName + " " + " فرع  " + branch.NameAr + "",
                                AllUsers = false,
                                SendDate = DateTime.Now,
                                ProjectId = 0,
                                TaskId = 0,
                                AddUser = UserId,
                                AddDate = DateTime.Now,
                                IsHidden = false
                            });
                            _notificationService.sendmobilenotification(userCounter.UserId ?? 0, Resources.MNAcc_Invoice, " يوجد فاتورة جديدة علي مشروع رقم  : " + Project.ProjectNo + " للعميل " + Project.CustomerName + " " + " فرع  " + branch.NameAr + "");
                        }
                        catch (Exception ex)
                        {

                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote4 = "فشل في ارسال اشعار لمن لدية صلاحية فاتورة";
                            _SystemAction.SaveAction("SaveProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                        }

                    }

                    _TaamerProContext.Notification.AddRange(ListOfPrivNotify);

                }

                var UserNotifPriv_email = _userNotificationPrivilegesService.GetUsersByPrivilegesIds(3251).Result;
                if (UserNotifPriv_email.Count() != 0)
                {
                    foreach (var userCounter in UserNotifPriv_email)
                    {
                        try
                        {
                            var Desc = " المستخدم " + userCounter.FullName + " تم اصدار فاتورة لمشروع رقم " + Project.ProjectNo + " للعميل " + Project.CustomerName + " فرع " + branch.NameAr;

                            SendMailNoti(0, Desc, "اصدار فاتورة علي مشروع", BranchId, UserId, userCounter.UserId ?? 0);


                            var htmlBody = "";


                            htmlBody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                             
                                                <table style=' border: 1px solid black; border-collapse: collapse;' align='center'>
                                                  <tr>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>رقم المشروع</th>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>اسم العميل</th>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>الفرع </th>
                                                  </tr>
                                                    <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + Project.ProjectNo + @"</td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + Project.CustomerName + @"</td>
                                              <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + branch.NameAr + @"</td>
                                                    </tr>
                                                </table>
                                            </body>
                                            </html>";
                            //SendMailNoti(projectId, Desc, "ايقاف مشروع", BranchId, UserId, proj.MangerId ?? 0);
                            SendMail_ProjectStamp(BranchId, UserId, userCounter.UserId ?? 0, "اصدار فاتورة علي مشروع", htmlBody, Url, ImgUrl, 6, true);
                        }
                        catch (Exception ex)
                        {

                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate5 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote5 = "فشل في ارسال ميل لمن لدية صلاحية فاتورة";
                            _SystemAction.SaveAction("SaveProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate5, UserId, BranchId, ActionNote5, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                        }


                    }

                }

                var UserNotifPriv_Mobile = _userNotificationPrivilegesService.GetUsersByPrivilegesIds(3253).Result;
                if (UserNotifPriv_Mobile.Count() != 0)
                {
                    foreach (var userCounter in UserNotifPriv_Mobile)
                    {
                        try
                        {
                            var userObj = _UsersRepository.GetById(userCounter.UserId ?? 0);
                            var NotStr = " المستخدم " + userCounter.FullName + " تم اصدار فاتورة لمشروع رقم " + Project.ProjectNo + " للعميل " + Project.CustomerName + " فرع " + branch.NameAr;
                            if (userObj.Mobile != null && userObj.Mobile != "")
                            {
                                var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                            }
                        }
                        catch (Exception ex)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate6 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote6 = "فشل في ارسال SMS لمن لدية صلاحية فاتورة";
                            _SystemAction.SaveAction("SaveProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate6, UserId, BranchId, ActionNote6, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                        }

                    }
                }
            }
            _TaamerProContext.SaveChanges();
            return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.ProjectSaved, ReturnedStr = Project.ProjectId.ToString() };
        }

        public bool SendMail_ProjectSavedWrong(int BranchId, string textBody, bool IsBodyHtml = false)
        {
            try
            {
                var Organization = _BranchesRepository.GetById(BranchId).OrganizationId;
                var Org = _OrganizationsRepository.GetById(Organization);

                var mail = new MailMessage();
                var email = "no-reply@tameercloud.com";
                var password = "gwk2!8Y9n@";
                var loginInfo = new NetworkCredential(email, password);
                mail.From = new MailAddress(email, "TAMEER-CLOUD-SYSTEM");

                mail.To.Add(new MailAddress("mohammeddawoud66@gmail.com"));
                mail.Subject = "فشل حفظ مشروع " + " " + Org.NameAr;

                mail.Body = textBody;
                mail.IsBodyHtml = IsBodyHtml;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var smtpClient = new SmtpClient("smtp.hostinger.com");
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = 587;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public void SetExpectedDate(int projid, string StartProjectDate,bool NewSetting)
        {
            try
            {
                var phaseTasks = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == projid && s.Type == 3 && s.IsMerig == -1).ToList();
                var tasksDependency = _TasksDependencyRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == projid);

                int GlobalFirsTaskID = 0;
                var TempDate = StartProjectDate;
                DateTime d = new DateTime();
                DateTime oDate = DateTime.ParseExact(StartProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var FirstNode = phaseTasks.Where(p => tasksDependency.All(p2 => p2.SuccessorId != p.PhaseTaskId));

                if (FirstNode.Count() > 0 && FirstNode!=null)
                {
                    if (FirstNode?.FirstOrDefault()?.TimeType == 1) //hour
                    {
                        d = oDate.AddHours(Convert.ToDouble(FirstNode?.FirstOrDefault()?.TimeMinutes));
                    }
                    else //day
                    {
                        d = oDate.AddDays(Convert.ToDouble(FirstNode?.FirstOrDefault()?.TimeMinutes));
                    }

                    TempDate = d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    FirstNode.FirstOrDefault().ExcpectedStartDate = StartProjectDate;
                    FirstNode.FirstOrDefault().ExcpectedEndDate = TempDate;
                    GlobalFirsTaskID = FirstNode.FirstOrDefault().PhaseTaskId;
                }
                bool tempcheck = false;
                int tempId = 0;

                int Mins = 0;
                int Mins_W = 0;

                foreach (var taskitem in phaseTasks)
                {
                    if(NewSetting==true)
                    {
                        if (taskitem.TimeType == 1)
                        {
                            taskitem.TaskTimeFrom = (taskitem.StartDateNew?? DateTime.Now).ToString("hh:mm tt", CultureInfo.InvariantCulture);
                            taskitem.TaskTimeTo = (taskitem.EndDateNew ?? DateTime.Now).ToString("hh:mm tt", CultureInfo.InvariantCulture);
                        }
                    }
                    else
                    {
                        if (taskitem.TimeType == 1)
                        {
                            taskitem.TaskTimeFrom = DateTime.Now.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                            taskitem.TaskTimeTo = DateTime.Now.AddHours(Convert.ToDouble(taskitem.TimeMinutes ?? 0)).ToString("hh:mm tt", CultureInfo.InvariantCulture);
                        }
                    }
                    

                    if (taskitem.PhaseTaskId != GlobalFirsTaskID)
                    {
                        int TaskIdSelect = taskitem.PhaseTaskId;
                        //int TaskIdsucc = tasksDependency.Where(s=>s.SuccessorId == taskitem.PhaseTaskId).FirstOrDefault().SuccessorId??0;
                        int TaskIdpre = tasksDependency.Where(s => s.SuccessorId == taskitem.PhaseTaskId)?.FirstOrDefault()?.PredecessorId ?? 0;
                        int TaskIdSelect_Remaining = taskitem.Remaining ?? 0;
                        Mins = TaskIdSelect_Remaining;
                        Mins_W = 0;
                        var Value = phaseTasks.Where(s => s.PhaseTaskId == TaskIdpre).FirstOrDefault();
                        var Pre_TaskIdSelect = 0;
                        if (Value!=null)
                        {
                            Pre_TaskIdSelect = Value.PhaseTaskId;
                        }
                        //var Pre_TaskIdSelect = phaseTasks.Where(s => s.PhaseTaskId == TaskIdpre).FirstOrDefault().PhaseTaskId;
                        if (Pre_TaskIdSelect != GlobalFirsTaskID)
                        {
                            //int Pre_TaskIdsucc = tasksDependency.Where(s => s.SuccessorId == Pre_TaskIdSelect).FirstOrDefault().SuccessorId ?? 0;
                            int Pre_TaskIdpre = tasksDependency.Where(s => s.SuccessorId == Pre_TaskIdSelect)?.FirstOrDefault()?.PredecessorId ?? 0;
                            int Pre_TaskIdSelect_Remaining = phaseTasks.Where(s => s.PhaseTaskId == Pre_TaskIdpre)?.FirstOrDefault()?.Remaining ?? 0;

                            for (int i = 0; i <= 1000; i++)
                            {



                                if (Pre_TaskIdSelect == GlobalFirsTaskID)
                                {
                                    int Pre_TaskIdSelect_RemainingFirsT = phaseTasks.Where(s => s.PhaseTaskId == GlobalFirsTaskID)?.FirstOrDefault()?.Remaining ?? 0;
                                    Mins += Pre_TaskIdSelect_RemainingFirsT;
                                    Mins_W += Pre_TaskIdSelect_RemainingFirsT;


                                    break;
                                }
                                else
                                {
                                    int Pre_TaskIdpre_prev = tasksDependency.Where(s => s.SuccessorId == Pre_TaskIdSelect)?.FirstOrDefault()?.PredecessorId ?? 0;
                                    int Pre_TaskIdSelect_RemainingR = phaseTasks.Where(s => s.PhaseTaskId == Pre_TaskIdSelect)?.FirstOrDefault()?.Remaining ?? 0;

                                    Mins += Pre_TaskIdSelect_RemainingR;
                                    Mins_W += Pre_TaskIdSelect_RemainingR;

                                    Pre_TaskIdSelect = Pre_TaskIdpre_prev;

                                }


                            }

                        }
                        else
                        {
                            int Pre_TaskIdSelect_RemainingFirsTT = phaseTasks.Where(s => s.PhaseTaskId == GlobalFirsTaskID)?.FirstOrDefault()?.Remaining ?? 0;
                            Mins += Pre_TaskIdSelect_RemainingFirsTT;
                            Mins_W += Pre_TaskIdSelect_RemainingFirsTT;

                        }
                        var newdate = oDate.AddMinutes(Convert.ToDouble(Mins));
                        var newdateStart = oDate.AddMinutes(Convert.ToDouble(Mins_W));

                        var TempDatenewdate = newdate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                        var TempDatenewdateStart = newdateStart.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                        taskitem.ExcpectedStartDate = TempDatenewdateStart;
                        taskitem.ExcpectedEndDate = TempDatenewdate;

                    }

                }


                _TaamerProContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var ex2 = ex.Message;
            }

        }

        public void SetExpectedDateNew(int projid, string StartProjectDate, bool NewSetting)
        {
            try
            {
                var phaseTasks = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == projid && s.Type == 3 && s.IsMerig == -1).ToList();
                var tasksDependency = _TasksDependencyRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == projid);

                int GlobalFirsTaskID = 0;
                var TempDate = StartProjectDate;
                DateTime d = new DateTime();
                DateTime dend = new DateTime();

                DateTime oDate = DateTime.ParseExact(StartProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime AccProjectStart = new DateTime(oDate.Year, oDate.Month, oDate.Day, 0, 0, 0);

                var FirstNode = phaseTasks.Where(p => tasksDependency.All(p2 => p2.SuccessorId != p.PhaseTaskId)).ToList();

                if (FirstNode.Count() > 0 && FirstNode != null)
                {
                    
                    //var StartDateDiff = ((AccProjectStart)-(FirstNode?.FirstOrDefault()?.StartDateNew ?? new DateTime()));
                    var DateDiff = ((FirstNode?.FirstOrDefault()?.EndDateNew ?? new DateTime()) - (FirstNode?.FirstOrDefault()?.StartDateNew ?? new DateTime()));
                    DateTime StartD = FirstNode?.FirstOrDefault()?.StartDateNew ?? new DateTime();

                    //d = AccProjectStart.Add(StartDateDiff);
                    AccProjectStart = new DateTime(oDate.Year, oDate.Month, oDate.Day, StartD.Hour, StartD.Minute, StartD.Second);
                    dend = AccProjectStart.Add(DateDiff);

                    //if (FirstNode?.FirstOrDefault()?.TimeType == 1) //hour
                    //{
                    //    d = oDate.AddHours(Convert.ToDouble(FirstNode?.FirstOrDefault()?.TimeMinutes));
                    //}
                    //else //day
                    //{
                    //    d = oDate.AddDays(Convert.ToDouble(FirstNode?.FirstOrDefault()?.TimeMinutes));
                    //}

                    TempDate = dend.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    FirstNode.FirstOrDefault().ExcpectedStartDate = StartProjectDate;
                    FirstNode.FirstOrDefault().ExcpectedEndDate = TempDate;
                    FirstNode.FirstOrDefault().TaskTimeFrom = (AccProjectStart).ToString("hh:mm tt", CultureInfo.InvariantCulture);
                    FirstNode.FirstOrDefault().TaskTimeTo = (dend).ToString("hh:mm tt", CultureInfo.InvariantCulture);
                    FirstNode.FirstOrDefault().StartDateNew = AccProjectStart;
                    FirstNode.FirstOrDefault().EndDateNew = dend;
                    GlobalFirsTaskID = FirstNode.FirstOrDefault().PhaseTaskId;
                }
                bool tempcheck = false;
                int tempId = 0;

                int Mins = 0;
                int Mins_W = 0;

                foreach (var taskitem in phaseTasks)
                {



                    if (taskitem.PhaseTaskId != GlobalFirsTaskID)
                    {
                        int TaskIdSelect = taskitem.PhaseTaskId;
                        //int TaskIdsucc = tasksDependency.Where(s=>s.SuccessorId == taskitem.PhaseTaskId).FirstOrDefault().SuccessorId??0;
                        int TaskIdpre = tasksDependency.Where(s => s.SuccessorId == taskitem.PhaseTaskId)?.FirstOrDefault()?.PredecessorId ?? 0;
                        int TaskIdSelect_Remaining = taskitem.Remaining ?? 0;
                        Mins = TaskIdSelect_Remaining;
                        Mins_W = 0;
                        var Value = phaseTasks.Where(s => s.PhaseTaskId == TaskIdpre).FirstOrDefault();
                        var Pre_TaskIdSelect = 0;
                        if (Value != null)
                        {
                            Pre_TaskIdSelect = Value.PhaseTaskId;
                        }
                        if (Pre_TaskIdSelect != GlobalFirsTaskID)
                        {
                            int Pre_TaskIdpre = tasksDependency.Where(s => s.SuccessorId == Pre_TaskIdSelect)?.FirstOrDefault()?.PredecessorId ?? 0;
                            //int Pre_TaskIdSelect_Remaining = phaseTasks.Where(s => s.PhaseTaskId == Pre_TaskIdpre)?.FirstOrDefault()?.Remaining ?? 0;

                            for (int i = 0; i <= 1000; i++)
                            {



                                if (Pre_TaskIdSelect == GlobalFirsTaskID)
                                {
                                    int Pre_TaskIdSelect_RemainingFirsT = phaseTasks.Where(s => s.PhaseTaskId == GlobalFirsTaskID)?.FirstOrDefault()?.Remaining ?? 0;
                                    Mins += Pre_TaskIdSelect_RemainingFirsT;
                                    Mins_W += Pre_TaskIdSelect_RemainingFirsT;


                                    break;
                                }
                                else
                                {
                                    int Pre_TaskIdpre_prev = tasksDependency.Where(s => s.SuccessorId == Pre_TaskIdSelect)?.FirstOrDefault()?.PredecessorId ?? 0;
                                    int Pre_TaskIdSelect_RemainingR = phaseTasks.Where(s => s.PhaseTaskId == Pre_TaskIdSelect)?.FirstOrDefault()?.Remaining ?? 0;

                                    Mins += Pre_TaskIdSelect_RemainingR;
                                    Mins_W += Pre_TaskIdSelect_RemainingR;

                                    Pre_TaskIdSelect = Pre_TaskIdpre_prev;

                                }


                            }

                        }
                        else
                        {
                            int Pre_TaskIdSelect_RemainingFirsTT = phaseTasks.Where(s => s.PhaseTaskId == GlobalFirsTaskID)?.FirstOrDefault()?.Remaining ?? 0;
                            Mins += Pre_TaskIdSelect_RemainingFirsTT;
                            Mins_W += Pre_TaskIdSelect_RemainingFirsTT;

                        }
                        var newdate = AccProjectStart.AddMinutes(Convert.ToDouble(Mins));
                        var newdateStart = AccProjectStart.AddMinutes(Convert.ToDouble(Mins_W));

                        var TempDatenewdate = newdate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                        var TempDatenewdateStart = newdateStart.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                        taskitem.ExcpectedStartDate = TempDatenewdateStart;
                        taskitem.ExcpectedEndDate = TempDatenewdate;

                        taskitem.TaskTimeFrom = (newdateStart).ToString("hh:mm tt", CultureInfo.InvariantCulture);
                        taskitem.TaskTimeTo = (newdate).ToString("hh:mm tt", CultureInfo.InvariantCulture);
                        taskitem.StartDateNew = newdateStart;
                        taskitem.EndDateNew = newdate;

                    }

                }


                _TaamerProContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var ex2 = ex.Message;
            }

        }

        public GeneralMessage UpdateProjectPhasesTasks(Project Project, int UserId, int BranchId)
        {


            try
            {
                var projectUpdated = _ProjectRepository.GetById(Project.ProjectId);


                var totaldays = 0.0;
                DateTime resultEnd = DateTime.ParseExact(Project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime resultStart = DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                totaldays = (resultEnd - resultStart).TotalDays + 1;
                if (projectUpdated.ProjectDate != Project.ProjectDate || projectUpdated.ProjectExpireDate != Project.ProjectExpireDate)
                {
                    projectUpdated.IsNotSent = false;

                }
                DateTime resultNow = DateTime.Now;

                DateTime resultEndSelect = DateTime.ParseExact(projectUpdated.FirstProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);


                if (resultNow < resultEndSelect)
                {
                    projectUpdated.FirstProjectExpireDate = Project.ProjectExpireDate;
                    projectUpdated.FirstProjectDate = Project.ProjectDate;

                }

                projectUpdated.NoOfDays = Convert.ToInt32(totaldays);
                projectUpdated.ProjectDate = Project.ProjectDate;
                projectUpdated.ProjectHijriDate = Project.ProjectHijriDate;
                projectUpdated.ProjectExpireDate = Project.ProjectExpireDate;
                projectUpdated.ProjectExpireHijriDate = Project.ProjectExpireHijriDate;
                projectUpdated.BuildingType = Project.BuildingType;


                if (projectUpdated.MangerId != Project.MangerId)
                {
                    var ProjectWorkersUpdated = _TaamerProContext.ProjectWorkers.Where(s => s.ProjectId == Project.ProjectId && s.WorkerType == 1).FirstOrDefault();
                    if (ProjectWorkersUpdated != null)
                    {
                        ProjectWorkersUpdated.UserId = Project.MangerId;
                    }

                }
                if (projectUpdated.CostCenterId != Project.CostCenterId)
                {
                    if (Project.CostCenterId != null)
                    {
                        var CostCenterProject = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.ProjId == Project.ProjectId).FirstOrDefault();
                        var CostCenterParent = _CostCenterRepository.GetById((int)Project.CostCenterId);

                        CostCenterProject.BranchId = CostCenterParent.BranchId;
                        CostCenterProject.ParentId = CostCenterParent.CostCenterId;
                        CostCenterProject.Level = (CostCenterParent.Level ?? 0) + 1;
                    }

                }
                if (projectUpdated.CustomerId != Project.CustomerId)
                {
                    if (Project.CustomerId != null)
                    {
                        var CostCenterProjectNew = _CostCenterRepository.GetMatching(s => s.IsDeleted == false && s.ProjId == Project.ProjectId).FirstOrDefault();
                        var CustomerNew = _CustomerRepository.GetById((int)Project.CustomerId);

                        CostCenterProjectNew.NameAr = CustomerNew.CustomerNameAr;
                        CostCenterProjectNew.NameEn = CustomerNew.CustomerNameEn;

                    }

                }


                if (Project.ProjectRequirementsGoals.Count()>0)
                {
                    _TaamerProContext.ProjectRequirementsGoals.RemoveRange(projectUpdated.ProjectRequirementsGoals.ToList());

                    foreach (var item in Project.ProjectRequirementsGoals.ToList())
                    {
                        item.AddDate = DateTime.Now;
                        item.AddUser = UserId;

                        item.ProjectId = Project.ProjectId;
                        _TaamerProContext.ProjectRequirementsGoals.Add(item);
                    }

                }

                /////add projectno to offerprice
                if (Project.OffersPricesId != null)
                {
                    try
                    {

                        var offer = _offersPricesRepository.GetById((int)Project.OffersPricesId);

                        var oldoffer = _offersPricesRepository.GetMatching(x => x.IsDeleted == false && x.ProjectId == Project.ProjectId).FirstOrDefault();                            
                        if(oldoffer!=null)
                        {
                            if (oldoffer.OffersPricesId != offer.OffersPricesId)
                            {
                                oldoffer.ProjectId = null;
                            }
                        }

                        offer.ProjectId = Project.ProjectId;
                    }
                    catch (Exception ex)
                    {

                    }
                }

                projectUpdated.CustomerId = Project.CustomerId;

                projectUpdated.CostCenterId = Project.CostCenterId;
                projectUpdated.MangerId = Project.MangerId;
                projectUpdated.CityId = Project.CityId;
                projectUpdated.ProjectDescription = Project.ProjectDescription;
                projectUpdated.ProjectTypeId = Project.ProjectTypeId;
                projectUpdated.SubProjectTypeId = Project.SubProjectTypeId;

                projectUpdated.BranchId = Project.BranchId;
                projectUpdated.OffersPricesId = Project.OffersPricesId;
                projectUpdated.DepartmentId = Project.DepartmentId;

                //ana 3ayz a5le mshroo3 bseer gded
                var ProjectIds = _TasksDependencyRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == Project.ProjectId).ToList();
                var Oldphases = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == Project.ProjectId).ToList();

                if (ProjectIds.Count() > 0)
                {
                    if (ProjectIds.FirstOrDefault().ProjSubTypeId != Project.SubProjectTypeId)
                    {
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكنك تعديل مشروع عليه سير بسير جديد" };

                    }
                    else
                    {
                        if ((projectUpdated.NewSetting??false) != Project.NewSetting)
                        {
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكنك تعديل مشروع عليه سير بسير جديد" };
                        }
                    }

                }
                projectUpdated.NewSetting = Project.NewSetting;


                bool checkMakeSetting = false;

                if (ProjectIds.Count() > 0) //mshro3 da kan bseeer
                {

                    if (ProjectIds.FirstOrDefault().ProjSubTypeId != Project.SubProjectTypeId)   //hwa hwaaaah nfs l seeer wla la l kan mwgoood
                    {

                        if (Project.SubProjectTypeId != null)
                        {
                            var projSubTypeSett2 = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId);
                            if (projSubTypeSett2.Count() != 0)
                            {
                                foreach (var item in projSubTypeSett2)
                                {
                                    if (item.UserId != null)
                                    {
                                        var UserCheck = _TaamerProContext.Users.Where(s => s.UserId == item.UserId).FirstOrDefault();

                                        if (UserCheck.IsDeleted == true)
                                        {
                                            //-----------------------------------------------------------------------------------------------------------------
                                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                            string ActionNote2 = "فشل في التعديل";
                                           _SystemAction.SaveAction("UpdateProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.failed_modify_check_users, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                            //-----------------------------------------------------------------------------------------------------------------
                                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.failed_modify_check_users };
                                        }
                                        //var UserVacation = _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && s.UserId == item.UserId && s.VacationStatus != 4 && (s.BackToWorkDate == null || s.BackToWorkDate.Equals(""))).Count();
                                        //if (UserVacation != 0)
                                        //{
                                        //    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Proj_SaveFailedUserVacationProSetting };
                                        //}
                                    }
                                }
                            }
                        }



                        if (Oldphases.Count() > 0)
                        {
                            foreach (var item in Oldphases)
                            {
                                if (item.Type == 3 && (item.Status == 2 || item.Status == 3))
                                {
                                    checkMakeSetting = false;
                                    //-----------------------------------------------------------------------------------------------------------------
                                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    string ActionNote2 = Resources.SaveFailedRunningStoppedTasksThatMustBeCompletedFirst;
                                   _SystemAction.SaveAction("UpdateProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                    //-----------------------------------------------------------------------------------------------------------------
                                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.SaveFailedRunningStoppedTasksThatMustBeCompletedFirst, ReturnedStr = "-1" };


                                }
                            }

                            _TaamerProContext.ProjectPhasesTasks.RemoveRange(Oldphases);
                            _TaamerProContext.TasksDependency.RemoveRange(ProjectIds);
                            checkMakeSetting = true;
                        }
                        else
                        {
                            checkMakeSetting = true;
                        }

                    }
                    else
                    {
                        checkMakeSetting = false;
                    }

                }
                else
                {
                    if (Oldphases.Count() > 0)
                    {
                        if (ProjectIds.Count() == 0)
                        {
                            _TaamerProContext.ProjectPhasesTasks.RemoveRange(Oldphases); //delete without main and without subtypemain
                            checkMakeSetting = true;
                        }
                        else
                        {
                            checkMakeSetting = false;

                        }
                    }
                    else
                    {
                        checkMakeSetting = true;
                    }
                }



                if (checkMakeSetting == true)
                {
                    //////////////////////////////////////////// project tasks and phases ///////////////////////////////////////////////////////////////////////
                    var projSubTypeSett = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId).ToList();
                    var projectPhasesTaskList = new List<ProjectPhasesTasks>();
                    var projectPhasesTaskObj = new ProjectPhasesTasks();
                    var projectWorkers = new List<ProjectWorkers>();
                    var projectWorkersPriv = new List<UserPrivileges>();
                    var ListOfTaskNotify = new List<Notification>();

                    var codePrefix = "TSK#";
                    //var prostartcode = _BranchesRepository.GetById(BranchId).TaskStartCode;
                    //if (prostartcode != null && prostartcode != "")
                    //{
                    //    codePrefix = prostartcode;
                    //}
                    var Value = _ProjectPhasesTasksRepository.GenerateNextTaskNumber(BranchId, codePrefix, 0).Result;
                    Value = Value - 1;

                    var TempTime = DateTime.Now.ToString("h:mm", CultureInfo.InvariantCulture);
                    var TempDate = Project.ProjectDate;
                    DateTime d = new DateTime();
                    DateTime oDate = DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    int? tempRemaining = 0;
                    if (projSubTypeSett.Count() != 0)
                    {
                        foreach (var item in projSubTypeSett)
                        {

                            string? NewValue = null;
                            if (item.Type == 3)
                            {
                                Value = Value + 1;
                                NewValue = string.Format("{0:000000}", Value);
                                if (codePrefix != "")
                                {
                                    NewValue = codePrefix + NewValue;
                                }
                            }
                            else
                            {
                                NewValue = null;
                            }

                            if (item.TimeType == 1)       //hour
                            {
                                TempDate = Project.ProjectDate;
                                TempTime = DateTime.Now.AddHours(Convert.ToDouble(item.TimeMinutes)).ToString("h:mm");
                                tempRemaining = item.TimeMinutes * 60;
                            }
                            else        //day
                            {
                                d = oDate.AddDays(Convert.ToDouble(item.TimeMinutes));
                                TempDate = d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                                TempTime = DateTime.Now.ToString("h:mm", CultureInfo.InvariantCulture);
                                tempRemaining = item.TimeMinutes * 60 * 24;
                            }
                            var dateonly_A = TempDate;//mfroood hna l end
                            var timeonly_A = DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                            DateTime dt_A = DateTime.ParseExact(dateonly_A + " " + timeonly_A, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                            int? ParentV = null;
                            if (item.ParentId == null)
                            {
                                ParentV = null;
                            }
                            else
                            {
                                //ParentV = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId && s.ProjectId == Project.ProjectId && s.SettingId == item.ParentId).FirstOrDefault()?.PhaseTaskId ?? null;
                                ParentV = projectPhasesTaskList.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId && s.ProjectId == Project.ProjectId && s.SettingId == item.ParentId).FirstOrDefault()?.PhaseTaskId ?? null;
                            }

                            projectPhasesTaskObj = new ProjectPhasesTasks
                            {
                                DescriptionAr = item.DescriptionAr,
                                DescriptionEn = item.DescriptionEn,
                                ProjSubTypeId = item.ProjSubTypeId,
                                TaskNo= NewValue,
                                TaskNoType = 1,
                                Type = item.Type,
                                TimeMinutes = item.TimeMinutes,
                                Cost = item.Cost,
                                //Remaining = item.TimeMinutes,
                                Remaining = tempRemaining,
                                Status = (item.Type == 3) ? 1 : 0,
                                StopCount = 0,
                                TimeType = item.TimeType,
                                IsUrgent = item.IsUrgent,
                                TaskType = item.TaskType,
                                StartDate = null,
                                EndDate = null,
                                ExcpectedStartDate = Project.ProjectDate,
                                ExcpectedEndDate = TempDate,
                                ParentId = ParentV,
                                UserId = item.UserId,
                                SettingId = item.SettingId,
                                PhasePriority = item.Priority,
                                ParentSettingId = ParentV,
                                Notes = item.Notes,
                                ProjectId = Project.ProjectId,
                                BranchId = item.BranchId,
                                AddUser = UserId,
                                AddDate = DateTime.Now,
                                PlusTime = false,
                                IsMerig = item.IsMerig,
                                IsConverted=0,
                                EndTime = TempTime,
                                TaskFullTime = dt_A.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)

                            };
                            _TaamerProContext.ProjectPhasesTasks.Add(projectPhasesTaskObj);
                            _TaamerProContext.SaveChanges();  // commit 
                            projectPhasesTaskList.Add(projectPhasesTaskObj);
                        }
                        //_TaamerProContext.ProjectPhasesTasks.AddRange(projectPhasesTaskList); ///////// project tasks pahses
                        //_TaamerProContext.SaveChanges();  // commit 

                    }
                    ////update parentId
                    //var LastAddedPhasesTask = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId && s.ProjectId == Project.ProjectId).ToList();
                    //foreach (var item in LastAddedPhasesTask)
                    //{
                    //    if (item.ParentSettingId == null)
                    //    {
                    //        item.ParentId = null;
                    //    }
                    //    else
                    //    {
                    //        item.ParentId = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId && s.ProjectId == Project.ProjectId && s.SettingId == item.ParentSettingId).FirstOrDefault()?.PhaseTaskId??null;
                    //    }
                    //}
                    // save pro dependency  from setting dependency
                    //var projSubTypeDependencySett = _TaamerProContext.TasksDependency.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId).ToList();
                    var projSubTypeDependencySett = _TaamerProContext.DependencySettings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId).ToList();

                    var projectPhasesCount = _ProjectPhasesTasksRepository.GetAllTasksByProjectIdWithoutBranch(Project.ProjectId, BranchId).Result.Count();

                    if (projectPhasesCount != 0)
                    {
                        foreach (var item in projSubTypeDependencySett)
                        {
                            var ProDependency = new TasksDependency();
                            ProDependency.ProjSubTypeId = item.ProjSubTypeId;

                            var Pre = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == item.ProjSubTypeId && s.SettingId == item.PredecessorId && s.ProjectId == Project.ProjectId && s.Type == 3).FirstOrDefault();

                            var PreSetting = 0;
                            if (Pre != null)
                            {
                                PreSetting = Pre.PhaseTaskId;
                            }
                            ProDependency.PredecessorId = PreSetting;
                            var Succ = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == item.ProjSubTypeId && s.SettingId == item.SuccessorId && s.ProjectId == Project.ProjectId && s.Type == 3).FirstOrDefault();

                            var SuccSetting = 0;
                            if (Succ != null)
                            {
                                SuccSetting = Succ.PhaseTaskId;
                            }
                            ProDependency.SuccessorId = SuccSetting;
                            ProDependency.Type = item.Type;
                            ProDependency.BranchId = BranchId;
                            ProDependency.AddUser = UserId;
                            ProDependency.ProjectId = Project.ProjectId;
                            ProDependency.AddDate = DateTime.Now;
                            _TaamerProContext.TasksDependency.Add(ProDependency);
                        }
                    }
                }





                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تعديل المشروع رقم " + Project.ProjectId;
               _SystemAction.SaveAction("UpdateProjectPhasesTasks", "ProjectPhasesTasksService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully};
            }
            catch (Exception ex)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = Resources.General_SavedFailed;
               _SystemAction.SaveAction("UpdateProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }

        }
        public GeneralMessage UpdateProjectPhasesTasksNew(Project Project, int UserId, int BranchId)
        {


            try
            {
                var projectUpdated = _ProjectRepository.GetById(Project.ProjectId);


                var totaldays = 0.0;
                DateTime resultEnd = DateTime.ParseExact(Project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime resultStart = DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                totaldays = (resultEnd - resultStart).TotalDays + 1;
                if (projectUpdated.ProjectDate != Project.ProjectDate || projectUpdated.ProjectExpireDate != Project.ProjectExpireDate)
                {
                    projectUpdated.IsNotSent = false;

                }
                DateTime resultNow = DateTime.Now;

                DateTime resultEndSelect = DateTime.ParseExact(projectUpdated.FirstProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);


                if (resultNow < resultEndSelect)
                {
                    projectUpdated.FirstProjectExpireDate = Project.ProjectExpireDate;
                    projectUpdated.FirstProjectDate = Project.ProjectDate;

                }

                projectUpdated.NoOfDays = Convert.ToInt32(totaldays);
                projectUpdated.ProjectDate = Project.ProjectDate;
                projectUpdated.ProjectHijriDate = Project.ProjectHijriDate;
                projectUpdated.ProjectExpireDate = Project.ProjectExpireDate;
                projectUpdated.ProjectExpireHijriDate = Project.ProjectExpireHijriDate;
                projectUpdated.BuildingType = Project.BuildingType;


                if (projectUpdated.MangerId != Project.MangerId)
                {
                    var ProjectWorkersUpdated = _TaamerProContext.ProjectWorkers.Where(s => s.ProjectId == Project.ProjectId && s.WorkerType == 1).FirstOrDefault();
                    if (ProjectWorkersUpdated != null)
                    {
                        ProjectWorkersUpdated.UserId = Project.MangerId;
                    }

                }
                if (projectUpdated.CostCenterId != Project.CostCenterId)
                {
                    if (Project.CostCenterId != null)
                    {
                        var CostCenterProject = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.ProjId == Project.ProjectId).FirstOrDefault();
                        var CostCenterParent = _CostCenterRepository.GetById((int)Project.CostCenterId);

                        CostCenterProject.BranchId = CostCenterParent.BranchId;
                        CostCenterProject.ParentId = CostCenterParent.CostCenterId;
                        CostCenterProject.Level = (CostCenterParent.Level ?? 0) + 1;
                    }

                }
                if (projectUpdated.CustomerId != Project.CustomerId)
                {
                    if (Project.CustomerId != null)
                    {
                        var CostCenterProjectNew = _CostCenterRepository.GetMatching(s => s.IsDeleted == false && s.ProjId == Project.ProjectId).FirstOrDefault();
                        var CustomerNew = _CustomerRepository.GetById((int)Project.CustomerId);

                        CostCenterProjectNew.NameAr = CustomerNew.CustomerNameAr;
                        CostCenterProjectNew.NameEn = CustomerNew.CustomerNameEn;

                    }

                }


                if (Project.ProjectRequirementsGoals.Count() > 0)
                {
                    _TaamerProContext.ProjectRequirementsGoals.RemoveRange(projectUpdated.ProjectRequirementsGoals.ToList());

                    foreach (var item in Project.ProjectRequirementsGoals.ToList())
                    {
                        item.AddDate = DateTime.Now;
                        item.AddUser = UserId;

                        item.ProjectId = Project.ProjectId;
                        _TaamerProContext.ProjectRequirementsGoals.Add(item);
                    }

                }

                /////add projectno to offerprice
                if (Project.OffersPricesId != null)
                {
                    try
                    {

                        var offer = _offersPricesRepository.GetById((int)Project.OffersPricesId);

                        var oldoffer = _offersPricesRepository.GetMatching(x => x.IsDeleted == false && x.ProjectId == Project.ProjectId).FirstOrDefault();
                        if (oldoffer != null)
                        {
                            if (oldoffer.OffersPricesId != offer.OffersPricesId)
                            {
                                oldoffer.ProjectId = null;
                            }
                        }

                        offer.ProjectId = Project.ProjectId;
                    }
                    catch (Exception ex)
                    {

                    }
                }

                projectUpdated.CustomerId = Project.CustomerId;

                projectUpdated.CostCenterId = Project.CostCenterId;
                projectUpdated.MangerId = Project.MangerId;
                projectUpdated.CityId = Project.CityId;
                projectUpdated.ProjectDescription = Project.ProjectDescription;
                projectUpdated.ProjectTypeId = Project.ProjectTypeId;
                projectUpdated.SubProjectTypeId = Project.SubProjectTypeId;

                projectUpdated.BranchId = Project.BranchId;
                projectUpdated.OffersPricesId = Project.OffersPricesId;
                projectUpdated.DepartmentId = Project.DepartmentId;

                //ana 3ayz a5le mshroo3 bseer gded
                var ProjectIds = _TasksDependencyRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == Project.ProjectId).ToList();
                var Oldphases = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == Project.ProjectId).ToList();

                if (ProjectIds.Count() > 0)
                {
                    if (ProjectIds.FirstOrDefault().ProjSubTypeId != Project.SubProjectTypeId)
                    {
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكنك تعديل مشروع عليه سير بسير جديد" };

                    }
                    else
                    {
                        if ((projectUpdated.NewSetting??false) != Project.NewSetting)
                        {
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكنك تعديل مشروع عليه سير بسير جديد" };
                        }
                    }

                }


                projectUpdated.NewSetting = Project.NewSetting;


                bool checkMakeSetting = false;

                if (ProjectIds.Count() > 0) //mshro3 da kan bseeer
                {

                    if (ProjectIds.FirstOrDefault().ProjSubTypeId != Project.SubProjectTypeId)   //hwa hwaaaah nfs l seeer wla la l kan mwgoood
                    {

                        if (Project.SubProjectTypeId != null)
                        {
                            var projSubTypeSett2 = _TaamerProContext.SettingsNew.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId);
                            if (projSubTypeSett2.Count() != 0)
                            {
                                foreach (var item in projSubTypeSett2)
                                {
                                    if (item.UserId != null)
                                    {
                                        var UserCheck = _TaamerProContext.Users.Where(s => s.UserId == item.UserId).FirstOrDefault();

                                        if (UserCheck.IsDeleted == true)
                                        {
                                            //-----------------------------------------------------------------------------------------------------------------
                                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                            string ActionNote2 = "فشل في التعديل";
                                            _SystemAction.SaveAction("UpdateProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.failed_modify_check_users, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                            //-----------------------------------------------------------------------------------------------------------------
                                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.failed_modify_check_users };
                                        }
                                        //var UserVacation = _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && s.UserId == item.UserId && s.VacationStatus != 4 && (s.BackToWorkDate == null || s.BackToWorkDate.Equals(""))).Count();
                                        //if (UserVacation != 0)
                                        //{
                                        //    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Proj_SaveFailedUserVacationProSetting };
                                        //}
                                    }
                                }
                            }
                        }



                        if (Oldphases.Count() > 0)
                        {
                            foreach (var item in Oldphases)
                            {
                                if (item.Type == 3 && (item.Status == 2 || item.Status == 3))
                                {
                                    checkMakeSetting = false;
                                    //-----------------------------------------------------------------------------------------------------------------
                                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    string ActionNote2 = Resources.SaveFailedRunningStoppedTasksThatMustBeCompletedFirst;
                                    _SystemAction.SaveAction("UpdateProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                    //-----------------------------------------------------------------------------------------------------------------
                                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.SaveFailedRunningStoppedTasksThatMustBeCompletedFirst, ReturnedStr = "-1" };


                                }
                            }

                            _TaamerProContext.ProjectPhasesTasks.RemoveRange(Oldphases);
                            _TaamerProContext.TasksDependency.RemoveRange(ProjectIds);
                            checkMakeSetting = true;
                        }
                        else
                        {
                            checkMakeSetting = true;
                        }

                    }
                    else
                    {
                        checkMakeSetting = false;
                    }

                }
                else
                {
                    if (Oldphases.Count() > 0)
                    {
                        if (ProjectIds.Count() == 0)
                        {
                            _TaamerProContext.ProjectPhasesTasks.RemoveRange(Oldphases); //delete without main and without subtypemain
                            checkMakeSetting = true;
                        }
                        else
                        {
                            checkMakeSetting = false;

                        }
                    }
                    else
                    {
                        checkMakeSetting = true;
                    }
                }



                if (checkMakeSetting == true)
                {
                    //////////////////////////////////////////// project tasks and phases ///////////////////////////////////////////////////////////////////////
                    var projSubTypeSett = _TaamerProContext.SettingsNew.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId).ToList();
                    var projectPhasesTaskList = new List<ProjectPhasesTasks>();
                    var projectPhasesTaskObj = new ProjectPhasesTasks();
                    var projectWorkers = new List<ProjectWorkers>();
                    var projectWorkersPriv = new List<UserPrivileges>();
                    var ListOfTaskNotify = new List<Notification>();

                    var codePrefix = "TSK#";
                    //var prostartcode = _BranchesRepository.GetById(BranchId).TaskStartCode;
                    //if (prostartcode != null && prostartcode != "")
                    //{
                    //    codePrefix = prostartcode;
                    //}
                    var Value = _ProjectPhasesTasksRepository.GenerateNextTaskNumber(BranchId, codePrefix, 0).Result;
                    Value = Value - 1;

                    var TempTime = DateTime.Now.ToString("h:mm", CultureInfo.InvariantCulture);
                    var TempDate = Project.ProjectDate;
                    DateTime d = new DateTime();
                    DateTime oDate = DateTime.ParseExact(Project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    int? tempRemaining = 0;
                    if (projSubTypeSett.Count() != 0)
                    {
                        foreach (var item in projSubTypeSett)
                        {

                            string? NewValue = null;
                            if (item.Type == 3)
                            {
                                Value = Value + 1;
                                NewValue = string.Format("{0:000000}", Value);
                                if (codePrefix != "")
                                {
                                    NewValue = codePrefix + NewValue;
                                }
                            }
                            else
                            {
                                NewValue = null;
                            }

                            if (item.TimeType == 1)       //hour
                            {
                                TempDate = Project.ProjectDate;
                                TempTime = DateTime.Now.AddHours(Convert.ToDouble(item.TimeMinutes)).ToString("h:mm");
                                tempRemaining = item.TimeMinutes * 60;
                            }
                            else        //day
                            {
                                d = oDate.AddDays(Convert.ToDouble(item.TimeMinutes));
                                TempDate = d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                                TempTime = DateTime.Now.ToString("h:mm", CultureInfo.InvariantCulture);
                                tempRemaining = item.TimeMinutes * 60 * 24;
                            }
                            var dateonly_A = TempDate;//mfroood hna l end
                            var timeonly_A = DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                            DateTime dt_A = DateTime.ParseExact(dateonly_A + " " + timeonly_A, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                            int? ParentV = null;
                            if (item.ParentId == null)
                            {
                                ParentV = null;
                            }
                            else
                            {
                                //ParentV = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId && s.ProjectId == Project.ProjectId && s.SettingId == item.ParentId).FirstOrDefault()?.PhaseTaskId ?? null;
                                ParentV = projectPhasesTaskList.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId && s.ProjectId == Project.ProjectId && s.SettingId == item.ParentId).FirstOrDefault()?.PhaseTaskId ?? null;

                            }

                            projectPhasesTaskObj = new ProjectPhasesTasks
                            {
                                DescriptionAr = item.DescriptionAr,
                                DescriptionEn = item.DescriptionEn,
                                ProjSubTypeId = item.ProjSubTypeId,
                                TaskNo= NewValue,
                                TaskNoType=1,
                                Type = item.Type,
                                TimeMinutes = item.TimeMinutes,
                                Cost = item.Cost,
                                //Remaining = item.TimeMinutes,
                                Remaining = tempRemaining,
                                Status = (item.Type == 3) ? item.IsTemp == true ? 3 : 1 : 0,
                                StopCount = 0,
                                TimeType = item.TimeType,
                                IsUrgent = item.IsUrgent,
                                TaskType = item.TaskType,
                                StartDate = null,
                                EndDate = null,
                                ExcpectedStartDate = Project.ProjectDate,
                                ExcpectedEndDate = TempDate,
                                ParentId = ParentV,
                                UserId = item.UserId,
                                SettingId = item.SettingId,
                                PhasePriority = item.Priority,
                                ParentSettingId = ParentV,
                                Notes = item.Notes,
                                ProjectId = Project.ProjectId,
                                BranchId = item.BranchId,
                                AddUser = UserId,
                                AddDate = DateTime.Now,
                                PlusTime = false,
                                IsMerig = item.IsMerig,
                                IsConverted = 0,
                                EndTime = TempTime,
                                TaskFullTime = dt_A.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture),
                                indentation = item.indentation,
                                taskindex = item.taskindex,
                                ReasonsId = item.ReasonsId,
                                Managerapproval = item.Managerapproval,
                                IsTemp=item.IsTemp,
                                StartDateNew = item.StartDate,
                                EndDateNew = item.EndDate,
                            };
                            _TaamerProContext.ProjectPhasesTasks.Add(projectPhasesTaskObj);
                            _TaamerProContext.SaveChanges();  // commit 
                            projectPhasesTaskList.Add(projectPhasesTaskObj); 

                        }
                        //_TaamerProContext.ProjectPhasesTasks.AddRange(projectPhasesTaskList); ///////// project tasks pahses
                        //_TaamerProContext.SaveChanges();  // commit 

                    }
                    ////update parentId
                    //var LastAddedPhasesTask = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId && s.ProjectId == Project.ProjectId).ToList();
                    //foreach (var item in LastAddedPhasesTask)
                    //{
                    //    if (item.ParentSettingId == null)
                    //    {
                    //        item.ParentId = null;
                    //    }
                    //    else
                    //    {
                    //        item.ParentId = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId && s.ProjectId == Project.ProjectId && s.SettingId == item.ParentSettingId).FirstOrDefault()?.PhaseTaskId??null;
                    //    }
                    //}
                    // save pro dependency  from setting dependency
                    //var projSubTypeDependencySett = _TaamerProContext.TasksDependency.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId).ToList();
                    var projSubTypeDependencySett = _TaamerProContext.DependencySettingsNew.Where(s => s.IsDeleted == false && s.ProjSubTypeId == Project.SubProjectTypeId).ToList();

                    var projectPhasesCount = _ProjectPhasesTasksRepository.GetAllTasksByProjectIdWithoutBranch(Project.ProjectId, BranchId).Result.Count();

                    if (projectPhasesCount != 0)
                    {
                        foreach (var item in projSubTypeDependencySett)
                        {
                            var ProDependency = new TasksDependency();
                            ProDependency.ProjSubTypeId = item.ProjSubTypeId;

                            var Pre = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == item.ProjSubTypeId && s.SettingId == item.PredecessorId && s.ProjectId == Project.ProjectId && s.Type == 3).FirstOrDefault();

                            var PreSetting = 0;
                            if (Pre != null)
                            {
                                PreSetting = Pre.PhaseTaskId;
                            }
                            ProDependency.PredecessorId = PreSetting;
                            var Succ = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == item.ProjSubTypeId && s.SettingId == item.SuccessorId && s.ProjectId == Project.ProjectId && s.Type == 3).FirstOrDefault();

                            var SuccSetting = 0;
                            if (Succ != null)
                            {
                                SuccSetting = Succ.PhaseTaskId;
                            }
                            ProDependency.SuccessorId = SuccSetting;
                            ProDependency.Type = item.Type;
                            ProDependency.BranchId = BranchId;
                            ProDependency.AddUser = UserId;
                            ProDependency.ProjectId = Project.ProjectId;
                            ProDependency.AddDate = DateTime.Now;
                            _TaamerProContext.TasksDependency.Add(ProDependency);
                        }
                    }
                }





                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تعديل المشروع رقم " + Project.ProjectId;
                _SystemAction.SaveAction("UpdateProjectPhasesTasks", "ProjectPhasesTasksService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = Resources.General_SavedFailed;
                _SystemAction.SaveAction("UpdateProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }

        }


        public GeneralMessage SaveMainPhases_P(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId)
        {
            try
            {

                var project = _ProjectRepository.GetById((int)ProjectPhasesTasks.ProjectId);

                ProjectPhasesTasks.DescriptionAr = "بدون مرحلة رئيسية";
                ProjectPhasesTasks.DescriptionEn = "without Main Phase";
                ProjectPhasesTasks.Type = 1;
                ProjectPhasesTasks.ProjectId = project.ProjectId;


                ProjectPhasesTasks.ProjSubTypeId = project.SubProjectTypeId;
                ProjectPhasesTasks.Remaining = 0;
                ProjectPhasesTasks.BranchId = BranchId;
                ProjectPhasesTasks.StopCount = 0;
                ProjectPhasesTasks.IsMerig = -1;

                ProjectPhasesTasks.AddUser = UserId;
                ProjectPhasesTasks.AddDate = DateTime.Now;
                ProjectPhasesTasks.PlusTime = false;
                ProjectPhasesTasks.IsConverted = 0;

                _TaamerProContext.ProjectPhasesTasks.Add(ProjectPhasesTasks);

                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة مرحلة جديدة" +" علي مشروع"+ project.ProjectNo;
               _SystemAction.SaveAction("SaveMainPhases_P", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedParm = ProjectPhasesTasks.PhaseTaskId };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ المرحلة";
               _SystemAction.SaveAction("SaveMainPhases_P", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage SavefourMainPhases_P(int projectid, int UserId, int BranchId)
        {
            var project = _ProjectRepository.GetById(projectid);
            var TaskName = "";
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0) TaskName = "بدأ المشروع(1)initiation";
                    else if (i == 1) TaskName = "تخطيط المشروع(2)planning";
                    else if (i == 2) TaskName = "تنفيذ المشروع(3)Executing";
                    else if (i == 3) TaskName = "إغلاق المشروع(4)Closing";
                    else TaskName = "";


                    _TaamerProContext.ProjectPhasesTasks.Add(new ProjectPhasesTasks
                    {
                        DescriptionAr = TaskName,
                        DescriptionEn = TaskName,
                        Type = 1,
                        ProjectId = project.ProjectId,
                        ProjSubTypeId = project.SubProjectTypeId,
                        Remaining = 0,
                        BranchId = project.BranchId,
                        StopCount = 0,
                        IsMerig = -1,
                        AddUser = UserId,
                        AddDate = DateTime.Now,
                        PlusTime = false,
                        IsConverted = 0,
                    });
                }


                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة مرحلة جديدة" + " علي مشروع" + project.ProjectNo;
                _SystemAction.SaveAction("SavefourMainPhases_P", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully};
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ المرحلة";
                _SystemAction.SaveAction("SavefourMainPhases_P", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage SaveSubPhases_P(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, int ParentId)
        {
            try
            {

                var project = _ProjectRepository.GetById((int)ProjectPhasesTasks.ProjectId);

                ProjectPhasesTasks.DescriptionAr = "بدون مرحلة فرعية";
                ProjectPhasesTasks.DescriptionEn = "without SubMain Phases";
                ProjectPhasesTasks.Type = 2;
                ProjectPhasesTasks.ProjectId = project.ProjectId;

                ProjectPhasesTasks.ParentId = ParentId;
                ProjectPhasesTasks.ProjSubTypeId = project.SubProjectTypeId;
                ProjectPhasesTasks.Remaining = 0;
                ProjectPhasesTasks.BranchId = BranchId;
                ProjectPhasesTasks.StopCount = 0;
                ProjectPhasesTasks.IsMerig = -1;

                ProjectPhasesTasks.AddUser = UserId;
                ProjectPhasesTasks.AddDate = DateTime.Now;
                ProjectPhasesTasks.PlusTime = false;
                ProjectPhasesTasks.IsConverted = 0;
                _TaamerProContext.ProjectPhasesTasks.Add(ProjectPhasesTasks);

                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة مرحلة فرعية" +"علي مشروع"+ project.ProjectNo;
               _SystemAction.SaveAction("SaveSubPhases_P", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedParm = ProjectPhasesTasks.PhaseTaskId };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ المرحلةالفرعية";
               _SystemAction.SaveAction("SaveSubPhases_P", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        //EditD2
        public GeneralMessage SaveNewProjectPhasesTasks(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId)
        {
            try
            {
                if (ProjectPhasesTasks.Type == 3)
                {
                    var UserVacation = _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && s.UserId == ProjectPhasesTasks.UserId && s.VacationStatus == 2 && s.DecisionType == 1
                    &&
                    ((s.BackToWorkDate == null || s.BackToWorkDate.Equals("")) || // لسه مرجعش من إجازة 
                    (
                        // أو عنده إجازة في نفس وقت المهمة
                        (!(s.StartDate == null || s.StartDate.Equals("")) && !(ProjectPhasesTasks.ExcpectedStartDate == null || ProjectPhasesTasks.ExcpectedStartDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ProjectPhasesTasks.ExcpectedStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                        (!(s.EndDate == null || s.EndDate.Equals("")) && !(ProjectPhasesTasks.ExcpectedEndDate == null || ProjectPhasesTasks.ExcpectedEndDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(ProjectPhasesTasks.ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                    )
                    );

                    //var UserVacation = _VacationRepository.GetMatching(s => s.IsDeleted == false && s.UserId == ProjectPhasesTasks.UserId && s.VacationStatus != 4 && string.IsNullOrEmpty(s.BackToWorkDate)).Count();
                    if (UserVacation.Count() != 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = Resources.General_SavedFailed;
                       _SystemAction.SaveAction("SaveNewProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.UserVac };
                    }
                }
                var project = _ProjectRepository.GetById(ProjectPhasesTasks.ProjectId??0);

                if (project != null)
                {
                    project.MotionProject = 1;
                    project.MotionProjectDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    project.MotionProjectNote = "أضافة مهمة علي مشروع";
                }

                var BranchIdOfUser = _UsersRepository.GetById(ProjectPhasesTasks.UserId ?? 0);

                var ProPhasesTasksUpdated = _ProjectPhasesTasksRepository.GetById(ProjectPhasesTasks.PhaseTaskId);
                if (ProjectPhasesTasks.PhaseTaskId == 0)
                {
                    int? tempRemaining = 0;
                    switch (ProjectPhasesTasks.TimeType)
                    {
                        case 1:
                            tempRemaining = ProjectPhasesTasks.TimeMinutes * 60;
                            break;
                        case 2:
                            tempRemaining = ProjectPhasesTasks.TimeMinutes * 60 * 24;
                            break;
                    }
                    ProjectPhasesTasks.ProjSubTypeId = project.SubProjectTypeId;
                    //ProjectPhasesTasks.Remaining = ProjectPhasesTasks.TimeMinutes;
                    ProjectPhasesTasks.Remaining = tempRemaining;

                    if (ProjectPhasesTasks.Type == 3)
                    {
                        ProjectPhasesTasks.Status = 1;
                        ProjectPhasesTasks.BranchId = BranchIdOfUser.BranchId;
                        ProjectPhasesTasks.EndTime = DateTime.Now.ToString("h:mm", CultureInfo.InvariantCulture);
                        var dateonly = ProjectPhasesTasks.ExcpectedEndDate;
                        var timeonly = DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                        DateTime dt1 = DateTime.ParseExact(dateonly + " " + timeonly, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        ProjectPhasesTasks.TaskFullTime = dt1.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                        ProjectPhasesTasks.IsMerig = -1;
                    }
                    else
                    {
                        ProjectPhasesTasks.BranchId = BranchId;
                    }
                    ProjectPhasesTasks.StopCount = 0;

                    ProjectPhasesTasks.AddUser = UserId;
                    ProjectPhasesTasks.AddDate = DateTime.Now;
                    ProjectPhasesTasks.PlusTime = false;
                    _TaamerProContext.ProjectPhasesTasks.Add(ProjectPhasesTasks);
                    if (ProjectPhasesTasks.Type == 3)
                    {
                        try
                        {
                            //SendMail(ProjectPhasesTasks, BranchId, UserId);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                else
                {
                    int? TempRem = 0;
                    if (ProPhasesTasksUpdated != null)
                    {
                        switch (ProPhasesTasksUpdated.TimeType)
                        {
                            case 1:
                                TempRem = ProPhasesTasksUpdated.TimeMinutes * 60;
                                break;
                            case 2:
                                TempRem = ProPhasesTasksUpdated.TimeMinutes * 60 * 24;
                                break;
                        }

                        ProPhasesTasksUpdated.DescriptionAr = ProjectPhasesTasks.DescriptionAr;
                        ProPhasesTasksUpdated.DescriptionEn = ProjectPhasesTasks.DescriptionEn;
                        ProPhasesTasksUpdated.UserId = ProjectPhasesTasks.UserId;
                        ProPhasesTasksUpdated.TimeMinutes = ProjectPhasesTasks.TimeMinutes;
                        ProPhasesTasksUpdated.Remaining = TempRem;
                        ProPhasesTasksUpdated.Cost = ProjectPhasesTasks.Cost;
                        ProPhasesTasksUpdated.TimeType = ProjectPhasesTasks.TimeType;
                        ProPhasesTasksUpdated.IsUrgent = ProjectPhasesTasks.IsUrgent;
                        ProPhasesTasksUpdated.TaskType = ProjectPhasesTasks.TaskType;
                        ProPhasesTasksUpdated.Notes = ProjectPhasesTasks.Notes;
                        ProPhasesTasksUpdated.UpdateUser = UserId;
                        ProPhasesTasksUpdated.PhasePriority = ProjectPhasesTasks.PhasePriority;
                        ProPhasesTasksUpdated.PlusTime = ProjectPhasesTasks.PlusTime;
                        ProPhasesTasksUpdated.UpdateDate = DateTime.Now;
                        ProPhasesTasksUpdated.TaskLongDesc = ProjectPhasesTasks.TaskLongDesc;

                    }
                }
                //////////add task notification
                if (ProjectPhasesTasks.Type == 3 && ProjectPhasesTasks.PhaseTaskId == 0 || ((ProPhasesTasksUpdated != null && ProPhasesTasksUpdated.UserId != ProjectPhasesTasks.UserId)))
                {
                    var branch = _BranchesRepository.GetById(BranchId);
                    var customer = _CustomerRepository.GetById((int)project.CustomerId);


                    try
                    {
                        var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(ProjectPhasesTasks.UserId ?? 0).Result;
                        if (UserNotifPriv.Count() != 0)
                        {
                            if (UserNotifPriv.Contains(352))
                            {
                                var UserNotification = new Notification();
                                UserNotification.ReceiveUserId = ProjectPhasesTasks.UserId;
                                UserNotification.Name = Resources.General_Newtasks;
                                UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                UserNotification.SendUserId = UserId;
                                UserNotification.Type = 1; // notification
                                UserNotification.Description = "مهمه  : " + ProjectPhasesTasks.DescriptionAr + " علي مشروع رقم " + project.ProjectNo + " للعميل " + customer.CustomerNameAr + " " + " فرع " + branch.NameAr + "";
                                UserNotification.AllUsers = false;
                                UserNotification.SendDate = DateTime.Now;
                                UserNotification.ProjectId = ProjectPhasesTasks.ProjectId;
                                UserNotification.TaskId = ProjectPhasesTasks.PhaseTaskId;
                                UserNotification.AddUser = UserId;
                                UserNotification.IsHidden = false;
                                UserNotification.NextTime = null;

                                UserNotification.AddDate = DateTime.Now;
                                _TaamerProContext.Notification.Add(UserNotification);
                                _notificationService.sendmobilenotification((int)ProjectPhasesTasks.UserId, Resources.General_Newtasks, "لديك مهمه جديدة : " + ProjectPhasesTasks.DescriptionAr + " علي مشروع رقم " + project.ProjectNo + " للعميل " + customer.CustomerNameAr + " " + " فرع " + branch.NameAr + "");
                            }


                            if (UserNotifPriv.Contains(353))
                            {
                                var userObj = _UsersRepository.GetById((int)ProjectPhasesTasks.UserId);

                                var NotStr = customer.CustomerNameAr + " للعميل  " + project.ProjectNo + " علي مشروع رقم " + ProjectPhasesTasks.DescriptionAr + " لديك مهمه جديدة  ";
                                if (userObj.Mobile != null && userObj.Mobile != "")
                                {
                                    var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                }
                            }


                            //if(UserNotifPriv.Contains(351))
                            //{
                            //    var Desc = branch.NameAr + " فرع " + customer.CustomerNameAr + " للعميل " + project.ProjectNo + " علي مشروع رقم " + ProjectPhasesTasks.DescriptionAr + " لديك مهمه جديدة : ";

                            //    SendMailNoti(project.ProjectId, Desc, "اضافة مهمة جديدة", BranchId, UserId, ProjectPhasesTasks.UserId ?? 0);
                            //}
                        }
                    }
                    catch (Exception)
                    {
                    }

                }
                /////////////////////////////////////////////////////
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة مهمة لمرحلة" +" باسم" + ProjectPhasesTasks.DescriptionAr;
               _SystemAction.SaveAction("SaveNewProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedParm = ProjectPhasesTasks.PhaseTaskId };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ المهمة";
               _SystemAction.SaveAction("SaveNewProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        //EditD3

        public int SaveNewProjectPhasesTasks2(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId)
        {
            var UserVacation = _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && s.UserId == ProjectPhasesTasks.UserId && s.VacationStatus == 2 && s.DecisionType == 1
            &&
            ((s.BackToWorkDate == null || s.BackToWorkDate.Equals("")) || // لسه مرجعش من إجازة 
            (
                // أو عنده إجازة في نفس وقت المهمة
                (!(s.StartDate == null || s.StartDate.Equals("")) && !(ProjectPhasesTasks.ExcpectedStartDate == null || ProjectPhasesTasks.ExcpectedStartDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ProjectPhasesTasks.ExcpectedStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                (!(s.EndDate == null || s.EndDate.Equals("")) && !(ProjectPhasesTasks.ExcpectedEndDate == null || ProjectPhasesTasks.ExcpectedEndDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(ProjectPhasesTasks.ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
            )
            );

            if (UserVacation.Count() != 0)
            {
                return -1;
            }
            else
            {
                if ((ProjectPhasesTasks.ExcpectedStartDate == null || ProjectPhasesTasks.ExcpectedEndDate == null) && ProjectPhasesTasks.TimeType == 2)
                {
                    return 0;
                }
                else
                {
                    try
                    {
                        if (ProjectPhasesTasks.ExcpectedStartDate == null || ProjectPhasesTasks.ExcpectedEndDate == null)
                        {

                            ProjectPhasesTasks.ExcpectedStartDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                            ProjectPhasesTasks.ExcpectedEndDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                        }
                        var BranchIdOfUser = _UsersRepository.GetById((int)ProjectPhasesTasks.UserId);
                        var project = _ProjectRepository.GetById((int)ProjectPhasesTasks.ProjectId);
                        if (project != null)
                        {
                            project.MotionProject = 1;
                            project.MotionProjectDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            project.MotionProjectNote = "أضافة مهمة علي مشروع";
                        }
                        var ProPhasesTasksUpdated = _ProjectPhasesTasksRepository.GetById(ProjectPhasesTasks.PhaseTaskId);
                        if (ProjectPhasesTasks.PhaseTaskId == 0)
                        {
                            int? tempRemaining = 0;
                            switch (ProjectPhasesTasks.TimeType)
                            {
                                case 1:
                                    tempRemaining = ProjectPhasesTasks.TimeMinutes * 60;
                                    break;
                                case 2:
                                    tempRemaining = ProjectPhasesTasks.TimeMinutes * 60 * 24;
                                    break;
                            }


                            var StatusTemp = ProjectPhasesTasks.Status;

                            ProjectPhasesTasks.ProjSubTypeId = project.SubProjectTypeId;
                            //ProjectPhasesTasks.Remaining = ProjectPhasesTasks.TimeMinutes;
                            ProjectPhasesTasks.Remaining = tempRemaining;

                            if (ProjectPhasesTasks.Type == 3)
                            {
                                ProjectPhasesTasks.Status = 1;
                                ProjectPhasesTasks.EndTime = DateTime.Now.ToString("h:mm", CultureInfo.InvariantCulture);
                                //ProjectPhasesTasks.TaskFullTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                                var dateonly = ProjectPhasesTasks.ExcpectedEndDate;
                                var timeonly = DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                                DateTime dt1 = DateTime.ParseExact(dateonly + " " + timeonly, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                                ProjectPhasesTasks.TaskFullTime = dt1.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                            }
                            ProjectPhasesTasks.IsMerig = -1;
                            ProjectPhasesTasks.StopCount = 0;
                            ProjectPhasesTasks.BranchId = BranchIdOfUser.BranchId;
                            ProjectPhasesTasks.AddUser = UserId;
                            ProjectPhasesTasks.AddDate = DateTime.Now;
                            ProjectPhasesTasks.PlusTime = false;
                            ProjectPhasesTasks.IsNew = 1;
                            _TaamerProContext.ProjectPhasesTasks.Add(ProjectPhasesTasks);
                            _TaamerProContext.SaveChanges();


                            if (StatusTemp == 2)
                            {
                                var ProPhasesTasksUpdated2 = _ProjectPhasesTasksRepository.GetById(ProjectPhasesTasks.PhaseTaskId);
                                ProPhasesTasksUpdated2.Status = StatusTemp;
                                _TaamerProContext.SaveChanges();
                            }

                            try
                            {
                                // SendMail(ProjectPhasesTasks, BranchId, UserId);
                            }
                            catch (Exception ex)
                            {
                            }

                            var branch = _BranchesRepository.GetById(BranchId);
                            var customer = _CustomerRepository.GetById((int)project.CustomerId);

                            try
                            {
                                var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(ProjectPhasesTasks.UserId ?? 0).Result;
                                if (UserNotifPriv.Count() != 0)
                                {
                                    if (UserNotifPriv.Contains(352))
                                    {
                                        var UserNotification = new Notification();
                                        UserNotification.ReceiveUserId = ProjectPhasesTasks.UserId;
                                        UserNotification.Name = Resources.General_Newtasks;
                                        UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                        UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                        UserNotification.SendUserId = UserId;
                                        UserNotification.Type = 1; // notification
                                        UserNotification.Description = "مهمه  : " + ProjectPhasesTasks.DescriptionAr + " علي مشروع رقم " + project.ProjectNo + " للعميل " + customer.CustomerNameAr + " " + " فرع " + branch.NameAr + "";
                                        UserNotification.AllUsers = false;
                                        UserNotification.SendDate = DateTime.Now;
                                        UserNotification.ProjectId = ProjectPhasesTasks.ProjectId;
                                        UserNotification.TaskId = ProjectPhasesTasks.PhaseTaskId;
                                        UserNotification.AddUser = UserId;
                                        UserNotification.AddDate = DateTime.Now;
                                        UserNotification.BranchId = BranchIdOfUser.BranchId;
                                        UserNotification.IsHidden = false;
                                        UserNotification.NextTime = null;
                                        _TaamerProContext.Notification.Add(UserNotification);
                                        _notificationService.sendmobilenotification((int)ProjectPhasesTasks.UserId, Resources.General_Newtasks, "لديك مهمه جديدة : " + ProjectPhasesTasks.DescriptionAr + " علي مشروع رقم " + project.ProjectNo + " للعميل " + customer.CustomerNameAr + " " + " فرع " + branch.NameAr + "");
                                    }


                                    if (UserNotifPriv.Contains(353))
                                    {
                                        var userObj = _UsersRepository.GetById((int)ProjectPhasesTasks.UserId);

                                        var NotStr = customer.CustomerNameAr + " للعميل  " + project.ProjectNo + " علي مشروع رقم " + ProjectPhasesTasks.DescriptionAr + " لديك مهمه جديدة  ";
                                        if (userObj.Mobile != null && userObj.Mobile != "")
                                        {
                                            var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                        }
                                    }

                                    //if (UserNotifPriv.Contains(351))
                                    //{
                                    //    var Desc = branch.NameAr + " فرع " + customer.CustomerNameAr + " للعميل " + project.ProjectNo + " علي مشروع رقم " + ProjectPhasesTasks.DescriptionAr + " لديك مهمه جديدة : ";

                                    //    SendMailNoti(project.ProjectId, Desc, "اضافة مهمة جديدة", BranchId, UserId, ProjectPhasesTasks.UserId ?? 0);
                                    //}
                                }
                            }
                            catch (Exception ex)
                            {
                            }

                        }
                        else
                        {
                            if (ProPhasesTasksUpdated != null)
                            {

                                int? tempRemainingU = 0;
                                switch (ProPhasesTasksUpdated.TimeType)
                                {
                                    case 1:
                                        tempRemainingU = ProPhasesTasksUpdated.TimeMinutes * 60;
                                        break;
                                    case 2:
                                        tempRemainingU = ProPhasesTasksUpdated.TimeMinutes * 60 * 24;
                                        break;
                                }

                                ProPhasesTasksUpdated.DescriptionAr = ProjectPhasesTasks.DescriptionAr;
                                ProPhasesTasksUpdated.DescriptionEn = ProjectPhasesTasks.DescriptionEn;
                                ProPhasesTasksUpdated.UserId = ProjectPhasesTasks.UserId;
                                ProPhasesTasksUpdated.TimeMinutes = ProjectPhasesTasks.TimeMinutes;
                                ProPhasesTasksUpdated.Remaining = tempRemainingU;
                                ProPhasesTasksUpdated.Cost = ProjectPhasesTasks.Cost;
                                ProPhasesTasksUpdated.TimeType = ProjectPhasesTasks.TimeType;
                                ProPhasesTasksUpdated.IsUrgent = ProjectPhasesTasks.IsUrgent;
                                ProPhasesTasksUpdated.TaskType = ProjectPhasesTasks.TaskType;
                                ProPhasesTasksUpdated.Notes = ProjectPhasesTasks.Notes;
                                ProPhasesTasksUpdated.UpdateUser = UserId;
                                ProPhasesTasksUpdated.BranchId = BranchIdOfUser.BranchId;
                                ProPhasesTasksUpdated.PlusTime = ProjectPhasesTasks.PlusTime;
                                //ProPhasesTasksUpdated.StartDate = ProjectPhasesTasks.StartDate;
                                //ProPhasesTasksUpdated.EndDate = ProjectPhasesTasks.EndDate;
                                ProPhasesTasksUpdated.ExcpectedStartDate = ProjectPhasesTasks.ExcpectedStartDate;
                                ProPhasesTasksUpdated.ExcpectedEndDate = ProjectPhasesTasks.ExcpectedEndDate;
                                ProPhasesTasksUpdated.UpdateDate = DateTime.Now;
                                //ProPhasesTasksUpdated.EndTime = DateTime.Now.ToString("h:mm");
                                ProPhasesTasksUpdated.TaskLongDesc = ProjectPhasesTasks.TaskLongDesc;
                                ProPhasesTasksUpdated.Totalhourstask = ProjectPhasesTasks.Totalhourstask;
                                ProPhasesTasksUpdated.Totaltaskcost = ProjectPhasesTasks.Totaltaskcost;
                                if (ProPhasesTasksUpdated.ExcpectedStartDate != ProjectPhasesTasks.ExcpectedStartDate)
                                {
                                    var dateonly = ProjectPhasesTasks.ExcpectedEndDate;
                                    var timeonly = DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                                    ProPhasesTasksUpdated.EndTime = DateTime.Now.ToString("h:mm", CultureInfo.InvariantCulture);
                                    DateTime dt1 = DateTime.ParseExact(dateonly + " " + timeonly, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                                    ProjectPhasesTasks.TaskFullTime = dt1.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                                }
                            }
                        }

                        _TaamerProContext.SaveChanges();
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "اضافة مهمة جديدة" + " باسم" + ProPhasesTasksUpdated.DescriptionAr;
                       _SystemAction.SaveAction("SaveNewProjectPhasesTasks2", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------
                        return ProjectPhasesTasks.PhaseTaskId;
                    }
                    catch (Exception ex)
                    {
                        //var x = ex.Message;
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "فشل في حفظ المهمة";
                       _SystemAction.SaveAction("SaveNewProjectPhasesTasks2", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return 0;
                    }
                }
            }
        }
        public GeneralMessage SaveNewProjectPhasesTasks_E(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, string Url, string ImgUrl)
        {
            if (ProjectPhasesTasks.UserId != 0 && ProjectPhasesTasks.UserId != null)
            {

                var UserVacation = _TaamerProContext.Vacation.AsEnumerable().Where(s => s.IsDeleted == false && s.UserId == ProjectPhasesTasks.UserId && s.VacationStatus == 2 && s.DecisionType == 1 && (s.BackToWorkDate == null || (s.BackToWorkDate ?? "") == "")).ToList();
                UserVacation = UserVacation.Where(s =>
                   // أو عنده إجازة في نفس وقت المهمة
                ((!(s.StartDate == null || s.StartDate.Equals("")) && !(ProjectPhasesTasks.ExcpectedStartDate == null || ProjectPhasesTasks.ExcpectedStartDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(ProjectPhasesTasks.ExcpectedStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    (!(s.StartDate == null || s.StartDate.Equals("")) && !(ProjectPhasesTasks.ExcpectedEndDate == null || ProjectPhasesTasks.ExcpectedEndDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ProjectPhasesTasks.ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                    ||
                    ((!(s.EndDate == null || s.EndDate.Equals("")) && !(ProjectPhasesTasks.ExcpectedStartDate == null || ProjectPhasesTasks.ExcpectedStartDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(ProjectPhasesTasks.ExcpectedStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    (!(s.EndDate == null || s.EndDate.Equals("")) && !(ProjectPhasesTasks.ExcpectedEndDate == null || ProjectPhasesTasks.ExcpectedEndDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ProjectPhasesTasks.ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                 ||
                ((!(s.StartDate == null || s.StartDate.Equals("")) && !(ProjectPhasesTasks.ExcpectedStartDate == null || ProjectPhasesTasks.ExcpectedStartDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ProjectPhasesTasks.ExcpectedStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                (!(s.EndDate == null || s.EndDate.Equals("")) && !(ProjectPhasesTasks.ExcpectedStartDate == null || ProjectPhasesTasks.ExcpectedStartDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(ProjectPhasesTasks.ExcpectedStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                ||
                ((!(s.StartDate == null || s.StartDate.Equals("")) && !(ProjectPhasesTasks.ExcpectedEndDate == null || ProjectPhasesTasks.ExcpectedEndDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ProjectPhasesTasks.ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                (!(s.EndDate == null || s.EndDate.Equals("")) && !(ProjectPhasesTasks.ExcpectedEndDate == null || ProjectPhasesTasks.ExcpectedEndDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(ProjectPhasesTasks.ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))

                ).ToList();

                if (UserVacation.Count() != 0)
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.UserVac };
                }
            }




            if ((ProjectPhasesTasks.ExcpectedStartDate == null || ProjectPhasesTasks.ExcpectedEndDate == null) && ProjectPhasesTasks.TimeType == 2)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Check_the_duration };
            }
            else
            {
                try
                {
                    if (ProjectPhasesTasks.ExcpectedStartDate == null || ProjectPhasesTasks.ExcpectedEndDate == null)
                    {
                        ProjectPhasesTasks.ExcpectedStartDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                        ProjectPhasesTasks.ExcpectedEndDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    }
                    var BranchIdOfUserOrDepartment = 0;
                    var project = _ProjectRepository.GetById(ProjectPhasesTasks.ProjectId ?? 0);
                    project.UpdateUser = UserId;
                    project.UpdateDate = DateTime.Now;

                    if (ProjectPhasesTasks.UserId != null)
                    {
                        BranchIdOfUserOrDepartment = _UsersRepository.GetById(ProjectPhasesTasks.UserId ?? 0).BranchId ?? 0;
                    }
                    else
                    {
                        BranchIdOfUserOrDepartment = _departmentRepository.GetDepartmentbyid(ProjectPhasesTasks.DepartmentId ?? 0).Result?.FirstOrDefault()?.BranchId ?? 0;
                    }
                    if (project != null)
                    {
                        BranchIdOfUserOrDepartment = project.BranchId;
                    }

                    if(ProjectPhasesTasks.TaskNo!="" && ProjectPhasesTasks.TaskNo!=null)
                    {
                        var codeExist = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.TaskNo == ProjectPhasesTasks.TaskNo && s.PhaseTaskId != ProjectPhasesTasks.PhaseTaskId).FirstOrDefault();
                        if (codeExist != null)
                        {

                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = "فشل في حفظ المهمة الكود موجود مسبقا";
                            _SystemAction.SaveAction("SaveNewProjectPhasesTasks_E", "ProjectPhasesTasksService", 1, "رقم المهمة موجود من قبل", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "رقم المهمة موجود من قبل" };
                        }
                    }



                    if (project != null)
                    {
                        project.MotionProject = 1;
                        project.MotionProjectDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        project.MotionProjectNote = "أضافة مهمة علي مشروع";
                    }
                    var ProPhasesTasksUpdated = _ProjectPhasesTasksRepository.GetById(ProjectPhasesTasks.PhaseTaskId);
                    if (ProjectPhasesTasks.PhaseTaskId == 0)
                    {
                        int? tempRemaining = 0;
                        DateTime startTime = DateTime.ParseExact(ProjectPhasesTasks.ExcpectedStartDate + " " + ProjectPhasesTasks.TaskTimeFrom, "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture);
                        DateTime endTime = DateTime.ParseExact(ProjectPhasesTasks.ExcpectedEndDate + " " + ProjectPhasesTasks.TaskTimeTo, "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture);
                        ProjectPhasesTasks.StartDateNew = startTime;
                        ProjectPhasesTasks.EndDateNew = endTime;

                        var DateDiff = ((ProjectPhasesTasks.EndDateNew ?? new DateTime()) - (ProjectPhasesTasks.StartDateNew ?? new DateTime()));
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
                        tempRemaining = Convert.ToInt32(DateDiff.TotalMinutes);
                        ProjectPhasesTasks.TimeMinutes = TimeMinutes;
                        ProjectPhasesTasks.TimeType = TimeType;


                        //switch (ProjectPhasesTasks.TimeType)
                        //{
                        //    case 1:
                        //        tempRemaining = ProjectPhasesTasks.TimeMinutes * 60;
                        //        break;
                        //    case 2:
                        //        if (ProjectPhasesTasks.ExcpectedStartDate == ProjectPhasesTasks.ExcpectedEndDate)
                        //        {
                        //            tempRemaining = ProjectPhasesTasks.TimeMinutes * 60;
                        //            ProjectPhasesTasks.TimeType = 1;

                        //        }
                        //        else
                        //        {


                        //            TimeSpan span = endTime.Subtract(startTime);
                        //            var TimeTestMins = span.TotalMinutes;
                        //            tempRemaining = ProjectPhasesTasks.TimeMinutes * 60 * 24;
                        //            tempRemaining =Convert.ToInt32(TimeTestMins);

                        //        }
                        //        break;
                        //}

                        if (ProjectPhasesTasks.NotVacCalc == true)
                        {
                            ProjectPhasesTasks.StartDate = ProjectPhasesTasks.ExcpectedStartDate;
                            ProjectPhasesTasks.EndDate = ProjectPhasesTasks.ExcpectedEndDate;
                            ProjectPhasesTasks.EndDateCalc = ProjectPhasesTasks.ExcpectedEndDate;
                        }

                        var StatusTemp = ProjectPhasesTasks.Status;
                        ProjectPhasesTasks.ProjSubTypeId = project?.SubProjectTypeId;
                        ProjectPhasesTasks.Remaining = tempRemaining;

                        if (ProjectPhasesTasks.Type == 3)
                        {
                            ProjectPhasesTasks.Status = 1;
                            ProjectPhasesTasks.EndTime = DateTime.Now.ToString("h:mm", CultureInfo.InvariantCulture);
                            var dateonly = ProjectPhasesTasks.ExcpectedEndDate;
                            var timeonly = DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                            DateTime dt1 = DateTime.ParseExact(dateonly + " " + timeonly, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            ProjectPhasesTasks.TaskFullTime = dt1.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                        }
                        ProjectPhasesTasks.IsMerig = -1;
                        ProjectPhasesTasks.StopCount = 0;
                        ProjectPhasesTasks.BranchId = BranchIdOfUserOrDepartment;
                        ProjectPhasesTasks.AddUser = UserId;
                        ProjectPhasesTasks.AddDate = DateTime.Now;
                        ProjectPhasesTasks.PlusTime = false;
                        ProjectPhasesTasks.AddTaskUserId = UserId;
                        ProjectPhasesTasks.IsNew = 1;
                        ProjectPhasesTasks.taskindex = 100000;
                        _TaamerProContext.ProjectPhasesTasks.Add(ProjectPhasesTasks);
                        _TaamerProContext.SaveChanges();

                        if (StatusTemp == 2)
                        {
                            //mfrod hna check tar5 bdaya b3d enharda wla abl enharda w 3la asasoo ha7dd ysh8l wla la
                            DateTime resultStart = DateTime.ParseExact(ProjectPhasesTasks.ExcpectedStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            DateTime resultEnd = DateTime.ParseExact(ProjectPhasesTasks.ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);


                            var ResultnowString = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            DateTime resultNow = DateTime.ParseExact(ResultnowString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            var ProPhasesTasksUpdated2 = _ProjectPhasesTasksRepository.GetById(ProjectPhasesTasks.PhaseTaskId);

                            if (resultStart == resultNow)
                            {
                                ProPhasesTasksUpdated2.Status = StatusTemp;
                            }
                            else
                            {
                                ProPhasesTasksUpdated2.Status = 1;
                            }

                            _TaamerProContext.SaveChanges();
                        }
                        else if (StatusTemp == 7)
                        {
                            //ProjectPhasesTasks.Status = StatusTemp;
                            ProjectPhasesTasks.Status = 3;
                            ProjectPhasesTasks.IsTemp = true;

                        }
                        var branch = _BranchesRepository.GetById(BranchId);
                        var customer = _CustomerRepository.GetById(project.CustomerId ?? 0);

                        try
                        {
                            if (ProjectPhasesTasks.UserId != 0 && ProjectPhasesTasks.UserId != null)
                            {
                                //var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(ProjectPhasesTasks.UserId ?? 0).Result;
                                //if (UserNotifPriv.Count() != 0)
                                //{
                                //    if (UserNotifPriv.Contains(352))
                                //    {
                                        var UserNotification = new Notification();
                                        UserNotification.ReceiveUserId = ProjectPhasesTasks.UserId;
                                        UserNotification.Name = Resources.General_Newtasks;
                                        UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                        UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                        UserNotification.SendUserId = UserId;
                                        UserNotification.Type = 1; // notification
                                        UserNotification.Description = "لديك مهمه جديدة : " + ProjectPhasesTasks.DescriptionAr +" رقم "+ ProjectPhasesTasks.TaskNo + " علي مشروع رقم " + project.ProjectNo + " للعميل " + customer.CustomerNameAr + " " + " فرع " + branch.NameAr + "";
                                        UserNotification.AllUsers = false;
                                        UserNotification.SendDate = DateTime.Now;
                                        UserNotification.ProjectId = ProjectPhasesTasks.ProjectId;
                                        UserNotification.TaskId = ProjectPhasesTasks.PhaseTaskId;
                                        UserNotification.AddUser = UserId;
                                        UserNotification.AddDate = DateTime.Now;
                                        UserNotification.BranchId = BranchIdOfUserOrDepartment;
                                        UserNotification.IsHidden = false;
                                        UserNotification.NextTime = null;
                                        _TaamerProContext.Notification.Add(UserNotification);
                                        _notificationService.sendmobilenotification(ProjectPhasesTasks.UserId ?? 0, Resources.General_Newtasks, "لديك مهمه جديدة : " + ProjectPhasesTasks.DescriptionAr + " رقم " + ProjectPhasesTasks.TaskNo + " علي مشروع رقم " + project.ProjectNo + " للعميل " + customer.CustomerNameAr + " " + " فرع " + branch.NameAr + "");
                                   // }


                                    //if (UserNotifPriv.Contains(353))
                                    //{
                                        var userObj = _UsersRepository.GetById(ProjectPhasesTasks.UserId ?? 0);

                                        //var NotStr = customer.CustomerNameAr + " للعميل  " + project.ProjectNo + " علي مشروع رقم " + ProjectPhasesTasks.DescriptionAr + " لديك مهمه جديدة  ";
                                        var NotStr = "لديك مهمة جديدة" + ProjectPhasesTasks.DescriptionAr + " رقم "+ ProjectPhasesTasks.TaskNo + "للعميل" + customer.CustomerNameAr + "علي مشروع رقم" + project.ProjectNo;
                                        if (userObj.Mobile != null && userObj.Mobile != "")
                                        {
                                            var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                        }
                                    //}


                                    //if (UserNotifPriv.Contains(351))
                                    //{

                                    //    //SendMailNoti(ProTaskUpdated.Project.ProjectId, Desc, "اضافة مهمة جديدة", BranchId, UserId, ProjectPhasesTasks.UserId ?? 0);
                                    //    // SendMailFinishTask2(ProjectPhasesTasks, " مهمة جديدة", BranchId, UserId, Url, ImgUrl, 6,0);

                                    //}
                                }
                            //}

                        }
                        catch (Exception ex)
                        {

                        }


                        if(project.NewSetting==true)
                        {
                            var AllPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s =>s.IsDeleted == false && s.ProjectId == project.ProjectId ).ToList();
                            AllPhasesTasks = AllPhasesTasks.OrderBy(x => x.taskindex).ToList();

                            var parentindex = AllPhasesTasks.Where(s => s.PhaseTaskId == ProjectPhasesTasks.ParentId).FirstOrDefault()!.taskindex ?? 0;
                            var Counter = 0;
                            foreach (var item in AllPhasesTasks)
                            {
                                if (Counter == (parentindex + 1))
                                {
                                    item.taskindex = Counter + 1;
                                    Counter = Counter + 2; 
                                }
                                else
                                {
                                    item.taskindex = Counter;
                                    Counter = Counter + 1; 
                                }
                            }
                            ProjectPhasesTasks.taskindex = parentindex + 1;
                        }



                    }
                    else
                    {
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Make_sure_to_enter_new_important_data };
                    }

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة مهمة جديدة" + " باسم" + ProjectPhasesTasks.DescriptionAr;
                    _SystemAction.SaveAction("SaveNewProjectPhasesTasks_E", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedParm = ProjectPhasesTasks.PhaseTaskId };
                }
                catch (Exception ex)
                {
                    //var x = ex.Message;
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ المهمة";
                    _SystemAction.SaveAction("SaveNewProjectPhasesTasks_E", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }
            }
        }

        public GeneralMessage ConvertTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, string Url, string ImgUrl)
        {
            try
            {
                var ProTaskUpdated = _ProjectPhasesTasksRepository.GetById(ProjectPhasesTasks.PhaseTaskId);
                if (ProTaskUpdated != null)
                {
                    //var BranchIdOfUser = _UsersRepository.GetById(ProjectPhasesTasks.UserId??0);
                    var BranchIdOfUserOrDepartment = 0;
                    var project = _ProjectRepository.GetById(ProTaskUpdated.ProjectId ?? 0);

                    BranchIdOfUserOrDepartment = _UsersRepository.GetById(ProjectPhasesTasks.UserId ?? 0).BranchId ?? 0;

                    if (project != null)
                    {
                        BranchIdOfUserOrDepartment = project.BranchId;
                    }
                    var userFrom = ProTaskUpdated.UserId;
                    ProTaskUpdated.UserId = ProjectPhasesTasks.UserId;
                    ProTaskUpdated.IsConverted = 2;
                    ProTaskUpdated.UpdateUser = UserId;
                    ProTaskUpdated.BranchId = BranchIdOfUserOrDepartment;
                    ProTaskUpdated.UpdateDate = DateTime.Now;
                    ProTaskUpdated.convertReason_admin = ProjectPhasesTasks.convertReason_admin;
                    try
                    {
                        var branch = _BranchesRepository.GetById(BranchId);

                        if (ProTaskUpdated != null && ProTaskUpdated.ProjectId > 0)
                        {
                            var projct = _ProjectRepository.GetById((int)ProTaskUpdated.ProjectId);
                            if (projct != null && projct.CustomerId > 0)
                            {
                                var cust = _CustomerRepository.GetById((int)projct.CustomerId);

                                //get notification configuration users and description
                                var (usersList, descriptionFromConfig) = _projectService.GetNotificationRecipients(NotificationCode.Task_TransferAccepted, ProTaskUpdated.ProjectId.Value);
                                var description = "تحويل المهمة";

                                if (descriptionFromConfig != null && descriptionFromConfig != "")
                                    description = descriptionFromConfig;

                                //if no configuration send to emp and manager
                                if (usersList == null || usersList.Count == 0)
                                {
                                    var UserNotification = new Notification();
                                    UserNotification.ReceiveUserId = ProjectPhasesTasks.UserId;
                                    UserNotification.Name = description;
                                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                    UserNotification.SendUserId = UserId;
                                    UserNotification.Type = 1; // notification
                                    UserNotification.Description = " مهمه : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projct.ProjectNo + " للعميل " + cust.CustomerNameAr + " بعد تحويلها من " + _UsersRepository.GetById(userFrom ?? 0).FullName + " " + "فرع  " + branch.NameAr + "" + " السبب " + ProjectPhasesTasks.convertReason_admin;
                                    UserNotification.AllUsers = false;
                                    UserNotification.SendDate = DateTime.Now;
                                    UserNotification.ProjectId = ProjectPhasesTasks.ProjectId;
                                    UserNotification.TaskId = ProjectPhasesTasks.PhaseTaskId;
                                    UserNotification.AddUser = UserId;
                                    UserNotification.BranchId = BranchIdOfUserOrDepartment;
                                    UserNotification.IsHidden = false;
                                    UserNotification.NextTime = null;
                                    UserNotification.AddDate = DateTime.Now;
                                    _TaamerProContext.Notification.Add(UserNotification);
                                    _TaamerProContext.SaveChanges();

                                    _notificationService.sendmobilenotification(ProjectPhasesTasks.UserId ?? 0, description, "لديك مهمه جديدة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + cust.CustomerNameAr + " بعد تحويلها من " + _UsersRepository.GetById(userFrom ?? 0).FullName + " " + "فرع  " + branch.NameAr + "");

                                    if (projct != null && projct.MangerId != null && projct.MangerId != 0)
                                    {
                                        var managernot = new Notification();
                                        managernot = UserNotification;
                                        managernot.ReceiveUserId = projct.MangerId;
                                        managernot.NotificationId = 0;
                                        _TaamerProContext.Notification.Add(managernot);
                                        _TaamerProContext.SaveChanges();
                                        _notificationService.sendmobilenotification(projct.MangerId ?? 0, description, " لديك مهمه جديدة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + cust.CustomerNameAr + " بعد تحويلها من " + _UsersRepository.GetById(userFrom ?? 0).FullName + " " + "فرع  " + branch.NameAr + "");

                                    }
                                    SendMailFinishTask2(ProTaskUpdated, description, BranchId, UserId, Url, ImgUrl, 5, userFrom.Value, cust.CustomerNameAr ?? "", project.ProjectNo ?? "", project.MangerId ?? 0, _UsersRepository.GetById(project.MangerId ?? 0).FullNameAr ?? "");

                                    var userObj = _UsersRepository.GetById(ProjectPhasesTasks.UserId ?? 0);

                                    var NotStr = _UsersRepository.GetById(userFrom ?? 0).FullName + "  بعد تحويلها من  " + cust.CustomerNameAr + " للعميل  " + projct.ProjectNo + " علي مشروع رقم " + ProTaskUpdated.TaskNo + " رقم " + ProTaskUpdated.DescriptionAr + " لديك مهمه جديدة  ";
                                    if (userObj.Mobile != null && userObj.Mobile != "")
                                    {
                                        var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                    }

                                }
                                else
                                {
                                    foreach (var user in usersList)
                                    {
                                        var UserNotification = new Notification();
                                        UserNotification.ReceiveUserId = user;
                                        UserNotification.Name = description;
                                        UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                        UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                        UserNotification.SendUserId = UserId;
                                        UserNotification.Type = 1; // notification
                                        UserNotification.Description = " مهمه : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projct.ProjectNo + " للعميل " + cust.CustomerNameAr + " بعد تحويلها من " + _UsersRepository.GetById(userFrom ?? 0).FullName + " " + "فرع  " + branch.NameAr + "" + " السبب " + ProjectPhasesTasks.convertReason_admin;
                                        UserNotification.AllUsers = false;
                                        UserNotification.SendDate = DateTime.Now;
                                        UserNotification.ProjectId = ProjectPhasesTasks.ProjectId;
                                        UserNotification.TaskId = ProjectPhasesTasks.PhaseTaskId;
                                        UserNotification.AddUser = UserId;
                                        UserNotification.BranchId = BranchIdOfUserOrDepartment;
                                        UserNotification.IsHidden = false;
                                        UserNotification.NextTime = null;
                                        UserNotification.AddDate = DateTime.Now;
                                        _TaamerProContext.Notification.Add(UserNotification);
                                        _TaamerProContext.SaveChanges();

                                        _notificationService.sendmobilenotification(user, description, "لديك مهمه جديدة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + cust.CustomerNameAr + " بعد تحويلها من " + _UsersRepository.GetById(userFrom ?? 0).FullName + " " + "فرع  " + branch.NameAr + "");
                                        SendMailFinishTask2(ProTaskUpdated, description, BranchId, user, Url, ImgUrl, 5, userFrom.Value, cust.CustomerNameAr ?? "", project.ProjectNo ?? "", project.MangerId ?? 0, _UsersRepository.GetById(project.MangerId ?? 0).FullNameAr ?? "");

                                        var userObj = _UsersRepository.GetById(user);

                                        var NotStr = _UsersRepository.GetById(userFrom ?? 0).FullName + "  بعد تحويلها من  " + cust.CustomerNameAr + " للعميل  " + projct.ProjectNo + " علي مشروع رقم " + ProTaskUpdated.TaskNo + " رقم " + ProTaskUpdated.DescriptionAr + " لديك مهمه جديدة  ";
                                        if (userObj.Mobile != null && userObj.Mobile != "")
                                        {
                                            var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                        }
                                    }


                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تحويل مهمة" + ProTaskUpdated.DescriptionAr;
               _SystemAction.SaveAction("ConvertTask", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                #region
                //SaveOperationsForTask
                Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                Pro_TaskOperation.PhaseTaskId = ProTaskUpdated!.PhaseTaskId;
                Pro_TaskOperation.OperationName = "تم تحويل المهمة";
                Pro_TaskOperation.UserId = ProjectPhasesTasks.UserId;
                _SystemAction.SaveTaskOperations(Pro_TaskOperation, UserId, BranchId);
                #endregion

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.TranfareTask };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تحويل المهمة";
               _SystemAction.SaveAction("ConvertTask", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedTranfareTask };
            }
        }

        public GeneralMessage SaveTaskLongDesc(int ProjectPhaseTasksId, string taskLongDesc, int UserId, int BranchId)
        {
            var task = _ProjectPhasesTasksRepository.GetById(ProjectPhaseTasksId);
            if (task != null)
            {
                task.TaskLongDesc = taskLongDesc;
                task.AskDetails = 0;
                task.UpdateDate = DateTime.Now;
                task.UpdateUser = UserId;

                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = "حفظ تفاصيل المهمة" +task.DescriptionAr;
               _SystemAction.SaveAction("SaveTaskLongDesc", "ProjectPhasesTasksService", 1,Resources.General_SavedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            else
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ تفاصيل المهمة";
               _SystemAction.SaveAction("ConvertTask", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public GeneralMessage SaveTaskSetting(List<ProjectPhasesTasks> ProjectPhasesTasks, int UserId, int BranchId)
        {
            try
            {

                foreach (ProjectPhasesTasks phase in ProjectPhasesTasks)
                {

                    var ProPhasesTasksUpdated = _ProjectPhasesTasksRepository.GetById(phase.PhaseTaskId);
                    int? TempRem = 0;
                    if (ProPhasesTasksUpdated != null)
                    {
                        switch (phase.TimeType)
                        {
                            case 1:
                                TempRem = phase.TimeMinutes * 60;
                                break;
                            case 2:
                                TempRem = phase.TimeMinutes * 60 * 24;
                                break;
                        }

                        ProPhasesTasksUpdated.DescriptionAr = phase.DescriptionAr;
                        ProPhasesTasksUpdated.DescriptionEn = phase.DescriptionEn;
                        ProPhasesTasksUpdated.UserId = phase.UserId;
                        ProPhasesTasksUpdated.TimeMinutes = phase.TimeMinutes;
                        ProPhasesTasksUpdated.Remaining = TempRem;
                        ProPhasesTasksUpdated.Cost = phase.Cost;
                        ProPhasesTasksUpdated.TimeType = phase.TimeType;
                        ProPhasesTasksUpdated.IsUrgent = phase.IsUrgent;
                        ProPhasesTasksUpdated.TaskType = phase.TaskType;
                        //ProPhasesTasksUpdated.Notes = phase.Notes;
                        ProPhasesTasksUpdated.UpdateUser = UserId;
                        //ProPhasesTasksUpdated.PhasePriority = phase.PhasePriority;
                        //ProPhasesTasksUpdated.PlusTime = phase.PlusTime;
                        ProPhasesTasksUpdated.UpdateDate = DateTime.Now;
                    }
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تعديل مهمة علي سير قائم" ;
               _SystemAction.SaveAction("SaveTaskSetting", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully};

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تعديل مهمة علي سير قائم";
               _SystemAction.SaveAction("SaveTaskSetting", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }

        }


        public GeneralMessage ConvertUserTasks(int FromUserId, int ToUserId, int UserId, int BranchId)
        {
            try
            {
                var SettingProjUser = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == FromUserId);
                if (SettingProjUser != null && SettingProjUser.Count() > 0)
                {
                    foreach (var item in SettingProjUser)
                    {
                        item.UserId = ToUserId;
                        item.UpdateUser = UserId;
                        item.UpdateDate = DateTime.Now;
                    }
                }
                var FromUserTasksNotStarted = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.Type == 3 && s.UserId == FromUserId && s.Status != 4);
                if (FromUserTasksNotStarted != null && FromUserTasksNotStarted.Count() > 0)
                {
                    foreach (var item in FromUserTasksNotStarted)
                    {
                        var projectData = _ProjectRepository.GetProjectByIdSome("rtl",item.ProjectId??0).Result;
                        var userFrom = ToUserId;
                        item.UserId = ToUserId;
                        item.IsConverted = 2;
                        item.UpdateUser = UserId;
                        item.UpdateDate = DateTime.Now;

                        #region
                        //SaveOperationsForTask
                        Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                        Pro_TaskOperation.PhaseTaskId = item.PhaseTaskId;
                        Pro_TaskOperation.OperationName = "تم تحويل المهمة";
                        Pro_TaskOperation.Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        Pro_TaskOperation.UserId = item.UserId;
                        Pro_TaskOperation.BranchId = BranchId;
                        Pro_TaskOperation.AddUser = UserId;
                        Pro_TaskOperation.AddDate = DateTime.Now;
                        _TaamerProContext.Pro_TaskOperations.Add(Pro_TaskOperation);
                        #endregion


                        try
                        {
                            var branch = _BranchesRepository.GetById(BranchId);

                            var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(ToUserId).Result;
                            if (UserNotifPriv.Count() != 0)
                            {
                                if (UserNotifPriv.Contains(362))
                                {
                                    var UserNotification = new Notification();
                                    UserNotification.ReceiveUserId = ToUserId;
                                    UserNotification.Name = "مهمة محولة";
                                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar"));
                                    UserNotification.SendUserId = UserId;
                                    UserNotification.Type = 1; // notification
                                    UserNotification.Description = "مهمه : " + item.DescriptionAr + " رقم "+ item.TaskNo +  ":" + item.Notes + " علي مشروع رقم " + projectData?.ProjectNo + " للعميل " + projectData?.CustomerName_W + " بعد تحويلها من " + _UsersRepository.GetById(userFrom).FullName + " " + " فرع " + branch.NameAr + "";
                                    UserNotification.AllUsers = false;
                                    UserNotification.SendDate = DateTime.Now;
                                    UserNotification.ProjectId = item.ProjectId;
                                    UserNotification.TaskId = item.PhaseTaskId;
                                    UserNotification.AddUser = UserId;
                                    UserNotification.IsHidden = false;
                                    UserNotification.NextTime = null;
                                    UserNotification.AddDate = DateTime.Now;
                                    _TaamerProContext.Notification.Add(UserNotification);
                                    _notificationService.sendmobilenotification(ToUserId, Resources.General_Newtasks, "لديك مهمه جديدة : " + item.DescriptionAr + " رقم " + item.TaskNo + ":" + item.Notes + " علي مشروع رقم " + projectData?.ProjectNo + " للعميل " + projectData?.CustomerName_W + " بعد تحويلها من " + _UsersRepository.GetById(userFrom).FullName + " " + " فرع " + branch.NameAr + "");
                                }

                                if (UserNotifPriv.Contains(363))
                                {
                                    var userObj = _UsersRepository.GetById(ToUserId);

                                    var NotStr = _UsersRepository.GetById(userFrom).FullName + "  بعد تحويلها من  " + projectData?.CustomerName_W + " للعميل  " + projectData?.ProjectNo + " علي مشروع رقم " + item.TaskNo + " رقم " + item.DescriptionAr + " لديك مهمه جديدة  ";
                                    if (userObj.Mobile != null && userObj.Mobile != "")
                                    {
                                        var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                    }
                                }


                                //if (UserNotifPriv.Contains(361))
                                //{
                                //    var Desc = branch.NameAr + " فرع " + _UsersRepository.GetById(userFrom).FullName + " بعد تحويلها من " + projectData?.CustomerName_W + " للعميل " + projectData?.ProjectNo + " علي مشروع رقم " + item.DescriptionAr + " لديك مهمه جديدة : ";

                                //    SendMailNoti(item.Project.ProjectId, Desc, "اضافة مهمة جديدة", BranchId, UserId, item.UserId ?? 0);
                                //}
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                var userProject = _ProjectRepository.GetMatching(s => s.IsDeleted == false && s.MangerId == FromUserId && s.Status != 1);
                if (userProject != null && userProject.Count() > 0)
                {
                    foreach (var item in userProject)
                    {
                        item.UserId = ToUserId;
                        item.UpdateUser = UserId;
                        item.UpdateDate = DateTime.Now;
                    }
                }
                var userWorkOrder = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && (s.ExecutiveEng == FromUserId || s.ResponsibleEng == FromUserId) && (s.WOStatus == 1 || s.WOStatus == 2));
                if (userWorkOrder != null && userWorkOrder.Count() > 0)
                {
                    foreach (var item in userWorkOrder)
                    {
                        item.ExecutiveEng = ToUserId;
                        item.ResponsibleEng = ToUserId;
                        item.UpdateUser = UserId;
                        item.UpdateDate = DateTime.Now;
                    }
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تحويل مهمة مستخدم" ;
               _SystemAction.SaveAction("ConvertUserTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.TranfareTask };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تحويل مهمة المستخدم";
               _SystemAction.SaveAction("ConvertUserTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedTranfareTask };
            }
        }


        public GeneralMessage ConvertUserTasksSome(int PhasesTaskId, int FromUserId, int ToUserId, int UserId, int BranchId, string Url, string ImgUrl)
        {
            try
            {
                var FromUserTasksNotStarted = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.Type == 3 && s.PhaseTaskId == PhasesTaskId && s.UserId == FromUserId && s.Status != 4);
                if (FromUserTasksNotStarted != null && FromUserTasksNotStarted.Count() > 0)
                {
                    foreach (var item in FromUserTasksNotStarted)
                    {
                        var projectData = _ProjectRepository.GetProjectByIdSome("rtl", item.ProjectId ?? 0).Result;

                        var userFrom = FromUserId;
                        item.UserId = ToUserId;
                        item.IsConverted = 2;
                        item.UpdateUser = UserId;
                        item.UpdateDate = DateTime.Now;

                        #region
                        //SaveOperationsForTask
                        Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                        Pro_TaskOperation.PhaseTaskId = item.PhaseTaskId;
                        Pro_TaskOperation.OperationName = "تم تحويل المهمة";
                        Pro_TaskOperation.Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        Pro_TaskOperation.UserId = item.UserId;
                        Pro_TaskOperation.BranchId = BranchId;
                        Pro_TaskOperation.AddUser = UserId;
                        Pro_TaskOperation.AddDate = DateTime.Now;
                        _TaamerProContext.Pro_TaskOperations.Add(Pro_TaskOperation);
                        #endregion

                        try
                        {
                            var branch = _BranchesRepository.GetById(BranchId);

                            var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(ToUserId).Result;
                            if (UserNotifPriv.Count() != 0)
                            {
                                if (UserNotifPriv.Contains(362))
                                {
                                    var UserNotification = new Notification();
                                    UserNotification.ReceiveUserId = ToUserId;
                                    UserNotification.Name = "مهمة محولة";
                                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                    UserNotification.SendUserId = UserId;
                                    UserNotification.Type = 1; // notification
                                    UserNotification.Description = "مهمه : " + item.DescriptionAr + " رقم "+item.TaskNo + ":" + item.Notes + " علي مشروع رقم " + projectData?.ProjectNo + " للعميل " + projectData?.CustomerName_W + " بعد تحويلها من " + _UsersRepository.GetById(userFrom).FullName + " " + " فرع " + branch.NameAr + "";
                                    UserNotification.AllUsers = false;
                                    UserNotification.SendDate = DateTime.Now;
                                    UserNotification.ProjectId = item.ProjectId;
                                    UserNotification.TaskId = item.PhaseTaskId;
                                    UserNotification.AddUser = UserId;
                                    UserNotification.IsHidden = false;
                                    UserNotification.NextTime = null;
                                    UserNotification.AddDate = DateTime.Now;
                                    _TaamerProContext.Notification.Add(UserNotification);
                                    _notificationService.sendmobilenotification(ToUserId, Resources.General_Newtasks, "لديك مهمه جديدة : " + item.DescriptionAr+ " رقم " + item.TaskNo + ":" + item.Notes + " علي مشروع رقم " + projectData?.ProjectNo + " للعميل " + projectData?.CustomerName_W + " بعد تحويلها من " + _UsersRepository.GetById(userFrom).FullName + " " + " فرع " + branch.NameAr + "");

                                }


                                if (UserNotifPriv.Contains(363))
                                {
                                    var userObj = _UsersRepository.GetById(ToUserId);

                                    var NotStr = _UsersRepository.GetById(userFrom).FullName + "  بعد تحويلها من  " + projectData?.CustomerName_W + " للعميل  " + projectData?.ProjectNo + " علي مشروع رقم " +item.TaskNo+ " رقم " + item.DescriptionAr + " لديك مهمه جديدة  ";
                                    if (userObj.Mobile != null && userObj.Mobile != "")
                                    {
                                        var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                    }
                                }

                                if (UserNotifPriv.Contains(361))
                                {
                                    var Desc = branch.NameAr + " فرع " + _UsersRepository.GetById(userFrom).FullName + " بعد تحويلها من " + projectData?.CustomerName_W + " للعميل " + projectData?.ProjectNo + " علي مشروع رقم " +item.TaskNo+" رقم "+ item.DescriptionAr + " لديك مهمه جديدة : ";

                                    //SendMailNoti(item.Project.ProjectId, Desc, "اضافة مهمة جديدة", BranchId, UserId, ToUserId);
                                    SendMailFinishTask2(item, "اضافة مهمة جديدة", BranchId, UserId, Url, ImgUrl, 5, FromUserId, projectData.CustomerName??"", projectData.ProjectNo ?? "", projectData.MangerId??0, projectData.ProjectMangerName??"");
                                }
                            }

                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تحويل مهمة";
               _SystemAction.SaveAction("ConvertUserTasksSome", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.TranfareTask };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تحويل المهمة";
               _SystemAction.SaveAction("ConvertUserTasksSome", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedTranfareTask };
            }
        }

        public GeneralMessage ConvertMoreUserTasks(List<int> PhasesTaskIds, int FromUserId, int ToUserId, int UserId, int BranchId, string Url, string ImgUrl)
        {
            try
            {
                if (PhasesTaskIds.Count() > 0)
                {
                    foreach (var PhasesTaskId in PhasesTaskIds)
                    {


                        var FromUserTasksNotStarted = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.Type == 3 && s.PhaseTaskId == PhasesTaskId && s.UserId == FromUserId && s.Status != 4);
                        if (FromUserTasksNotStarted != null && FromUserTasksNotStarted.Count() > 0)
                        {
                            foreach (var item in FromUserTasksNotStarted)
                            {
                                var projectData = _ProjectRepository.GetProjectByIdSome("rtl", item.ProjectId ?? 0).Result;

                                var userFrom = FromUserId;
                                item.UserId = ToUserId;
                                item.IsConverted = 2;
                                item.UpdateUser = UserId;
                                item.UpdateDate = DateTime.Now;

                                #region
                                //SaveOperationsForTask
                                Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                                Pro_TaskOperation.PhaseTaskId = item.PhaseTaskId;
                                Pro_TaskOperation.OperationName = "تم تحويل المهمة";
                                Pro_TaskOperation.Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                Pro_TaskOperation.UserId = item.UserId;
                                Pro_TaskOperation.BranchId = BranchId;
                                Pro_TaskOperation.AddUser = UserId;
                                Pro_TaskOperation.AddDate = DateTime.Now;
                                _TaamerProContext.Pro_TaskOperations.Add(Pro_TaskOperation);
                                #endregion

                                try
                                {
                                    var branch = _BranchesRepository.GetById(BranchId);

                                    var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(ToUserId).Result;
                                    if (UserNotifPriv.Count() != 0)
                                    {
                                        if (UserNotifPriv.Contains(362))
                                        {
                                            var UserNotification = new Notification();
                                            UserNotification.ReceiveUserId = ToUserId;
                                            UserNotification.Name = "مهمة محولة";
                                            UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                            UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                            UserNotification.SendUserId = UserId;
                                            UserNotification.Type = 1; // notification
                                            UserNotification.Description = " مهمه : " + item.DescriptionAr + " رقم " + item.TaskNo + ":" + item.Notes + " علي مشروع رقم " + projectData?.ProjectNo + " للعميل " + projectData?.CustomerName_W + " بعد تحويلها من " + _UsersRepository.GetById(userFrom).FullName + " " + " فرع " + branch.NameAr + "";
                                            UserNotification.AllUsers = false;
                                            UserNotification.SendDate = DateTime.Now;
                                            UserNotification.ProjectId = item.ProjectId;
                                            UserNotification.TaskId = item.PhaseTaskId;
                                            UserNotification.AddUser = UserId;
                                            UserNotification.IsHidden = false;
                                            UserNotification.NextTime = null;
                                            UserNotification.AddDate = DateTime.Now;
                                            _TaamerProContext.Notification.Add(UserNotification);
                                            _notificationService.sendmobilenotification(ToUserId, Resources.General_Newtasks, "لديك مهمه جديدة : " + item.DescriptionAr + " رقم " + item.TaskNo + ":" + item.Notes + " علي مشروع رقم " + projectData?.ProjectNo + " للعميل " + projectData?.CustomerName_W + " بعد تحويلها من " + _UsersRepository.GetById(userFrom).FullName + " " + " فرع " + branch.NameAr + "");

                                        }


                                        if (UserNotifPriv.Contains(363))
                                        {
                                            var userObj = _UsersRepository.GetById(ToUserId);

                                            var NotStr = _UsersRepository.GetById(userFrom).FullName + "  بعد تحويلها من  " + projectData?.CustomerName_W + " للعميل  " + projectData?.ProjectNo + " علي مشروع رقم "+item.TaskNo + " رقم " + item.DescriptionAr + " لديك مهمه جديدة  ";
                                            if (userObj.Mobile != null && userObj.Mobile != "")
                                            {
                                                var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                            }
                                        }

                                        if (UserNotifPriv.Contains(361))
                                        {
                                            var Desc = branch.NameAr + " فرع " + _UsersRepository.GetById(userFrom).FullName + " بعد تحويلها من " + projectData?.CustomerName_W + " للعميل " + projectData?.ProjectNo + " علي مشروع رقم " +item.TaskNo +" رقم "+ item.DescriptionAr + " لديك مهمه جديدة : ";

                                            //SendMailNoti(item.Project.ProjectId, Desc, "اضافة مهمة جديدة", BranchId, UserId, ToUserId);
                                            SendMailFinishTask2(item, "اضافة مهمة جديدة", BranchId, UserId, Url, ImgUrl, 5, FromUserId, projectData.CustomerName??"", projectData.ProjectNo ?? "", projectData.MangerId ?? 0, projectData.ProjectMangerName ?? "");

                                        }
                                    }

                                }
                                catch (Exception)
                                {

                                }
                            }
                        }
                        _TaamerProContext.SaveChanges();
                    }
                }
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تحويل مهمة";
               _SystemAction.SaveAction("ConvertUserTasksSome", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.TranfareTask };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تحويل المهمة";
               _SystemAction.SaveAction("ConvertUserTasksSome", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedTranfareTask };
            }
        }

        public GeneralMessage RequestConvertTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, string Url, string ImgUrl)
        {
            try
            {
                var ProTaskUpdated = _ProjectPhasesTasksRepository.GetById(ProjectPhasesTasks.PhaseTaskId);
                if (ProTaskUpdated != null)
                {
                    var userFrom = ProTaskUpdated.UserId;
                    ProTaskUpdated.IsConverted = ProjectPhasesTasks.IsConverted;
                    ProTaskUpdated.convertReason = ProjectPhasesTasks.convertReason;
                }

                string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);


                        try
                        {
                    if (ProTaskUpdated != null && ProTaskUpdated.ProjectId > 0)
                    {
                        var projectData = _ProjectRepository.GetProjectByIdSome("rtl", ProTaskUpdated.ProjectId ?? 0).Result;
                        if (projectData != null && projectData.CustomerId > 0)
                        {

                            //get notification configuration users and description
                            var (usersList, descriptionFromConfig) = _projectService.GetNotificationRecipients(NotificationCode.Task_TransferRequested, ProTaskUpdated.ProjectId.Value);
                            var description = "لديك طلب تحويل المهمة";

                            if (descriptionFromConfig != null && descriptionFromConfig != "")
                                description = descriptionFromConfig;

                            //if no configuration send to emp and manager
                            if (usersList == null || usersList.Count == 0)
                            {
                                var UserNotification = new Notification();
                                UserNotification.ReceiveUserId = ProTaskUpdated.UserId;
                                UserNotification.Name = description;
                                UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                UserNotification.SendUserId = UserId;
                                UserNotification.Type = 1; // notification
                                UserNotification.Description = "طلب تحويل  : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName ?? "";
                                UserNotification.AllUsers = false;
                                UserNotification.SendDate = DateTime.Now;
                                UserNotification.ProjectId = ProTaskUpdated.ProjectId;
                                UserNotification.TaskId = ProTaskUpdated.PhaseTaskId;
                                UserNotification.AddUser = UserId;
                                UserNotification.IsHidden = false;
                                UserNotification.NextTime = null;
                                UserNotification.AddDate = DateTime.Now;
                                _TaamerProContext.Notification.Add(UserNotification);
                                _TaamerProContext.SaveChanges();
                                if (projectData != null && projectData.MangerId != null && projectData.MangerId != 0)
                                {
                                    var managernot = new Notification();
                                    managernot = UserNotification;
                                    managernot.ReceiveUserId = projectData.MangerId;
                                    managernot.NotificationId = 0;
                                    _TaamerProContext.Notification.Add(managernot);
                                    _TaamerProContext.SaveChanges();
                                    _notificationService.sendmobilenotification(projectData.MangerId ?? 0, description, "طلب تحويل المهمة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + "   السبب   " + ProjectPhasesTasks.convertReason);

                                }
                                _notificationService.sendmobilenotification(ProTaskUpdated.UserId ?? 0, description, "طلب تحويل المهمة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + "   السبب   " + ProjectPhasesTasks.convertReason);

                                var Desc = formattedDate + " بتاريخ " + ProTaskUpdated.DescriptionAr + " طلب تحويل المهمة ";
                                SendMailFinishTask2(ProTaskUpdated, description, BranchId, UserId, Url, ImgUrl, 3, 0, projectData.CustomerName ?? "", projectData.ProjectNo ?? "", projectData.MangerId ?? 0, projectData.ProjectMangerName ?? "");

                                var userObj = _UsersRepository.GetById(projectData.MangerId ?? 0);
                                var NotStr = formattedDate + " بتاريخ " + ProTaskUpdated.TaskNo + " رقم " + ProTaskUpdated.DescriptionAr + " طلب تحويل المهمة ";
                                if (userObj.Mobile != null && userObj.Mobile != "")
                                {
                                    var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                }

                                var userObj2 = _UsersRepository.GetById(ProTaskUpdated.UserId ?? 0);
                                if (userObj2.Mobile != null && userObj2.Mobile != "")
                                {
                                    var result = _userNotificationPrivilegesService.SendSMS(userObj2.Mobile, NotStr, UserId, BranchId);
                                }
                            }

                            else
                            {
                                foreach (var user in usersList)
                                {
                                    var UserNotification = new Notification();
                                    UserNotification.ReceiveUserId = user;
                                    UserNotification.Name = description;
                                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                    UserNotification.SendUserId = UserId;
                                    UserNotification.Type = 1; // notification
                                    UserNotification.Description = "طلب تحويل  : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName ?? "";
                                    UserNotification.AllUsers = false;
                                    UserNotification.SendDate = DateTime.Now;
                                    UserNotification.ProjectId = ProTaskUpdated.ProjectId;
                                    UserNotification.TaskId = ProTaskUpdated.PhaseTaskId;
                                    UserNotification.AddUser = UserId;
                                    UserNotification.IsHidden = false;
                                    UserNotification.NextTime = null;
                                    UserNotification.AddDate = DateTime.Now;
                                    _TaamerProContext.Notification.Add(UserNotification);
                                    _TaamerProContext.SaveChanges();

                                    _notificationService.sendmobilenotification(user, description, "طلب تحويل المهمة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + "   السبب   " + ProjectPhasesTasks.convertReason);

                                    var Desc = formattedDate + " بتاريخ " + ProTaskUpdated.DescriptionAr + " طلب تحويل المهمة ";
                                    SendMailFinishTask2(ProTaskUpdated, description, BranchId, user, Url, ImgUrl, 3, 0, projectData.CustomerName ?? "", projectData.ProjectNo ?? "", projectData.MangerId ?? 0, projectData.ProjectMangerName ?? "");

                                    var userObj = _UsersRepository.GetById(user);
                                    var NotStr = formattedDate + " بتاريخ " + ProTaskUpdated.TaskNo + " رقم " + ProTaskUpdated.DescriptionAr + " طلب تحويل المهمة ";
                                    if (userObj.Mobile != null && userObj.Mobile != "")
                                    {
                                        var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, user, BranchId);
                                    }
                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {

                }


                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "طلب تحويل مهمة";
               _SystemAction.SaveAction("RequestConvertTask", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                #region
                //SaveOperationsForTask
                Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                Pro_TaskOperation.PhaseTaskId = ProTaskUpdated!.PhaseTaskId;
                Pro_TaskOperation.OperationName = "طلب تحويل المهمة";
                _SystemAction.SaveTaskOperations(Pro_TaskOperation, UserId, BranchId);
                #endregion
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.askTranfareSucc };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في طلب تحويل مهمة";
               _SystemAction.SaveAction("RequestConvertTask", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.askTranfareFailed };
            }
        }
        public GeneralMessage SetUserTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, string Lang, int BranchId)
        {
            try
            {
                var ProTaskUpdated = _ProjectPhasesTasksRepository.GetById(ProjectPhasesTasks.PhaseTaskId);
                if (ProTaskUpdated != null)
                {
                    var project = _ProjectRepository.GetById(ProTaskUpdated.ProjectId??0);
                    var codePrefix = "TSK#";
                    var Value = _ProjectPhasesTasksRepository.GenerateNextTaskNumber(BranchId, codePrefix, 0).Result;
                    var NewValue = string.Format("{0:000000}", Value);
                    if (codePrefix != "")
                    {
                        NewValue = codePrefix + NewValue;
                    }
                    ProjectPhasesTasks.TaskNo = NewValue;

                    ProTaskUpdated.UserId = ProjectPhasesTasks.UserId;
                    ProTaskUpdated.UpdateUser = UserId;
                    ProTaskUpdated.UpdateDate = DateTime.Now;
                    ProTaskUpdated.TaskNo = ProjectPhasesTasks.TaskNo;


                    var branch = _BranchesRepository.GetById(BranchId);
                    var customer = _CustomerRepository.GetById(project.CustomerId ?? 0);
                    var BranchIdOfUserOrDepartment = 0;
                    if (ProjectPhasesTasks.UserId != null)
                    {
                        BranchIdOfUserOrDepartment = _UsersRepository.GetById(ProjectPhasesTasks.UserId ?? 0).BranchId ?? 0;
                    }
                    else
                    {
                        BranchIdOfUserOrDepartment = _departmentRepository.GetDepartmentbyid(ProjectPhasesTasks.DepartmentId ?? 0).Result?.FirstOrDefault()?.BranchId ?? 0;
                    }
                    if (project != null)
                    {
                        BranchIdOfUserOrDepartment = project.BranchId;
                    }
                    try
                    {
                        if (ProjectPhasesTasks.UserId != 0 && ProjectPhasesTasks.UserId != null)
                        {
                            var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(ProjectPhasesTasks.UserId ?? 0).Result;
                            if (UserNotifPriv.Count() != 0)
                            {
                                if (UserNotifPriv.Contains(352))
                                {
                                    var UserNotification = new Notification();
                                    UserNotification.ReceiveUserId = ProjectPhasesTasks.UserId;
                                    UserNotification.Name = Resources.General_Newtasks;
                                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                    UserNotification.SendUserId = UserId;
                                    UserNotification.Type = 1; // notification
                                    UserNotification.Description = "لديك مهمه جديدة : " + ProjectPhasesTasks.DescriptionAr + " رقم " + ProjectPhasesTasks.TaskNo + " علي مشروع رقم " + project.ProjectNo + " للعميل " + customer.CustomerNameAr + " " + " فرع " + branch.NameAr + "";
                                    UserNotification.AllUsers = false;
                                    UserNotification.SendDate = DateTime.Now;
                                    UserNotification.ProjectId = ProjectPhasesTasks.ProjectId;
                                    UserNotification.TaskId = ProjectPhasesTasks.PhaseTaskId;
                                    UserNotification.AddUser = UserId;
                                    UserNotification.AddDate = DateTime.Now;
                                    UserNotification.BranchId = BranchIdOfUserOrDepartment;
                                    UserNotification.IsHidden = false;
                                    UserNotification.NextTime = null;
                                    _TaamerProContext.Notification.Add(UserNotification);
                                    _notificationService.sendmobilenotification(ProjectPhasesTasks.UserId ?? 0, Resources.General_Newtasks, "لديك مهمه جديدة : " + ProjectPhasesTasks.DescriptionAr + " علي مشروع رقم " + project.ProjectNo + " للعميل " + customer.CustomerNameAr + " " + " فرع " + branch.NameAr + "");
                                }


                                if (UserNotifPriv.Contains(353))
                                {
                                    var userObj = _UsersRepository.GetById(ProjectPhasesTasks.UserId ?? 0);

                                    //var NotStr = customer.CustomerNameAr + " للعميل  " + project.ProjectNo + " علي مشروع رقم " + ProjectPhasesTasks.DescriptionAr + " لديك مهمه جديدة  ";
                                    var NotStr = "لديك مهمة جديدة" + ProjectPhasesTasks.DescriptionAr + " رقم " + ProjectPhasesTasks.TaskNo + "للعميل" + customer.CustomerNameAr + "علي مشروع رقم" + project.ProjectNo;
                                    if (userObj.Mobile != null && userObj.Mobile != "")
                                    {
                                        var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                    }
                                }


                                if (UserNotifPriv.Contains(351))
                                {

                                    //SendMailNoti(ProTaskUpdated.Project.ProjectId, Desc, "اضافة مهمة جديدة", BranchId, UserId, ProjectPhasesTasks.UserId ?? 0);
                                    // SendMailFinishTask2(ProjectPhasesTasks, " مهمة جديدة", BranchId, UserId, Url, ImgUrl, 6,0);

                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }
                _TaamerProContext.SaveChanges();


                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "سحب مهمة";
                _SystemAction.SaveAction("SetUserTask", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في سحب مهمة";
                _SystemAction.SaveAction("SetUserTask", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage PlayPauseTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, string Lang, int BranchId)
        {
            try
            {
                if (ProjectPhasesTasks.Status == 2)
                {
                   


                    var taskPredecessor = _TasksDependencyRepository.GetMatching(s => s.IsDeleted == false && s.SuccessorId == ProjectPhasesTasks.PhaseTaskId).ToList(); // get task predecessor
                    foreach (var item in taskPredecessor)
                    {
                        var task = _ProjectPhasesTasksRepository.GetTaskById(item.PredecessorId ?? 0, Lang, UserId).Result;
                        if (task.Status == 1 || task.Status == 2)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = Resources.General_SavedFailed;
                           _SystemAction.SaveAction("PlayPauseTask", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Cantruntask , ReturnedStrExtra= task.UserName, ReturnedStrExtra2= task.DescriptionAr };
                        }
                    }
                }
                var ProTaskUpdated = _ProjectPhasesTasksRepository.GetById(ProjectPhasesTasks.PhaseTaskId);

                if (ProTaskUpdated != null)
                {
                    if (ProTaskUpdated.IsTemp == true && ProjectPhasesTasks.Status == 2)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDateV = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNoteV = "المهمة متوقفة لشرط إداري";
                        _SystemAction.SaveAction("FinishTask", "ProjectPhasesTasksService", 1, "المهمة متوقفة لشرط إداري", "", "", ActionDateV, UserId, BranchId, ActionNoteV, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ActionNoteV };
                    }

                    var project = _ProjectRepository.GetById(ProTaskUpdated.ProjectId??0);
                    if (project != null)
                    {
                        project.MotionProject = 1;
                        project.MotionProjectDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        project.MotionProjectNote = "تشغيل/ايقاف مهمة";
                    }

                    ProTaskUpdated.Status = ProjectPhasesTasks.Status;
                    if (ProTaskUpdated.Status == 3)
                    {
                        ProTaskUpdated.StopCount += 1;
                    }
                    ProTaskUpdated.UpdateUser = UserId;
                    ProTaskUpdated.UpdateDate = DateTime.Now;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تشغيل /ايقاف المهمة" + ProTaskUpdated.DescriptionAr;
               _SystemAction.SaveAction("PlayPauseTask", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------


                #region
                //SaveOperationsForTask
                Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                Pro_TaskOperation.PhaseTaskId = ProTaskUpdated.PhaseTaskId;
                if (ProTaskUpdated.Status == 2) Pro_TaskOperation.OperationName = "تشغيل المهمة";
                else Pro_TaskOperation.OperationName = "إيقاف المهمة";
                _SystemAction.SaveTaskOperations(Pro_TaskOperation, UserId, BranchId);
                #endregion

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = ProjectPhasesTasks.Status == 2 ? Resources.Pro_taskStart : Resources.Pro_stopTask };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تشغيل /ايقاف المهمة";
               _SystemAction.SaveAction("PlayPauseTask", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ProjectPhasesTasks.Status == 2 ? Resources.taskDontrun : Resources.taskDontstop };
            }
        }
        public GeneralMessage FinishTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, string URL, string ImgUrl)
        {
            try
            {

                var ProTaskUpdated = _ProjectPhasesTasksRepository.GetById(ProjectPhasesTasks.PhaseTaskId);
                if (ProTaskUpdated != null)
                {
                    if (ProTaskUpdated.IsTemp == true && ProjectPhasesTasks.Status == 4)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDateV = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNoteV = "المهمة متوقفة لشرط إداري";
                        _SystemAction.SaveAction("FinishTask", "ProjectPhasesTasksService", 1, "المهمة متوقفة لشرط إداري", "", "", ActionDateV, UserId, BranchId, ActionNoteV, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ActionNoteV };
                    }
                    if (ProTaskUpdated.Managerapproval==1 || ProTaskUpdated.Managerapproval == 2) 
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDateV = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNoteV = "هذه المهمة مشروطة بموافقة المدير ؛ ولا يمكنك إنهاء المهمة إلا بعد أن تتم مراجعتها من صاحب الصلاحية";
                       _SystemAction.SaveAction("FinishTask", "ProjectPhasesTasksService", 1, "هذه المهمة مشروطة بموافقة المدير ؛ ولا يمكنك إنهاء المهمة إلا بعد أن تتم مراجعتها من صاحب الصلاحية", "", "", ActionDateV, UserId, BranchId, ActionNoteV, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ActionNoteV };
                    }
                    else
                    {
                        if (ProTaskUpdated.Status != 1)
                        {
                            ProTaskUpdated.Status = ProjectPhasesTasks.Status;
                            ProTaskUpdated.ExecPercentage = ProjectPhasesTasks.ExecPercentage;
                            if (ProjectPhasesTasks.Status == 4)
                            {
                                ProTaskUpdated.EndDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                            }
                            ProTaskUpdated.UpdateUser = UserId;
                            ProTaskUpdated.UpdateDate = DateTime.Now;
                        }
                        else
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = "لا يمكنك اعطاء نسبة لمهمة لم تبدأ";
                           _SystemAction.SaveAction("FinishTask", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                        }
                    }
                    string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                    if (ProTaskUpdated != null && ProTaskUpdated.ProjectId > 0  && ProjectPhasesTasks.Status==4)
                    {
                        var projectData = _ProjectRepository.GetProjectByIdSome("rtl", ProTaskUpdated.ProjectId ?? 0).Result;
                        if (projectData != null && projectData.CustomerId > 0)
                        {
                            var (usersList, descriptionFromConfig) = _projectService.GetNotificationRecipients(NotificationCode.Task_Completed, ProTaskUpdated.ProjectId.Value);
                            var description = "انهاء المهمة";

                            if (descriptionFromConfig != null && descriptionFromConfig != "")
                                description = descriptionFromConfig;

                            if (usersList == null || usersList.Count == 0)
                            {

                                var UserNotification = new Notification();
                                UserNotification.ReceiveUserId = ProTaskUpdated.UserId;
                                UserNotification.Name = description;
                                UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                UserNotification.SendUserId = UserId;
                                UserNotification.Type = 1; // notification
                                UserNotification.Description = "تم انهاء المهمة  : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName ?? "";
                                UserNotification.AllUsers = false;
                                UserNotification.SendDate = DateTime.Now;
                                UserNotification.ProjectId = ProTaskUpdated.ProjectId;
                                UserNotification.TaskId = ProTaskUpdated.PhaseTaskId;
                                UserNotification.AddUser = UserId;
                                UserNotification.IsHidden = false;
                                UserNotification.NextTime = null;
                                UserNotification.AddDate = DateTime.Now;
                                _TaamerProContext.Notification.Add(UserNotification);
                                _TaamerProContext.SaveChanges();
                                if (projectData.MangerId != null)
                                {
                                    var managernot = new Notification();
                                    managernot = UserNotification;
                                    managernot.ReceiveUserId = projectData.MangerId;
                                    managernot.NotificationId = 0;
                                    _TaamerProContext.Add(managernot);
                                    _TaamerProContext.SaveChanges();
                                    _notificationService.sendmobilenotification(projectData.MangerId ?? 0, " انهاء المهمة ", "  تم انهاء المهمة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + "");

                                }

                                _notificationService.sendmobilenotification(ProTaskUpdated.UserId ?? 0, " انهاء المهمة ", "  تم انهاء المهمة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + "");


                                var Desc = formattedDate + " بتاريخ " + ProTaskUpdated.TaskNo + " رقم " + ProTaskUpdated.DescriptionAr + "  انهاء المهمة ";

                                SendMailFinishTask2(ProTaskUpdated, description, BranchId, UserId, URL, ImgUrl, 1, 0, projectData.CustomerName ?? "", projectData.ProjectNo ?? "", projectData.MangerId ?? 0, projectData.ProjectMangerName ?? "");

                            }
                            else
                            {
                                foreach (var user in usersList)
                                {
                                    var UserNotification = new Notification();
                                    UserNotification.ReceiveUserId = user;
                                    UserNotification.Name = description;
                                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                    UserNotification.SendUserId = UserId;
                                    UserNotification.Type = 1; // notification
                                    UserNotification.Description = "تم انهاء المهمة  : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName ?? "";
                                    UserNotification.AllUsers = false;
                                    UserNotification.SendDate = DateTime.Now;
                                    UserNotification.ProjectId = ProTaskUpdated.ProjectId;
                                    UserNotification.TaskId = ProTaskUpdated.PhaseTaskId;
                                    UserNotification.AddUser = UserId;
                                    UserNotification.IsHidden = false;
                                    UserNotification.NextTime = null;
                                    UserNotification.AddDate = DateTime.Now;
                                    _TaamerProContext.Notification.Add(UserNotification);
                                    _TaamerProContext.SaveChanges();
                                    _notificationService.sendmobilenotification(user , description, "  تم انهاء المهمة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + "");
                                    var Desc = formattedDate + " بتاريخ " + ProTaskUpdated.TaskNo + " رقم " + ProTaskUpdated.DescriptionAr + "  انهاء المهمة ";

                                    SendMailFinishTask2(ProTaskUpdated, description, BranchId, user, URL, ImgUrl, 1, 0, projectData.CustomerName ?? "", projectData.ProjectNo ?? "", projectData.MangerId ?? 0, projectData.ProjectMangerName ?? "");

                                }
                            }
                        }

                    }

                }

                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "انهاءالمهمة" + ProTaskUpdated.DescriptionAr ;
               _SystemAction.SaveAction("FinishTask", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);

                #region
                //SaveOperationsForTask
                Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                Pro_TaskOperation.PhaseTaskId = ProTaskUpdated.PhaseTaskId;
                if (ProTaskUpdated.Status == 4) Pro_TaskOperation.OperationName = "انهاء المهمة";
                else Pro_TaskOperation.OperationName = "اعطاء نسب للمهمة";
                _SystemAction.SaveTaskOperations(Pro_TaskOperation, UserId, BranchId);
                #endregion
                //-----------------------------------------------------------------------------------------------------------------
                //SendMailFinishTask(ProTaskUpdated, BranchId, UserId);
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.finishTask };

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في انهاءالمهمة";
               _SystemAction.SaveAction("FinishTask", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.failedfinishTask };
            }
        }
        public GeneralMessage FinishTaskManager(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, string URL, string ImgUrl)
        {
            try
            {

                var ProTaskUpdated = _ProjectPhasesTasksRepository.GetById(ProjectPhasesTasks.PhaseTaskId);
                if (ProTaskUpdated != null)
                {

                    if (ProTaskUpdated.IsTemp == true && ProjectPhasesTasks.Status == 2)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDateV = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNoteV = "المهمة متوقفة لشرط إداري";
                        _SystemAction.SaveAction("FinishTask", "ProjectPhasesTasksService", 1, "المهمة متوقفة لشرط إداري", "", "", ActionDateV, UserId, BranchId, ActionNoteV, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ActionNoteV };
                    }
                    else
                    {
                        if (ProTaskUpdated.Status != 1)
                        {
                            ProTaskUpdated.Status = ProjectPhasesTasks.Status;
                            ProTaskUpdated.ExecPercentage = ProjectPhasesTasks.ExecPercentage;
                            if (ProjectPhasesTasks.Status == 4)
                            {
                                ProTaskUpdated.EndDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                            }
                            ProTaskUpdated.UpdateUser = UserId;
                            ProTaskUpdated.UpdateDate = DateTime.Now;
                        }
                        else
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = "لا يمكنك اعطاء نسبة لمهمة لم تبدأ";
                            _SystemAction.SaveAction("FinishTask", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                        }
                    }

                }

                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "انهاءالمهمة" + ProTaskUpdated.DescriptionAr;
                _SystemAction.SaveAction("FinishTask", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                //SendMailFinishTask(ProTaskUpdated, BranchId, UserId);
                #region
                //SaveOperationsForTask
                Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                Pro_TaskOperation.PhaseTaskId = ProTaskUpdated.PhaseTaskId;
                if (ProTaskUpdated.Status == 4) Pro_TaskOperation.OperationName = "انهاء المهمة";
                else Pro_TaskOperation.OperationName = "اعطاء نسب للمهمة";
                _SystemAction.SaveTaskOperations(Pro_TaskOperation, UserId, BranchId);
                #endregion

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.finishTask };

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في انهاءالمهمة";
                _SystemAction.SaveAction("FinishTask", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.failedfinishTask };
            }
        }

        public GeneralMessage FinishTaskCheck(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, string URL, string ImgUrl)
        {
            try
            {

                var ProTaskUpdated = _ProjectPhasesTasksRepository.GetById(ProjectPhasesTasks.PhaseTaskId);
                var projectData = _ProjectRepository.GetProjectByIdSome("rtl", ProTaskUpdated.ProjectId ?? 0).Result;

                if (ProTaskUpdated != null)
                {

                    if (ProTaskUpdated.Status != 1)
                    {

                        var ParentPhaseForTask = _ProjectPhasesTasksRepository.GetById((int)ProTaskUpdated.ParentId);
                        if (ParentPhaseForTask != null)
                        {
                            var AllphasesProject = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == ProTaskUpdated.ProjectId);


                            //var RemainingTasks2 = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.Type == 3 && s.ParentId == ParentPhaseForTask.PhaseTaskId && s.PhaseTaskId != ProTaskUpdated.PhaseTaskId && s.Status != 4);
                            var RemainingTasks = AllphasesProject.Where(s => s.Type == 3 && s.ParentId == ParentPhaseForTask.PhaseTaskId && s.PhaseTaskId != ProTaskUpdated.PhaseTaskId && s.Status != 4).ToList();

                            var RemainingCheck = 0;
                            //var MainPhasesInPRoject2 = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.Type == 1 && s.ProjectId == ProTaskUpdated.ProjectId);
                            var MainPhasesInPRoject = AllphasesProject.Where(s => s.Type == 1).ToList();

                            if (MainPhasesInPRoject != null && MainPhasesInPRoject.Count() != 0)
                            {
                                foreach (var item in MainPhasesInPRoject)
                                {
                                    //var SubPhasesInProject2 = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.Type == 2 && s.ParentId == item.PhaseTaskId);
                                    var SubPhasesInProject = AllphasesProject.Where(s => s.Type == 2 && s.ParentId == item.PhaseTaskId).ToList();

                                    if (SubPhasesInProject != null && SubPhasesInProject.Count() != 0)
                                    {
                                        foreach (var itemSub in SubPhasesInProject)
                                        {
                                            //var TasksInProject2 = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.Type == 3 && s.ParentId == itemSub.PhaseTaskId && s.PhaseTaskId != itemSub.PhaseTaskId && s.Status != 4);
                                            var TasksInProject = AllphasesProject.Where(s => s.Type == 3 && s.ParentId == itemSub.PhaseTaskId && s.PhaseTaskId != itemSub.PhaseTaskId && s.Status != 4).ToList();

                                            if (TasksInProject != null && TasksInProject.Count() != 0)
                                            {
                                                RemainingCheck = 1;
                                                break;
                                            }
                                        }
                                    }


                                }
                            }




                            var checkin = 0;
                            if (RemainingTasks != null && RemainingTasks.Count() == 0)
                            {
                                string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                ParentPhaseForTask.Active = false;

                                var ActiveSubPhaseProject = _ProjectRepository.GetMatching(s => s.ActiveSubPhaseId == ParentPhaseForTask.PhaseTaskId).FirstOrDefault();
                                if (ActiveSubPhaseProject != null)
                                {
                                    ActiveSubPhaseProject.ActiveSubPhaseId = null;

                                    //InsertNoti

                                    ProjectArchivesRe Pro = new ProjectArchivesRe();
                                    Pro.ProjectId = ActiveSubPhaseProject.ProjectId;
                                    Pro.ReDate = formattedDate;
                                    Pro.AddDate = DateTime.Now;
                                    Pro.Re_TypeID = 2;
                                    Pro.Re_PhasesTaskId = ParentPhaseForTask.PhaseTaskId;
                                    Pro.Re_TypeName = "انتهاء مرحلة فرعية";
                                    _TaamerProContext.ProjectArchivesRe.Add(Pro);



                                    var result =_projectService.GetNotificationRecipients(NotificationCode.Project_SubStageCompleted, ActiveSubPhaseProject.ProjectId);

                                    string desc = result.Description;
                                    if (string.IsNullOrWhiteSpace(desc))
                                    {
                                        desc = formattedDate + " بتاريخ " + ParentPhaseForTask.DescriptionAr + " انتهاء مرحلة فرعية ";
                                    }

                                    var users = result.Users;
                                    if (users == null || !users.Any())
                                    {
                                        var managerId = projectData.MangerId ?? 0;
                                        if (managerId > 0)
                                        {
                                            users = new List<int> { managerId };
                                        }
                                    }

                                    foreach (var userId in users)
                                    {
                                        try
                                        {
                                            var UserNotification = new Notification();
                                            UserNotification.ReceiveUserId = userId;
                                            UserNotification.Name = desc;// "انتهاء مهمة";
                                            UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                            UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                            UserNotification.SendUserId = 1;
                                            UserNotification.Type = 1; // notification
                                            UserNotification.Description = "تم انتهاء : " + ProTaskUpdated.DescriptionAr + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + (projectData?.CustomerName ?? "") + " " + " فرع " + _BranchesRepository.GetById(BranchId).NameAr + "";
                                            UserNotification.AllUsers = false;
                                            UserNotification.SendDate = DateTime.Now;
                                            UserNotification.ProjectId = ProTaskUpdated.ProjectId;
                                            UserNotification.TaskId = ProTaskUpdated.PhaseTaskId;
                                            UserNotification.AddUser = UserId;
                                            UserNotification.AddDate = DateTime.Now;
                                            UserNotification.BranchId = BranchId;
                                            UserNotification.IsHidden = false;
                                            UserNotification.NextTime = null;
                                            _TaamerProContext.Notification.Add(UserNotification);
                                            _notificationService.sendmobilenotification(userId , Resources.finishTask, "تم انتهاء المهمة : " + ProTaskUpdated.DescriptionAr + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + (projectData?.ContractorName ?? "") + " " + " فرع " + _BranchesRepository.GetById(BranchId).NameAr + "");

                                            SendMailNoti(ActiveSubPhaseProject.ProjectId, desc, desc, BranchId, UserId, userId);
                                        }
                                        catch (Exception ex)
                                        {
                                            // Log if needed
                                            Console.WriteLine($"Mail failed for user {userId}: {ex.Message}");
                                        }

                                    }

                                    //End



                                    if (RemainingCheck == 0)
                                    {
                                        var ParentSub = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.PhaseTaskId == ParentPhaseForTask.ParentId).FirstOrDefault();
                                        if (ParentSub != null)
                                        {
                                            ///////////////////// finish active Mainphase for project
                                            checkin = 1;
                                            ActiveSubPhaseProject.ActiveMainPhaseId = null;

                                            //InsertNoti
                                            //string formattedDate2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                                            ProjectArchivesRe Pro2 = new ProjectArchivesRe();
                                            Pro2.ProjectId = ActiveSubPhaseProject.ProjectId;
                                            Pro2.ReDate = formattedDate;
                                            Pro2.Re_TypeID = 2;
                                            Pro2.AddDate = DateTime.Now;
                                            Pro2.Re_PhasesTaskId = ParentSub.PhaseTaskId;
                                            Pro2.Re_TypeName = "انتهاء مرحلة رئيسية";
                                            _TaamerProContext.ProjectArchivesRe.Add(Pro2);



                                            var result2 =_projectService.GetNotificationRecipients(NotificationCode.Project_MainStageCompleted, ActiveSubPhaseProject.ProjectId);

                                            string desc2 = string.IsNullOrWhiteSpace(result2.Description)
                                                ? formattedDate + " بتاريخ " + ParentPhaseForTask.DescriptionAr + " انتهاء مرحلة رئيسية"
                                                : result2.Description;

                                            var users2 = result2.Users;
                                            if (users2 == null || !users2.Any())
                                            {
                                                var managerId = projectData.MangerId ?? 0;
                                                if (managerId > 0)
                                                    users2 = new List<int> { managerId };
                                            }

                                            foreach (var userId in users2)
                                            {
                                                try
                                                {
                                                    var UserNotification = new Notification();
                                                    UserNotification.ReceiveUserId = userId;
                                                    UserNotification.Name = desc2;
                                                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                                    UserNotification.SendUserId = 1;
                                                    UserNotification.Type = 1; // notification
                                                    UserNotification.Description = desc2;// "تم انتهاء : " + ProTaskUpdated.DescriptionAr + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + (projectData?.CustomerName ?? "") + " " + " فرع " + _BranchesRepository.GetById(BranchId).NameAr + "";
                                                    UserNotification.AllUsers = false;
                                                    UserNotification.SendDate = DateTime.Now;
                                                    UserNotification.ProjectId = ProTaskUpdated.ProjectId;
                                                    UserNotification.TaskId = ProTaskUpdated.PhaseTaskId;
                                                    UserNotification.AddUser = UserId;
                                                    UserNotification.AddDate = DateTime.Now;
                                                    UserNotification.BranchId = BranchId;
                                                    UserNotification.IsHidden = false;
                                                    UserNotification.NextTime = null;
                                                    _TaamerProContext.Notification.Add(UserNotification);
                                                    _notificationService.sendmobilenotification(userId , Resources.finishTask, "تم انتهاء المهمة : " + ProTaskUpdated.DescriptionAr + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + (projectData?.ContractorName ?? "") + " " + " فرع " + _BranchesRepository.GetById(BranchId).NameAr + "");

                                                    SendMailNoti(ActiveSubPhaseProject.ProjectId, desc2, "انتهاء مرحلة رئيسية", BranchId, UserId, userId);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine($"Mail failed for user {userId}: {ex.Message}");
                                                }

                                                var userObj2 = _UsersRepository.GetById(userId);
                                                if (!string.IsNullOrEmpty(userObj2?.Mobile))
                                                {
                                                    var NotStr2 = formattedDate + " بتاريخ " + ParentPhaseForTask.DescriptionAr + " انتهاء مرحلة رئيسية";
                                                    var result_2 = _userNotificationPrivilegesService.SendSMS(userObj2.Mobile, NotStr2, UserId, BranchId);
                                                }
                                            }

                                        }
                                    }


                                }
                                if (checkin == 0)
                                {
                                    ///////////////////// finish active Mainphase for project
                                    var ActiveMainPhaseProject = _ProjectRepository.GetMatching(s => s.ActiveMainPhaseId == ParentPhaseForTask.PhaseTaskId).FirstOrDefault();
                                    if (ActiveMainPhaseProject != null)
                                    {
                                        ActiveMainPhaseProject.ActiveMainPhaseId = null;

                                        //InsertNoti
                                        //string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                                        ProjectArchivesRe Pro = new ProjectArchivesRe();
                                        Pro.ProjectId = ActiveMainPhaseProject.ProjectId;
                                        Pro.ReDate = formattedDate;
                                        Pro.Re_TypeID = 2;
                                        Pro.AddDate = DateTime.Now;
                                        Pro.Re_PhasesTaskId = ParentPhaseForTask.PhaseTaskId;
                                        Pro.Re_TypeName = "انتهاء مرحلة رئيسية";
                                        _TaamerProContext.ProjectArchivesRe.Add(Pro);



                                        var result2 =_projectService.GetNotificationRecipients(NotificationCode.Project_MainStageCompleted, ActiveSubPhaseProject.ProjectId);

                                        string desc2 = string.IsNullOrWhiteSpace(result2.Description)
                                            ? formattedDate + " بتاريخ " + ParentPhaseForTask.DescriptionAr + " انتهاء مرحلة رئيسية"
                                            : result2.Description;

                                        var users2 = result2.Users;
                                        if (users2 == null || !users2.Any())
                                        {
                                            var managerId = projectData.MangerId ?? 0;
                                            if (managerId > 0)
                                                users2 = new List<int> { managerId };
                                        }

                                        foreach (var userId in users2)
                                        {
                                            try
                                            {
                                                var UserNotification = new Notification();
                                                UserNotification.ReceiveUserId = userId;
                                                UserNotification.Name = desc2;
                                                UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                                UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                                UserNotification.SendUserId = 1;
                                                UserNotification.Type = 1; // notification
                                                UserNotification.Description = "تم انتهاء : " + ProTaskUpdated.DescriptionAr + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + (projectData?.CustomerName ?? "") + " " + " فرع " + _BranchesRepository.GetById(BranchId).NameAr + "";
                                                UserNotification.AllUsers = false;
                                                UserNotification.SendDate = DateTime.Now;
                                                UserNotification.ProjectId = ProTaskUpdated.ProjectId;
                                                UserNotification.TaskId = ProTaskUpdated.PhaseTaskId;
                                                UserNotification.AddUser = UserId;
                                                UserNotification.AddDate = DateTime.Now;
                                                UserNotification.BranchId = BranchId;
                                                UserNotification.IsHidden = false;
                                                UserNotification.NextTime = null;
                                                _TaamerProContext.Notification.Add(UserNotification);
                                                _notificationService.sendmobilenotification(userId , desc2, "تم انتهاء المهمة : " + ProTaskUpdated.DescriptionAr + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + (projectData?.ContractorName ?? "") + " " + " فرع " + _BranchesRepository.GetById(BranchId).NameAr + "");

                                                SendMailNoti(ActiveSubPhaseProject.ProjectId, desc2, desc2, BranchId, UserId, userId);
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine($"Mail failed for user {userId}: {ex.Message}");
                                            }

                                            var userObj2 = _UsersRepository.GetById(userId);
                                            if (!string.IsNullOrEmpty(userObj2?.Mobile))
                                            {
                                                var NotStr2 = formattedDate + " بتاريخ " + ParentPhaseForTask.DescriptionAr + " انتهاء مرحلة رئيسية";
                                                var result = _userNotificationPrivilegesService.SendSMS(userObj2.Mobile, NotStr2, UserId, BranchId);
                                            }
                                        }


                                        //End
                                    }
                                }

                            }
                        }
                        ///

                        var project = _ProjectRepository.GetById((int)ProTaskUpdated.ProjectId);
                        if (project != null)
                        {
                            project.MotionProject = 1;
                            project.MotionProjectDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            project.MotionProjectNote = "انهاء مهمة";
                        }
                    }

                }
                if (ProjectPhasesTasks.Status == 4)
                {
                    
                    var UserNotifPriv4 = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(projectData.MangerId ?? 0).Result;
                    string formattedDate2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    if (ProTaskUpdated.ProjectId != null && ProTaskUpdated.ProjectId != 0)
                    {
                        
                        if (projectData.CustomerId != null && projectData.CustomerId != 0)
                        {
                            //var cust = _TaamerProContext.Customer.Where(x => x.CustomerId == proj.CustomerId).FirstOrDefault();

                            if (UserNotifPriv4.Count() != 0)
                            {
                                if (UserNotifPriv4.Contains(3272))
                                {
                                    var UserNotification = new Notification();
                                    UserNotification.ReceiveUserId = ProTaskUpdated.UserId;
                                    UserNotification.Name = "انتهاء مهمة";
                                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                    UserNotification.SendUserId = 1;
                                    UserNotification.Type = 1; // notification
                                    UserNotification.Description = "تم انتهاء : " + ProTaskUpdated.DescriptionAr + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + (projectData?.CustomerName??"") + " " + " فرع " + _BranchesRepository.GetById(BranchId).NameAr + "";
                                    UserNotification.AllUsers = false;
                                    UserNotification.SendDate = DateTime.Now;
                                    UserNotification.ProjectId = ProTaskUpdated.ProjectId;
                                    UserNotification.TaskId = ProTaskUpdated.PhaseTaskId;
                                    UserNotification.AddUser = UserId;
                                    UserNotification.AddDate = DateTime.Now;
                                    UserNotification.BranchId = BranchId;
                                    UserNotification.IsHidden = false;
                                    UserNotification.NextTime = null;
                                    _TaamerProContext.Notification.Add(UserNotification);
                                    _notificationService.sendmobilenotification(ProTaskUpdated.UserId ?? 0, Resources.finishTask, "تم انتهاء المهمة : " + ProTaskUpdated.DescriptionAr + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + (projectData?.ContractorName??"") + " " + " فرع " + _BranchesRepository.GetById(BranchId).NameAr + "");


                                }
                                if (UserNotifPriv4.Contains(3271))
                                {
                                    var Desc2 = formattedDate2 + " بتاريخ " + ProTaskUpdated.DescriptionAr + "انتهاء مهمة ";

                                    try
                                    {
                                        SendMailFinishTask2(ProTaskUpdated, "تهانينا ، لقد انهيت مهمة علي المشروع ", BranchId, UserId, URL, ImgUrl, 1, 0, projectData.CustomerName??"", projectData.ProjectNo ?? "", projectData.MangerId ?? 0, projectData.ProjectMangerName ?? "");
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }


                                if (ProTaskUpdated.ProjectGoals != null && ProTaskUpdated.ProjectGoals != 0)
                                {
                                    var req = _TaamerProContext.ProjectRequirementsGoals.Where(x => x.RequirementGoalId == ProTaskUpdated.ProjectGoals).FirstOrDefault();
                                    if (req != null)
                                    {
                                        req.RequirementsandGoals = _TaamerProContext.RequirementsandGoals.Where(x => x.RequirementId == req.RequirementId).FirstOrDefault();
                                        if (ProTaskUpdated.ProjectId != null && ProTaskUpdated.ProjectId != 0)
                                        {
                                            //var proj2 = _TaamerProContext.Project.Where(x => x.ProjectId == ProTaskUpdated.ProjectId).FirstOrDefault();
                                            if (projectData.CustomerId != null && projectData.CustomerId != 0)
                                            {
                                                //var cust2 = _TaamerProContext.Customer.Where(x => x.CustomerId == projectData.CustomerId).FirstOrDefault();

                                                if (UserNotifPriv4.Contains(3282))
                                                {
                                                    var UserNotification = new Notification();
                                                    UserNotification.ReceiveUserId = ProTaskUpdated.UserId;
                                                    UserNotification.Name = "تحقق هدف";
                                                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                                    UserNotification.SendUserId = 1;
                                                    UserNotification.Type = 1; // notification
                                                    UserNotification.Description = "تم تحقق  : " + req?.RequirementsandGoals?.RequirmentName + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + (projectData?.CustomerName??"") + " " + " فرع " + _BranchesRepository.GetById(BranchId).NameAr + "";
                                                    UserNotification.AllUsers = false;
                                                    UserNotification.SendDate = DateTime.Now;
                                                    UserNotification.ProjectId = ProTaskUpdated.ProjectId;
                                                    UserNotification.TaskId = ProTaskUpdated.PhaseTaskId;
                                                    UserNotification.AddUser = UserId;
                                                    UserNotification.AddDate = DateTime.Now;
                                                    UserNotification.BranchId = BranchId;
                                                    UserNotification.IsHidden = false;
                                                    UserNotification.NextTime = null;
                                                    _TaamerProContext.Notification.Add(UserNotification);
                                                    _notificationService.sendmobilenotification(ProTaskUpdated.UserId ?? 0, Resources.finishTask, "تم تحقق الهدف : " + req?.RequirementsandGoals?.RequirmentName + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + (projectData.CustomerName??"") + " " + " فرع " + _BranchesRepository.GetById(BranchId).NameAr + "");

                                                }
                                            }
                                            if (UserNotifPriv4.Contains(3281))
                                            {
                                                var Desc2 = formattedDate2 + " بتاريخ " + ProTaskUpdated.ProjectRequirementsGoals.RequirementsandGoals.RequirmentName + "انجاز هدف ";

                                                try
                                                {
                                                    SendMailFinishTask3(ProTaskUpdated, req.RequirementsandGoals.RequirmentName, BranchId, UserId, projectData.CustomerName??"");
                                                }
                                                catch (Exception)
                                                {

                                                }
                                            }


                                            if (UserNotifPriv4.Contains(3283))
                                            {
                                                var userObj = _UsersRepository.GetById(projectData.MangerId ?? 0);
                                                var NotStr = formattedDate2 + " بتاريخ " + req.RequirementsandGoals.RequirmentName + " تم انجاز هدف ";
                                                if (userObj.Mobile != null && userObj.Mobile != "")
                                                {
                                                    var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                                }

                                            }


                                        }


                                    }
                                }
                            }
                        }
                    }

                }


                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "انهاءالمهمة" + ProTaskUpdated.DescriptionAr + "مشروع" + projectData.ProjectNo;
                _SystemAction.SaveAction("FinishTask", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                //SendMailFinishTask(ProTaskUpdated, BranchId, UserId);
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.finishTask };

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في انهاءالمهمة";
                _SystemAction.SaveAction("FinishTask", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.failedfinishTask };
            }
        }

        public GeneralMessage ChangePriorityTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId)
        {
            try
            {

                var ProTaskUpdated = _ProjectPhasesTasksRepository.GetById(ProjectPhasesTasks.PhaseTaskId);

                if (ProTaskUpdated != null)
                {
                    if (ProTaskUpdated.PhasePriority != ProjectPhasesTasks.PhasePriority)
                    {
                        ProTaskUpdated.PhasePriority = ProjectPhasesTasks.PhasePriority;
                    }
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تغيير أولوية المهمة";
               _SystemAction.SaveAction("ChangePriorityTask", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                //SendMailFinishTask(ProTaskUpdated, BranchId, UserId);
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تغيير أولوية المهمة";
               _SystemAction.SaveAction("ChangePriorityTask", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        //EditD4
        public GeneralMessage ChangeTaskTime(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, string Url, string ImgUrl)
        {
            try
            {
                var ProTaskUpdated = _ProjectPhasesTasksRepository.GetById(ProjectPhasesTasks.PhaseTaskId);
                var projectData = _ProjectRepository.GetProjectByIdSome("rtl", ProTaskUpdated.ProjectId ?? 0).Result;
                if (ProTaskUpdated != null)
                {
                    

                    int? TimeMin = ProjectPhasesTasks.TimeMinutes ?? 1;
                    int? TimeTy = ProjectPhasesTasks.TimeType ?? 1;
                    int? tempRemainingU = 0;
                    switch (TimeTy)
                    {
                        case 1:
                            tempRemainingU = TimeMin * 60;
                            break;
                        case 2:
                            tempRemainingU = TimeMin * 60 * 24;
                            break;
                    }
                    ProTaskUpdated.EndDate= ProTaskUpdated.ExcpectedEndDate = ProjectPhasesTasks.EndDate;


                    if (ProTaskUpdated.EndDateNew == null) ProTaskUpdated.EndDateNew = Convert.ToDateTime(ProTaskUpdated.ExcpectedEndDate);
                    if (ProTaskUpdated.StartDateNew == null) ProTaskUpdated.StartDateNew = Convert.ToDateTime(ProTaskUpdated.ExcpectedStartDate);

                    DateTime newDateTime = (ProTaskUpdated.EndDateNew ?? new DateTime()).AddMinutes(tempRemainingU ?? 0);
                    ProTaskUpdated.EndDateNew = newDateTime;
                    ProTaskUpdated.EndDate = (ProTaskUpdated.EndDateNew ?? new DateTime()).ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    ProTaskUpdated.ExcpectedEndDate = (ProTaskUpdated.EndDateNew ?? new DateTime()).ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));


                    ProTaskUpdated.EndDateCalc = ProjectPhasesTasks.EndDate;

                    if (!(ProTaskUpdated.StartDate == null || ProTaskUpdated.StartDate == ""))
                    {
                        ProTaskUpdated.ExcpectedStartDate = ProTaskUpdated.StartDate;
                    }
                    var DateDiff = ((ProTaskUpdated.EndDateNew ?? new DateTime()) - (ProTaskUpdated.StartDateNew ?? new DateTime()));
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



                    ProTaskUpdated.TimeType = TimeType;

                    ProTaskUpdated.TimeMinutes = ProTaskUpdated.TimeMinutes + TimeMinutes;
                    if (ProTaskUpdated.Remaining >= 0)
                    {
                        ProTaskUpdated.Remaining = ProTaskUpdated.Remaining + tempRemainingU;
                    }
                    else
                    {
                        ProTaskUpdated.Remaining = tempRemainingU;
                    }

                    ProTaskUpdated.TaskTimeFrom = (ProTaskUpdated.StartDateNew ?? DateTime.Now).ToString("hh:mm tt", CultureInfo.InvariantCulture);
                    ProTaskUpdated.TaskTimeTo = (ProTaskUpdated.EndDateNew ?? DateTime.Now).ToString("hh:mm tt", CultureInfo.InvariantCulture);



                    ProTaskUpdated.Cost = ProjectPhasesTasks.Cost;
                    ProTaskUpdated.PlusTime = false;

                    ProTaskUpdated.UpdateUser = UserId;
                    ProTaskUpdated.UpdateDate = DateTime.Now;
                    ProTaskUpdated.PlusTimeReason_admin = ProjectPhasesTasks.PlusTimeReason_admin;

                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تغيير وقت المهمة";
               _SystemAction.SaveAction("ChangeTaskTime", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                // SendMailChangeTaskTime(ProTaskUpdated, BranchId, UserId, 1);
                #region
                //SaveOperationsForTask
                Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                Pro_TaskOperation.PhaseTaskId = ProTaskUpdated!.PhaseTaskId;
                Pro_TaskOperation.OperationName = "تم تمديد المهمة";
                _SystemAction.SaveTaskOperations(Pro_TaskOperation, UserId, BranchId);
                #endregion

                try
                {
                    #region Notifications
                    var (usersList, descriptionFromConfig) = _projectService.GetNotificationRecipients(NotificationCode.Task_ExtensionAccepted, ProTaskUpdated.ProjectId.Value);
                    var description = "تمديد مهمة";

                    if (descriptionFromConfig != null && descriptionFromConfig != "")
                        description = descriptionFromConfig;

                    if (usersList == null || usersList.Count == 0)
                    {
                        var UserNotification = new Notification();
                        UserNotification.ReceiveUserId = ProTaskUpdated.UserId;
                        UserNotification.Name = description;
                        UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                        UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                        UserNotification.SendUserId = UserId;
                        UserNotification.Type = 1; // notification
                        UserNotification.Description = " تم تمديد المهمة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + " السبب :  " + ProjectPhasesTasks.PlusTimeReason_admin;
                        UserNotification.AllUsers = false;
                        UserNotification.SendDate = DateTime.Now;
                        UserNotification.ProjectId = ProTaskUpdated.ProjectId;
                        UserNotification.TaskId = ProTaskUpdated.PhaseTaskId;
                        UserNotification.AddUser = UserId;
                        UserNotification.IsHidden = false;
                        UserNotification.NextTime = null;
                        UserNotification.AddDate = DateTime.Now;
                        _TaamerProContext.Notification.Add(UserNotification);
                        _TaamerProContext.SaveChanges();
                        if (projectData != null && projectData.MangerId != null && projectData.MangerId != 0)
                        {
                            var managernot = new Notification();
                            managernot = UserNotification;
                            managernot.ReceiveUserId = projectData.UserId;
                            managernot.NotificationId = 0;
                            _TaamerProContext.Notification.Add(managernot);
                            _TaamerProContext.SaveChanges();
                            _notificationService.sendmobilenotification(projectData.MangerId ?? 0, description, "تم تمديد المهمة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + "   السبب   " + ProjectPhasesTasks.PlusTimeReason_admin);

                        }

                        _notificationService.sendmobilenotification(ProTaskUpdated.UserId ?? 0, description, "تم تمديد المهمة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + "   السبب   " + ProjectPhasesTasks.PlusTimeReason_admin);

                        SendMailFinishTask2(ProTaskUpdated, description, BranchId, ProTaskUpdated.UserId ??0, Url, ImgUrl, 4, 0, projectData.CustomerName ?? "", projectData.ProjectNo ?? "", projectData.MangerId ?? 0, projectData.ProjectMangerName ?? "");

                    }
                    else
                    {
                        foreach (var user in usersList)
                        {
                            var UserNotification = new Notification();
                            UserNotification.ReceiveUserId = ProTaskUpdated.UserId;
                            UserNotification.Name = "تمديد مهمة";
                            UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                            UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                            UserNotification.SendUserId = UserId;
                            UserNotification.Type = 1; // notification
                            UserNotification.Description = " تم تمديد المهمة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + " السبب :  " + ProjectPhasesTasks.PlusTimeReason_admin;
                            UserNotification.AllUsers = false;
                            UserNotification.SendDate = DateTime.Now;
                            UserNotification.ProjectId = ProTaskUpdated.ProjectId;
                            UserNotification.TaskId = ProTaskUpdated.PhaseTaskId;
                            UserNotification.AddUser = UserId;
                            UserNotification.IsHidden = false;
                            UserNotification.NextTime = null;
                            UserNotification.AddDate = DateTime.Now;
                            _TaamerProContext.Notification.Add(UserNotification);
                            _TaamerProContext.SaveChanges();
                            _notificationService.sendmobilenotification(user , "  تمديد المهمة ", "تم تمديد المهمة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + "   السبب   " + ProjectPhasesTasks.PlusTimeReason_admin);

                            SendMailFinishTask2(ProTaskUpdated, description, BranchId, user, Url, ImgUrl, 4, 0, projectData.CustomerName ?? "", projectData.ProjectNo ?? "", projectData.MangerId ?? 0, projectData.ProjectMangerName ?? "");

                        }
                    }
                        #endregion
                }catch(Exception ex)
                {

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.Pro_taskPluseTime };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تغيير وقت المهمة";
               _SystemAction.SaveAction("ChangeTaskTime", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.failedTaskPlusTime };
            }
        }
        public GeneralMessage PlustimeTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, string Url, string ImgUrl)
        {
            try
            {
                var ProTaskUpdated = _ProjectPhasesTasksRepository.GetById(ProjectPhasesTasks.PhaseTaskId);
                if (ProTaskUpdated != null)
                {
                    //ProTaskUpdated.Status = ProjectPhasesTasks.Status;
                    ProTaskUpdated.PlusTime = true;
                    //ProTaskUpdated.Active = false;
                    ProTaskUpdated.UpdateUser = UserId;
                    ProTaskUpdated.UpdateDate = DateTime.Now;
                    ProTaskUpdated.PlusTimeReason = ProjectPhasesTasks.PlusTimeReason;
                }
                string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                try
                {
                    if(ProTaskUpdated !=null && ProTaskUpdated.ProjectId > 0) 
                    {
                        var projectData = _ProjectRepository.GetProjectByIdSome("rtl", ProTaskUpdated.ProjectId ?? 0).Result;
                        if (projectData != null && projectData.CustomerId > 0)
                        {
                            //get notification configuration users and description
                            var (usersList, descriptionFromConfig) = _projectService.GetNotificationRecipients(NotificationCode.Task_ExtensionRequested, ProTaskUpdated.ProjectId.Value);
                            var description = "لديك طلب تمديد المهمة";

                            if (descriptionFromConfig != null && descriptionFromConfig != "")
                                description = descriptionFromConfig;

                            //if no configuration send to emp and manager
                            if (usersList == null || usersList.Count == 0)
                            {
                                var UserNotification = new Notification();
                                UserNotification.ReceiveUserId = ProTaskUpdated.UserId;
                                UserNotification.Name = description;
                                UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                UserNotification.SendUserId = UserId;
                                UserNotification.Type = 1; // notification
                                UserNotification.Description = "طلب تمديد : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + " السبب :  " + ProjectPhasesTasks.PlusTimeReason;
                                UserNotification.AllUsers = false;
                                UserNotification.SendDate = DateTime.Now;
                                UserNotification.ProjectId = ProTaskUpdated.ProjectId;
                                UserNotification.TaskId = ProTaskUpdated.PhaseTaskId;
                                UserNotification.AddUser = UserId;
                                UserNotification.IsHidden = false;
                                UserNotification.NextTime = null;
                                UserNotification.AddDate = DateTime.Now;
                                _TaamerProContext.Notification.Add(UserNotification);
                                _TaamerProContext.SaveChanges();
                                if (projectData != null && projectData.MangerId != null && projectData.MangerId != 0)
                                {
                                    var managernot = new Notification();
                                    managernot = UserNotification;
                                    managernot.ReceiveUserId = projectData.MangerId;
                                    managernot.NotificationId = 0;
                                    _TaamerProContext.Notification.Add(managernot);
                                    _TaamerProContext.SaveChanges();
                                    _notificationService.sendmobilenotification(projectData.MangerId ?? 0, description, "طلب تمديد المهمة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + "   السبب   " + ProjectPhasesTasks.PlusTimeReason);

                                }
                                _notificationService.sendmobilenotification(ProTaskUpdated.UserId ?? 0, description, "طلب تمديد المهمة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + "   السبب   " + ProjectPhasesTasks.PlusTimeReason);

                                SendMailFinishTask2(ProTaskUpdated, description, BranchId, UserId, Url, ImgUrl, 2, 0, projectData.CustomerName ?? "", projectData.ProjectNo ?? "", projectData.MangerId ?? 0, projectData.ProjectMangerName ?? "");
                                var userObj = _UsersRepository.GetById(projectData.MangerId ?? 0);
                                var NotStr = formattedDate + " بتاريخ " + ProTaskUpdated.TaskNo + " رقم " + ProTaskUpdated.DescriptionAr + " طلب تمديد المهمة ";
                                if (userObj.Mobile != null && userObj.Mobile != "")
                                {
                                    var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                }

                                var userObj2 = _UsersRepository.GetById(ProTaskUpdated.UserId ?? 0);
                                if (userObj2.Mobile != null && userObj2.Mobile != "")
                                {
                                    var result = _userNotificationPrivilegesService.SendSMS(userObj2.Mobile, NotStr, UserId, BranchId);
                                }
                            }
                            else
                            {
                                foreach (var user in usersList)
                                {



                                    var UserNotification = new Notification();
                                    UserNotification.ReceiveUserId = user;
                                    UserNotification.Name = description;
                                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                    UserNotification.SendUserId = UserId;
                                    UserNotification.Type = 1; // notification
                                    UserNotification.Description = "طلب تمديد : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + " السبب :  " + ProjectPhasesTasks.PlusTimeReason;
                                    UserNotification.AllUsers = false;
                                    UserNotification.SendDate = DateTime.Now;
                                    UserNotification.ProjectId = ProTaskUpdated.ProjectId;
                                    UserNotification.TaskId = ProTaskUpdated.PhaseTaskId;
                                    UserNotification.AddUser = UserId;
                                    UserNotification.IsHidden = false;
                                    UserNotification.NextTime = null;
                                    UserNotification.AddDate = DateTime.Now;
                                    _TaamerProContext.Notification.Add(UserNotification);
                                    _TaamerProContext.SaveChanges();
                                    SendMailFinishTask2(ProTaskUpdated, description, BranchId, user, Url, ImgUrl, 2, 0, projectData.CustomerName ?? "", projectData.ProjectNo ?? "", projectData.MangerId ?? 0, projectData.ProjectMangerName ?? "");
                                    var userObj = _UsersRepository.GetById(user);
                                    var NotStr = formattedDate + " بتاريخ " + ProTaskUpdated.TaskNo + " رقم " + ProTaskUpdated.DescriptionAr + " طلب تمديد المهمة ";
                                    if (userObj.Mobile != null && userObj.Mobile != "")
                                    {
                                        var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                    }
                                }

                            }
                        }
                        }
                }
                catch (Exception)
                {

                }




                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تمديد وقت المهمة";
               _SystemAction.SaveAction("PlustimeTask", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                //SendMailFinishTask(ProTaskUpdated, BranchId, UserId);
                #region
                //SaveOperationsForTask
                Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                Pro_TaskOperation.PhaseTaskId = ProTaskUpdated!.PhaseTaskId;
                Pro_TaskOperation.OperationName = "طلب تمديد المهمة";
                _SystemAction.SaveTaskOperations(Pro_TaskOperation, UserId, BranchId);
                #endregion

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.Pro_PlusTasktime };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تمديد وقت المهمة";
               _SystemAction.SaveAction("PlustimeTask", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Pro_faildPlustimeTask };
            }
        }
        public GeneralMessage RefusePlustimeTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, string Url, string ImgUrl)
        {
            try
            {
                var ProTaskUpdated = _ProjectPhasesTasksRepository.GetById(ProjectPhasesTasks.PhaseTaskId);
                if (ProTaskUpdated != null)
                {
                    //ProTaskUpdated.Status = ProjectPhasesTasks.Status;
                    ProTaskUpdated.PlusTime = false;
                    //ProTaskUpdated.Active = false;
                    ProTaskUpdated.UpdateUser = UserId;
                    ProTaskUpdated.UpdateDate = DateTime.Now;
                    ProTaskUpdated.PlusTimeReason_admin = ProjectPhasesTasks.PlusTimeReason_admin;

                }
                string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                try
                {
                    if (ProTaskUpdated != null && ProTaskUpdated.ProjectId > 0)
                    {
                        var projectData = _ProjectRepository.GetProjectByIdSome("rtl", ProTaskUpdated.ProjectId ?? 0).Result;
                        if (projectData != null && projectData.CustomerId > 0)
                        {

                            //get notification configuration users and description
                            var (usersList, descriptionFromConfig) = _projectService.GetNotificationRecipients(NotificationCode.Task_ExtensionRejected, ProTaskUpdated.ProjectId.Value);
                            var description = "رفض تمديد المهمة";

                            if (descriptionFromConfig != null && descriptionFromConfig != "")
                                description = descriptionFromConfig;

                            //if no configuration send to emp and manager
                            if (usersList == null || usersList.Count == 0)
                            {
                                var UserNotification = new Notification();
                                UserNotification.ReceiveUserId = ProTaskUpdated.UserId;
                                UserNotification.Name = description;
                                UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                UserNotification.SendUserId = UserId;
                                UserNotification.Type = 1; // notification
                                UserNotification.Description = "رفض تمديد : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + " السبب :  " + ProjectPhasesTasks.PlusTimeReason_admin;
                                UserNotification.AllUsers = false;
                                UserNotification.SendDate = DateTime.Now;
                                UserNotification.ProjectId = ProTaskUpdated.ProjectId;
                                UserNotification.TaskId = ProTaskUpdated.PhaseTaskId;
                                UserNotification.AddUser = UserId;
                                UserNotification.IsHidden = false;
                                UserNotification.NextTime = null;
                                UserNotification.AddDate = DateTime.Now;
                                _TaamerProContext.Notification.Add(UserNotification);
                                _TaamerProContext.SaveChanges();
                                if (projectData != null && projectData.MangerId != null && projectData.MangerId != 0)
                                {
                                    var managernot = new Notification();
                                    managernot = UserNotification;
                                    managernot.ReceiveUserId = projectData.UserId;
                                    managernot.NotificationId = 0;
                                    _TaamerProContext.Notification.Add(managernot);
                                    _TaamerProContext.SaveChanges();
                                    _notificationService.sendmobilenotification(projectData.MangerId ?? 0, description, "رفض تمديد المهمة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + "   السبب   " + ProjectPhasesTasks.PlusTimeReason_admin);

                                }

                                _notificationService.sendmobilenotification(ProTaskUpdated.UserId ?? 0, description, "رفض تمديد المهمة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + "   السبب   " + ProjectPhasesTasks.PlusTimeReason_admin);



                                SendMailFinishTask2(ProTaskUpdated, description, BranchId, UserId, Url, ImgUrl, 7, 0, projectData.CustomerName ?? "", projectData.ProjectNo ?? "", projectData.MangerId ?? 0, projectData.ProjectMangerName ?? "");

                                var userObj = _UsersRepository.GetById(projectData.MangerId ?? 0);
                                var NotStr = formattedDate + " بتاريخ " + ProTaskUpdated.TaskNo + " رقم " + ProTaskUpdated.DescriptionAr + " طلب تمديد المهمة ";
                                if (userObj.Mobile != null && userObj.Mobile != "")
                                {
                                    var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                }

                                var userObj2 = _UsersRepository.GetById(ProTaskUpdated.UserId ?? 0);
                                if (userObj2.Mobile != null && userObj2.Mobile != "")
                                {
                                    var result = _userNotificationPrivilegesService.SendSMS(userObj2.Mobile, NotStr, UserId, BranchId);
                                }
                            }
                            else
                            {
                                foreach (var user in usersList)
                                {

                                    var UserNotification = new Notification();
                                    UserNotification.ReceiveUserId = user;
                                    UserNotification.Name = description;
                                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                    UserNotification.SendUserId = UserId;
                                    UserNotification.Type = 1; // notification
                                    UserNotification.Description = "رفض تمديد : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + " السبب :  " + ProjectPhasesTasks.PlusTimeReason_admin;
                                    UserNotification.AllUsers = false;
                                    UserNotification.SendDate = DateTime.Now;
                                    UserNotification.ProjectId = ProTaskUpdated.ProjectId;
                                    UserNotification.TaskId = ProTaskUpdated.PhaseTaskId;
                                    UserNotification.AddUser = UserId;
                                    UserNotification.IsHidden = false;
                                    UserNotification.NextTime = null;
                                    UserNotification.AddDate = DateTime.Now;
                                    _TaamerProContext.Notification.Add(UserNotification);
                                    _TaamerProContext.SaveChanges();
                                    _notificationService.sendmobilenotification(user,description, "رفض تمديد المهمة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + "   السبب   " + ProjectPhasesTasks.PlusTimeReason_admin);
                                    SendMailFinishTask2(ProTaskUpdated, description, BranchId, user, Url, ImgUrl, 7, 0, projectData.CustomerName ?? "", projectData.ProjectNo ?? "", projectData.MangerId ?? 0, projectData.ProjectMangerName ?? "");

                                    var userObj2 = _UsersRepository.GetById(user);
                                    var NotStr = formattedDate + " بتاريخ " + ProTaskUpdated.TaskNo + " رقم " + ProTaskUpdated.DescriptionAr + " طلب تمديد المهمة ";

                                    if (userObj2.Mobile != null && userObj2.Mobile != "")
                                    {
                                        var result = _userNotificationPrivilegesService.SendSMS(userObj2.Mobile, NotStr, UserId, BranchId);
                                    }
                                }
                            }


                        }
                    }
                }
                catch (Exception)
                {

                }

                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "رفض تمديد وقت المهمة";
               _SystemAction.SaveAction("RefusePlustimeTask", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                //SendMailFinishTask(ProTaskUpdated, BranchId, UserId);
                #region
                //SaveOperationsForTask
                Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                Pro_TaskOperation.PhaseTaskId = ProTaskUpdated!.PhaseTaskId;
                Pro_TaskOperation.OperationName = "رفض تمديد المهمة";
                _SystemAction.SaveTaskOperations(Pro_TaskOperation, UserId, BranchId);
                #endregion
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم رفض تمديد وقت المهمة" };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في رفض تمديد وقت المهمة";
               _SystemAction.SaveAction("RefusePlustimeTask", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Pro_faildPlustimeTask };
            }
        }

        public GeneralMessage RefuseConvertTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, string Url, string ImgUrl)
        {
            try
            {
                var ProTaskUpdated = _ProjectPhasesTasksRepository.GetById(ProjectPhasesTasks.PhaseTaskId);
                if (ProTaskUpdated != null)
                {
                    //ProTaskUpdated.Status = ProjectPhasesTasks.Status;
                    ProTaskUpdated.IsConverted = 0;
                    //ProTaskUpdated.Active = false;
                    ProTaskUpdated.UpdateUser = UserId;
                    ProTaskUpdated.UpdateDate = DateTime.Now;
                    ProTaskUpdated.convertReason_admin = ProjectPhasesTasks.convertReason_admin;

                }
                string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                try
                {
                    if (ProTaskUpdated != null && ProTaskUpdated.ProjectId > 0)
                    {
                        var projectData = _ProjectRepository.GetProjectByIdSome("rtl", ProTaskUpdated.ProjectId ?? 0).Result;
                        if (projectData != null && projectData.CustomerId > 0)
                        {
                            //get notification configuration users and description
                            var (usersList, descriptionFromConfig) = _projectService.GetNotificationRecipients(NotificationCode.Task_TransferRejected, ProTaskUpdated.ProjectId.Value);
                            var description = " رفض تحويل مهمة";

                            if (descriptionFromConfig != null && descriptionFromConfig != "")
                                description = descriptionFromConfig;

                            //if no configuration send to emp and manager
                            if (usersList == null || usersList.Count == 0)
                            {
                                var UserNotification = new Notification();
                                UserNotification.ReceiveUserId = ProTaskUpdated.UserId;
                                UserNotification.Name = description;
                                UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                UserNotification.SendUserId = UserId;
                                UserNotification.Type = 1; // notification
                                UserNotification.Description = "رفض تحويل : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + " السبب :  " + ProjectPhasesTasks.PlusTimeReason;
                                UserNotification.AllUsers = false;
                                UserNotification.SendDate = DateTime.Now;
                                UserNotification.ProjectId = ProTaskUpdated.ProjectId;
                                UserNotification.TaskId = ProTaskUpdated.PhaseTaskId;
                                UserNotification.AddUser = UserId;
                                UserNotification.IsHidden = false;
                                UserNotification.NextTime = null;
                                UserNotification.AddDate = DateTime.Now;
                                _TaamerProContext.Notification.Add(UserNotification);
                                _TaamerProContext.SaveChanges();
                                if (projectData != null && projectData.MangerId != null && projectData.MangerId != 0)
                                {
                                    var managernot = new Notification();
                                    managernot = UserNotification;
                                    managernot.ReceiveUserId = projectData.MangerId;
                                    managernot.NotificationId = 0;
                                    _TaamerProContext.Notification.Add(managernot);
                                    _TaamerProContext.SaveChanges();
                                    _notificationService.sendmobilenotification(projectData.MangerId ?? 0, description, "رفض تحويل المهمة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + "   السبب   " + ProjectPhasesTasks.convertReason_admin);

                                }

                                _notificationService.sendmobilenotification(ProTaskUpdated.UserId ?? 0, description, "رفض تحويل المهمة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + "   السبب   " + ProjectPhasesTasks.convertReason_admin);

                                SendMailFinishTask2(ProTaskUpdated, description, BranchId, UserId, Url, ImgUrl, 8, 0, projectData.CustomerName ?? "", projectData.ProjectNo ?? "", projectData.MangerId ?? 0, projectData.ProjectMangerName ?? "");

                                var userObj = _UsersRepository.GetById(projectData.MangerId ?? 0);
                                var NotStr = formattedDate + " بتاريخ " + ProTaskUpdated.TaskNo + " رقم " + ProTaskUpdated.DescriptionAr + " رفض تحويل المهمة ";
                                if (userObj.Mobile != null && userObj.Mobile != "")
                                {
                                    var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                }

                                var userObj2 = _UsersRepository.GetById(ProTaskUpdated.UserId ?? 0);
                                if (userObj2.Mobile != null && userObj2.Mobile != "")
                                {
                                    var result = _userNotificationPrivilegesService.SendSMS(userObj2.Mobile, NotStr, UserId, BranchId);
                                }
                            }
                            else
                            {

                                foreach (var user in usersList)
                                {
                                    var UserNotification = new Notification();
                                    UserNotification.ReceiveUserId = ProTaskUpdated.UserId;
                                    UserNotification.Name = description;
                                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                    UserNotification.SendUserId = UserId;
                                    UserNotification.Type = 1; // notification
                                    UserNotification.Description = "رفض تحويل : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + " السبب :  " + ProjectPhasesTasks.PlusTimeReason;
                                    UserNotification.AllUsers = false;
                                    UserNotification.SendDate = DateTime.Now;
                                    UserNotification.ProjectId = ProTaskUpdated.ProjectId;
                                    UserNotification.TaskId = ProTaskUpdated.PhaseTaskId;
                                    UserNotification.AddUser = UserId;
                                    UserNotification.IsHidden = false;
                                    UserNotification.NextTime = null;
                                    UserNotification.AddDate = DateTime.Now;
                                    _TaamerProContext.Notification.Add(UserNotification);
                                    _TaamerProContext.SaveChanges();
                                    _notificationService.sendmobilenotification(user, description, "رفض تحويل المهمة : " + ProTaskUpdated.DescriptionAr + " رقم " + ProTaskUpdated.TaskNo + ":" + ProTaskUpdated.Notes + " علي مشروع رقم " + projectData.ProjectNo + " للعميل " + projectData.CustomerName + "   السبب   " + ProjectPhasesTasks.convertReason_admin);

                                    SendMailFinishTask2(ProTaskUpdated, description, BranchId, user, Url, ImgUrl, 8, 0, projectData.CustomerName ?? "", projectData.ProjectNo ?? "", projectData.MangerId ?? 0, projectData.ProjectMangerName ?? "");

                                    var userObj = _UsersRepository.GetById(user);
                                    var NotStr = formattedDate + " بتاريخ " + ProTaskUpdated.TaskNo + " رقم " + ProTaskUpdated.DescriptionAr + " رفض تحويل المهمة ";
                                    if (userObj.Mobile != null && userObj.Mobile != "")
                                    {
                                        var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                    }
                                }

                            }
                        }
                    }
                }
                catch (Exception)
                {

                }

                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "رفض تحويل المهمة";
                _SystemAction.SaveAction("RefusePlustimeTask", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                //SendMailFinishTask(ProTaskUpdated, BranchId, UserId);
                #region
                //SaveOperationsForTask
                Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                Pro_TaskOperation.PhaseTaskId = ProTaskUpdated!.PhaseTaskId;
                Pro_TaskOperation.OperationName = "رفض تحويل المهمة";
                _SystemAction.SaveTaskOperations(Pro_TaskOperation, UserId, BranchId);
                #endregion
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم رفض تحويل المهمة" };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في رفض تحويل المهمة";
                _SystemAction.SaveAction("RefusePlustimeTask", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Pro_faildPlustimeTask };
            }
        }


        public GeneralMessage AskForMoreDetails(int ProjectPhasesTaskid, int askdetai, int UserId, int BranchId)
        {
            try
            {
                var ProTaskUpdated = _ProjectPhasesTasksRepository.GetById(ProjectPhasesTaskid);
                if (ProTaskUpdated != null)
                {
                    //ProTaskUpdated.Status = ProjectPhasesTasks.Status;
                    ProTaskUpdated.AskDetails = askdetai;
                    //ProTaskUpdated.Active = false;
                    ProTaskUpdated.UpdateUser = UserId;
                    ProTaskUpdated.UpdateDate = DateTime.Now;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "طلب تفاصيل اكثر";
               _SystemAction.SaveAction("AskForMoreDetails", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                //SendMailFinishTask(ProTaskUpdated, BranchId, UserId);
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.Pro_PlusTasktime };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في طلب تفاصيل اكثر";
               _SystemAction.SaveAction("AskForMoreDetails", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Pro_faildPlustimeTask };
            }
        }

        public GeneralMessage DeleteProjectPhasesTasks(int PhaseTaskId, int UserId, int BranchId)
        {
            try
            {

                var taskdependency = _TasksDependencyRepository.GetMatching(s => s.IsDeleted == false && (s.PredecessorId == PhaseTaskId || s.SuccessorId == PhaseTaskId)).ToList();
                if (taskdependency != null && taskdependency.Count > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = Resources.General_DeletedFailed;
                   _SystemAction.SaveAction("DeleteProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.DeleteTaskSetting_Failed };
                }
                else
                {
                    ProjectPhasesTasks projectPhasesTasks = _ProjectPhasesTasksRepository.GetById(PhaseTaskId);


                    if (projectPhasesTasks.Type == 1) //MainPhase
                    {

                        var projectPhasesTasks_S = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ParentId == projectPhasesTasks.PhaseTaskId); //tasks and submain in MainPhase
                        foreach (var item in projectPhasesTasks_S)
                        {
                            if (item.Type == 2) //subphases in mainphases
                            {

                                var projectPhasesTasks_SS = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ParentId == item.PhaseTaskId); //tasks in subphases in mainphases
                                foreach (var item2 in projectPhasesTasks_SS)
                                {
                                    if (item2.Status == 2 || item2.Status == 4)
                                    {
                                        //-----------------------------------------------------------------------------------------------------------------
                                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                        string ActionNote2 = Resources.General_SavedFailed;
                                       _SystemAction.SaveAction("DeleteProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                        //-----------------------------------------------------------------------------------------------------------------
                                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedToDeleteTaskInPhasestasks_M };//لا يمكنك حذف مرحلة رئيسية بها مهام قيد التشغيل او منتهية
                                    }
                                }



                            }
                            else //tasks in main phases
                            {
                                if (item.Status == 2 || item.Status == 4)
                                {
                                    //-----------------------------------------------------------------------------------------------------------------
                                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    string ActionNote2 = Resources.General_SavedFailed;
                                   _SystemAction.SaveAction("DeleteProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                    //-----------------------------------------------------------------------------------------------------------------
                                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedToDeleteTaskInPhasestasks_M };
                                }
                            }

                        }  //check

                        foreach (var items in projectPhasesTasks_S) //Delete tasks and submain in maintask
                        {

                            if (items.Type == 2)
                            {
                                ProjectPhasesTasks projectPhasesTasksTasksD1 = _ProjectPhasesTasksRepository.GetById(items.PhaseTaskId); //Delete Submain
                                var projectPhasesTasks_SS = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ParentId == items.PhaseTaskId); //tasks in subphases in mainphases


                                projectPhasesTasksTasksD1.IsDeleted = true;
                                projectPhasesTasksTasksD1.DeleteDate = DateTime.Now;
                                projectPhasesTasksTasksD1.DeleteUser = UserId;
                                _TaamerProContext.SaveChanges();


                                foreach (var item3 in projectPhasesTasks_SS)
                                {
                                    ProjectPhasesTasks projectPhasesTasksTasksDT = _ProjectPhasesTasksRepository.GetById(item3.PhaseTaskId);

                                    projectPhasesTasksTasksDT.IsDeleted = true;
                                    projectPhasesTasksTasksDT.DeleteDate = DateTime.Now;
                                    projectPhasesTasksTasksDT.DeleteUser = UserId;
                                    _TaamerProContext.SaveChanges();
                                } //Delete tasks in submain


                            }
                            else
                            {
                                ProjectPhasesTasks projectPhasesTasksTasksD1TT = _ProjectPhasesTasksRepository.GetById(items.PhaseTaskId);
                                projectPhasesTasksTasksD1TT.IsDeleted = true;
                                projectPhasesTasksTasksD1TT.DeleteDate = DateTime.Now;
                                projectPhasesTasksTasksD1TT.DeleteUser = UserId;
                                _TaamerProContext.SaveChanges();
                            } //Delete Tasks in MainPhase



                        }
                        projectPhasesTasks.IsDeleted = true;
                        projectPhasesTasks.DeleteDate = DateTime.Now;
                        projectPhasesTasks.DeleteUser = UserId;
                        _TaamerProContext.SaveChanges();
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " حذف المهمة رقم " + PhaseTaskId;
                       _SystemAction.SaveAction("DeleteProjectPhasesTasks", "ProjectPhasesTasksService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

                    }
                    else if (projectPhasesTasks.Type == 2) //SubMain
                    {

                        var projectPhasesTasks_S = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ParentId == projectPhasesTasks.PhaseTaskId); //tasks in submain
                        foreach (var item in projectPhasesTasks_S)
                        {
                            if (item.Status == 2 || item.Status == 4)
                            {
                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote2 = Resources.General_SavedFailed;
                               _SystemAction.SaveAction("DeleteProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                //-----------------------------------------------------------------------------------------------------------------
                                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedToDeleteTaskInPhasestasks_SM };//لا يمكنك حذف مرحلة فرعية بها مهام قيد التشغيل او منتهية
                            }
                        }

                        foreach (var item1 in projectPhasesTasks_S) //Delete tasks in submain
                        {
                            ProjectPhasesTasks projectPhasesTasksTasksD1 = _ProjectPhasesTasksRepository.GetById(item1.PhaseTaskId);

                            projectPhasesTasksTasksD1.IsDeleted = true;
                            projectPhasesTasksTasksD1.DeleteDate = DateTime.Now;
                            projectPhasesTasksTasksD1.DeleteUser = UserId;
                            _TaamerProContext.SaveChanges();

                        }
                        projectPhasesTasks.IsDeleted = true;
                        projectPhasesTasks.DeleteDate = DateTime.Now;
                        projectPhasesTasks.DeleteUser = UserId;
                        _TaamerProContext.SaveChanges();
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " حذف المهمة رقم " + PhaseTaskId;
                       _SystemAction.SaveAction("DeleteProjectPhasesTasks", "ProjectPhasesTasksService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

                    }
                    else //Tasks
                    {
                        if (projectPhasesTasks.Status == 2 || projectPhasesTasks.Status == 4)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote = Resources.General_SavedFailed;
                           _SystemAction.SaveAction("DeleteProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedToDeleteTaskInPhasestasks_T };//لا يمكنك حذف مهام قيد التشغيل او منتهية
                        }
                        else
                        {
                            projectPhasesTasks.IsDeleted = true;
                            projectPhasesTasks.DeleteDate = DateTime.Now;
                            projectPhasesTasks.DeleteUser = UserId;
                            _TaamerProContext.SaveChanges();
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote = " حذف المهمة رقم " + PhaseTaskId;
                           _SystemAction.SaveAction("DeleteProjectPhasesTasks", "ProjectPhasesTasksService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
                        }

                    }

                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف المهمة رقم " + PhaseTaskId; ;
               _SystemAction.SaveAction("DeleteProjectPhasesTasks", "ProjectPhasesTasksService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public GeneralMessage DeleteProjectPhasesTasksNEW(int PhaseTaskId, int UserId, int BranchId)
        {
            try
            {
                List<TasksDependency> TaskDependcy = new List<TasksDependency>();

                //awl task f seeer
                var taskdependency_Predecessor_Task = _TasksDependencyRepository.GetMatching(s => s.IsDeleted == false && (s.PredecessorId == PhaseTaskId)).ToList();
                if (taskdependency_Predecessor_Task != null && taskdependency_Predecessor_Task.Count > 0)
                {
                    var ProjSubTypeId_V = taskdependency_Predecessor_Task.FirstOrDefault().ProjSubTypeId;
                    var Type_V = taskdependency_Predecessor_Task.FirstOrDefault().Type;
                    var ProjectId_V = taskdependency_Predecessor_Task.FirstOrDefault().ProjectId;
                    var BranchId_V = taskdependency_Predecessor_Task.FirstOrDefault().BranchId;
                    var AddUser_V = taskdependency_Predecessor_Task.FirstOrDefault().AddUser;
                    var AddDate_V = taskdependency_Predecessor_Task.FirstOrDefault().AddDate;


                    //task feh ableha w fe b3deha
                    var taskdependency_Successor = _TasksDependencyRepository.GetMatching(s => s.IsDeleted == false && (s.SuccessorId == PhaseTaskId)).ToList();
                    if (taskdependency_Successor != null && taskdependency_Successor.Count > 0)
                    {

                        foreach (var Successor_Task_item in taskdependency_Successor)
                        {
                            foreach (var Predecessor_Task_item in taskdependency_Predecessor_Task)
                            {
                                TaskDependcy.Add(new TasksDependency
                                {
                                    PredecessorId = Successor_Task_item.PredecessorId,
                                    SuccessorId = Predecessor_Task_item.SuccessorId,
                                    ProjSubTypeId = ProjSubTypeId_V,
                                    Type = Type_V,
                                    ProjectId = ProjectId_V,
                                    BranchId = BranchId_V,
                                    AddUser = AddUser_V,
                                    AddDate = AddDate_V,
                                    IsDeleted = false

                                });
                                Successor_Task_item.IsDeleted = true;
                                Predecessor_Task_item.IsDeleted = true;
                            }
                        }

                        _TaamerProContext.TasksDependency.AddRange(TaskDependcy);

                    }
                    else
                    {
                        //awl task f seeer
                        foreach (var Predecessor_Task_item in taskdependency_Predecessor_Task)
                        {
                            Predecessor_Task_item.IsDeleted = true;
                        }

                    }

                }
                else
                {
                    //a5r task f seer
                    var taskdependency_Successor_Task = _TasksDependencyRepository.GetMatching(s => s.IsDeleted == false && (s.SuccessorId == PhaseTaskId)).ToList();
                    if (taskdependency_Successor_Task != null && taskdependency_Successor_Task.Count > 0)
                    {
                        foreach (var Successor_Task_item in taskdependency_Successor_Task)
                        {
                            Successor_Task_item.IsDeleted = true;
                        }
                    }

                }


                ProjectPhasesTasks projectPhasesTasks = _ProjectPhasesTasksRepository.GetById(PhaseTaskId);

                if (projectPhasesTasks.Type == 1) //MainPhase
                {

                    var projectPhasesTasks_S = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ParentId == projectPhasesTasks.PhaseTaskId); //tasks and submain in MainPhase
                    foreach (var item in projectPhasesTasks_S)
                    {
                        if (item.Type == 2) //subphases in mainphases
                        {

                            var projectPhasesTasks_SS = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ParentId == item.PhaseTaskId); //tasks in subphases in mainphases
                            foreach (var item2 in projectPhasesTasks_SS)
                            {
                                if (item2.Status == 2 || item2.Status == 4)
                                {
                                    //-----------------------------------------------------------------------------------------------------------------
                                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    string ActionNote2 = Resources.General_SavedFailed;
                                   _SystemAction.SaveAction("DeleteProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                    //-----------------------------------------------------------------------------------------------------------------
                                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedToDeleteTaskInPhasestasks_M };//لا يمكنك حذف مرحلة رئيسية بها مهام قيد التشغيل او منتهية
                                }
                            }



                        }
                        else //tasks in main phases
                        {
                            if (item.Status == 2 || item.Status == 4)
                            {
                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote2 = Resources.General_SavedFailed;
                               _SystemAction.SaveAction("DeleteProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                //-----------------------------------------------------------------------------------------------------------------
                                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedToDeleteTaskInPhasestasks_M };
                            }
                        }

                    }  //check

                    foreach (var items in projectPhasesTasks_S) //Delete tasks and submain in maintask
                    {

                        if (items.Type == 2)
                        {
                            ProjectPhasesTasks projectPhasesTasksTasksD1 = _ProjectPhasesTasksRepository.GetById(items.PhaseTaskId); //Delete Submain
                            var projectPhasesTasks_SS = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ParentId == items.PhaseTaskId); //tasks in subphases in mainphases


                            projectPhasesTasksTasksD1.IsDeleted = true;
                            projectPhasesTasksTasksD1.DeleteDate = DateTime.Now;
                            projectPhasesTasksTasksD1.DeleteUser = UserId;
                            _TaamerProContext.SaveChanges();


                            foreach (var item3 in projectPhasesTasks_SS)
                            {
                                ProjectPhasesTasks projectPhasesTasksTasksDT = _ProjectPhasesTasksRepository.GetById(item3.PhaseTaskId);

                                projectPhasesTasksTasksDT.IsDeleted = true;
                                projectPhasesTasksTasksDT.DeleteDate = DateTime.Now;
                                projectPhasesTasksTasksDT.DeleteUser = UserId;
                                _TaamerProContext.SaveChanges();
                            } //Delete tasks in submain


                        }
                        else
                        {
                            ProjectPhasesTasks projectPhasesTasksTasksD1TT = _ProjectPhasesTasksRepository.GetById(items.PhaseTaskId);
                            projectPhasesTasksTasksD1TT.IsDeleted = true;
                            projectPhasesTasksTasksD1TT.DeleteDate = DateTime.Now;
                            projectPhasesTasksTasksD1TT.DeleteUser = UserId;
                            _TaamerProContext.SaveChanges();
                        } //Delete Tasks in MainPhase



                    }
                    projectPhasesTasks.IsDeleted = true;
                    projectPhasesTasks.DeleteDate = DateTime.Now;
                    projectPhasesTasks.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف المهمة رقم " + PhaseTaskId;
                   _SystemAction.SaveAction("DeleteProjectPhasesTasks", "ProjectPhasesTasksService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

                }
                else if (projectPhasesTasks.Type == 2) //SubMain
                {

                    var projectPhasesTasks_S = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ParentId == projectPhasesTasks.PhaseTaskId); //tasks in submain
                    foreach (var item in projectPhasesTasks_S)
                    {
                        if (item.Status == 2 || item.Status == 4)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = Resources.General_SavedFailed;
                           _SystemAction.SaveAction("DeleteProjectPhasesTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedToDeleteTaskInPhasestasks_SM };//لا يمكنك حذف مرحلة فرعية بها مهام قيد التشغيل او منتهية
                        }
                    }

                    foreach (var item1 in projectPhasesTasks_S) //Delete tasks in submain
                    {
                        ProjectPhasesTasks projectPhasesTasksTasksD1 = _ProjectPhasesTasksRepository.GetById(item1.PhaseTaskId);

                        projectPhasesTasksTasksD1.IsDeleted = true;
                        projectPhasesTasksTasksD1.DeleteDate = DateTime.Now;
                        projectPhasesTasksTasksD1.DeleteUser = UserId;
                        _TaamerProContext.SaveChanges();

                    }
                    projectPhasesTasks.IsDeleted = true;
                    projectPhasesTasks.DeleteDate = DateTime.Now;
                    projectPhasesTasks.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف المهمة رقم " + PhaseTaskId;
                   _SystemAction.SaveAction("DeleteProjectPhasesTasks", "ProjectPhasesTasksService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

                }
                else //Tasks
                {
                    projectPhasesTasks.IsDeleted = true;
                    projectPhasesTasks.DeleteDate = DateTime.Now;
                    projectPhasesTasks.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف المهمة رقم " + PhaseTaskId;
                   _SystemAction.SaveAction("DeleteProjectPhasesTasks", "ProjectPhasesTasksService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف المهمة رقم " + PhaseTaskId; ;
               _SystemAction.SaveAction("DeleteProjectPhasesTasks", "ProjectPhasesTasksService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }

        public GeneralMessage ShowmanagerapprovalTask(int PhaseTaskId, int UserId, int BranchId)
        {
            try
            {
                ProjectPhasesTasks projectPhasesTasks = _ProjectPhasesTasksRepository.GetById(PhaseTaskId);
                if (projectPhasesTasks.Status == 4)
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase ="لا يمكنك الحفظ .. المهمة منتهية" };
                }
                else
                {
                    projectPhasesTasks.Managerapproval = 2;
                    _TaamerProContext.SaveChanges();
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }

            }
            catch (Exception)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage AcceptmanagerapprovalTask(int PhaseTaskId, int UserId, int BranchId)
        {
            try
            {
                ProjectPhasesTasks projectPhasesTasks = _ProjectPhasesTasksRepository.GetById(PhaseTaskId);
                if (projectPhasesTasks.Status == 4)
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكنك الحفظ .. المهمة منتهية" };
                }
                else
                {
                    projectPhasesTasks.Managerapproval = 3;
                    _TaamerProContext.SaveChanges();
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }

            }
            catch (Exception)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage ConditionTaskStopR(int PhaseTaskId, int UserId, int BranchId)
        {
            try
            {
                ProjectPhasesTasks projectPhasesTasks = _ProjectPhasesTasksRepository.GetById(PhaseTaskId);

                projectPhasesTasks.IsTemp = false;
                projectPhasesTasks.ReasonsId= null;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تم تحقيق الشرط الإداري و استمرار حركة المخطط" + PhaseTaskId;
                _SystemAction.SaveAction("VoucherTaskStopR", "ProjectPhasesTasksService", 3, "تم تحقيق الشرط الإداري و استمرار حركة المخطط", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم تحقيق الشرط الإداري و استمرار حركة المخطط" };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في تحقيق الشرط الإداري و استمرار حركة المخطط رقم " + PhaseTaskId; ;
                _SystemAction.SaveAction("VoucherTaskStopR", "ProjectPhasesTasksService", 3, "فشل في تحقيق الشرط الإداري و استمرار حركة المخطط ", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage VoucherTaskStop(int PhaseTaskId, int UserId, int BranchId)
        {
            try
            {
                ProjectPhasesTasks ProTaskUpdated = _ProjectPhasesTasksRepository.GetById(PhaseTaskId);
                if (ProTaskUpdated.Status == 4)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " فشل في تحديد مهمة الفاتورة لمهمة منتهية رقم " + PhaseTaskId;
                   _SystemAction.SaveAction("VoucherTaskStop", "ProjectPhasesTasksService", 3, "فشل في توقف حركة سير المشروع عند مهمة منتهية مسبقا", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed};
                }
                else
                {
                    ProTaskUpdated.IsTemp = true;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تحديد مهمة الفاتورة رقم " + PhaseTaskId;
                   _SystemAction.SaveAction("VoucherTaskStop", "ProjectPhasesTasksService", 3, "تم توقف حركة سير المشروع عند هذه المهمة لحين سداد الفاتورة بنجاح", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    #region
                    //SaveOperationsForTask
                    Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                    Pro_TaskOperation.PhaseTaskId = ProTaskUpdated!.PhaseTaskId;
                    Pro_TaskOperation.OperationName = "توقف حركة المهمة لسداد فاتورة";
                    _SystemAction.SaveTaskOperations(Pro_TaskOperation, UserId, BranchId);
                    #endregion
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في تحديد مهمة الفاتورة رقم " + PhaseTaskId;
               _SystemAction.SaveAction("VoucherTaskStop", "ProjectPhasesTasksService", 3, "فشل في توقف حركة سير المشروع عند هذه المهمة لحين سداد الفاتورة", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed};
            }
        }
        public GeneralMessage VoucherTaskStopR(int PhaseTaskId, int UserId, int BranchId)
        {
            try
            {
                ProjectPhasesTasks ProTaskUpdated = _ProjectPhasesTasksRepository.GetById(PhaseTaskId);

                ProTaskUpdated.IsTemp = false;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تحديد مهمة الفاتورة رقم " + PhaseTaskId;
               _SystemAction.SaveAction("VoucherTaskStopR", "ProjectPhasesTasksService", 3, "تم استمرار حركة سير المشروع عند هذه المهمة وتم سداد الفاتورة بنجاح", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                #region
                //SaveOperationsForTask
                Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                Pro_TaskOperation.PhaseTaskId = ProTaskUpdated!.PhaseTaskId;
                Pro_TaskOperation.OperationName = "استمرار حركة المهمة لفاتورة";
                _SystemAction.SaveTaskOperations(Pro_TaskOperation, UserId, BranchId);
                #endregion
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في تحديد مهمة الفاتورة رقم " + PhaseTaskId; ;
               _SystemAction.SaveAction("VoucherTaskStopR", "ProjectPhasesTasksService", 3, "فشل في استمرار حركة سير المشروع عند هذه المهمة ", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed};
            }
        }

        public GeneralMessage TransferTask(int PhaseTaskId, int MainSelectId, int SubSelectId, int UserId, int BranchId)
        {
            try
            {
                ProjectPhasesTasks ProTaskUpdated = _ProjectPhasesTasksRepository.GetById(PhaseTaskId);

                if (SubSelectId > 1)
                {
                    ProTaskUpdated.ParentId = SubSelectId;

                }
                else if (MainSelectId > 1)
                {
                    ProTaskUpdated.ParentId = MainSelectId;

                }
                else
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote3 = " فشل في نقل مهمة رقم " + PhaseTaskId; ;
                   _SystemAction.SaveAction("VoucherTaskStopR", "ProjectPhasesTasksService", 3, "فشل في نقل مهمة ", "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Failed_transfer_task };
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "  نقل المهمة بنجاح رقم " + PhaseTaskId;
               _SystemAction.SaveAction("VoucherTaskStopR", "ProjectPhasesTasksService", 3, Resources.task_successfully_transferred, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                #region
                //SaveOperationsForTask
                Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                Pro_TaskOperation.PhaseTaskId = ProTaskUpdated!.PhaseTaskId;
                Pro_TaskOperation.OperationName = "نقل المهمة";
                _SystemAction.SaveTaskOperations(Pro_TaskOperation, UserId, BranchId);
                #endregion
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.task_successfully_transferred };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في نقل مهمة رقم " + PhaseTaskId; ;
               _SystemAction.SaveAction("VoucherTaskStopR", "ProjectPhasesTasksService", 3, "فشل في نقل مهمة ", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Failed_transfer_task };
            }
        }

        public GeneralMessage RestartTask(int id,string RetrievedReason, int UserId, int BranchId)
        {
            try
            {
                var ProTaskUpdated = _ProjectPhasesTasksRepository.GetById(id);
                if (ProTaskUpdated != null)
                {
                    //ProTaskUpdated.Status = ProjectPhasesTasks.Status;
                    ProTaskUpdated.Status = 3;
                    ProTaskUpdated.IsRetrieved = 1;
                    ProTaskUpdated.RetrievedReason = RetrievedReason;
                    //ProTaskUpdated.Active = false;
                    ProTaskUpdated.UpdateUser = UserId;
                    ProTaskUpdated.UpdateDate = DateTime.Now;
                    _TaamerProContext.SaveChanges();
                }

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " اعادة تشغيل المهمه" + ProTaskUpdated.DescriptionAr;
               _SystemAction.SaveAction("RestartTask", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                //SendMailFinishTask(ProTaskUpdated, BranchId, UserId);
                #region
                //SaveOperationsForTask
                Pro_TaskOperations Pro_TaskOperation = new Pro_TaskOperations();
                Pro_TaskOperation.PhaseTaskId = ProTaskUpdated!.PhaseTaskId;
                Pro_TaskOperation.OperationName = "اعادة تشغيل المهمه";
                _SystemAction.SaveTaskOperations(Pro_TaskOperation, UserId, BranchId);
                #endregion
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في اعادة تشغيل المهمه";
               _SystemAction.SaveAction("RestartTask", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage UpdateIsNew(int TaskId, int UserId, int BranchId)
        {
            var projectno = "0";
            var description = "";
            try
            {
                var ProTaskUpdated = _ProjectPhasesTasksRepository.GetById(TaskId);
              
                if (ProTaskUpdated != null)
                {
                    ProTaskUpdated.IsNew = 0;
                    ProTaskUpdated.UpdateUser = UserId;
                    ProTaskUpdated.UpdateDate = DateTime.Now;
                    description = ProTaskUpdated.DescriptionAr;
                    _TaamerProContext.SaveChanges();
                    projectno = _TaamerProContext.Project.Where(s => s.ProjectId == ProTaskUpdated.ProjectId)!.FirstOrDefault()!.ProjectNo ?? "0";
                }

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "  قراءة المهمه  " +"  "+ ProTaskUpdated.DescriptionAr + "مشروع"+ projectno;
               _SystemAction.SaveAction("UpdateIsNew", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                //SendMailFinishTask(ProTaskUpdated, BranchId, UserId);
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في قراءة المهمة" + "  " + description + "مشروع" + projectno;
                _SystemAction.SaveAction("UpdateIsNew", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public async Task<ProjectPhasesTasksVM> GetTaskById(int TaskId, string Lang,int? UserId)
        {
            var task =await _ProjectPhasesTasksRepository.GetTaskById(TaskId, Lang, UserId);
            return task;
        }
        public int? GetProjectNoById(int? TaskId, string Lang)
        {
            var ProjectIdR = _ProjectPhasesTasksRepository.GetMatching(t => t.PhaseTaskId == TaskId).FirstOrDefault().ProjectId;
            return ProjectIdR;
        }

        public async Task<ProjectPhasesTasksVM> GetTaskByUserId(int TaskId, int UserId)
        {
            var task =await _ProjectPhasesTasksRepository.GetTaskByUserId(TaskId, UserId);
            return task;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllSubPhasesTasks(int SubPhaseId, string Lang)
        {
            var PhaseTasks = await _ProjectPhasesTasksRepository.GetAllSubPhasesTasks(SubPhaseId, Lang);
            return PhaseTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllSubPhasesTasksbyUserId(int SubPhaseId, int UserId)
        {
            var PhaseTasks =await _ProjectPhasesTasksRepository.GetAllSubPhasesTasksbyUserId(SubPhaseId, UserId);
            return PhaseTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllSubPhasesTasksbyUserId2(int SubPhaseId, int UserId)
        {
            var PhaseTasks =await _ProjectPhasesTasksRepository.GetAllSubPhasesTasksbyUserId2(SubPhaseId, UserId);
            return PhaseTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetProjectMainPhasesByProjectId(int ProjectId)
        {
            var PhaseTasks =await _ProjectPhasesTasksRepository.GetProjectMainPhasesByProjectId(ProjectId);
            return PhaseTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetProjectSubPhasesByProjectId(int MainPhaseId)
        {
            var PhaseTasks =await _ProjectPhasesTasksRepository.GetProjectSubPhasesByProjectId(MainPhaseId);
            return PhaseTasks;
        }

        public IEnumerable<object> FillProjectMainPhases(int ProjectId)
        {
            return _ProjectPhasesTasksRepository.GetProjectMainPhasesByProjectId(ProjectId).Result.Select(x => new
            {
                Id = x.PhaseTaskId,
                Name = x.DescriptionAr,
                NameEn = x.DescriptionEn
            });
        }
        public IEnumerable<object> FillUsersTasksVacationSelect(int UserId)
        {
            var PhasesTask = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.IsMerig == -1 && s.Type == 3 && s.UserId == UserId && (s.Status == 2 || s.Status == 1));
            var PhasesTask2 = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.IsMerig == -1 && s.Type == 3 && s.UserId == UserId && s.Status == 10);

            var Vacation = _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && s.DecisionType == 1 && s.UserId == UserId && s.VacationStatus == 2);



            if (Vacation.Count() > 0 && Vacation != null)
            {
                foreach (var vac in Vacation)
                {
                    PhasesTask = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.IsMerig == -1 && s.Type == 3 && s.UserId == UserId && (s.Status == 2 || s.Status == 1)).ToList();
                    PhasesTask = PhasesTask.Where(m => ((vac.StartDate == null || vac.StartDate.Equals("")) || (!(m.ExcpectedStartDate == null || m.ExcpectedStartDate.Equals("")) && DateTime.ParseExact(m.ExcpectedStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(vac.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && ((vac.EndDate == null || vac.EndDate.Equals(""))|| (m.ExcpectedEndDate == null || m.ExcpectedEndDate.Equals("")) || DateTime.ParseExact(m.ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(vac.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)));


                    var Phases = PhasesTask2.Union(PhasesTask);
                    PhasesTask2 = Phases.ToList();
                }

            }

            return PhasesTask2.Select(x => new
            {
                Id = x.PhaseTaskId,
                Name = x.DescriptionAr,
                NameE = x.ExcpectedStartDate
            });
        }

        public IEnumerable<object> FillUsershaveRunningTasks()
        {
            var PhasesTaskUsers = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.IsMerig == -1 &&
                s.Type == 3 && (s.Status == 1 || s.Status == 2 || s.Status == 3)).Select(x => x.UserId).ToList();
            var Users = _UsersRepository.GetMatching(x => !x.IsDeleted && PhasesTaskUsers.Contains(x.UserId)).Select(x => new
            {
                Id = x.UserId,
                Name = x.FullNameAr == null ? x.FullName : x.FullNameAr,
                NameE = x.FullName
            });
            return Users;
        }

        public IEnumerable<object> FillUsershaveRunningTasks(int BranchId)
        {
            var PhasesTaskUsers = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.IsMerig == -1 &&
                s.Type == 3 && (s.Status == 1 || s.Status == 2 || s.Status == 3) && s.BranchId==BranchId).Select(x => x.UserId).ToList();
            var Users = _UsersRepository.GetMatching(x => !x.IsDeleted && PhasesTaskUsers.Contains(x.UserId)).Select(x => new
            {
                Id = x.UserId,
                Name = x.FullNameAr == null ? x.FullName : x.FullNameAr,
                NameE = x.FullName
            });
            return Users;
        }


        public IEnumerable<object> FillUsershaveRunningTasksWithBranch(int BranchId)
        {
            var PhasesTaskUsers = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.IsMerig == -1 &&
                s.Type == 3 && (s.Status == 1 || s.Status == 2 || s.Status == 3) && s.BranchId==BranchId).Select(x => x.UserId).ToList();
            var Users = _UsersRepository.GetMatching(x => !x.IsDeleted && PhasesTaskUsers.Contains(x.UserId)).Select(x => new
            {
                Id = x.UserId,
                Name = x.FullNameAr == null ? x.FullName : x.FullNameAr,
                NameE = x.FullName
            });
            return Users;
        }

        public IEnumerable<object> FillProjectSubPhases(int MainPhaseId)
        {
            return _ProjectPhasesTasksRepository.GetProjectSubPhasesByProjectId(MainPhaseId).Result.Select(x => new
            {
                Id = x.PhaseTaskId,
                Name = x.DescriptionAr,
                NameEn = x.DescriptionEn
            });
        }
        public async Task<int> GetUserTaskCount(int? UserId, int BranchId)
        {
            var UserTaskCount =await _ProjectPhasesTasksRepository.GetUserTaskCount(UserId, BranchId);
            return UserTaskCount;
        }
        private bool SendMail(ProjectPhasesTasks ProjectPhasesTasks, int BranchId, int UserId)
        {
            try
            {
                DateTime date = new DateTime();
                var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                //var DateOfTask = ProjectPhasesTasks.AddDate.Value.ToString("yyyy-MM-dd HH:MM");
                var DateOfTask = ProjectPhasesTasks.AddDate.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                //var str = date.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                var project = _ProjectRepository.GetById((int)ProjectPhasesTasks.ProjectId);
                //string textBody = "<table><tr><td>" + ProjectPhasesTasks.DescriptionAr + ":" + ProjectPhasesTasks.Notes + "</td><td> : لديك مهمه جديدة </td></tr><tr><td>" + ProjectPhasesTasks.Project.ProjectNo + "</td><td> : علي مشروع رقم </td></tr><tr><td>" + ProjectPhasesTasks.Project.customer.CustomerNameAr + "</td><td> : للعميل </td></tr></table>";
                var timeType = "days";
                if (ProjectPhasesTasks.TimeType == 1)
                {
                    timeType = "hours";
                }
                string textBody = "Dear Mr." + ProjectPhasesTasks.Users.FullName + ", you have a new task in Date " + DateOfTask + "<br/> " + "<table border='1'style='text-align:center;padding:3px;'><tr><td style='border=1px solid #eee'>Text Message</td><td>" + ProjectPhasesTasks.DescriptionAr + "</td></tr><tr><td>Project No</td><td>" + ProjectPhasesTasks.Project.ProjectNo + "</td></tr><tr><td>Customer name </td><td>" + ProjectPhasesTasks.Project.customer.CustomerNameAr + "</td></tr><tr><td>Duraion </td><td>" + ProjectPhasesTasks.TimeMinutes + " " + timeType + " </td></tr><tr><td>Start Date</td><td>" + ProjectPhasesTasks.StartDate + "</td></tr></table>";
                var EmailSett = _EmailSettingRepository.GetEmailSetting(branch).Result;
                var mail = new MailMessage();
                var loginInfo = new NetworkCredential(EmailSett.SenderEmail, EmailSett.Password);

                if (EmailSett.DisplayName != null)
                {
                    mail.From = new MailAddress(EmailSett.SenderEmail, EmailSett.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress(EmailSett.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                }
                //mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
                mail.To.Add(new MailAddress(_UsersRepository.GetById((int)ProjectPhasesTasks.UserId).Email));
                //mail.Subject = "لديك مهمة جديده علي مشروع رقم " + project.ProjectNo + "";
                mail.Subject = "New Task";
                try
                {
                    mail.Body = textBody;// "لديك مهمه جديدة : " + ProjectPhasesTasks.DescriptionAr + ":" + ProjectPhasesTasks.Notes + " علي مشروع رقم " + ProjectPhasesTasks.Project.ProjectNo + " للعميل " + ProjectPhasesTasks.Project.customer.CustomerNameAr;
                    mail.IsBodyHtml = true;
                }
                catch (Exception)
                {
                    mail.Body = ProjectPhasesTasks.DescriptionAr + " : " + ProjectPhasesTasks.Notes ?? "";
                }
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(EmailSett.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(EmailSett.Port);

                smtpClient.Send(mail);
                return true;
            }
            catch (Exception wx)
            {
                return false;
            }
        }
        private bool SendMailFinishTask(ProjectPhasesTasks ProjectPhasesTasks, int BranchId, int UserId)
        {
            try
            {
                DateTime date = new DateTime();
                var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                //var DateOfTask = ProjectPhasesTasks.AddDate.Value.ToString("yyyy-MM-dd HH:MM");
                var DateOfTask = ProjectPhasesTasks.UpdateDate.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                var EndOfTask = ProjectPhasesTasks.UpdateDate.Value.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //var str = date.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                var project = _ProjectRepository.GetById((int)ProjectPhasesTasks.ProjectId);
                var Status = "";
                if (DateTime.ParseExact(ProjectPhasesTasks.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(EndOfTask, "yyyy-MM-dd", CultureInfo.InvariantCulture))
                {
                    Status = "On Time";
                }
                else
                {
                    Status = "Delay";
                }
                var timeType = "days";
                if (ProjectPhasesTasks.TimeType == 1)
                {
                    timeType = "hours";
                }
                //string textBody = "<table><tr><td>" + ProjectPhasesTasks.DescriptionAr + ":" + ProjectPhasesTasks.Notes + "</td><td> : لديك مهمه جديدة </td></tr><tr><td>" + ProjectPhasesTasks.Project.ProjectNo + "</td><td> : علي مشروع رقم </td></tr><tr><td>" + ProjectPhasesTasks.Project.customer.CustomerNameAr + "</td><td> : للعميل </td></tr></table>";
                string textBody = "Dear Mr." + ProjectPhasesTasks.Users.FullName + ", Your Task Completed Successfully and was closed in " + DateOfTask + "<br/> " + "<table border='1'style='text-align:center;padding:3px;'><tr><td>Task Description</td><td>" + ProjectPhasesTasks.DescriptionAr + "</td></tr><tr><td>Project No</td><td>" + ProjectPhasesTasks.Project.ProjectNo + "</td></tr><tr><td>Customer name </td><td>" + ProjectPhasesTasks.Project.customer.CustomerNameAr + "</td></tr><tr><td>Duraion </td><td>" + ProjectPhasesTasks.TimeMinutes + " " + timeType + " days</td></tr><tr><td>Start Date</td><td>" + ProjectPhasesTasks.StartDate + "</td></tr><tr><td>Closing Date</td><td>" + DateOfTask + "</td></tr><tr><td>Task Status</td><td>" + Status + "</td></tr></table>";
                var EmailSett = _EmailSettingRepository.GetEmailSetting(branch).Result;
                var mail = new MailMessage();
                var loginInfo = new NetworkCredential(EmailSett.SenderEmail, EmailSett.Password);
                if (EmailSett.DisplayName != null)
                {
                    mail.From = new MailAddress(EmailSett.SenderEmail, EmailSett.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress(EmailSett.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                }
                //mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
                mail.To.Add(new MailAddress(_UsersRepository.GetById((int)ProjectPhasesTasks.UserId).Email));
                //mail.Subject = "لديك مهمة جديده علي مشروع رقم " + project.ProjectNo + "";
                mail.Subject = "TameerCloud Task completed Successfully";
                try
                {
                    mail.Body = textBody;// "لديك مهمه جديدة : " + ProjectPhasesTasks.DescriptionAr + ":" + ProjectPhasesTasks.Notes + " علي مشروع رقم " + ProjectPhasesTasks.Project.ProjectNo + " للعميل " + ProjectPhasesTasks.Project.customer.CustomerNameAr;
                    mail.IsBodyHtml = true;
                }
                catch (Exception)
                {
                    mail.Body = ProjectPhasesTasks.DescriptionAr + " : " + ProjectPhasesTasks.Notes ?? "";
                }
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(EmailSett.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(EmailSett.Port);

                smtpClient.Send(mail);
                return true;
            }
            catch (Exception wx)
            {
                return false;
            }
        }


        private bool SendMailFinishTask2(ProjectPhasesTasks ProjectPhasesTasks, string subject, int BranchId, int UserId, string URL, string ImgUrl, int type, int from,string? CustomerName,string ProjectNo,int MangerId,string? ProjectMangerName)
        {
            try
            {
                DateTime date = new DateTime();
                var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                var org = _OrganizationsRepository.GetById(branch);
                var DateOfTask = ProjectPhasesTasks.UpdateDate.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                var EndOfTask = ProjectPhasesTasks.UpdateDate.Value.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

                var timeType = "يوم";
                if (ProjectPhasesTasks.TimeType == 1)
                {
                    timeType = "ساعه";
                }
                var adduser = "";
                if (ProjectPhasesTasks.AddTaskUserId != null)
                {
                    adduser = _UsersRepository.GetById((int)ProjectPhasesTasks.AddTaskUserId).FullNameAr ?? _UsersRepository.GetById((int)ProjectPhasesTasks.AddTaskUserId).FullName;
                }
                else
                {
                    adduser = "سير مشروع";
                }
                var title = "";
                var body = "";
                if (type == 1)
                {
                    title = "قمت بانهاء المهمة المبينة تفاصيلها في الجدول بتاريخ :" + DateOfTask;
                    body = PopulateBody(ProjectPhasesTasks, ProjectMangerName ?? "", adduser ?? "", DateOfTask, ProjectPhasesTasks.TimeMinutes + " " + timeType, DateOfTask, URL, title ?? "", org!.NameAr??"", CustomerName??"", ProjectNo??"",1);

                }
                else if (type == 2)
                {
                    title = " لقد طلب تمديد مهمة ارسلت اليك بواسطة / " + _UsersRepository.GetById(UserId).FullNameAr ?? _UsersRepository.GetById(UserId).FullName;
                    body = PopulateBody(ProjectPhasesTasks, ProjectMangerName ?? "", adduser ?? "", DateOfTask, ProjectPhasesTasks.TimeMinutes + " " + timeType, DateOfTask, URL, title ?? "", org!.NameAr??"", CustomerName ?? "", ProjectNo ?? "",2);

                }
                else if (type == 3)
                {
                    title = " لقد طلب تحويل مهمة ارسلت اليك بواسطة / " + _UsersRepository.GetById(UserId).FullNameAr ?? _UsersRepository.GetById(UserId).FullName;
                    body = PopulateBody(ProjectPhasesTasks, ProjectMangerName ?? "", adduser ?? "", DateOfTask, ProjectPhasesTasks.TimeMinutes + " " + timeType, DateOfTask, URL, title ?? "", org!.NameAr??"", CustomerName ?? "", ProjectNo ?? "",3);

                }
                else if (type == 4)
                {
                    title = " لقد تم قبول طلبك لتمديد المهمة التي تنتهي في  / " + ProjectPhasesTasks.EndDate;
                    body = PopulateBody(ProjectPhasesTasks, _UsersRepository.GetById(ProjectPhasesTasks!.UserId ?? 0)!.FullName ?? "", adduser ?? "", DateOfTask, ProjectPhasesTasks.TimeMinutes + " " + timeType, DateOfTask, URL, title ?? "", org!.NameAr??"", CustomerName ?? "", ProjectNo ?? "",4);

                }
                else if (type == 5)
                {
                    title = " لديك مهمة جديدة تم تحويلها من   / " + _UsersRepository.GetById(from).FullNameAr ?? _UsersRepository.GetById(from).FullName; ;
                    body = PopulateBody(ProjectPhasesTasks, _UsersRepository.GetById(ProjectPhasesTasks!.UserId ?? 0)!.FullName ?? "", adduser??"", DateOfTask, ProjectPhasesTasks.TimeMinutes + " " + timeType, DateOfTask, URL, title??"", org!.NameAr??"", CustomerName ?? "", ProjectNo ?? "",5);

                }

                else if (type == 6)
                {
                    title = "لديك مهمة جديدة المبين تفاصيلها في الجدول التالي";
                    body = PopulateBody(ProjectPhasesTasks, _UsersRepository.GetById(ProjectPhasesTasks!.UserId ?? 0)!.FullName ?? "", adduser??"", DateOfTask, ProjectPhasesTasks.TimeMinutes + " " + timeType, DateOfTask, URL, title, org!.NameAr??"", CustomerName ?? "", ProjectNo ?? "",6);

                }
                else if (type == 7)
                {
                    title = " رفض تمديد المهمة المبين تفاصيلها في الجدول التالي";
                    body = PopulateBody(ProjectPhasesTasks, _UsersRepository.GetById(ProjectPhasesTasks!.UserId ?? 0)!.FullName ?? "", adduser ?? "", DateOfTask, ProjectPhasesTasks.TimeMinutes + " " + timeType, DateOfTask, URL, title, org!.NameAr ?? "", CustomerName ?? "", ProjectNo ?? "", 7);

                }
                else if (type == 8)
                {
                    title = " رفض تحويل المهمة المبين تفاصيلها في الجدول التالي";
                    body = PopulateBody(ProjectPhasesTasks, _UsersRepository.GetById(ProjectPhasesTasks!.UserId ?? 0)!.FullName ?? "", adduser ?? "", DateOfTask, ProjectPhasesTasks.TimeMinutes + " " + timeType, DateOfTask, URL, title, org!.NameAr ?? "", CustomerName ?? "", ProjectNo ?? "", 8);

                }

                var EmailSett = _EmailSettingRepository.GetEmailSetting(branch).Result;
                var mail = new MailMessage();
                var loginInfo = new NetworkCredential(EmailSett.SenderEmail, EmailSett.Password);
                if (EmailSett.DisplayName != null)
                {
                    mail.From = new MailAddress(EmailSett.SenderEmail, EmailSett.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress(EmailSett.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                }

                System.Net.Mail.LinkedResource logo = new System.Net.Mail.LinkedResource(ImgUrl);
                logo.ContentId = "companylogo";
                // done HTML formatting in the next line to display my bayanatech logo
                AlternateView av1 = AlternateView.CreateAlternateViewFromString(body.Replace("{date}", DateOfTask), null, MediaTypeNames.Text.Html);
                av1.LinkedResources.Add(logo);
                mail.AlternateViews.Add(av1);
                if (type == 4  || type == 6)
                {
                    mail.To.Add(new MailAddress(_UsersRepository.GetById(UserId).Email));

                }
                else if (type == 1 || type == 2 || type == 3 ||type==5 || type==4 || type==7 || type==8)
                {
                    mail.To.Add(new MailAddress(_UsersRepository.GetById(UserId).Email));
                    if (MangerId != null && MangerId > 0)
                    {
                        mail.To.Add(new MailAddress(_UsersRepository.GetById(MangerId).Email));
                    }

                }
                else
                {
                    if (MangerId != null && MangerId > 0)
                    {
                        mail.To.Add(new MailAddress(_UsersRepository.GetById(MangerId).Email));
                    }
                }
                mail.Subject = subject;
                try
                {
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                }
                catch (Exception)
                {
                    mail.Body = ProjectPhasesTasks.DescriptionAr + " : " + ProjectPhasesTasks.Notes ?? "";
                }
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var smtpClient = new SmtpClient(EmailSett.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(EmailSett.Port);
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception wx)
            {
                return false;
            }
        }


        public bool SendMail_ProjectStamp(int BranchId, int UserId, int ReceivedUser, string Subject, string textBody, string Url, string ImgUrl, int type, bool IsBodyHtml = false)
        {
            try
            {
                var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                var org = _OrganizationsRepository.GetById(branch);


                string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                var mail = new MailMessage();
                var EmailSett = _EmailSettingRepository.GetEmailSetting(branch).Result;
                var email = EmailSett.SenderEmail;
                var loginInfo = new NetworkCredential(EmailSett.SenderEmail, EmailSett.Password);
                // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
                if (EmailSett.DisplayName != null)
                {
                    mail.From = new MailAddress(email, EmailSett.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress(email, "لديك اشعار من نظام تعمير السحابي");
                }
                var title = "";
                var body = "";


                if (type == 6)
                {
                    title = "اصدار فاتورة أثناء انشاء المشروع";
                    body = PopulateBody2(textBody, _UsersRepository.GetUserById(ReceivedUser, "rtl").Result.FullName, title, "مع تحيات قسم ادارة المساريع", Url, org.NameAr);
                }

                System.Net.Mail.LinkedResource logo = new System.Net.Mail.LinkedResource(ImgUrl);
                logo.ContentId = "companylogo";
                // done HTML formatting in the next line to display my bayanatech logo
                AlternateView av1 = AlternateView.CreateAlternateViewFromString(body.Replace("{Header}", title), null, MediaTypeNames.Text.Html);
                av1.LinkedResources.Add(logo);
                mail.AlternateViews.Add(av1);
                mail.To.Add(new MailAddress(_UsersRepository.GetById(ReceivedUser).Email));


                mail.Subject = Subject;

                mail.Body = textBody;
                mail.IsBodyHtml = IsBodyHtml;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(EmailSett.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(EmailSett.Port);

                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string PopulateBody2(string bodytxt, string fullname, string header, string footer, string url, string orgname)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(url))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{FullName}", fullname);
            body = body.Replace("{Body}", bodytxt);
            body = body.Replace("{Header}", header);
            body = body.Replace("{Footer}", footer);
            body = body.Replace("{orgname}", orgname);





            return body;
        }




        public string PopulateBody(ProjectPhasesTasks projectphase, string fullname, string addtaskname, string date, string time, string dateoftask, string url, string title, string orgname,string? CustomerName,string ProjectNo,int type)
        {
            string body = string.Empty;
            string reason = "";
            using (StreamReader reader = new StreamReader(url))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{FullName}", fullname);
            body = body.Replace("{date}", title);
            body = body.Replace("{TaskNo}", projectphase.TaskNo);
            body = body.Replace("{Description}", projectphase.DescriptionAr);
            body = body.Replace("{ProjectNember}", ProjectNo??"");
            body = body.Replace("{CustomerName}", CustomerName??"");

            body = body.Replace("{Duration}", time);
            body = body.Replace("{StartDate}", projectphase.StartDateNew.ToString());
            body = body.Replace("{EndDate}", projectphase.EndDateNew.ToString());
            body = body.Replace("{AssUser}", addtaskname);
            body = body.Replace("{orgname}", orgname);
            if(type==2)
            {
             reason = "<tr>\r\n<td>السبب</td>\r\n <td>"+ projectphase.PlusTimeReason + "</td>\r\n  </tr>";

            }
            else if(type==3)
            {
                 reason = "<tr>\r\n<td>السبب</td>\r\n <td>" + projectphase.convertReason + "</td>\r\n  </tr>";

            }
            else if (type == 4 || type==7)
            {
                reason = "<tr>\r\n<td>السبب</td>\r\n <td>" + projectphase.PlusTimeReason_admin + "</td>\r\n  </tr>";

            }
            else if (type == 5 || type == 8)
            {
                reason = "<tr>\r\n<td>السبب</td>\r\n <td>" + projectphase.convertReason_admin + "</td>\r\n  </tr>";

            }
            else
            {
                reason = "";
            }
            body = body.Replace("{Reason}", reason);








            return body;
        }

        private bool SendMailFinishTask3(ProjectPhasesTasks ProjectPhasesTasks, string goalname, int BranchId, int UserId,string? CustomerName)
        {
            try
            {
                DateTime date = new DateTime();
                var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                //var DateOfTask = ProjectPhasesTasks.AddDate.Value.ToString("yyyy-MM-dd HH:MM");
                var DateOfTask = ProjectPhasesTasks.UpdateDate.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                var EndOfTask = ProjectPhasesTasks.UpdateDate.Value.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //var str = date.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                //var project = _ProjectRepository.GetById((int)ProjectPhasesTasks.ProjectId);
                var Status = "";
                if (DateTime.ParseExact(ProjectPhasesTasks.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(EndOfTask, "yyyy-MM-dd", CultureInfo.InvariantCulture))
                {
                    Status = "On Time";
                }
                else
                {
                    Status = "Delay";
                }
                var timeType = "days";
                if (ProjectPhasesTasks.TimeType == 1)
                {
                    timeType = "hours";
                }
                //string textBody = "<table><tr><td>" + ProjectPhasesTasks.DescriptionAr + ":" + ProjectPhasesTasks.Notes + "</td><td> : لديك مهمه جديدة </td></tr><tr><td>" + ProjectPhasesTasks.Project.ProjectNo + "</td><td> : علي مشروع رقم </td></tr><tr><td>" + ProjectPhasesTasks.Project.customer.CustomerNameAr + "</td><td> : للعميل </td></tr></table>";
                string textBody = "  السيد " + _UsersRepository.GetById((int)ProjectPhasesTasks.Project.MangerId).FullName + "   المحترم" + "<br/> " + "السلام عليكم ورحمة الله وبركاتة " + "<br/> " + "<h2>  قمت بتحقيق الهدف للمهمة المبينة تفاصيلها في الجدول بتاريخ   : " + DateOfTask + "  </h2> <br/> " + "<table border='1'style='text-align:center;padding:3px;'><tr><td>وصف المهمة</td><td>" + ProjectPhasesTasks.DescriptionAr + "</td></tr><td> الهدف</td><td>" + goalname + "</td></tr><tr><td>رقم المشروع</td><td>" + ProjectPhasesTasks.Project.ProjectNo + "</td></tr><tr><td>اسم العميل </td><td>" + (CustomerName??"") + "</td></tr><tr><td>المدة </td><td>" + ProjectPhasesTasks.TimeMinutes + " " + timeType + " </td></tr><tr><td>تارخ البداية</td><td>" + ProjectPhasesTasks.ExcpectedStartDate + "</td></tr><tr><td>تاريخ الانتهاء</td><td>" + DateOfTask + "</td></tr></table> <br/> مع تحيات قسم ادارة المشاريع ";
                var EmailSett = _EmailSettingRepository.GetEmailSetting(branch).Result;
                var mail = new MailMessage();
                var loginInfo = new NetworkCredential(EmailSett.SenderEmail, EmailSett.Password);
                if (EmailSett.DisplayName != null)
                {
                    mail.From = new MailAddress(EmailSett.SenderEmail, EmailSett.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress(EmailSett.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                }
                //mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
                mail.To.Add(new MailAddress(_UsersRepository.GetById((int)ProjectPhasesTasks.Project.MangerId).Email));
                //mail.Subject = "لديك مهمة جديده علي مشروع رقم " + project.ProjectNo + "";
                mail.Subject = "تهانينا ، لقد انجزت هدف علي المشروع ";
                try
                {
                    mail.Body = textBody;// "لديك مهمه جديدة : " + ProjectPhasesTasks.DescriptionAr + ":" + ProjectPhasesTasks.Notes + " علي مشروع رقم " + ProjectPhasesTasks.Project.ProjectNo + " للعميل " + ProjectPhasesTasks.Project.customer.CustomerNameAr;
                    mail.IsBodyHtml = true;
                }
                catch (Exception)
                {
                    mail.Body = ProjectPhasesTasks.DescriptionAr + " : " + ProjectPhasesTasks.Notes ?? "";
                }
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(EmailSett.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(EmailSett.Port);

                smtpClient.Send(mail);
                return true;
            }
            catch (Exception wx)
            {
                return false;
            }
        }
        private bool SendMailChangeTaskTime(ProjectPhasesTasks ProjectPhasesTasks, int BranchId, int UserId, int flag)
        {
            try
            {
                DateTime date = new DateTime();
                var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                //var DateOfTask = ProjectPhasesTasks.AddDate.Value.ToString("yyyy-MM-dd HH:MM");
                var DateOfTask = ProjectPhasesTasks.UpdateDate.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                var EndOfTask = ProjectPhasesTasks.UpdateDate.Value.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //var str = date.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                //var project = _ProjectRepository.GetById((int)ProjectPhasesTasks.ProjectId);
                var Status = "";
                if (DateTime.ParseExact(ProjectPhasesTasks.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(EndOfTask, "yyyy-MM-dd", CultureInfo.InvariantCulture))
                {
                    Status = "On Time";
                }
                else
                {
                    Status = "Delay";
                }
                //string textBody = "<table><tr><td>" + ProjectPhasesTasks.DescriptionAr + ":" + ProjectPhasesTasks.Notes + "</td><td> : لديك مهمه جديدة </td></tr><tr><td>" + ProjectPhasesTasks.Project.ProjectNo + "</td><td> : علي مشروع رقم </td></tr><tr><td>" + ProjectPhasesTasks.Project.customer.CustomerNameAr + "</td><td> : للعميل </td></tr></table>";
                string textBody = "";
                if (flag == 1)
                {
                    textBody = "Dear Mr." + ProjectPhasesTasks.Users.FullName + ", Your request to change The task" + ProjectPhasesTasks.DescriptionAr + "End Date is accept and it will be ended in" + ProjectPhasesTasks.EndDate;
                }
                var EmailSett = _EmailSettingRepository.GetEmailSetting(branch).Result;
                var mail = new MailMessage();
                var loginInfo = new NetworkCredential(EmailSett.SenderEmail, EmailSett.Password);

                if (EmailSett.DisplayName != null)
                {
                    mail.From = new MailAddress(EmailSett.SenderEmail, EmailSett.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress(EmailSett.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                }

                //mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
                mail.To.Add(new MailAddress(_UsersRepository.GetById((int)ProjectPhasesTasks.UserId).Email));
                //mail.Subject = "لديك مهمة جديده علي مشروع رقم " + project.ProjectNo + "";
                mail.Subject = "TameerCloud Change Task End date";
                try
                {
                    mail.Body = textBody;// "لديك مهمه جديدة : " + ProjectPhasesTasks.DescriptionAr + ":" + ProjectPhasesTasks.Notes + " علي مشروع رقم " + ProjectPhasesTasks.Project.ProjectNo + " للعميل " + ProjectPhasesTasks.Project.customer.CustomerNameAr;
                    mail.IsBodyHtml = true;
                }
                catch (Exception)
                {
                    mail.Body = ProjectPhasesTasks.DescriptionAr + " : " + ProjectPhasesTasks.Notes ?? "";
                }
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(EmailSett.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(EmailSett.Port);

                smtpClient.Send(mail);
                return true;
            }
            catch (Exception wx)
            {
                return false;
            }
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetInprogressTasksByUserId(int UserId, int BranchId)
        {
            return await _ProjectPhasesTasksRepository.GetInprogressTasksByUserId(UserId, BranchId);
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectCurrentTasks(int ProjectId)
        {
            return await _ProjectPhasesTasksRepository.GetAllProjectCurrentTasks(ProjectId);
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectId(int ProjectId, int BranchId)
        {
            var Tasks =await _ProjectPhasesTasksRepository.GetAllTasksByProjectId(ProjectId, BranchId);
            return Tasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectIdS(int? ProjectId, int? DepartmentId, string DateFrom, string DateTo, int BranchId)
        {
            var Tasks = await _ProjectPhasesTasksRepository.GetAllTasksByProjectIdS(ProjectId,DepartmentId, DateFrom, DateTo, BranchId);
            return Tasks;
        }


        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectIdS(int? ProjectId, int? DepartmentId, string DateFrom, string DateTo, int BranchId,string? SearchText)
        {
            var Tasks = await _ProjectPhasesTasksRepository.GetAllTasksByProjectIdS(ProjectId, DepartmentId, DateFrom, DateTo, BranchId, SearchText);
            return Tasks;
        }


        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectIdW(int BranchId)
        {
            var Tasks = await _ProjectPhasesTasksRepository.GetAllTasksByProjectIdW(BranchId);
            return Tasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectIdW2(string DateFrom, string DateTo, int BranchId)
        {
            var Tasks = await _ProjectPhasesTasksRepository.GetAllTasksByProjectIdW2(DateFrom, DateTo, BranchId);
            return Tasks;
        }
        public async Task<IEnumerable<rptGetEmpDoneTasksVM>> GetDoneTasksDGV(string FromDate, string ToDate, int UserID, string Con)
        {
            var Tasks =await  _ProjectPhasesTasksRepository.GetDoneTasksDGV(FromDate, ToDate, UserID, Con);
            return Tasks;
        }

        public async Task<IEnumerable<rptGetEmpUndergoingTasksVM>> GetUndergoingTasksDGV(string FromDate, string ToDate, int UserID, string Con)
        {
            var Tasks =await _ProjectPhasesTasksRepository.GetUndergoingTasksDGV(FromDate, ToDate, UserID, Con);
            return Tasks;
        }

        public async Task<IEnumerable<rptGetEmpDelayedTasksVM>> GetEmpDelayedTasksDGV(string FromDate, string ToDate, int UserID, string Con)
        {
            var Tasks =await _ProjectPhasesTasksRepository.GetEmpDelayedTasksDGV(FromDate, ToDate, UserID, Con);
            return Tasks;
        }

        public async Task<IEnumerable<rptGetDoneWorkOrdersByExecEmp>> GetEmpDoneWOsDGV(int UserID, string Con)
        {
            var Tasks =await _ProjectPhasesTasksRepository.GetEmpDoneWOsDGV(UserID, Con);
            return Tasks;
        }

        public async Task<IEnumerable<rptGetOnGoingWorkOrdersByExecEmp>> GetEmpUnderGoingWOsDGV(int UserID, string Con)
        {
            var Tasks =await _ProjectPhasesTasksRepository.GetEmpUnderGoingWOsDGV(UserID, Con);
            return Tasks;
        }

        public async Task<IEnumerable<rptGetDelayedWorkOrdersByExecEmpVM>> GetEmpDelayedWOsDGV(int UserID, string Con)
        {
            var Tasks =await _ProjectPhasesTasksRepository.GetEmpDelayedWOsDGV(UserID, Con);
            return Tasks;
        }
        public int GetAllTasksBySubPhase(int SubPhases)
        {
            var Count = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ParentId == SubPhases && s.Type == 3 && s.Status != 1).Count();
            return Count;
        }

        //EditD5
        public GeneralMessage MerigTasks(int[] TasksIdArray, string Description, string Note, int UserId, int BranchId)
        {
            try
            {
                var GTask = _ProjectPhasesTasksRepository.GetById(TasksIdArray[0]);

                var settings = new ProjectPhasesTasks();
                decimal? totalcost = 0;
                var AssginUserId = GTask.UserId;
                var StartDate = GTask.StartDate;
                var EndDate = GTask.EndDate;
                var ExcpectedEndDate = GTask.ExcpectedEndDate;
                var ExcpectedStartDate = GTask.ExcpectedStartDate;
                int? TimeDay = 0;
                var countday = 0;
                var CountHour = 0;
                int? TimeHour = 0;
                for (var i = 0; i < TasksIdArray.Length; i++)
                {
                    var Task = _ProjectPhasesTasksRepository.GetById(TasksIdArray[i]);
                    var taskremoved= _TaamerProContext.TasksDependency.Where(s => s.IsDeleted && s.PredecessorId == Task.SettingId || s.SuccessorId == Task.SettingId).ToList();
                    if(taskremoved.Count()>0)
                    {
                        _TaamerProContext.TasksDependency.RemoveRange(taskremoved);
                    }
                    if (AssginUserId != Task.UserId)
                    {
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_CantMerig };
                    }

                    if (DateTime.ParseExact(ExcpectedStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) > DateTime.ParseExact(Task.ExcpectedStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))
                    {
                        ExcpectedStartDate = Task.ExcpectedStartDate;
                    }
                    if (DateTime.ParseExact(ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(Task.ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))
                    {
                        ExcpectedEndDate = Task.ExcpectedEndDate;
                    }
                    var TaskCost = Task.Cost;
                    if (TaskCost == null)
                    {
                        TaskCost = 0;
                    }
                    totalcost = totalcost + TaskCost;
                    if (Task.TimeType == 1)
                    {
                        CountHour = CountHour + 1;
                        TimeHour = TimeHour + Task.TimeMinutes;
                    }
                    else if (Task.TimeType == 2)
                    {
                        countday = countday + 1;
                        TimeDay = TimeDay + Task.TimeMinutes;
                    }
                }
                if (CountHour == TasksIdArray.Length)
                {
                    settings.TimeType = 1;
                    settings.TimeMinutes = TimeHour;
                    settings.Remaining = TimeHour * 60;
                }
                else
                {
                    var TotalHour = (Convert.ToInt16(TimeHour / 24)) + 1;
                    settings.TimeType = 2;
                    settings.TimeMinutes = TimeDay;
                    settings.Remaining = TimeDay * 60 * 24;
                }

                settings.AddUser = UserId;
                settings.ProjectId = GTask.ProjectId;
                settings.Status = 1;
                settings.BranchId = GTask.BranchId;
                settings.DescriptionAr = Description;
                settings.DescriptionEn = Description;
                settings.AddDate = DateTime.Now;
                settings.Cost = totalcost;
                settings.UserId = GTask.UserId;
                //settings.TimeMinutes = TotalTime;
                settings.Type = 3;
                settings.ParentId = GTask.ParentId;
                settings.ProjSubTypeId = GTask.ProjSubTypeId;
                settings.Notes = Note;
                //settings.StartDate = StartDate;
                //settings.EndDate = EndDate;
                settings.ExcpectedStartDate = ExcpectedStartDate;
                settings.ExcpectedEndDate = ExcpectedEndDate;
                settings.TaskType = GTask.TaskType;
                settings.PhasePriority = 5;
                settings.IsMerig = -1;
                settings.IsConverted = 0;
                _TaamerProContext.ProjectPhasesTasks.Add(settings);
                _TaamerProContext.SaveChanges();
                for (var i = 0; i < TasksIdArray.Length; i++)
                {
                    var UpdatedTask = _TaamerProContext.ProjectPhasesTasks.Where(s => s.PhaseTaskId == TasksIdArray[i]).FirstOrDefault();
                    if(UpdatedTask !=null)
                    {
                        UpdatedTask.IsMerig = settings.PhaseTaskId;
                        UpdatedTask.UpdateDate = DateTime.Now;
                        UpdatedTask.UpdateUser = UserId;
                    }
                }
                _TaamerProContext.SaveChanges(); 
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "دمج مهام";
               _SystemAction.SaveAction("MerigTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception x)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
               _SystemAction.SaveAction("MerigTasks", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectTasksByPhaseId(int id)
        {
            var Settings =await _ProjectPhasesTasksRepository.GetAllProjectTasksByPhaseId(id);
            return Settings;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetProjectsWithProSettingVM()
        {
            var Settings =await _ProjectPhasesTasksRepository.GetProjectsWithProSettingVM();
            return Settings;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetProjectsWithoutProSettingVM()
        {
            var Settings =await _ProjectPhasesTasksRepository.GetProjectsWithoutProSettingVM();
            return Settings;
        }

        private bool SendMailNoti(int ProjectID, string Desc, string Subject, int BranchId, int UserId, int ToUserID)
        {
            try
            {
                string Email = _UsersRepository.GetById(ToUserID).Email ?? "";

                if (Email != "")
                {
                    var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                    string textBody = Desc;
                    var EmailSett = _EmailSettingRepository.GetEmailSetting(branch).Result;
                    var mail = new MailMessage();
                    var loginInfo = new NetworkCredential(EmailSett.SenderEmail, EmailSett.Password);
                    if (EmailSett.DisplayName != null)
                    {
                        mail.From = new MailAddress(EmailSett.SenderEmail, EmailSett.DisplayName);
                    }
                    else
                    {
                        mail.From = new MailAddress(EmailSett.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                    }

                    //mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
                    mail.To.Add(new MailAddress(Email));
                    mail.Subject = Subject;
                    mail.Body = textBody;
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var smtpClient = new SmtpClient(EmailSett.Host);
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = loginInfo;
                    //smtpClient.Port = 587;
                    smtpClient.Port = Convert.ToInt32(EmailSett.Port);

                    smtpClient.Send(mail);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception wx)
            {
                return false;
            }
        }


        public RptAllEmpPerformance getempdata(ProjectPhasesTasksVM Search, string Lang, string Con)
        {
            try
            {
                if (Search.BranchId == null)
                {
                    Search.BranchId = 0;

                }
                string Today = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                bool? AllStatusExptEnd = null;
                //var AccBranchesList = BranchesList ?? new List<int>();
                var AccBranchesList = new List<int>();
                if (AccBranchesList.Count() == 0)
                {
                    AccBranchesList.Add(Search.BranchId??0);
                }

                RptAllEmpPerformance rptAll = new RptAllEmpPerformance();
                rptAll.Latetask = _ProjectPhasesTasksRepository.GetLateTasksByUserIdreport(Search.StartDate, Search.EndDate, Search.UserId, (int)Search.BranchId, Lang).Result.Count().ToString();
                rptAll.Inprogress = _ProjectPhasesTasksRepository.GetTasksINprogressByUserIdUser(Search.UserId, Lang, 2, (int)Search.BranchId, Search.StartDate, Search.EndDate).Result.Count().ToString();
                rptAll.Notstarted = _ProjectPhasesTasksRepository.GetNewTasksByUserIdReport(Search.StartDate, Search.EndDate ?? "", Search.UserId, (int)Search.BranchId, Lang, AllStatusExptEnd).Result.Count().ToString();
                rptAll.Completed = GetEmpDoneWOsDGV((int)Search.UserId, Con).Result.Count().ToString();
                rptAll.Retrived = GetAllProjectPhasesTasksbystatus(Search.UserId, (int)Search.BranchId, 7, Lang, Search.StartDate, Search.EndDate, AccBranchesList).Result.Count().ToString();
                rptAll.UserName = _UsersRepository.GetById((int)Search.UserId).FullName;
                rptAll.CompletePercentage = (_homerservice.GetAllUserStatistics((int)Search.UserId, (int)Search.BranchId, Lang).Result.TotalInProressCount).ToString();
                rptAll.LatePercentage = (_homerservice.GetAllUserStatistics((int)Search.UserId, (int)Search.BranchId, Lang).Result.TotalLateCount).ToString();
                // rptAll.LatePercentage = _ProjectPhasesTasksRepository.GetProjectPhasesTasksCountByStatus(null, 2, (int)Search.BranchId).ToString();GetAllProjectPhasesTasksU
                rptAll.AlmostLate = _ProjectPhasesTasksRepository.GetAlmostLateTasksByUserId((int)Search.UserId, Search.StartDate, Search.EndDate, (int)Search.BranchId, Lang).Result.Count().ToString();

                rptAll.ProjectInProgress = _ProjectRepository.GetProjectsInprogress((int)Search.UserId, (int)Search.BranchId).Result.Count().ToString();
                rptAll.ProjectLate = _ProjectRepository.GetProjectsLatereport((int)Search.UserId, (int)Search.BranchId, Search.StartDate, Search.EndDate ?? Today).Result.Count().ToString();
                rptAll.ProjectStoped = _ProjectRepository.GetProjectsStopped((int)Search.UserId, (int)Search.BranchId).Result.Count().ToString();
                rptAll.ProjectWithout = _ProjectRepository.GetProjectsWithoutaction((int)Search.UserId, (int)Search.BranchId).Result.Count().ToString();
                rptAll.ProjectAlmostfinish = _projectService.GetAlmostFinish((int)Search.UserId, (int)Search.BranchId).Result.Count().ToString();
                //    allEmpPerformances.Add(rptAll);
                //}




                return rptAll;
            }
            catch (Exception ex)
            {
                return new RptAllEmpPerformance();

            }
        }

        public List<RptAllEmpPerformance> getempdataNew(ProjectPhasesTasksVM Search, string Lang, string Con, int BranchId)
        {
            try
            {
                string Today = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                var FullReport = _UsersRepository.GetFullReport(Search, Lang, Today, BranchId).Result.ToList().Where(s => s.UserId != 1);
                List<RptAllEmpPerformance> allEmpPerformances = new List<RptAllEmpPerformance>();

                foreach (var rptAll in FullReport)
                {

                    RptAllEmpPerformance rptAllNew = new RptAllEmpPerformance();

                    rptAllNew.UserName = rptAll.FullNameAr == "" ? rptAll.FullName : rptAll.FullNameAr;
                    var ProjectLateVar = rptAll.ProjectLate_VM.Where(s => DateTime.ParseExact(s.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(Today, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                    var ProjectInProgressVar = rptAll.ProjectInProgress_VM.Where(s => DateTime.ParseExact(s.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) > DateTime.ParseExact(Today, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                    var ProjectAlmostfinishVar = rptAll.ProjectAlmostfinish_VM.Where(s => DateTime.ParseExact(s.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) > DateTime.ParseExact(Today, "yyyy-MM-dd", CultureInfo.InvariantCulture));

                    var ProjectWithoutVar1 = rptAll.ProjectWithout_VM;
                    var ProjectWithoutVar2 = rptAll.ProjectWithout_VM2.Where(s => (DateTime.ParseExact(s.MotionProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddDays(30) < DateTime.ParseExact(Today, "yyyy-MM-dd", CultureInfo.InvariantCulture)));


                    if (Search.StartDate == "" || Search.StartDate == null || Search.EndDate == "" || Search.EndDate == null)
                    {

                        if (rptAll.AllPhases_VM.Count() > 0)
                        {
                            rptAllNew.CompletePercentage = ((Convert.ToDecimal(rptAll.AllFinishPhases_VM.Count()) / (Convert.ToDecimal(rptAll.AllPhases_VM.Count())) * 100)).ToString();
                            rptAllNew.LatePercentage = ((Convert.ToDecimal(rptAll.AllLatePhases_VM.Count()) / (Convert.ToDecimal(rptAll.AllPhases_VM.Count())) * 100)).ToString();
                        }
                        rptAllNew.Completed = rptAll.AllFinishPhases_VM.Count().ToString();
                        rptAllNew.Latetask = rptAll.Latetask_VM.Count().ToString();
                        rptAllNew.Inprogress = rptAll.Inprogress_VM.Count().ToString();
                        rptAllNew.StoppedTasks = rptAll.StoppedTasks_VM.Count().ToString();

                        rptAllNew.Notstarted = rptAll.Notstarted_VM.Count().ToString();
                        rptAllNew.Retrived = rptAll.Retrived_VM.Count().ToString();
                        rptAllNew.AlmostLate = rptAll.AlmostLate_VM.Count().ToString();
                        rptAllNew.ProjectInProgress = ProjectInProgressVar.Count().ToString();
                        rptAllNew.ProjectLate = ProjectLateVar.Count().ToString();

                        rptAllNew.ProjectStoped = rptAll.ProjectStoped_VM.Count().ToString();
                        rptAllNew.ProjectWithout = (ProjectWithoutVar1.Count() + ProjectWithoutVar2.Count()).ToString();

                        rptAllNew.ProjectAlmostfinish = ProjectAlmostfinishVar.Count().ToString();
                    }
                    else
                    {

                        var AllPhasescount = rptAll.AllPhases_VM.Where(m => (string.IsNullOrEmpty(Search.StartDate) || (!string.IsNullOrEmpty(m.StartDate) && DateTime.ParseExact(m.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Search.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(Search.EndDate) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Search.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Count();
                        var AllFinishPhasescount = rptAll.AllFinishPhases_VM.Where(m => (string.IsNullOrEmpty(Search.StartDate) || (!string.IsNullOrEmpty(m.StartDate) && DateTime.ParseExact(m.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Search.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(Search.EndDate) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Search.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Count();
                        var AllLatePhasescount = rptAll.AllLatePhases_VM.Where(m => (string.IsNullOrEmpty(Search.StartDate) || (!string.IsNullOrEmpty(m.StartDate) && DateTime.ParseExact(m.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Search.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(Search.EndDate) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Search.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Count();

                        if (AllPhasescount > 0)
                        {
                            rptAllNew.CompletePercentage = ((Convert.ToDecimal(AllFinishPhasescount) / (Convert.ToDecimal(AllPhasescount)) * 100)).ToString();
                            rptAllNew.LatePercentage = ((Convert.ToDecimal(AllLatePhasescount) / (Convert.ToDecimal(AllPhasescount)) * 100)).ToString();
                        }

                        //rptAllNew.ProjectLate = ProjectLateVar.Count().ToString();
                        rptAllNew.Completed = rptAll.AllFinishPhases_VM.Where(m => (string.IsNullOrEmpty(Search.StartDate) || (!string.IsNullOrEmpty(m.StartDate) && DateTime.ParseExact(m.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Search.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(Search.EndDate) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Search.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Count().ToString();
                        rptAllNew.Latetask = rptAll.Latetask_VM.Where(m => (string.IsNullOrEmpty(Search.StartDate) || (!string.IsNullOrEmpty(m.StartDate) && DateTime.ParseExact(m.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Search.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(Search.EndDate) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Search.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Count().ToString();
                        rptAllNew.Inprogress = rptAll.Inprogress_VM.Where(m => (string.IsNullOrEmpty(Search.StartDate) || (!string.IsNullOrEmpty(m.StartDate) && DateTime.ParseExact(m.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Search.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(Search.EndDate) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Search.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Count().ToString();
                        rptAllNew.StoppedTasks = rptAll.StoppedTasks_VM.Where(m => (string.IsNullOrEmpty(Search.StartDate) || (!string.IsNullOrEmpty(m.StartDate) && DateTime.ParseExact(m.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Search.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(Search.EndDate) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Search.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Count().ToString();

                        rptAllNew.Notstarted = rptAll.Notstarted_VM.Where(m => (string.IsNullOrEmpty(Search.StartDate) || (!string.IsNullOrEmpty(m.StartDate) && DateTime.ParseExact(m.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Search.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(Search.EndDate) || string.IsNullOrEmpty(m.EndDate) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Search.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Count().ToString();
                        rptAllNew.Retrived = rptAll.Retrived_VM.Where(m => (string.IsNullOrEmpty(Search.StartDate) || (!string.IsNullOrEmpty(m.StartDate) && DateTime.ParseExact(m.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Search.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(Search.EndDate) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Search.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Count().ToString();
                        rptAllNew.AlmostLate = rptAll.AlmostLate_VM.Where(m => (string.IsNullOrEmpty(Search.StartDate) || (!string.IsNullOrEmpty(m.StartDate) && DateTime.ParseExact(m.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Search.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(Search.EndDate) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Search.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Count().ToString();
                        rptAllNew.ProjectInProgress = ProjectInProgressVar.Where(m => (string.IsNullOrEmpty(Search.StartDate) || (!string.IsNullOrEmpty(m.ProjectDate) && DateTime.ParseExact(m.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Search.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(Search.EndDate) || string.IsNullOrEmpty(m.ProjectExpireDate) || DateTime.ParseExact(m.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Search.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Count().ToString();
                        rptAllNew.ProjectLate = ProjectLateVar.Where(m => (string.IsNullOrEmpty(Search.StartDate) || (!string.IsNullOrEmpty(m.ProjectDate) && DateTime.ParseExact(m.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Search.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(Search.EndDate) || string.IsNullOrEmpty(m.ProjectExpireDate) || DateTime.ParseExact(m.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Search.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Count().ToString();
                        rptAllNew.ProjectStoped = rptAll.ProjectStoped_VM.Where(m => (string.IsNullOrEmpty(Search.StartDate) || (!string.IsNullOrEmpty(m.ProjectDate) && DateTime.ParseExact(m.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Search.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(Search.EndDate) || string.IsNullOrEmpty(m.ProjectExpireDate) || DateTime.ParseExact(m.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Search.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Count().ToString();
                        rptAllNew.ProjectAlmostfinish = ProjectAlmostfinishVar.Where(m => (string.IsNullOrEmpty(Search.StartDate) || (!string.IsNullOrEmpty(m.ProjectDate) && DateTime.ParseExact(m.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Search.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(Search.EndDate) || string.IsNullOrEmpty(m.ProjectExpireDate) || DateTime.ParseExact(m.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Search.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Count().ToString();

                        var promo1 = ProjectWithoutVar1.Where(m => (string.IsNullOrEmpty(Search.StartDate) || (!string.IsNullOrEmpty(m.ProjectDate) && DateTime.ParseExact(m.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Search.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(Search.EndDate) || string.IsNullOrEmpty(m.ProjectExpireDate) || DateTime.ParseExact(m.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Search.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Count().ToString();
                        var promo2 = ProjectWithoutVar2.Where(m => (string.IsNullOrEmpty(Search.StartDate) || (!string.IsNullOrEmpty(m.ProjectDate) && DateTime.ParseExact(m.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Search.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(Search.EndDate) || string.IsNullOrEmpty(m.ProjectExpireDate) || DateTime.ParseExact(m.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Search.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Count().ToString();
                        rptAllNew.ProjectWithout = promo1 + promo2;


                    }
                    allEmpPerformances.Add(rptAllNew);

                    //------------------------------------------------------------

                    //----------------------------------------------------------
                }

                return allEmpPerformances;

            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public async Task<List<RptAllEmpPerformance>> getempdataNew_Proc(PerformanceReportVM Search, string Lang, string Con, int BranchId)
        {
            var data = await _UsersRepository.getempdataNew_Proc(Search, Lang, Con, BranchId);
            return data;
        }
        public GeneralMessage UpdateprojectphaseRequirment(int projectphaseid, int status, int UserId, int BranchId)
        {
            try
            {

                var project = _ProjectPhasesTasksRepository.GetById(projectphaseid);
                if (status == 0)
                {
                    project.ProjectGoals = null;
                }
                else
                {
                    project.ProjectGoals = status;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تعديل  هدف";
               _SystemAction.SaveAction("UpdateprojectphaseRequirment", "ProjectPhasesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تعديل الهدف";
               _SystemAction.SaveAction("UpdateprojectphaseRequirment", "ProjectPhasesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public Task<IEnumerable<Pro_TaskOperationsVM>> GetTaskOperationsByTaskId(int PhasesTaskId)
        {
            var Tasks = _ProjectPhasesTasksRepository.GetTaskOperationsByTaskId(PhasesTaskId);
            return Tasks;
        }

        public GeneralMessage SaveTaskOperations(Pro_TaskOperations TaskOperations, int UserId, int BranchId)
        {
            try
            {
                TaskOperations.UserId = UserId;
                TaskOperations.BranchId = BranchId;
                TaskOperations.AddUser = UserId;
                TaskOperations.AddDate = DateTime.Now;
                _TaamerProContext.Pro_TaskOperations.Add(TaskOperations);
                _TaamerProContext.SaveChanges();
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public async Task<string> GenerateNextTaskNumber(int BranchId, int? ProjectId)
        {
            var codePrefix = "TSK#";
            //var prostartcode = _BranchesRepository.GetById(BranchId).TaskStartCode;
            //if (prostartcode != null && prostartcode != "")
            //{
            //    codePrefix = prostartcode;
            //}

            //var Value = _ProjectPhasesTasksRepository.GenerateNextTaskNumber(BranchId, codePrefix, 0).Result;
            //var NewValue = string.Format("{0:000000}", Value);
            //if (codePrefix != "")
            //{
            //    NewValue = codePrefix + NewValue;
            //}


            var Value = _ProjectPhasesTasksRepository.GenerateNextTaskNumber(BranchId, codePrefix, 0).Result;
            var NewValue = string.Format("{0:000000}", Value);
            NewValue = codePrefix + NewValue;
            return (NewValue);
        }

        public async Task<string> GetTaskCode_S(int BranchId, int? ProjectId)
        {
            var codePrefix = "";
            var prostartcode = _BranchesRepository.GetById(BranchId).TaskStartCode;
            if (prostartcode != null && prostartcode != "")
            {
                codePrefix = prostartcode;
            }
            return codePrefix;
        }

    }
}

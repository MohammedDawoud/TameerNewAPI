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
    public class DependencySettingsService :   IDependencySettingsService
    {
        private readonly IDependencySettingsRepository _DependencySettingsRepository; 
        private readonly ISettingsRepository _SettingsRepository;
        private readonly INodeLocationsRepository _NodeLocationsRepository;
        private readonly IUserBranchesRepository _branchRepository;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly IUsersRepository _UsersRepository;
        private readonly IUserBranchesRepository _UserBranchesRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IProSettingDetailsRepository _ProSettingDetailsRepository;


        public DependencySettingsService(TaamerProjectContext dataContext, ISettingsRepository SettingsRepository
            , ISystemAction systemAction, IDependencySettingsRepository DependencySettingsRepository,
            INodeLocationsRepository NodeLocationsRepository, IUserBranchesRepository branchRepository,
            IBranchesRepository BranchesRepository, IUsersRepository UsersRepository, IUserBranchesRepository UserBranchesRepository
            , IProSettingDetailsRepository ProSettingDetailsRepository
            )
        {
            _DependencySettingsRepository = DependencySettingsRepository;
            _SettingsRepository = SettingsRepository;
            _NodeLocationsRepository = NodeLocationsRepository;
            _BranchesRepository = BranchesRepository;
            _branchRepository = branchRepository;
            _UsersRepository = UsersRepository;
            _UserBranchesRepository = UserBranchesRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _ProSettingDetailsRepository = ProSettingDetailsRepository;

        }

        public Task<IEnumerable<DependencySettingsVM>> GetAllDependencySettings(int? SuccessorId, int BranchId)
        {
            var dependencies = _DependencySettingsRepository.GetAllDependencySettings(SuccessorId, BranchId);
            return dependencies;
        }
        public List<AccountTreeVM> GetProjSubTypeIdSettingTree(int ProjectSubTypeId, int BranchId)
        {
            //var sett = _SettingsRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == ProjectSubTypeId && s.BranchId == BranchId && s.IsMerig == null).OrderBy(s => s.SettingId);
            var sett = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjectSubTypeId && s.BranchId == BranchId && s.IsMerig == null).OrderBy(s => s.SettingId);


            if (sett != null && sett.Count() > 0)
            {
                List<AccountTreeVM> treeItems = new List<AccountTreeVM>();
                foreach (var item in sett)
                {
                    treeItems.Add(new AccountTreeVM(item.SettingId.ToString(), ((item.ParentId == 0 || item.ParentId == null) ? "#" : item.ParentId.ToString()), item.DescriptionAr = item.DescriptionAr));
                }
                return treeItems;
            }
            else
            {
                return new List<AccountTreeVM>();
            }
        }
        public GeneralMessage SaveDependencySettings(int ProjectSubTypeId, List<DependencySettings> TaskLink, List<NodeLocations> NodeLocList,int UserId, int BranchId)
        {
            try
            {
                //  var existingDependencySettings = _DependencySettingsRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == ProjectSubTypeId);
                var existingDependencySettings = _TaamerProContext.DependencySettings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjectSubTypeId);


                if (existingDependencySettings != null && existingDependencySettings.Count() > 0 )
                {
                    _TaamerProContext.DependencySettings.RemoveRange(existingDependencySettings);
                }
                if(TaskLink!=null)
                {
                    foreach (var item in TaskLink)
                    {
                        if (item.PredecessorId != null && item.SuccessorId != null)
                        {
                            if (item.PredecessorId == 0 || item.SuccessorId == 0)
                            {

                            }
                            else
                            {
                                var dependncy = new DependencySettings();
                                dependncy.PredecessorId = item.PredecessorId;
                                dependncy.SuccessorId = item.SuccessorId;
                                dependncy.Type = 0;
                                dependncy.ProjSubTypeId = ProjectSubTypeId;
                                dependncy.IsDeleted = false;
                                dependncy.AddDate = DateTime.Now;
                                dependncy.AddUser = UserId;
                                dependncy.BranchId = BranchId;
                                _TaamerProContext.DependencySettings.Add(dependncy);
                            }

                        }
                    }

                }
                // var existingNodeLocation = _NodeLocationsRepository.GetMatching(s => s.ProSubTypeId == ProjectSubTypeId);
                var existingNodeLocation = _TaamerProContext.NodeLocations.Where(s => s.ProSubTypeId == ProjectSubTypeId);

                if (existingNodeLocation != null && existingDependencySettings.Count() > 0)
                {
                    _TaamerProContext.NodeLocations.RemoveRange(existingNodeLocation);
                }
                if(NodeLocList!=null)
                {
                    foreach (var item in NodeLocList)
                    {

                        //var SettingTasks=_SettingsRepository.GetMatching(s => s.SettingId == item.SettingId && s.Type==3 && s.ProjSubTypeId==ProjectSubTypeId).Select(x=>x.ParentId);
                        // var SettingPhase = _SettingsRepository.GetMatching(s => s.SettingId == item.SettingId && s.Type != 3 && s.ProjSubTypeId == ProjectSubTypeId && s.IsDeleted==false).Select(x=>x.SettingId);
                        var SettingPhase = _TaamerProContext.Settings.Where(s => s.SettingId == item.SettingId && s.Type != 3 && s.ProjSubTypeId == ProjectSubTypeId && s.IsDeleted == false).Select(x => x.SettingId);

                        foreach (var i in SettingPhase)
                        {
                            //var count=_SettingsRepository.GetMatching(s=>s.Type == 3 && s.ProjSubTypeId == ProjectSubTypeId && s.ParentId==i && s.IsDeleted==false).Count();

                            var count = _TaamerProContext.Settings.Where(s => s.Type == 3 && s.ProjSubTypeId == ProjectSubTypeId && s.ParentId == i && s.IsDeleted == false).Count();

                            if (count==0)
                            {
                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote2 = "فشل في حفظ سير المهام";
                                 _SystemAction.SaveAction("SaveDependencySettings", "DependencySettingsService", 1, Resources.CanNotSaveTheStepsWithOutTasks, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                //-----------------------------------------------------------------------------------------------------------------

                                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.CanNotSaveTheStepsWithOutTasks };
                            }
                        }

                        var Loc = new NodeLocations();
                        Loc.ProSubTypeId = ProjectSubTypeId;
                        Loc.SettingId = item.SettingId;
                        Loc.Location = item.Location;
                        Loc.AddDate = DateTime.Now;
                        Loc.AddUser = UserId;
                        _TaamerProContext.NodeLocations.Add(item);
                        _TaamerProContext.SaveChanges();
                        //var relatedSett = _SettingsRepository.GetById(item.SettingId);
                        Settings? relatedSett = _TaamerProContext.Settings.Where(s => s.SettingId == item.SettingId).FirstOrDefault();
                        if (relatedSett != null)
                        {
                            relatedSett.LocationId = item.LocationId;
                        }

                        
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "حفظ سير المهام ل  " + _TaamerProContext.ProjectSubTypes.FirstOrDefault(x => x.SubTypeId == ProjectSubTypeId).NameAr ;
                     _SystemAction.SaveAction("SaveDependencySettings", "DependencySettingsService", 1, Resources.TasksSavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.TasksSavedSuccessfully};
                }
                else
                {
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ  سير المهام" + _TaamerProContext.ProjectSubTypes.FirstOrDefault(x => x.SubTypeId == ProjectSubTypeId).NameAr;
                    _SystemAction.SaveAction("SaveDependencySettings", "DependencySettingsService", 1, Resources.CanNotSaveTheStepsWithOutTasks, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.CanNotSaveTheStepsWithOutTasks };
                }

            }
            catch (Exception ex)
            {
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ حفظ سير المهام";
                 _SystemAction.SaveAction("SaveDependencySettings", "DependencySettingsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage SaveDependencySettingsNew(int ProjectSubTypeId, List<DependencySettingsNew> TaskLinkList, List<SettingsNew> TasksList, int UserId, int BranchId)
        {
            try
            {
                var existingSettings = _TaamerProContext.SettingsNew.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjectSubTypeId);

                if (existingSettings != null && existingSettings.Count() > 0)
                {
                    _TaamerProContext.SettingsNew.RemoveRange(existingSettings);
                }
                if (TasksList != null) {

                    foreach (var settings in TasksList)
                    {
                        if (settings.Type == 3)
                        {
                            Users? BranchIdOfUser = _TaamerProContext.Users.Where(s => s.UserId == settings.UserId).FirstOrDefault();
                            var StartDateV = (settings.StartDate??DateTime.Now).ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            var EndDateV = (settings.EndDate ?? DateTime.Now).ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

                            var UserVacation = _TaamerProContext.Vacation.AsEnumerable().Where(s => s.IsDeleted == false && s.UserId == settings.UserId && s.VacationStatus == 2 && s.DecisionType == 1 && (s.BackToWorkDate == null || (s.BackToWorkDate ?? "") == "")).ToList();
                            UserVacation = UserVacation.Where(s =>

                            ((!(s.StartDate == null || s.StartDate.Equals("")) && !(settings.StartDate == null || settings.StartDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(StartDateV, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                                (!(s.StartDate == null || s.StartDate.Equals("")) && !(settings.EndDate == null || settings.EndDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(EndDateV, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                                ||
                                ((!(s.EndDate == null || s.EndDate.Equals("")) && !(settings.StartDate == null || settings.StartDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(StartDateV, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                                (!(s.EndDate == null || s.EndDate.Equals("")) && !(settings.EndDate == null || settings.EndDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(EndDateV, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                                ||

                                ((!(s.StartDate == null || s.StartDate.Equals("")) && !(settings.StartDate == null || settings.StartDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(StartDateV, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                                (!(s.EndDate == null || s.EndDate.Equals("")) && !(settings.StartDate == null || settings.StartDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(StartDateV, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                                ||
                                ((!(s.StartDate == null || s.StartDate.Equals("")) && !(settings.EndDate == null || settings.EndDate.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(EndDateV, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                                (!(s.EndDate == null || s.EndDate.Equals("")) && !(settings.EndDate == null || settings.EndDate.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(EndDateV, "yyyy-MM-dd", CultureInfo.InvariantCulture)))

                            ).ToList();
                            if (UserVacation.Count() != 0)
                            {
                                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Proj_SaveFailedUserVacationProSetting };
                            }


                            var DateDiff = ((settings.EndDate??new DateTime()) - (settings.StartDate ?? new DateTime()));
                            var TimeType = 1;
                            var TimeMinutes = 0;

                            if (DateDiff.TotalHours < 24)
                            {
                                TimeType = 1;
                                TimeMinutes =Convert.ToInt32(DateDiff.TotalHours);
                            }
                            else
                            {
                                TimeType = 2;
                                TimeMinutes = Convert.ToInt32(DateDiff.TotalDays)+1;
                            }
                            settings.TimeType = TimeType;
                            settings.TimeMinutes = TimeMinutes;

                            settings.BranchId = BranchIdOfUser.BranchId;
                            settings.EndTime = DateTime.Now.ToString("h:mm");                         
                        }
                        else
                        {
                            settings.BranchId = BranchId;

                        }
                        
                        if (settings.SettingId == 0)
                        {
                            settings.AddUser = UserId;
                            settings.IsMerig = -1;                         
                            settings.AddDate = DateTime.Now;
                            _TaamerProContext.SettingsNew.Add(settings);

                        }
                    }
                    _TaamerProContext.SaveChanges();

                    var tasksEdit = _TaamerProContext.SettingsNew.Where(s => (s.Type == 2 || s.Type == 3) && s.ProjSubTypeId == ProjectSubTypeId && s.IsDeleted == false).ToList();

                    foreach (var taskk in tasksEdit)
                    {
                        var ParentIdV = _TaamerProContext.SettingsNew.Where(s => s.taskindex == taskk.ParentId && s.ProjSubTypeId == ProjectSubTypeId && s.IsDeleted == false).Select(x => x.SettingId).FirstOrDefault();
                        taskk.ParentId = ParentIdV;
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------
                    var existingDependencySettings = _TaamerProContext.DependencySettingsNew.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjectSubTypeId);

                    if (existingDependencySettings != null && existingDependencySettings.Count() > 0)
                    {
                        _TaamerProContext.DependencySettingsNew.RemoveRange(existingDependencySettings);
                    }


                    if (TaskLinkList != null)
                    {
                        foreach (var item in TaskLinkList)
                        {
                            var PredecessorIdV = _TaamerProContext.SettingsNew.Where(s => s.taskindex == item.PredecessorId && s.Type == 3 && s.ProjSubTypeId == ProjectSubTypeId && s.IsDeleted == false).Select(x => x.SettingId).FirstOrDefault();
                            var SuccessorIdV = _TaamerProContext.SettingsNew.Where(s => s.taskindex == item.SuccessorId && s.Type == 3 && s.ProjSubTypeId == ProjectSubTypeId && s.IsDeleted == false).Select(x => x.SettingId).FirstOrDefault();

                            if (PredecessorIdV == 0 || SuccessorIdV == 0)
                            {

                            }
                            else
                            {
                                var dependncy = new DependencySettingsNew();
                                dependncy.PredecessorId = PredecessorIdV;
                                dependncy.SuccessorId = SuccessorIdV;
                                dependncy.Type = 0;
                                dependncy.ProjSubTypeId = ProjectSubTypeId;
                                dependncy.IsDeleted = false;
                                dependncy.AddDate = DateTime.Now;
                                dependncy.AddUser = UserId;
                                dependncy.BranchId = BranchId;
                                _TaamerProContext.DependencySettingsNew.Add(dependncy);
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


        public GeneralMessage DeleteDependencySettings(int DependencyId,int UserId,int BranchId)
        {
            try
            {
                // DependencySettings DependencySettings = _DependencySettingsRepository.GetById(DependencyId);
                DependencySettings? DependencySettings = _TaamerProContext.DependencySettings.Where(s => s.DependencyId == DependencyId).FirstOrDefault();
                if (DependencySettings != null)
                {
                    _TaamerProContext.DependencySettings.Remove(DependencySettings);

                    //dependency.IsDeleted = true;
                    //dependency.DeleteDate = DateTime.Now;
                    //dependency.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف  سير المهام رقم " +_TaamerProContext.ProjectSubTypes.FirstOrDefault(x => x.ProjectTypeId == DependencySettings.ProjSubTypeId).NameAr; ;
                    _SystemAction.SaveAction("DeleteDependencySettings", "DependencySettingsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }


                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف  سير المهام رقم " + DependencyId; ;
                 _SystemAction.SaveAction("DeleteDependencySettings", "DependencySettingsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public TasksNodeVM GetTasksNodeByProSubTypeId(int ProjSubTypeId, int BranchId, int UserId)
        {
            var NodeTasks = new TasksNodeVM();
            NodeTasks.nodeDataArray = _SettingsRepository.GetAllSettingsByProjectID(ProjSubTypeId).Result;
            NodeTasks.linkDataArray = _DependencySettingsRepository.GetAllDependencyByProjSubTypeId(ProjSubTypeId, BranchId).Result;
            // var succesorIds = _DependencySettingsRepository.GetMatching(s => s.IsDeleted == false &&  s.ProjSubTypeId == ProjSubTypeId).Select(s => s.SuccessorId);
            var succesorIds = _TaamerProContext.DependencySettings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeId).Select(s => s.SuccessorId);


            //var predecessorIds = _DependencySettingsRepository.GetMatching(s => s.IsDeleted == false &&  s.ProjSubTypeId == ProjSubTypeId).Select(s => s.PredecessorId);
            var predecessorIds = _TaamerProContext.DependencySettings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeId).Select(s => s.PredecessorId);

            NodeTasks.firstLevelNode = predecessorIds.Except(succesorIds).Select(s => new SettingsVM
            {
                SettingId = s.Value,
            });
            return NodeTasks;
        }
        public TasksNodeNewVM GetTasksNodeByProSubTypeIdNew(int ProjSubTypeId, int BranchId, int UserId)
        {
            var NodeTasks = new TasksNodeNewVM();
            NodeTasks.nodeDataArray = _SettingsRepository.GetAllSettingsByProjectIDNew(ProjSubTypeId).Result;
            NodeTasks.linkDataArray = _DependencySettingsRepository.GetAllDependencyByProjSubTypeIdNew(ProjSubTypeId, BranchId).Result;
            return NodeTasks;
        }

        public GeneralMessage TransferSettingNEW(int ProjSubTypeFromId, int ProjSubTypeToId, int BranchId, int UserId)
        {
            try
            {
                //remove
                // var copiesRemove = _SettingsRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeToId /*&& s.BranchId == BranchId*/ && s.IsMerig == -1); //phases and tasks
                var copiesRemove = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeToId /*&& s.BranchId == BranchId*/ && s.IsMerig == -1).ToList(); //phases and tasks

                //var copiesSettingsRemove = _DependencySettingsRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeToId /*&& s.BranchId == BranchId*/);
                var copiesSettingsRemove = _TaamerProContext.DependencySettings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeToId /*&& s.BranchId == BranchId*/).ToList();

                _TaamerProContext.Settings.RemoveRange(copiesRemove);
                _TaamerProContext.DependencySettings.RemoveRange(copiesSettingsRemove);



                // var copies = _SettingsRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeFromId /*&& s.BranchId == BranchId*/ && s.IsMerig == -1); //phases and tasks
                var copies = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeFromId /*&& s.BranchId == BranchId*/ && s.IsMerig == -1).ToList(); //phases and tasks


                foreach (var item in copies)
                {
                    item.ProjSubTypeId = ProjSubTypeToId;
                    item.CopySettingId = item.SettingId;
                    item.BranchId = BranchId;
                    item.AddDate = DateTime.Now;
                    item.AddUser = UserId;
                    item.SettingId = 0;
                }
                _TaamerProContext.Settings.AddRange(copies);
                _TaamerProContext.SaveChanges();
                foreach (var item in copies) // new copies ==> update parentId
                {
                    if (item.ParentId == null)
                    {
                        item.ParentId = null;
                    }
                    else
                    {

                        var tempParentId = 0;

                        var TempCopParentId = copies.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeToId && s.CopySettingId == item.ParentId).ToList();
                        if (TempCopParentId.Count() > 0)
                        {
                            tempParentId = TempCopParentId.FirstOrDefault().SettingId;
                            item.ParentId = tempParentId;

                        }
                        else
                        {
                            item.ParentId = null;
                        }
                        //item.ParentId = copies.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeToId && s.CopySettingId == item.ParentId).FirstOrDefault().SettingId;

                        //item.ParentId = tempParentId;
                    }
                }
                ///// dependency setting 
                //  var copiesSettings = _DependencySettingsRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeFromId /*&& s.BranchId == BranchId*/);
                var copiesSettings = _TaamerProContext.DependencySettings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeFromId /*&& s.BranchId == BranchId*/).ToList();

                foreach (var item in copiesSettings)
                {
                    var tempPre = 0;
                    var tempSucc = 0;

                    var TempCopPre = copies.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeToId && s.CopySettingId == item.PredecessorId).ToList();
                    if(TempCopPre.Count() > 0)
                    {
                        tempPre = TempCopPre.FirstOrDefault().SettingId;
                    }
                    else
                    {
                        tempPre = -1;
                    }
                    var TempCopSucc = copies.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeToId && s.CopySettingId == item.SuccessorId).ToList();
                    if (TempCopSucc.Count() > 0)
                    {
                        tempSucc = TempCopSucc.FirstOrDefault().SettingId;

                    }
                    else
                    {
                        tempSucc = -1;
                    }
                    //item.PredecessorId = copies.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeToId/* && s.BranchId == BranchId*/ && s.CopySettingId == item.PredecessorId).FirstOrDefault().SettingId;
                    //item.SuccessorId = copies.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeToId /*&& s.BranchId == BranchId*/ && s.CopySettingId == item.SuccessorId).FirstOrDefault().SettingId;
                    item.DependencyId = 0;
                    item.PredecessorId = tempPre;
                    item.SuccessorId = tempSucc;
                    item.ProjSubTypeId = ProjSubTypeToId;
                    item.BranchId = BranchId;
                    item.IsDeleted = false;
                    item.Type = 0;
                    item.AddDate = DateTime.Now;
                    item.AddUser = UserId;

                }
                _TaamerProContext.DependencySettings.AddRange(copiesSettings);



                //var resultNum = _ProSettingDetailsRepository.GenerateNextProSettingNumber();
                //var proSettingDetails = new ProSettingDetails();

                //var proSettingDetailsUpdated = _ProSettingDetailsRepository.GetMatching(s=>s.IsDeleted==false && s.ProjectSubtypeId == ProjSubTypeToId);


                //proSettingDetails.AddDate = DateTime.Now;
                //proSettingDetails.AddUser = UserId;
                //proSettingDetails.IsDeleted = false;
                //proSettingDetails.ProSettingNo = resultNum.ToString();
                //proSettingDetails.ProSettingNote = "سير تم نسخه مسبقا";
                //proSettingDetails.ProjectTypeId = proSettingDetailsUpdated.FirstOrDefault().ProjectTypeId;
                //proSettingDetails.ProjectSubtypeId = proSettingDetailsUpdated.FirstOrDefault().ProjectSubtypeId;
                //_ProSettingDetailsRepository.Add(proSettingDetails);



                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "نقل سير المهام";
                 _SystemAction.SaveAction("TransferSetting", "DependencySettingsService", 1, Resources.flow_moved_successfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.flow_moved_successfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.flow_moved_faild;
                 _SystemAction.SaveAction("TransferSetting", "DependencySettingsService", 1, Resources.flow_moved_faild, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.flow_moved_faild };
            }
        }
        public GeneralMessage TransferSettingNewGrantt(int ProjSubTypeFromId, int ProjSubTypeToId, int BranchId, int UserId)
        {
            try
            {
                //remove
                var copiesRemove = _TaamerProContext.SettingsNew.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeToId /*&& s.BranchId == BranchId*/ && s.IsMerig == -1).ToList(); //phases and tasks

                //var copiesSettingsRemove = _DependencySettingsRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeToId /*&& s.BranchId == BranchId*/);
                var copiesSettingsRemove = _TaamerProContext.DependencySettingsNew.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeToId /*&& s.BranchId == BranchId*/).ToList();

                _TaamerProContext.SettingsNew.RemoveRange(copiesRemove);
                _TaamerProContext.DependencySettingsNew.RemoveRange(copiesSettingsRemove);



                // var copies = _SettingsRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeFromId /*&& s.BranchId == BranchId*/ && s.IsMerig == -1); //phases and tasks
                var copies = _TaamerProContext.SettingsNew.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeFromId /*&& s.BranchId == BranchId*/ && s.IsMerig == -1).ToList(); //phases and tasks


                foreach (var item in copies)
                {
                    item.ProjSubTypeId = ProjSubTypeToId;
                    item.CopySettingId = item.SettingId;
                    item.BranchId = BranchId;
                    item.AddDate = DateTime.Now;
                    item.AddUser = UserId;
                    item.SettingId = 0;
                }
                _TaamerProContext.SettingsNew.AddRange(copies);
                _TaamerProContext.SaveChanges();
                foreach (var item in copies) // new copies ==> update parentId
                {
                    if (item.ParentId == null)
                    {
                        item.ParentId = null;
                    }
                    else
                    {

                        var tempParentId = 0;

                        var TempCopParentId = copies.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeToId && s.CopySettingId == item.ParentId).ToList();
                        if (TempCopParentId.Count() > 0)
                        {
                            tempParentId = TempCopParentId.FirstOrDefault().SettingId;
                            item.ParentId = tempParentId;

                        }
                        else
                        {
                            item.ParentId = null;
                        }
                    }
                }
                var copiesSettings = _TaamerProContext.DependencySettingsNew.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeFromId /*&& s.BranchId == BranchId*/).ToList();

                foreach (var item in copiesSettings)
                {
                    var tempPre = 0;
                    var tempSucc = 0;

                    var TempCopPre = copies.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeToId && s.CopySettingId == item.PredecessorId).ToList();
                    if (TempCopPre.Count() > 0)
                    {
                        tempPre = TempCopPre.FirstOrDefault().SettingId;
                    }
                    else
                    {
                        tempPre = -1;
                    }
                    var TempCopSucc = copies.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeToId && s.CopySettingId == item.SuccessorId).ToList();
                    if (TempCopSucc.Count() > 0)
                    {
                        tempSucc = TempCopSucc.FirstOrDefault().SettingId;

                    }
                    else
                    {
                        tempSucc = -1;
                    }
                    item.DependencyId = 0;
                    item.PredecessorId = tempPre;
                    item.SuccessorId = tempSucc;
                    item.ProjSubTypeId = ProjSubTypeToId;
                    item.BranchId = BranchId;
                    item.IsDeleted = false;
                    item.Type = 0;
                    item.AddDate = DateTime.Now;
                    item.AddUser = UserId;

                }
                _TaamerProContext.DependencySettingsNew.AddRange(copiesSettings);

                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "نقل سير المهام";
                _SystemAction.SaveAction("TransferSetting", "DependencySettingsService", 1, Resources.flow_moved_successfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.flow_moved_successfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.flow_moved_faild;
                _SystemAction.SaveAction("TransferSetting", "DependencySettingsService", 1, Resources.flow_moved_faild, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.flow_moved_faild };
            }
        }

        public GeneralMessage TransferSetting(int ProjSubTypeFromId, int ProjSubTypeToId, int BranchId, int UserId)
        {
            try
            {
                //var copies = _SettingsRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeFromId /*&& s.BranchId == BranchId*/ && s.IsMerig == -1); //phases and tasks
                var copies = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeFromId /*&& s.BranchId == BranchId*/ && s.IsMerig == -1); //phases and tasks

                foreach (var item in copies)
                {
                    item.ProjSubTypeId = ProjSubTypeToId;
                    item.CopySettingId = item.SettingId;
                    item.BranchId = BranchId;
                    item.AddDate = DateTime.Now;
                    item.AddUser = UserId;
                }
                _TaamerProContext.Settings.AddRange(copies);
                _TaamerProContext.SaveChanges();
                foreach (var item in copies) // new copies ==> update parentId
                {
                   if (item.ParentId == null)
                      {
                            item.ParentId = null;
                      }
                      else
                      {
                           item.ParentId = copies.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeToId  && s.CopySettingId == item.ParentId).FirstOrDefault().SettingId;
                      }
                }
                ///// dependency setting 
                //  var copiesSettings = _DependencySettingsRepository.GetMatching(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeFromId /*&& s.BranchId == BranchId*/);
                var copiesSettings = _TaamerProContext.DependencySettings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeFromId /*&& s.BranchId == BranchId*/);


                foreach (var item in copiesSettings)
                {
                    item.PredecessorId = copies.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeToId/* && s.BranchId == BranchId*/ && s.CopySettingId == item.PredecessorId).FirstOrDefault().SettingId;
                    item.SuccessorId = copies.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProjSubTypeToId /*&& s.BranchId == BranchId*/ && s.CopySettingId == item.SuccessorId).FirstOrDefault().SettingId;
                    item.ProjSubTypeId = ProjSubTypeToId;
                    item.BranchId = BranchId;
                    item.AddDate = DateTime.Now;
                    item.AddUser = UserId;
                }
                _TaamerProContext.DependencySettings.AddRange(copiesSettings);
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "نقل سير المهام";
                 _SystemAction.SaveAction("TransferSetting", "DependencySettingsService", 1, Resources.flow_moved_successfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.flow_moved_successfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.flow_moved_faild;
                 _SystemAction.SaveAction("TransferSetting", "DependencySettingsService", 1, Resources.flow_moved_faild, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.flow_moved_faild };
            }
        }
      

    }
}

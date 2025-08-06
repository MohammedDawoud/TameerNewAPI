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
using System.Data.SqlClient;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Google.Apis.Drive.v3.Data;
using System.Net.Mail;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class SettingsService :  ISettingsService
    {
        private readonly ISettingsRepository _SettingsRepository;
        private readonly IUsersRepository _UsersRepository;
        private readonly IVacationRepository _VacationRepository;
        private readonly ITasksDependencyRepository _TasksDependencyRepository;
        private readonly IDependencySettingsRepository _DependencySettingsRepository;
         private readonly IBranchesRepository _branchesRepository;
        private readonly INotificationRepository _NotificationRepository;
        private readonly IUserNotificationPrivilegesService _userNotificationPrivilegesService;
        private readonly IEmailSettingRepository _EmailSettingRepository;
        private readonly INotificationService _notificationService;

        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public SettingsService(ISettingsRepository SettingsRepository, IUsersRepository UsersRepository,
            IVacationRepository VacationRepository, ITasksDependencyRepository TasksDependencyRepository,
            IDependencySettingsRepository DependencySettingsRepository, IBranchesRepository branchesRepository,
            INotificationRepository NotificationRepository, IUserNotificationPrivilegesService userNotificationPrivilegesService,
           IEmailSettingRepository EmailSettingRepository, INotificationService notificationService, 
           TaamerProjectContext dataContext, ISystemAction systemAction
            )
        {
            _SettingsRepository = SettingsRepository;
            _UsersRepository = UsersRepository;
            _VacationRepository = VacationRepository;
            _TasksDependencyRepository = TasksDependencyRepository;
            _DependencySettingsRepository = DependencySettingsRepository;
             _branchesRepository = branchesRepository;
            _userNotificationPrivilegesService = userNotificationPrivilegesService;
            _EmailSettingRepository = EmailSettingRepository;
            _notificationService = notificationService;

            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task<IEnumerable<SettingsVM>> GetAllMainPhases(int? ProSubTypeId, int BranchId)
        {
            var Settings = _SettingsRepository.GetAllMainPhases(ProSubTypeId, BranchId);
            return Settings;
        }
        public Task<IEnumerable<SettingsVM>> GetAllSubPhases(int? ParentId, int BranchId)
        {
            var Settings = _SettingsRepository.GetAllSubPhases(ParentId, BranchId);
            return Settings;
        }
        public Task<IEnumerable<SettingsVM>> GetAllSettingsByProjectID(int ProSubTypeId)
        {
            var Settings = _SettingsRepository.GetAllSettingsByProjectID(ProSubTypeId);
            return Settings;
        }
        public Task<IEnumerable<SettingsNewVM>> GetAllSettingsByProjectIDNew(int ProSubTypeId)
        {
            var Settings = _SettingsRepository.GetAllSettingsByProjectIDNew(ProSubTypeId);
            return Settings;
        }
        public Task<IEnumerable<SettingsVM>> GetAllSettingsByProjectIDAll()
        {
            var Settings = _SettingsRepository.GetAllSettingsByProjectIDAll();
            return Settings;
        }
        public Task<IEnumerable<SettingsNewVM>> GetAllSettingsByProjectIDAllNew()
        {
            var Settings = _SettingsRepository.GetAllSettingsByProjectIDAllNew();
            return Settings;
        }
        public Task<IEnumerable<SettingsVM>> GetAllTasks(int? ProSubTypeId, int BranchId)
        {
            var Settings = _SettingsRepository.GetAllTasks(ProSubTypeId, BranchId);
            return Settings;
        }
        public GeneralMessage SaveSettings(Settings settings, int UserId, int BranchId)
        {
            try
            {
                //var BranchIdOfUser = _UsersRepository.GetById(settings.UserId);
                Users? BranchIdOfUser = _TaamerProContext.Users.Where(s => s.UserId == settings.UserId).FirstOrDefault();

                //switch (settings.TimeType)
                //{
                //    //case 1:
                //    //    settings.TimeMinutes = settings.TimeMinutes;
                //    //    break;
                //    //case 2:
                //    //    settings.TimeMinutes = settings.TimeMinutes * 24;
                //    //    break;
                //    //case 3:
                //    //    settings.TimeMinutes = settings.TimeMinutes * 24 * 7;
                //    //    break;
                //    //case 4:
                //    //    settings.TimeMinutes = settings.TimeMinutes * 24 * 7 * 30;
                //    //    break;
                //}
                if (settings.Type == 3)
                {
                    // var UserVacation = _VacationRepository.GetMatching(s => s.IsDeleted == false && s.EmployeeId == settings.UserId && s.VacationStatus != 4).Count();
                    var UserVacation = _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && s.EmployeeId == settings.UserId && s.VacationStatus != 4).Count();
                    
                    
                    if (UserVacation != 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "فشل في حفظ خيارات الخدمة" + settings.DescriptionAr;
                        _SystemAction.SaveAction("SaveSettings", "SettingsService", 1, "Resources.UserVac", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = " Resources.UserVac " };
                        }
                }
                if (settings.SettingId == 0)
                {
                    settings.AddUser = UserId;
                    settings.IsMerig = -1;
                    if (settings.Type == 3)
                    {
                        settings.BranchId = BranchIdOfUser.BranchId;
                        settings.EndTime = DateTime.Now.ToString("h:mm");
                    }
                    else
                    {
                        settings.BranchId = BranchId;
                    }
                    settings.AddDate = DateTime.Now;
                    _TaamerProContext.Settings.Add(settings);

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة خيارات خدمة جديدة" + settings.DescriptionAr;
                    _SystemAction.SaveAction("SaveSettings", "SettingsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_SavedSuccessfully  };
                    }
                else
                {
                    //var SettingsUpdated = _SettingsRepository.GetById(settings.SettingId);
                    Settings? SettingsUpdated = _TaamerProContext.Settings.Where(s => s.SettingId == settings.SettingId).FirstOrDefault();

                    if (SettingsUpdated != null)
                    {
                        SettingsUpdated.DescriptionAr = settings.DescriptionAr;
                        SettingsUpdated.DescriptionEn = settings.DescriptionEn;
                        SettingsUpdated.UserId = settings.UserId;
                        SettingsUpdated.TimeMinutes = settings.TimeMinutes;
                        SettingsUpdated.Cost = settings.Cost;
                        SettingsUpdated.TimeType = settings.TimeType;
                        SettingsUpdated.IsUrgent = settings.IsUrgent;
                        SettingsUpdated.TaskType = settings.TaskType;
                        SettingsUpdated.Notes = settings.Notes;
                        SettingsUpdated.UpdateUser = UserId;
                        SettingsUpdated.Priority = settings.Priority;
                        // SettingsUpdated.EndTime = DateTime.Now.ToString("h:mm");
                        SettingsUpdated.StartDate = settings.StartDate;
                        SettingsUpdated.EndDate = settings.EndDate;
                        if (SettingsUpdated.StartDate != settings.StartDate)
                        {
                            SettingsUpdated.EndTime = DateTime.Now.ToString("h:mm");
                        }
                        if (SettingsUpdated.Type == 3)
                        {
                            SettingsUpdated.BranchId = BranchIdOfUser.BranchId;
                        }
                        else
                        {
                            SettingsUpdated.BranchId = BranchId;
                        }
                        SettingsUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل خيارات الخدمة رقم " + settings.DescriptionAr;
                    _SystemAction.SaveAction("SaveSettings", "SettingsService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                    }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ خيارات خدمة" + settings.DescriptionAr;
                _SystemAction.SaveAction("SaveSettings", "SettingsService", 1, "Resources.General_SavedFailed", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed  };
            }
        }
        public IEnumerable<object> GetSetting_Statment(string Con, string SelectStetment)
        {
            SqlConnection con = new SqlConnection(Con);
            SqlDataAdapter da = new SqlDataAdapter(SelectStetment, Con);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);

            DataTable myDataTable = ds.Tables[0];
            con.Close();

            return myDataTable.AsEnumerable().Select(row => new
            {

                SettingId = row[0].ToString(),
                TaskName = row[1].ToString(),
                SettingNo = row[2].ToString(),
                SettingNote = row[3].ToString(),
                UserId = row[4].ToString(),
            });
        }
        public GeneralMessage ConvertUserSettingsSome(int SettingId, int FromUserId, int ToUserId, int UserId, int BranchId)
        {
            try
            {
                //var projectUpdated = _SettingsRepository.GetMatching(s => s.IsDeleted == false && s.SettingId == SettingId);
                var projectUpdated = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.SettingId == SettingId);

                if (projectUpdated != null && projectUpdated.Count() > 0)
                {
                    foreach (var item in projectUpdated)
                    {
                        var userFrom = FromUserId;
                        item.UserId = ToUserId;
                        item.UpdateUser = UserId;
                        item.UpdateDate = DateTime.Now;
                        try
                        {
                            //var branch = _branchesRepository.GetById(BranchId);
                            Branch? branch = _TaamerProContext.Branch.Where(s => s.BranchId == BranchId).FirstOrDefault();

                            var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(ToUserId);
                            if (UserNotifPriv.Result.Count() != 0)
                            {
                                if (UserNotifPriv.Result.Contains(3222))
                                {
                                    var UserNotification = new Notification();
                                    UserNotification.ReceiveUserId = ToUserId;
                                    UserNotification.Name = "Resources.General_Newtasks";
                                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                    UserNotification.SendUserId = 1;
                                    UserNotification.Type = 1; // notification
                                    UserNotification.Description = "تم تحويل مهمة سير من  : " +  _UsersRepository.GetById(userFrom).FullName + " الي  " + _UsersRepository.GetById(ToUserId).FullName;
                                    UserNotification.AllUsers = false;
                                    UserNotification.SendDate = DateTime.Now;
                                    UserNotification.ProjectId = 0;
                                    UserNotification.TaskId = 0;
                                    UserNotification.AddUser = UserId;
                                    UserNotification.IsHidden = false;
                                    UserNotification.NextTime = null;
                                    UserNotification.AddDate = DateTime.Now;
                                    _TaamerProContext.Notification.Add(UserNotification);
                                    _notificationService.sendmobilenotification(ToUserId, " Resources.General_Newtasks", "تم تحويل مهمة سير من  : " + _UsersRepository.GetById(userFrom).FullName + " الي  " + _UsersRepository.GetById(ToUserId).FullName);
                                }

                                if (UserNotifPriv.Result.Contains(3223))
                                {
                                   // var userObj = _UsersRepository.GetById(ToUserId);
                                    Users? userObj = _TaamerProContext.Users.Where(s => s.UserId == ToUserId).FirstOrDefault();

                                    var NotStr = _UsersRepository.GetById(ToUserId).FullName + " الي  " + _UsersRepository.GetById(userFrom).FullName + " تم تحويل مهمة سير من  ";
                                    if (userObj.Mobile != null && userObj.Mobile != "")
                                    {
                                        var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                    }
                                }

                                if (UserNotifPriv.Result.Contains(3221))
                                {
                                    var Desc = _UsersRepository.GetById(ToUserId).FullName + " الي  " + _UsersRepository.GetById(userFrom).FullName + " تم تحويل مهمة سير من  ";

                                    SendMailNoti(0, Desc, "تحويل مهمة سير", BranchId, UserId, ToUserId);
                                }
                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تحويل مهمة سير" + projectUpdated.Select(x=>x.DescriptionAr);
                _SystemAction.SaveAction("ConvertUserSettingsSome", "SettingsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تحويل مهمة سير";
                _SystemAction.SaveAction("ConvertUserSettingsSome", "SettingsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage ConvertMoreUserSettings(List<int> SettingIds, int FromUserId, int ToUserId, int UserId, int BranchId)
        {
            try
            {
                //var projectUpdated = _SettingsRepository.GetMatching(s => s.IsDeleted == false && s.SettingId == SettingId);
                foreach (int SettingId in SettingIds)
                {
                    var projectUpdated = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.SettingId == SettingId);

                    if (projectUpdated != null && projectUpdated.Count() > 0)
                    {
                        foreach (var item in projectUpdated)
                        {
                            var userFrom = item.UserId??0;
                            item.UserId = ToUserId;
                            item.UpdateUser = UserId;
                            item.UpdateDate = DateTime.Now;
                            try
                            {
                                //var branch = _branchesRepository.GetById(BranchId);
                                Branch? branch = _TaamerProContext.Branch.Where(s => s.BranchId == BranchId).FirstOrDefault();

                                var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(ToUserId);
                                if (UserNotifPriv.Result.Count() != 0)
                                {
                                    if (UserNotifPriv.Result.Contains(3222))
                                    {
                                        var UserNotification = new Notification();
                                        UserNotification.ReceiveUserId = ToUserId;
                                        UserNotification.Name = "Resources.General_Newtasks";
                                        UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                        UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                        UserNotification.SendUserId = 1;
                                        UserNotification.Type = 1; // notification
                                        UserNotification.Description = "تم تحويل مهمة سير من  : " + _UsersRepository.GetById(userFrom).FullName + " الي  " + _UsersRepository.GetById(ToUserId).FullName;
                                        UserNotification.AllUsers = false;
                                        UserNotification.SendDate = DateTime.Now;
                                        UserNotification.ProjectId = 0;
                                        UserNotification.TaskId = 0;
                                        UserNotification.AddUser = UserId;
                                        UserNotification.IsHidden = false;
                                        UserNotification.NextTime = null;
                                        UserNotification.AddDate = DateTime.Now;
                                        _TaamerProContext.Notification.Add(UserNotification);
                                        _notificationService.sendmobilenotification(ToUserId, Resources.General_Newtasks, "تم تحويل مهمة سير من  : " + _UsersRepository.GetById(userFrom).FullName + " الي  " + _UsersRepository.GetById(ToUserId).FullName);
                                    }

                                    if (UserNotifPriv.Result.Contains(3223))
                                    {
                                        // var userObj = _UsersRepository.GetById(ToUserId);
                                        Users? userObj = _TaamerProContext.Users.Where(s => s.UserId == ToUserId).FirstOrDefault();

                                        var NotStr = _UsersRepository.GetById(ToUserId).FullName + " الي  " + _UsersRepository.GetById(userFrom).FullName + " تم تحويل مهمة سير من  ";
                                        if (userObj.Mobile != null && userObj.Mobile != "")
                                        {
                                            var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                        }
                                    }

                                    if (UserNotifPriv.Result.Contains(3221))
                                    {
                                        var Desc = _UsersRepository.GetById(ToUserId).FullName + " الي  " + _UsersRepository.GetById(userFrom).FullName + " تم تحويل مهمة سير من  ";

                                        SendMailNoti(0, Desc, "تحويل مهمة سير", BranchId, UserId, ToUserId);
                                    }
                                }

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    _TaamerProContext.SaveChanges();
                }

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تحويل مهمة سير";
                _SystemAction.SaveAction("ConvertUserSettingsSome", "SettingsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تحويل مهمة سير";
                _SystemAction.SaveAction("ConvertUserSettingsSome", "SettingsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage SaveSettingsList(List<Settings> settings, int UserId, int BranchId)
        {
            try
            {
                bool IsInsert = false;
                foreach (Settings setting in settings)
                {
                    //var BranchIdOfUser = _UsersRepository.GetById(setting.UserId);
                    Users? BranchIdOfUser = _TaamerProContext.Users.Where(s => s.UserId == setting.UserId).FirstOrDefault();
                    
                    //if (setting.Type == 3)
                    //{
                    //    //var UserVacation = _VacationRepository.GetMatching(s => s.IsDeleted == false && s.EmployeeId == setting.UserId && s.VacationStatus != 4).Count();
                    //    if (UserVacation != 0)
                    //    {
                    //        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.UserVac };
                    //    }
                    //}
                    if (setting.SettingId == 0)
                    {
                        setting.AddUser = UserId;
                        setting.IsMerig = -1;
                        if (setting.Type == 3)
                        {
                            setting.BranchId = BranchIdOfUser.BranchId;
                            setting.EndTime = DateTime.Now.ToString("h:mm");
                        }
                        else
                        {
                            setting.BranchId = BranchId;
                        }
                        setting.AddDate = DateTime.Now;
                        _TaamerProContext.Settings.Add(setting);
                        IsInsert = true;
                    }
                    else
                    {
                        //var SettingsUpdated = _SettingsRepository.GetById(setting.SettingId);
                        Settings? SettingsUpdated = _TaamerProContext.Settings.Where(s => s.SettingId == setting.SettingId).FirstOrDefault();

                        if (SettingsUpdated != null)
                        {
                            SettingsUpdated.DescriptionAr = setting.DescriptionAr;
                            SettingsUpdated.DescriptionEn = setting.DescriptionEn;
                            SettingsUpdated.UserId = setting.UserId;
                            SettingsUpdated.TimeMinutes = setting.TimeMinutes;
                            SettingsUpdated.Cost = setting.Cost;
                            SettingsUpdated.TimeType = setting.TimeType;
                            SettingsUpdated.IsUrgent = setting.IsUrgent;
                            SettingsUpdated.TaskType = setting.TaskType;
                            SettingsUpdated.Notes = setting.Notes;
                            SettingsUpdated.UpdateUser = UserId;
                            SettingsUpdated.Priority = setting.Priority;
                            SettingsUpdated.RequirmentId = setting.RequirmentId;
                            // SettingsUpdated.EndTime = DateTime.Now.ToString("h:mm");
                            SettingsUpdated.StartDate = setting.StartDate;
                            SettingsUpdated.EndDate = setting.EndDate;
                            if (SettingsUpdated.StartDate != setting.StartDate)
                            {
                                SettingsUpdated.EndTime = DateTime.Now.ToString("h:mm");
                            }
                            if (SettingsUpdated.Type == 3)
                            {
                                SettingsUpdated.BranchId = BranchIdOfUser.BranchId;
                            }
                            else
                            {
                                SettingsUpdated.BranchId = BranchId;
                            }
                            SettingsUpdated.UpdateDate = DateTime.Now;
                        }
                    }

                    _TaamerProContext.SaveChanges();
                }
                if (IsInsert)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة مجموعة خيارات خدمة جديدة" + settings.Select(x=>x.DescriptionAr);
                    _SystemAction.SaveAction("SaveSettingsList", "SettingsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }
                else
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل مجموعة خيارات خدمة " + settings.Select(x => x.DescriptionAr);
                    _SystemAction.SaveAction("SaveSettingsList", "SettingsService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ مجموعة خيارات خدمة";
                _SystemAction.SaveAction("SaveSettingsList", "SettingsService", 1, "Resources.General_SavedFailed", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =Resources.General_SavedFailed };
                }
        
        }
        public int SaveSettings2(Settings settings, int UserId, int BranchId)
        {
            try
            {
                // var BranchIdOfUser = _UsersRepository.GetById(settings.UserId);
                Users? BranchIdOfUser = _TaamerProContext.Users.Where(s => s.UserId == settings.UserId).FirstOrDefault();

                //switch (settings.TimeType)
                //{
                //    //case 1:
                //    //    settings.TimeMinutes = settings.TimeMinutes;
                //    //    break;
                //    //case 2:
                //    //    settings.TimeMinutes = settings.TimeMinutes * 24;
                //    //    break;
                //    //case 3:
                //    //    settings.TimeMinutes = settings.TimeMinutes * 24 * 7;
                //    //    break;
                //    //case 4:
                //    //    settings.TimeMinutes = settings.TimeMinutes * 24 * 7 * 30;
                //    //    break;
                //}
                if (settings.SettingId == 0)
                {
                    settings.AddUser = UserId;
                    settings.IsMerig = -1;
                    if (settings.Type == 3)
                    {
                        settings.BranchId = BranchIdOfUser.BranchId;
                    }
                    else
                    {
                        settings.BranchId = BranchId;
                    }
                    settings.AddDate = DateTime.Now;
                    _TaamerProContext.Settings.Add(settings);
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة خيارات خدمة جديدة";
                    _SystemAction.SaveAction("SaveSettings2", "SettingsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return settings.SettingId;
                }
                else
                {
                    //var SettingsUpdated = _SettingsRepository.GetById(settings.SettingId);
                    Settings? SettingsUpdated = _TaamerProContext.Settings.Where(s => s.SettingId == settings.SettingId).FirstOrDefault();

                    if (SettingsUpdated != null)
                    {
                        SettingsUpdated.DescriptionAr = settings.DescriptionAr;
                        SettingsUpdated.DescriptionEn = settings.DescriptionEn;
                        SettingsUpdated.UserId = settings.UserId;
                        SettingsUpdated.TimeMinutes = settings.TimeMinutes;
                        SettingsUpdated.Cost = settings.Cost;
                        SettingsUpdated.TimeType = settings.TimeType;
                        SettingsUpdated.IsUrgent = settings.IsUrgent;
                        SettingsUpdated.TaskType = settings.TaskType;
                        SettingsUpdated.Notes = settings.Notes;
                        SettingsUpdated.UpdateUser = UserId;
                        if (SettingsUpdated.Type == 3)
                        {
                            SettingsUpdated.BranchId = BranchIdOfUser.BranchId;
                        }
                        else
                        {
                            SettingsUpdated.BranchId = BranchId;
                        }
                        SettingsUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل خيارات خدمة برقم "  + settings.SettingId;
                    _SystemAction.SaveAction("SaveSettings2", "SettingsService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return settings.SettingId;
                }
                
            }
            catch (Exception ex)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ خيارات خدمة برقم " + settings.SettingId;
                _SystemAction.SaveAction("SaveSettings2", "SettingsService", 1,Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return 0;
            }
        }
        public GeneralMessage DeleteSettings(int SettingId, int UserId, int BranchId)
        {
            try
            {
                //var prodependency = _DependencySettingsRepository.GetMatching(s => s.IsDeleted == false && (s.PredecessorId == SettingId || s.SuccessorId == SettingId)).ToList();
                var prodependency = _TaamerProContext.DependencySettings.Where(s => s.IsDeleted == false && (s.PredecessorId == SettingId || s.SuccessorId == SettingId)).ToList();

                //  var proMainPhase = _SettingsRepository.GetMatching(s => s.IsDeleted == false && s.ParentId == SettingId).ToList();
                var proMainPhase = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.ParentId == SettingId).ToList();

                if (prodependency != null && prodependency.Count > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " فشل في حذف مرحلة رقم " + SettingId; ;
                    _SystemAction.SaveAction("DeleteSettings", "SettingsService", 3, Resources.DeleteTaskSetting_Failed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.DeleteTaskSetting_Failed };
                    }

                else if (proMainPhase != null && proMainPhase.Count > 0)
                {
                    
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " فشل في حذف مرحلة رقم " + SettingId; ;
                    _SystemAction.SaveAction("DeleteSettings", "SettingsService", 3, "توجد مهام او مراحل في هذة المرحلة الرجاء حذفهم اولا", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.TasksDeletethemFirst };
                }
                else
                {
                    //Settings settings = _SettingsRepository.GetById(SettingId);
                    Settings? settings = _TaamerProContext.Settings.Where(s => s.SettingId == SettingId).FirstOrDefault();
                    if (settings != null)
                    {
                        settings.IsDeleted = true;
                        settings.DeleteDate = DateTime.Now;
                        settings.DeleteUser = UserId;
                        _TaamerProContext.SaveChanges();

                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " حذف مرحلة  " + settings.DescriptionAr;
                        _SystemAction.SaveAction("DeleteSettings", "SettingsService", 3, "Resources.General_DeletedSuccessfully", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                    }


                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
                    }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف خيارات الخدمة ,خيار رقم " + SettingId; ;
                _SystemAction.SaveAction("DeleteSettings", "SettingsService", 3, " Resources.General_DeletedFailed", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  Resources.General_DeletedFailed };
                }
        }
        public GeneralMessage ConvertTasksSubToMain(int SettingId, int UserId, int BranchId)
        {
            try
            {
                //  var prodependency = _DependencySettingsRepository.GetMatching(s => s.IsDeleted == false && (s.PredecessorId == SettingId || s.SuccessorId == SettingId)).ToList();
                var prodependency = _TaamerProContext.DependencySettings.Where(s => s.IsDeleted == false && (s.PredecessorId == SettingId || s.SuccessorId == SettingId)).ToList();


                // var TasksPhase = _SettingsRepository.GetMatching(s => s.IsDeleted == false && s.ParentId == SettingId && s.Type==3).ToList();
                var TasksPhase = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.ParentId == SettingId && s.Type == 3).ToList();

                if (prodependency != null && prodependency.Count > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote3 = " فشل في حذف مرحلة رقم " + SettingId; ;
                    _SystemAction.SaveAction("ConvertTasksSubToMain", "SettingsService", 3, Resources.DeleteTaskSetting_Failed, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.DeleteTaskSetting_Failed };
                }
                
                if (TasksPhase != null && TasksPhase.Count > 0)
                {
                    

                }
                else
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " فشل في نقل مهام مرحلة فرعية الي مرحلة رئيسية رقم " + SettingId; ;
                    _SystemAction.SaveAction("ConvertTasksSubToMain", "SettingsService", 3, "لا توجد مهام  في هذة المرحلة لنقلها ", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.noTasksPointTransfer };
                }

                // Settings settings = _SettingsRepository.GetById(SettingId);
                Settings? settings = _TaamerProContext.Settings.Where(s => s.SettingId == SettingId).FirstOrDefault();

                foreach (var task in TasksPhase)
                {
                    task.ParentId = settings.ParentId;
                }

                settings.UpdateDate = DateTime.Now;
                settings.UpdateUser = UserId;
                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تم نقل جميع المهام من المرحلة الفرعية الي المرحلة الرئيسية رقم " + SettingId;
                _SystemAction.SaveAction("ConvertTasksSubToMain", "SettingsService", 3, "Resources.General_DeletedSuccessfully", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في نقل مهام مرحلة فرعية الي مرحلة رئيسية رقم " + SettingId; ;
                _SystemAction.SaveAction("ConvertTasksSubToMain", "SettingsService", 3, " فشل في نقل مهام مرحلة فرعية الي مرحلة رئيسية ", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed  };
            }
        }

        public System.Boolean checkHaveMainPhases(int? Param, int BranchId)
        {
            if (_SettingsRepository.GetAllMainPhases(Param, BranchId) != null)
            {
                return true;
            }
            return false;
        }
        public IEnumerable<object> FillMainPhases(int? Param, int BranchId)
        {
            var FillMainPhases = _SettingsRepository.GetAllMainPhases(Param, BranchId).Result.Select(s => new
            {
                Id = s.SettingId,
                Name = s.DescriptionAr
            });
            return FillMainPhases;
        }
        public IEnumerable<object> FillSubPhases(int? Param, int BranchId)
        {
            return _SettingsRepository.GetAllSubPhases(Param, BranchId).Result.Select(s => new
            {
                Id = s.SettingId,
                Name = s.DescriptionAr
            });
        }
        public  IEnumerable<SettingsVM>  GetAllTasksByPhaseId(int PhaseId)
        {
            var tasks =  _SettingsRepository.GetAllTasksByPhaseId(PhaseId).Result.ToList();
            if (tasks.Count > 0)
            {
                var x = tasks[0].UserId;
                for (int i = 0; i < tasks.Count(); i++)
                {
                    //  var UserVacation = _VacationRepository.GetMatching(s => s.IsDeleted == false && s.EmployeeId == tasks[i].UserId && s.VacationStatus != 4).Count();
                    var UserVacation = _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && s.EmployeeId == tasks[i].UserId && s.VacationStatus != 4).Count();

                    tasks[i].VacationCount = UserVacation;
                }
            }
            return tasks;
        }
        public GeneralMessage MerigTasks(int[] TasksIdArray, string Description, string Note, int UserId, int BranchId)
        {
            try
            {
                //var GTask = _SettingsRepository.GetById(TasksIdArray[0]);
                Settings? GTask = _TaamerProContext.Settings.Where(s => s.SettingId == TasksIdArray[0]).FirstOrDefault();
             

                var settings = new Settings();
                decimal? totalcost = 0;
                var AssginUserId = GTask.UserId;
                var StartDate = GTask.StartDate;
                var EndDate = GTask.EndDate;
                int? TimeDay = 0;
                var countday = 0;
                var CountHour = 0;
                int? TimeHour = 0;
                for (var i = 0; i < TasksIdArray.Length; i++)
                {
                    //var Task = _SettingsRepository.GetById(TasksIdArray[i]);
                    Settings? Task = _TaamerProContext.Settings.Where(s => s.SettingId == TasksIdArray[i]).FirstOrDefault();

                    var Dependencyl= _TaamerProContext.DependencySettings.Where(s => s.IsDeleted && s.PredecessorId == Task.SettingId || s.SuccessorId == Task.SettingId).ToList();
                    if(Dependencyl.Count()>0)
                    {
                        _TaamerProContext.DependencySettings.RemoveRange(Dependencyl);
                    }

                    if (AssginUserId != Task.UserId)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في دمج خيارات خدمة";
                        _SystemAction.SaveAction("MerigTasks", "SettingsService", 2, Resources.General_CantMerig, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_CantMerig };
                        }
                    if (StartDate != null)
                    {
                        if (DateTime.ParseExact(StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) > DateTime.ParseExact(Task.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))
                        {
                            StartDate = Task.StartDate;
                        }
                    }
                    if (EndDate != null)
                    {
                        if (DateTime.ParseExact(EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(Task.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))
                        {
                            EndDate = Task.EndDate;
                        }
                    }
                    var TaskCost = Task.Cost;
                    if (TaskCost == null)
                    {
                        TaskCost = 0;
                    }
                    totalcost = totalcost + TaskCost;
                    if(Task.TimeType == 1)
                    {
                        CountHour = CountHour + 1;
                        TimeHour = TimeHour + Task.TimeMinutes;
                    }
                    else if (Task.TimeType == 2)
                    {
                        countday = countday + 1;
                        TimeDay = TimeDay + Task.TimeMinutes;
                    }
                    //TotalTime = TotalTime + Task.TimeMinutes;
                }
                if(CountHour == TasksIdArray.Length)
                {
                    settings.TimeType = 1;
                    settings.TimeMinutes = TimeHour;
                }
                else
                {
                    var TotalHour = (Convert.ToInt16(TimeHour / 24)) + 1;
                    settings.TimeType = 2;
                    settings.TimeMinutes = TimeDay;
                }
                settings.AddUser = UserId;
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
                settings.StartDate = StartDate;
                settings.EndDate = EndDate;
                settings.TaskType = GTask.TaskType;
                settings.Priority = 5;
                settings.IsMerig = -1;
                // settings.TimeType = timeType;

                _TaamerProContext.Settings.Add(settings);
                _TaamerProContext.SaveChanges();
                for (var i = 0; i < TasksIdArray.Length; i++)
                {
                    //var UpdatedTask = _SettingsRepository.GetById(TasksIdArray[i]);
                    Settings? UpdatedTask = _TaamerProContext.Settings.Where(s => s.SettingId == TasksIdArray[i]).FirstOrDefault();
                    if(UpdatedTask!=null)
                    {
                        UpdatedTask.IsMerig = settings.SettingId;
                        UpdatedTask.UpdateDate = DateTime.Now;
                        UpdatedTask.UpdateUser = UserId;
                    }
                } 
                
                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " دمج خيارات خدمة " ;
                _SystemAction.SaveAction("MerigTasks", "SettingsService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };

                }
            catch (Exception x)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في دمج خيارات خدمة";
                _SystemAction.SaveAction("MerigTasks", "SettingsService", 2, "Resources.General_SavedFailed", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  Resources.General_SavedFailed  };
            }
        }

        private bool SendMailNoti(int ProjectID, string Desc, string Subject, int BranchId, int UserId, int ToUserID)
        {
            
            try
            {
                string Email = _TaamerProContext.Users.Where(s => s.UserId == ToUserID)?.FirstOrDefault()?.Email ?? "";
                if (Email != "")
                {
                    int OrganizationId;
                    Branch? branch = _TaamerProContext.Branch.Where(s => s.BranchId == BranchId).FirstOrDefault();
                    if (branch != null)
                    {
                        OrganizationId = branch.OrganizationId;
                    }
                    string textBody = Desc;
                    var mail = new MailMessage();
                    var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Password);

                    if (_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.DisplayName != null)
                    {
                        mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.DisplayName);
                    }
                    else
                    {
                        mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                    }
                   // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
                    mail.To.Add(new MailAddress(Email));
                    mail.Subject = Subject;
                    mail.Body = textBody;
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Host);
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = loginInfo;
                    //smtpClient.Port = 587;
                    smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Port);

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


        


    }
}

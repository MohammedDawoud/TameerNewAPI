using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using System.Security.Cryptography;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class UsersService : IUsersService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IUsersRepository _UsersRepository;
        private readonly IJobRepository _JobRepository;
        private readonly IEmployeesRepository _EmployeesRepository;
        private readonly IBranchesRepository _branchRepository;
        private readonly IOrganizationsRepository _organizationservice;
        private readonly IEmailSettingRepository _EmailSettingRepository;
        private readonly IUserBranchesRepository _userBranchesRepository;
        private readonly INotificationService _notificationService;

        public UsersService(TaamerProjectContext dataContext, ISystemAction systemAction, IUsersRepository usersRepository, IJobRepository jobRepository
            ,IEmployeesRepository employeesRepository, IBranchesRepository branchesRepository,IOrganizationsRepository organizationsRepository, IEmailSettingRepository emailSettingRepository
            , IUserBranchesRepository userBranchesRepository, INotificationService notificationService)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _UsersRepository = usersRepository;
            _JobRepository = jobRepository;
            _EmployeesRepository = employeesRepository;
            _branchRepository = branchesRepository;
            _organizationservice = organizationsRepository;
            _EmailSettingRepository = emailSettingRepository;
            _userBranchesRepository = userBranchesRepository;
            _notificationService = notificationService;
        }

        public async Task<int> GetMaxOrderNumber()
        {
            var workorders =await _JobRepository.GetMaxOrderNumber();
            return workorders;
        }


        public async Task<IEnumerable<UsersVM>> GetAllUsersOnline2()
        {
            var Users =await _UsersRepository.GetAllUsersOnline2();
            return Users;
        }


        public async Task<UsersVM> CheckISOnline(int userid)
        {
            var Users =await _UsersRepository.CheckISOnline(userid);
            return Users;
        }


        public async Task<IEnumerable<UsersVM>> GetAllUsers()
        {
            var Users =await _UsersRepository.GetAllUsers();
            return Users;
        }
        public IEnumerable<Users> GetAllUsersCount()
        {
            var Users = _UsersRepository.GetMatching(s => s.IsDeleted == false).ToList();
            return Users;
        }
        public async Task<IEnumerable<UsersVM>> GetAllUsersNotOpenUser(int UserId)
        {
            var Users =await _UsersRepository.GetAllUsersNotOpenUser(UserId);
            return Users;
        }
        //
        public int CheckEmail(int UserId, string email)
        {
            var Users = _UsersRepository.GetMatching(s => s.IsDeleted == false && s.Email == email).Count();
            return Users;
        }
        public async Task<IEnumerable<UsersVM>> GetUserAndBranchByNameSearch(Users users)
        {
            var Users =await _UsersRepository.GetUserAndBranchByNameSearch(users);
            return Users;
        }
        public async Task<IEnumerable<UsersVM>> GetAllOtherUsers(int UserId)
        {
            var Users =await _UsersRepository.GetAllOtherUsers(UserId);
            return Users;
        }
        //111
        public IEnumerable<UsersVM> GetSomeusers()
        {
            //int? item;
            List<UsersVM> Some = new List<UsersVM>();
            //var Users1 = _UsersRepository.GetAllOtherUsers(1);
            var Users1 = _UsersRepository.GetAllUsers().Result;
            var Employees = _EmployeesRepository.GetAllUsersEmployees().Result;
            var Exist = 0;

            foreach (var item in Users1)
            {
                Exist = 0;
                foreach (var item2 in Employees)
                {

                    if (item.UserId == item2.UserId)
                    {
                        Exist = 1;
                    }
                }
                if (Exist == 0)
                {
                    Some.Add(item);
                    Exist = 0;
                }
            }
            //Users1 = _UsersRepository.get(Employees);
            return Some;
        }
        //111
        public async Task<IEnumerable<UsersVM>> GetAllOnlineUsers(int UserId)
        {
            var Users = await _UsersRepository.GetAllOnlineUsers(UserId);
            return Users;
        }
        public int GetOnlineUsers()
        {
            var Users = _UsersRepository.GetMatching(s => s.IsDeleted == false && s.ISOnlineNew == true).Count();
            return Users;
        }
        public GeneralMessage SaveUsers(Users users, int UserId, string link, string logo, string url, string resetCode, string Con, int BranchId)
        {
            try
            {
                string DefaultValue = resetCode;
                var codeExist = _UsersRepository.GetMatching(s => s.IsDeleted == false && s.UserId != users.UserId && s.UserName == users.UserName).FirstOrDefault();
                if (codeExist != null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ مستخدم" +users.FullNameAr;
                   _SystemAction.SaveAction("SaveUsers", "UsersService", 1, @Resources.changeusername, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.changeusername };
                }

                //var OrganizationEmail = 
                var MatchedUserName = _UsersRepository.GetMatching(s => s.IsDeleted == false && s.UserName == users.UserName && s.UserId != users.UserId);
                var MatchedUserEmail = _UsersRepository.GetMatching(s => s.IsDeleted == false && s.Email == users.Email && s.UserId != users.UserId);
                if (MatchedUserName.Count() != 0)
                {

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ مستخدم"+users.FullNameAr;
                   _SystemAction.SaveAction("SaveUsers", "UsersService", 1, "Resources.changeusername", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.changeusername};
                }
                if (MatchedUserEmail.Count() != 0)
                {

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ مستخدم" +users.FullNameAr;
                   _SystemAction.SaveAction("SaveUsers", "UsersService", 1," Resources.changeemail", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.changeemail };
                }

                if (users.UserId == 0)
                {
                    //var UserUserName = _UsersRepository.SearchUsersOfUserName(users.UserName);
                    //var UserEmail = _UsersRepository.SearchUsersOfEmail(users.Email);
                    //if (UserUserName != 0)
                    //{
                    //    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.changeusername };
                    //}
                    //if (UserEmail != 0)
                    //{
                    //    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.changeemail };
                    //}
                    users.Password = EncryptValue(DefaultValue);// EncryptValue(users.Password);
                    users.IsOnline = false;
                    users.ISOnlineNew = false;
                    users.AddUser = UserId;
                    users.AddDate = DateTime.Now;
                    users.Session = 5;
                    users.ActiveTime = null;
                    users.IsActivated = false;
                    users.AppearWelcome = 1;





                    // users.Session = 15;
                    _TaamerProContext.Users.Add(users);
                    _TaamerProContext.SaveChanges();
                    // save group priviledge for user
                    if (users.GroupId != null)
                    {
                        var groupPriv = _TaamerProContext.GroupPrivileges.Where(s => s.IsDeleted == false && s.GroupId == users.GroupId);
                        if (groupPriv != null && groupPriv.Count() > 0)
                        {
                            //foreach (var item in groupPriv)
                            //{
                            //    var userPriv = new UserPrivileges();
                            //    userPriv.UserId = users.UserId;
                            //    userPriv.PrivilegeId = item.PrivilegeId;
                            //    users.AddUser = UserId;
                            //    users.AddDate = DateTime.Now;
                            //    _UserPrivilegesRepository.Add(userPriv);
                            //}
                            SqlConnection con = new SqlConnection(Con);
                            con.Open();

                            foreach (var item in groupPriv)
                            {
                                SqlCommand _cmd = new SqlCommand("exec InsertUserprivs " + users.UserId.ToString() + "," + item.PrivilegeId.ToString() + ",1,1,1,1" + "," + UserId.ToString());
                                _cmd.Connection = con;
                                _cmd.ExecuteNonQuery();
                                ///////////////////////////////////////////
                                //_TaamerProContext.SaveChanges();
                            }

                            con.Close();
                        }
                    }
                    //Notification Privilages
                    try
                    {
                        SqlConnection con = new SqlConnection(Con);
                        con.Open();
                        SqlCommand _cmd = new SqlCommand("exec InsertDefaultNotifPriv " + users.UserId.ToString());
                        _cmd.Connection = con;
                        _cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    {

                    }
                    // save user Branches 
                    try
                    {
                        foreach (var item in users.BranchesList)
                        {
                            var userBranch = new UserBranches();
                            userBranch.UserId = users.UserId;
                            userBranch.BranchId = item;
                            userBranch.AddUser = UserId;
                            userBranch.AddDate = DateTime.Now;
                            _TaamerProContext.UserBranches.Add(userBranch);
                        }
                    }
                    catch (Exception ex)
                    {

                    }


                    _TaamerProContext.SaveChanges();
                    //Update 
                    var Updatelink = UpdateLastLinkvalidDate(users.UserId, link);
                    try
                    {
                        SendEmailToConfirmLogin(users, resetCode, link, UserId, BranchId, logo, url);
                    }
                    catch { }
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة مستخدم جديد" +users.FullNameAr;
                   _SystemAction.SaveAction("SaveUsers", "UsersService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.senduseremail, ReturnedParm = users.UserId };
                }
                else
                {
                    var user = _UsersRepository.GetById(users.UserId);

                    var Activetime = user.ActiveTime;
                    users.ExpireDate = users.ExpireDate ?? "0";

                    if (users.AccStatusConfirm != "tadmin")
                    {
                        if ((users.Status == 0 || users.ExpireDate != "0"))
                        {
                            var tasksByUserId = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.UserId == users.UserId && s.Type == 3 && s.Status != 4).Count();
                            if (tasksByUserId != 0)
                            {

                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote3 = "فشل في تعديل بيانات مستخدم" + user.FullNameAr;
                                _SystemAction.SaveAction("SaveUsers", "UsersService", 2, Resources.userHave + Resources.userTasks, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                                //-----------------------------------------------------------------------------------------------------------------

                                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "Current job assignments must be transferred to another user", ReturnedParm = users.UserId };
                            }
                            var userProject = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.MangerId == users.UserId && s.Status != 1).Count();
                            if (userProject > 0)
                            {

                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote2 = "فشل في تعديل بيانات مستخدم" + user.FullNameAr;
                                _SystemAction.SaveAction("SaveUsers", "UsersService", 2, "Resources.userHave +  + userProject + مشاريع يجب الغاءه لإتمام العملية", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                //-----------------------------------------------------------------------------------------------------------------

                                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "مشاريع يجب الغاءه لإتمام العملية", ReturnedParm = users.UserId };
                            }

                            var SettingProjUser = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == users.UserId).ToList();
                            if (SettingProjUser != null && SettingProjUser.Count() > 0)
                            {
                                var Values = SettingProjUser.GroupBy(p => p.ProjSubTypeId).Select(g => g.First()).ToList();

                                foreach (var item in Values)
                                {
                                    var settings = _TaamerProContext.ProSettingDetails.Where(s => s.IsDeleted == false && s.ProjectSubtypeId == item.ProjSubTypeId).ToList();
                                    if (settings != null && settings.Count() > 0)
                                    {
                                        //-----------------------------------------------------------------------------------------------------------------
                                        string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                        string ActionNote4 = "فشل في تعديل بيانات مستخدم" + user.FullNameAr;
                                        _SystemAction.SaveAction("SaveUsers", "UsersService", 3, "المستخدم موجود علي  سير مشروع لا يمكن حذفه", "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                                        //-----------------------------------------------------------------------------------------------------------------
                                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "المستخدم موجود علي  سير مشروع لا يمكن تغيير حالته او إيقافه" };
                                    }

                                }

                            }

                        }

                    }
                    var UsersUpdated = _UsersRepository.GetById(users.UserId);
                    var checkstatus = 0;
                    var GroupIdCheck = 0;
                    if (UsersUpdated != null)
                    {

                        checkstatus = Convert.ToInt32(UsersUpdated.Status);
                        GroupIdCheck = UsersUpdated.GroupId ?? 0;
                        UsersUpdated.FullName = users.FullName;
                        UsersUpdated.JobId = users.JobId;
                        UsersUpdated.DepartmentId = users.DepartmentId;
                        UsersUpdated.Email = users.Email;
                        UsersUpdated.Mobile = users.Mobile;
                        UsersUpdated.EmpId = users.EmpId;
                        UsersUpdated.UserName = users.UserName;
                        UsersUpdated.Notes = users.Notes;
                        //UsersUpdated.Session = users.Session == null ? 2 : users.Session;
                        UsersUpdated.UpdateUser = UserId;
                        UsersUpdated.ExpireDate = users.ExpireDate??"0";
                        UsersUpdated.BranchId = users.BranchId;
                        //UsersUpdated.ImgUrl = users.ImgUrl;
                        UsersUpdated.ActiveTime = Activetime;
                        UsersUpdated.UpdateDate = DateTime.Now;
                        UsersUpdated.SupEngineerName = users.SupEngineerName;
                        UsersUpdated.SupEngineerCert = users.SupEngineerCert;
                        UsersUpdated.SupEngineerNationalId = users.SupEngineerNationalId;
                        //UsersUpdated.StampUrl = users.StampUrl;
                        UsersUpdated.GroupId = users.GroupId;
                        UsersUpdated.TimeId = users.TimeId;
                        UsersUpdated.FullNameAr = users.FullNameAr;


                        if (users.TimeId != null)
                        {
                            var Employee = _TaamerProContext.Employees.Where(s => s.IsDeleted == false && s.UserId == users.UserId);

                            if (Employee.Count() > 0)
                            {
                                Employee.FirstOrDefault().DawamId = users.TimeId;
                            }
                        }

                        var AccStatusConfirm_Decrypt = "";
                        if (UsersUpdated.AccStatusConfirm != null)
                        {
                            try
                            {
                                AccStatusConfirm_Decrypt = DecryptValue(UsersUpdated.AccStatusConfirm);
                            }
                            catch (Exception ex)
                            {
                                AccStatusConfirm_Decrypt = "";
                            }
                        }
                        else
                        {
                            AccStatusConfirm_Decrypt = "";
                        }

                        if (users.AccStatusConfirm != "tadmin")
                        {
                            if ((UsersUpdated.Status != users.Status) && (UsersUpdated.Status == 0) && (AccStatusConfirm_Decrypt == "tadmin"))
                            {
                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote2 = "فشل في حفظ تغيير حالة الحساب " +user.FullNameAr;
                               _SystemAction.SaveAction("SaveUsers", "UsersService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                //-----------------------------------------------------------------------------------------------------------------

                                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                            }
                        }
                        if(user.ExpireDate!="0")
                        {
                            UsersUpdated.Status = 0;
                        }
                        else
                        {
                            UsersUpdated.Status = users.Status;
                        }
                        



                        // update user brances 
                        var userbranches = _TaamerProContext.UserBranches.Where(s => s.IsDeleted == false && s.UserId == UsersUpdated.UserId);
                        _TaamerProContext.UserBranches.RemoveRange(userbranches);
                        if (users.BranchesList != null)
                        {
                            foreach (var item in users.BranchesList)
                            {
                                var userBranch = new UserBranches();
                                userBranch.UserId = users.UserId;
                                userBranch.BranchId = item;
                                userBranch.AddUser = UserId;
                                userBranch.AddDate = DateTime.Now;
                                _TaamerProContext.UserBranches.Add(userBranch);
                            }
                        }
                        if (users.Status != checkstatus)
                        {
                            var AccStatusConfirm = users.AccStatusConfirm;
                            if (users.Status == 0)
                            {

                                UsersUpdated.AccStatusConfirm = EncryptValue(AccStatusConfirm);
                                try
                                {

                                    SendMail2(BranchId, users.UserId, users.FullName, 1);
                                }
                                catch { }
                            }
                            else
                            {
                                UsersUpdated.AccStatusConfirm = EncryptValue(AccStatusConfirm);
                                try
                                {
                                    SendMail2(BranchId, users.UserId, users.FullName, 2);
                                }
                                catch { }

                            }
                        }
                        try
                        {
                            UsersUpdated.AccStatusConfirm = EncryptValue(users.AccStatusConfirm);
                        }
                        catch (Exception ex)
                        {
                        }

                        _TaamerProContext.SaveChanges();

                        if (users.GroupId != GroupIdCheck)
                        {
                            if (users.GroupId != null)//&& UsersUpdated.GroupId != users.GroupId)
                            {
                                var groupPriv = _TaamerProContext.GroupPrivileges.Where(s => s.IsDeleted == false && s.GroupId == users.GroupId);
                                if (groupPriv != null && groupPriv.Count() > 0)
                                {
                                    SqlConnection con = new SqlConnection(Con);
                                    con.Open();
                                    foreach (var item in groupPriv)
                                    {
                                        SqlCommand cmd = new SqlCommand("exec InsertUserprivs " + users.UserId.ToString() + "," + item.PrivilegeId.ToString() + ",1,1,1,1" + "," + UserId.ToString());
                                        cmd.Connection = con;
                                        cmd.ExecuteNonQuery();
                                    }
                                    con.Close();
                                }
                            }
                        }
                    }


                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل بيانات مستخدم  " + users.FullNameAr;
                    _SystemAction.SaveAction("SaveUsers", "UsersService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedParm = users.UserId };
                }


            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ بيانات مستخدم" +users.FullNameAr;
               _SystemAction.SaveAction("SaveUsers", "UsersService", 1, Resources.General_SavedFailed,"", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage SaveUsersProfile(Users users, int UserId, string link, string logo, string url, string resetCode, string Con, int BranchId)
        {
            try
            {
                string DefaultValue = resetCode;
                var codeExist = _UsersRepository.GetMatching(s => s.IsDeleted == false && s.UserId != users.UserId && s.UserName == users.UserName).FirstOrDefault();
                if (codeExist != null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote1 = "فشل في حفظ مستخدم" +users.FullNameAr;
                   _SystemAction.SaveAction("SaveUsers", "UsersService", 1, "Resources.changeusername", "", "", ActionDate1, UserId, BranchId, ActionNote1, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.changeusername };
                }

                //var OrganizationEmail = 
                var MatchedUserName = _UsersRepository.GetMatching(s => s.IsDeleted == false && s.UserName == users.UserName && s.UserId != users.UserId);
                var MatchedUserEmail = _UsersRepository.GetMatching(s => s.IsDeleted == false && s.Email == users.Email && s.UserId != users.UserId);
                if (MatchedUserName.Count() != 0)
                {

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "فشل في حفظ مستخدم" + users.FullNameAr;
                    _SystemAction.SaveAction("SaveUsers", "UsersService", 1, "Resources.changeusername", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.changeusername };
                }
                if (MatchedUserEmail.Count() != 0)
                {

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote3 = "فشل في حفظ مستخدم" + users.FullNameAr;
                    _SystemAction.SaveAction("SaveUsers", "UsersService", 1, "Resources.changeemail", "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.changeemail };
                }

                var user = _UsersRepository.GetById(users.UserId);

                var Activetime = user.ActiveTime;

                var UsersUpdated = _UsersRepository.GetById(users.UserId);
                var checkstatus = 0;
                var GroupIdCheck = 0;
                if (UsersUpdated != null)
                {

                    checkstatus = Convert.ToInt32(UsersUpdated.Status);
                    GroupIdCheck = UsersUpdated.GroupId ?? 0;
                    UsersUpdated.FullName = users.FullName;
                    UsersUpdated.FullNameAr = users.FullNameAr;
                    UsersUpdated.JobId = users.JobId;
                    //UsersUpdated.DepartmentId = users.DepartmentId;
                    UsersUpdated.Email = users.Email;
                    UsersUpdated.Mobile = users.Mobile;
                    //UsersUpdated.EmpId = users.EmpId;
                    UsersUpdated.UserName = users.UserName;
                    UsersUpdated.Notes = users.Notes;
                    UsersUpdated.Session = users.Session == null ? 2 : users.Session;
                    UsersUpdated.UpdateUser = UserId;
                    //UsersUpdated.Password = EncryptValue(users.Password);
                    //UsersUpdated.ExpireDate = users.ExpireDate;
                    //UsersUpdated.BranchId = users.BranchId;
                    UsersUpdated.ActiveTime = Activetime;
                    UsersUpdated.UpdateDate = DateTime.Now;
                    //UsersUpdated.SupEngineerName = users.SupEngineerName;
                    //UsersUpdated.SupEngineerCert = users.SupEngineerCert;
                    //UsersUpdated.SupEngineerNationalId = users.SupEngineerNationalId;
                    //UsersUpdated.GroupId = users.GroupId;
                    //UsersUpdated.TimeId = users.TimeId;
                    //UsersUpdated.FullNameAr = users.FullNameAr;
                    UsersUpdated.AppearWelcome = users.AppearWelcome;
                    UsersUpdated.AppearInInvoicePrint = users.AppearInInvoicePrint;

                    if (users.StampUrl != null)
                    {
                        UsersUpdated.StampUrl = users.StampUrl;
                    }
                    if (users.ImgUrl != null)
                    {
                        UsersUpdated.ImgUrl = users.ImgUrl;
                    }


                    _TaamerProContext.SaveChanges();
                }


                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تعديل بيانات مستخدم  " + users.FullNameAr;
               _SystemAction.SaveAction("SaveUsers", "UsersService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedParm = users.UserId };


            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ بيانات مستخدم" + users.FullNameAr;
                _SystemAction.SaveAction("SaveUsers", "UsersService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public bool SendEmailToConfirmLogin(Users users, string resetCode, string link, int UserId, int BranchId, string emailFor = "VerifyAccount", string Emailbody = "")
        {

            try
            {

                var mail = new MailMessage();

                var barnchData = _branchRepository.GetById((int)users.BranchId);
                var OrganizationData = _organizationservice.GetBranchOrganization(barnchData.OrganizationId).Result;
                // var OrganizationData = _organizationservice.GetBranchOrganization((int)users.BranchId);
                var SenderEmail = _EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.SenderEmail;
                var loginInfo = new NetworkCredential(SenderEmail, _EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.Password);

                if (_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.DisplayName != null)
                {
                    mail.From = new MailAddress(SenderEmail, _EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress(SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                }

                // mail.From = new MailAddress(SenderEmail);
                mail.To.Add(new MailAddress(users.Email));
                string subject = "مستخدم جديد ";
                mail.Subject = subject;

                Emailbody = Emailbody.Replace("{UserNameUrl}", users.UserName);
                Emailbody = Emailbody.Replace("{FullNameUrl}", users.FullNameAr ?? users.FullName);
                Emailbody = Emailbody.Replace("{PasswordUrl}", resetCode);
                Emailbody = Emailbody.Replace("{OrgNamearUrl}", OrganizationData.NameAr);
                Emailbody = Emailbody.Replace("{orgname}", OrganizationData.NameAr);

                //Emailbody = Emailbody.Replace("{OrgNameEnUrl}", OrganizationData.NameEn);

                mail.Body = Emailbody;
                LinkedResource logo = new LinkedResource(emailFor);
                logo.ContentId = "companylogo";
                // done HTML formatting in the next line to display my bayanatech logo
                AlternateView av1 = AlternateView.CreateAlternateViewFromString(Emailbody, null, MediaTypeNames.Text.Html);
                av1.LinkedResources.Add(logo);
                mail.AlternateViews.Add(av1);
                mail.IsBodyHtml = true;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.Port);

                smtpClient.Credentials = loginInfo;
                SmtpClient mailSender = new SmtpClient("localhost"); //use this if you are in the development server
                smtpClient.Send(mail);
                //Update 
                //var Updatelink = UpdateLastLinkvalidDate(users.UserId, link + "/" + resetCode);
                var Updatelink = UpdateLastLinkvalidDate(users.UserId, link);
                //// notify admin
                var AdminNotification = new Notification();
                var admin = _UsersRepository.GetMatching(s => s.IsDeleted == false && s.IsAdmin == true).FirstOrDefault();
                AdminNotification.ReceiveUserId = admin != null ? admin.UserId : 1;
                AdminNotification.Name = "إنشاء كلمة مرور لمستخدم جديد   ";
                AdminNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                AdminNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                AdminNotification.SendUserId = users.UserId;
                AdminNotification.Type = 1; // notification
                AdminNotification.Description = "المستخدم " + users.FullName + " طلب إنشاء مستخدم جديد علي النظام ";
                AdminNotification.AllUsers = false;
                AdminNotification.SendDate = DateTime.Now;
                AdminNotification.AddUser = users.UserId;
                AdminNotification.AddDate = DateTime.Now;
                AdminNotification.NextTime = null;
                _TaamerProContext.Notification.Add(AdminNotification);
                _notificationService.sendmobilenotification(admin != null ? admin.UserId : 1, "إنشاء كلمة مرور لمستخدم جديد   ", "المستخدم " + users.FullName + " طلب إنشاء مستخدم جديد علي النظام ");

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "إرسال بريد إلكتروني للمستخدم" + users.FullNameAr;
                _SystemAction.SaveAction("SendEmailToConfirmLogin", "UsersService", 1, "تم إرسال بريد إلكتروني لمستخدم لتأكيد الدخول", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return true;
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = "فشل في إرسال بريد إلكتروني للمستخدم" + users.FullNameAr;
                _SystemAction.SaveAction("SendEmailToConfirmLogin", "UsersService", 1, "فشل في إرسال بريد إلكتروني لمستخدم لتأكيد الدخول", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return false;
            }

        }
        public GeneralMessage DeleteUsers(int UserId, int BranchId)
        {
            try
            {
                var userTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.UserId == UserId && s.Type == 3 && s.BranchId == BranchId && s.Status != 4).Count();
                if (userTasks > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " فشل في حذف مستخدم  " + UserId; ;
                   _SystemAction.SaveAction("DeleteUsers", "UsersService", 3, "Resources.userHave + userTasks + Resources.userTasks", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.userHave + userTasks + Resources.userTasks };
                }
                var userProject = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.MangerId == UserId && s.BranchId == BranchId && s.Status != 1).Count();
                if (userProject > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " فشل في حذف مستخدم رقم " + UserId; ;
                    _SystemAction.SaveAction("DeleteUsers", "UsersService", 3, "Resources.userHave + userProject + Resources.UserProjects", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.userHave + userProject + Resources.UserProjects };
                }
                var userWorkOrder = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && (s.ExecutiveEng == UserId || s.ResponsibleEng == UserId) && (s.WOStatus == 1 || s.WOStatus == 2)).Count();
                if (userWorkOrder > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote3 = " فشل في حذف مستخدم رقم " + UserId; ;
                    _SystemAction.SaveAction("DeleteUsers", "UsersService", 3, Resources.userHave + userWorkOrder + Resources.userWorkOrder, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.userHave + userWorkOrder + Resources.userWorkOrder };
                }
                Users users = _UsersRepository.GetById(UserId);
                users.IsDeleted = true;
                users.DeleteDate = DateTime.Now;
                users.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote4 = " حذف مستخدم  " + users.FullNameAr;
                _SystemAction.SaveAction("DeleteUsers", "UsersService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate4, UserId, BranchId, ActionNote4, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف مستخدم رقم " + UserId; ;
                _SystemAction.SaveAction("DeleteUsers", "UsersService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }

        //Edit 2-6-2020
        public GeneralMessage DeleteUsers2(int Users, int UserId, int BranchId)
        {
            try
            {
                //var UserF = _UsersRepository.GetMatching(s => s.IsDeleted == false && s.IsAdmin == true && s.UserId == Users);
                //if (UserF != null && UserF.Count() > 0)
                //{
                //    //-----------------------------------------------------------------------------------------------------------------
                //    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //    string ActionNote = " فشل في حذف مستخدم رقم " + Users ;
                //    _SystemAction.SaveAction("DeleteUsers2", "UsersService", 3, "لا يمكنك حذف حساب الادمن", "", "", ActionDate, Users, BranchId, ActionNote, 0);
                //    //-----------------------------------------------------------------------------------------------------------------

                //    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكنك حذف حساب الادمن" };
                //}

                var SettingProjUser = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == Users).ToList();
                if (SettingProjUser != null && SettingProjUser.Count() > 0)
                {
                    var Values = SettingProjUser.GroupBy(p => p.ProjSubTypeId).Select(g => g.First()).ToList();

                    foreach (var item in Values)
                    {
                        var settings = _TaamerProContext.ProSettingDetails.Where(s => s.IsDeleted == false && s.ProjectSubtypeId == item.ProjSubTypeId).ToList();
                        if (settings != null && settings.Count() > 0)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote = " فشل في حذف مستخدم رقم " + Users;
                            _SystemAction.SaveAction("DeleteUsers2", "UsersService", 3, "المستخدم موجود علي  سير مشروع لا يمكن حذفه", "", "", ActionDate, Users, BranchId, ActionNote, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.IsLinkedToAnotherProcess };
                        }

                    }

                }
                var userTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.UserId == Users && s.Type == 3 && s.Status != 4 && s.IsMerig == -1).Count();
                if (userTasks > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " فشل في حذف مستخدم رقم " + Users;
                   _SystemAction.SaveAction("DeleteUsers2", "UsersService", 3, Resources.userHave + userTasks + Resources.userTasks, "", "", ActionDate, Users, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "userHave", ReturnedStrExtra = userTasks.ToString(), ReturnedStrExtra2 = "userTasks" };
                }
                var userProject = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.MangerId == Users && s.Status != 1).Count();
                if (userProject > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " فشل في حذف مستخدم رقم " + Users;
                   _SystemAction.SaveAction("DeleteUsers2", "UsersService", 3, Resources.userHave + userProject + Resources.UserProjects, "", "", ActionDate, Users, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "userHave", ReturnedStrExtra = userProject.ToString(), ReturnedStrExtra2 = "UserProjects" };
                }
                var userWorkOrder = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && (s.ExecutiveEng == Users || s.ResponsibleEng == Users) && (s.WOStatus == 1 || s.WOStatus == 2)).Count();
                if (userWorkOrder > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " فشل في حذف مستخدم رقم " + Users;
                   _SystemAction.SaveAction("DeleteUsers2", "UsersService", 3, Resources.userHave + userWorkOrder + Resources.userWorkOrder, "", "", ActionDate, Users, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "userHave", ReturnedStrExtra = userWorkOrder.ToString(), ReturnedStrExtra2 = "userWorkOrder" };
                }
                var emp = _TaamerProContext.Employees.Where(x => x.IsDeleted == false && x.UserId == Users).FirstOrDefault();
                if (emp != null)
                {
                    emp.UserId = null;
                }
                Users users = _UsersRepository.GetById(Users);
                users.IsDeleted = true;
                users.DeleteDate = DateTime.Now;
                users.DeleteUser = Users;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote4 = " حذف مستخدم  " + users.FullNameAr;
               _SystemAction.SaveAction("DeleteUsers2", "UsersService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate4, UserId, BranchId, ActionNote4, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف مستخدم رقم " + Users;
                _SystemAction.SaveAction("DeleteUsers2", "UsersService", 3, Resources.General_DeletedFailed, "", "", ActionDate, Users, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }


        public IEnumerable<object> FillUserSelect(int ExcludedUserId)
        {
            return _UsersRepository.GetMatching(s => s.IsDeleted == false && s.UserId != ExcludedUserId && s.IsAdmin != true && s.Status == 1 && s.ExpireDate == "0").Select(s => new
            {
                Id = s.UserId,
                Name = s.FullNameAr == null ? s.FullName : s.FullNameAr,
                DepartmentId = s.DepartmentId,
            });
        }
        public IEnumerable<object> FillAllUserSelect(int UserId)
        {
            return _UsersRepository.GetMatching(s => s.IsDeleted == false && s.IsAdmin != true && s.Status == 1 && s.ExpireDate == "0" && s.UserId != UserId).Select(s => new
            {
                Id = s.UserId,
                Name = s.FullNameAr == null ? s.FullName : s.FullNameAr,
                DepartmentId = s.DepartmentId,
            });
        }
        //public IEnumerable<object> FillAllUserSelectExcept(int UserId, int ExceptUserId)
        //{
        //    return _UsersRepository.GetMatching(s => s.IsDeleted == false && s.IsAdmin != true && s.Status == 1 && s.ExpireDate == "0" && s.UserId != UserId).Select(s => new
        //    {
        //        Id = s.UserId,
        //        Name = s.FullName
        //    });
        //}
        public IEnumerable<object> FillAllUsersSelectAll(int UserId)
        {
            return _UsersRepository.GetMatching(s => s.IsDeleted == false && s.IsAdmin != true && s.Status == 1 && s.ExpireDate == "0").Select(s => new
            {
                Id = s.UserId,
                Name = s.FullNameAr == null ? s.FullName : s.FullNameAr,
                DepartmentId = s.DepartmentId,
            });
        }
        public IEnumerable<object> FillAllUsersSelectAllGrantt(int UserId,string param)
        {
            return _UsersRepository.GetMatching(s => s.IsDeleted == false && s.IsAdmin != true && s.Status == 1 && s.ExpireDate == "0").Select(s => new
            {
                Id = s.UserId,
                Key = s.FullNameAr == null ? s.FullName : s.FullNameAr,
                ImgUrl = param+(s.ImgUrl ?? "/distnew/images/userprofile.png"),
            });
        }


        public IEnumerable<object> FillAllUsersSelectAll(int UserId,int BranchId)
        {
            return _UsersRepository.GetMatching(s => s.IsDeleted == false && s.IsAdmin != true && s.BranchId== BranchId && s.Status == 1 && s.ExpireDate == "0").Select(s => new
            {
                Id = s.UserId,
                Name = s.FullNameAr == null ? s.FullName : s.FullNameAr,
                DepartmentId = s.DepartmentId,
            });
        }
        public GeneralMessage ChangePassword(Users users, int UserId, int BranchId)
        {
            try
            {
                var UsersUpdated = _UsersRepository.GetById(users.UserId);
                // UsersUpdated.Password = users.Password;
                UsersUpdated.Password = EncryptValue(users.Password);
                UsersUpdated.UpdateUser = UserId;
                UsersUpdated.UpdateDate = DateTime.Now;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تغيير كلمة السر للمستخدم  " + UsersUpdated.FullNameAr;
                _SystemAction.SaveAction("ChangePassword", "UsersService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في  تغيير كلمة السر للمستخدم رقم " + users;
                _SystemAction.SaveAction("ChangePassword", "UsersService", 2, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage ChangeLinkvalidPassword(Users users, int UserId, int BranchId)
        {
            try
            {
                var UsersUpdated = _UsersRepository.GetById(users.UserId);
                // UsersUpdated.Password = users.Password;
                UsersUpdated.Password = EncryptValue(users.Password);
                UsersUpdated.UpdateUser = UserId;
                UsersUpdated.UpdateDate = DateTime.Now;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تغيير لينك التفعيل ";
                _SystemAction.SaveAction("ChangeLinkvalidPassword", "UsersService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في تغيير لينك التفعيل ";
                _SystemAction.SaveAction("ChangeLinkvalidPassword", "UsersService", 2, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }



        public GeneralMessage DeleteDeviceId(int user, int UserId, int BranchId)
        {
            try
            {
                var UsersUpdated = _UsersRepository.GetById(user);
                // UsersUpdated.Password = users.Password;
                UsersUpdated.DeviceId = null;
                UsersUpdated.UpdateUser = UserId;
                UsersUpdated.UpdateDate = DateTime.Now;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف معرف الجوال ";
                _SystemAction.SaveAction("ChangeLinkvalidPassword", "UsersService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف معرف فلجوال ";
                _SystemAction.SaveAction("ChangeLinkvalidPassword", "UsersService", 2, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }



        public GeneralMessage ChangeUserImage(Users users, int UserId, int BranchId)
        {
            try
            {
                var UsersUpdated = _UsersRepository.GetById(users.UserId);
                UsersUpdated.ImgUrl = users.ImgUrl;
                UsersUpdated.UpdateUser = UserId;
                UsersUpdated.UpdateDate = DateTime.Now;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تغيير صورةالمستخدم رقم " + users.UserId;
                _SystemAction.SaveAction("ChangeUserImage", "UsersService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في تغيير صورةالمستخدم  " + users.FullNameAr; ;
                _SystemAction.SaveAction("ChangeUserImage", "UsersService", 2, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }



        public GeneralMessage ChangeUserStamp(Users users, int UserId, int BranchId)
        {
            try
            {
                var UsersUpdated = _UsersRepository.GetById(users.UserId);
                if (!string.IsNullOrEmpty(users.StampUrl))
                    UsersUpdated.StampUrl = EncryptValue(users.StampUrl);
                else
                    UsersUpdated.StampUrl = users.StampUrl;
                UsersUpdated.UpdateUser = UserId;
                UsersUpdated.UpdateDate = DateTime.Now;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تغيير ختم المستخدم  " + users.FullNameAr;
                _SystemAction.SaveAction("ChangeUserStamp", "UsersService", 2,"Resources.userHave + userWorkOrder + Resources.userWorkOrder", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في تغيير ختم المستخدم  " + users.FullNameAr; ;
                _SystemAction.SaveAction("ChangeUserStamp", "UsersService", 2, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage SaveUserPrivilegesUsers(int AssignedUserId, List<int> Privs, int UserId, int BranchId, string Con)
        {
            try
            {
                SqlConnection con = new SqlConnection(Con);
                con.Open();
                SqlCommand cmd = new SqlCommand("delete Sys_UserPrivileges where userid =" + AssignedUserId.ToString());
                cmd.Connection = con;
                cmd.ExecuteNonQuery();


                if (Privs != null)
                {
                    foreach (var item in Privs)
                    {
                        ////////////////////////////////
                        SqlCommand _cmd = new SqlCommand("exec InsertUserprivs " + AssignedUserId.ToString() + "," + item.ToString() + ",1,1,1,1" + "," + UserId.ToString());
                        _cmd.Connection = con;
                        _cmd.ExecuteNonQuery();
                        ///////////////////////////////////////////
                    }
                    //_TaamerProContext.SaveChanges();

                    con.Close();
                }
                var user = new Users();
                try
                {
                    user=_UsersRepository.GetById(AssignedUserId);

                }
                catch (Exception ex)
                {

                }
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "حفظ صلاحيات المستخدم" + user.FullNameAr;
                   _SystemAction.SaveAction("SaveUserPrivilegesUsers", "UsersService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ صلاحيات المستخدم";
                _SystemAction.SaveAction("SaveUserPrivilegesUsers", "UsersService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage SaveGroupPrivilegesUsers(int GroupId, List<int> Privs, int UserId, int BranchId, string Con)
        {
            try
            {
                var grpUsers = _UsersRepository.GetMatching(s => s.IsDeleted == false && s.GroupId == GroupId);

                SqlConnection con = new SqlConnection(Con);
                SqlCommand cmd, _cmd;
                con.Open();

                foreach (var grpUser in grpUsers)
                {
                    cmd = new SqlCommand("delete Sys_UserPrivileges where userid =" + grpUser.UserId.ToString());
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();

                    if (Privs != null)
                    {
                        foreach (var item in Privs)
                        {
                            ////////////////////////////////
                            _cmd = new SqlCommand("exec InsertUserprivs " + grpUser.UserId.ToString() + "," + item.ToString() + ",1,1,1,1" + "," + UserId.ToString());
                            _cmd.Connection = con;
                            _cmd.ExecuteNonQuery();
                            ///////////////////////////////////////////
                        }
                        //_TaamerProContext.SaveChanges();
                    }
                }
                con.Close();
                var user = new Groups();
                try
                {
                    user = _TaamerProContext.Groups.Where(x=>x.GroupId== GroupId).FirstOrDefault();

                }
                catch (Exception ex)
                {

                }
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "حفظ صلاحيات المجموعه" + user.NameAr;
                _SystemAction.SaveAction("SaveGroupPrivilegesUsers", "UsersService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase =  Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ صلاحيات المجموعه";
                _SystemAction.SaveAction("SaveGroupPrivilegesUsers", "UsersService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public List<int> GetPrivilegesIdsByUserId(int UserId)
        {
            var PrivIds = new List<int>();
            var Privs = _TaamerProContext.UserPrivileges.Where(s => s.UserId == UserId && s.IsDeleted == false).OrderBy(o => o.UserPrivId).ToList();
            if (Privs != null && Privs.Count > 0)
            {
                PrivIds = Privs.Select(s => s.PrivilegeId ?? 0).ToList();
            }
            return PrivIds;
        }
        public List<int> GetBranchesByUserId(int UserId)
        {
            var branchesIds = new List<int>();
            var branches = _TaamerProContext.UserBranches.Where(s => s.UserId == UserId && s.IsDeleted == false).ToList();
            if (branches != null && branches.Count > 0)
            {
                branchesIds = branches.Select(s => s.BranchId).ToList();
            }
            return branchesIds;
        }
        public async Task<IEnumerable<BranchesVM>> GetAllBranchesByUserName(string Lang, string UseName)
        {
            var user = _UsersRepository.GetMatching(s => s.IsDeleted == false && s.UserName == UseName).FirstOrDefault();
            if (user != null)
            {
                var branches = await _userBranchesRepository.GetAllBranchesByUserId(Lang, user.UserId);
                return branches;
            }
            return null;
        }
        public bool ValidateUserCofidential(string UserName, string Password, string activationCode)
        {
            if (UserName == "admin")
            {

                var user = _UsersRepository.GetMatching(s => s.UserName == UserName && s.IsDeleted == false && DecryptValue(s.Password) == Password.Trim() &&
                (s.ActivationCode == activationCode || String.IsNullOrEmpty(s.ActivationCode))).FirstOrDefault();

                if (user != null)
                {
                    return true;
                }
                return false;

            }
            else
            {
                //Status == 0 --> disactive
                var user = _UsersRepository.GetMatching(s => s.UserName == UserName && s.IsDeleted == false && DecryptValue(s.Password) == Password.Trim() &&
                (s.ActivationCode == activationCode || String.IsNullOrEmpty(s.ActivationCode))).FirstOrDefault();
                if (user != null)
                {
                    return true;
                }
                return false;

            }

        }
        public bool ProcessActivationCode(string UserName, string Password, int BranchId)
        {
            var user = _UsersRepository.GetMatching(s => s.UserName == UserName && s.IsDeleted == false && DecryptValue(s.Password) == Password.Trim()).FirstOrDefault();
            if (user != null && user.IsAdmin != true)
            {
                try
                {
                    var lastLogin = DateTime.Now.Subtract(user.LastLoginDate ?? DateTime.Now).TotalHours;
                    if (lastLogin == 0 || lastLogin > 2)
                    {
                        user.ActivationCode = GenerateRandomNo().ToString();
                        _TaamerProContext.SaveChanges();
                        var mail = new MailMessage();
                        var barnchData = _branchRepository.GetById(user.BranchId??0);
                        var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.Password);
                        //var loginInfo = new NetworkCredential("bayanatech4788@gmail.com", "123456789@support");

                        if (_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.DisplayName != null)
                        {
                            mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.DisplayName);
                        }
                        else
                        {
                            mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                        }

                        // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).SenderEmail);
                        mail.To.Add(new MailAddress(_UsersRepository.GetById(user.UserId).Email));
                        mail.Subject = "كود تفعيل تسجيل الدخول لنظام تعمير برو";
                        mail.Body = "كود التفعيل الخاص بكم هو " + user.ActivationCode + "";
                        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.Host);
                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        //smtpClient.Port = 587;
                        smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.Port);

                        smtpClient.Credentials = loginInfo;
                        smtpClient.Send(mail);
                        return true;
                    }
                    else if (lastLogin != 0 && lastLogin < 2)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }
        //
        public bool ForgetPassword(string Email, string link, string emailFor = "VerifyAccount", string Emailbody = "")
        {

            var user = _UsersRepository.GetMatching(s => s.IsDeleted == false && s.Email == Email.Trim()).FirstOrDefault();
            if (user != null && user.IsAdmin != true)
            {
                try
                {
                    var ValidPassword = DecryptValue(user.Password);
                    var mail = new MailMessage();
                    var barnchData = _branchRepository.GetById((int)user.BranchId);
                    //var loginInfo = new NetworkCredential("bayanatech4788@gmail.com", "123456789@support");
                    var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.Password);

                    if (_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.DisplayName != null)
                    {
                        mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.DisplayName);
                    }
                    else
                    {
                        mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                    }

                    // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).SenderEmail);
                    mail.To.Add(new MailAddress(_UsersRepository.GetById(user.UserId).Email));
                    string subject = "نسيت كلمة المرور ";
                    mail.Subject = subject;
                    mail.Body = Emailbody;
                    LinkedResource logo = new LinkedResource(emailFor);
                    logo.ContentId = "companylogo";
                    // done HTML formatting in the next line to display my bayanatech logo
                    AlternateView av1 = AlternateView.CreateAlternateViewFromString(Emailbody, null, MediaTypeNames.Text.Html);
                    av1.LinkedResources.Add(logo);
                    mail.AlternateViews.Add(av1);
                    mail.IsBodyHtml = true;
                    //Dawoud Added
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.Host);
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    //smtpClient.Port = 587;
                    smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.Port);

                    smtpClient.Credentials = loginInfo;
                    smtpClient.Send(mail);
                    //Update 
                    var Updatelink = UpdateLastLinkvalidDate(user.UserId, link);
                    //// notify admin
                    var AdminNotification = new Notification();
                    var admin = _UsersRepository.GetMatching(s => s.IsDeleted == false && s.IsAdmin == true).FirstOrDefault();
                    AdminNotification.ReceiveUserId = admin != null ? admin.UserId : 1;
                    AdminNotification.Name = "طلب إعادة تعيين كلمة المرور";
                    AdminNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                    AdminNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                    AdminNotification.SendUserId = user.UserId;
                    AdminNotification.Type = 1; // notification
                    AdminNotification.Description = "Resources.Pro_User + user.FullName + Resources.sendPassword";
                    AdminNotification.AllUsers = false;
                    AdminNotification.SendDate = DateTime.Now;
                    AdminNotification.AddUser = user.UserId;
                    AdminNotification.AddDate = DateTime.Now;
                    _TaamerProContext.Notification.Add(AdminNotification);
                    _notificationService.sendmobilenotification(admin != null ? admin.UserId : 1, "طلب إعادة تعيين كلمة المرور", Resources.Pro_User + user.FullName + Resources.sendPassword);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }
        public string ForgetPasswordError(string Email, string link, string emailFor = "VerifyAccount", string Emailbody = "")
        {

            var user = _UsersRepository.GetMatching(s => s.IsDeleted == false && s.Email == Email.Trim()).FirstOrDefault();
            if (user != null && user.IsAdmin != true)
            {
                try
                {
                    var ValidPassword = DecryptValue(user.Password);
                    var mail = new MailMessage();
                    var barnchData = _branchRepository.GetById((int)user.BranchId);
                    //var loginInfo = new NetworkCredential("bayanatech4788@gmail.com", "123456789@support");
                    var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.Password);

                    if (_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.DisplayName != null)
                    {
                        mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.DisplayName);
                    }
                    else
                    {
                        mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                    }

                    // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).SenderEmail);
                    mail.To.Add(new MailAddress(_UsersRepository.GetById(user.UserId).Email));
                    string subject = "نسيت كلمة المرور ";
                    mail.Subject = subject;
                    mail.Body = Emailbody;
                    LinkedResource logo = new LinkedResource(emailFor);
                    logo.ContentId = "companylogo";
                    // done HTML formatting in the next line to display my bayanatech logo
                    AlternateView av1 = AlternateView.CreateAlternateViewFromString(Emailbody, null, MediaTypeNames.Text.Html);
                    av1.LinkedResources.Add(logo);
                    mail.AlternateViews.Add(av1);
                    mail.IsBodyHtml = true;
                    //Dawoud Added
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.Host);
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    //smtpClient.Port = 587;
                    smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.Port);

                    smtpClient.Credentials = loginInfo;
                    smtpClient.Send(mail);
                    //Update 
                    var Updatelink = UpdateLastLinkvalidDate(user.UserId, link);
                    //// notify admin
                    var AdminNotification = new Notification();
                    var admin = _UsersRepository.GetMatching(s => s.IsDeleted == false && s.IsAdmin == true).FirstOrDefault();
                    AdminNotification.ReceiveUserId = admin != null ? admin.UserId : 1;
                    AdminNotification.Name = "طلب إعادة تعيين كلمة المرور";
                    AdminNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                    AdminNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                    AdminNotification.SendUserId = user.UserId;
                    AdminNotification.Type = 1; // notification
                    AdminNotification.Description = Resources.Pro_User + user.FullName + @Resources.sendPassword;
                    AdminNotification.AllUsers = false;
                    AdminNotification.SendDate = DateTime.Now;
                    AdminNotification.AddUser = user.UserId;
                    AdminNotification.AddDate = DateTime.Now;
                    _TaamerProContext.Notification.Add(AdminNotification);
                    _notificationService.sendmobilenotification(admin != null ? admin.UserId : 1, "طلب إعادة تعيين كلمة المرور", Resources.Pro_User + user.FullName + Resources.sendPassword);
                    return "true";
                }
                catch (Exception ex)
                {
                    return ex.Message + "--" + ex.InnerException;
                }
            }
            return "false";
        }

        public bool RetrievePassword(string UserName, string Email, string emailFor = "VerifyAccount", string Emailbody = "")
        {
            var user = _UsersRepository.GetMatching(s => s.UserName == UserName && s.IsDeleted == false && s.Email == Email.Trim()).FirstOrDefault();
            if (user != null && user.IsAdmin != true)
            {
                try
                {
                    var ValidPassword = DecryptValue(user.Password);
                    var mail = new MailMessage();
                    var barnchData = _branchRepository.GetById((int)user.BranchId);
                    //var loginInfo = new NetworkCredential("bayanatech4788@gmail.com", "123456789@support");
                    var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.Password);

                    if (_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.DisplayName != null)
                    {
                        mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.DisplayName);
                    }
                    else
                    {
                        mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                    }

                    //mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).SenderEmail);
                    mail.To.Add(new MailAddress(_UsersRepository.GetById(user.UserId).Email));
                    mail.Subject = "استرجاع كلمة المرور";
                    mail.Body = "الرقم السري الخاص بكم  للدخول للنظام هو " + ValidPassword + "";

                    LinkedResource logo = new LinkedResource(emailFor);
                    logo.ContentId = "companylogo";
                    // done HTML formatting in the next line to display my bayanatech logo
                    AlternateView av1 = AlternateView.CreateAlternateViewFromString(Emailbody.Replace("{AccountPassword}", ValidPassword), null, MediaTypeNames.Text.Html);
                    av1.LinkedResources.Add(logo);
                    mail.AlternateViews.Add(av1);
                    mail.IsBodyHtml = true;
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.Host);
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    //smtpClient.Port = 587;
                    smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(barnchData.OrganizationId).Result.Port);

                    smtpClient.Credentials = loginInfo;
                    smtpClient.Send(mail);

                    //// notify admin
                    var AdminNotification = new Notification();
                    var admin = _UsersRepository.GetMatching(s => s.IsDeleted == false && s.IsAdmin == true).FirstOrDefault();
                    AdminNotification.ReceiveUserId = admin != null ? admin.UserId : 1;
                    AdminNotification.Name = "طلب ارسال رقم سري";
                    AdminNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                    AdminNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                    AdminNotification.SendUserId = user.UserId;
                    AdminNotification.Type = 1; // notification
                    AdminNotification.Description =" Resources.Pro_User + user.FullName + Resources.sendPassword";
                    AdminNotification.AllUsers = false;
                    AdminNotification.SendDate = DateTime.Now;
                    AdminNotification.AddUser = user.UserId;
                    AdminNotification.AddDate = DateTime.Now;
                    _TaamerProContext.Notification.Add(AdminNotification);
                    _notificationService.sendmobilenotification(admin != null ? admin.UserId : 1, "طلب ارسال رقم سري", Resources.Pro_User + user.FullName + Resources.sendPassword);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }

        private bool SendMail(int BranchId, int UserId, string fullname)
        {
            try
            {

                var DateOfChanged = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));

                var mail = new MailMessage();
                var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(BranchId).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(BranchId).Result.Password);

                if (_EmailSettingRepository.GetEmailSetting(BranchId).Result.DisplayName != null)
                {
                    mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(BranchId).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(BranchId).Result.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(BranchId).Result.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                }

                // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(BranchId).SenderEmail);
                mail.To.Add(new MailAddress(_UsersRepository.GetById(UserId).Email));
                //mail.Subject = "لديك مهمة جديده علي مشروع رقم " + project.ProjectNo + "";
                mail.Subject = "Change Password";
                try
                {
                    mail.Body = "Dear Mr." + fullname + ", Your Password was changed successfully at date : " + DateOfChanged + "";
                    mail.IsBodyHtml = true;
                }
                catch (Exception)
                {
                    mail.Body = "";
                }
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(BranchId).Result.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(BranchId).Result.Port);

                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception wx)
            {
                return false;
            }
        }

        private bool SendMail2(int BranchId, int UserId, string fullname, int flag)
        {
            try
            {



                var DateOfChanged = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));

                var mail = new MailMessage();
                var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(BranchId).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(BranchId).Result.Password);

                if (_EmailSettingRepository.GetEmailSetting(BranchId).Result.DisplayName != null)
                {
                    mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(BranchId).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(BranchId).Result.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(BranchId).Result.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                }

                // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(BranchId).SenderEmail);
                mail.To.Add(new MailAddress(_UsersRepository.GetById(UserId).Email));
                //mail.Subject = "لديك مهمة جديده علي مشروع رقم " + project.ProjectNo + "";
                mail.Subject = "Update in the user account";



                if (flag == 1)
                {
                    try
                    {
                        mail.Body = "Dear Mr." + fullname + ", Your account has been Suspended at date : " + DateOfChanged + "";
                        mail.IsBodyHtml = true;
                    }
                    catch (Exception)
                    {
                        mail.Body = "";
                    }
                }
                else
                {
                    try
                    {
                        mail.Body = "Dear Mr." + fullname + ", Your account has been Enabled at date : " + DateOfChanged + "";
                        mail.IsBodyHtml = true;
                    }
                    catch (Exception)
                    {
                        mail.Body = "";
                    }
                }
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(BranchId).Result.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(BranchId).Result.Port);

                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ChangePasswordLink(string link, string password, int UserId, int BranchId)
        {
            try
            {


                //var user = _UsersRepository.GetMatching(s => s.RecoverPasswordLink == link.Trim() && s.IsDeleted == false).FirstOrDefault();
                var user = _UsersRepository.GetUserByRecoverLink(link).Result;
                //int UserID 
                try
                {
                    var Valid = user.RecoverPasswordDate.Value.AddHours(3) > DateTime.Now;
                    if (Valid == true)
                    {
                        var UsersUpdated = _UsersRepository.GetById(user.UserId);
                        // UsersUpdated.Password = users.Password;
                        UsersUpdated.Password = EncryptValue(password);
                        UsersUpdated.UpdateUser = user.UserId;
                        UsersUpdated.UpdateDate = DateTime.Now;
                        _TaamerProContext.SaveChanges();

                        //var branch = _BranchesRepository.GetById(BranchId);

                        //try
                        //{
                        //    SendMail(branchid, user.UserId, user.FullName);
                        //}
                        //catch (Exception ex)
                        //{

                        //    return true;
                        //}

                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " تغيير كلمة السر للمستخدم  " + user.FullNameAr;
                        _SystemAction.SaveAction("ChangePasswordLink", "UsersService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                        return true;
                    }
                    else
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "فشل في تغيير كلمة السر للمستخدم  " + user.FullNameAr;
                        _SystemAction.SaveAction("ChangePasswordLink", "UsersService", 2, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return false;
                    }
                }
                catch (Exception ex)
                {
                    var UsersUpdated = _UsersRepository.GetById(user.UserId);
                    // UsersUpdated.Password = users.Password;
                    UsersUpdated.Password = EncryptValue(password);
                    UsersUpdated.UpdateUser = user.UserId;
                    UsersUpdated.UpdateDate = DateTime.Now;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تغيير كلمة السر للمستخدم  " + user.FullNameAr;
                    _SystemAction.SaveAction("ChangePasswordLink", "UsersService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return true;
                }
                //return true;
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تغيير كلمة السر للمستخدم رقم " + UserId;
                _SystemAction.SaveAction("ChangePasswordLink", "UsersService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return false;
            }

        }
        public async Task<UsersVM> GetUser(string username)
        {
            return await _UsersRepository.GetUser(username);
        }
        public async Task<UsersLoginVM> GetUserLogin(string username, string password)
        {
            return await _UsersRepository.GetUserLogin(username, password);
        }
        public async Task<UsersVM> GetUser_tadmin(string username)
        {
            return await _UsersRepository.GetUser_tadmin(username);
        }
        public async Task<UsersVM> GetUserById(int UserId, string Lang)
        {
            var result = _UsersRepository.GetUserById(UserId, Lang).Result;
            if(result!=null)
            {
                if (!string.IsNullOrEmpty(result.StampUrl))
                {
                    try
                    {
                        result.StampUrl = DecryptValue1(result.StampUrl);
                    }
                    catch (Exception ex)
                    {

                    }
                    //string ImageIn = Server.Map
                }
            }

            return result;
        }

        public async Task<UsersVM> GetUserById2(int UserId, string Lang)
        {
            var result = await _UsersRepository.GetUserById(UserId, Lang);

            return result;
        }
        public async Task<UsersVM> GetUserByEmailId(string EmailId)
        {
            return await _UsersRepository.GetUserByEmailId(EmailId);
        }
        public async Task<UsersVM> GetUserByVeificationLinkId(string LinkId)
        {
            return await _UsersRepository.GetUserByVeificationLinkId(LinkId);
        }
        public int UpdateOnlineStatus(bool IsOnline, int User, int UserId, int BranchId)
        {
            try
            {
                var UsersUpdated = _UsersRepository.GetById(User);
                UsersUpdated.IsOnline = IsOnline;
                if (IsOnline == false)
                {
                    UsersUpdated.LastSeenDate = DateTime.Now;
                }
                else
                {
                    //UsersUpdated.LastSeenDate = null;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تعديل حالة الاتصال للمستخدم رقم " + UsersUpdated.FullNameAr;
                _SystemAction.SaveAction("UpdateOnlineStatus", "UsersService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return 1;
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تعديل حالة الاتصال للمستخدم رقم" + User;
                _SystemAction.SaveAction("UpdateOnlineStatus", "UsersService", 2, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return 0;
            }

        }

        public int UpdateOnlineStatus2(bool IsOnlineNew, int User, int UserId, int BranchId)
        {
            try
            {
                var UsersUpdated = _UsersRepository.GetById(User);
                if (UsersUpdated != null)
                {
                    UsersUpdated.ISOnlineNew = IsOnlineNew;
                    if (IsOnlineNew == false)
                    {
                        UsersUpdated.LastSeenDate = DateTime.Now;
                    }
                    else
                    {
                        //UsersUpdated.LastSeenDate = null;
                    }
                    _TaamerProContext.SaveChanges();
                }

                ////-----------------------------------------------------------------------------------------------------------------
                //string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //string ActionNote = " تعديل حالة الاتصال للمستخدم رقم " + User;
                //SaveAction("UpdateOnlineStatus2", "UsersService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                ////-----------------------------------------------------------------------------------------------------------------

                return 1;
            }
            catch (Exception ex)
            {
                ////-----------------------------------------------------------------------------------------------------------------
                //string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //string ActionNote = "فشل في تعديل حالة الاتصال للمستخدم رقم" + User;
                //SaveAction("UpdateOnlineStatus2", "UsersService", 2, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                ////-----------------------------------------------------------------------------------------------------------------

                return 0;
            }

        }

        public int LogoutUser(bool IsOnlineNew, int User, int UserId, int BranchId)
        {
            int result = this.UpdateOnlineStatus2(IsOnlineNew, User, UserId, BranchId);
            if (result == 1)
            {  ////-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تعديل حالة الاتصال للمستخدم رقم " + User;
                _SystemAction.SaveAction("LogoutUser", "UsersService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
            }

            else
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تعديل حالة الاتصال للمستخدم رقم" + User;
                _SystemAction.SaveAction("LogoutUser", "UsersService", 2, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
            }
            return result;
        }

        public int UpdateActiveTime(int UserId, int BranchId)
        {
            try
            {
                var UsersUpdated = _UsersRepository.GetById(UserId);
                if (UsersUpdated != null)
                {
                    UsersUpdated.ActiveTime = DateTime.Now;
                    _TaamerProContext.SaveChanges();
                }


                ////-----------------------------------------------------------------------------------------------------------------
                //string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //string ActionNote = " تعديل وقت التفعيل للمستخدم رقم " + UserId;
                //SaveAction("UpdateActiveTime", "UsersService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                ////-----------------------------------------------------------------------------------------------------------------

                return 1;
            }
            catch (Exception)
            {
                ////-----------------------------------------------------------------------------------------------------------------
                //string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //string ActionNote = "فشل في تعديل وقت التفعيل للمستخدم رقم " + UserId;
                //SaveAction("SaveClause", "Acc_ClausesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                ////-----------------------------------------------------------------------------------------------------------------

                return 0;
            }
        }
        public int UpdateLastLoginDate(int UserId)
        {
            try
            {
                var UsersUpdated = _UsersRepository.GetById(UserId);
                UsersUpdated.LastLoginDate = UsersUpdated.LastLogOutDate; //LastLogout is used as CurrentLogin Column (*-*)
                UsersUpdated.LastLogOutDate = DateTime.Now;
                _TaamerProContext.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int ClearExpireDate(int UserId)
        {
            try
            {
                var UsersUpdated = _UsersRepository.GetById(UserId);
                UsersUpdated.ExpireDate = "";
                _TaamerProContext.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int UpdateLastLinkvalidDate(int UserId, string link)
        {
            try
            {
                var UsersUpdated = _UsersRepository.GetById(UserId);
                UsersUpdated.RecoverPasswordLink = link;
                UsersUpdated.RecoverPasswordDate = DateTime.Now;
                _TaamerProContext.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private string EncryptValue(string value)
        {
            string hash = "f0xle@rn";
            byte[] data = Encoding.UTF8.GetBytes(value);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDesc = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform cryptoTransform = tripDesc.CreateEncryptor();
                    byte[] result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(result, 0, result.Length);
                }
            }
        }
        private string DecryptValue(string value)
        {
            string hash = "f0xle@rn";
            byte[] data = Convert.FromBase64String(value); ;
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDesc = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform cryptoTransform = tripDesc.CreateDecryptor();
                    byte[] result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);
                    return Encoding.UTF8.GetString(result);
                }
            }
        }
        public string DecryptValue1(string value)
        {
            string hash = "f0xle@rn";
            byte[] data = Convert.FromBase64String(value); ;
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDesc = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform cryptoTransform = tripDesc.CreateDecryptor();
                    byte[] result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);
                    return Encoding.UTF8.GetString(result);
                }
            }
        }
        //Generate RandomNo
        private int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }


        public GeneralMessage CloseUser(Users users, int userid)
        {

            if (users.UserId != 0)
            {
                if (users.UserId == userid)
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.usercantaway };

                }
                else
                {
                    var Userss = _UsersRepository.GetById(users.UserId);
                    if (Userss.UserName == "admin")
                    {
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.admincantaway };
                    }
                    else
                    {
                        try
                        {
                            var UsersUpdated = _UsersRepository.GetById(users.UserId);
                            UsersUpdated.ISOnlineNew = false;
                            UsersUpdated.LastSeenDate = DateTime.Now;
                            _TaamerProContext.SaveChanges();
                            return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase =  Resources.UserAway };

                        }
                        catch (Exception)
                        {
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.sys_falidToaway };
                        }
                    }

                }

            }
            else
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.SelectUser };
            }

        }
        public async Task<IEnumerable<UsersVM>> GetFullReport(ProjectPhasesTasksVM Search, string Lang, string Today, int BranchId)
        {
            var Users = await _UsersRepository.GetFullReport(Search, Lang, Today, BranchId);
            return Users;
        }
        public GeneralMessage UpdateAppActiveStatus(bool IsActivated, int User, int UserId, int BranchId)
        {
            try
            {
                var UsersUpdated = _UsersRepository.GetById(User);
                if(UsersUpdated!=null)
                {
                    UsersUpdated.IsActivated = IsActivated;
                    UsersUpdated.UpdateDate = DateTime.Now;
                    _TaamerProContext.SaveChanges();
                }
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تعديل حالة التفعيل  للتطبيق للمستخدم رقم " + User;
                _SystemAction.SaveAction("UpdateAppActiveStatus", "UsersService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase =  Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تعديل حالة التفعيل  للتطبيق للمستخدم رقم" + User;
                _SystemAction.SaveAction("UpdateAppActiveStatus", "UsersService", 2, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedSave };
            }

        }
        public GeneralMessage Disappearewelcomeuser(int UserId, int BranchId)
        {
            try
            {
                var UsersUpdated = _UsersRepository.GetById(UserId);
                UsersUpdated.AppearWelcome = null;
                UsersUpdated.UpdateUser = UserId;
                UsersUpdated.UpdateDate = DateTime.Now;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "  اخفاء رسالة ترحيب المستخدم  " + UsersUpdated.FullNameAr;
                _SystemAction.SaveAction("ChangeUserImage", "UsersService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase =  Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في اخفاء رسالة ترحيب المستخدم رقم " + UserId; ;
                _SystemAction.SaveAction("ChangeUserImage", "UsersService", 2, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public int UpdateQrCodeUser(int UserId, string Qrcodeurl)
        {
            try
            {
                var UsersUpdated = _UsersRepository.GetById(UserId);
                UsersUpdated.QrCodeUrl = Qrcodeurl;
                UsersUpdated.UpdateDate = DateTime.Now;
                _TaamerProContext.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}

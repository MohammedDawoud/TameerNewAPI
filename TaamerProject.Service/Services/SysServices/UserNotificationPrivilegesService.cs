using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class UserNotificationPrivilegesService :   IUserNotificationPrivilegesService
    {
        private readonly IUserNotificationPrivilegesRepository _userNotificationPrivilegesRepository;
        private readonly IUsersRepository _UsersRepository;
        
        private readonly ICustomerSMSService _customerSMSService;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public UserNotificationPrivilegesService(TaamerProjectContext dataContext, IUserNotificationPrivilegesRepository userNotificationPrivilegesRepository
            ,ISystemAction systemAction, IUsersRepository usersRepository, ICustomerSMSService customerSMSService)
        {
            _userNotificationPrivilegesRepository = userNotificationPrivilegesRepository;
            _UsersRepository = usersRepository;
            
            _customerSMSService = customerSMSService;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task<IEnumerable<UserNotificationPrivilegesVM>> GetUsersByPrivilegesIds(int? Priv)
        {
            var Privileges = _userNotificationPrivilegesRepository.GetUsersByPrivilegesIds(Priv);
            
            return Privileges;
        }


        public GeneralMessage SaveUserPrivilegesUsers(int AssignedUserId, List<int> Privs, int UserId, int BranchId, string Con)
        {
            try
            {
                SqlConnection con = new SqlConnection(Con);
                con.Open();
                SqlCommand cmd = new SqlCommand("delete Sys_UserNotificationPrivileges where userid =" + AssignedUserId.ToString());
                cmd.Connection = con;
                cmd.ExecuteNonQuery();


                if (Privs != null)
                {
                    foreach (var item in Privs)
                    {
                        ////////////////////////////////
                        SqlCommand _cmd = new SqlCommand("exec InsertUserNotifprivs " + AssignedUserId.ToString() + "," + item.ToString() + ",1,1,1,1" + "," + UserId.ToString());
                        _cmd.Connection = con;
                        _cmd.ExecuteNonQuery();
                        ///////////////////////////////////////////
                    }
                    //_uow.SaveChanges();

                    con.Close();
                }

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "حفظ صلاحيات إشعار المستخدم";
                _SystemAction.SaveAction("SaveUserPrivilegesUsers", "UsersService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK ,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ صلاحيات إشعار المستخدم";
                _SystemAction.SaveAction("SaveUserPrivilegesUsers", "UsersService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest ,ReasonPhrase = Resources.General_SavedFailed };
                }
        }
        public  GeneralMessage SaveGroupPrivilegesUsers(int GroupId, List<int> Privs, int UserId, int BranchId, string Con)
        {
            try
            {
               // var grpUsers = _UsersRepository.GetMatching(s => s.IsDeleted == false && s.GroupId == GroupId);
                var grpUsers =  _TaamerProContext.Users.Where(s => s.IsDeleted == false && s.GroupId == GroupId);
                if (grpUsers !=null)
                {

                    SqlConnection con = new SqlConnection(Con);
                    SqlCommand cmd, _cmd;
                    con.Open();

                    foreach (var grpUser in grpUsers)
                    {
                        cmd = new SqlCommand("delete Sys_UserNotificationPrivileges where userid =" + grpUser.UserId.ToString());
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();

                        if (Privs != null)
                        {
                            foreach (var item in Privs)
                            {
                                ////////////////////////////////
                                _cmd = new SqlCommand("exec InsertUserNotifprivs " + grpUser.UserId.ToString() + "," + item.ToString() + ",1,1,1,1" + "," + UserId.ToString());
                                _cmd.Connection = con;
                                _cmd.ExecuteNonQuery();
                                ///////////////////////////////////////////
                            }
                            //_uow.SaveChanges();
                        }
                    }
                    con.Close();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "حفظ صلاحيات إشعار المستخدم";
                    _SystemAction.SaveAction("SaveGroupPrivilegesUsers", "UsersService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ صلاحيات إشعار المستخدم";
                _SystemAction.SaveAction("SaveGroupPrivilegesUsers", "UsersService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public async Task<List<int>> GetPrivilegesIdsByUserId(int UserId)
        {
            try
            {
                var PrivIds = new List<int>();
                // var Privs = _userNotificationPrivilegesRepository.GetMatching(s => s.UserId == UserId && s.IsDeleted == false).OrderBy(o => o.UserPrivId).ToList();
                var Privs =  _TaamerProContext.UserNotificationPrivileges.Where(s => s.UserId == UserId && s.IsDeleted == false).OrderBy(o => o.UserPrivId).ToList();

                if (Privs != null && Privs.Count > 0)
                {
                    PrivIds = Privs.Select(s => s.PrivilegeId ?? 0).ToList();
                }
                return PrivIds;

            }
            catch (Exception ex)
            {
                return new List<int>();
                throw;
            }

        }

        public bool SendSMS(string ReceiveNumber, string Message, int UserId, int BranchId)
        {
            var result = _customerSMSService.SaveCustomerSMS_Notification(ReceiveNumber, Message, UserId, BranchId);
            if (result.StatusCode==HttpStatusCode.OK)
                return true;
            else
                return false;
        }




    }
}

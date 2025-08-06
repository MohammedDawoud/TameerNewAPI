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
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class GroupPrivilegeService :  IGroupPrivilegeService
    {
        private readonly IGroupPrivilegesRepository _GroupPrivilegesRepository;
        private readonly IUsersService _usersService;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;


        public GroupPrivilegeService(IGroupPrivilegesRepository GroupPrivilegesRepository, IUsersService usersService,
            TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _GroupPrivilegesRepository = GroupPrivilegesRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _usersService = usersService;
        }

        public GeneralMessage SaveUserPrivilegesGroups(int GroupId, List<int> Privs, int UserId, int BranchId)
        {
            try
            {
                bool usersMsg = false;
                // var existgroupPriv = _GroupPrivilegesRepository.GetMatching(s => s.IsDeleted == false && s.GroupId == GroupId);
                var existgroupPriv = _TaamerProContext.GroupPrivileges.Where(s => s.IsDeleted == false && s.GroupId == GroupId);

                if (existgroupPriv != null && existgroupPriv.Count() > 0)
                {
                    _TaamerProContext.GroupPrivileges.RemoveRange(existgroupPriv);
                }
                if (Privs != null)
                {
                    foreach (var item in Privs)
                    {
                        var Priv = new GroupPrivileges();
                        Priv.GroupId = GroupId;
                        Priv.PrivilegeId = item;
                        Priv.BranchId = BranchId;
                        Priv.IsDeleted = false;
                        Priv.AddDate = DateTime.Now;
                        Priv.AddUser = UserId;
                        _TaamerProContext.GroupPrivileges.Add(Priv);
                    }
                     _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة صلاحية مجموعات";
                    _SystemAction.SaveAction("SaveUserPrivilegesGroups", "GroupPrivilegeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    //usersMsg = _usersService.SaveGroupPrivilegesUsers(GroupId, Privs, UserId).Result;
                }
                //if(usersMsg)
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully } ;
                //else
                //    return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase ="فشل في حفظ الصلاحيات للمستخدمين" };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ صلاحيات المجموعات";
                _SystemAction.SaveAction("SaveUserPrivilegesGroups", "GroupPrivilegeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =Resources.General_SavedFailed };
            }
        }

        public GeneralMessage SaveUserPrivilegesGroups2(int GroupId, List<int> Privs, int UserId, int BranchId,string Con)
        {
            try
            {
                //var existgroupPriv = _GroupPrivilegesRepository.GetMatching(s => s.IsDeleted == false && s.GroupId == GroupId);
                var existgroupPriv = _TaamerProContext.GroupPrivileges.Where(s => s.IsDeleted == false && s.GroupId == GroupId);

                if (existgroupPriv != null && existgroupPriv.Count() > 0)
                {
                    _TaamerProContext.GroupPrivileges.RemoveRange(existgroupPriv);
                }
                if (Privs != null)
                {
                    foreach (var item in Privs)
                    {
                        var Priv = new GroupPrivileges();
                        Priv.GroupId = GroupId;
                        Priv.PrivilegeId = item;
                        Priv.BranchId = BranchId;
                        Priv.IsDeleted = false;
                        Priv.AddDate = DateTime.Now;
                        Priv.AddUser = UserId;
                        _TaamerProContext.GroupPrivileges.Add(Priv);
                    }
                     _TaamerProContext.SaveChanges();



                    try
                    {

                        SqlConnection con = new SqlConnection(Con);
                        con.Open();
                        SqlCommand cmd = new SqlCommand("delete Sys_UserPrivileges where userid =" + UserId);//AssignedUserId.ToString()
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();


                        if (Privs != null)
                        {
                            foreach (var item in Privs)
                            {
                                ////////////////////////////////
                                SqlCommand _cmd = new SqlCommand("exec InsertUserprivs " + UserId + "," + item.ToString() + ",1,1,1,1" + "," + UserId.ToString()); //AssignedUserId.ToString() awl wahda bs mn 3la shmal
                                _cmd.Connection = con;
                                _cmd.ExecuteNonQuery();
                                ///////////////////////////////////////////
                            }
                            // _TaamerProContext.SaveChanges();

                            con.Close();
                        }
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "اضافة صلاحية مجموعات";
                        _SystemAction.SaveAction("SaveUserPrivilegesGroups2", "GroupPrivilegeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_SavedSuccessfully };
                    }
                    catch (Exception)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote3 = "فشل في حفظ صلاحية المجموعات";
                        _SystemAction.SaveAction("SaveUserPrivilegesGroups2", "GroupPrivilegeService", 1, Resources.General_SavedFailed, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed  };
                        }



                }
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة صلاحية مجموعات";
                _SystemAction.SaveAction("SaveUserPrivilegesGroups2", "GroupPrivilegeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ صلاحية المجموعات";
                _SystemAction.SaveAction("SaveUserPrivilegesGroups2", "GroupPrivilegeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =Resources.General_SavedFailed };
            }
        }

        public List<int> GetPrivilegesIdsByGroupId(int GroupId)
        {
            var PrivIds = new List<int>();
            //var Privs = _GroupPrivilegesRepository.GetMatching(s => s.GroupId == GroupId && s.IsDeleted == false).ToList();
            var Privs = _TaamerProContext.GroupPrivileges.Where(s => s.GroupId == GroupId && s.IsDeleted == false).ToList();

            if (Privs != null && Privs.Count > 0)
            {
                PrivIds = Privs.Select(s => s.PrivilegeId ?? 0).ToList();
            }
            return PrivIds;
        }
    

    }
}

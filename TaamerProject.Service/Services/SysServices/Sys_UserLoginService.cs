using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common.FIlterModels;
using TaamerProject.Models.Common;
using TaamerProject.Service.Interfaces;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Service.Generic;
using TaamerProject.Repository.Repositories;
using System.Net;
using TaamerProject.Service.IGeneric;
using TaamerP.Service.LocalResources;
using Dropbox.Api.TeamLog;

namespace TaamerProject.Service.Services
{
    public class Sys_UserLoginService : ISys_UserLoginService
    {

        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly ISys_UserLoginRepository _Sys_UserLoginRepository;
        public Sys_UserLoginService(ISys_UserLoginRepository Sys_UserLoginRepository
            , TaamerProjectContext dataContext
            , ISystemAction systemAction)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _Sys_UserLoginRepository = Sys_UserLoginRepository;
        }

        public Task<IEnumerable<Sys_UserLoginVM>> GetAllUserLogin(int Type)
        {
            var UserLogin = _Sys_UserLoginRepository.GetAllUserLogin(Type);
            return UserLogin;
        }
        public Task<UsersLoginVM> GetUserLogin(string Email, string Password, int Type)
        {
            var UserLogin = _Sys_UserLoginRepository.GetUserLogin(Email, Password,Type);
            return UserLogin;
        }
        public GeneralMessage SaveUserLogin(Sys_UserLogin UserLogin, int UserId, int BranchId)
        {
            try
            {

                if (UserLogin.UserLoginId == 0)
                {


                    UserLogin.AddUser = UserId;
                    UserLogin.AddDate = DateTime.Now;
                    _TaamerProContext.Sys_UserLogin.Add(UserLogin);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة مستخدم عميل أو مقاول جديد";
                    _SystemAction.SaveAction("SaveUserLogin", "Sys_UserLoginService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully};
                }
                else
                {
                    //var SupplierUpdated = _Sys_UserLoginRepository.GetById(UserLogin.UserLoginId);
                    var UserLoginUpdated = _TaamerProContext.Sys_UserLogin.Where(s => s.UserLoginId == UserLogin.UserLoginId).FirstOrDefault();

                    if (UserLoginUpdated != null)
                    {                       
                        UserLoginUpdated.NameAr = UserLogin.NameAr;
                        UserLoginUpdated.NameEn = UserLogin.NameEn;
                        UserLoginUpdated.UpdateUser = UserId;
                        UserLoginUpdated.UpdateDate = DateTime.Now;
                       
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل مستخدم عميل أو مقاول رقم " + UserLogin.UserLoginId;
                    _SystemAction.SaveAction("SaveUserLogin", "Sys_UserLoginService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully};
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ مستخدم عميل أو مقاول";
                _SystemAction.SaveAction("SaveUserLogin", "Sys_UserLoginService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteUserLogin(int UserLoginId, int UserId,int BranchId)
        {
            try
            {

                Sys_UserLogin? UserLogin = _TaamerProContext.Sys_UserLogin.Where(s => s.UserLoginId == UserLoginId).FirstOrDefault();
                if(UserLogin!=null)
                {
                    UserLogin.IsDeleted = true;
                    UserLogin.DeleteDate = DateTime.Now;
                    UserLogin.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف مستخدم عميل أو مقاول رقم " + UserLoginId;
                    _SystemAction.SaveAction("DeleteUserLogin", "Sys_UserLoginService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف مستخدم عميل أو مقاول رقم " + UserLoginId; ;
                _SystemAction.SaveAction("DeleteUserLogin", "Sys_UserLoginService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public GeneralMessage ConfirmUserLogin(List<int> UserLoginIds, Int16 Status, int UserId, int BranchId)
        {
            try
            {

                var UserLoginList = _TaamerProContext.Sys_UserLogin.Where(s => UserLoginIds.Contains(s.UserLoginId)).ToList();
                foreach (var UserLogin in UserLoginList)
                {
                    UserLogin.Status = Status;
                    UserLogin.UpdateDate = DateTime.Now;
                    UserLogin.UpdateUser = UserId;
                }
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تغيير حالة مستخدم عميل أو مقاول  ";
                _SystemAction.SaveAction("DeleteUserLogin", "Sys_UserLoginService", 3, " تغيير حالة مستخدم عميل أو مقاول  ", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = " تغيير حالة مستخدم عميل أو مقاول  " };

            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في تغيير حالة مستخدم عميل أو مقاول  " ; ;
                _SystemAction.SaveAction("DeleteUserLogin", "Sys_UserLoginService", 3, " فشل في تغيير حالة مستخدم عميل أو مقاول  ", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = " فشل في تغيير حالة مستخدم عميل أو مقاول  " };
            }
        }


    }
}

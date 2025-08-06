using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using TaamerProject.Models;
using TaamerProject.Models.Common.FIlterModels;
using TaamerProject.Models.Common;
using TaamerProject.Service.Interfaces;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Service.Generic;
using TaamerProject.Service.IGeneric;
using Twilio.Base;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class FilesAuthService: IFilesAuthService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        private readonly IFilesAuthRepository _FilesAuthRepository;

        public FilesAuthService(IFilesAuthRepository FilesAuthRepository
            , TaamerProjectContext dataContext
            , ISystemAction systemAction)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _FilesAuthRepository = FilesAuthRepository;
        }
        public Task<IEnumerable<FilesAuthVM>> GetAllFilesAuth()
        {
            var FilesAuth = _FilesAuthRepository.GetAllFilesAuth();
            return FilesAuth;
        }
        public Task<FilesAuthVM> GetFilesAuthByTypeId(int TypeId)
        {
            var FilesAuth = _FilesAuthRepository.GetFilesAuthByTypeId(TypeId);
            return FilesAuth;
        }


        public GeneralMessage SaveFileAuth(FilesAuth FileAuth, int UserId, int BranchId)
        {
            try
            {

                if (FileAuth.FilesAuthId == 0)
                {
                    FileAuth.AddUser = UserId;
                    FileAuth.AddDate = DateTime.Now;
                    FileAuth.Code = "";
                    _TaamerProContext.FilesAuth.Add(FileAuth);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = Resources.addnewitem;
                    _SystemAction.SaveAction("SaveFileAuth", "FilesAuthService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully,ReturnedParm= FileAuth.FilesAuthId };

                }
                else
                {
                    //var FileAuthrUpdated = _FilesAuthRepository.GetById(FileAuth.FileAuthId);
                    var FileAuthrUpdated = _TaamerProContext.FilesAuth.Where(s => s.FilesAuthId == FileAuth.FilesAuthId).FirstOrDefault();

                    if (FileAuthrUpdated != null)
                    {
                        FileAuthrUpdated.AppKey = FileAuth.AppKey;
                        FileAuthrUpdated.AppSecret = FileAuth.AppSecret;
                        FileAuthrUpdated.RedirectUri = FileAuth.RedirectUri;
                        //FileAuthrUpdated.Code = FileAuth.Code;
                        //FileAuthrUpdated.AccessToken = FileAuth.AccessToken;
                        //FileAuthrUpdated.RefreshToken = FileAuth.RefreshToken;
                        FileAuthrUpdated.FolderName = FileAuth.FolderName;
                        FileAuthrUpdated.Sendactive = FileAuth.Sendactive??false;
                        //FileAuthrUpdated.ExpiresIn = FileAuth.ExpiresIn;
                        //FileAuthrUpdated.CreationDate = FileAuth.CreationDate;
                        FileAuthrUpdated.TypeId = FileAuth.TypeId;
                        FileAuthrUpdated.BranchId = FileAuth.BranchId;
                        FileAuthrUpdated.UpdateUser = UserId;
                        FileAuthrUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل اعدادت الملفات رقم " + FileAuth.FilesAuthId;
                    _SystemAction.SaveAction("SaveFileAuth", "FilesAuthService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedParm = FileAuth.FilesAuthId };
                }

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ اعدادت الملفات";
                _SystemAction.SaveAction("SaveFileAuth", "FilesAuthService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed, ReturnedParm = FileAuth.FilesAuthId };
            }
        }
        public GeneralMessage UpdateTokenData(FilesAuth FileAuth,int TypeId)
        {
            try
            {

                var FileAuthrUpdated = _TaamerProContext.FilesAuth.Where(s => s.TypeId == TypeId).FirstOrDefault();

                if (FileAuthrUpdated != null)
                {
                    FileAuthrUpdated.AccessToken = FileAuth.AccessToken;
                    if(!(FileAuth.RefreshToken == null || FileAuth.RefreshToken == ""))
                    {
                        FileAuthrUpdated.RefreshToken = FileAuth.RefreshToken;
                    }
                    FileAuthrUpdated.ExpiresIn = FileAuth.ExpiresIn;
                    FileAuthrUpdated.CreationDate = DateTime.Now;
                }
                _TaamerProContext.SaveChanges();

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage DeleteFileAuth(int FilesAuthId, int UserId, int BranchId)
        {
            try
            {
                FilesAuth? FileAuth = _TaamerProContext.FilesAuth.Where(s => s.FilesAuthId == FilesAuthId).FirstOrDefault();
                if (FileAuth != null)
                {
                    FileAuth.IsDeleted = true;
                    FileAuth.DeleteDate = DateTime.Now;
                    FileAuth.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف اعدادت الملفات رقم " + FilesAuthId;
                    _SystemAction.SaveAction("DeleteFileAuth", "FilesAuthService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف اعدادت الملفات رقم " + FilesAuthId; ;
                _SystemAction.SaveAction("DeleteFileAuth", "FilesAuthService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
    }
}

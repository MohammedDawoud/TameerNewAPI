using System.Globalization;
using System.Net;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class ArchiveFilesService : IArchiveFilesService
    {

        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IArchiveFilesRepository _ArchiveFilesRepository;

        public ArchiveFilesService(TaamerProjectContext dataContext , ISystemAction systemAction, IArchiveFilesRepository archiveFilesRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _ArchiveFilesRepository = archiveFilesRepository;

        }
        public async Task<IEnumerable<ArchiveFilesVM>> GetAllArchiveFiles(string SearchText, int BranchId)
        {
            var ArchiveFiles = await _ArchiveFilesRepository.GetAllArchiveFiles(SearchText, BranchId);
            return ArchiveFiles;
        }
        public GeneralMessage SaveArchiveFiles(ArchiveFiles ArchiveFiles, int UserId, int BranchId)
        {
            try
            {
                if (ArchiveFiles.ArchiveFileId == 0)
                {
                    ArchiveFiles.AddUser = UserId;
                    ArchiveFiles.BranchId = BranchId;
                    ArchiveFiles.AddDate = DateTime.Now;
                    _TaamerProContext.ArchiveFiles.Add(ArchiveFiles);
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة ملف ارشيف جديد";
                    _SystemAction.SaveAction("SaveArchiveFiles", "ArchiveFilesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    var ArchiveFilesUpdated = _TaamerProContext.ArchiveFiles.Where(x=>x.ArchiveFileId==ArchiveFiles.ArchiveFileId).FirstOrDefault();
                    if (ArchiveFilesUpdated != null)
                    {
                        ArchiveFilesUpdated.NameAr = ArchiveFiles.NameAr;
                        ArchiveFilesUpdated.NameEn = ArchiveFiles.NameEn;
                        ArchiveFilesUpdated.UpdateUser = UserId;
                        ArchiveFilesUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل  ملف ارشيف رقم " + ArchiveFiles.ArchiveFileId;
                    _SystemAction.SaveAction("SaveArchiveFiles", "ArchiveFilesService", 2,Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حفظ ملف الارشيف ";
                _SystemAction.SaveAction("SaveArchiveFiles", "ArchiveFilesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

            }
        }
        public GeneralMessage DeleteArchiveFiles(int ArchiveFileId, int UserId, int BranchId)
        {
            try
            {
                ArchiveFiles ArchiveFiles = _TaamerProContext.ArchiveFiles.Where(x => x.ArchiveFileId == ArchiveFileId).FirstOrDefault();
                ArchiveFiles.IsDeleted = true;
                ArchiveFiles.DeleteDate = DateTime.Now;
                ArchiveFiles.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف  ملف ارشيف رقم " + ArchiveFileId;
                _SystemAction.SaveAction("DeleteArchiveFiles", "ArchiveFilesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف  ملف ارشيف رقم " + ArchiveFileId; ;
                _SystemAction.SaveAction("DeleteArchiveFiles", "ArchiveFilesService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };


            }
        }
        public IEnumerable<object> FillArchiveFilesSelect(int BranchId)
        {
            return _ArchiveFilesRepository.GetAllArchiveFiles("", BranchId).Result.Select(s => new
            {
                Id = s.ArchiveFileId,
                Name = s.NameAr
            });
        }
    }
}

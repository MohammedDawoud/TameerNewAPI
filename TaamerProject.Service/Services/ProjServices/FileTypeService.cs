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
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class FileTypeService : IFileTypeService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IFileTypeRepository _FileTypeRepository;



        public FileTypeService(TaamerProjectContext dataContext, ISystemAction systemAction, IFileTypeRepository fileTypeRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _FileTypeRepository = fileTypeRepository;
        }
        public async Task<IEnumerable<FileTypeVM>> GetAllFileTypes(string SearchText, int BranchId)
        {
            var FileTypes =await _FileTypeRepository.GetAllFileTypes(SearchText, BranchId);
            return FileTypes;
        }
        public GeneralMessage SaveFileType(FileType fileType, int UserId, int BranchId)
        {
            try
            {
                if (fileType.FileTypeId == 0)
                {
                    fileType.BranchId = BranchId;
                    fileType.AddUser = UserId;
                    fileType.AddDate = DateTime.Now;
                    _TaamerProContext.FileType.Add(fileType);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة نوع ملف جديد";
                    _SystemAction.SaveAction("SaveFileType", "FileTypeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var FileTypeUpdated = _FileTypeRepository.GetById(fileType.FileTypeId);
                    if (FileTypeUpdated != null)
                    {
                        FileTypeUpdated.NameAr = fileType.NameAr;
                        FileTypeUpdated.NameEn = fileType.NameEn;
                        FileTypeUpdated.UpdateUser = UserId;
                        FileTypeUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل نوع ملف رقم " + fileType.FileTypeId;
                    _SystemAction.SaveAction("SaveFileType", "FileTypeService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };
                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ نوع ملف";
                _SystemAction.SaveAction("SaveFileType", "FileTypeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteFileType(int FileTypeId, int UserId, int BranchId)
        {
            try
            {
                FileType fileType = _FileTypeRepository.GetById(FileTypeId);
                fileType.IsDeleted = true;
                fileType.DeleteDate = DateTime.Now;
                fileType.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف نوع ملف رقم " + FileTypeId;
                _SystemAction.SaveAction("DeleteFileType", "FileTypeService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف نوع ملف رقم " + FileTypeId; ;
                _SystemAction.SaveAction("DeleteFileType", "FileTypeService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }

        public IEnumerable<object> FillFileTypeSelect(int BranchId)
        {
            return _FileTypeRepository.GetAllFileTypes("", BranchId).Result.Select(s => new
            {
                Id = s.FileTypeId,
                Name = s.NameAr,
                NameEn = s.NameEn
            });
        }
    }
}

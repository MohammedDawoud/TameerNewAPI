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
using Microsoft.EntityFrameworkCore;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class TrailingFilesService :  ITrailingFilesService
    {
        private readonly ITrailingFilesRepository _TrailingFilesRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public TrailingFilesService(TaamerProjectContext dataContext, ITrailingFilesRepository trailingFilesRepository, 
            ISystemAction systemAction)
        {
            _TrailingFilesRepository = trailingFilesRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        //public IEnumerable<TrailingFilesVM> GetAllTrailingFiles()
        //{
        //    var TrailingFiles = _TrailingFilesRepository.GetAllTrailingFiles().ToList();
        //    return TrailingFiles;
        //}
        public  GeneralMessage SaveTrailingFiles(TrailingFiles trailingFiles, int UserId, int BranchId)
        {
            try
            {
              
                if (trailingFiles.FileId == 0)
                {
                    trailingFiles.AddUser = UserId;
                    trailingFiles.AddDate = DateTime.Now;
                    _TaamerProContext.TrailingFiles.Add(trailingFiles);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة متابعة الملفات جديدة";
                    _SystemAction.SaveAction("SaveTrailingFiles", "TrailingFilesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    //var TrailingFilesUpdated = _TrailingFilesRepository.GetById(trailingFiles.FileId);
                    TrailingFiles? TrailingFilesUpdated =  _TaamerProContext.TrailingFiles.Where(s => s.FileId == trailingFiles.FileId).FirstOrDefault();

                    if (TrailingFilesUpdated != null)
                    {
                        TrailingFilesUpdated.FileName = trailingFiles.FileName;
                        TrailingFilesUpdated.FileUrl = trailingFiles.FileUrl;
                        TrailingFilesUpdated.TypeId = trailingFiles.TypeId;
                        TrailingFilesUpdated.ProjectId = trailingFiles.ProjectId;
                        TrailingFilesUpdated.TrailingId = trailingFiles.TrailingId;
                        TrailingFilesUpdated.Notes = trailingFiles.Notes;
                        TrailingFilesUpdated.BranchId = trailingFiles.BranchId;
                        TrailingFilesUpdated.UpdateUser = UserId;
                        TrailingFilesUpdated.UpdateDate = DateTime.Now;

                    }
                    _TaamerProContext.SaveChanges();  
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل متابعة ملفات رقم " + trailingFiles.FileId;
                    _SystemAction.SaveAction("SaveTrailingFiles", "TrailingFilesService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception)
            { //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ  متابعة الملفات";
                _SystemAction.SaveAction("SaveTrailingFiles", "TrailingFilesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public  GeneralMessage DeleteTrailingFiles( int FileId, int UserId, int BranchId)
        {
            try
            {
                //TrailingFiles trailingFiles = _TrailingFilesRepository.GetById(FileId);
                TrailingFiles? trailingFiles =  _TaamerProContext.TrailingFiles.Where(s => s.FileId == FileId).FirstOrDefault();
                if (trailingFiles != null)
                {
                    trailingFiles.IsDeleted = true;
                    trailingFiles.DeleteDate = DateTime.Now;
                    trailingFiles.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف متابعة ملفات رقم " + FileId;
                    _SystemAction.SaveAction("DeleteTrailingFiles", "TrailingFilesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف متابعة ملفات رقم " + FileId; ;
                _SystemAction.SaveAction("DeleteClause", "Acc_ClausesService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }

    
    }
}

          
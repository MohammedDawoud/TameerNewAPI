using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class Pro_filesReasonsService : IPro_filesReasonsService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IPro_filesReasonsRepository _Pro_filesReasonsRepository;
        public Pro_filesReasonsService(IPro_filesReasonsRepository pro_filesReasonsRepository
            , TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _TaamerProContext = dataContext; _SystemAction = systemAction;
            _Pro_filesReasonsRepository = pro_filesReasonsRepository;
        }

        public Task<IEnumerable<Pro_filesReasonsVM>> GetAllfilesReasons()
        {
            var Reasons = _Pro_filesReasonsRepository.GetAllfilesReasons();
            return Reasons;
        }
        public GeneralMessage SaveReason(Pro_filesReasons Reason, int UserId, int BranchId)
        {
            try
            {

                if (Reason.ReasonsId == 0)
                {
                    Reason.AddUser = UserId;
                    Reason.AddDate = DateTime.Now;
                    _TaamerProContext.Pro_filesReasons.Add(Reason);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة سبب جديد";
                    _SystemAction.SaveAction("SaveReason", "Pro_filesReasonsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var ReasonsUpdated = _TaamerProContext.Pro_filesReasons.Where(s => s.ReasonsId == Reason.ReasonsId).FirstOrDefault();

                    if (ReasonsUpdated != null)
                    {
                        ReasonsUpdated.NameAr = Reason.NameAr;
                        ReasonsUpdated.NameEn = Reason.NameEn;
                        ReasonsUpdated.UpdateUser = UserId;
                        ReasonsUpdated.UpdateDate = DateTime.Now;

                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل سبب رقم " + Reason.ReasonsId;
                    _SystemAction.SaveAction("SaveReason", "Pro_filesReasonsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ السبب";
                _SystemAction.SaveAction("SaveReason", "Pro_filesReasonsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteReason(int ReasonId, int UserId, int BranchId)
        {
            try
            {
                Pro_filesReasons? Reason = _TaamerProContext.Pro_filesReasons.Where(s => s.ReasonsId == ReasonId).FirstOrDefault();
                if (Reason != null)
                {
                    Reason.IsDeleted = true;
                    Reason.DeleteDate = DateTime.Now;
                    Reason.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف سبب رقم " + ReasonId;
                    _SystemAction.SaveAction("DeleteReason", "Pro_filesReasonsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف سبب رقم " + ReasonId; ;
                _SystemAction.SaveAction("DeleteReason", "Pro_filesReasonsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
    }
}

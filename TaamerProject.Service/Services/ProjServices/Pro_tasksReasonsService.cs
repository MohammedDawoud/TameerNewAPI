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
    public class Pro_tasksReasonsService : IPro_tasksReasonsService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IPro_tasksReasonsRepository _Pro_tasksReasonsRepository;
        public Pro_tasksReasonsService(IPro_tasksReasonsRepository pro_tasksReasonsRepository
            , TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _TaamerProContext = dataContext; _SystemAction = systemAction;
            _Pro_tasksReasonsRepository = pro_tasksReasonsRepository;
        }

        public Task<IEnumerable<Pro_tasksReasonsVM>> GetAlltasksReasons()
        {
            var Reasons = _Pro_tasksReasonsRepository.GetAlltasksReasons();
            return Reasons;
        }
        public GeneralMessage SaveReason(Pro_tasksReasons Reason, int UserId, int BranchId)
        {
            try
            {

                if (Reason.ReasonsId == 0)
                {
                    Reason.AddUser = UserId;
                    Reason.AddDate = DateTime.Now;
                    _TaamerProContext.Pro_tasksReasons.Add(Reason);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة سبب جديد";
                    _SystemAction.SaveAction("SaveReason", "Pro_tasksReasonsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var ReasonsUpdated = _TaamerProContext.Pro_tasksReasons.Where(s => s.ReasonsId == Reason.ReasonsId).FirstOrDefault();

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
                    _SystemAction.SaveAction("SaveReason", "Pro_tasksReasonsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ السبب";
                _SystemAction.SaveAction("SaveReason", "Pro_tasksReasonsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteReason(int ReasonId, int UserId, int BranchId)
        {
            try
            {
                Pro_tasksReasons? Reason = _TaamerProContext.Pro_tasksReasons.Where(s => s.ReasonsId == ReasonId).FirstOrDefault();
                if (Reason != null)
                {
                    Reason.IsDeleted = true;
                    Reason.DeleteDate = DateTime.Now;
                    Reason.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف سبب رقم " + ReasonId;
                    _SystemAction.SaveAction("DeleteReason", "Pro_tasksReasonsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف سبب رقم " + ReasonId; ;
                _SystemAction.SaveAction("DeleteReason", "Pro_tasksReasonsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
    }
}

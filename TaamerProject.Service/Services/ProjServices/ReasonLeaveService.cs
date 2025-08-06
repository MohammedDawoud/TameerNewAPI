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
using TaamerProject.Repository.Repositories;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class ReasonLeaveService :   IReasonLeaveService
    {
        private readonly IReasonLeaveRepository _reasonLeaveRepository;
 
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public ReasonLeaveService(IReasonLeaveRepository reasonLeaveRepository,
            TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _reasonLeaveRepository = reasonLeaveRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;

        }

        public Task<IEnumerable<ReasonLeaveVM>> GetAllreasons(string SearchText)
        {
            var reasons = _reasonLeaveRepository.GetAllreasons(SearchText);
            return reasons;
        }

        public Task<ReasonLeaveVM> Getreasonbyid(int ReasonId)
        {
            var reasons = _reasonLeaveRepository.Getreasonbyid(ReasonId);
            return reasons;
        }
        public GeneralMessage SaveReason(ReasonLeave reson, int UserId, int BranchId)
        {
            try
            {
     
                if (reson.ReasonId == 0)
                {
                    reson.AddUser = UserId;
                    reson.AddDate = DateTime.Now;
                    _TaamerProContext.ReasonLeave.Add(reson);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة سبب جديد";
                    _SystemAction.SaveAction("SaveJob", "JobService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    //var reasonUpdated = _reasonLeaveRepository.GetById(reson.ReasonId);
                    ReasonLeave? reasonUpdated = _TaamerProContext.ReasonLeave.Where(s => s.ReasonId == reson.ReasonId).FirstOrDefault();

                    if (reasonUpdated != null)
                    {
                        //JobUpdated.JobCode = job.JobCode;
                        reasonUpdated.ReasonTxt = reson.ReasonTxt;
                        reasonUpdated.DesecionTxt = reson.DesecionTxt;
              
                        reasonUpdated.UpdateUser = UserId;
                        reasonUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل سبب رقم " + reson.ReasonId;
                    _SystemAction.SaveAction("SaveJob", "JobService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_EditedSuccessfully };

                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ السبب";
                _SystemAction.SaveAction("SaveJob", "JobService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteReason(int ReasonId, int UserId, int BranchId)
        {
            try
            {
                //ReasonLeave reson = _reasonLeaveRepository.GetById(ReasonId);
                ReasonLeave? reson = _TaamerProContext.ReasonLeave.Where(s => s.ReasonId == ReasonId).FirstOrDefault();
                if (reson != null)
                {
                    if (ReasonId == 1 || ReasonId == 2 || ReasonId == 3 || ReasonId == 4)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = Resources.General_SavedFailed;
                        _SystemAction.SaveAction("DeleteReason", "ResonLeaveService", 1, Resources.CanNotDeleteMainReason, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.CanNotDeleteMainReason };
                    }
                    reson.IsDeleted = true;
                    reson.DeleteDate = DateTime.Now;
                    reson.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف سبب رقم " + ReasonId;
                    _SystemAction.SaveAction("DeleteClause", "Acc_ClausesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ السبب";
                _SystemAction.SaveAction("SaveJob", "JobService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public Task<IEnumerable<ReasonLeaveVM>> FillReasonSelect(string SearchText = "")
        {
            var resonLeave = _reasonLeaveRepository.GetAllreasons(SearchText);
            return resonLeave;
        }
        

    }
}

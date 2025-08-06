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
    public class AppraisalService : IAppraisalService
    {

        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IAppraisalRepository _AppraisalRepository;

        public AppraisalService(IAllowanceTypeRepository allowanceRepository
            , TaamerProjectContext dataContext
            , ISystemAction systemAction, IAppraisalRepository appraisalRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _AppraisalRepository = appraisalRepository;

        }
        public async Task<IEnumerable<AppraisalVM>> GetAllAppraisal()
        {
            var Appraisal =await _AppraisalRepository.GetAllAppraisal();
            return Appraisal;
        }

        public async Task<IEnumerable<AppraisalVM>> SearchAppraisal(AppraisalVM AppraisalySearch, string lang, int BranchId)
        {
            var Custody =  _AppraisalRepository.SearchAppraisal(AppraisalySearch, lang, BranchId).Result.ToList();
            return Custody;
        }
        public GeneralMessage SaveAppraisal(Appraisal appraisal, int UserId, int BranchId)
        {
            try
            {
                if (appraisal.AppraisalId == 0)
                {
                    appraisal.IsDeleted = false;
                    appraisal.AddUser = UserId;
                    appraisal.AddDate = DateTime.Now;
                    _TaamerProContext.Appraisal.Add(appraisal);
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة توصية جديد";
                    _SystemAction.SaveAction("SaveAppraisal", "AppraisalService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    var AppraisalUpdated = _TaamerProContext.Appraisal.Where(x => x.AppraisalId == appraisal.AppraisalId).FirstOrDefault();
                    if (AppraisalUpdated != null)
                    {
                        AppraisalUpdated.EmpId = appraisal.EmpId;
                        AppraisalUpdated.Degree = appraisal.Degree;
                        AppraisalUpdated.ManagerId = appraisal.ManagerId;
                        AppraisalUpdated.MonthDate = appraisal.MonthDate;
                        AppraisalUpdated.Month = appraisal.Month;
                        AppraisalUpdated.Year = appraisal.Year;
                        AppraisalUpdated.UpdateUser = UserId;
                        AppraisalUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل  توصية رقم " + appraisal.AppraisalId;
                    _SystemAction.SaveAction("SaveAppraisal", "AppraisalService", 2,Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };



                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حفظ التوصية ";
                _SystemAction.SaveAction("SaveAppraisal", "AppraisalService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteAppraisal(int AppraisalId, int UserId, int BranchId)
        {
            try
            {
                Appraisal apprai = _TaamerProContext.Appraisal.Where(x=>x.AppraisalId==AppraisalId).FirstOrDefault();
                apprai.IsDeleted = true;
                apprai.DeleteDate = DateTime.Now;
                apprai.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف  توصية رقم " + AppraisalId;
                _SystemAction.SaveAction("DeleteAppraisal", "AppraisalService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف  توصية رقم " + AppraisalId; ;
                _SystemAction.SaveAction("DeleteAppraisal", "AppraisalService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

            }
        }
    }
}

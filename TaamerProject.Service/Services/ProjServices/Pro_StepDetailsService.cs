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
using System.Globalization;

namespace TaamerProject.Service.Services
{
    public class Pro_StepDetailsService: IPro_StepDetailsService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        private readonly IPro_StepDetailsRepository _Pro_StepDetailsRepository;

        public Pro_StepDetailsService(IPro_StepDetailsRepository Pro_StepDetailsRepository, TaamerProjectContext dataContext
            , ISystemAction systemAction)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _Pro_StepDetailsRepository = Pro_StepDetailsRepository;
        }
        public Task<IEnumerable<Pro_StepDetailsVM>> GetAllStepDetails()
        {
            var StepDetails = _Pro_StepDetailsRepository.GetAllStepDetails();
            return StepDetails;
        }


        public GeneralMessage SaveStepDetail(Pro_StepDetails StepDetail, int UserId, int BranchId)
        {
            try
            {
                if (StepDetail.StepDetailId == 0)
                {
                    StepDetail.AddUser = UserId;
                    StepDetail.AddDate = DateTime.Now;
                    _TaamerProContext.Pro_StepDetails.Add(StepDetail);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = Resources.addnewitem;
                    _SystemAction.SaveAction("SaveStepDetail", "Pro_StepDetailsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    var StepDetailrUpdated = _TaamerProContext.Pro_StepDetails.Where(s => s.StepDetailId == StepDetail.StepDetailId).FirstOrDefault();

                    if (StepDetailrUpdated != null)
                    {
                        StepDetailrUpdated.NameAr = StepDetail.NameAr??"";
                        StepDetailrUpdated.NameEn = StepDetail.NameEn??"";
                        StepDetailrUpdated.StepId = StepDetail.StepId;
                        StepDetailrUpdated.StepName = StepDetail.StepName??"";
                        StepDetailrUpdated.UpdateUser = UserId;
                        StepDetailrUpdated.UpdateDate = DateTime.Now;

                    }

                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل خطوة في مرحلة المشروع رقم " + StepDetail.StepDetailId;
                    _SystemAction.SaveAction("SaveStepDetail", "Pro_StepDetailsService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ خطوة في مرحلة المشروع";
                _SystemAction.SaveAction("SaveStepDetail", "Pro_StepDetailsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage DeleteStepDetail(int StepDetailid, int UserId, int BranchId)
        {
            try
            {
                Pro_StepDetails? StepDetail = _TaamerProContext.Pro_StepDetails.Where(s => s.StepDetailId == StepDetailid).FirstOrDefault();
                if (StepDetail != null)
                {
                    StepDetail.IsDeleted = true;
                    StepDetail.DeleteDate = DateTime.Now;
                    StepDetail.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف خطوة في مرحلة المشروع رقم " + StepDetailid;
                    _SystemAction.SaveAction("DeleteStepDetail", "Pro_StepDetailsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف خطوة في مرحلة المشروع رقم " + StepDetailid; ;
                _SystemAction.SaveAction("DeleteStepDetail", "Pro_StepDetailsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
    }
}

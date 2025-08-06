using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Models.ViewModels;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services.AccServices
{
    public class CommercialActivityService : ICommercialActivityService
    {
        private readonly ICommercialActivityRepository _activityRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public CommercialActivityService(TaamerProjectContext dataContext, ICommercialActivityRepository activityRepository,
            ISystemAction systemAction)
        {
            _activityRepository = activityRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }

        public Task<IEnumerable<CommercialActivityVM>> GetCommercialActivities(string SearchText,int Type)
        {
            return _activityRepository.GetCommercialActivities(SearchText, Type);
        }
        public GeneralMessage DeleteCommercialActivity(int Id, int UserId, int BranchId)
        {
            try
            {
                CommercialActivity? commercialActivity = _TaamerProContext.CommercialActivities.Where(s => s.CommercialActivityId == Id).FirstOrDefault();
                if (commercialActivity != null)
                {
                    commercialActivity.IsDeleted = true;
                    commercialActivity.DeleteDate = DateTime.Now;
                    commercialActivity.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف نوع النشاط رقم " + Id;
                    _SystemAction.SaveAction("DeleteCommercialActivity", "CommercialActivityService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف النشاط رقم " + Id ;
                _SystemAction.SaveAction("DeleteCommercialActivity", "CommercialActivityService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }

        }

        public GeneralMessage SaveCommercialActivity(CommercialActivity commercialActivity, int UserId, int BranchId)
        {
            try
            {

                if (commercialActivity.CommercialActivityId == 0)
                {
                    commercialActivity.AddUser = UserId;
                    commercialActivity.AddDate = DateTime.Now;
                    _TaamerProContext.CommercialActivities.Add(commercialActivity);
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة نشاط جديد";
                    _SystemAction.SaveAction("SaveCommercialActivity", "CommercialActivityService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                  CommercialActivity? commercialUpdate = _TaamerProContext.CommercialActivities.Where(s => s.CommercialActivityId == commercialActivity.CommercialActivityId).FirstOrDefault();
                    if (commercialUpdate != null)
                    {
                        commercialUpdate.NameAr = commercialActivity.NameAr;
                        commercialUpdate.NameEn = commercialActivity.NameEn;
                        commercialUpdate.UpdateUser = UserId;
                        commercialUpdate.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل نشاط رقم " + commercialActivity.CommercialActivityId;
                    _SystemAction.SaveAction("SaveCommercialActivity", "CommercialActivityService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ نشاط";
                _SystemAction.SaveAction("SaveCommercialActivity", "CommercialActivityService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }

        }


    }
}

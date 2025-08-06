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
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class Acc_TotalSpacesRangeService : IAcc_TotalSpacesRangeService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IAcc_TotalSpacesRangeRepository _acc_TotalSpacesRange;
        public Acc_TotalSpacesRangeService(IAcc_TotalSpacesRangeRepository acc_TotalSpacesRange
            , TaamerProjectContext dataContext
            , ISystemAction systemAction)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _acc_TotalSpacesRange = acc_TotalSpacesRange;
        }
        public  Task<IEnumerable<Acc_TotalSpacesRangeVM>> GetAllTotalSpacesRange(string SearchText)
        {
            var TotalSpacesRange =  _acc_TotalSpacesRange.GetAllTotalSpacesRange(SearchText);
            return TotalSpacesRange;
        }
        public GeneralMessage SaveTotalSpacesRange(Acc_TotalSpacesRange TotalSpacesRange, int UserId, int BranchId)
        {
            try
            {

                if (TotalSpacesRange.TotalSpacesRangeId == 0)
                {


                    TotalSpacesRange.AddUser = UserId;
                    TotalSpacesRange.AddDate = DateTime.Now;
                    _TaamerProContext.Acc_TotalSpacesRange.Add(TotalSpacesRange);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة مدي المساحة جديد";
                    _SystemAction.SaveAction("SaveTotalSpacesRange", "Acc_TotalSpacesRangeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    var TotalSpacesRangeUpdated = _TaamerProContext.Acc_TotalSpacesRange.Where(x=>x.TotalSpacesRangeId==TotalSpacesRange.TotalSpacesRangeId).FirstOrDefault();
                    if (TotalSpacesRangeUpdated != null)
                    {

                        TotalSpacesRangeUpdated.TotalSpacesRengeName = TotalSpacesRange.TotalSpacesRengeName;
                        TotalSpacesRangeUpdated.RangeValue = TotalSpacesRange.RangeValue;
                        TotalSpacesRangeUpdated.UpdateUser = UserId;
                        TotalSpacesRangeUpdated.UpdateDate = DateTime.Now;

                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل مدي المساحة رقم " + TotalSpacesRange.TotalSpacesRangeId;
                    _SystemAction.SaveAction("SaveTotalSpacesRange", "Acc_TotalSpacesRangeService", 2,Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully};

                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ المدي المساحي";
                _SystemAction.SaveAction("SaveTotalSpacesRange", "Acc_TotalSpacesRangeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

            }
        }
        public GeneralMessage DeleteTotalSpacesRange(int TotalSpacesRangeId, int UserId, int BranchId)
        {
            try
            {


                if (TotalSpacesRangeId == 1 || TotalSpacesRangeId == 2 || TotalSpacesRangeId == 3)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " فشل في حذف مدي المساحة رقم " + TotalSpacesRangeId; ;
                    _SystemAction.SaveAction("DeleteTotalSpacesRange", "Acc_TotalSpacesRangeService", 3, Resources.Cannot_delete_this_space_range, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Cannot_delete_this_space_range };

                }


                Acc_TotalSpacesRange TotalSpacesRange = _TaamerProContext.Acc_TotalSpacesRange.Where(x=>x.TotalSpacesRangeId==TotalSpacesRangeId).FirstOrDefault();
                TotalSpacesRange.IsDeleted = true;
                TotalSpacesRange.DeleteDate = DateTime.Now;
                TotalSpacesRange.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف مدي المساحة رقم " + TotalSpacesRangeId;
                _SystemAction.SaveAction("DeleteTotalSpacesRange", "Acc_TotalSpacesRangeService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };


            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف مدي المساحة رقم " + TotalSpacesRangeId; ;
                _SystemAction.SaveAction("DeleteTotalSpacesRange", "Acc_TotalSpacesRangeService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };

            }
        }


    }
}

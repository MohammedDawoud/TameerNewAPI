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
    public class OfficalHolidayService : IOfficalHolidayService
    {


        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IOfficalHolidayRepository _OfficalHolidayRepository;



        public OfficalHolidayService(TaamerProjectContext dataContext, ISystemAction systemAction, IOfficalHolidayRepository officalHolidayRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _OfficalHolidayRepository = officalHolidayRepository;
        }
        public async Task<IEnumerable<OfficalHolidayVM>> GetAllOfficalHoliday()
        {
            var GetAll =await _OfficalHolidayRepository.GetAllOfficalHoliday();
            return GetAll;
        }

        public async Task<IEnumerable<OfficalHolidayVM>> GetAllOfficalHolidaySearch(OfficalHolidayVM Search, int BranchId)
        {
            return await _OfficalHolidayRepository.GetAllOfficalHolidaySearch(Search);
        }


        public GeneralMessage SaveOfficalHoliday(OfficalHoliday Holiday, int UserId, int BranchId)
        {
            try
            {
                if (Holiday.Id == 0)
                {
                    Holiday.AddUser = UserId;
                    Holiday.AddDate = DateTime.Now;
                    _TaamerProContext.OfficalHoliday.Add(Holiday);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة عطلة رسمية";
                   _SystemAction.SaveAction("SaveOfficalHoliday", "OfficalHolidayService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    var CarMovementsUpdated = _TaamerProContext.OfficalHoliday.Where(x=>x.Id==Holiday.Id).FirstOrDefault();
                    if (CarMovementsUpdated.Id == 1 || CarMovementsUpdated.Id == 2)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote1 = "فشل في حفظ العطلة الرسمية";
                        _SystemAction.SaveAction("SaveOfficalHoliday", "OfficalHolidayService", 1, Resources.General_SavedFailed, "", "", ActionDate1, UserId, BranchId, ActionNote1, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.leaveCannotModified };

                    }
                    if (CarMovementsUpdated != null)
                    {
                        CarMovementsUpdated.FromDate = Holiday.FromDate;
                        CarMovementsUpdated.ToDate = Holiday.ToDate;
                        CarMovementsUpdated.Description = Holiday.Description;

                        CarMovementsUpdated.UpdateUser = UserId;
                        CarMovementsUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "تعديل عطلة رسمية رقم " + Holiday.Id;
                    _SystemAction.SaveAction("SaveOfficalHoliday", "OfficalHolidayService", 1, "تم التعديل", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully};

                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ العطلة الرسمية";
                _SystemAction.SaveAction("SaveOfficalHoliday", "OfficalHolidayService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteOfficalHoliday(int Id, int UserId, int BranchId)
        {
            try
            {

                OfficalHoliday OfficalHoliday = _TaamerProContext.OfficalHoliday.Where(x=>x.Id==Id).FirstOrDefault();
                if (OfficalHoliday.Id == 1 || OfficalHoliday.Id == 2)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote1 = " فشل في حذف العطلة رقم " + Id; ;
                    _SystemAction.SaveAction("DeleteOfficalHoliday", "OfficalHolidayService", 3, Resources.General_DeletedFailed, "", "", ActionDate1, UserId, BranchId, ActionNote1, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.leaveCannotDeleted };
                }
                OfficalHoliday.IsDeleted = true;
                OfficalHoliday.DeleteDate = DateTime.Now;
                OfficalHoliday.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف العطلة رقم " + Id;
                _SystemAction.SaveAction("DeleteOfficalHoliday", "OfficalHolidayService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف العطلة رقم " + Id; ;
                _SystemAction.SaveAction("DeleteOfficalHoliday", "OfficalHolidayService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
    }
}


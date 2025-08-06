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
    public class FiscalyearsService : IFiscalyearsService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IFiscalyearsRepository _FiscalyearsRepository;
        private readonly ISystemSettingsRepository _SystemSettingsRepository;


        public FiscalyearsService(TaamerProjectContext dataContext, ISystemAction systemAction, IFiscalyearsRepository fiscalyearsRepository, ISystemSettingsRepository systemSettingsRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _FiscalyearsRepository = fiscalyearsRepository;
            _SystemSettingsRepository = systemSettingsRepository;
        }
        public async Task<IEnumerable<FiscalyearsVM>> GetAllFiscalyears()
        {
            var Fiscalyears =await _FiscalyearsRepository.GetAllFiscalyears();
            return Fiscalyears;
        }

        public int CheckYearExist(int? yearId)
        {
            var Fiscalyears = _FiscalyearsRepository.CheckYearExist(yearId);
            return Fiscalyears;
        }
        public int GetYearID(int FiscalId)
        {
            var Fiscalyears = _FiscalyearsRepository.GetYearID(FiscalId);
            return Fiscalyears;
        }

        public GeneralMessage SaveFiscalyears(FiscalYears fiscalyears, int UserId, int BranchId)
        {
            try
            {
                if (fiscalyears.FiscalId == 0)
                {
                    var Fiscalyears = _FiscalyearsRepository.CheckYearExist(fiscalyears.YearId);
                    if (Fiscalyears > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حفظ سنة مالية";
                        _SystemAction.SaveAction("SaveFiscalyears", "FiscalyearsService", 1, Resources.AlreadyFind, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.AlreadyFind };
                    }
                    fiscalyears.AddUser = UserId;
                    fiscalyears.AddDate = DateTime.Now;
                    fiscalyears.IsActive = false;
                    _TaamerProContext.FiscalYears.Add(fiscalyears);
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة  سنة مالية جديد";
                    _SystemAction.SaveAction("SaveFiscalyears", "FiscalyearsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var FiscalyearsUpdated = _FiscalyearsRepository.GetById(fiscalyears.FiscalId);
                    if (FiscalyearsUpdated != null)
                    {
                        FiscalyearsUpdated.YearId = fiscalyears.YearId;
                        FiscalyearsUpdated.YearName = fiscalyears.YearName;
                        FiscalyearsUpdated.UserId = UserId;
                        FiscalyearsUpdated.UpdateUser = UserId;
                        FiscalyearsUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل  سنة مالية رقم " + fiscalyears.FiscalId;
                    _SystemAction.SaveAction("SaveFiscalyears", "FiscalyearsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ  سنة مالية";
                _SystemAction.SaveAction("SaveClause", "FiscalyearsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }



        }
        public GeneralMessage DeleteFiscalyears(int FiscalId, int UserId, int BranchId)
        {
            try
            {
                FiscalYears Fiscal = _FiscalyearsRepository.GetById(FiscalId);

                var Tran = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.YearId == Fiscal.YearId);
                if (Tran.Count() > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " فشل في حذف  سنة مالية عليها معاملات ";
                    _SystemAction.SaveAction("DeleteFiscalyears", "FiscalyearsService", 3, Resources.General_DeletedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ActionNote2 };
                }
                if (Fiscal.IsActive == true)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " فشل في حذف  السنة المالية الحالية  ";
                    _SystemAction.SaveAction("DeleteFiscalyears", "FiscalyearsService", 3, Resources.General_DeletedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ActionNote2 };
                }
                Fiscal.IsDeleted = true;
                Fiscal.DeleteDate = DateTime.Now;
                Fiscal.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف  سنة مالية رقم " + FiscalId;
                _SystemAction.SaveAction("DeleteFiscalyears", "FiscalyearsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف  سنة مالية رقم " + FiscalId;
                _SystemAction.SaveAction("DeleteFiscalyears", "FiscalyearsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public GeneralMessage ActivateFiscalYear(int FiscalId, int SystemSettingId, int UserId, int BranchId)
        {
            var fisicalyear = "";
            try
            {
                var ActiveYears = _FiscalyearsRepository.GetMatching(s => s.IsDeleted == false && s.IsActive == true);
                foreach (var item in ActiveYears)
                {
                    item.IsActive = false;
                }
                var FiscalyearsUpdated = _FiscalyearsRepository.GetById(FiscalId);
                if (FiscalyearsUpdated != null)
                {
                    FiscalyearsUpdated.IsActive = true;
                    FiscalyearsUpdated.UpdateUser = UserId;
                    FiscalyearsUpdated.UpdateDate = DateTime.Now;
                    fisicalyear = FiscalyearsUpdated.YearName;
                }

                var SystemSettingsUpdated = _TaamerProContext.SystemSettings.Where(x=>x.SettingId==SystemSettingId).FirstOrDefault();
                if (SystemSettingsUpdated != null)
                {
                    SystemSettingsUpdated.FiscalYear = FiscalyearsUpdated.FiscalId;
                    SystemSettingsUpdated.UpdateUser = UserId;
                    SystemSettingsUpdated.UpdateDate = DateTime.Now;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تحديد سنة مالية رقم " + fisicalyear;
                _SystemAction.SaveAction("ActivateFiscalYear", "FiscalyearsService", 2, "تم تحديد السنه الماليه بنجاح", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم تحديد السنه الماليه بنجاح" };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ سنة مالية";
                _SystemAction.SaveAction("ActivateFiscalYear", "FiscalyearsService", 1, "فشل في تحديد السنه الماليه", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في تحديد السنه الماليه" };
            }
        }
        public FiscalyearsVM GetActiveYear()
        {
            var ActivateBranch = _FiscalyearsRepository.GetMatching(s => s.IsDeleted == false && s.IsActive == true).FirstOrDefault();
            if (ActivateBranch != null)
            {
                return new FiscalyearsVM { FiscalId = ActivateBranch.FiscalId, YearId = ActivateBranch.YearId };
            }
            return null;
        }
        public FiscalyearsVM GetActiveYearID(int FiscalId)
        {
            var ActivateBranch = _FiscalyearsRepository.GetMatching(s => s.IsDeleted == false && s.FiscalId == FiscalId).FirstOrDefault();
            if (ActivateBranch != null)
            {
                return new FiscalyearsVM { FiscalId = ActivateBranch.FiscalId, YearId = ActivateBranch.YearId };
            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common.FIlterModels;
using TaamerProject.Models.Common;
using TaamerProject.Service.Interfaces;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Service.Generic;
using TaamerProject.Repository.Repositories;
using System.Net;
using TaamerProject.Service.IGeneric;
using TaamerP.Service.LocalResources;


namespace TaamerProject.Service.Services
{
    public class VoucherSettingsService :  IVoucherSettingsService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IVoucherSettingsRepository _IVoucherSettingsRepository;
        public VoucherSettingsService( TaamerProjectContext dataContext
            , ISystemAction systemAction
            , IVoucherSettingsRepository voucherSettingsRepository)
        {            
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _IVoucherSettingsRepository = voucherSettingsRepository;
        }
        public Task<IEnumerable<VoucherSettingsVM>> GetAllVoucherSettings(int BranchId)
        {
            var settings = _IVoucherSettingsRepository.GetAllVoucherSettings(BranchId);
            return settings;
        }
        public GeneralMessage SaveVoucherSettings(int Type, List<int> AccountIds, int UserId, int BranchId)
        {
            try
            {

                var ExistingSettings = _TaamerProContext.VoucherSettings.Where(s => s.Type == Type && s.BranchId == BranchId);
                if (ExistingSettings != null)
                {
                    _TaamerProContext.VoucherSettings.RemoveRange(ExistingSettings);
                }
                foreach (var id in AccountIds)
                {
                    var setting = new VoucherSettings { Type = Type, AccountId = id, IsDeleted = false,AddUser = UserId, AddDate = DateTime.Now,BranchId = BranchId };
                    _TaamerProContext.VoucherSettings.Add(setting);
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "إضافة تجهيز سندات جديدة";
                _SystemAction.SaveAction("SaveVoucherSettings", "VoucherSettingsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                 
            }
            catch (Exception)
            {//-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ تجهيز السندات";
                _SystemAction.SaveAction("SaveVoucherSettings", "VoucherSettingsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteVoucherSettings(int settingId, int UserId,int BranchId)
        {
            try
            {
                //VoucherSettings settings = _IVoucherSettingsRepository.GetById(settingId);
                VoucherSettings? settings = _TaamerProContext.VoucherSettings.Where(s => s.SettingId == settingId).FirstOrDefault();
                if(settings != null)
                {
                    settings.IsDeleted = true;
                    settings.DeleteDate = DateTime.Now;
                    settings.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف تجهيز سند رقم " + settingId;
                    _SystemAction.SaveAction("DeleteVoucherSettings", "VoucherSettingsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            { 
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف  تجهيز سند رقم " + settingId; ;
                _SystemAction.SaveAction("DeleteVoucherSettings", "VoucherSettingsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                //
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed  };
            }
        }
        public Task<List<int>> GetAccountIdsByType(int Type,int BranchId)
        {
            return _IVoucherSettingsRepository.GetAccountIdsByType(Type, BranchId);
        }

        /***********/
    

    }
}

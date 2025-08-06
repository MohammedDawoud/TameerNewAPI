using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class SMSSettingRepository : ISMSSettingsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public SMSSettingRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<SMSSettingsVM> GetsmsSetting(int BranchId)
        {
            var SMSSettings = _TaamerProContext.SMSSettings.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new SMSSettingsVM
            {
                SettingId = x.SettingId,
                MobileNo = x.MobileNo,
                Password = x.Password,
                SenderName = x.SenderName,
                UserId = x.UserId,
                BranchId = x.BranchId,
                ApiUrl = x.ApiUrl,
                UserName = x.UserName,
                SendCustomerSMS=x.SendCustomerSMS,
                
            }).FirstOrDefault();
            return SMSSettings;
        }
    }
}

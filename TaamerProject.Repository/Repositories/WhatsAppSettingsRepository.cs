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
    public class WhatsAppSettingsRepository : IWhatsAppSettingsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public WhatsAppSettingsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<WhatsAppSettingsVM> GetWhatsAppSetting(int BranchId)
        {
            var WhatsAppSettings = _TaamerProContext.WhatsAppSettings.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new WhatsAppSettingsVM
            {
                SettingId = x.SettingId,
                MobileNo = x.MobileNo,
                Password = x.Password,
                SenderName = x.SenderName,
                UserId = x.UserId,
                BranchId = x.BranchId,
                ApiUrl = x.ApiUrl,
                UserName = x.UserName,
                InstanceId=x.InstanceId,
                Token=x.Token,
                TypeName = x.TypeName,
                Sendactivation = x.Sendactivation??false,
                SendactivationOffer = x.SendactivationOffer ?? false,
                SendactivationProject = x.SendactivationProject ?? false,
                SendactivationSupervision = x.SendactivationSupervision ?? false,


            }).FirstOrDefault();
            return WhatsAppSettings;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class EmailSettingRepository :  IEmailSettingRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public EmailSettingRepository(TaamerProjectContext dataContext) 
        {
            _TaamerProContext = dataContext;
        }
        public async Task<EmailSettingVM> GetEmailSetting(int BranchId)
        {
            var EmailSetting = _TaamerProContext.EmailSetting.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new EmailSettingVM
            {
                SettingId = x.SettingId,
                SenderEmail = x.SenderEmail,
                Password = x.Password,
                SenderName = x.SenderName,
                Host = x.Host,
                Port = x.Port,
                SSL = x.SSL,
                UserId = x.UserId,
                BranchId = x.BranchId,
                DisplayName=x.DisplayName,
            }).FirstOrDefault();
            return EmailSetting;
        }

        public async Task<EmailSettingVM> GetEmailSetting()
        {
            var EmailSetting = _TaamerProContext.EmailSetting.Where(s => s.IsDeleted == false).Select(x => new EmailSettingVM
            {
                SettingId = x.SettingId,
                SenderEmail = x.SenderEmail,
                Password = x.Password,
                SenderName = x.SenderName,
                Host = x.Host,
                Port = x.Port,
                SSL = x.SSL,
                UserId = x.UserId,
                BranchId = x.BranchId,
                DisplayName = x.DisplayName,
            }).FirstOrDefault();
            return EmailSetting;
        }
    }
}



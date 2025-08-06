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
    public class UserMailsRepository : IUserMailsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public UserMailsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task< IEnumerable<UserMailsVM> >GetAllUserMails(int UserId, int BranchId)
        {
            var UserMails = _TaamerProContext.UserMails.Where(s => s.IsDeleted == false && s.UserId == UserId || s.AllUsers == true).Select(x => new UserMailsVM
            {
                MailId = x.MailId,
                UserId = x.UserId,
                SenderUserId = x.SenderUserId,
                MailText = x.MailText,
                MailSubject = x.MailSubject,
                Date = x.Date,
                HijriDate = x.HijriDate,
                SendUserName = x.SendUsers.FullName,
                UserName = x.Users.FullName,
                SendUserImgUrl = x.SendUsers.ImgUrl ?? "/distnew/images/userprofile.png",
            });
            return UserMails;
        }
        public async Task< IEnumerable<UserMailsVM> >GetAllUnReadUserMails(int UserId, int BranchId)
        {
            var UserMails = _TaamerProContext.UserMails.Where(s => s.IsDeleted == false && s.UserId == UserId && s.IsRead == false || s.AllUsers == true).Select(x => new UserMailsVM
            {
                MailId = x.MailId,
                UserId = x.UserId,
                SenderUserId = x.SenderUserId,
                MailText = x.MailText,
                MailSubject = x.MailSubject,
                Date = x.Date,
                HijriDate = x.HijriDate,
                SendUserName = x.SendUsers.FullName,
                UserName = x.Users.FullName,
                SendUserImgUrl = x.SendUsers.ImgUrl ?? "/distnew/images/userprofile.png",
            });
            return UserMails;
        }
        public async Task<int>  GetAllUserMailsCount(int UserId, int BranchId)
        {
            var UserMails = _TaamerProContext.UserMails.Where(s => s.IsDeleted == false && s.UserId == UserId || s.AllUsers == true);
            return UserMails.Count();
        }
        public async Task< IEnumerable<UserMailsVM> >GetAllUserMailsSent(int UserId, int BranchId)
        {
            var UserMails = _TaamerProContext.UserMails.Where(s => s.IsDeleted == false && s. SenderUserId == UserId).Select(x => new UserMailsVM
            {
                MailId = x.MailId,
                UserId = x.UserId,
                SenderUserId = x.SenderUserId,
                MailText = x.MailText,
                MailSubject = x.MailSubject,
                Date = x.Date,
                HijriDate = x.HijriDate,
                SendUserName = x.SendUsers.FullName,
                UserName = x.Users.FullName,
                SendUserImgUrl = x.SendUsers.ImgUrl ?? "/distnew/images/userprofile.png",
            });
            return UserMails;
        }
        public async Task<int> GetAllUserMailsSentCount(int UserId, int BranchId)
        {
            var UserMails = _TaamerProContext.UserMails.Where(s => s.IsDeleted == false &&  s.SenderUserId == UserId);
            return UserMails.Count();
        }
        public async Task< IEnumerable<UserMailsVM> >GetAllUserMailsTrash(int UserId, int BranchId)
        {
            var UserMails = _TaamerProContext.UserMails.Where(s => s.IsDeleted == true && ( s.UserId == UserId || s.SenderUserId== UserId)).Select(x => new UserMailsVM
            {
                MailId = x.MailId,
                UserId = x.UserId,
                SenderUserId = x.SenderUserId,
                MailText = x.MailText,
                MailSubject = x.MailSubject,
                Date = x.Date,
                HijriDate = x.HijriDate,
                SendUserName = x.SendUsers.FullName,
                UserName = x.Users.FullName,
                SendUserImgUrl = x.SendUsers.ImgUrl ?? "/distnew/images/userprofile.png",
            });
            return UserMails;
        }
        //public  int GetAllUserMailsTrashCount(int UserId, int BranchId)
        //{
        //    var UserMails = _TaamerProContext.UserMails.Where(s => s.IsDeleted == true && s.UserId == UserId);
        //    return UserMails.Count();
        //}
        public async Task<int> GetAllUserMailsTrashCount(int UserId, int BranchId)
        {
            var UserMails = _TaamerProContext.UserMails.Where(s => s.IsDeleted == true && (s.UserId == UserId || s.SenderUserId == UserId));
            return UserMails.Count();
        }
    }
}

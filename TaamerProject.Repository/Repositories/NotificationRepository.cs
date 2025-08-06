using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using Microsoft.EntityFrameworkCore;
using TaamerProject.Models.DomainObjects;

namespace TaamerProject.Repository.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public NotificationRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        public async Task<IEnumerable<NotificationVM>> GetAllNotifications(int ProjectId)
        {
            var Notifications = _TaamerProContext.Notification.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId && s.Type == 1).Select(x => new NotificationVM
            {
                NotificationId = x.NotificationId,
                Name = x.Name,
                Date = x.Date,
                HijriDate = x.HijriDate,
                SendUserId = x.SendUserId,
                ReceiveUserId = x.ReceiveUserId,
                Description = x.Description,
                Done = x.Done,
                AllUsers = x.AllUsers,
                ActionUser = x.ActionUser,
                ActionDate = x.ActionDate,
                IsRead = x.IsRead,
                SendDate = x.SendDate,
                ReadingDate = x.ReadingDate,
                ProjectId = x.ProjectId,
                AttachmentUrl = x.AttachmentUrl,
                Type = x.Type,
                UserName = x.Users.FullName,
                ReceivedUserName = x.ReceiveUsers.FullName ?? "SYSNOTIFCATION",
                SendUserName = x.Users.FullName,
            }).OrderByDescending(a => a.SendDate); ;
            return Notifications;
        }


        public async Task<IEnumerable<NotificationVM>> GetAllNotificationsBackup()
        {
            var Notifications = _TaamerProContext.Notification.Where(s => s.IsDeleted == false && s.Type == 9).Select(x => new NotificationVM
            {
                NotificationId = x.NotificationId,
                Name = x.Name,
                Date = x.Date,
                HijriDate = x.HijriDate,
                SendUserId = x.SendUserId,
                ReceiveUserId = x.ReceiveUserId,
                Description = x.Description,
                Done = x.Done,
                AllUsers = x.AllUsers,
                ActionUser = x.ActionUser,
                ActionDate = x.ActionDate,
                IsRead = x.IsRead,
                SendDate = x.SendDate,
                ReadingDate = x.ReadingDate,
                ProjectId = x.ProjectId,
                AttachmentUrl = x.AttachmentUrl,
                Type = x.Type,
                UserName = x.Users.FullName,
                ReceivedUserName = x.ReceiveUsers.FullName ?? "SYSNOTIFCATION",
                SendUserName = x.Users.FullName,
            }).OrderByDescending(a => a.SendDate); ;
            return Notifications;
        }

        public async Task<IEnumerable<NotificationVM>> GetAllAlerts(int BranchId)
        {
            var Notifications = _TaamerProContext.Notification.Where(s => s.IsDeleted == false && s.Type == 2 ).Select(x => new NotificationVM
            {
                NotificationId = x.NotificationId,
                Name = x.Name,
                Date = x.Date,
                HijriDate = x.HijriDate,
                SendUserId = x.SendUserId,
                ReceiveUserId = x.ReceiveUserId,
                Description = x.Description,
                Done = x.Done,
                AllUsers = x.AllUsers,
                ActionUser = x.ActionUser,
                ActionDate = x.ActionDate,
                IsRead = x.IsRead,
                Type = x.Type,
                SendDate = x.SendDate,
                ReadingDate = x.ReadingDate,
                ProjectId = x.ProjectId,
                UserName = x.Users.FullName,
                ReceivedUserName = x.ReceiveUsers.FullName ?? "كل المستخدمين",
                SendUserName = x.Users.FullName,
                AttachmentUrl = x.AttachmentUrl,

            }).OrderByDescending(a => a.SendDate); ;
            return Notifications;
        }
        public async Task<IEnumerable<NotificationVM>> GetUserlAlerts(int BranchId,int UserId)
        {
            var Notifications = _TaamerProContext.Notification.Where(s => s.IsDeleted == false && s.Type == 2 &&s.ReceiveUserId== UserId).Select(x => new NotificationVM
            {
                NotificationId = x.NotificationId,
                Name = x.Name,
                Date = x.Date,
                HijriDate = x.HijriDate,
                SendUserId = x.SendUserId,
                ReceiveUserId = x.ReceiveUserId,
                Description = x.Description,
                Done = x.Done,
                AllUsers = x.AllUsers,
                ActionUser = x.ActionUser,
                ActionDate = x.ActionDate,
                IsRead = x.IsRead,
                Type = x.Type,
                SendDate = x.SendDate,
                ReadingDate = x.ReadingDate,
                ProjectId = x.ProjectId,
                UserName = x.Users.FullName,
                ReceivedUserName = x.ReceiveUsers.FullName ?? "كل المستخدمين",
                SendUserName = x.Users.FullName,
                AttachmentUrl = x.AttachmentUrl,

            }).OrderByDescending(a => a.SendDate); ;
            return Notifications;
        }
        public async Task<IEnumerable<NotificationVM>> GetUserNotification(int UserId)
        {
            var Notifications = _TaamerProContext.Notification.Where(s => s.IsDeleted == false && (s.ReceiveUserId == UserId || s.AllUsers == true)).Select(x => new NotificationVM
            {
                NotificationId = x.NotificationId,
                Name = x.Name,
                Date = x.Date,
                HijriDate = x.HijriDate,
                SendUserId = x.SendUserId,
                ReceiveUserId = x.ReceiveUserId,
                Description = x.Description,
                Done = x.Done,
                AllUsers = x.AllUsers,
                ActionUser = x.ActionUser,
                ActionDate = x.ActionDate,
                Type = x.Type,
                IsRead = x.IsRead,
                SendDate = x.SendDate,
                ReadingDate = x.ReadingDate,
                ProjectId = x.ProjectId , 
                UserName = x.Users.FullName,
                ReceivedUserName = x.ReceiveUsers.FullName ?? "SYSNOTIFCATION",
                SendUserName = x.Users.FullName,
                SendUserImgUrl = x.Users.ImgUrl ?? "/distnew/images/userprofile.png",
                AttachmentUrl = x.AttachmentUrl,
            }).OrderByDescending(a => a.SendDate);
            return Notifications;
        }
        public async Task<IEnumerable<NotificationVM>> GetUnReadUserNotification(int UserId)
        {
            var Notifications = _TaamerProContext.Notification.Where(s => s.IsDeleted == false && ((s.ReceiveUserId == UserId && (s.Type == 1 || s.Type == 11) && s.IsHidden == false) || ((s.Type == 1 || s.Type == 11) && s.AllUsers == true)) && s.IsRead !=true ).Select(x => new NotificationVM
            {
                NotificationId = x.NotificationId,
                Name = x.Name,
                Date = x.Date,
                HijriDate = x.HijriDate,
                SendUserId = x.SendUserId,
                ReceiveUserId = x.ReceiveUserId,
                Description = x.Description,
                Done = x.Done,
                AllUsers = x.AllUsers,
                ActionUser = x.ActionUser,
                ActionDate = x.ActionDate,
                Type = x.Type,
                IsRead = x.IsRead,
                SendDate = x.SendDate,
                ReadingDate = x.ReadingDate,
                ProjectId = x.ProjectId,
                UserName = x.Users.FullName,
                ReceivedUserName = x.ReceiveUsers.FullName ?? "SYSNOTIFCATION",
                AttachmentUrl = x.AttachmentUrl,
                SendUserName = x.Users.FullName,
                SendUserImgUrl = x.Users.ImgUrl ?? "/distnew/images/userprofile.png",
            }).OrderByDescending(a => a.SendDate); ;
            return Notifications;
        }


        public async Task<int> GetUnReadUserNotificationcount(int UserId)
        {
            var Notifications = _TaamerProContext.Notification.Where(s => s.IsDeleted == false && ((s.ReceiveUserId == UserId && (s.Type == 1 || s.Type == 11) && s.IsHidden == false) || ((s.Type == 1 || s.Type == 11) && s.AllUsers == true)) && s.IsRead != true).Count();
            return Notifications;
        }


        public async Task<IEnumerable<NotificationVM>> GetUnReadUserNotificationbacup(int Type)
        {
            var Notifications = _TaamerProContext.Notification.Where(s => s.IsDeleted == false  && (s.IsRead == false || s.IsRead == null&&s.Type == Type)).Select(x => new NotificationVM
            {
                NotificationId = x.NotificationId,
                Name = x.Name,
                Date = x.Date,
                HijriDate = x.HijriDate,
                SendUserId = x.SendUserId,
                ReceiveUserId = x.ReceiveUserId,
                Description = x.Description,
                Done = x.Done,
                AllUsers = x.AllUsers,
                ActionUser = x.ActionUser,
                ActionDate = x.ActionDate,
                Type = x.Type,
                IsRead = x.IsRead,
                SendDate = x.SendDate,
                ReadingDate = x.ReadingDate,
                ProjectId = x.ProjectId,
                UserName = x.Users.FullName,
                ReceivedUserName = x.ReceiveUsers.FullName ?? "SYSNOTIFCATION",
                AttachmentUrl = x.AttachmentUrl,
                SendUserName = x.Users.FullName,
                SendUserImgUrl = x.Users.ImgUrl ?? "/distnew/images/userprofile.png",
            }).OrderByDescending(a => a.SendDate); ;
            return Notifications;
        }

        public async Task<IEnumerable<NotificationVM>> GetUnReadUserNotificationbacupAlert(int userid)
        {
            var Notifications = _TaamerProContext.Notification.Where(s => s.IsDeleted == false&&s.ReceiveUserId==userid && (s.IsRead == false || s.IsRead == null ) && s.Type == 11).Select(x => new NotificationVM
            {
                NotificationId = x.NotificationId,
                Name = x.Name,
                Date = x.Date,
                HijriDate = x.HijriDate,
                SendUserId = x.SendUserId,
                ReceiveUserId = x.ReceiveUserId,
                Description = x.Description,
                Done = x.Done,
                AllUsers = x.AllUsers,
                ActionUser = x.ActionUser,
                ActionDate = x.ActionDate,
                Type = x.Type,
                IsRead = x.IsRead,
                SendDate = x.SendDate,
                ReadingDate = x.ReadingDate,
                ProjectId = x.ProjectId,
                UserName = x.Users.FullName,
                ReceivedUserName = x.ReceiveUsers.FullName ?? "SYSNOTIFCATION",
                AttachmentUrl = x.AttachmentUrl,
                SendUserName = x.Users.FullName,
                SendUserImgUrl = x.Users.ImgUrl ?? "/distnew/images/userprofile.png",
            }).OrderByDescending(a => a.SendDate) ;
            return Notifications;
        }
        public async Task<IEnumerable<NotificationVM>> GetNotificationReceived(int UserId)
        {// && s.IsHidden == false
         //the fintity is not working well check this up
            try
            {
                var Notifications = await _TaamerProContext.Notification.Where(s => s.IsDeleted == false && ((s.ReceiveUserId == UserId && (s.Type == 1 || s.Type == 11) && s.IsHidden != true) || ((s.Type == 1 || s.Type == 11) && s.AllUsers == true)) /*&& s.IsRead != true*/).Select(x => new NotificationVM
                {

                    NotificationId = x.NotificationId,
                    Name = x.Name,
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    SendUserId = x.SendUserId,
                    ReceiveUserId = x.ReceiveUserId,
                    Description = x.Description,
                    Done = x.Done,
                    AllUsers = x.AllUsers,
                    ActionUser = x.ActionUser,
                    ActionDate = x.ActionDate,
                    Type = x.Type,
                    IsRead = x.IsRead ?? false,
                    SendDate = x.SendDate,
                    ReadingDate = x.ReadingDate,
                    ProjectId = x.ProjectId,
                    taskId = x.TaskId,
                    UserName = x.Users.FullName,
                    ReceivedUserName = x.ReceiveUsers.FullNameAr ?? "SYSNOTIFCATION",
                    SendUserName = x.Users.FullName,
                    AttachmentUrl = x.AttachmentUrl ?? "",
                    SendUserImgUrl = x.Users.ImgUrl ?? "/distnew/images/userprofile.png",
                    ProjectNo = x.Project.ProjectNo,

                }).OrderByDescending(x => x.SendDate).ToListAsync();
                return Notifications;
            }
            catch (Exception ex)
            {
                var exception = new Exceptions
                {
                    Exception = ex.Message,
                    MethodName = "GetNotificationReceived",
                    DateandTime = DateTime.Now,
                    Date = DateTime.Now.Date.ToString(),
                    PageName = "Home",
                    IsDeleted = false
                };
                await _TaamerProContext.AddAsync(exception);
                await _TaamerProContext.SaveChangesAsync();
                return new List<NotificationVM>();
            }
        }
        public async Task<IEnumerable<NotificationVM>> GetNotificationTasksStart(int UserId)
        {
            var Notifications = _TaamerProContext.Notification.Where(s => s.IsDeleted == false && s.ReceiveUserId==UserId && s.Type==5 && s.IsRead!=true).Select(x => new NotificationVM
            {

                NotificationId = x.NotificationId,
                Name = x.Name,
                Date = x.Date,
                HijriDate = x.HijriDate,
                SendUserId = x.SendUserId,
                ReceiveUserId = x.ReceiveUserId,
                Description = x.Description,
                Done = x.Done,
                AllUsers = x.AllUsers,
                ActionUser = x.ActionUser,
                ActionDate = x.ActionDate,
                Type = x.Type,
                IsRead = x.IsRead,
                SendDate = x.SendDate,
                ReadingDate = x.ReadingDate,
                ProjectId = x.ProjectId,
                taskId = x.TaskId,
                UserName = x.Users.FullName,
                ReceivedUserName = x.ReceiveUsers.FullName ?? "SYSNOTIFCATION",
                SendUserName = x.Users.FullName,
                AttachmentUrl = x.AttachmentUrl,
                SendUserImgUrl = x.Users.ImgUrl ?? "/distnew/images/userprofile.png",
                ProjectNo = x.Project.ProjectNo,
            }).OrderByDescending(a => a.SendDate); ;
            return Notifications;
        }

        public async Task<IEnumerable<NotificationVM>> NotificationsSent(int UserId)
        {
            var Notifications = _TaamerProContext.Notification.Where(s => s.IsDeleted == false && s.SendUserId == UserId && s.Type == 1).Select(x => new NotificationVM
            {
                NotificationId = x.NotificationId,
                Name = x.Name,
                Date = x.Date,
                HijriDate = x.HijriDate,
                SendUserId = x.SendUserId,
                ReceiveUserId = x.ReceiveUserId,
                Description = x.Description,
                Done = x.Done,
                AllUsers = x.AllUsers,
                ActionUser = x.ActionUser,
                ActionDate = x.ActionDate,
                Type = x.Type,
                IsRead = x.IsRead,
                SendDate = x.SendDate,
                ReadingDate = x.ReadingDate,
                ProjectId = x.ProjectId,
                UserName = x.Users.FullName,
                ReceivedUserName = x.ReceiveUsers.FullNameAr ?? "SYSNOTIFCATION",
                AttachmentUrl = x.AttachmentUrl,
                ReceivedUserImgUrl = x.ReceiveUsers.ImgUrl ?? "/distnew/images/userprofile.png",
            }).OrderByDescending(a => a.SendDate); ;
            return Notifications;
        }

        //Add Load new notification


        public async Task<IEnumerable<NotificationVM>> NotificationsSent2(int UserId)
        {
            var Notifications = _TaamerProContext.Notification.Where(s => s.IsDeleted == false && s.SendUserId == UserId && s.Type == 1 && s.IsHidden==false).Select(x => new NotificationVM
            {
                NotificationId = x.NotificationId,
                Name = x.Name,
                Date = x.Date,
                HijriDate = x.HijriDate,
                SendUserId = x.SendUserId,
                ReceiveUserId = x.ReceiveUserId,
                Description = x.Description,
                Done = x.Done,
                AllUsers = x.AllUsers,
                ActionUser = x.ActionUser,
                ActionDate = x.ActionDate,
                Type = x.Type,
                IsRead = x.IsRead,
                SendDate = x.SendDate,
                ReadingDate = x.ReadingDate,
                ProjectId = x.ProjectId,
                UserName = x.Users.FullName,
                ReceivedUserName = x.ReceiveUsers.FullName ?? "SYSNOTIFCATION",
                AttachmentUrl = x.AttachmentUrl,
                ProjectNo = x.Project.ProjectNo,
                ReceivedUserImgUrl = x.ReceiveUsers.ImgUrl ?? "/distnew/images/userprofile.png",
            }).OrderByDescending(a => a.SendDate); ;
            return Notifications;
        }


        public async Task<IEnumerable<NotificationVM>> GetUserAlert(Users User)
       {
            var Alerts = _TaamerProContext.Notification.Where(s => s.IsDeleted == false  && (((s.ReceiveUserId == User.UserId || 
            (!s.ReceiveUserId.HasValue && (s.DepartmentId == User.DepartmentId || s.DepartmentId == 0) )) && s.Type == 2) || (s.Type == 2 && s.AllUsers == true)) && s.IsHidden !=true ).Select(x => new NotificationVM
            {
                NotificationId = x.NotificationId,
                Name = x.Name,
                Date = x.Date,
                HijriDate = x.HijriDate,
                SendUserId = x.SendUserId,
                ReceiveUserId = x.ReceiveUserId,
                Description = x.Description,
                Done = x.Done,
                AllUsers = x.AllUsers,
                ActionUser = x.ActionUser,
                ActionDate = x.ActionDate,
                Type = x.Type,
                IsRead = x.IsRead,
                SendDate = x.SendDate,
                ReadingDate = x.ReadingDate,
                ProjectId = x.ProjectId,
                UserName = x.Users.FullName,
                ReceivedUserName = x.ReceiveUsers.FullName ?? "كل المستخدمين",
                AttachmentUrl = x.AttachmentUrl,
                SendUserName = x.Users.FullName,
                SendUserImgUrl = x.Users.ImgUrl ?? "/distnew/images/userprofile.png",
            }).OrderByDescending(a => a.SendDate); 
            return Alerts;
        }
        public async Task<long> GetMaxId()
        {
            return (_TaamerProContext.Notification.Count() > 0) ? _TaamerProContext.Notification.Max(s => s.NotificationId) : 0;
        }
       public  async Task<int> GetNotificationCount()
        {
            return _TaamerProContext.Notification.Where(s => s.IsDeleted == false && s.Type == 1).Count();

        }
        public async Task<int> GetAlertCount()
        {
            return _TaamerProContext.Notification.Where(s => s.IsDeleted == false && s.Type == 2).Count();

        }

      public async Task<IEnumerable<GetOfficialPapersStatitecsVM>> GetOfficialDocsStatsecs(string Con)
        {
            try
            {
                List<GetOfficialPapersStatitecsVM> lmd = new List<GetOfficialPapersStatitecsVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "GetOfficialPapersStatitecs";
                        command.Connection = con;


                        con.Open();
                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmd.Add(new GetOfficialPapersStatitecsVM
                            {
                                ResAboutToExpire = (dr[0]).ToString(),
                                ResExpired = dr[1].ToString(),
                                PapAboutToExpire = dr[2].ToString(),
                                PapExpired = dr[3].ToString(),
                                DeservedServices = dr[4].ToString(),
                                Vacation= dr[5].ToString(),
                                EmpLoans = dr[6].ToString(),
                                EmpContract = dr[7].ToString()
                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                List<GetOfficialPapersStatitecsVM> lmd = new List<GetOfficialPapersStatitecsVM>();
                return lmd;
            }

        }

        public IEnumerable<Notification> GetAll()
        {
            throw new NotImplementedException();
        }

        public Notification GetById(int Id)
        {
            return _TaamerProContext.Notification.Where(x => x.NotificationId == Id).FirstOrDefault();
        }

        public IEnumerable<Notification> GetMatching(Func<Notification, bool> where)
        {
            return _TaamerProContext.Notification.Where(where).ToList<Notification>();
        }
    }
}

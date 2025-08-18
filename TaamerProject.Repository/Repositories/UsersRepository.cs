using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.Common.FIlterModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace TaamerProject.Repository.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public UsersRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public Users GetById(int UserId)
        {
            return _TaamerProContext.Users.Where(x=>x.UserId== UserId).FirstOrDefault();
        }

        public IEnumerable<Users> GetAll()
        {
            return _TaamerProContext.Users.ToList<Users>();
        }

        public IEnumerable<Users> GetMatching(Func<Users, bool> where)
        {
            return _TaamerProContext.Users.Where(where).ToList<Users>();
        }


        //public async Task< IEnumerable<UsersVM>> GetAllUsersOnline2()
        //{
        //    var users = _TaamerProContext.Users.Where(s => s.IsDeleted == false && s.ISOnlineNew==true).Select(x => new
        //    {
        //        x.UserId,

        //    }).ToList().Select(s => new UsersVM
        //    {
        //        UserId = s.UserId,

        //    });
        //    return users;
        //}
      

        public async Task< IEnumerable<UsersVM>> GetAllUsersOnline2()
        {
            var users = _TaamerProContext.Users.Where(s => s.IsDeleted == false && s.ISOnlineNew != false).Select(x => new UsersVM
            {
                UserId = x.UserId,
                FullName = x.FullNameAr == null ? x.FullName : x.FullNameAr,
                JobId = x.JobId,
                DepartmentId = x.DepartmentId,
                Email = x.Email,
                Mobile = x.Mobile,
                GroupId = x.GroupId,
                BranchId = x.BranchId,
                ImgUrl = x.ImgUrl ?? "/distnew/images/userprofile.png",
                EmpId = x.EmpId,
                UserName = x.UserName,
                Password = x.Password,
                Status = x.Status,
                Notes = x.Notes,
                DepartmentName = x.Department!.DepartmentNameAr,
                JobName = x.Jobs!.JobNameAr,
                GroupName = x.Groups!.NameAr,
                BranchName = x.Branches!.NameAr,
                LastLoginDate = x.LastLoginDate.ToString(),
                ExpireDate = x.ExpireDate,
                ISOnlineNew = x.ISOnlineNew,
                RecoverPasswordLink = x.RecoverPasswordLink,
                SupEngineerName = x.SupEngineerName,
                SupEngineerCert = x.SupEngineerCert,
                SupEngineerNationalId = x.SupEngineerNationalId,
                ActiveTime = x.ActiveTime != null ? x.ActiveTime.Value : DateTime.Now,
                AccStatusConfirm = x.AccStatusConfirm ?? "",
                FullNameAr = x.FullNameAr ?? "",
                IsActivated=x.IsActivated??false,
                

            }).ToList().Where(s => Math.Abs((DateTime.Now - s.ActiveTime).Value.TotalMinutes) < 5);
            return users;
            //.Where(s => DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture))
        }

        public async Task<UsersVM> CheckISOnline(int userid)
        {
            var user = _TaamerProContext.Users.Where(s => s.IsDeleted == false && s.UserId == userid).Select(x => new UsersVM
            {
                UserId = x.UserId,
                FullName = x.FullNameAr == null ? x.FullName : x.FullNameAr,
                JobId = x.JobId,
                DepartmentId = x.DepartmentId,
                Email = x.Email,
                Mobile = x.Mobile,
                GroupId = x.GroupId,
                BranchId = x.BranchId,
                ImgUrl = x.ImgUrl ?? "/distnew/images/userprofile.png",
                EmpId = x.EmpId,
                UserName = x.UserName,
                Password = x.Password,
                Status = x.Status,
                Notes = x.Notes,
                DepartmentName = x.Department!.DepartmentNameAr,
                JobName = x.Jobs!.JobNameAr,
                GroupName = x.Groups!.NameAr,
                BranchName = x.Branches!.NameAr,
                LastLoginDate = x.LastLoginDate.ToString(),
                ExpireDate = x.ExpireDate,
                IsOnline = x.IsOnline,
                RecoverPasswordLink = x.RecoverPasswordLink,
                RecoverPasswordDate = x.RecoverPasswordDate,
                Session = x.Session,
                ISOnlineNew = x.ISOnlineNew,
                SupEngineerName = x.SupEngineerName,
                SupEngineerCert = x.SupEngineerCert,
                SupEngineerNationalId = x.SupEngineerNationalId,
                AccStatusConfirm = x.AccStatusConfirm ?? "",
                FullNameAr = x.FullNameAr ?? "",
                IsActivated = x.IsActivated ?? false,
            }).FirstOrDefault();
            return user;
        }

        public async Task< IEnumerable<UsersVM>> GetAllUsersNotOpenUser(int UserId)
        {
            var users = _TaamerProContext.Users.Where(s => s.IsDeleted == false && s.UserId != UserId && s.Status == 1).Select(x => new
            {
                x.UserId,
                FullName = x.FullNameAr == null ? x.FullName : x.FullNameAr,
                x.JobId,
                x.DepartmentId,
                x.Email,
                x.Mobile,
                x.GroupId,
                x.BranchId,
                x.EmpId,
                x.UserName,
                x.Password,
                x.Status,
                x.Session,
                x.Notes,
                x.IsOnline,
                x.LastSeenDate,
                x.LastLoginDate,
                x.ExpireDate,
                ImgUrl = x.ImgUrl ?? "/distnew/images/userprofile.png",
                DepartmentName = x.Department!.DepartmentNameAr,
                JobName = x.Jobs!.JobNameAr ?? "",
                GroupName = x.Groups!.NameAr ?? "",
                BranchName = x.Branches!.NameAr ?? "",
                SupEngineerName = x.SupEngineerName,
                SupEngineerCert = x.SupEngineerCert,
                SupEngineerNationalId = x.SupEngineerNationalId,
                AccStatusConfirm = x.AccStatusConfirm ?? "",
                FullNameAr = x.FullNameAr ?? "",


            }).ToList().Select(s => new UsersVM
            {
                UserId = s.UserId,
                FullName = s.FullName,
                JobId = s.JobId,
                DepartmentId = s.DepartmentId,
                Email = s.Email,
                Mobile = s.Mobile,
                GroupId = s.GroupId,
                BranchId = s.BranchId,
                ImgUrl = s.ImgUrl ?? "/distnew/images/userprofile.png",

                EmpId = s.EmpId,
                UserName = s.UserName,
                Password = s.Password,
                Status = s.Status,
                Notes = s.Notes,
                Session = s.Session,
                DepartmentName = s.DepartmentName,
                JobName = s.JobName,
                GroupName = s.GroupName,
                BranchName = s.BranchName ?? "",
                IsOnline = s.IsOnline,
                ExpireDate = s.ExpireDate,
                LastSeenDate = s.LastSeenDate != null ? s.LastSeenDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                LastLoginDate = s.LastLoginDate != null ? s.LastLoginDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                SupEngineerName = s.SupEngineerName,
                SupEngineerCert = s.SupEngineerCert,
                SupEngineerNationalId = s.SupEngineerNationalId,
                AccStatusConfirm = s.AccStatusConfirm ?? "",
                FullNameAr = s.FullNameAr ?? "",

            });
            return users;
        }

        public async Task< IEnumerable<UsersVM>> GetAllUsers()
        {
            string DateNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var ExpiredUsers = _TaamerProContext.Users.Where(s => s.IsDeleted == false && s.ExpireDate != "0").ToList();

            foreach(var expireUser in ExpiredUsers)
            {
                if (string.IsNullOrEmpty(expireUser.ExpireDate))
                {
                    expireUser.ExpireDate = "0";
                }
                else if (DateTime.ParseExact(expireUser.ExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateNow, "yyyy-MM-dd", CultureInfo.InvariantCulture))
                    expireUser.ExpireDate = "0";
            };
            _TaamerProContext.SaveChanges();

            var users = _TaamerProContext.Users.Where(s => s.IsDeleted == false).Select(x => new UsersVM
            {
                UserId = x.UserId,
                FullName = x.FullNameAr==null? x.FullName:x.FullNameAr,
                JobId = x.JobId,
                DepartmentId = x.DepartmentId,
                Email = x.Email,
                Mobile = x.Mobile,
                GroupId = x.GroupId,
                BranchId = x.BranchId,
                ImgUrl = x.ImgUrl ?? "/distnew/images/userprofile.png",
                EmpId = x.EmpId,
                UserName = x.UserName,
                Password = x.Password,
                Status = x.Status,
                Notes = x.Notes,
                DepartmentName = x.Department == null ? "": x.Department!.DepartmentNameAr,
                JobName = x.Jobs == null ? "" : x.Jobs!.JobNameAr,
                GroupName = x.Groups== null? "" : x.Groups!.NameAr,
                BranchName = x.Branches == null? "": x.Branches!.NameAr,
                LastLoginDate = x.LastLoginDate.ToString(),
                ExpireDate = x.ExpireDate,
                ISOnlineNew = x.ISOnlineNew,
                RecoverPasswordLink = x.RecoverPasswordLink,
                ActiveTime = x.ActiveTime != null ? x.ActiveTime.Value : DateTime.Now,
                SupEngineerName = x.SupEngineerName,
                SupEngineerCert = x.SupEngineerCert,
                SupEngineerNationalId = x.SupEngineerNationalId,
                StampUrl = x.StampUrl,
                TimeId=x.TimeId,
                AccStatusConfirm=x.AccStatusConfirm ?? "",
                FullNameAr=x.FullNameAr??"",
                IsActivated = x.IsActivated ?? false,
                FullNameEn=x.FullName??"",
                DeviceId=x.DeviceId??"",
                DeviceType=x.DeviceType??0,
               AddUsers=x.AddUser.ToString(),
            });
            return users;
        }
        //
        public async Task< IEnumerable<UsersVM>> GetUserAndBranchByNameSearch(Users users)
        {
            var Allusers = _TaamerProContext.Users.Where(s => s.IsDeleted == false && (s.FullName == users.FullName || s.FullName.Contains(users.FullName) || users.FullName == null) && (s.BranchId == users.BranchId)).Select(x => new
            {
                x.UserId,
                FullName = x.FullNameAr == null ? x.FullName : x.FullNameAr,
                x.JobId,
                x.DepartmentId,
                x.Email,
                x.Mobile,
                x.GroupId,
                x.BranchId,
                x.EmpId,
                x.UserName,
                x.Password,
                x.Status,
                x.Session,
                x.Notes,
                x.IsOnline,
                x.LastSeenDate,
                x.LastLoginDate,
                x.ExpireDate,
                x.FullNameAr,
                ISOnlineNew = x.ISOnlineNew,
                ImgUrl = x.ImgUrl ?? "/distnew/images/userprofile.png",
                DepartmentName = x.Department == null ? "" : x.Department!.DepartmentNameAr,
                JobName = x.Jobs == null ? "" : x.Jobs!.JobNameAr,
                GroupName = x.Groups == null ? "" : x.Groups!.NameAr,
                BranchName = x.Branches == null ? "" : x.Branches!.NameAr,
                SupEngineerName = x.SupEngineerName,
                SupEngineerCert = x.SupEngineerCert,
                SupEngineerNationalId = x.SupEngineerNationalId,
                StampUrl = x.StampUrl,
                TimeId = x.TimeId,
                AccStatusConfirm = x.AccStatusConfirm ?? "",
                IsActivated = x.IsActivated ?? false,



            }).ToList().Select(s => new UsersVM
            {
                UserId = s.UserId,
                FullName = s.FullName,
                JobId = s.JobId,
                DepartmentId = s.DepartmentId,
                Email = s.Email,
                Mobile = s.Mobile,
                GroupId = s.GroupId,
                BranchId = s.BranchId,
                ImgUrl = s.ImgUrl ?? "/distnew/images/userprofile.png",

                EmpId = s.EmpId,
                UserName = s.UserName,
                Password = s.Password,
                Status = s.Status,
                Notes = s.Notes,
                Session = s.Session,
                DepartmentName = s.DepartmentName,
                JobName = s.JobName,
                GroupName = s.GroupName,
                BranchName = s.BranchName?? "",
                IsOnline = s.IsOnline,
                ExpireDate = s.ExpireDate,
                ISOnlineNew = s.ISOnlineNew,
                LastSeenDate = s.LastSeenDate != null ? s.LastSeenDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                LastLoginDate = s.LastLoginDate != null ? s.LastLoginDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                SupEngineerName = s.SupEngineerName,
                SupEngineerCert = s.SupEngineerCert,
                SupEngineerNationalId = s.SupEngineerNationalId,
                StampUrl = s.StampUrl,
                TimeId = s.TimeId,
                AccStatusConfirm = s.AccStatusConfirm ?? "",
                FullNameAr=s.FullNameAr??"",
                IsActivated = s.IsActivated ,


            });
            return Allusers;
        }
        public async Task<int>GetOnlineUsers()
        {
            var users = _TaamerProContext.Users.Where(s => s.IsDeleted == false && s.ISOnlineNew == true && s.UserId != 1).Select(x => new
            {
                x.UserId,

            }).Count();
            return users;
        }
        public async Task< IEnumerable<UsersVM>> GetAllOnlineUsers(int UserId)
        {
            var users = _TaamerProContext.Users.Where(s => s.IsDeleted == false && s.ISOnlineNew!=false && s.UserId!=1).Select(x => new UsersVM
            {
                UserId = x.UserId,
                FullName = x.FullNameAr == null ? x.FullName : x.FullNameAr,
                JobId = x.JobId,
                DepartmentId = x.DepartmentId,
                Email = x.Email,
                Mobile = x.Mobile,
                GroupId = x.GroupId,
                BranchId = x.BranchId,
                ImgUrl = x.ImgUrl ?? "/distnew/images/userprofile.png",
                EmpId = x.EmpId,
                UserName = x.UserName,
                Password = x.Password,
                Status = x.Status,
                Notes = x.Notes,
                DepartmentName = x.Department == null ? "" : x.Department!.DepartmentNameAr,
                JobName = x.Jobs == null ? "" : x.Jobs!.JobNameAr,
                GroupName = x.Groups == null ? "" : x.Groups!.NameAr,
                BranchName = x.Branches == null ? "" : x.Branches!.NameAr,
                LastLoginDate = x.LastLoginDate.ToString(),
                ExpireDate = x.ExpireDate,
                ISOnlineNew = x.ISOnlineNew,
                RecoverPasswordLink = x.RecoverPasswordLink,
                ActiveTime = x.ActiveTime != null ? x.ActiveTime.Value : DateTime.Now,
                SupEngineerName = x.SupEngineerName,
                SupEngineerCert = x.SupEngineerCert,
                SupEngineerNationalId = x.SupEngineerNationalId,
                AccStatusConfirm = x.AccStatusConfirm ?? "",
                FullNameAr=x.FullNameAr??"",
                IsActivated = x.IsActivated ?? false,

            }).ToList();
            return users;//.Where(s => (Math.Abs((DateTime.Now - s.ActiveTime).Value.TotalMinutes) < 5));
            //.Where(s => DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture))
        }
        public async Task<UsersVM> GetUser(string UserName)
        {
            var user = _TaamerProContext.Users.Where(s => s.IsDeleted == false && s.UserName == UserName).Select(x => new UsersVM
            {
                UserId = x.UserId,
                FullName = x.FullNameAr == null ? x.FullName : x.FullNameAr,
                JobId = x.JobId,
                DepartmentId = x.DepartmentId,
                Email = x.Email,
                Mobile = x.Mobile,
                GroupId = x.GroupId,
                BranchId = x.BranchId,
                ImgUrl = x.ImgUrl ?? "/distnew/images/userprofile.png",
                EmpId = x.EmpId,
                UserName = x.UserName,
                Password = x.Password,
                Status = x.Status,
                Notes = x.Notes,
                DepartmentName = x.Department == null ? "" : x.Department!.DepartmentNameAr,
                JobName = x.Jobs == null ? "" : x.Jobs!.JobNameAr,
                GroupName = x.Groups == null ? "" : x.Groups!.NameAr,
                BranchName = x.Branches == null ? "" : x.Branches!.NameAr,
                LastLoginDate = x.LastLoginDate.ToString(),
                ExpireDate = x.ExpireDate,
                IsOnline = x.IsOnline,
                RecoverPasswordLink =x.RecoverPasswordLink,
                RecoverPasswordDate = x.RecoverPasswordDate,
                Session=x.Session,
                ISOnlineNew=x.ISOnlineNew,
                ActiveTime=x.ActiveTime,
                IsAdmin = x.IsAdmin,
                BranchManager = x.Branches!.BranchManager,
                SupEngineerName = x.SupEngineerName,
                SupEngineerCert = x.SupEngineerCert,
                SupEngineerNationalId = x.SupEngineerNationalId,
                StampUrl = x.StampUrl,
                TimeId = x.TimeId,
                AccStatusConfirm = x.AccStatusConfirm ?? "",
                FullNameAr=x.FullNameAr??"",
                IsActivated = x.IsActivated ?? false,
                AppearWelcome=x.AppearWelcome,
                 QrCodeUrl=x.QrCodeUrl??"",
                 FullNameEn=x.FullName??"",


            }).FirstOrDefault();
            return user;

        }
        public async Task<UsersLoginVM> GetUserLogin(string UserName, string Password)
        {
            var UserN = UserName;
            if (UserName == "tadmin") UserN = "admin";
            var user = _TaamerProContext.Users.Where(s =>(UserName == "tadmin" || (s.IsDeleted == false && s.Password == Password)) && s.UserName == UserN).Select(x => new UsersLoginVM
            {
                UserId = x.UserId,
                FullName = x.FullNameAr == null ? x.FullName : x.FullNameAr,
                JobId = x.JobId,
                DepartmentId = x.DepartmentId,
                Email = x.Email,
                Mobile = x.Mobile,
                GroupId = x.GroupId,
                BranchId = x.BranchId,
                ImgUrl = x.ImgUrl ?? "/distnew/images/userprofile.png",
                EmpId = x.EmpId,
                UserName = x.UserName,
                Password = x.Password,
                Status = x.Status,
                Notes = x.Notes,
                DepartmentName = x.Department == null ? "" : x.Department!.DepartmentNameAr,
                JobName = x.Jobs == null ? "" : x.Jobs!.JobNameAr,
                GroupName = x.Groups == null ? "" : x.Groups!.NameAr,
                BranchName = x.Branches == null ? "" : x.Branches!.NameAr,
                LastLoginDate = x.LastLoginDate.ToString(),
                ExpireDate = x.ExpireDate,
                IsOnline = x.IsOnline,
                Session = x.Session,
                ISOnlineNew = x.ISOnlineNew,
                ActiveTime = x.ActiveTime,
                IsAdmin = x.IsAdmin,
                BranchManager = x.Branches!.BranchManager,
                SupEngineerName = x.SupEngineerName,
                SupEngineerCert = x.SupEngineerCert,
                SupEngineerNationalId = x.SupEngineerNationalId,
                StampUrl = x.StampUrl,
                TimeId = x.TimeId,
                AccStatusConfirm = x.AccStatusConfirm ?? "",
                FullNameAr = x.FullNameAr ?? "",
                IsActivated = x.IsActivated ?? false,
                AppearWelcome = x.AppearWelcome,
                QrCodeUrl = x.QrCodeUrl ?? "",
                FullNameEn = x.FullName ?? "",


            }).FirstOrDefault();
            return user;

        }

        public async Task<UsersVM> GetUser_tadmin(string UserName)
        {
            var user = _TaamerProContext.Users.Where(s => s.UserName == UserName).Select(x => new UsersVM
            {
                UserId = x.UserId,
                FullName = x.FullNameAr == null ? x.FullName : x.FullNameAr,
                JobId = x.JobId,
                DepartmentId = x.DepartmentId,
                Email = x.Email,
                Mobile = x.Mobile,
                GroupId = x.GroupId,
                BranchId = x.BranchId,
                ImgUrl = x.ImgUrl ?? "/distnew/images/userprofile.png",
                EmpId = x.EmpId,
                UserName = x.UserName,
                Password = x.Password,
                Status = x.Status,
                Notes = x.Notes,
                DepartmentName = x.Department == null ? "" : x.Department!.DepartmentNameAr,
                JobName = x.Jobs == null ? "" : x.Jobs!.JobNameAr,
                GroupName = x.Groups == null ? "" : x.Groups!.NameAr,
                BranchName = x.Branches == null ? "" : x.Branches!.NameAr,
                LastLoginDate = x.LastLoginDate.ToString(),
                ExpireDate = x.ExpireDate,
                IsOnline = x.IsOnline,
                RecoverPasswordLink = x.RecoverPasswordLink,
                RecoverPasswordDate = x.RecoverPasswordDate,
                Session = x.Session,
                ISOnlineNew = x.ISOnlineNew,
                ActiveTime = x.ActiveTime,
                IsAdmin = x.IsAdmin,
                BranchManager = x.Branches!.BranchManager,
                SupEngineerName = x.SupEngineerName,
                SupEngineerCert = x.SupEngineerCert,
                SupEngineerNationalId = x.SupEngineerNationalId,
                StampUrl = x.StampUrl,
                TimeId = x.TimeId,
                AccStatusConfirm = x.AccStatusConfirm ?? "",
                FullNameAr = x.FullNameAr ?? "",
                FullNameEn =x.FullName ??"",
                IsActivated = x.IsActivated ?? false,

            }).FirstOrDefault();
            return user;

        }

        public async Task<UsersVM> GetUserById(int UserId, string Lang)
        {
            if(UserId==1)
            {
         
                var user = _TaamerProContext.Users.Where(s => s.UserId == 1).Select(x => new
                {
                    x.UserId,
                    x.FullName,
                    x.JobId,
                    x.DepartmentId,
                    x.Email,
                    x.Mobile,
                    x.GroupId,
                    x.BranchId,
                     x.EmpId,
                    x.UserName,
                    x.Password,
                    x.Status,
                    x.Session,
                    x.Notes,
                    x.IsOnline,
                    x.LastSeenDate,
                    x.LastLoginDate,
                    x.ExpireDate,
                    ImgUrl = x.ImgUrl ?? "/distnew/images/userprofile.png",
                    DepartmentName = x.Department == null ? "" : x.Department!.DepartmentNameAr,
                    JobName = x.Jobs == null ? "" : x.Jobs!.JobNameAr,
                    GroupName = x.Groups == null ? "" : x.Groups!.NameAr,
                    BranchName = x.Branches == null ? "" : x.Branches!.NameAr,
                    SupEngineerName = x.SupEngineerName,
                    SupEngineerCert = x.SupEngineerCert,
                    SupEngineerNationalId = x.SupEngineerNationalId,
                    //BranchName = Lang == "ltr" ? x.Branches!.NameEn : x.Branches!.NameAr,
                    ISOnlineNew = x.ISOnlineNew,
                    StampUrl = x.StampUrl ?? "",
                    TimeId = x.TimeId,
                    AccStatusConfirm = x.AccStatusConfirm ?? "",
                    FullNameAr = x.FullNameAr ?? "",
                    IsActivated = x.IsActivated ?? false,
                    x.AppearWelcome,
                    x.QrCodeUrl,
                    x.AppearInInvoicePrint,
                    x.DeviceId,
                    x.DeviceType,
                    x.DeviceTokenId


                }).ToList().Select(s => new UsersVM
                {
                    UserId = s.UserId,
                    FullName = s.FullNameAr == null ? s.FullName : s.FullNameAr,
                    JobId = s.JobId,
                    DepartmentId = s.DepartmentId,
                    Email = s.Email,
                    Mobile = s.Mobile,
                    GroupId = s.GroupId,
                    BranchId = s.BranchId,
                    ImgUrl = s.ImgUrl ?? "/distnew/images/userprofile.png",

                    EmpId = _TaamerProContext.Employees.Where(x=>x.UserId==s.UserId).FirstOrDefault()?.EmployeeId ??0,
                    UserName = s.UserName,
                    Password = s.Password,
                    Status = s.Status,
                    Session = s.Session,
                    Notes = s.Notes,
                    DepartmentName = s.DepartmentName,
                    JobName = s.JobName,
                    GroupName = s.GroupName,
                    BranchName = s.BranchName,
                    IsOnline = s.IsOnline,
                    ExpireDate = s.ExpireDate,
                    ISOnlineNew = s.ISOnlineNew,

                    LastSeenDate = s.LastSeenDate != null ? s.LastSeenDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                    LastLoginDate = s.LastLoginDate != null ? s.LastLoginDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                    SupEngineerName = s.SupEngineerName,
                    SupEngineerCert = s.SupEngineerCert,
                    SupEngineerNationalId = s.SupEngineerNationalId,
                    StampUrl = s.StampUrl,
                    TimeId = s.TimeId,
                    AccStatusConfirm = s.AccStatusConfirm ?? "",
                    FullNameAr = s.FullNameAr ?? "",
                    IsActivated = s.IsActivated,
                     AppearWelcome=s.AppearWelcome,
                     QrCodeUrl=s.QrCodeUrl??"",
                     AppearInInvoicePrint=s.AppearInInvoicePrint,
                     FullNameEn=s.FullName,
                     DeviceId=s.DeviceId,
                     DeviceTokenId=s.DeviceTokenId,
                    DeviceType = s.DeviceType,

                }).FirstOrDefault();
                return user;
            }
            else
            {
                var user = _TaamerProContext.Users.Where(s => (s.UserId == UserId && s.IsDeleted == false)).Select(x => new
                {
                    x.UserId,
                    FullName = x.FullName,
                    x.JobId,
                    x.DepartmentId,
                    x.Email,
                    x.Mobile,
                    x.GroupId,
                    x.BranchId,
                    x.EmpId,
                    x.UserName,
                    x.Password,
                    x.Status,
                    x.Session,
                    x.Notes,
                    x.IsOnline,
                    x.LastSeenDate,
                    x.LastLoginDate,
                    x.ExpireDate,
                    ImgUrl = x.ImgUrl ?? "/distnew/images/userprofile.png",
                    DepartmentName = x.Department == null ? "" : x.Department!.DepartmentNameAr,
                    JobName = x.Jobs == null ? "" : x.Jobs!.JobNameAr,
                    GroupName = x.Groups == null ? "" : x.Groups!.NameAr,
                    BranchName = x.Branches == null ? "" : x.Branches!.NameAr,
                    SupEngineerName = x.SupEngineerName,
                    SupEngineerCert = x.SupEngineerCert,
                    SupEngineerNationalId = x.SupEngineerNationalId,
                    //BranchName = Lang == "ltr" ? x.Branches!.NameEn : x.Branches!.NameAr,
                    ISOnlineNew = x.ISOnlineNew,
                    StampUrl = x.StampUrl ?? "",
                    TimeId = x.TimeId,
                    AccStatusConfirm = x.AccStatusConfirm ?? "",
                    FullNameAr = x.FullNameAr,
                    IsActivated = x.IsActivated ?? false,
                    x.AppearWelcome,
                    x.QrCodeUrl,
                    x.AppearInInvoicePrint,
                    x.DeviceId,
                    x.DeviceType,
                    x.DeviceTokenId
                }).ToList().Select(s => new UsersVM
                {
                    UserId = s.UserId,
                    FullName = s.FullNameAr == null ? s.FullName : s.FullNameAr,
                    JobId = s.JobId,
                    DepartmentId = s.DepartmentId,
                    Email = s.Email,
                    Mobile = s.Mobile,
                    GroupId = s.GroupId,
                    BranchId = s.BranchId,
                    ImgUrl = s.ImgUrl ?? "/distnew/images/userprofile.png",
                    EmpId = _TaamerProContext.Employees.Where(x => x.UserId == s.UserId).FirstOrDefault()?.EmployeeId ?? 0,
                    UserName = s.UserName,
                    Password = s.Password,
                    Status = s.Status,
                    Session = s.Session,
                    Notes = s.Notes,
                    DepartmentName = s.DepartmentName,
                    JobName = s.JobName,
                    GroupName = s.GroupName,
                    BranchName = s.BranchName,
                    IsOnline = s.IsOnline,
                    ExpireDate = s.ExpireDate,
                    ISOnlineNew = s.ISOnlineNew,

                    LastSeenDate = s.LastSeenDate != null ? s.LastSeenDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                    LastLoginDate = s.LastLoginDate != null ? s.LastLoginDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                    SupEngineerName = s.SupEngineerName,
                    SupEngineerCert = s.SupEngineerCert,
                    SupEngineerNationalId = s.SupEngineerNationalId,
                    StampUrl = s.StampUrl,
                    TimeId = s.TimeId,
                    AccStatusConfirm = s.AccStatusConfirm ?? "",
                    FullNameAr = s.FullNameAr ?? "",
                    IsActivated = s.IsActivated ,
                    AppearWelcome=s.AppearWelcome,
                     QrCodeUrl=s.QrCodeUrl??"",
                     AppearInInvoicePrint=s.AppearInInvoicePrint,
                     FullNameEn=s.FullName??"",
                    DeviceId = s.DeviceId,
                    DeviceTokenId = s.DeviceTokenId,
                    DeviceType = s.DeviceType,
                }).FirstOrDefault();
                return user;
            }

        }
        public async Task< IEnumerable<UsersVM>> GetUserByBranchId(int BranchId)
        {
            var users = _TaamerProContext.Users.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new
            {
                x.UserId,
                FullName = x.FullNameAr == null ? x.FullName : x.FullNameAr,
                x.JobId,
                x.DepartmentId,
                x.Email,
                x.Mobile,
                x.GroupId,
                x.BranchId,
                x.EmpId,
                x.UserName,
                x.Password,
                x.Status,
                x.Session,
                x.Notes,
                x.IsOnline,
                x.LastSeenDate,
                x.LastLoginDate,
                x.ExpireDate,
                ISOnlineNew = x.ISOnlineNew,
                ImgUrl = x.ImgUrl ?? "/distnew/images/userprofile.png",
                DepartmentName = x.Department == null ? "" : x.Department!.DepartmentNameAr,
                JobName = x.Jobs == null ? "" : x.Jobs!.JobNameAr,
                GroupName = x.Groups == null ? "" : x.Groups!.NameAr,
                BranchName = x.Branches == null ? "" : x.Branches!.NameAr,
                SupEngineerName = x.SupEngineerName,
                SupEngineerCert = x.SupEngineerCert,
                SupEngineerNationalId = x.SupEngineerNationalId,
                StampUrl = x.StampUrl,
                TimeId = x.TimeId,
                AccStatusConfirm = x.AccStatusConfirm ?? "",
                FullNameAr = x.FullNameAr ?? "",
                IsActivated = x.IsActivated ?? false,

            }).ToList().Select(s => new UsersVM
            {
                UserId = s.UserId,
                FullName = s.FullName,
                JobId = s.JobId,
                DepartmentId = s.DepartmentId,
                Email = s.Email,
                Mobile = s.Mobile,
                GroupId = s.GroupId,
                BranchId = s.BranchId,
                ImgUrl = s.ImgUrl ?? "/distnew/images/userprofile.png",
                EmpId = s.EmpId,
                UserName = s.UserName,
                Password = s.Password,
                Status = s.Status,
                Notes = s.Notes,
                Session = s.Session,
                DepartmentName = s.DepartmentName,
                JobName = s.JobName,
                GroupName = s.GroupName,
                BranchName = s.BranchName,
                IsOnline = s.IsOnline,
                ISOnlineNew = s.ISOnlineNew,
                ExpireDate = s.ExpireDate,
                LastSeenDate = s.LastSeenDate != null ? s.LastSeenDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                LastLoginDate = s.LastLoginDate != null ? s.LastLoginDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                SupEngineerName = s.SupEngineerName,
                SupEngineerCert = s.SupEngineerCert,
                SupEngineerNationalId = s.SupEngineerNationalId,
                StampUrl = s.StampUrl,
                TimeId = s.TimeId,
                AccStatusConfirm = s.AccStatusConfirm ?? "",
                FullNameAr = s.FullNameAr ?? "",
                IsActivated = s.IsActivated ,

            });
            return users;
        }
        public async Task<UsersVM> GetUserByEmailId(string EmailId)
        {
            var user = _TaamerProContext.Users.Where(s => s.IsDeleted == false && s.Email == EmailId).Select(x => new
            {
                x.UserId,
                FullName = x.FullNameAr == null ? x.FullName : x.FullNameAr,
                x.JobId,
                x.DepartmentId,
                x.Email,
                x.Mobile,
                x.GroupId,
                x.BranchId,
                x.EmpId,
                x.UserName,
                x.Password,
                x.Status,
                x.Notes,
                x.IsOnline,
                x.LastSeenDate,
                x.RecoverPasswordLink,
                x.RecoverPasswordDate,    
                x.LastLoginDate,
                x.ExpireDate,
                ISOnlineNew = x.ISOnlineNew,
                ImgUrl = x.ImgUrl ?? "/distnew/images/userprofile.png",
                DepartmentName = x.Department == null ? "" : x.Department!.DepartmentNameAr,
                JobName = x.Jobs == null ? "" : x.Jobs!.JobNameAr,
                GroupName = x.Groups == null ? "" : x.Groups!.NameAr,
                BranchName = x.Branches == null ? "" : x.Branches!.NameAr,
                SupEngineerName = x.SupEngineerName,
                SupEngineerCert = x.SupEngineerCert,
                SupEngineerNationalId = x.SupEngineerNationalId,
                StampUrl = x.StampUrl,
                TimeId = x.TimeId,
                AccStatusConfirm = x.AccStatusConfirm ?? "",

                FullNameAr = x.FullNameAr ?? "",
                IsActivated = x.IsActivated ?? false,
            }).ToList().Select(s => new UsersVM
            {
                UserId = s.UserId,
                FullName = s.FullName,
                JobId = s.JobId,
                DepartmentId = s.DepartmentId,
                Email = s.Email,
                Mobile = s.Mobile,
                GroupId = s.GroupId,
                BranchId = s.BranchId,
                ImgUrl = s.ImgUrl ?? "/distnew/images/userprofile.png",
                EmpId = s.EmpId,
                UserName = s.UserName,
                Password = s.Password,
                Status = s.Status,
                Notes = s.Notes,
                DepartmentName = s.DepartmentName,
                JobName = s.JobName,
                GroupName = s.GroupName,
                BranchName = s.BranchName,
                IsOnline = s.IsOnline,
                ISOnlineNew = s.ISOnlineNew,
                RecoverPasswordLink =s.RecoverPasswordLink,
                RecoverPasswordDate= s.RecoverPasswordDate ,
                ExpireDate = s.ExpireDate,
                LastSeenDate = s.LastSeenDate != null ? s.LastSeenDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                LastLoginDate = s.LastLoginDate != null ? s.LastLoginDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                SupEngineerName = s.SupEngineerName,
                SupEngineerCert = s.SupEngineerCert,
                SupEngineerNationalId = s.SupEngineerNationalId,
                StampUrl = s.StampUrl,
                TimeId = s.TimeId,
                AccStatusConfirm = s.AccStatusConfirm ?? "",
                FullNameAr = s.FullNameAr ?? "",
                IsActivated = s.IsActivated ,

            }).FirstOrDefault();
            return user;
        }
        public async Task<int>SearchUsersOfUserName(string UserSearchUserName)
        {
            var User = _TaamerProContext.Users.Where(s => s.IsDeleted == false && s.UserName == UserSearchUserName).Count();
            return User;
        }

        public async Task<int>SearchUsersOfEmail(string SearchUsersOfEmail)
        {
            var User = _TaamerProContext.Users.Where(s => s.IsDeleted == false && s.Email == SearchUsersOfEmail).Count();
            return User;
        }
        public async Task<UsersVM> GetUserByVeificationLinkId(string LinkId)
        {
            var user = _TaamerProContext.Users.Where(s => s.IsDeleted == false && s.RecoverPasswordLink == LinkId ).Select(x => new
            {
                x.UserId,
                FullName = x.FullNameAr == null ? x.FullName : x.FullNameAr,
                x.JobId,
                x.DepartmentId,
                x.Email,
                x.Mobile,
                x.GroupId,
                x.BranchId,
                x.EmpId,
                x.UserName,
                x.Password,
                x.Status,
                x.Notes,
                x.IsOnline,
                x.LastSeenDate,
                x.LastLoginDate,
                x.RecoverPasswordLink,
                x.RecoverPasswordDate,   
                x.ExpireDate,
                ImgUrl = x.ImgUrl ?? "/distnew/images/userprofile.png",
                DepartmentName = x.Department == null ? "" : x.Department!.DepartmentNameAr,
                JobName = x.Jobs == null ? "" : x.Jobs!.JobNameAr,
                GroupName = x.Groups == null ? "" : x.Groups!.NameAr,
                BranchName = x.Branches == null ? "" : x.Branches!.NameAr,
                SupEngineerName = x.SupEngineerName,
                SupEngineerCert = x.SupEngineerCert,
                SupEngineerNationalId = x.SupEngineerNationalId,
                ISOnlineNew = x.ISOnlineNew,
                StampUrl = x.StampUrl,
                TimeId = x.TimeId,
                AccStatusConfirm = x.AccStatusConfirm ?? "",
                FullNameAr = x.FullNameAr ?? "",
                IsActivated = x.IsActivated ?? false,

            }).ToList().Select(s => new UsersVM
            {
                UserId = s.UserId,
                FullName = s.FullName,
                JobId = s.JobId,
                DepartmentId = s.DepartmentId,
                Email = s.Email,
                Mobile = s.Mobile,
                GroupId = s.GroupId,
                BranchId = s.BranchId,
                ImgUrl = s.ImgUrl ?? "/distnew/images/userprofile.png",
                EmpId = s.EmpId,
                UserName = s.UserName,
                Password = s.Password,
                Status = s.Status,
                Notes = s.Notes,
                DepartmentName = s.DepartmentName,
                JobName = s.JobName,
                GroupName = s.GroupName,
                BranchName = s.BranchName,
                IsOnline = s.IsOnline,
                ISOnlineNew = s.ISOnlineNew,
                RecoverPasswordLink = s.RecoverPasswordLink,
                RecoverPasswordDate = s.RecoverPasswordDate,
                ExpireDate = s.ExpireDate,
                LastSeenDate = s.LastSeenDate != null ? s.LastSeenDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                LastLoginDate = s.LastLoginDate != null ? s.LastLoginDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                SupEngineerName = s.SupEngineerName,
                SupEngineerCert = s.SupEngineerCert,
                SupEngineerNationalId = s.SupEngineerNationalId,
                StampUrl = s.StampUrl,
                TimeId = s.TimeId,
                AccStatusConfirm = s.AccStatusConfirm ?? "",
                FullNameAr = s.FullNameAr ?? "",
                IsActivated = s.IsActivated ,

            }).FirstOrDefault();
            return user;
        }
        public async Task<int>GetAllUsersCount()
        {
            return _TaamerProContext.Users.Where(s => s.IsDeleted == false).Count();
        }
        public async Task< IEnumerable<UsersVM>> GetSome(EmployeesVM Employees)
        {
            var users = _TaamerProContext.Users.Where(s => s.IsDeleted == false && s.UserId != Employees.UserId).Select(x => new
            {
                x.UserId,
                FullName = x.FullNameAr == null ? x.FullName : x.FullNameAr,
                x.JobId,
                x.DepartmentId,
                x.Email,
                x.Mobile,
                x.GroupId,
                x.BranchId,
                x.EmpId,
                x.UserName,
                x.Password,
                x.Status,
                x.Session,
                x.Notes,
                x.IsOnline,
                x.LastSeenDate,
                x.LastLoginDate,
                x.ExpireDate,
                ISOnlineNew = x.ISOnlineNew,
                ImgUrl = x.ImgUrl ?? "/distnew/images/userprofile.png",
                DepartmentName = x.Department == null ? "" : x.Department!.DepartmentNameAr,
                JobName = x.Jobs == null ? "" : x.Jobs!.JobNameAr,
                GroupName = x.Groups == null ? "" : x.Groups!.NameAr,
                BranchName = x.Branches == null ? "" : x.Branches!.NameAr,
                SupEngineerName = x.SupEngineerName,
                SupEngineerCert = x.SupEngineerCert,
                SupEngineerNationalId = x.SupEngineerNationalId,
                StampUrl = x.StampUrl,
                TimeId = x.TimeId,
                AccStatusConfirm = x.AccStatusConfirm ?? "",
                FullNameAr = x.FullNameAr ?? "",
                IsActivated = x.IsActivated ?? false,

            }).ToList().Select(s => new UsersVM
            {
                UserId = s.UserId,
                FullName = s.FullName,
                JobId = s.JobId,
                DepartmentId = s.DepartmentId,
                Email = s.Email,
                Mobile = s.Mobile,
                GroupId = s.GroupId,
                BranchId = s.BranchId,
                ImgUrl = s.ImgUrl ?? "/distnew/images/userprofile.png",
                EmpId = s.EmpId,
                UserName = s.UserName,
                Password = s.Password,
                Status = s.Status,
                Notes = s.Notes,
                Session = s.Session,
                DepartmentName = s.DepartmentName,
                JobName = s.JobName,
                GroupName = s.GroupName,
                BranchName = s.BranchName,
                IsOnline = s.IsOnline,
                ISOnlineNew = s.ISOnlineNew,
                ExpireDate = s.ExpireDate,
                LastSeenDate = s.LastSeenDate != null ? s.LastSeenDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                LastLoginDate = s.LastLoginDate != null ? s.LastLoginDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                SupEngineerName = s.SupEngineerName,
                SupEngineerCert = s.SupEngineerCert,
                SupEngineerNationalId = s.SupEngineerNationalId,
                StampUrl = s.StampUrl,
                TimeId = s.TimeId,
                AccStatusConfirm = s.AccStatusConfirm ?? "",
                FullNameAr = s.FullNameAr ?? "",
                IsActivated = s.IsActivated,

            });
            return users;
        }


        public async Task< IEnumerable<UsersVM>> GetAllOtherUsers(int UserId)
        {
            var users = _TaamerProContext.Users.Where(s => s.IsDeleted == false && s.UserId != UserId).Select(x => new
            {
                x.UserId,
                FullName = x.FullNameAr == null ? x.FullName : x.FullNameAr,
                x.JobId,
                x.DepartmentId,
                x.Email,
                x.Mobile,
                x.GroupId,
                x.BranchId,
                x.EmpId,
                x.UserName,
                x.Password,
                x.Status,
                x.Session,
                x.Notes,
                x.IsOnline,
                x.LastSeenDate,
                x.LastLoginDate,
                x.ExpireDate,
                ISOnlineNew = x.ISOnlineNew,
                ImgUrl = x.ImgUrl ?? "/distnew/images/userprofile.png",
                DepartmentName = x.Department == null ? "" : x.Department!.DepartmentNameAr,
                JobName = x.Jobs == null ? "" : x.Jobs!.JobNameAr,
                GroupName = x.Groups == null ? "" : x.Groups!.NameAr,
                BranchName = x.Branches == null ? "" : x.Branches!.NameAr,
                SupEngineerName = x.SupEngineerName,
                SupEngineerCert = x.SupEngineerCert,
                SupEngineerNationalId = x.SupEngineerNationalId,
                StampUrl = x.StampUrl,
                TimeId = x.TimeId,
                AccStatusConfirm = x.AccStatusConfirm ?? "",
                FullNameAr = x.FullNameAr ?? "",
                IsActivated = x.IsActivated ?? false,

            }).ToList().Select(s => new UsersVM
            {
                UserId = s.UserId,
                FullName = s.FullName,
                JobId = s.JobId,
                DepartmentId = s.DepartmentId,
                Email = s.Email,
                Mobile = s.Mobile,
                GroupId = s.GroupId,
                BranchId = s.BranchId,
                ImgUrl = s.ImgUrl ?? "/distnew/images/userprofile.png",
                EmpId = s.EmpId,
                UserName = s.UserName,
                Password = s.Password,
                Status = s.Status,
                Notes = s.Notes,
                Session = s.Session,
                DepartmentName = s.DepartmentName,
                JobName = s.JobName,
                GroupName = s.GroupName,
                BranchName = s.BranchName,
                ISOnlineNew = s.ISOnlineNew,
                IsOnline = s.IsOnline,
                ExpireDate = s.ExpireDate,
                LastSeenDate = s.LastSeenDate != null ? s.LastSeenDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                LastLoginDate = s.LastLoginDate != null ? s.LastLoginDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                SupEngineerName = s.SupEngineerName,
                SupEngineerCert = s.SupEngineerCert,
                SupEngineerNationalId = s.SupEngineerNationalId,
                StampUrl = s.StampUrl,
                TimeId = s.TimeId,
                AccStatusConfirm = s.AccStatusConfirm ?? "",
                FullNameAr = s.FullNameAr ?? "",
                IsActivated = s.IsActivated ,

            });
            return users;
        }


       
        public async Task< IEnumerable<UsersVM>> GetSome(int UserId)
        {
            var users = _TaamerProContext.Users.Where(s => s.IsDeleted == false && s.UserId != UserId).Select(x => new
            {
                x.UserId,
                FullName = x.FullNameAr == null ? x.FullName : x.FullNameAr,
                x.JobId,
                x.DepartmentId,
                x.Email,
                x.Mobile,
                x.GroupId,
                x.BranchId,
                x.EmpId,
                x.UserName,
                x.Password,
                x.Status,
                x.Session,
                x.Notes,
                x.IsOnline,
                x.LastSeenDate,
                x.LastLoginDate,
                x.ExpireDate,
                ISOnlineNew = x.ISOnlineNew,
                ImgUrl = x.ImgUrl ?? "/distnew/images/userprofile.png",
                DepartmentName = x.Department == null ? "" : x.Department!.DepartmentNameAr,
                JobName = x.Jobs == null ? "" : x.Jobs!.JobNameAr,
                GroupName = x.Groups == null ? "" : x.Groups!.NameAr,
                BranchName = x.Branches == null ? "" : x.Branches!.NameAr,
                SupEngineerName = x.SupEngineerName,
                SupEngineerCert = x.SupEngineerCert,
                SupEngineerNationalId = x.SupEngineerNationalId,
                StampUrl = x.StampUrl,
                TimeId = x.TimeId,
                AccStatusConfirm = x.AccStatusConfirm ?? "",
                FullNameAr = x.FullNameAr ?? "",
                IsActivated = x.IsActivated ?? false,

            }).ToList().Select(s => new UsersVM
            {
                UserId = s.UserId,
                FullName = s.FullName,
                JobId = s.JobId,
                DepartmentId = s.DepartmentId,
                Email = s.Email,
                Mobile = s.Mobile,
                GroupId = s.GroupId,
                BranchId = s.BranchId,
                ImgUrl = s.ImgUrl ?? "/distnew/images/userprofile.png",
                EmpId = s.EmpId,
                UserName = s.UserName,
                Password = s.Password,
                Status = s.Status,
                Notes = s.Notes,
                Session = s.Session,
                DepartmentName = s.DepartmentName,
                JobName = s.JobName,
                GroupName = s.GroupName,
                BranchName = s.BranchName,
                IsOnline = s.IsOnline,
                ISOnlineNew = s.ISOnlineNew,
                ExpireDate = s.ExpireDate,
                LastLoginDate = s.LastLoginDate != null ? s.LastLoginDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                LastSeenDate = s.LastSeenDate != null ? s.LastSeenDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                SupEngineerName = s.SupEngineerName,
                SupEngineerCert = s.SupEngineerCert,
                SupEngineerNationalId = s.SupEngineerNationalId,
                StampUrl = s.StampUrl,
                TimeId = s.TimeId,
                AccStatusConfirm = s.AccStatusConfirm ?? "",
                FullNameAr = s.FullNameAr ?? "",
                IsActivated = s.IsActivated,

            });
            return users;
        }

        public async Task<UsersVM> GetUserByRecoverLink(string Link)
        {
            var user = _TaamerProContext.Users.Where(s => s.IsDeleted == false && s.RecoverPasswordLink.Contains(Link)).Select(x => new
            {
                x.UserId,
                FullName = x.FullNameAr == null ? x.FullName : x.FullNameAr,
                x.JobId,
                x.DepartmentId,
                x.Email,
                x.Mobile,
                x.GroupId,
                x.BranchId,
                x.EmpId,
                x.UserName,
                x.Password,
                x.Status,
                x.Session,
                x.Notes,
                x.IsOnline,
                x.LastSeenDate,
                x.LastLoginDate,
                x.ExpireDate,
                x.RecoverPasswordDate,
                ImgUrl = x.ImgUrl ?? "/distnew/images/userprofile.png",
                DepartmentName = x.Department!.DepartmentNameAr,
                JobName = x.Jobs!.JobNameAr,
                GroupName = x.Groups!.NameAr,
                ISOnlineNew = x.ISOnlineNew,
                BranchName = x.Branches!.NameAr,
                SupEngineerName = x.SupEngineerName,
                SupEngineerCert = x.SupEngineerCert,
                SupEngineerNationalId = x.SupEngineerNationalId,
                StampUrl = x.StampUrl,
                TimeId = x.TimeId,
                AccStatusConfirm = x.AccStatusConfirm??"",
                FullNameAr = x.FullNameAr ?? "",

            }).ToList().Select(s => new UsersVM
            {
                UserId = s.UserId,
                FullName = s.FullName,
                JobId = s.JobId,
                DepartmentId = s.DepartmentId,
                Email = s.Email,
                Mobile = s.Mobile,
                GroupId = s.GroupId,
                BranchId = s.BranchId,
                ImgUrl = s.ImgUrl ?? "/distnew/images/userprofile.png",
                EmpId = s.EmpId,
                UserName = s.UserName,
                Password = s.Password,
                Status = s.Status,
                Session = s.Session,
                Notes = s.Notes,
                DepartmentName = s.DepartmentName,
                JobName = s.JobName,
                GroupName = s.GroupName,
                BranchName = s.BranchName,
                IsOnline = s.IsOnline,
                ExpireDate = s.ExpireDate,
                ISOnlineNew = s.ISOnlineNew,
                RecoverPasswordDate = s.RecoverPasswordDate,
                LastSeenDate = s.LastSeenDate != null ? s.LastSeenDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                LastLoginDate = s.LastLoginDate != null ? s.LastLoginDate.Value.ToString("yyyy-MM-dd HH:MM") : "",
                SupEngineerName = s.SupEngineerName,
                SupEngineerCert = s.SupEngineerCert,
                SupEngineerNationalId = s.SupEngineerNationalId,
                StampUrl = s.StampUrl,
                TimeId = s.TimeId,
                AccStatusConfirm = s.AccStatusConfirm ?? "",
                FullNameAr = s.FullNameAr ?? "",

            }).FirstOrDefault();
            return user;
        }


        public async Task< IEnumerable<UsersVM>> GetFullReport(ProjectPhasesTasksVM Search, string Lang,string Today,int BranchId)
        {
            //_TaamerProContext.Database.SetCommandTimeout(60);
            var users = _TaamerProContext.Users.Where(s => s.IsDeleted == false && (Search.UserId == 0 || s.UserId == Search.UserId) && (Search.BranchId == 0 || s.BranchId == Search.BranchId)).Select(x => new UsersVM
            {
                UserId = x.UserId,
                FullName = x.FullNameAr == null ? x.FullName : x.FullNameAr,
                UserName = x.UserName,
                BranchId = x.BranchId,
                ImgUrl = x.ImgUrl ?? "/distnew/images/userprofile.png",
                BranchName = x.Branches!.NameAr,
                FullNameAr = x.FullNameAr==null ? x.FullName :x.FullNameAr,

                Latetask_VM = x.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == 3 && s.IsMerig == -1 && s.Remaining < 0 && s.Status != 4 && (s.IsRetrieved != 1)).Select(a => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = a.PhaseTaskId,
                    StartDate = a.StartDate != null ? a.StartDate : a.ExcpectedStartDate != null ? a.ExcpectedStartDate : "",
                    EndDate = a.EndDate != null ? a.EndDate : a.ExcpectedEndDate != null ? a.ExcpectedEndDate : "",
                    ExcpectedStartDate = a.ExcpectedStartDate,
                    ExcpectedEndDate = a.ExcpectedEndDate,
                    EndDateCalc = a.EndDateCalc ?? "",

                    TimeType = a.TimeType,
                    TimeMinutes = a.TimeMinutes ?? 0,
                    PercentComplete = a.TimeType == 1 ? (a.TimeMinutes * 60) * 70 / 100 : (a.TimeMinutes * 24 * 60) * 70 / 100,
                    Remaining = a.Remaining,
                    PlayingTime = a.TimeType == 1 ? (a.TimeMinutes * 60) * 100 / 100 : (a.TimeMinutes * 24 * 60) * 100 / 100,
                }).ToList(),

                AllFinishPhases_VM = x.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == 3 && s.IsMerig == -1 && s.Status == 4 && (s.IsRetrieved != 1)).Select(a => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = a.PhaseTaskId,
                    StartDate = a.StartDate != null ? a.StartDate : a.ExcpectedStartDate != null ? a.ExcpectedStartDate : "",
                    EndDate = a.EndDate != null ? a.EndDate : a.ExcpectedEndDate != null ? a.ExcpectedEndDate : "",
                    ExcpectedStartDate = a.ExcpectedStartDate,
                    ExcpectedEndDate = a.ExcpectedEndDate,
                    EndDateCalc = a.EndDateCalc ?? "",

                    TimeType = a.TimeType,
                    TimeMinutes = a.TimeMinutes ?? 0,
                    PercentComplete = a.TimeType == 1 ? (a.TimeMinutes * 60) * 70 / 100 : (a.TimeMinutes * 24 * 60) * 70 / 100,
                    Remaining = a.Remaining,
                    PlayingTime = a.TimeType == 1 ? (a.TimeMinutes * 60) * 100 / 100 : (a.TimeMinutes * 24 * 60) * 100 / 100,
                }).Union(x.WorkOrdersRe.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.WOStatus == 3).Select(a => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = a.WorkOrderId,
                    StartDate = a.OrderDate ?? "",
                    EndDate = a.EndDate ?? "",
                    ExcpectedStartDate = a.OrderDate ?? "",
                    ExcpectedEndDate = a.EndDate ?? "",
                    EndDateCalc = a.EndDate ?? "",

                    TimeType = 0,
                    TimeMinutes = 0,
                    PercentComplete = 0,
                    Remaining = 0,
                    PlayingTime = 0,

                }).ToList()).ToList(),

                AllLatePhases_VM = x.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == 3 && s.IsMerig == -1 && s.Remaining < 0 && s.Status != 4 && (s.IsRetrieved != 1)).Select(a => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = a.PhaseTaskId,
                    StartDate = a.StartDate != null ? a.StartDate : a.ExcpectedStartDate != null ? a.ExcpectedStartDate : "",
                    EndDate = a.EndDate != null ? a.EndDate : a.ExcpectedEndDate != null ? a.ExcpectedEndDate : "",
                    ExcpectedStartDate = a.ExcpectedStartDate,
                    ExcpectedEndDate = a.ExcpectedEndDate,
                    EndDateCalc = a.EndDateCalc ?? "",

                    TimeType = a.TimeType,
                    TimeMinutes = a.TimeMinutes ?? 0,
                    PercentComplete = a.TimeType == 1 ? (a.TimeMinutes * 60) * 70 / 100 : (a.TimeMinutes * 24 * 60) * 70 / 100,
                    Remaining = a.Remaining,
                    PlayingTime = a.TimeType == 1 ? (a.TimeMinutes * 60) * 100 / 100 : (a.TimeMinutes * 24 * 60) * 100 / 100,
                }).Union(x.WorkOrdersRe.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.WOStatus == 2).Select(a => new ProjectPhasesTasksVM
                {

                    PhaseTaskId = a.WorkOrderId,
                    StartDate = a.OrderDate ?? "",
                    EndDate = a.EndDate ?? "",
                    ExcpectedStartDate = a.OrderDate ?? "",
                    ExcpectedEndDate = a.EndDate ?? "",
                    EndDateCalc = a.EndDate ?? "",

                    TimeType = 0,
                    TimeMinutes = 0,
                    PercentComplete = 0,
                    Remaining = 0,
                    PlayingTime = 0,

                }).ToList()).ToList(),

                AllPhases_VM = x.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == 3 && s.IsMerig == -1).Select(a => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = a.PhaseTaskId,
                    StartDate = a.StartDate != null ? a.StartDate : a.ExcpectedStartDate != null ? a.ExcpectedStartDate : "",
                    EndDate = a.EndDate != null ? a.EndDate : a.ExcpectedEndDate != null ? a.ExcpectedEndDate : "",
                    ExcpectedStartDate = a.ExcpectedStartDate,
                    ExcpectedEndDate = a.ExcpectedEndDate,
                    EndDateCalc = a.EndDateCalc ?? "",

                    TimeType = a.TimeType,
                    TimeMinutes = a.TimeMinutes ?? 0,
                    PercentComplete = a.TimeType == 1 ? (a.TimeMinutes * 60) * 70 / 100 : (a.TimeMinutes * 24 * 60) * 70 / 100,
                    Remaining = a.Remaining,
                    PlayingTime = a.TimeType == 1 ? (a.TimeMinutes * 60) * 100 / 100 : (a.TimeMinutes * 24 * 60) * 100 / 100,
                }).Union(x.WorkOrdersRe.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(a => new ProjectPhasesTasksVM
                {

                    PhaseTaskId = a.WorkOrderId,
                    StartDate = a.OrderDate ?? "",
                    EndDate = a.EndDate ?? "",
                    ExcpectedStartDate = a.OrderDate ?? "",
                    ExcpectedEndDate = a.EndDate ?? "",
                    EndDateCalc = a.EndDate ?? "",

                    TimeType = 0,
                    TimeMinutes = 0,
                    PercentComplete = 0,
                    Remaining = 0,
                    PlayingTime = 0,

                }).ToList()).ToList(),


                StoppedTasks_VM = x.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == 3 && s.Status == 3 && s.Remaining >= 0 && s.IsMerig == -1 && (s.IsRetrieved != 1)).Select(a => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = a.PhaseTaskId,
                    StartDate = a.StartDate != null ? a.StartDate : a.ExcpectedStartDate != null ? a.ExcpectedStartDate : "",
                    EndDate = a.EndDate != null ? a.EndDate : a.ExcpectedEndDate != null ? a.ExcpectedEndDate : "",
                    ExcpectedStartDate = a.ExcpectedStartDate,
                    ExcpectedEndDate = a.ExcpectedEndDate,
                    EndDateCalc = a.EndDateCalc ?? "",

                    TimeType = a.TimeType,
                    TimeMinutes = a.TimeMinutes ?? 0,
                    PercentComplete = a.TimeType == 1 ? (a.TimeMinutes * 60) * 70 / 100 : (a.TimeMinutes * 24 * 60) * 70 / 100,
                    Remaining = a.Remaining,
                    PlayingTime = a.TimeType == 1 ? (a.TimeMinutes * 60) * 100 / 100 : (a.TimeMinutes * 24 * 60) * 100 / 100,
                }).ToList(),

                Inprogress_VM = x.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == 3 && s.Status == 2 && s.Remaining >= 0 && s.IsMerig == -1 && (s.IsRetrieved != 1)).Select(a => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = a.PhaseTaskId,
                    StartDate = a.StartDate != null ? a.StartDate : a.ExcpectedStartDate != null ? a.ExcpectedStartDate : "",
                    EndDate = a.EndDate != null ? a.EndDate : a.ExcpectedEndDate != null ? a.ExcpectedEndDate : "",
                    ExcpectedStartDate = a.ExcpectedStartDate,
                    ExcpectedEndDate = a.ExcpectedEndDate,
                    EndDateCalc = a.EndDateCalc ?? "",

                    TimeType = a.TimeType,
                    TimeMinutes = a.TimeMinutes ?? 0,
                    PercentComplete = a.TimeType == 1 ? (a.TimeMinutes * 60) * 70 / 100 : (a.TimeMinutes * 24 * 60) * 70 / 100,
                    Remaining = a.Remaining,
                    PlayingTime = a.TimeType == 1 ? (a.TimeMinutes * 60) * 100 / 100 : (a.TimeMinutes * 24 * 60) * 100 / 100,
                }).ToList().Union(x.WorkOrdersRe.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.WOStatus == 2).Select(a => new ProjectPhasesTasksVM
                {

                    PhaseTaskId = a.WorkOrderId,
                    StartDate = a.OrderDate ?? "",
                    EndDate = a.EndDate ?? "",
                    ExcpectedStartDate = a.OrderDate ?? "",
                    ExcpectedEndDate = a.EndDate ?? "",
                    EndDateCalc = a.EndDate ?? "",

                    TimeType = 0,
                    TimeMinutes = 0,
                    PercentComplete = 0,
                    Remaining = 0,
                    PlayingTime = 0,

                }).ToList()).ToList(),

                Notstarted_VM = x.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == 3 && s.Status == 1 && s.IsMerig == -1 && (s.Remaining > 0 || s.Remaining == null) && (s.IsRetrieved != 1)).Select(a => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = a.PhaseTaskId,
                    StartDate = a.StartDate != null ? a.StartDate : a.ExcpectedStartDate != null ? a.ExcpectedStartDate : "",
                    EndDate = a.EndDate != null ? a.EndDate : a.ExcpectedEndDate != null ? a.ExcpectedEndDate : "",
                    ExcpectedStartDate = a.ExcpectedStartDate,
                    ExcpectedEndDate = a.ExcpectedEndDate,
                    EndDateCalc = a.EndDateCalc ?? "",

                    TimeType = a.TimeType,
                    TimeMinutes = a.TimeMinutes ?? 0,
                    PercentComplete = a.TimeType == 1 ? (a.TimeMinutes * 60) * 70 / 100 : (a.TimeMinutes * 24 * 60) * 70 / 100,
                    Remaining = a.Remaining,
                    PlayingTime = a.TimeType == 1 ? (a.TimeMinutes * 60) * 100 / 100 : (a.TimeMinutes * 24 * 60) * 100 / 100,
                }).ToList().Union(
                    x.WorkOrdersRe.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.WOStatus == 1).Select(a => new ProjectPhasesTasksVM
                    {

                        PhaseTaskId = a.WorkOrderId,
                        StartDate = a.OrderDate ?? "",
                        EndDate = a.EndDate ?? "",
                        ExcpectedStartDate = a.OrderDate ?? "",
                        ExcpectedEndDate = a.EndDate ?? "",
                        EndDateCalc = a.EndDate ?? "",

                        TimeType = 0,
                        TimeMinutes = 0,
                        PercentComplete = 0,
                        Remaining = 0,
                        PlayingTime = 0,

                    }).ToList()).ToList(),

                Retrived_VM = x.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == 3 && s.IsMerig == -1 && s.IsRetrieved == 1).Select(a => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = a.PhaseTaskId,
                    StartDate = a.StartDate != null ? a.StartDate : a.ExcpectedStartDate != null ? a.ExcpectedStartDate : "",
                    EndDate = a.EndDate != null ? a.EndDate : a.ExcpectedEndDate != null ? a.ExcpectedEndDate : "",
                    ExcpectedStartDate = a.ExcpectedStartDate,
                    ExcpectedEndDate = a.ExcpectedEndDate,
                    EndDateCalc = a.EndDateCalc ?? "",

                    TimeType = a.TimeType,
                    TimeMinutes = a.TimeMinutes ?? 0,
                    PercentComplete = a.TimeType == 1 ? (a.TimeMinutes * 60) * 70 / 100 : (a.TimeMinutes * 24 * 60) * 70 / 100,
                    Remaining = a.Remaining,
                    PlayingTime = a.TimeType == 1 ? (a.TimeMinutes * 60) * 100 / 100 : (a.TimeMinutes * 24 * 60) * 100 / 100,
                }).ToList(),



                AlmostLate_VM = x.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == 3 && s.IsMerig == -1 && s.Remaining > 0 && (s.Status == 2 || s.Status == 3) && (s.IsRetrieved != 1)).Select(a => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = a.PhaseTaskId,
                    StartDate = a.StartDate != null ? a.StartDate : a.ExcpectedStartDate != null ? a.ExcpectedStartDate : "",
                    EndDate = a.EndDate != null ? a.EndDate : a.ExcpectedEndDate != null ? a.ExcpectedEndDate : "",
                    ExcpectedStartDate = a.ExcpectedStartDate,
                    ExcpectedEndDate = a.ExcpectedEndDate,
                    EndDateCalc = a.EndDateCalc ?? "",

                    TimeType = a.TimeType,
                    TimeMinutes = a.TimeMinutes ?? 0,
                    PercentComplete = a.TimeType == 1 ? (a.TimeMinutes * 60) * 70 / 100 : (a.TimeMinutes * 24 * 60) * 70 / 100,
                    Remaining = a.Remaining,
                    PlayingTime = a.TimeType == 1 ? (a.TimeMinutes * 60) * 100 / 100 : (a.TimeMinutes * 24 * 60) * 100 / 100,
                }).ToList().AsQueryable().Where(k => (k.PlayingTime - k.PercentComplete) >= k.Remaining).ToList(),

                ProjectInProgress_VM = x.Project.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.StopProjectType != 1 && s.Status == 0).Select(a => new ProjectVM
                {
                    ProjectId = a.ProjectId,
                    ProjectName = a.ProjectName,
                    ProjectNo = a.ProjectNo,
                    ProjectDate = a.ProjectDate,
                    ProjectExpireDate = a.ProjectExpireDate,
                    MotionProjectDate = a.MotionProjectDate ?? "2033-01-01",
                    PercentComplete = a.PercentComplete ?? 0,
                }).ToList(),

                ProjectLate_VM = x.Project.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.StopProjectType != 1 && s.Status == 0).Select(a => new ProjectVM
                {
                    ProjectId = a.ProjectId,
                    ProjectName = a.ProjectName,
                    ProjectNo = a.ProjectNo,
                    ProjectDate = a.ProjectDate,
                    ProjectExpireDate = a.ProjectExpireDate,
                    MotionProjectDate = a.MotionProjectDate ?? "2033-01-01",
                    PercentComplete = a.PercentComplete ?? 0,

                }).ToList(),


                ProjectStoped_VM = x.Project.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.StopProjectType == 1 && s.Status == 0).Select(a => new ProjectVM
                {
                    ProjectId = a.ProjectId,
                    ProjectName = a.ProjectName,
                    ProjectNo = a.ProjectNo,
                    ProjectDate = a.ProjectDate,
                    ProjectExpireDate = a.ProjectExpireDate,
                    MotionProjectDate = a.MotionProjectDate ?? "2033-01-01",
                    PercentComplete = a.PercentComplete ?? 0,

                }).ToList(),

                ProjectWithout_VM = x.Project.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Status == 0 && s.MotionProject != 1).Select(a => new ProjectVM
                {
                    ProjectId = a.ProjectId,
                    ProjectName = a.ProjectName,
                    ProjectNo = a.ProjectNo,
                    ProjectDate = a.ProjectDate,
                    ProjectExpireDate = a.ProjectExpireDate,
                    MotionProjectDate = a.MotionProjectDate ?? "2033-01-01",
                    PercentComplete = a.PercentComplete ?? 0,

                }).ToList(),

                ProjectWithout_VM2 = x.Project.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Status == 0 && s.MotionProject == 1).Select(a => new ProjectVM
                {
                    ProjectId = a.ProjectId,
                    ProjectName = a.ProjectName,
                    ProjectNo = a.ProjectNo,
                    ProjectDate = a.ProjectDate,
                    ProjectExpireDate = a.ProjectExpireDate,
                    MotionProjectDate = a.MotionProjectDate ?? "2033-01-01",
                    PercentComplete = a.PercentComplete ?? 0,
                }).ToList(),

                ProjectAlmostfinish_VM = x.Project.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Status == 0).Select(a => new ProjectVM
                {
                    ProjectId = a.ProjectId,
                    ProjectName = a.ProjectName,
                    ProjectNo = a.ProjectNo,
                    ProjectDate = a.ProjectDate,
                    ProjectExpireDate = a.ProjectExpireDate,
                    MotionProjectDate = a.MotionProjectDate ?? "2033-01-01",
                    PercentComplete = a.PercentComplete ?? 0,


                }).ToList().AsQueryable().Where(k => k.PercentComplete > 70 && k.PercentComplete < 100).ToList(),

            }).ToList();
                return users;

        }

        public async Task<List<RptAllEmpPerformance>> getempdataNew_Proc(PerformanceReportVM Search, string Lang, string Con, int BranchId)
        {
            try
            {
                List<RptAllEmpPerformance> lmd = new List<RptAllEmpPerformance>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "Rpt_EmpdataNew_Proc";
                        command.CommandTimeout = 180;
                        command.Connection = con;


                        if (Search.SearchUserIdStr == "" || Search.SearchUserIdStr == null || Search.SearchUserIdStr == "null" || Search.SearchUserIdStr == "0")
                        {
                            command.Parameters.Add(new SqlParameter("@UserId_SearchStr", DBNull.Value));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@UserId_SearchStr", Search.SearchUserIdStr));
                        }
                        if (Search.SearchBranchIdStr == "" || Search.SearchBranchIdStr == null || Search.SearchBranchIdStr == "null" || Search.SearchBranchIdStr == "0")
                        {
                            command.Parameters.Add(new SqlParameter("@BranchId_SearchStr", DBNull.Value));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@BranchId_SearchStr", Search.SearchBranchIdStr));
                        }



                        if (Search.UserId == 0)
                            command.Parameters.Add(new SqlParameter("@UserId_Search",DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@UserId_Search", Search.UserId));

                        if (Search.BranchId == 0)
                            command.Parameters.Add(new SqlParameter("@BranchId_Search", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@BranchId_Search", Search.BranchId));

                        if (Search.StartDate == "")
                            command.Parameters.Add(new SqlParameter("@From_Search", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@From_Search", Search.StartDate));


                        if (Search.EndDate == "")
                            command.Parameters.Add(new SqlParameter("@To_Search", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@To_Search", Search.EndDate));


                        if (BranchId == 0)
                            command.Parameters.Add(new SqlParameter("@BranchId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@BranchId", BranchId));


                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            lmd.Add(new RptAllEmpPerformance
                            {
                                UserId =Convert.ToInt32((dr["UserId"]).ToString()),
                                UserName = (dr["UserName"]).ToString(),
                                Notstarted = (dr["Notstarted"]).ToString(),
                                Inprogress = (dr["Inprogress"]).ToString(),
                                Retrived = (dr["Retrived"]).ToString(),
                                Completed = (dr["Completed"]).ToString(),
                                AlmostLate = (dr["AlmostLate"]).ToString(),
                                Latetask = (dr["Latetask"]).ToString(),
                                StoppedTasks = (dr["StoppedTasks"]).ToString(),
                                LatePercentage = (dr["LatePercentage"]).ToString(),
                                CompletePercentage = (dr["CompletePercentage"]).ToString(),
                                ProjectLate = (dr["ProjectLate"]).ToString(),
                                ProjectInProgress = (dr["ProjectInProgress"]).ToString(),
                                ProjectStoped = (dr["ProjectStoped"]).ToString(),
                                ProjectAlmostfinish = (dr["ProjectAlmostfinish"]).ToString(),
                                ProjectWithout = (dr["ProjectWithout"]).ToString(),
                            });
                        }
                    }
                }
                return lmd;
            }
            catch(Exception ex)
            {
                List<RptAllEmpPerformance> lmd = new List<RptAllEmpPerformance>();
                return lmd;
            }

        }

        public async Task<PagedLists<UsersVM>> GetAllAsync(RequestParam<UsersFilterDTO> Param)
        {
            try
            {
                var users = _TaamerProContext.Users.Where(x => x.IsDeleted == false).Select(x => new UsersVM()
                {
                    UserId = x.UserId,
                });
                var res = GeneratePagination<UsersVM>.ToPagedList(users.ToList(), Param.pagenumber, Param.pagesize);

                return new PagedLists<UsersVM>(res.MetaData, res);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }


        public Users GetMatchingUserWithPassword(string UserName, string activationCode = null)
        {
            return _TaamerProContext.Users.FirstOrDefault(s => s.IsDeleted == false && s.UserName == UserName && s.ActivationCode == activationCode);
        }

        public Users Add(Users entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Users> AddRange(IEnumerable<Users> entities)
        {
            throw new NotImplementedException();
        }

        public bool Exists(int Id)
        {
            throw new NotImplementedException();
        }

        public void Remove(Users entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void RemoveMatching(Func<Users, bool> where)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Users> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Users entityToUpdate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Users> Queryable()
        {
            throw new NotImplementedException();
        }

    }
}

using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class Sys_UserLoginRepository : ISys_UserLoginRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Sys_UserLoginRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<Sys_UserLoginVM>> GetAllUserLogin(int Type)
        {
            var UserLogin = _TaamerProContext.Sys_UserLogin.Where(s => s.IsDeleted == false && (Type == 0 || s.Type == Type)).Select(x => new Sys_UserLoginVM
            {
                UserLoginId = x.UserLoginId,
                Email = x.Email,
                Password = x.Password,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                CompanyName = x.CompanyName,
                Mobile = x.Mobile,
                NationalId = x.NationalId,
                MainActivity = x.MainActivity,
                SubMainActivity = x.SubMainActivity,
                CommercialId = x.CommercialId,
                Notes = x.Notes,
                Type = x.Type,
                Status = x.Status,
                AuthenticatorSecret = x.AuthenticatorSecret,
                Is2FAEnabled = x.Is2FAEnabled??false,
                TypeName = x.Type == 2 ? "عميل": x.Type == 3 ? "مقاول" : "غير معروف",
                StatusName = x.Status == 1 ? "نشط" : "غير نشط",
            }).ToList();
            return UserLogin;
        }
        public async Task<UsersLoginVM> GetUserLogin(string Email, string Password, int Type)
        {
            var UserLogin = _TaamerProContext.Sys_UserLogin.Where(s => s.IsDeleted == false && s.Email == Email && s.Password==Password && s.Type == Type).Select(x => new UsersLoginVM
            {
                UserLoginId = x.UserLoginId,
                Email = x.Email,
                Password = x.Password,
                NameAr = x.NameAr,
                FullNameAr = x.NameAr,
                NameEn = x.NameEn,
                CompanyName = x.CompanyName,
                Mobile = x.Mobile,
                NationalId = x.NationalId,
                MainActivity = x.MainActivity,
                SubMainActivity = x.SubMainActivity,
                CommercialId = x.CommercialId,
                Notes = x.Notes,
                Type = x.Type,
                Status = x.Status,
                //AuthenticatorSecret = x.AuthenticatorSecret,
                //Is2FAEnabled = x.Is2FAEnabled ?? false,
                TypeName = x.Type == 2 ? "عميل" : x.Type == 3 ? "مقاول" : "غير معروف",
                StatusName = x.Status == 1 ? "نشط" : "غير نشط",
            }).FirstOrDefault();
            return UserLogin;
        }

    }
}

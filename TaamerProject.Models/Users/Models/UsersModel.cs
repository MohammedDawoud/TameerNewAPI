using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class UsersModel
    {
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public int JobId { get; set; }
        public int? DepartmentId { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public int GroupId { get; set; }
        public int BranchId { get; set; }
        public string ImgUrl { get; set; }
        public string? Notes { get; set; }
        public int EmpId { get; set; }
        public bool IsAdmin { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string ActivationCode { get; set; }
        public string VisualCode { get; set; }
        public int Status { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastSeenDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public int Session { get; set; }
        public string RecoverPasswordLink { get; set; }
        public DateTime RecoverPasswordDate { get; set; }
        public string ExpireDate { get; set; }
        public int AddUser { get; set; }
        public int UpdateUser { get; set; }
        public int DeleteUser { get; set; }
        public DateTime? AddDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime LastLogOutDate { get; set; }
        public bool ISOnlineNew { get; set; }
        public DateTime ActiveTime { get; set; }
        public string SupEngineerName { get; set; }
        public string SupEngineerNationalId { get; set; }
        public string SupEngineerCert { get; set; }
        public string StampUrl { get; set; }
        public int TimeId { get; set; }
        public string AccStatusConfirm { get; set; }
        public string? FullNameAr { get; set; }
        public DateTime ResetDate { get; set; }
        public string EncryptedCode { get; set; }
        public bool IsActivated { get; set; }
        public string? DeviceTokenId { get; set; }
        public int? DeviceType { get; set; }
        public int AppearWelcome { get; set; }
        public string QrCodeUrl { get; set; }
        public int AppearInInvoicePrint { get; set; }
        public string DeviceId { get; set; }
        public int IsAppMailDownloadSent { get; set; }

    }
}

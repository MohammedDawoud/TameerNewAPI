
using System.ComponentModel.DataAnnotations.Schema;

namespace TaamerProject.Models
{
    public class Users : Auditable
    {
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public int? JobId { get; set; }
        public int? DepartmentId { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public int? GroupId { get; set; }
        public int? BranchId { get; set; }
        public string? ImgUrl { get; set; }
        public int? EmpId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int? Status { get; set; }
        public string? Notes { get; set; }
        public bool? IsOnline { get; set; }
        public bool? IsAdmin { get; set; }
        public string? ExpireDate { get; set; }
        public DateTime? LastSeenDate { get; set; }
        public DateTime? LastLoginDate { get; set; }

        //Use it as CurrentLogin
        public DateTime? LastLogOutDate { get; set; }
        public string? ActivationCode { get; set; }
        public string? VisualCode { get; set; }
        public int? Session { get; set; }
        public string? RecoverPasswordLink { get; set; }
        public DateTime? RecoverPasswordDate { get; set; }
        public bool? ISOnlineNew { get; set; }
        public DateTime? ActiveTime { get; set; }

        public string? SupEngineerName { get; set; }
        public string? SupEngineerNationalId { get; set; }
        public string? SupEngineerCert { get; set; }
        public string? StampUrl { get; set; }
        public int? TimeId { get; set; }
        public string? AccStatusConfirm { get; set; }
        public string? FullNameAr { get; set; }
        public int? AppearWelcome { get; set; }
        public string? QrCodeUrl { get; set; }
        public int? AppearInInvoicePrint { get; set; }
        [NotMapped]
        public List<int>? BranchesList { get; set; }
        public virtual Department? Department { get; set; }
        public virtual Groups? Groups { get; set; }
        public virtual Job? Jobs { get; set; }
        public virtual Branch? Branches { get; set; }
        public virtual List<UserPrivileges>? UserPrivileges { get; set; }
        public virtual List<ProjectPhasesTasks>? ProjectPhasesTasks { get; set; }
        //public virtual Project Project { get; set; }
        public virtual List<Project>? Project { get; set; }
        public virtual List<Project>? ProjectUpdate { get; set; }
        public virtual List<Project>? ProjectAdd { get; set; }


        public virtual List<WorkOrders>? WorkOrders { get; set; }
        public virtual List<WorkOrders>? WorkOrdersRe { get; set; }
        public virtual List<WorkOrders>? WorkOrdersEx { get; set; }

        public bool? IsActivated { get; set; }
        public string? EncryptedCode { get; set; }

        public int? DeviceType { get; set; }
        public string? DeviceTokenId { get; set; }
        public string? DeviceId { get; set; }


    }
}

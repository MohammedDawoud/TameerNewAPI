using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class UsersVM
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
        public int? Session { get; set; }
        public string? RecoverPasswordLink { get; set; }
        public DateTime? RecoverPasswordDate { get; set; }
        public string? ExpireDate { get; set; }
        public string? Notes { get; set; }
        public string?  DepartmentName  { get; set; }
        public string? JobName { get; set; }
        public string? GroupName { get; set; }
        public string? BranchName { get; set; }
        public bool? IsOnline { get; set; }
        public string? LastSeenDate { get; set; }
        public string? LastLoginDate { get; set; }
        public bool? ISOnlineNew { get; set; }
        public bool? IsAdmin { get; set; }
        public DateTime? ActiveTime { get; set; }
        public string? BranchManager { get; set; }
        public string? StampUrl { get; set; }
        public int? TimeId { get; set; }
        public string? AccStatusConfirm { get; set; }

        public string? SupEngineerName { get; set; }
        public string? SupEngineerNationalId { get; set; }
        public string? SupEngineerCert { get; set; }
        public string? FullNameAr { get; set; }
        public string? FullNameEn { get; set; }

        public string? DeviceId { get; set; }

        public string? AddUsers { get; set; }

        public string? tokenkey { get; set; }
        public List<int> UserPrivilliges { get; set; }
        //For FullReport
        public List<ProjectPhasesTasksVM>? Latetask_VM { get; set; }
        public List<ProjectPhasesTasksVM>? Inprogress_VM { get; set; }
        public List<ProjectPhasesTasksVM>? StoppedTasks_VM { get; set; }

        public List<ProjectPhasesTasksVM>? Notstarted_VM { get; set; }
        public List<ProjectPhasesTasksVM>? Completed_VM { get; set; }
        public List<ProjectPhasesTasksVM>? Retrived_VM { get; set; }
        public List<ProjectPhasesTasksVM>? CompletePercentage_VM { get; set; }
        public List<ProjectPhasesTasksVM>? LatePercentage_VM { get; set; }
        public List<ProjectPhasesTasksVM>? AlmostLate_VM { get; set; }

        public List<ProjectVM>? ProjectLate_VM { get; set; }
        public List<ProjectVM>? ProjectInProgress_VM { get; set; }
        public List<ProjectVM>? ProjectStoped_VM { get; set; }
        public List<ProjectVM>? ProjectWithout_VM { get; set; }
        public List<ProjectVM>? ProjectWithout_VM2 { get; set; }

        public List<ProjectVM>? AllProject_VM { get; set; }
        public List<ProjectVM>? ProjectWork_VM { get; set; }

        public List<ProjectVM>? ProjectAlmostfinish_VM { get; set; }


        public List<ProjectPhasesTasksVM>? AllPhases_VM { get; set; }
        public List<ProjectPhasesTasksVM>? AllFinishPhases_VM { get; set; }
        public List<ProjectPhasesTasksVM>? AllLatePhases_VM { get; set; }





        //For FullReport
        public string? Latetask { get; set; }
        public string? Inprogress { get; set; }
        public string? Notstarted { get; set; }
        public string? Completed { get; set; }
        public string? Retrived { get; set; }
        public int CompletePercentage { get; set; }
        public int LatePercentage { get; set; }
        public string? AlmostLate { get; set; }

        public string? ProjectLate { get; set; }
        public string? ProjectInProgress { get; set; }
        public string? ProjectStoped { get; set; }
        public string? ProjectWithout { get; set; }
        public string? ProjectAlmostfinish { get; set; }


        public bool? IsActivated { get; set; }
        public string? EncryptedCode { get; set; }

        public int? AppearWelcome { get; set; }

        public string? QrCodeUrl { get; set; }

        public int? DeviceType { get; set; }
        public string? DeviceTokenId { get; set; }

        public int? AppearInInvoicePrint { get; set; }

    }
}

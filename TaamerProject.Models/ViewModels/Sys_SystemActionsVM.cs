using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Sys_SystemActionsVM
    {
        public int SysID { get; set; }
        public string? FunctionName { get; set; }
        public string? ServiceName { get; set; }
        public int? ActionType { get; set; }
        public string? MessageName { get; set; }
        public string? ModuleName { get; set; }
        public string? PageName { get; set; }
        public string? ActionDate { get; set; }
        public int? UserId { get; set; }
        public int? BranchId { get; set; }
        public string? Note { get; set; }
        public int? Success { get; set; }

        public string? UserName { get; set; }
        public string? BranchName { get; set; }
        public string? ActionTypeName { get; set; }

        public string? SuccessName { get; set; }
        public DateTime? FullDate { get; set; }


    }
}

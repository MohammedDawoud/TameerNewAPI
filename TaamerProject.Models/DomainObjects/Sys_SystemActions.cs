

namespace TaamerProject.Models
{
    public class Sys_SystemActions : Auditable
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

        public virtual Users? Users { get; set; }
        public virtual Branch? Branches { get; set; }




    }
}

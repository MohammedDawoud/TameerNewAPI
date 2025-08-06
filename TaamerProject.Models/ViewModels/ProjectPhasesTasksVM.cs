using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.DomainObjects;

namespace TaamerProject.Models
{
    
    public class ProjectPhasesTasksVM
    {
        public int PhaseTaskId { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public int? ParentId { get; set; }
        public int? ProjSubTypeId { get; set; }
        public int? Type { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public int? ProjectId { get; set; }
        public int? TimeMinutes { get; set; }
        public int? TimeType { get; set; }
        public int? SettingId { get; set; }

        public int? Remaining { get; set; }
        public bool? IsUrgent { get; set; }
        public bool? IsTemp { get; set; }
        public int? TaskType { get; set; }
        public int? Status { get; set; }
        public int? OldStatus { get; set; }
        public bool? Active { get; set; }
        public int? StopCount { get; set; }
        public int? OrderNo { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public decimal? PercentComplete { get; set; }
        public decimal? Cost { get; set; }
        public int? ToUserId { get; set; }
        public string? Notes { get; set; }
        public int? BranchId { get; set; }
        public string? MainPhaseName { get; set; }
        public string? SubPhaseName { get; set; }
        public string? ProjectTypeName { get; set; }
        public string? ProjectDescription { get; set; }
        public string? FirstProjectDate { get; set; }
        public string? TimeStrProject { get; set; }

        public string? ProjectSubTypeName { get; set; }
        public string? ClientName { get; set; }
        public string? ClientName_W { get; set; }

        public string? ProjectNumber { get; set; }
        public string? Project { get; set; }
        public string? ProjectMangerName { get; set; }
        public string? TaskTypeName { get; set; }
        public string? StatusName { get; set; }
        public string? TimeStr { get; set; }
        public string? TaskStart { get; set; }
        public string? TaskEnd { get; set; }
        public string? TimeTypeName { get; set; }
        public string? ProTypeName { get; set; }
        public string? NodeLocation { get; set; }
        public int? PlayingTime { get; set; }
        public string? FullTaskDescription { get; set; }
        public decimal? TaskNotStarted { get; set; }
        public decimal? TaskInProgress { get; set; }
        public decimal? TaskDone { get; set; }
        public decimal? TaskLate { get; set; }
        public int? PhasePriority { get; set; }
        public int? ExecPercentage { get; set; }
        public bool? IsRead { get; set; }
        public bool? PlusTime { get; set; }
        public int? IsConverted { get; set; }
        public string? PhasesTaskName { get; set; }

        public int? TaskOn { get; set; }
        public int? MainPhaseId { get; set; }
        public int? SubPhaseId { get; set; }
        public int? IsMerig { get; set; }
        public string? EndTime { get; set; }
        public string? TaskFullTime { get; set; }
        public string? ExcpectedStartDate { get; set; }
        public string? ExcpectedEndDate { get; set; }
        public int? StopProjectType { get; set; }
        public DateTime AddDate { get; set; }
        public string? TaskLongDesc { get; set; }
        public int? ProjectTaskExist { get; set; }
        public int? IsRetrieved { get; set; }
        public int? ProjectGoals { get; set; }
        public int? Lastgoal{ get; set; }
        public int? ProjectTypeId { get; set; }

        public string? ProjectManagerImg { get; set; }
        public int? ProjectManagerId { get; set; }
        public string? AddedTaskName { get; set; }
        public string? AddedTaskImg { get; set; }
        public int? AddTaskUserId { get; set; }
        public string? UserImg { get; set; }
        public DateTime? AddDate2 { get; set; }

        public int? AskDetails { get; set; }
        public int? IsNew { get; set; }
        public string? TaskTimeFrom { get; set; }
        public string? TaskTimeTo { get; set; }
        public string? EndDateCalc { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string? ProjectExpireDate { get; set; }
        public string? SettingName { get; set; }
        public string? RetrievedReason { get; set; }
        public int? NumAdded { get; set; }
        public string? Mobile { get; set; }
        public bool? NotVacCalc { get; set; }
        public int? indentation { get; set; }
        public int? taskindex { get; set; }
        public DateTime? StartDateNew { get; set; }
        public DateTime? EndDateNew { get; set; }
        public Int16? Managerapproval { get; set; }
        public int? ReasonsId { get; set; }
        public string? ReasonsIdText { get; set; }
        public bool? NewSetting { get; set; }

        public List<ContactList>? ContactLists { get; set; }

        public string? PlusTimeReason { get; set; }
        public string? convertReason { get; set; }
        public string? PlusTimeReason_admin { get; set; }
        public string? convertReason_admin { get; set; }
        public decimal? Totalhourstask { get; set; }
        public decimal? Totaltaskcost { get; set; }
        public string? TaskNo { get; set; }
        public int? TaskNoType { get; set; }
    }






    public class PerformanceReportVM
    {
        public int? UserId { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public int? BranchId { get; set; }
        public string? SearchUserIdStr { get; set; }
        public string? SearchBranchIdStr { get; set; }
        public int? AccBranchId { get; set; }

    }

}

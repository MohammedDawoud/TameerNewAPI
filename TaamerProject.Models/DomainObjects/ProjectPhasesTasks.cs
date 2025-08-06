using System;
using System.ComponentModel.DataAnnotations;
using TaamerProject.Models.DomainObjects;

namespace TaamerProject.Models
{
    public class ProjectPhasesTasks : Auditable
    {
        public int PhaseTaskId { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public int? ParentId { get; set; }
        public int? ProjSubTypeId { get; set; }
        public int? Type { get; set; }
        public int? UserId { get; set; }
        public int? ProjectId { get; set; }
        public int? TimeMinutes { get; set; }
        public int? TimeType { get; set; }
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
        public long? LocationId { get; set; }
        public virtual Project? Project { get; set; }
        public int? SettingId { get; set; }
        public int? ParentSettingId { get; set; }
        public int? PhasePriority { get; set; }
        public int? ExecPercentage { get; set; }
        public virtual ProjectSubTypes? ProjectSubTypes { get; set; }
        //public virtual ProjectPhasesTasks? MainPhase { get; set; }
        public virtual ProjectPhasesTasks? SubPhase { get; set; }
        public virtual Users? Users { get; set; }
        public virtual NodeLocations? NodeLocations { get; set; }
        public virtual Settings? Settings { get; set; }
        public virtual TaskType? TaskTypeModel { get; set; }
        public bool? IsRead { get; set; }
        public bool? PlusTime { get; set; }
        public int? IsConverted { get; set; }
        public int? IsMerig { get; set; }
        public string? EndTime { get; set; }
        public string? TaskFullTime { get; set; }
        public string? ExcpectedStartDate { get; set; }
        public string? ExcpectedEndDate { get; set; }
        public string?  TaskLongDesc { get; set; }
        public int? IsRetrieved { get; set; }

        public int? ProjectGoals { get; set; }
        public int? AddTaskUserId { get; set; }

        public int? AskDetails { get; set; }
        public int? IsNew { get; set; }
        public string? TaskTimeFrom { get; set; }
        public string? TaskTimeTo { get; set; }
        public string? EndDateCalc { get; set; }
        public int? DepartmentId { get; set; }
        public string? RetrievedReason { get; set; }
        public int? NumAdded { get; set; }
        public bool? NotVacCalc { get; set; }
        public int? indentation { get; set; }
        public int? taskindex { get; set; }
        public DateTime? StartDateNew { get; set; }
        public DateTime? EndDateNew { get; set; }
        public Int16? Managerapproval { get; set; }
        public int? ReasonsId { get; set; }
        public virtual Pro_tasksReasons? tasksReasons { get; set; }

        public virtual Users? AddTaskUser { get; set; }
        public ProjectRequirementsGoals? ProjectRequirementsGoals { get; set; }
        public virtual Department? department { get; set; }
        public virtual List<ContactList>? ContactLists { get; set; }

        public string? PlusTimeReason { get; set; }
        public string? convertReason { get; set; }
        public string? PlusTimeReason_admin { get; set; }
        public string? convertReason_admin { get; set; }
        public decimal? Totalhourstask { get; set; }
        public decimal? Totaltaskcost { get; set; }
        public string? TaskNo { get; set; }
        public int? TaskNoType { get; set; }
        public virtual List<Pro_TaskOperations>? TaskOperationsList { get; set; }

    }
}

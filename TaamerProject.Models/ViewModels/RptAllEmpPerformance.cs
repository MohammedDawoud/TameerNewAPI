using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class RptAllEmpPerformance
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Latetask { get; set; }
        public string? Inprogress { get; set; }
        public string? StoppedTasks { get; set; }

        public string? Notstarted { get; set; }
        public string? Completed { get; set; }
        public string? Retrived { get; set; }
        public string? CompletePercentage { get; set; }
        public string? LatePercentage { get; set; }
        public string? AlmostLate { get; set; }

        public string? ProjectLate { get; set; }
        public string? ProjectInProgress { get; set; }
        public string? ProjectStoped { get; set; }
        public string? ProjectWithout { get; set; }
        public string? ProjectAlmostfinish { get; set; }
    }

    public class ProjectVMNewCounting
    {
        public string? BranchId { get; set; }
        public string? BranchName { get; set; }
        public string? GetProjectsStoppedVMCount { get; set; }
        public string? GetProjectsWithoutContractVMVMCount { get; set; }
        public string? GetLateProjectsVMCount { get; set; }
        public string? GetProjectsWithoutProSettingVMCount { get; set; }
        public string? GetProjectsWithProSettingVMCount { get; set; }
        public string? GetProjectsSupervisionVMVMCount { get; set; }
        public string? GetdestinationsUploadVMCount { get; set; }
        public string? GetProjectsInProgressCount { get; set; }
        public string? GetProjectsNaturalCount { get; set; }
    }

    public class ProjectVMNewStat
    {
        public string? GetProjectsContractId { get; set; }
        public string? GetProjectsPhases { get; set; }
        public string? GetProjectsInvoice { get; set; }
        public string? GetProjectsVouchers { get; set; }
    }
    public class ProjectVMPhasesDetails
    {
        public int? PhaseTaskId { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public int? ParentId { get; set; }
        public int? Type { get; set; }
        public decimal? alltaskscount { get; set; }
        public decimal? alltaskspercent { get; set; }
        public decimal? phasePercent { get; set; }
    }
    public class ProjectVMProc
    {
        public int? ProjectId { get; set; }
        public string? ProjectNo { get; set; }
        public string? CustomerName { get; set; }
        public int? Status { get; set; }
    }
}

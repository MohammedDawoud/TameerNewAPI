using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.ViewModels
{
 public class rptProjectStatus_phases
    {
        public string? ProjectName { get; set; }
        public string? CustomerName { get; set; }
        public string? ProjectNo { get; set; }
        public string? PieceNo { get; set; }
        public string? District { get; set; }
        public string? ProjectManager { get; set; }
        public List<ParentPhase> ParentPhases { get; set; } = new List<ParentPhase>();
    }

    public class ParentPhase
    {
        public string? ParentPhaseId { get; set; }
        public string? ParentPhaseName { get; set; }
        public string? ParentStartDate { get; set; }
        public string? ParentEndDate { get; set; }
        public string? ParentExecPercentage { get; set; }
        public string? ExpectedEndPhase { get; set; }
        public string? ChildTaskCount { get; set; }
        public List<ChildPhase> ChildPhases { get; set; } = new List<ChildPhase>();
    }

    public class ChildPhase
    {
        public string? ChildPhaseId { get; set; }
        public string? ChildPhaseName { get; set; }
        public string? ChildStartDate { get; set; }
        public string? ChildEndDate { get; set; }
        public string? ChildExecPercentage { get; set; }
        public string? SubTaskCount { get; set; }
        public List<ChildPhaseTask> Tasks { get; set; } = new List<ChildPhaseTask>();
    }

    public class ChildPhaseTask
    {
        public string? TaskId { get; set; }
        public string? TaskName { get; set; }
        public string? TaskStartDate { get; set; }
        public string? TaskEndDate { get; set; }
        public string? TaskEndDateCalc { get; set; }
        public string? TaskExecPercentage { get; set; }
        public string? TaskStatus { get; set; }
        public string? TimeType { get; set; }
        public string? TimeMinutes { get; set; }
        public string? AssignedUserFullName { get; set; }
        public string? TimeStr { get; set; }
    }

}

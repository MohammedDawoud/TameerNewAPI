using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.ViewModels
{
    public class rptProjectStatus
    {

        public string? ProjectName { get; set; }
        public string? CustomerName { get; set; }
        public string? ProjectNo { get; set; }
        public string? PieceNo { get; set; }
        public string? District { get; set; }
        public string? ProjectManager { get; set; }
        public List<Phase> ParentTasks { get; set; } = new List<Phase>();
    }

    public class Phase
    {
        public string? ParentTaskId { get; set; }
        public string? ParentTaskName { get; set; }
        public string? ParentStartDate { get; set; }
        public string? ParentEndDate { get; set; }
        public string? ParentExecPercentage { get; set; }
        public string? ExpectedEndPhase { get; set; }
        public List<proTask> ChildTasks { get; set; } = new List<proTask>();
    }

    public class proTask
    {
        public string? ChildTaskId { get; set; }
        public string? ChildTaskName { get; set; }
        public string? ChildStartDate { get; set; }
        public string? ChildEndDate { get; set; }
        public string? ChildEndDateCalc { get; set; }
        public string? ChildExecPercentage { get; set; }
        public string? TaskStatus { get; set; }
        public string? TimeType { get; set; }
        public string? TimeMinutes { get; set; }
        public string? AssignedUserFullName { get; set; }
        public string? TimeStr { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.DomainObjects;

namespace TaamerProject.Models
{
    public class WorkOrdersVM
    {
        public int WorkOrderId { get; set; }
        public string? OrderNo { get; set; }
        public int? UserId { get; set; }
        public string? OrderDate { get; set; }
        public string? OrderHijriDate { get; set; }
        public int? ResponsibleEng { get; set; }
        public int? ExecutiveEng { get; set; }
        public int? CustomerId { get; set; }
        public string? Mediator { get; set; }
        public string? Required { get; set; }
        public string? Note { get; set; }
        public decimal? OrderValue { get; set; }
        public decimal? OrderPaid { get; set; }
        public decimal? OrderRemaining { get; set; }
        public decimal? OrderDiscount { get; set; }
        public decimal? OrderTax { get; set; }
        public decimal? OrderValueAfterTax { get; set; }
        public string? DiscountReason { get; set; }
        public string? Sketch { get; set; }
        public string? District { get; set; }
        public string? Location { get; set; }
        public string? PieceNo { get; set; }
        public string? InstrumentNo { get; set; }
        public string? ExecutiveType { get; set; }
        public string? ContractNo { get; set; }
        public string? AgentId { get; set; }
        public string? AgentMobile { get; set; }
        public string? Social { get; set; }
        public int? BranchId { get; set; }
        public string? UserName { get; set; }
        public string? ResponsibleEngName { get; set; }
        public string? ExecutiveEngName { get; set; }
        public string? ResponsibleEngImg { get; set; }
        public string? ExecutiveEngImg { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerName_W { get; set; }

        public string? EndDate { get; set; }
        public int? WOStatus { get; set; }
        public string? WOStatustxt { get; set; }
        public string? TaskStatusName { get; set; }
        public decimal? PercentComplete { get; set; }
        public string? StrTime { get; set; }
        public int? NoOfDays { get; set; }
        public int? ProjectId { get; set; }
        public int? IsConverted { get; set; }
        public bool? PlusTime { get; set; }
        public string? AttatchmentUrl { get; set; }
        public string? ProjectNo { get; set; }
        public int? StopProjectType { get; set; }

        public string? AddOrderName { get; set; }
        public string? AddOrderImg { get; set; }
        public string? ProjectMangerName { get; set; }
        public string? ProjectManagerImg { get; set; }
        public string? RequiredOrder { get; set; }
        public string? ByUser { get; set; }
        public string? Duration { get; set; }
        public string? EmpName { get; set; }
        public string? JobName { get; set; }
        public int? PhasePriority { get; set; }
        public string? PhasePriorityName { get; set; }
        public string? DateFrom { get; set; }
        public string? DateTo { get; set; }
        public List<ContactList>? ContactLists { get; set; }


    }
}

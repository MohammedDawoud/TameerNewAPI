

using System.ComponentModel.DataAnnotations.Schema;
using TaamerProject.Models.DomainObjects;

namespace TaamerProject.Models
{
    public class WorkOrders : Auditable
    {
        public int WorkOrderId { get; set; }
        public string? OrderNo { get; set; }
        [ForeignKey("UserId")]
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
        public decimal? PercentComplete { get; set; }
        public int? BranchId { get; set; }
        public string? EndDate { get; set; }
        public int? WOStatus { get; set; }
        public string? WOStatustxt { get; set; }
        public string? AttatchmentUrl { get; set; }
        
        public int NoOfDays { get; set; }
        public int? ProjectId { get; set; }
        public int? IsConverted { get; set; }
        public bool? PlusTime { get; set; }
        public int? PhasePriority { get; set; }
        public int? OrderNoType { get; set; }
        public virtual List<Pro_TaskOperations>? TaskOperationsList { get; set; }
        public virtual Project? Project { get; set; }
        public virtual Users? User { get; set; }

        public virtual Users? ResponsibleEngineer { get; set; }

        public virtual Users? ExecutiveEngineer { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual List<ContactList>? ContactLists { get; set; }

    }
}

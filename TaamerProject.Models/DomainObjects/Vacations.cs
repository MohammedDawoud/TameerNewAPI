using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Vacation : Auditable
    {
        public int VacationId { get; set; }
        public int? EmployeeId { get; set; }
        public int? VacationTypeId { get; set; }
        public string? StartDate { get; set; }
        public string? StartHijriDate { get; set; }
        public string? EndDate { get; set; }
        public string? EndHijriDate { get; set; }
        public string? VacationReason { get; set; }
        public int? VacationStatus { get; set; }
        public bool? IsDiscount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public int? UserId { get; set; }
        public int? BranchId { get; set; }
        public string? Date { get; set; }
        public string? AcceptedDate { get; set; }
        public int? DaysOfVacation { get; set; }
        public int? DecisionType { get; set; }
        public string? BackToWorkDate { get; set; }
        public int? AcceptedUser { get; set; }

        public VacationType? VacationTypeName { get; set; }
        public Employees? EmployeeName { get; set; }
        public virtual Users? UserAcccept { get; set; }

        public string? FileUrl { get; set; }
        public string? FileName { get; set; }

        //public Branch Branches { get; set; }

    }
}

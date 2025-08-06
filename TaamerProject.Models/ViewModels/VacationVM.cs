using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class VacationVM
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
        public string? VacationTypeName { get; set; }
        public string? VacationStatusName { get; set; }
        public string? EmployeeName { get; set; }
        public bool IsSearch { get; set; }
        public string? Date { get; set; }
        public string? AcceptedDate { get; set; }
        public int DaysOfVacation { get; set; }
        public string? TimeStr { get; set; }
        public string? BranchName { get; set; }
        public int? DecisionType { get; set; }
        public string?  BackToWorkDate { get; set; }
        public string? AcceptUser { get; set; }
        public string? FileUrl { get; set; }

        public string? FileName { get; set; }
        public string? EmployeeNo { get; set; }
        public string? EmployeeJob { get; set; }
        public string? IdentityNo { get; set; }

    }
}

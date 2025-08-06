using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class LoanDetailsVM
    {
        public int LoanDetailsId { get; set; }
        public int? LoanId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Date { get; set; }
        public bool Finished { get; set; }
        public int? SanadId { get; set; }
        public int? UserId { get; set; }
        public string? AcceptedDate { get; set; }
        public string? EmployeeName { get; set; }
        public int? MonthNo { get; set; }
        public decimal? AmountPayed { get; set; }
        public decimal? AmountNotPayed { get; set; }


    }
}

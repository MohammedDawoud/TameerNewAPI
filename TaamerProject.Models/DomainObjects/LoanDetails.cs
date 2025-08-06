using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class LoanDetails : Auditable
    {
        public int LoanDetailsId { get; set; }
        public int? LoanId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Date { get; set; }
        public bool Finished { get; set; }
        public int? SanadId { get; set; }
        public int? UserId { get; set; }
        public virtual Loan? Loan { get; set; }

    }
}

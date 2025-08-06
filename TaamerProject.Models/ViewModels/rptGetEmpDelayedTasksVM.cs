using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
  public  class rptGetEmpDelayedTasksVM
    {
        public string? DescriptionAr { get; set; }
        public string? Duration { get; set; }
        public string? ProjectNo { get; set; }
        public string? ClientName { get; set; }
        public string? StartDate { get; set; }
        public string? DelayTime { get; set; }
        public string? EmpName { get; set; }
        public string? JobName { get; set; }
    }

    public class rptGetEmpLoans
    {
        public string? LoanID { get; set; }
        public string? date { get; set; }
        public string? Amount { get; set; }
        public string? MonthNo { get; set; }
        public string? NameAr { get; set; }
        public string? Payed { get; set; }
    }
}

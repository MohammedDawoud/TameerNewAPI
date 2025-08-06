using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Acc_EmpFinYears: Auditable
    {
        public int Acc_EmpFinYearID { get; set; }
        public int? EmpID { get; set; }
        public int? BranchID { get; set; }
        public int? YearID { get; set; }
        public virtual Branch? branch { get; set; }
        public virtual FiscalYears? fiscalYears { get; set; }
        public virtual Users? user { get; set; }
    }
}

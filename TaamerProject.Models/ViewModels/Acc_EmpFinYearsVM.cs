using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Acc_EmpFinYearsVM
    {
        public int Acc_EmpFinYearID { get; set; }
        public int? EmpID { get; set; }
        public int? BranchID { get; set; }
        public int? YearID { get; set; }
        public string? Username { get; set; }
        public string? Userjob { get; set; }
        public int BranchID2 { get; set; }
        public string? Branchname { get; set; }
        public int? YearValue { get; set; }
    }
}

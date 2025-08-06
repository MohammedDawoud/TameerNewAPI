using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class FollowProjVM
    {
        public int FollowProjId { get; set; }
        public int? ProjectId { get; set; }
        public int? EmpId { get; set; }
        public string? TimeNo { get; set; }
        public string? TimeType { get; set; }
        public string? EmpRate { get; set; }
        public decimal? Amount { get; set; }
        public decimal? ExpectedCost { get; set; }
        public bool? ConfirmRate { get; set; }
        public decimal? EmpSalary { get; set; }
        public decimal? ProContractAmount { get; set; }
        public decimal? ProCost_E { get; set; }
        public string? EmployeeName { get; set; }
        public string? TimeStr{ get; set; }
    }
}

using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class CostCentersVM
    {
        public int CostCenterId { get; set; }
        public string? Code { get; set; }
        public string? CostCenterName { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int? ParentId { get; set; }
        public int? Level { get; set; }
        public int? BranchId { get; set; }
        public int? ProjId { get; set; }
        public int? CustomerId { get; set; }
        public object ParentCostCenterCode { get; set; }
        public object ParentCostCenterName { get; set; }

        public decimal? TotalCredit { get; set; }
        public decimal? TotalDepit { get; set; }
        public decimal? TotalCreditBalance { get; set; }
        public decimal? TotalDepitBalance { get; set; }
        public decimal? TotalBalance { get; set; }
        public List<CostCentersVM> ChildCosrCenters { get; set; }
        public List<TransactionsVM> Transactions { get; set; }
        public List<TransactionsVM> CostCenterAccsTransactions { get; set; }
    }
}

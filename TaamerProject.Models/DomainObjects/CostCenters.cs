using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class CostCenters : Auditable
    {
        public int CostCenterId { get; set; }
        public string? Code { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int? ParentId { get; set; }
        public int? Level { get; set; }
        public int? BranchId { get; set; }
        public int? ProjId { get; set; }
        public int? CustomerId { get; set; }

        public CostCenters? ParentCostCenter { get; set; }
        public virtual List<Transactions>? Transactions { get; set; }
        public virtual List<CostCenters>? ChildsCostCenter { get; set; }
    }
}

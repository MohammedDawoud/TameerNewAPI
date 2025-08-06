using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ContractServices : Auditable
    {
        public int ContractServicesId { get; set; }
        public int? ContractId { get; set; }

        public int? ServiceId { get; set; }
        public int? ServiceQty { get; set; }
        public string? serviceoffertxt { get; set; }
        public int? BranchId { get; set; }
        public int? TaxType { get; set; }

        public decimal? Serviceamountval { get; set; }

        public virtual Contracts? Contracts { get; set; }
        public virtual Acc_Services_Price? serviceprice { get; set; }
    }
}

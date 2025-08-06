using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class AccServicesPricesVM
    {
        public int  Id{ get; set; }
        public string? Name{ get; set; }
        public int ServicesId { get; set; }
        public string? ServicesName { get; set; }
        public decimal? Amount { get; set; }
        public string? AccountName { get; set; }
        public int? AccountId { get; set; }
        public int? ProjectId { get; set; }
        public int? ProjectSubTypeID { get; set; }
        public int? ParentId { get; set; }
        public int? CostCenterId { get; set; }

        public string? ProjectName { get; set; }
        public string? ProjectSubTypeName{ get; set; }
        public int? PackageId { get; set; }
        public string? AmountAndPackage { get; set; }

        public string? PackageName { get; set; }

        public decimal? MeterPrice1 { get; set; }
        public decimal? MeterPrice2 { get; set; }
        public decimal? MeterPrice3 { get; set; }
        public int? PackageRatio1 { get; set; }
        public int? PackageRatio2 { get; set; }
        public int? PackageRatio3 { get; set; }
        public string? ServiceName_EN { get; set; }
        public int? ServiceType { get; set; }
        public string? ServiceTypeName { get; set; }


    }
}

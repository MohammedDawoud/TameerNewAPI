using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Acc_PackagesVM
    {
        public int PackageId { get; set; }
        public string? PackageName { get; set; }
        public decimal? MeterPrice1 { get; set; }
        public decimal? MeterPrice2 { get; set; }
        public decimal? MeterPrice3 { get; set; }
        public int? PackageRatio1 { get; set; }
        public int? PackageRatio2 { get; set; }
        public int? PackageRatio3 { get; set; }
    }
}

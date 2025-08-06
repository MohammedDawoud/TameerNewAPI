using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class CurrencyVM
    {
        public int CurrencyId { get; set; }
        public string? CurrencyCode { get; set; }
        public string? CurrencyNameAr { get; set; }
        public string? CurrencyNameEn { get; set; }
        public int PartCount { get; set; }
        public string? PartNameAr { get; set; }
        public string? PartNameEn { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}

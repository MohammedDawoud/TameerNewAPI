using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Currency : Auditable
    {
        public int CurrencyId { get; set; }
        public string? CurrencyCode { get; set; }
        public string? CurrencyNameAr { get; set; }
        public string? CurrencyNameEn { get; set; }
        public int PartCount { get; set; }
        public string? PartNameAr { get; set; }
        public string? PartNameEn { get; set; }
        public decimal ExchangeRate { get; set; }
        public int BranchId { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Banks : Auditable
    {
        public int BankId { get; set; }
        public string? Code { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Notes { get; set; }

        public string? BanckLogo { get; set; }

    }
}

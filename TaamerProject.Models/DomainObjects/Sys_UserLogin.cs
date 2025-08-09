using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Sys_UserLogin : Auditable
    {
        public int UserLoginId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? CompanyName { get; set; }
        public string? Mobile { get; set; }
        public string? NationalId { get; set; }
        public string? MainActivity { get; set; }
        public string? SubMainActivity { get; set; }
        public string? CommercialId { get; set; }
        public string? Notes { get; set; }
        public Int16? Type { get; set; }
        public Int16? Status { get; set; }
        public string? AuthenticatorSecret { get; set; }
        public bool? Is2FAEnabled { get; set; }

    }
}

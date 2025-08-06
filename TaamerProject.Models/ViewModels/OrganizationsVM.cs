using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class OrganizationsVM
    {
        public int OrganizationId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? LogoUrl { get; set; }
        public string? ServerName { get; set; }
        public string? Address { get; set; }
        public string? WebSite { get; set; }
        public string? Fax { get; set; }
        public int? CityId { get; set; }
        public int? ReportType { get; set; }
        public string? TaxCode { get; set; }
        public string? NotificationsMail { get; set; }
        public decimal? VAT { get; set; }
        public int? VATSetting { get; set; }
        public string? PostalCode { get; set; }
        public string? Password { get; set; }
        public string? SenderName { get; set; }
        public string? Host { get; set; }
        public string? Port { get; set; }
        public bool? SSL { get; set; } 
        public string? AccountBank { get; set; }
        public string? IsFooter { get; set; }
        public string? EditUserName { get; set; }
        public DateTime? EditUserDate { get; set; }
        public int? RepresentorEmpId { get; set; }
        public string? PostalCodeFinal { get; set; }
        public string? ExternalPhone { get; set; }
        public string? Country { get; set; }
        public string? Neighborhood { get; set; }
        public string? StreetName { get; set; }
        public string? BuildingNumber { get; set; }
        public string? AccountBank2 { get; set; }

        public string? Engineering_License { get; set; }
        public string? Engineering_LicenseDate { get; set; }
        public string? ComDomainLink { get; set; }
        public string? ComDomainAddress { get; set; }


        public string? CityName { get; set; }
        public string? OrgName { get; set; }

        public int? BankId { get; set; }
        public int? BankId2 { get; set; }
        public string? BankIdImgURL { get; set; }
        public string? BankId2ImgURL { get; set; }

        public string? CSR { get; set; }
        public string? PrivateKey { get; set; }
        public string? PublicKey { get; set; }
        public string? SecreteKey { get; set; }

        public string? ApiBaseUri { get; set; }


        public string? LastvesionAndroid { get; set; }
        public string? LastversionIOS { get; set; }
        public string? MessageUpdateAr { get; set; }
        public string? MessageUpdateEn { get; set; }

        public string? SupportMessageAr { get; set; }
        public string? SupportMessageEn { get; set; }
        public string? TameerAPIURL { get; set; }
        public bool? SendCustomerMail { get; set; }
        public bool? SendCustomerSMS { get; set; }
        public int? ModeType { get; set; }


    }
}


namespace TaamerProject.Models
{
    public class SystemSettings : Auditable
    {
        public int SettingId { get; set; }
        public int? FiscalYear { get; set; }
        public int? BranchId { get; set; }
        public int? CurrencyId { get; set; }
        public int? AttendenceId { get; set; }
        public string? CustGenerateCode { get; set; }
        public string? ProjGenerateCode { get; set; }
        public string? OfferGenerateCode { get; set; }

        public string? ContractGenerateCode { get; set; }
        public string? EmpGenerateCode { get; set; }
        public string? BranchGenerateCode { get; set; }
        public int? DecimalPoints { get; set; }
        public string? NoReplyMail { get; set; }
        public int? ActiveCodeInterval { get; set; }
        public int? ActiveUserNumber { get; set; }
        public string? VoucherGenerateCode { get; set; }
        public bool? LogErrors { get; set; }
        public bool? EnableNotification { get; set; }
        public bool? EnableSMS { get; set; }
        public int? SMTPPort { get; set; }
        public int? DefaultUserSession  { get ; set;}
        public int? PhoneNoDigits { get; set; }
        public int? MobileNoDigits { get; set; }
        public int? NationalIDDigits { get; set; }

        public string? ContractGenerateCode2 { get; set; }

        public bool? CustomerMailIsRequired { get; set; }
        public bool? CustomerNationalIdIsRequired { get; set; }
        public bool? OrgDataIsRequired { get; set; }

        public bool? CustomerphoneIsRequired { get; set; }

        public string? Contract_Con_Code { get; set; }
        public string? Contract_Sup_Code { get; set; }
        public string? Contract_Des_Code { get; set; }
        public bool? UploadInvZatca { get; set; }
        public string? ZatcaCheckCode { get; set; }
        public string? DestinationCheckCode { get; set; }

        public int? ContractEndNote { get; set; }
        public int? ResedentEndNote { get; set; }
        public bool? ValueAddedSeparated { get; set; }

        public virtual Users? UpdateUserT { get; set; }





    }
}

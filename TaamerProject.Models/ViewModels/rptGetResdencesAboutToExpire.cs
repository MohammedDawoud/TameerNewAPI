using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
   public class rptGetResdencesAboutToExpireVM
    {
        public string? NationalId { get; set; }
        public string? NameAr { get; set; }
        public string? Nationality { get; set; }
        public string? NationalIdEndDate { get; set; }
        public string? NotifiDate { get; set; }
        public string? Department { get; set; }
        public string? Branch { get; set; }
        public string? ContractEndDate { get; set; }
        public string? JobName { get; set; }

    }

    public class rptGetResdencesExpiredVM
    {
        public string? NationalId { get; set; }
        public string? NameAr { get; set; }
        public string? Nationality { get; set; }
        public string? NationalIdEndDate { get; set; }
        public string? Department { get; set; }
        public string? Branch { get; set; }
        public string? ContractEndDate { get; set; }
        public string? JobName { get; set; }
        public string? DirectManager { get; set; }

        
    }

    public class rptGetOfficialDocsAboutToExpire
    {
        public string? NameAr { get; set; }
        public string? Number { get; set; }
        public string? DocSource { get; set; }
        public string? ExpiredDate { get; set; }
        public string? NotifiDate { get; set; }
        public string? Branch { get; set; }
        public string? Notes { get; set;}
    }

    public class rptGetOfficialDocsExpiredVM
    {
        public string? NameAr { get; set; }
        public string? Number { get; set; }
        public string? DocSource { get; set; }
        public string? ExpiredDate { get; set; }
        public string? Branch { get; set; }
        public string? Notes { get; set; }
        public string? AttachmentUrl { get; set; }

        
    }

    public class GetOfficialPapersStatitecsVM
    {
        public string? ResAboutToExpire { get; set; }
        public string? ResExpired { get; set; }
        public string? PapAboutToExpire { get; set; }
        public string? PapExpired { get; set; }
        public string? DeservedServices { get; set; }
        public string? Vacation { get; set; }
        public string? EmpLoans { get; set; }
        public string? EmpContract { get; set; }
    }

    public class rptGetDeservedServicesVM
    {
        public string?  Number { get; set; }
        public string? AccCode { get; set; }
        public string? Department { get; set; }
        public string? ExpireDate { get; set; }
        public string? Branch { get; set; }
    }

    public class rptGetAboutToStartVacationsVM
    {
        public string? EmpNo { get; set; }
        public string? EmpName { get; set; }
        public string? Nationality { get; set; }
        public string? DepName { get; set; }
        public string? Branch { get; set; }
        public string? StartDate { get; set; }
    }

    public class rptGetEmpContractsAboutToExpireVM
    {
       
        public string? ContractNo { get; set; }
        public string? NameAr { get; set; }
        public string? Nationality { get; set; }
        public string? Department { get; set; }
        public string? Branch { get; set; }
        public string? ContractEndDate { get; set; }
        public string? JobName { get; set;}
        public string? Salary { get; set;}
        public string? Duration { get; set;}

    }

    public class FillSelectVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ProjectNo { get; set; }
        public string? CustomerNameW { get; set; }
        public string? CustomerName { get; set; }
        public int? CustomerId { get; set; }
        public int? ContractId { get; set; }
        public int? TypeCode { get; set; }
        public int? ProjectTypeId { get; set; }

        
    }
}

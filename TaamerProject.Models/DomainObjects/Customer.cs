using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaamerProject.Models.DomainObjects;
namespace TaamerProject.Models
{
    public class Customer : Auditable
    {
        public int CustomerId { get; set; }
        public string? CustomerCode { get; set; }
        public string? CustomerNameAr { get; set; }
        public string? CustomerNameEn { get; set; }
        public string? CustomerNationalId { get; set; }
        public int? NationalIdSource { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerMobile { get; set; }
        public int? CustomerTypeId { get; set; }
        public string? Notes { get; set; }
        public string? LogoUrl { get; set; }
        public string? AttachmentUrl { get; set; }
        public int? CommercialActivity { get; set; }
        public string? CommercialRegister { get; set; }
        public string? CommercialRegDate { get; set; }
        public string? CommercialRegHijriDate { get; set; }
        public int? AccountId { get; set; }
        public int? GeneralManager { get; set; }
        public string? AgentName { get; set; }
        public int? AgentType { get; set; }
        public string? AgentNumber { get; set; }
        public string? AgentAttachmentUrl { get; set; }
        public string? ResponsiblePerson { get; set; }
        public int BranchId { get; set; }
        public bool IsPrivate { get; set; }
        public string? CompAddress { get; set; }
        public string? PostalCodeFinal { get; set; }
        public string? ExternalPhone { get; set; }
        public string? Country { get; set; }
        public string? Neighborhood { get; set; }
        public string? StreetName { get; set; }
        public string? BuildingNumber { get; set; }
        public int? CityId { get; set; }
        
        public virtual City? city { get; set; }
        public virtual Branch? Branch { get; set; }

        public virtual CommercialActivity? commercialActivities { get; set; }
        public virtual CommercialActivity? BranchActivity { get; set; }
        public Accounts? Accounts { get; set; }
        public virtual List<Project>? Projects { get; set; }
        public virtual List<Invoices>? Invoicess { get; set; }
        public virtual List<Transactions>? Transactions { get; set; }
        public virtual List<Customer_Branches>? Customer_Branches { get; set; }

        public virtual Users? AddUsers { get; set; }
        [NotMapped]
        public List<int>? OtherBranches { get; set; }

}
}

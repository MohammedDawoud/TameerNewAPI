using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.DomainObjects;

namespace TaamerProject.Models
{
    public class CustomerVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int CustomerId { get; set; }
        public int? BranchId { get; set; }
        public string? CustomerCode { get; set; }
        public string? CustomerName { get; set; }
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
        public string? ProjectNo { get; set; }
        public int? GeneralManager { get; set; }
        public string? AgentName { get; set; }
        public int? AgentType { get; set; }
        public string? AgentNumber { get; set; }
        public string? AgentAttachmentUrl { get; set; }
        public string? ResponsiblePerson { get; set; }
        public string? AccountName { get; set; }
        public DateTime? AddDate { get; set; }
        public string? CustomerTypeName { get; set; }
        public string? AddUser { get; set; }
        public string? CompAddress { get; set; }
        public string? PostalCodeFinal { get; set; }
        public string? ExternalPhone { get; set; }
        public string? Country { get; set; }
        public string? Neighborhood { get; set; }
        public string? StreetName { get; set; }
        public string? BuildingNumber { get; set; }
        public string? CommercialRegInvoice { get; set; }
        public int? CityId { get; set; }
        public string? CityName { get; set; }
        public int? NoOfCustProj { get; set; }
        public string? NoOfCustProjMark { get; set; }

        public string? AddedcustomerImg { get; set; }
        public string? BranchName { get; set; }

        public List<ProjectVM> Projects { get; set; }
        public string? AccountCodee { get; set; }
        public decimal? TotalRevenue {get; set;}
        public decimal? TotalExpenses { get; set; }
        public List<InvoicesVM> Invoices { get; set; }
        public List<TransactionsVM> Transactions { get; set; }
        public List<Customer_Branches>? Customer_Branches { get; set; }
        public List<int>? OtherBranches { get; set; }

        public string? CommercialActivityName { get; set; }
        public string? BranchActivityName { get; set; }

    }
}

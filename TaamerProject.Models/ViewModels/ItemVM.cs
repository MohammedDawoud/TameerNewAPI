using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ItemVM
    {
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int? TypeId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public string? SachetNo { get; set; }
        public string? FormNo { get; set; }
        public string? Color { get; set; }
        public string? IssuancePlace { get; set; }
        public string? IssuanceDate { get; set; }
        public string? IssuanceHijriDate { get; set; }
        public string? IssuanceEndDate { get; set; }
        public string? IssuanceEndHijriDate { get; set; }
        public string? SupplyDate { get; set; }
        public string? SupplyHijriDate { get; set; }
        public string? PlateNo { get; set; }
        public string? InsuranceNo { get; set; }
        public string? InsuranceEndDate { get; set; }
        public string? InsuranceEndHijriDate { get; set; }
        public string? LiscenceFileUrl { get; set; }
        public string? InsuranceFileUrl { get; set; }
        public int? BranchId { get; set; }
        public int? Status { get; set; }
        public int? Ramainder { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ContractsVM
    {
        public int ContractId { get; set; }
        public string? ContractNo { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public  int? CustomerId { get; set; }
        public  int? ProjectId { get; set; }
        public  int? CityId { get; set; }
        public string? InstrumentNo { get; set; }
        public string? InstrumentDate { get; set; }
        public int? Type { get; set; }
        public decimal? Value { get; set; }
        //public decimal Value { get; set; }
        public string? ValueText { get; set; }
        public string? District { get; set; }
        public int? TaxType { get; set; }
        public decimal? TaxesValue { get; set; }
        public int? BranchId { get; set; }
        public string? AttachmentUrl { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerName_W { get; set; }

        public string? CustomerMobile { get; set; }
        public string? ProjectName { get; set; }
        public string? ProjectNo { get; set; }
        public string? CustomerTypeName { get; set; }
        public string? ProjectTypeName { get; set; }
        public decimal? TotalRemainingPayment { get; set; }
        public decimal? TotalPaidPayment { get; set; }
        public string? PaymentHijriDate { get; set; }
        public decimal AdvancePayValue { get; set; }
        public decimal MonthlyPayValue { get; set; }
        public int PaymentsCount { get; set; }
        public decimal LastPayValue { get; set; }
        public int GregorianHijriPay { get; set; }
        public int PayType { get; set; }
        public decimal? TotalValue { get; set; }
        public string? TotalValuetxt { get; set; }

        public  int? OrgId { get; set; }
        public  int? OrgEmpId { get; set; }
        public  int? OrgEmpJobId { get; set; }
        public  int? ServiceId { get; set; }
        public bool IsChecked { get; set; }
        public bool IsSearch { get; set; }
        public string? dateFrom { get; set; }
        public string? dateTo { get; set; }
        public  int? UpdateUser { get; set; }
        public string? ServiceName { get; set; }
        public string? AttachmentUrlExtra { get; set; }


        public string? Engineering_License { get; set; }
        public string? Engineering_LicenseDate { get; set; }
        public string? Appr_LetterDate_Des { get; set; }
        public string? EngServ_OfferDate_Des { get; set; }
        public string? MaxPay_Des { get; set; }
        public string? ContractDurCommit_Des { get; set; }
        public string? ContPeriod_Des { get; set; }
        public string? TeamWork_Num1_Des { get; set; }
        public string? TeamWork_Note1_Des { get; set; }
        public string? TeamWork_Num2_Des { get; set; }
        public string? TeamWork_Note2_Des { get; set; }
        public string? TeamWork_Num3_Des { get; set; }
        public string? TeamWork_Note3_Des { get; set; }
        public string? TeamWork_Num4_Des { get; set; }
        public string? TeamWork_Note4_Des { get; set; }
        public string? TeamWork_Num5_Des { get; set; }
        public string? TeamWork_Note5_Des { get; set; }
        public string? TeamWork_Num6_Des { get; set; }
        public string? TeamWork_Note6_Des { get; set; }
        public string? TeamWork_Num7_Des { get; set; }
        public string? TeamWork_Note7_Des { get; set; }
        public string? TeamWork_Num8_Des { get; set; }
        public string? TeamWork_Note8_Des { get; set; }
        public string? TeamWork_Num9_Des { get; set; }
        public string? TeamWork_Note9_Des { get; set; }
        public string? TeamWork_Num10_Des { get; set; }
        public string? TeamWork_Note10_Des { get; set; }
        public string? Cons_TotalFees_Des { get; set; }
        public string? ContractorName_Des { get; set; }
        public string? ContDate_Des { get; set; }

        public string? ProjBriefDesc_Des { get; set; }
        public  int? TotalServiceCount { get; set; }
        public string? ProjectDescription { get; set; }
        public  int? ManagerId { get; set; }
        public decimal? Oper_expeValue { get; set; }
        public string? FirstProjectDate { get; set; }
        public string? FirstProjectExpireDate { get; set; }
        public  int? StopProjectType { get; set; }
        public int? ProjectTypeId { get; set; }


    }
}
